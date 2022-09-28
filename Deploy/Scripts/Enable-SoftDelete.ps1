param(
 [Parameter(Mandatory=$True)]
 [string]
 $keyVaultName
)

#******************************************************************************
# Script body
# Execution begins here
#******************************************************************************
$ErrorActionPreference = "Stop"

($resource = Get-AzResource -ResourceId (Get-AzKeyVault -VaultName $keyVaultName).ResourceId).Properties | Add-Member -MemberType "NoteProperty" -Name "enableSoftDelete" -Value "true" -force
Set-AzResource -resourceid $resource.ResourceId -Properties $resource.Properties -force