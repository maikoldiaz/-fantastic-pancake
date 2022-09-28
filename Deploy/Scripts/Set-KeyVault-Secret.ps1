param(
 [Parameter(Mandatory=$True)]
 [string]
 $keyVaultName,

 [Parameter(Mandatory=$True)]
 [string]
 $secretName,

 [Parameter(Mandatory=$True)]
 [string]
 $secretValue)

#******************************************************************************
# Script body
# Execution begins here
#******************************************************************************
az keyvault secret set --vault-name $keyVaultName --name $secretName --value $secretValue