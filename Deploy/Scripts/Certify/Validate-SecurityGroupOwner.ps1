param
(
    [Parameter(Mandatory = $true)]
    [String]$sqlMsiGroupId,

    [Parameter(Mandatory = $true)]
    [String]$servicePrincipal
)

Write-Output "Verifying the security group owner for SQL MSI Group"

$owners = az ad group owner list --group $sqlMsiGroupId | ConvertFrom-Json

if(!$owners -or $owners.count -eq 0)
{
    Write-Error "The service principal is not correctly configured for sqlMsiGroupId. The security group SQL MSI Integration, doesn't have the owner as the service principal for this environment. Please add the service principal as the owner in the azure portal, and then re-run this pipeline."
}

$owner = $owners | Where-Object { $_.objectType -eq "ServicePrincipal" }
Write-Output "Got the owner of the security group" $owner
    if ($owner -and $owner.appId -eq $servicePrincipal)
        {
            Write-Output "The service principal is correctly configured for sqlMsiGroupId"
        }
        else{
            $exceptionObject = [System.Exception]@{Source="Validate-SecurityGroupOwner.ps1";HelpLink="https://docs.microsoft.com/en-us/azure/active-directory/fundamentals/active-directory-accessmanagement-managing-group-owners"}
            Write-Error -Exception $exceptionObject -Message "The service principal is not correctly configured for sqlMsiGroupId : $sqlMsiGroupId. The application Id of the owner of the SQL MSI Security group does not match with the Service principal configured in the variable group, for this environment. Please check if the servicePrincipal is correctly configured in the variable group, and if it is, please configure the owner of the security group, as this service principal, in the azure portal."
        }




