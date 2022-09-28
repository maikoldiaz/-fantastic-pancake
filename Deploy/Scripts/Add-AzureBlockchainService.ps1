param(
 [Parameter(Mandatory=$true)]
 [string]
 $blockchainMemberName,

 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory=$true)]
 [string]
 $consortiumName,

 [Parameter(Mandatory=$true)]
 [string]
 $consortiumPassword,

 [Parameter(Mandatory=$true)]
 [string]
 $memberPassword,

 [Parameter(Mandatory=$true)]
 [string]
 $sku = "S0"
 )

Write-Output "starting deployment"

$consortiumName.ToUpper()
$resourceGroup = az group show --name $resourceGroupName --output json | ConvertFrom-Json
$location = $resourceGroup.location
Write-Output "Deployment location is $location"

$resource = az resource list --name $blockchainMemberName --resource-group $resourceGroupName

if($resource -eq "[]")
{
   Write-Output "Blockchain service with name: $blockchainMemberName not found in the given resource group: $resourceGroupName, Starting Blockchain infra deployment"
   az resource create --resource-group $resourceGroupName --name $blockchainMemberName --resource-type "Microsoft.Blockchain/blockchainMembers" --is-full-object --properties "{""""location"""": """"$location"""", """"properties"""": { """"password"""": """"$memberPassword"""", """"protocol"""": """"Quorum"""", """"consortium"""": """"$consortiumName"""", """"consortiumManagementAccountPassword"""": """"$consortiumPassword""""  }, """"sku"""": { """"name"""": """"$sku"""" }}"
}

Write-Output "Getting Default node key"