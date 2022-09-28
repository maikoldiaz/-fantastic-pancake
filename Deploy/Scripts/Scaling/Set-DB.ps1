param
(
    [Parameter(Mandatory = $true)]
    [string]$ModulePath,
    [Parameter(Mandatory = $true)]
    [String]$ResourceGroupName,
    [Parameter(Mandatory = $true)]
    [String]$ServerNameFullname,
    [Parameter(Mandatory = $true)]
    [String]$DatabaseName,
    [Parameter(Mandatory = $true)]
    [String]$StorageAccountConnectionString,
    [Parameter(Mandatory = $true)]
    [String]$SqlServerConnectionString,
    [Parameter(Mandatory = $true)]
    [string]$BlobContainerName,
    [Parameter(Mandatory = $true)]
    [string]$blobDownloadLocation,
    [Parameter(Mandatory = $true)]
    [string]$userIdRestore,
    [Parameter(Mandatory = $true)]
    [string]$userPasswordRestore,
    [Parameter(Mandatory = $true)]
    [String]$packageExecutable
)


Import-Module -Name "$ModulePath\GetValue"
Import-Module -Name "$ModulePath\EditData"
Import-Module -Name "$ModulePath\RestoreDatabaseFromStorage"
Import-Module -Name "$ModulePath\RemoveDatabase"

# Rename Database
function Rename-Database($connectionString, $existingName, $newName) {
    $connectionString = $connectionString.Replace("Initial Catalog=$DatabaseName" , "")
    Write-Output "Renaming database $existingName to $newName..."
    $query = "EXEC master..sp_renamedb '" + $existingName + "','" + $newName + "'";
    Invoke-Sqlcmd -ConnectionString $connectionString -Query $query;
}

$ServerName = $ServerNameFullname.Split(".")[0]
$userId = $userIdRestore
$password = $userPasswordRestore
$extractedUserId = Get-Value -InputString $SqlServerConnectionString -Key "User ID"
$extractedPassword = Get-Value -InputString $SqlServerConnectionString -Key "Password" #pragma: allowlist secret
$storageAccountName = Get-Value -InputString $storageAccountConnectionString -Key "AccountName"
$storageAccountKey = Get-Value -InputString $storageAccountConnectionString -Key "AccountKey"

$adminSqlServerConnectionString = Edit-Data -InputString $SqlServerConnectionString -Source $extractedUserId -Destination $userId

$adminSqlServerConnectionString = Edit-Data -InputString $adminSqlServerConnectionString -Source $extractedPassword -Destination $password

$newDatabase = $DatabaseName + "_new";
$databaseBackupName = $DatabaseName + "_new"


Restore-DatabaseFromStorage -ResourceGroupName $ResourceGroupName -ServerName $ServerNameFullname -ConnectionString $adminSqlServerConnectionString -DatabaseName $newDatabase -BlobDownloadLocation $blobDownloadLocation -PackageExecutablePath $packageExecutable -DatabaseBackupName $databaseBackupName -InstanceName $ServerName -Username $userId -Password $password -StorageAccountName $storageAccountName -StorageAccountKey $storageAccountKey -ContainerName $BlobContainerName


Remove-Database -DatabaseName $DatabaseName -ConnectionString $adminSqlServerConnectionString;

Rename-Database -connectionString $adminSqlServerConnectionString -existingName $newDatabase -NewName $DatabaseName