param
(
    [Parameter(Mandatory = $true)]
    [string]$ModulePath,

    [Parameter(Mandatory = $false)]
    [String]$SqlServerConnectionString,

    [Parameter(Mandatory = $false)]
    [String]$DatabaseName,

    [Parameter(Mandatory = $false)]
    [String]$StorageAccountConnectionString,

    [Parameter(Mandatory = $false)]
    [String]$BlobContainerName,

    [Parameter(Mandatory = $false)]
    [String]$SqlPackageExePath,

    [Parameter(Mandatory = $false)]
    [String]$offshoreChanges,

    [Parameter(Mandatory = $false)]
    [String]$customerDbBackUp
)

Import-Module -Name "$ModulePath\GetValue"
Import-Module -Name "$ModulePath\ClearDatabaseUsers"
Import-Module -Name "$ModulePath\GetDatabaseUserRole"

function Backup-DatabaseToStorage($SqlServerConnectionString, $databaseBackupFileName, $storageContext, $container, $packageExecutable, $pathToBackUp) {

    $users = Get-DatabaseUserRole -ConnectionString $SqlServerConnectionString

    if (($users.ItemArray.Count -gt 2) -and $users.ItemArray.Contains("truedbuser")) {
        Clear-DatabaseUsers -Users $users -ConnectionString $SqlServerConnectionString
    }

    Write-Output "Initiating database export to storage account as file named $databaseBackupFileName..."
    $datetime = Get-Date -Format "MMddyyyyHHmm"
    $bacpacFilename = $databaseBackupFileName + ".bacpac"
    $output = & $packageExecutable /a:Export /tf:$bacpacFilename /scs:$SqlServerConnectionString
    if ($LastExitCode -ne 0) {
        Write-Output "ERROR: "
        Write-Output $output
        throw "Failed to export bacpac for backup"
    }
    else {
        Write-Output $output
    }

    $bacpacFilenameTimeStamp = $databaseBackupFileName + $datetime + ".bacpac"
    $output = & $packageExecutable /a:Export /tf:$bacpacFilenameTimeStamp /scs:$SqlServerConnectionString
    if ($LastExitCode -ne 0) {
        Write-Output "ERROR: "
        Write-Output $output
        throw "Failed to export bacpac for backup"
    }
    else {
        Write-Output $output
    }

    if ($pathToBackUp) {

        $existing = Get-AzStorageBlob  -Blob "$pathToBackUp/$bacpacFilename" -Container $container -Context $storageContext -ErrorAction Continue

        if ($existing) {
            $destBlob = "$pathToBackUp/$databaseBackupFileName" + "_PreviousBuild.bacpac"
            Start-AzStorageBlobCopy -SrcContainer $container -SrcBlob "$pathToBackUp/$bacpacFilename" -DestContainer $container -DestBlob "$destBlob" -Context $storageContext -Force
        }

        Set-AzStorageBlobContent -Blob "$pathToBackUp/$bacpacFilename" -Container $container -Context $storageContext -File $bacpacFilename -Force
        Set-AzStorageBlobContent -Blob "$pathToBackUp/$bacpacFilenameTimeStamp" -Container $container -Context $storageContext -File $bacpacFilenameTimeStamp -Force

    }
    else {
        Set-AzStorageBlobContent -Blob $bacpacFilename -Container $container -Context $storageContext -File $bacpacFilename -Force
    }
    Write-Output "Export Completed"
}

$storageAccountName = Get-Value -InputString $StorageAccountConnectionString -Key "AccountName"
$storageAccountKey = Get-Value -InputString $storageAccountConnectionString -Key "AccountKey"
$storageContext = New-AzStorageContext -StorageAccountName $storageAccountName -StorageAccountKey $storageAccountKey

if ($offshoreChanges -eq "true") {
    $databaseBackupName = $DatabaseName + "_offshore";
    Backup-DatabaseToStorage -databaseBackupFileName $databaseBackupName -storageContext $storageContext -container $BlobContainerName -packageExecutable $SqlPackageExePath -pathToBackUp "offshore" -SqlServerConnectionString $SqlServerConnectionString
}
else {

    if ($customerDbBackUp -eq "true") {
        $ErrorActionPreference = 'Stop'
        $databaseBackupName = $DatabaseName + "_customer"
        # Clearing database backup in blob container if already exists
        #Clear-OldDatabaseBackupFromStorage -storageContext $storageContext -blobName $databaseBackupName -container $BlobContainerName
        # Taking database backup and uploading it to storage account blob container
        Backup-DatabaseToStorage -databaseBackupFileName $databaseBackupName -storageContext $storageContext -container $BlobContainerName -packageExecutable $SqlPackageExePath -pathToBackUp "customer" -SqlServerConnectionString $SqlServerConnectionString
    }
    else {
        Write-Output "BackUp Skipped for Offshore DB"
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