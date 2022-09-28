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
     [String]$ticketId,

     [Parameter(Mandatory = $false)]
     [String]$flagRecalculate
 )

Import-Module -Name "$ModulePath\GetTokenForApi"

function Process-RecalculateOwnership($headers){
   $requrlOwnerShipBalance =  "https://$($apiBaseUrl)-api.ecopetrol.com.co/true/api/v1/ownership/recalculateBalance?ticketId=$($ticketId)"
   Write-Output 'Executing Ownership recalculate balance...'
   $resultOwnerShipBalance = Invoke-RestMethod -Uri $requrlOwnerShipBalance -Headers $headers -Method POST -ContentType "application/json" 
   Write-Output "$($resultOwnerShipBalance)"
}


function Process-RecalculateCutOff($headers){
   $requrlCutOffBalance = "https://$($apiBaseUrl)-api.ecopetrol.com.co/true/api/v1/cutoff/recalculateBalance?ticketId=$($ticketId)"
   Write-Output 'Executing cutoff recalculate balance...'
   $resultCutoffBalance = Invoke-RestMethod -Uri $requrlCutOffBalance -Headers $headers -Method POST -ContentType "application/json" -Body $json
   Write-Output "$($resultCutoffBalance)"
}

function Process-RecalculateBalance() {

   $headers = New-Object 'System.Collections.Generic.Dictionary[String,String]'
   $headers.Add("Accept", "application/json")
   $headers.Add("Authorization", "Bearer " + $script:accessToken)

   if( $flagRecalculate -eq 'ownership'){
      Process-RecalculateOwnership($headers)
   }
   
   if( $flagRecalculate -eq 'cutoff'){
      Process-RecalculateCutOff($headers)
   }

   Write-Output 'completed'
}

function Api-flow() {

   Write-Output "$($adTokenUrl)"
   Write-Output "$(Get-Date)"

   Write-Output "Generating Authentication token..."
   $script:accessToken = Get-AccessToken -tenantId $tenantId -clientId $clientId -clientSecret $clientSecret
   $script:resourceUrl = ''

   $script:resourceUrl = $apiBaseUrl

   Process-RecalculateBalance

   Write-Output "$(Get-Date)"
}

try {
   Api-flow;
}
catch {
   Write-Output $_.Exception.Message
   throw;
}
