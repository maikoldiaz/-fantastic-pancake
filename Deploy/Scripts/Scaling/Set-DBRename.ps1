param
(
    [Parameter(Mandatory = $true)]
    [string]$ModulePath,
    [Parameter(Mandatory = $true)]
    [String]$ServerNameFullname,
    [Parameter(Mandatory = $true)]
    [String]$DatabaseName,
    [Parameter(Mandatory = $true)]
    [String]$SqlServerConnectionString,
    [Parameter(Mandatory = $true)]
    [string]$userIdRestore,
    [Parameter(Mandatory = $true)]
    [string]$userPasswordRestore,
    [Parameter(Mandatory = $true)]
    [string]$oldDB,
    [Parameter(Mandatory = $true)]
    [string]$renameDB
)

Import-Module -Name "$ModulePath\GetValue"
Import-Module -Name "$ModulePath\EditData"

# Rename Database
function Rename-Database($connectionString, $existingName, $newName, $DatabaseName) {
    $connectionStringNoDB = $connectionString.Replace("Initial Catalog=$DatabaseName" , "")
    Write-Output "Renaming database $existingName to $newName..."
    $query = "EXEC master..sp_renamedb '" + $existingName + "','" + $newName + "'";

    Invoke-Sqlcmd -ConnectionString $connectionStringNoDB -Query $query;

    $connectionString = $connectionString.Replace("Initial Catalog=$DatabaseName" , "Initial Catalog=$newName")
    $query = "IF USER_ID('truedbuser') IS NULL CREATE USER truedbuser FOR LOGIN truedbuser;"
    Invoke-Sqlcmd -ConnectionString $connectionString -Query $query;

    $query="EXEC sp_addrolemember 'db_owner', 'truedbuser'"
    Invoke-Sqlcmd -ConnectionString $connectionString -Query $query;
}


$ServerName = $ServerNameFullname.Split(".")[0]
$userId = $userIdRestore
$password = $userPasswordRestore
$extractedUserId = Get-Value -InputString $SqlServerConnectionString -Key "User ID"
$extractedPassword = Get-Value -InputString $SqlServerConnectionString -Key "Password" #pragma: allowlist secret

$adminSqlServerConnectionString = Edit-Data -InputString $SqlServerConnectionString -Source $extractedUserId -Destination $userId

$adminSqlServerConnectionString = Edit-Data -InputString $adminSqlServerConnectionString -Source $extractedPassword -Destination $password

Rename-Database -connectionString $adminSqlServerConnectionString -existingName $oldDB -NewName $renameDB -DatabaseName $DatabaseName
