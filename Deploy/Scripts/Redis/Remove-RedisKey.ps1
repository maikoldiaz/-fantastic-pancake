param(
 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory=$true)]
 [string]
 $storageAccountName,

 [Parameter(Mandatory=$true)]
 [string]
 $containerName,

 [Parameter(Mandatory=$true)]
 [string]
 $blobName
 )

$keys = Get-AzStorageAccountKey -ResourceGroupName $resourceGroupName -Name $storageAccountName -ErrorAction SilentlyContinue

if($keys)
{
	$storageContext = New-AzStorageContext -StorageAccountName $storageAccountName -StorageAccountKey $keys[0].Value
	Remove-AzStorageBlob -Container $containerName -Blob $blobName -Context $storageContext -ErrorAction SilentlyContinue
}