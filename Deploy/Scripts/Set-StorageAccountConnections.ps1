##Parameters
param(
 [string]
 $storageAccountName,

 [string]
 $resourceGroupName,

 [string]
 $storageAccountKeyName,

 [string]
 $storageAccessKeyName,

 [string]
 $dataEncryptionStorageSecret,

 [string]
 $webAppsForAppSettingsUpdate,

 [string]
 $vaultName
)
function Save-SecretInKeyVault($secretName, $secretValue, $vaultName) {
	$Secret = ConvertTo-SecureString -String $secretValue -AsPlainText -Force #pragma: allowlist secret
	Set-AzKeyVaultSecret -VaultName $vaultName -Name $secretName -SecretValue $Secret
}

function Save-AppSettingsStorageConnString($storageConnectionString, $webAppsForAppSettingsUpdate, $isStagingSlot) {
	$webAppsForAppSettingsUpdate = $webAppsForAppSettingsUpdate.split(",")
	#Update the connection string in function apps
	foreach ($webapp in $webAppsForAppSettingsUpdate) {

		if($isStagingSlot -eq "true")
		{
			$app = Get-AzWebAppSlot -ResourceGroupName $resourceGroupName -Name $webapp -Slot staging
		}else{
			$app = Get-AzWebApp -ResourceGroupName $resourceGroupName -Name $webapp
		}

		$newAppSettingList = @{}

		Write-Verbose "Read all existing settings..."
		ForEach ($kvp in $app.SiteConfig.AppSettings) {
			$newAppSettingList[$kvp.Name] = $kvp.Value
		}

		$newAppSettingList.IntegrationStorageConnectionString = $storageConnectionString
		$newAppSettingList.AzureWebJobsStorage = $storageConnectionString
		$newAppSettingList.WEBSITE_CONTENTAZUREFILECONNECTIONSTRING = $storageConnectionString

		if($isStagingSlot -eq "true")
		{
		# Post updated app back to azure
			Set-AzWebAppSlot -ResourceGroupName $resourceGroupName -Name $webapp -AppSettings $newAppSettingList -Slot staging
		}else{
			Set-AzWebApp -ResourceGroupName $resourceGroupName -Name $webapp -AppSettings $newAppSettingList
		}
	}
}


$data = (Get-AzStorageAccountKey -ResourceGroupName $resourceGroupName -AccountName $storageAccountName)

$accountKey = $data[0].Value

$accountConnectionString = "DefaultEndpointsProtocol=https;AccountName=$storageAccountName;AccountKey=$accountKey;EndpointSuffix=core.windows.net"

Save-SecretInKeyVault -secretName $storageAccountKeyName -secretValue $accountConnectionString -vaultName $vaultName

Save-SecretInKeyVault -secretName $storageAccessKeyName -secretValue $accountKey -vaultName $vaultName

Save-SecretInKeyVault -secretName $dataEncryptionStorageSecret -secretValue $accountConnectionString -vaultName $vaultName

Save-AppSettingsStorageConnString -storageConnectionString $accountConnectionString -webAppsForAppSettingsUpdate $webAppsForAppSettingsUpdate -isStagingSlot "false"

Save-AppSettingsStorageConnString -storageConnectionString $accountConnectionString -webAppsForAppSettingsUpdate $webAppsForAppSettingsUpdate -isStagingSlot "true"