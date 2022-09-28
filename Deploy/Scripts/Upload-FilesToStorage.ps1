##Parameters
param
(
    [Parameter(Mandatory = $true)]
    [string]$ModulePath,
    [Parameter(Mandatory = $true)]
    [string]$FilePath,
    [Parameter(Mandatory = $true)]
    [string]$StorageConnectionString,
    [Parameter(Mandatory = $true)]
    [string]$ContainerName
)

Import-Module -Name "$ModulePath\GetValue"

function Export-File($storageContext, $containers, $path) {
    Write-Output 'Uploading files...'

    $localFilePath = Get-ChildItem -Path $path -File
    foreach ($file in $localFilePath) {
        $fileName = $file.Name
        $fullFilePath = $path + $fileName

        $container = ""
        if ($fileName -eq "MOV_OP_CONSOLIDADOS_TRUE.csv") {
            $container = $containers[0]
        }
        elseif ($fileName -eq "MOV_TT_CONSOLIDADOS_TRUE.csv") {
            $container = $containers[1]
        }
        elseif ($fileName -eq "RELACION_NODOS_OP_TRUE.csv" -OR $fileName -eq "RELACION_NODOS_TT_TRUE.csv") {
            $container = $containers[2]
        }
        elseif ($fileName -eq "OwnershipPercentageValues.csv") {
            $container = $containers[3]
        }
        else {
            $container = $ContainerName
        }

        Set-AzureStorageBlobContent -File $fullFilePath -Container $container -Blob $fileName -Context $storageContext -Force
    }
}


try {
    $storageAccountName = Get-Value -InputString $StorageConnectionString -Key "AccountName"
    $storageAccountKey = Get-Value -InputString $StorageConnectionString -Key "AccountKey"
    $storageContext = New-AzureStorageContext -StorageAccountName $storageAccountName -StorageAccountKey $storageAccountKey

    $containerNames = $ContainerName.Split(',')
    Export-File -storageContext $storageContext -containers $containerNames -path $FilePath

    Write-Output "Upload complete."
}
catch {
    Write-Error -Message "Error Message: " -Exception $_.Exception
    throw $_.Exception
}