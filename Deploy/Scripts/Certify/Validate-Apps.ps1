param(
    [Parameter(Mandatory=$true)]
    [string]
    $clientId,

    [Parameter(Mandatory=$true)]
    [string]
    $clientSecret,

    [Parameter(Mandatory=$true)]
    [string]
    $tenant
)

Write-Output "Starting certification..."

$result = az login --service-principal --username $clientId --password $clientSecret --tenant $tenant --allow-no-subscriptions
if ($result)
    {
        Write-Output "Validated successfully!"
    }
    else{
        $exceptionObject = [System.Exception]@{Source="Validate-SecurityGroupOwner.ps1";HelpLink="https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#upload-a-certificate-or-create-a-secret-for-signing-in"}
        Write-Error -Exception $exceptionObject -Message "Validation failed for the client! Please check if the clientId and secret and secret are configured correctly in the variable group, for this app. If the secret is not correct, please create a new secret for this app, update the secret in the variable group and re-run the pipeline."
    }