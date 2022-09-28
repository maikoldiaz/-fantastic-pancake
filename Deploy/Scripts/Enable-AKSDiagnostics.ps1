param(
 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory=$true)]
 [string]
 $aksResourceName,

 [Parameter(Mandatory=$true)]
 [string]
 $logAnalyticsResourceId
 )

Write-Output "starting deployment"

$existing = az aks show -g $resourceGroupName -n $aksResourceName

if($existing -match "logAnalyticsWorkspaceResourceID"){
	Write-Output "Diagnostics Already Enabled"
}else{
	az aks enable-addons -a monitoring -n $aksResourceName -g $resourceGroupName --workspace-resource-id $logAnalyticsResourceId
}
