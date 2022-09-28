param(
 [Parameter(Mandatory = $true)]
 [string]
 $storageName,

 [Parameter(Mandatory = $true)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory = $true)]
 [string]
 $vaultName,

 [Parameter(Mandatory = $true)]
 [string]
 $serviceBusNamespace,

 [Parameter(Mandatory = $true)]
 [string]
 $sqlconnectionstring,

 [Parameter(Mandatory = $true)]
 [string]
 $odbcsqlconnectionstring,

 [Parameter(Mandatory = $true)]
 [string]
 $sql,

 [Parameter(Mandatory = $true)]
 [string]
 $storage,

 [Parameter(Mandatory = $true)]
 [string]
 $serviceBus,

 [Parameter(Mandatory = $true)]
 [string]
 $redis,

 [Parameter(Mandatory = $true)]
 [string]
 $redisName,

 [Parameter(Mandatory = $true)]
 [string]
 $signalRName,

 [Parameter(Mandatory = $true)]
 [string]
 $signalR,

 [Parameter(Mandatory = $true)]
 [string]
 $keyType
)

function Save-SecretInKeyVault($secretName, $secretValue, $vaultName) {
	$Secret = ConvertTo-SecureString -String $secretValue -AsPlainText -Force #pragma: allowlist secret
	Set-AzKeyVaultSecret -VaultName $vaultName -Name $secretName -SecretValue $Secret
}

if ($sql -eq "true") {
	Save-SecretInKeyVault -secretName "SqlConnectionString" -secretValue $sqlconnectionstring -vaultName $vaultName

	Save-SecretInKeyVault -secretName "OdbcSqlConnectionString" -secretValue $odbcsqlconnectionstring -vaultName $vaultName
}

if ($storage -eq "true") {

	if($keyType -eq "primary")
	{
		New-AzStorageAccountKey -ResourceGroupName $resourceGroupName -Name $storageName -KeyName key1
		$accessKeys = Get-AzStorageAccountKey -ResourceGroupName $resourceGroupName -Name $storageName
		$key = $accessKeys[0].Value
	}else{
		New-AzStorageAccountKey -ResourceGroupName $resourceGroupName -Name $storageName -KeyName key2
		$accessKeys = Get-AzStorageAccountKey -ResourceGroupName $resourceGroupName -Name $storageName
		$key = $accessKeys[1].Value
	}

	$connectionString = "DefaultEndpointsProtocol=https;AccountName=$storageName;AccountKey=$key;EndpointSuffix=core.windows.net"
	Save-SecretInKeyVault -secretName "storageaccesskey" -secretValue $key -vaultName $vaultName

	Save-SecretInKeyVault -secretName "StorageConnectionString" -secretValue $connectionString -vaultName $vaultName

	Save-SecretInKeyVault -secretName "Settings--StorageConnectionString" -secretValue $connectionString -vaultName $vaultName
}

if ($serviceBus -eq "true") {
	if($keyType -eq "primary")
	{
		$serviceBusConnection = New-AzServiceBusKey -ResourceGroupName $resourceGroupName -Namespace $serviceBusNamespace -Name RootManageSharedAccessKey -RegenerateKey PrimaryKey
	}else{
		$serviceBusConnection = New-AzServiceBusKey -ResourceGroupName $resourceGroupName -Namespace $serviceBusNamespace -Name RootManageSharedAccessKey -RegenerateKey SecondaryKey
	}
	Save-SecretInKeyVault -secretName "IntServiceBusConnectionString" -secretValue $serviceBusConnection.PrimaryConnectionString -vaultName $vaultName
}

if($redis -eq "true"){
	if($keyType -eq "primary")
	{
		$keys = New-AzRedisCacheKey -ResourceGroupName $resourceGroupName -Name $redisName -KeyType Primary -Force
		$redisKey = $keys.PrimaryKey
	}else{
		$keys = New-AzRedisCacheKey -ResourceGroupName $resourceGroupName -Name $redisName -KeyType Secondary -Force
		$redisKey = $keys.SecondaryKey
	}
	$connString = "$redisName.redis.cache.windows.net:6380,password=$redisKey,ssl=True,abortConnect=False"
	Save-SecretInKeyVault -secretName "RedisCacheConnectionString" -secretValue $connString -vaultName $vaultName
	Save-SecretInKeyVault -secretName "Settings--RedisConnectionString" -secretValue $connString -vaultName $vaultName
}

if($signalR -eq "true"){
	if($keyType -eq "primary")
	{
		$keys = New-AzSignalRKey -ResourceGroupName $resourceGroupName -Name $signalRName -KeyType Primary
		$signalRKey = $keys.PrimaryKey
	}else{
		$keys = New-AzSignalRKey -ResourceGroupName $resourceGroupName -Name $signalRName -KeyType Secondary
		$signalRKey = $keys.SecondaryKey
	}

	$signalRConnString = "Endpoint=https://$signalRName.service.signalr.net;AccessKey=$signalRKey;Version=1.0;"
	Save-SecretInKeyVault -secretName "Settings--SignalRConnectionString" -secretValue $signalRConnString -vaultName $vaultName
}

