param(
 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory=$true)]
 [string]
 $functionAppName
)

az functionapp config set --always-on true --name $functionAppName --resource-group $resourceGroupName


