param(
    [Parameter(Mandatory = $true)]
    [string]$storageName,

    [Parameter(Mandatory = $true)]
    [string]$storageTableName,

    [Parameter(Mandatory = $true)]
    [string]$resourceGroupName
)
Install-Module AzTable -Force

$storageAccount = Get-AzStorageAccount -ResourceGroupName $resourceGroupName -Name $storageName -ErrorAction SilentlyContinue

Remove-AzStorageTable -Name $storageTableName -Context $storageAccount.Context -Force -ErrorAction SilentlyContinue

Write-Output "Storage Table removed : $storageTableName"
