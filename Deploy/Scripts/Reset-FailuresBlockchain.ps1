param
(
    [Parameter(Mandatory=$true)]
    [string]$ModulePath,

    [Parameter(Mandatory = $false)]
    [String]$tenantId,

    [Parameter(Mandatory = $false)]
    [String]$clientId,

    [Parameter(Mandatory = $false)]
    [String]$clientSecret,

    [Parameter(Mandatory = $false)]
    [String]$apiBaseUrl,

    [Parameter(Mandatory = $false)]
    [String]$isCritical,

    [Parameter(Mandatory = $false)]
    [int]$takeRecords
)


Import-Module -Name "$ModulePath\GetTokenForApi"

function Process-FailureBlockchain() {
   $headers = New-Object 'System.Collections.Generic.Dictionary[String,String]'
   $headers.Add("Accept", "application/json")
   $headers.Add("Content-Type", "application/json")
   $headers.Add("Authorization", "Bearer " + $script:accessToken)

   $requrl =  "https://$($apiBaseUrl)-api.ecopetrol.com.co/true/api/v1/failures/blockchain"
   Write-Output 'Getting the failures blockchain...'
   $isCritical = $isCritical.ToLower()
    $url = $requrl+"?isCritical=$isCritical&takeRecords=$takeRecords"
   $result = Invoke-RestMethod -Uri $url -Headers $headers -Method get -ContentType "application/json" 

   $json = ConvertTo-Json -InputObject $result
   Write-Output 'Reset the failed records...'
   Invoke-RestMethod -Uri $requrl -Headers $headers -Method POST -ContentType "application/json" -Body $json
   Write-Output 'Reset completed'
}

function Api-flow() {

   Write-Output "$($adTokenUrl)"
   Write-Output "$(Get-Date)"

   Write-Output "Generating Authentication token..."
   $script:accessToken = Get-AccessToken -tenantId $tenantId -clientId $clientId -clientSecret $clientSecret
   $script:resourceUrl = ''

   $script:resourceUrl = $apiBaseUrl

   Process-FailureBlockchain

   Write-Output "$(Get-Date)"
}

try {
   Api-flow;
}
catch {
   Write-Output $_.Exception.Message
   throw;
}