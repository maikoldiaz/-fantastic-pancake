<#
 .Synopsis
  Get access token api

 .Parameter tenantId
  The enviroment for the url api

 .Parameter clientId
  Client for connect to api

 .Parameter clientSecret
  Secret api for connect to api

 .Example
   Get-AccessToken -tenantId "i34h08fuh3oo8945kej" -clientId "kas8d8asd" -clientSecret "js874h4h23942k"
#>

function Get-AccessToken($tenantId, $clientId, $clientSecret) {

    $adTokenUrl = "https://login.microsoftonline.com/$($tenantId)/oauth2/v2.0/token"


$body = @{
    grant_type = "client_credentials"
    client_id = $clientId
    client_secret = $clientSecret
    scope = "api://$($clientId)/.default"
} 

    $response = Invoke-RestMethod -Method 'Post' -Uri $adTokenUrl -ContentType "application/x-www-form-urlencoded" -Body $body

    return $response.access_token
}

Export-ModuleMember -Function Get-AccessToken