param(
    [Parameter(Mandatory = $true)]
    [string]$vaultName,
    [string]$targetObjectId,
    [Parameter(Mandatory=$true)]
	[string]$resourceGroupName,
	[string]$permissions
)

$ErrorActionPreference = 'stop'

if ($targetObjectId) {
    if ($permissions)
    {
        Write-Output "Received selective permissions for secrets, from parameters."
        $permissionsToSecrets = $permissions.Split(',')
    }
    else{
        Write-Output "Setting default secret permissions for secrets."
        $permissionsToSecrets = "Get,List,Set,Delete,Recover,Backup,Restore".Split(',')
    }
        Set-AzKeyVaultAccessPolicy -VaultName $vaultName -ResourceGroupName $resourceGroupName -ObjectId $targetObjectId -PermissionsToKeys Get,List,Update,Create,Import,Delete,Recover,Backup,Restore,unwrapKey,wrapKey -PermissionsToSecrets $permissionsToSecrets -PermissionsToCertificates  Get,List,Update,Create,Import,Delete,Recover,ManageContacts,ManageIssuers,GetIssuers,ListIssuers,SetIssuers,DeleteIssuers -ErrorAction Stop -BypassObjectIdValidation
}