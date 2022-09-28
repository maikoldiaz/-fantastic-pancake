param(
 [Parameter(Mandatory=$true)]
 [string]
 $spId,

 [Parameter(Mandatory=$true)]
 [string]
 $spSecret,

 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory=$true)]
 [string]
 $clusterName,

 [Parameter(Mandatory=$true)]
 [string]
 $serverApplicationId,

 [Parameter(Mandatory=$true)]
 [string]
 $serverApplicationSecret,

 [Parameter(Mandatory=$true)]
 [string]
 $clientAppId
 )

Write-Output "starting deployment"

#Generate the Secret using this command and update this in the variable group.
#$spSecret= az ad sp credential reset --name $spId --append --query password -o tsv #pragma: allowlist secret

az aks update-credentials --resource-group $resourceGroupName --name $clusterName --reset-service-principal --service-principal $spId --client-secret $spSecret

az aks update-credentials --resource-group $resourceGroupName --name $clusterName --reset-aad --aad-server-app-id $serverApplicationId --aad-server-app-secret $serverApplicationSecret --aad-client-app-id $clientAppId