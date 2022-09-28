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
    [string]$customerRestore,
    [Parameter(Mandatory = $true)]
    [string]$blobDownloadLocation,
    [Parameter(Mandatory = $true)]
    [string]$userIdRestore,
    [Parameter(Mandatory = $true)]
    [string]$userPasswordRestore,
    [Parameter(Mandatory = $true)]
    [String]$packageExecutable,
    [Parameter(Mandatory = $false)]
    [String]$offshoreChanges,
    [Parameter(Mandatory = $false)]
    [String]$dbusers
)

Import-Module -Name "$ModulePath\GetValue"
Import-Module -Name "$ModulePath\EditData"
Import-Module -Name "$ModulePath\RestoreDatabaseFromStorage"
Import-Module -Name "$ModulePath\RemoveDatabase"
Import-Module -Name "$ModulePath\RestoreDatabaseUsers"

function Confirm-BackUp($databaseBackupFileName, $storageName, $storageKey, $container) {
    $azureContext = New-AzStorageContext -StorageAccountName $storageName -StorageAccountKey $storageKey
    $blob = Get-AzStorageBlob -Blob $databaseBackupFileName -Container $container -Context $azureContext -ErrorAction Continue

    if ($blob) {
        return $true
    }
    else {
        return $false
    }
}

# Rename Database
function Rename-Database($connectionString, $existingName, $newName) {
    $connectionString = $connectionString.Replace("Initial Catalog=$DatabaseName" , "")
    Write-Output "Renaming database $existingName to $newName..."
    $query = "EXEC master..sp_renamedb '" + $existingName + "','" + $newName + "'";
    Invoke-Sqlcmd -ConnectionString $connectionString -Query $query;
}


function Restore-User($SqlServerConnectionString, $newuser, $role) {
    $dbuser = (Select-object @{n = 'username'; e = { $newuser } } -InputObject '')

    $dbrole = (Select-object @{n = 'role'; e = { $role } }, @{n = 'username'; e = { $newuser } } -InputObject '')

    Restore-DatabaseUsers -Users $dbuser -Roles $dbrole -ConnectionString $SqlServerConnectionString
}

# Variables

$ServerName = $ServerNameFullname.Split(".")[0]
$userId = $userIdRestore
$password = $userPasswordRestore
$extractedUserId = Get-Value -InputString $SqlServerConnectionString -Key "User ID"
$extractedPassword = Get-Value -InputString $SqlServerConnectionString -Key "Password" #pragma: allowlist secret
$storageAccountName = Get-Value -InputString $storageAccountConnectionString -Key "AccountName"
$storageAccountKey = Get-Value -InputString $storageAccountConnectionString -Key "AccountKey"

$adminSqlServerConnectionString = Edit-Data -InputString $SqlServerConnectionString -Source $extractedUserId -Destination $userId

$adminSqlServerConnectionString = Edit-Data -InputString $adminSqlServerConnectionString -Source $extractedPassword -Destination $password

if ($customerRestore -eq "true") {

    $newDatabase = $DatabaseName + "_new";
    $databaseBackupName = $DatabaseName + "_customer"
    # Create testbed database from blob
    Restore-DatabaseFromStorage -ResourceGroupName $ResourceGroupName -ServerName $ServerNameFullname -ConnectionString $adminSqlServerConnectionString -DownloadBasePath "customer" -DatabaseName $newDatabase -BlobDownloadLocation $blobDownloadLocation -PackageExecutablePath $packageExecutable -DatabaseBackupName $databaseBackupName -InstanceName $ServerName -Username $userId -Password $password -StorageAccountName $storageAccountName -StorageAccountKey $storageAccountKey -ContainerName $BlobContainerName

    Remove-Database -DatabaseName $DatabaseName -ConnectionString $adminSqlServerConnectionString;

    # Rename new database
    Rename-Database -connectionString $adminSqlServerConnectionString -existingName $newDatabase -NewName $DatabaseName
    if($dbusers.Length -gt 0) {
        $dbusers = $dbusers.Split(';')

        foreach ($dbuser in $dbusers) {
            $userdata = $dbuser.split('-')
            Restore-User -SqlServerConnectionString $adminSqlServerConnectionString -newuser $userdata[0] -role $userdata[1]
        }
    }
}
else {

    if ($offshoreChanges -eq "true") {
        $newDatabase = $DatabaseName + "_offshore";
        $offshoreDacpacFileName = "offshore/$newDatabase.bacpac"
        $backUp = Check-BackUp -databaseBackupFileName $offshoreDacpacFileName -storageName $storageAccountName -storageKey $storageAccountKey -container $BlobContainerName

        if ($backUp) {
            Restore-DatabaseFromStorage -ResourceGroupName $ResourceGroupName -ServerName $ServerNameFullname -DownloadBasePath "offshore" -DatabaseName $newDatabase -BlobDownloadLocation $blobDownloadLocation -PackageExecutablePath $packageExecutable -DatabaseBackupName $newDatabase -InstanceName $ServerName -Username $userId -Password $password -StorageAccountName $storageAccountName -StorageAccountKey $storageAccountKey -ContainerName $BlobContainerName -ConnectionString $adminSqlServerConnectionString
            Remove-Database -DatabaseName $DatabaseName -ConnectionString $adminSqlServerConnectionString
            Rename-Database -connectionString $adminSqlServerConnectionString -existingName $newDatabase -NewName $DatabaseName
        }
        else {
            Write-Output "No Database BackUp present under offshore folder."
        }
    }
    else {
        Write-Output "offshore DB deployment Skipped."
    }
}

##Set Output Variable.
$key = "storageNameLA"
if ($offshoreChanges -eq $true) {
    $value = $storageAccountName + "off"
}
else {
    $value = $storageAccountName
}
Write-Output "##vso[task.setvariable variable=$key;]$value"