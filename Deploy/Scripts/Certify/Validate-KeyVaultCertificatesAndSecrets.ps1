param( 
    [Parameter(Mandatory=$true)]
    [string]
    $requiredSecretKeys,

    [Parameter(Mandatory=$true)]
    [string]
    $requiredCertificateKeys,

    [parameter(Mandatory=$true)]
    [string]
    $keyVaultName
)

$requiredSecretKeysArray = $requiredSecretKeys.Split(",").Trim()
$requiredCertificateKeysArray = $requiredCertificateKeys.Split(",").Trim()

Write-Output "Required secret keys : $requiredSecretKeys"
Write-Output "Required certificate keys : $requiredCertificateKeys"

$existingSecretKeys= (az keyvault secret list --vault-name $keyVaultName | ConvertFrom-Json).Name
Write-Output "Existing secret keys : $existingSecretKeys"

$existingCertificateKeys= (az keyvault certificate list --vault-name $keyVaultName | ConvertFrom-Json).Name
Write-Output "Existing certificate keys : $existingCertificateKeys"

$requiredSecretKeysNotPresentInKeyVault = Compare-Object $requiredSecretKeysArray $existingSecretKeys | Where-Object {$_.sideindicator -eq "<="} | ForEach-Object {$_.inputobject}
$requiredCertificateKeysNotPresentInKeyVault = Compare-Object $requiredCertificateKeysArray $existingCertificateKeys | Where-Object {$_.sideindicator -eq "<="} | ForEach-Object {$_.inputobject}

if($requiredSecretKeysNotPresentInKeyVault -and $requiredCertificateKeysNotPresentInKeyVault)
{
    Write-Error "Required secret keys not present in key vault : $requiredSecretKeysNotPresentInKeyVault ; Required certificate keys not present in key vault : $requiredCertificateKeysNotPresentInKeyVault"
}

if($requiredSecretKeysNotPresentInKeyVault)
{
    Write-Error "Required secret keys not present in key vault : $requiredSecretKeysNotPresentInKeyVault"
}

if($requiredCertificateKeysNotPresentInKeyVault)
{
    Write-Error "Required certificate keys not present in key vault : $requiredCertificateKeysNotPresentInKeyVault"
}

Write-Output "Verification of secrets and certificates completed successfully!"
