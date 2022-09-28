param(
	[Parameter(Mandatory=$true)]
	[string]$resourceGroupName
)

function Get-StatusOfDeployment($resourceGroupName){
	return Get-AzResourceGroupDeployment -ResourceGroupName $resourceGroupName -Name "pid-adb8eac6-989a-5354-8580-19055546ec74"
}

$status = Get-StatusOfDeployment -resourceGroupName $resourceGroupName

while($status.ProvisioningState -ne "Succeeded")
{
	$status =  Get-StatusOfDeployment -resourceGroupName $resourceGroupName
}
