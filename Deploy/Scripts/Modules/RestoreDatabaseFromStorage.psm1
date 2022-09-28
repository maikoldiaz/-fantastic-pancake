<#
 .Synopsis
  Restores database using bacpac file stored in a blob container

 .Parameter ServerName
  The full server name that can be directly extracted from sql connection string

  .Parameter ConnectionString
  The sql connection string

  .Parameter DownloadBasePath
  The base path where the bacpac file is downloaded

  .Parameter DatabaseBackupName
  The base name of bacpac file

  .Parameter InstanceName
  The sql server name

  .Parameter DatabaseName
  The name of the database that can be extracted sql connection string

  .Parameter Username
  The login user name that can be extracted sql connection string

  .Parameter Password
  The login password that can be extracted sql connection string

  .Parameter StorageAccountName
  The storage account name that can be extract from storage connection string

  .Parameter StorageAccountKey
  The storage account key that can be extract from storage connection string

  .Parameter ContainerName
  The container name where backup is stored

  .Parameter BlobDownloadLocation
  The base path where the container content is downloaded

  .Parameter ResourceGroupName
  The name of the resource group

  .Parameter PackageExecutablePath
  The location of sqlpackage.exe for creating database from bacpac file downloaded from blob
#>

Import-Module -Name "$PSScriptRoot\RemoveDatabase"

function Restore-DatabaseFromStorage($ServerName, $ConnectionString, $DownloadBasePath, $DatabaseBackupName, $InstanceName, $DatabaseName, $Username, $Password, $StorageAccountName, $StorageAccountKey, $ContainerName, $BlobDownloadLocation, $PackageExecutablePath, $ResourceGroupName) {

    $databases = Get-AzSqlInstanceDatabase -InstanceName $InstanceName -ResourceGroupName $ResourceGroupName ` | Where-Object { $_.Name.ToString().Equals($DatabaseName) }

    if ($databases) {
        Write-Output "Database already exists"
        Remove-Database -DatabaseName $DatabaseName -ConnectionString $ConnectionString
    }

    Write-Output "Initiating database import from storage account..."
    $bacpacFilename = "";
    if($DownloadBasePath) {
        New-Item -Path "$blobDownloadLocation$DownloadBasePath" -ItemType directory
        $bacpacFilename = "$DownloadBasePath/" + $DatabaseBackupName + ".bacpac"
    } else {
        $bacpacFilename = $DatabaseBackupName + ".bacpac"
    }

    $context = New-AzStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey $StorageAccountKey
    Get-AzStorageBlobContent -Container $ContainerName -Blob $bacpacFilename -Destination $BlobDownloadLocation -Context $context -Force

    $customConnectionString = "Data Source=tcp:$ServerName;Initial Catalog=$DatabaseName;Persist Security Info=False;User ID=$Username;Password=$Password;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

    #$PackageExecutablePath = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\150\sqlpackage.exe"

    $bacpacLocation = "";
    if($BlobDownloadLocation) {
        $bacpacLocation = "$BlobDownloadLocation$bacpacFilename"
    }else {
        $bacpacLocation = $bacpacFilename
    }

    $output = & $PackageExecutablePath /a:import /tcs:$customConnectionString /sf:$bacpacLocation /p:DatabaseEdition=Standard /p:DatabaseServiceObjective=S2 /p:Storage=File
    if ($LastExitCode -ne 0)
    {
        Write-Output "ERROR: "
        Write-Output $output
        Remove-Database -DatabaseName $DatabaseName -ConnectionString $ConnectionString
        throw "Failed to import database"
    } else {
        Write-Output $output
    }
}

Export-ModuleMember -Function Restore-DatabaseFromStorage