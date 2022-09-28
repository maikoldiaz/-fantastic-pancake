param(
 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory=$true)]
 [string]
 $appServiceName,

 [Parameter(Mandatory=$true)]
 [string]
 $ruleName,

 [Parameter(Mandatory=$true)]
 [string]
 $vnetName,

 [Parameter(Mandatory=$true)]
 [string]
 $subnetName

 )

 Write-Output "Adding the access Restriction to App Service by adding the Vnet."

az webapp config access-restriction add -g $resourceGroupName -n $appServiceName --rule-name $ruleName --action Allow --vnet-name $vnetName --subnet $subnetName --priority 300
