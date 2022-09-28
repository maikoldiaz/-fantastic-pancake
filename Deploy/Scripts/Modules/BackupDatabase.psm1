<#
 .Synopsis
  backs up the database to the storage account

 .Parameter DatabaseBackupName
  The database backup name

 .Parameter ConnectionString
  The sql connection string

 .Parameter ContainerName
  The name of the blob container

 .Parameter StorageContext
  The storage context generated from storage account credentials

 .Parameter UploadToStorage
  The flag if set "True" uploads to storage account

 .Example
   Backup-Database -DatabaseBackupName $databaseBackupName -StorageContext $storageContext -ContainerName $containerName -ConnectionString $connectionString
#>

function Backup-Database($DatabaseBackupName, $StorageContext, $ContainerName, $ConnectionString, $PackageExecutablePath, $UploadToStorage) {
    Write-Output "Initiating database export as file named $DatabaseBackupName..."
    $bacpacFilename = $DatabaseBackupName + ".bacpac"
    $output = & $PackageExecutablePath /a:Export /tf:$bacpacFilename /scs:$ConnectionString
    if ($LastExitCode -ne 0)
    {
        Write-Output "ERROR: "
        Write-Output $output
        throw "Failed to create bacpac for backup"
    } else {
        Write-Output $output
    }
    if($UploadToStorage -eq "true") {
        Set-AzStorageBlobContent -Blob $bacpacFilename -Container $ContainerName -Context $StorageContext -File $bacpacFilename -Force
        Write-Output "Upload Completed"
    }
}

Export-ModuleMember -Function Backup-Database