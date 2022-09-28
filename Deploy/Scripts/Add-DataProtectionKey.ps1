param
(
    [Parameter(Mandatory = $true)]
    [string]
    $vaultName,

    [Parameter(Mandatory = $true)]
    [string]
    $keyName
)
function Add-Key()
{
    az keyvault key create --vault-name $vaultName --name $keyName --ops decrypt encrypt sign unwrapKey wrapKey
}

function Add-KeyPermission($dataProtectionKeyDetails)
{
    $differenceInKeyOps = Compare-Object -ReferenceObject $dataProtectionKeyDetails.key.keyOps -DifferenceObject @('decrypt', 'encrypt', 'sign', 'unwrapKey', 'wrapKey')

    if ($differenceInKeyOps.InputObject.Count -gt 0)
        {
            Write-Output "Creating the key vault key $keyName with the required permissions."
            Add-Key
        }
        else{
            Write-Output "No updates performed. The required permissions already exist."
        }
}

try{
    $dataProtectionKeyDetails = az keyvault key show --vault-name $vaultName --name $keyName | ConvertFrom-Json
    Add-KeyPermission -dataProtectionKeyDetails $dataProtectionKeyDetails
}
catch
{
    Write-Output "No keys exist in the key vault. Adding the key $keyName."
    Add-Key
}