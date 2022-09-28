param(
[Parameter(Mandatory=$true)]
[String]$resourceGroup,
[Parameter(Mandatory=$true)]
[String]$apimOauthServerName,
[Parameter(Mandatory=$true)]
[String]$apimName,
[Parameter(Mandatory=$true)]
[String]$clientId,
[Parameter(Mandatory=$true)]
[String]$clientSecret,
[Parameter(Mandatory=$true)]
[String]$grantType,
[Parameter(Mandatory=$true)]
[String]$defaultScope,
[Parameter(Mandatory=$true)]
[String]$tenantId
)

Set-Item Env:\SuppressAzurePowerShellBreakingChangeWarnings "true"

Write-Output "Getting Context"
$azcontext = New-AzApiManagementContext -ResourceGroupName $resourceGroup -ServiceName $apimName

$authorizationEndpointUrl = "https://login.microsoftonline.com/$tenantId/oauth2/v2.0/authorize"
$tokenEndpointUrl = "https://login.microsoftonline.com/$tenantId/oauth2/v2.0/token"

Write-Output "Getting Apim Oauth Server"
$authServer = Get-AzApiManagementAuthorizationServer -Context $azcontext -ServerId $apimOauthServerName -ErrorAction SilentlyContinue

if(!$authServer){
    Write-Output "Creating New Authorization Server"
    New-AzApiManagementAuthorizationServer -Context $azcontext -ServerId $apimOauthServerName -Name $apimOauthServerName -ClientRegistrationPageUrl "http://localhost" -AuthorizationEndpointUrl $authorizationEndpointUrl -TokenEndpointUrl $tokenEndpointUrl -ClientId $clientId -ClientSecret $clientSecret -AuthorizationRequestMethods @('Get') -DefaultScope $defaultScope -GrantTypes @($grantType) -ClientAuthenticationMethods @('Body') -TokenBodyParameters @{} -AccessTokenSendingMethods @('AuthorizationHeader')
}
else{
    Write-Output "Updating an existing Authorization Server"
    Set-AzApiManagementAuthorizationServer -Context $azcontext -ServerId $apimOauthServerName -Name $apimOauthServerName -ClientRegistrationPageUrl "http://localhost" -AuthorizationEndpointUrl $authorizationEndpointUrl -TokenEndpointUrl $tokenEndpointUrl -ClientId $clientId -ClientSecret $clientSecret -AuthorizationRequestMethods @('Get') -DefaultScope $defaultScope -GrantTypes @($grantType) -ClientAuthenticationMethods @('Body') -TokenBodyParameters @{} -AccessTokenSendingMethods @('AuthorizationHeader')
}

Write-Output "Authorization Server Setup Successful."