param(
 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory=$true)]
 [string]
 $dataScienceVirtualMachineName,


 [Parameter(Mandatory=$true)]
 [string]
 $keyVaultName
)

az vm identity assign --name $dataScienceVirtualMachineName --resource-group $resourceGroupName

$id = az vm identity assign --name $dataScienceVirtualMachineName --resource-group $resourceGroupName --query 'systemAssignedIdentity' -o tsv

az keyvault set-policy --name $keyVaultName --object-id $id --secret-permissions get list


