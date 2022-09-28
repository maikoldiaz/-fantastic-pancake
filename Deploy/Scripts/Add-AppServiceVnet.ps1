param(
 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory=$true)]
 [string]
 $appServiceName,

 [Parameter(Mandatory=$true)]
 [string]
 $vnetName,

 [Parameter(Mandatory=$true)]
 [string]
 $subnetNameofWebAppVnet
 )

 Write-Output "Adding Vnet Configuration to Web App."

az webapp vnet-integration add -g $resourceGroupName -n $appServiceName --vnet $vnetName.ToUpper() --subnet $subnetNameofWebAppVnet.ToUpper()