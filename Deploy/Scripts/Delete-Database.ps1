#Parameters
param(
    [Parameter(Mandatory = $true)]
    [string]$ModulePath,
    [Parameter(Mandatory=$true)]
	[string]$SqlServerConnectionString,
    [Parameter(Mandatory=$true)]
	[string]$userId,
    [Parameter(Mandatory=$true)]
	[string]$userPassword,
    [Parameter(Mandatory=$true)]
	[string]$DatabaseName,
    [Parameter(Mandatory=$true)]
	[string]$cloneDatabaseName
)

Import-Module -Name "$ModulePath\GetValue"
Import-Module -Name "$ModulePath\EditData"
Import-Module -Name "$ModulePath\RemoveDatabase"

$extractedUserName = Get-Value -InputString $SqlServerConnectionString -Key "User ID"
$extractedPassword = Get-Value -InputString $SqlServerConnectionString -Key "Password"
$adminSqlServerConnectionString = Edit-Data -InputString $SqlServerConnectionString -Source $extractedUserName -Destination $userId;
$adminSqlServerConnectionString = Edit-Data -InputString $SqlServerConnectionString -Source $extractedPassword -Destination $userPassword;
$adminSqlServerConnectionString = Edit-Data -InputString $SqlServerConnectionString -Source $DatabaseName -Destination $backupDatabaseName;
Remove-Database -DatabaseName $cloneDatabaseName -ConnectionString $adminSqlServerConnectionString