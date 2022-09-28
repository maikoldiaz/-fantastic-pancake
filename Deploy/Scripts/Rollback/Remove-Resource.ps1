param(
	[Parameter(Mandatory=$true)]
    [string]$resourceGroupName,
	[Parameter(Mandatory=$true)]
    [string]$resourceName,
    [Parameter(Mandatory=$true)]
    [string]$resourceType
 )

Remove-AzResource -ResourceGroupName $resourceGroupName -ResourceName $resourceName -ResourceType $resourceType -Force -ErrorAction SilentlyContinue