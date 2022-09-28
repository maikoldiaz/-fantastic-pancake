param
(
    [string]$ModulePath,

    [String]$ResourceGroupName,

    [String]$ServerNameFullname,

    [String]$DatabaseName,

    [String]$StorageAccountConnectionString,

    [String]$SqlServerConnectionString,

    [string]$BlobContainerName,

    [string]$userIdRestore,

    [string]$userPasswordRestore,

    [String]$SqlPackageExePath
)

Import-Module -Name "$ModulePath\GetValue"
Import-Module -Name "$ModulePath\EditData"
Import-Module -Name "$ModulePath\RemoveDatabase"

# Recreates database from backup in blob
function Restore-LatestDatabaseFromStorage($packageExe, $resourceGroup, $connectionString, $blobContainerName, $dbServerName, $dbName, $dbUserName, $dbPassword, $storageName, $storageKey) {
    $databases = Get-AzSqlInstanceDatabase -InstanceName $ServerName -ResourceGroupName $resourceGroup | Where-Object { $_.Name.ToString().Equals($dbName) }

    if ($databases) {
        Write-Output "Database already exists"
        Remove-Database -DatabaseName $dbName -ConnectionString $connectionString
    }

    Write-Output "Initiating database import from storage account..."

    $context = New-AzStorageContext -StorageAccountName $storageName -StorageAccountKey $storageKey
    $blobs = Get-AzureStorageBlob -Container $blobContainerName -Context $context | Sort-Object @{expression = "LastModified"; Descending = $true }
    $latestBlob = $blobs[0]

    Write-Output "Importing blob - $latestBlob ..."
    Get-AzStorageBlobContent -Container $blobContainerName -Blob $latestBlob.Name -Context $context -Force

    $connectionString = "Data Source=tcp:$ServerNameFullname;Initial Catalog=$dbName;Persist Security Info=False;User ID=$dbUserName;Password=$dbPassword;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

    #$packageExe = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\150\sqlpackage.exe"

    & $packageExe /a:import /tcs:$connectionString /sf:$latestBlob.Name /p:DatabaseEdition=Standard /p:DatabaseServiceObjective=S2 /p:Storage=File
}

try {
    # Variables
    $ServerName = $ServerNameFullname.Split(".")[0]
    $userId = $userIdRestore
    $password = $userPasswordRestore
    $extractedUserId = Get-Value -InputString $SqlServerConnectionString -Key "User ID"
    $extractedPassword = Get-Value -InputString $SqlServerConnectionString -Key "Password" #pragma: allowlist secret
    $storageAccountName = Get-Value -InputString $storageAccountConnectionString -Key "AccountName" #pragma: allowlist secret
    $storageAccountKey = Get-Value -InputString $storageAccountConnectionString -Key "AccountKey"

    $adminSqlServerConnectionString = Edit-Data -InputString $SqlServerConnectionString -Source $extractedUserId -Destination $userId;
    $adminSqlServerConnectionString = Edit-Data -InputString $adminSqlServerConnectionString -Source $extractedPassword -Destination $password;

    Remove-Database -DatabaseName $DatabaseName -ConnectionString $adminSqlServerConnectionString;

    # Create testbed database from latest blob
    Restore-LatestDatabaseFromStorage -connectionString $adminSqlServerConnectionString -resourceGroup $ResourceGroupName -blobContainerName $BlobContainerName -dbName $DatabaseName -dbServerName $ServerName -dbUserName $userId -dbPassword $password -storageName $storageAccountName -storageKey $storageAccountKey -packageExe $SqlPackageExePath
}
catch {
    Write-Error -Message "Error Message: " -Exception $_.Exception
    throw $_.Exception
}