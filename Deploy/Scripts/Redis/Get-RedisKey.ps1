param(
 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory=$true)]
 [string]
 $redisName
 )

$keys = Get-AzRedisCacheKey -ResourceGroupName $resourceGroupName -Name $redisName

if($keys)
{
	##Set Output Variable.
	$key = "rediskey"
	$value = $keys.PrimaryKey
	Write-Output "##vso[task.setvariable variable=$key;]$value"
}else{
	Write-Output "Error occurred retrieving the Redis Key"
	exit 1
}