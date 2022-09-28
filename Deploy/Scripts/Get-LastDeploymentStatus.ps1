param(
	[string][parameter(Mandatory = $true)] $deploymentName,
	[string][parameter(Mandatory = $true)] $resourceGroupName
)

az deployment group wait --created --name $deploymentName --resource-group $resourceGroupName