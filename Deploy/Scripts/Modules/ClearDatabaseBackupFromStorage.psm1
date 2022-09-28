<#
 .Synopsis
  backs up the database to the storage account

 .Parameter BlobName
  The blob name

 .Parameter ContainerName
  The name of the blob container

 .Parameter StorageContext
  The storage context generated from storage account credentials

 .Example
   Clear-DatabaseBackupFromStorage -StorageContext $storageContext -BlobName $blobName -ContainerName $containerName
#>

function Clear-DatabaseBackupFromStorage($StorageContext, $BlobName, $ContainerName) {
    $blobFileName = $BlobName + ".bacpac";
    $blob = Get-AzStorageBlob -Blob $blobFileName -Container $ContainerName -Context $StorageContext -ErrorAction Ignore
    if (-not $blob)
    {
        Write-Output "No previous backup found to delete."
    } else
    {
        Write-Output ("Removing old backup: " + $BlobName)
        Remove-AzStorageBlob -Blob $blobFileName -Container $ContainerName -Context $StorageContext
    }
}

Export-ModuleMember -Function Clear-DatabaseBackupFromStorage