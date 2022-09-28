param
(
    [Parameter(Mandatory = $true)]
    [string]
    $ModulePath,

    [Parameter(Mandatory = $true)]
    [String]$FolderCsvPath,

    [Parameter(Mandatory = $true)]
    [String]$SqlServerConnectionString,
	
    [Parameter(Mandatory = $true)]
    [String]$QueryNameScript,
	
    [Parameter(Mandatory = $true)]
    [String]$updateAction	
)

$BasePath = "$ModulePath/Develop/Ecp.True/DI/Ecp.True.DI.Sql/Scripts/PostDeployment"
$OneTimerScriptsPath = "$BasePath/OneTimerScripts"
$QueriesPath = "$BasePath/Operations"

function Install-Sql-Module() {
    if (Get-Module -ListAvailable -Name SqlServer) {
        Write-Host "##vso[task.logissue type=warning;]SQL Already Installed"
    } 
    else {
        Install-Module -Name SqlServer -Force
    }
}

function New-Path($path) {
    if (!(Test-Path -Path $path )) {
        New-Item -ItemType directory -Path $path
        Write-Host "👉 Directorio '$path' creado"
    }
    else {
        Write-Host "👉 El directorio '$path' ya existe"
    }
}

function Get-ScriptResults($QueryNameScript) {
    $DateTime = Get-Date -Format "_MM-dd-yyyy__HH-mm"
    $SanitizedScriptName = [System.Io.Path]::GetFileNameWithoutExtension($QueryNameScript)
    $FileCsvName = "$FolderCsvPath/$SanitizedScriptName$DateTime.csv"
    try {
        if ($updateAction -eq "True") {
            Write-Host "👉 Invocando comando sql de archivo de actualización: $SanitizedScriptName.sql"
            
            $data = Invoke-Sqlcmd -ConnectionString $SqlServerConnectionString -InputFile "$OneTimerScriptsPath/$SanitizedScriptName.sql"
        }
        else {
            Write-Host "👉 Invocando comando sql de archivo de consulta: $SanitizedScriptName.sql"

            $data = Invoke-Sqlcmd -ConnectionString $SqlServerConnectionString -InputFile "$QueriesPath/$SanitizedScriptName.sql"
        }
        Write-Host "👉 Inicializando archivo de exportación de datos con nombre: $FileCsvName"
        $data | Export-Csv -Path "$FileCsvName" -NoTypeInformation
    }
    catch {
        Write-Error "🛑 $_.Exception.Message"
        throw;
    }
}

Install-Sql-Module;

$scripts = $QueryNameScript.Split()
Write-Host "👉 Consultas de operación disponibles:"
Get-ChildItem $QueriesPath | Write-Host

Write-Host "👉 OneTimerScripts de existentes:"
Get-ChildItem $OneTimerScriptsPath | Write-Host
            
New-Path $FolderCsvPath

foreach ($script in $scripts) {
    Get-ScriptResults -QueryNameScript $script;
}