##Parameters
param
(
    [Parameter(Mandatory = $true)]
	[string]$objectId,

    [Parameter(Mandatory = $true)]
	[string]$resourceGroup,

    [Parameter(Mandatory = $true)]
	[string]$resourceName,

    [Parameter(Mandatory = $true)]
	[string]$roleDefinitionName,

    [Parameter(Mandatory = $true)]
	[string]$resourceType
)

##Script Execution Starts Here

$existing = Get-AzRoleAssignment -ObjectId $objectId -ResourceGroupName $resourceGroup -ResourceName $resourceName -ResourceType $resourceType -RoleDefinitionName $roleDefinitionName -ErrorAction Continue

if($existing)
{
	Write-Output "Assignment in place."
}else{
	New-AzRoleAssignment -ObjectId $objectId -RoleDefinitionName $roleDefinitionName -ResourceName $resourceName -ResourceType $resourceType -ResourceGroupName $resourceGroup
}