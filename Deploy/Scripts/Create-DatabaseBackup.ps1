param
(
    [Parameter(Mandatory = $true)]
    [string]$ModulePath,

    [Parameter(Mandatory = $false)]
    [String]$SqlServerConnectionString,

    [Parameter(Mandatory = $true)]
    [String]$ServerName,

    [Parameter(Mandatory = $false)]
    [String]$DatabaseName,

    [Parameter(Mandatory = $false)]
    [String]$StorageAccountConnectionString,

    [Parameter(Mandatory = $false)]
    [String]$BlobContainerName,

    [Parameter(Mandatory = $false)]
    [String]$SqlPackageExePath,

    [Parameter(Mandatory = $false)]
    [String]$Perf,

    [Parameter(Mandatory = $false)]
    [String]$Clone,

    [Parameter(Mandatory = $false)]
    [String]$userIdRestore,

    [Parameter(Mandatory = $false)]
    [String]$userPasswordRestore,

    [Parameter(Mandatory = $false)]
    [string]$ResourceGroupName,

    [Parameter(Mandatory = $false)]
    [string]$Certification,

    [Parameter(Mandatory = $false)]
    [string]$MsiSqlConnectionString
)

Import-Module -Name "$ModulePath\GetValue"
Import-Module -Name "$ModulePath\GetDatabaseUsers"
Import-Module -Name "$ModulePath\GetDatabaseUserRole"
Import-Module -Name "$ModulePath\ClearDatabaseUsers"
Import-Module -Name "$ModulePath\BackupDatabase"
Import-Module -Name "$ModulePath\RestoreDatabaseUsers"
Import-Module -Name "$ModulePath\RemoveDatabase"
Import-Module -Name "$ModulePath\EditData"

try {
    $ErrorActionPreference = 'Stop'

    # Extract storage account name and key from the storage connection string
    $storageAccountName = Get-Value -InputString $StorageAccountConnectionString -Key "AccountName"
    $storageAccountKey = Get-Value -InputString $StorageAccountConnectionString -Key "AccountKey"
    # Generate storage context from name and key
    $storageContext = New-AzStorageContext -StorageAccountName $storageAccountName -StorageAccountKey $storageAccountKey

    # Gets the current database users (except inbuilt users or the current user)
    $users = Get-DatabaseUsers -ConnectionString $SqlServerConnectionString;
    # Gets the current users' roles
    $roles = Get-DatabaseUserRole -ConnectionString $SqlServerConnectionString;
    # Deletes all the users
    Clear-DatabaseUsers -ConnectionString $SqlServerConnectionString -Users $users;

    # Current timestamp used for bacpac (database backup) file name
    $currentTimeStamp = Get-Date -format "yyyy_MM_dd_HH_mm";

    # the name with which bacpac file will be created locally.
    $databasebackupFileName = "";

    # clone db name
    $cloneDbName = ""
    if($Certification -eq "true") {
        $cloneDbName = $DatabaseName + "_testbed";
    } else {
        $cloneDbName = $DatabaseName + "_old";
    }

    # generate serveradmin connection string
    $extractedUserId = Get-Value -InputString $SqlServerConnectionString -Key "User ID"
    $extractedPassword = Get-Value -InputString $SqlServerConnectionString -Key "Password" #pragma: allowlist secret

    # create custom connection string only clone or certification is true.
    $customConnectionString = "";
    if($Clone -eq "true" -or $Certification -eq "true") {
        $customConnectionString = Edit-Data -InputString $SqlServerConnectionString -Source $extractedUserId -Destination $userIdRestore;
        $customConnectionString = Edit-Data -InputString $customConnectionString -Source $extractedPassword -Destination $userPasswordRestore;
        $customConnectionString = Edit-Data -InputString $customConnectionString -Source $DatabaseName -Destination $cloneDbName;   
    }
    
    if($Perf -eq "true")
    {
        $databasebackupFileName = $DatabaseName + "_" + "new";
        Backup-Database -DatabaseBackupName $databasebackupFileName -StorageContext $storageContext -ContainerName $BlobContainerName -PackageExecutablePath $SqlPackageExePath -ConnectionString $SqlServerConnectionString -UploadToStorage "true"
        $databasebackupFileName = $DatabaseName +"_"+$ServerName + "_" + $currentTimeStamp;
        Backup-Database -DatabaseBackupName $databasebackupFileName -StorageContext $storageContext -ContainerName $BlobContainerName -PackageExecutablePath $SqlPackageExePath -ConnectionString $SqlServerConnectionString -UploadToStorage "true"
        Restore-DatabaseUsers -ConnectionString $SqlServerConnectionString -Users $users -Roles $roles;
    }
    elseif($Certification -eq "true")
    {
        $databasebackupFileName = $DatabaseName + "_" + "new";
        Backup-Database -DatabaseBackupName $databasebackupFileName -StorageContext $storageContext -ContainerName $BlobContainerName -PackageExecutablePath $SqlPackageExePath -ConnectionString $SqlServerConnectionString
        $variableName = "testbedconnectionstring"
        Write-Output "##vso[task.setvariable variable=$variableName;]$customConnectionString"
        $variableName = "msitestbedconnectionstring"
        $msitestbedconnectionstring = Edit-Data -InputString $MsiSqlConnectionString -Source $DatabaseName -Destination $cloneDbName;
        Write-Output "##vso[task.setvariable variable=$variableName;]$msitestbedconnectionstring"
    }
    else
    {
        $databasebackupFileName = $DatabaseName +"_"+$ServerName + "_" + $currentTimeStamp;
        Backup-Database -DatabaseBackupName $databasebackupFileName -StorageContext $storageContext -ContainerName $BlobContainerName -PackageExecutablePath $SqlPackageExePath -ConnectionString $SqlServerConnectionString -UploadToStorage "true"
        Restore-DatabaseUsers -ConnectionString $SqlServerConnectionString -Users $users -Roles $roles;
    }

    # Create clone database from bacpac
    if($Clone -eq "true") {

        $instanceName = $ServerName.Split(".")[0]

        # remove if database already exists
        $databases = Get-AzSqlInstanceDatabase -InstanceName $instanceName -ResourceGroupName $ResourceGroupName ` | Where-Object { $_.Name.ToString().Equals($cloneDbName) }
        if ($databases) {
            Write-Output "Database already exists, so deleting it..."
            Remove-Database -DatabaseName $cloneDbName -ConnectionString $customConnectionString
        }

        # create database from bacpac file
        Write-Output "Cloning database, target name - $cloneDbName.."
        $bacpacLocation = $databasebackupFileName + ".bacpac";
        $output = & $SqlPackageExePath /a:import /tcs:$customConnectionString /sf:$bacpacLocation /p:DatabaseEdition=Standard /p:DatabaseServiceObjective=S2 /p:Storage=File
        if ($LastExitCode -ne 0)
        {
            Write-Output "ERROR: "
            Write-Output $output
            Remove-Database -DatabaseName $DatabaseName -ConnectionString $customConnectionString
            throw "Failed to import database"
        } else {
            Write-Output $output
            Write-Output "Cloned database"
        }
    }
} catch {
    Write-Error -Message "Error Message: " -Exception $_.Exception
	throw $_.Exception
}