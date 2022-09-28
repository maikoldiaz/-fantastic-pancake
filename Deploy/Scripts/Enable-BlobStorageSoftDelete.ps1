param(
 [Parameter(Mandatory=$True)]
 [string]
 $storageAccountName,
 [Parameter(Mandatory=$True)]
 [int]
 $retentionDays
)

$name = $storageAccountName
$existingAccount = Get-AzStorageAccount | where-object{$_.StorageAccountName -match $name}
if ($existingAccount)
{
    $existingAccount | Enable-AzStorageDeleteRetentionPolicy -RetentionDays $retentionDays
    Write-Output "Enabled soft delete on blob storage successfully."
} else {
    Write-Output "The storage account does not exist."
}