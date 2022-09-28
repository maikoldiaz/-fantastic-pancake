param
(
    [Parameter(Mandatory = $true)]
    [String]$zipFilePath,

    [Parameter(Mandatory = $true)]
    [String]$env,

    [Parameter(Mandatory = $true)]
    [String]$dnsNameForApi,

    [Parameter(Mandatory = $true)]
    [String]$nodeApprovalsFlowUniqueGuid,

    [Parameter(Mandatory = $true)]
    [String]$nodeDeltaApprovalsFlowUniqueGuid,

    [Parameter(Mandatory = $false)]
    [String]$apiScope,

    [Parameter(Mandatory = $false)]
    [String]$tenantId,

    [Parameter(Mandatory = $false)]
    [String]$appClientId,

    [Parameter(Mandatory = $false)]
    [String]$appClientSecret
)

$apiBaseUrl = "https://$dnsNameForApi"

function Format-ApprovalFlowParam() {

    $tempPath = "$zipFileEnvPath\Aprobacin-Propiedad-Nodo-Diaria.json"

    $jsonData = Get-Content $tempPath -Encoding UTF8 | ConvertFrom-Json
    $jsonData.properties.definition.actions.TRY.actions.Compose_JSON_Input.inputs.clientId = $appClientId
    $jsonData.properties.definition.actions.TRY.actions.Compose_JSON_Input.inputs.clientSecret = $appClientSecret
    $jsonData.properties.definition.actions.TRY.actions.Compose_JSON_Input.inputs.apiUrl = $apiBaseUrl
    $jsonData.properties.definition.actions.TRY.actions.Compose_JSON_Input.inputs.tenantId = $tenantId
    $jsonData.properties.definition.actions.TRY.actions.Compose_JSON_Input.inputs.scope = "api://$apiScope/.default"

    Remove-Item -Path $tempPath

    ConvertTo-Json -InputObject $jsonData -Depth 100 | ForEach-Object { [System.Text.RegularExpressions.Regex]::Unescape($_) } | Out-File $tempPath -Encoding utf8 -Force

    Format-UTFByteOrderMark -filePath $tempPath

    Start-Sleep -Seconds 3
}

function Format-ApprovalDeltaFlowParam() {

    $tempPath = "$zipFileEnvPath\Aprobacin-Balance-Oficial-Nodo-Mensual.json"

    $jsonData = Get-Content $tempPath -Encoding UTF8 | ConvertFrom-Json
    $jsonData.properties.definition.actions.TRY.actions.Compose_JSON_Input.inputs.clientId = $appClientId
    $jsonData.properties.definition.actions.TRY.actions.Compose_JSON_Input.inputs.clientSecret = $appClientSecret
    $jsonData.properties.definition.actions.TRY.actions.Compose_JSON_Input.inputs.apiUrl = $apiBaseUrl
    $jsonData.properties.definition.actions.TRY.actions.Compose_JSON_Input.inputs.tenantId = $tenantId
    $jsonData.properties.definition.actions.TRY.actions.Compose_JSON_Input.inputs.scope = "api://$apiScope/.default"

    Remove-Item -Path $tempPath

    ConvertTo-Json -InputObject $jsonData -Depth 100 | ForEach-Object { [System.Text.RegularExpressions.Regex]::Unescape($_) } | Out-File $tempPath -Encoding utf8 -Force

    Format-UTFByteOrderMark -filePath $tempPath

    Start-Sleep -Seconds 3
}

function Format-UTFByteOrderMark($filePath) {
    [System.IO.FileInfo] $file = Get-Item -Path $filePath
    $sequenceBOM = New-Object System.Byte[] 3
    $reader = $file.OpenRead()
    $bytesRead = $reader.Read($sequenceBOM, 0, 3)
    $reader.Dispose()
    #A UTF-8+BOM string will start with the three following bytes. Hex: 0xEF0xBB0xBF, Decimal: 239 187 191
    if ($bytesRead -eq 3 -and $sequenceBOM[0] -eq 239 -and $sequenceBOM[1] -eq 187 -and $sequenceBOM[2] -eq 191) {
        $utf8NoBomEncoding = New-Object System.Text.UTF8Encoding($False)
        [System.IO.File]::WriteAllLines($filePath, (Get-Content $filePath), $utf8NoBomEncoding)
        Write-Output "Remove UTF-8 BOM successfully"
    }
    else {
        Write-Warning "Not UTF-8 BOM file"
    }
}

function Format-SolutionXml($zipFileEnvPath, $env, $nodeApprovalsFlowUniqueGuid, $nodeDeltaApprovalsFlowUniqueGuid) {
    $defaultPlaceholderGuid = "949664e3-7a8f-ea11-a811-000d3a569fe1"
    $defaultDeltaApprovalPlaceholderGuid = "cd21b265-90d1-ea11-a812-000d3a33fa14"

    $tempPath = "$zipFileEnvPath\$env\customizations.xml"
    $xmlData = Get-Content $tempPath -Encoding UTF8

    #Approvals Specific Implementation
    $environment = $env.ToUpper()
    $flowDisplayName = '"Aprobación-Propiedad-Nodo-Diaria"'
    $envSpecificDisplayName = $flowDisplayName.Replace("Diaria", "Diaria-" + $environment[0])

    #Delta Approvals Specific Implementation
    $flowDeltaApprovalDisplayName = '"Aprobación-Balance-Oficial-Nodo-Mensual"'
    $envSpecificDeltaApprovalDisplayName = $flowDeltaApprovalDisplayName.Replace("Mensual", "Mensual-" + $environment[0])

    Remove-Item -Path $tempPath

    $xmlData.Replace($flowDisplayName, $envSpecificDisplayName).Replace($defaultPlaceholderGuid, $nodeApprovalsFlowUniqueGuid).TrimEnd().Replace($flowDeltaApprovalDisplayName, $envSpecificDeltaApprovalDisplayName).Replace($defaultDeltaApprovalPlaceholderGuid, $nodeDeltaApprovalsFlowUniqueGuid).TrimEnd() |  Out-File $tempPath -Encoding default


    $tempPath = "$zipFileEnvPath\$env\solution.xml"
    $xmlData = Get-Content $tempPath -Encoding UTF8

    $xmlData.Replace($defaultPlaceholderGuid, $nodeApprovalsFlowUniqueGuid).Replace($defaultDeltaApprovalPlaceholderGuid, $nodeDeltaApprovalsFlowUniqueGuid) |  Out-File $tempPath -Encoding default
}

$zipFileEnvPath = $zipFilePath + $env

$folderContent = Get-ChildItem -Path $zipFileEnvPath"\"

if (!$folderContent) {
    throw "FolderEmpty"
}

Format-SolutionXml -zipFileEnvPath $zipFilePath -env $env -nodeApprovalsFlowUniqueGuid $nodeApprovalsFlowUniqueGuid -nodeDeltaApprovalsFlowUniqueGuid $nodeDeltaApprovalsFlowUniqueGuid
Format-ApprovalFlowParam
Format-ApprovalDeltaFlowParam