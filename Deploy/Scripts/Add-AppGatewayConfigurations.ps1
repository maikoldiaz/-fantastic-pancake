param(
 [Parameter(Mandatory=$true)]
 [string]
 $ResourceGroupName,

 [Parameter(Mandatory=$true)]
 [string]
 $AppGatewayName,

 [Parameter(Mandatory=$true)]
 [string]
 $CertPwd,

 [Parameter(Mandatory=$true)]
 [string]
 $CertFilepath,

 [Parameter(Mandatory=$true)]
 [string]
 $backendPoolName,

 [Parameter(Mandatory=$true)]
 [string]
 $BackendFqdns,

 [Parameter(Mandatory=$true)]
 [string]
 $frontendportName,

 [Parameter(Mandatory=$true)]
 [string]
 $httpListenerName,

 [Parameter(Mandatory=$true)]
 [string]
 $probeConfigName,

 [Parameter(Mandatory=$true)]
 [string]
 $backendHttpSettingsName,

 [Parameter(Mandatory=$true)]
 [string]
 $RuleName,

 [Parameter(Mandatory=$true)]
 [string]
 $httpListenerHostName,

 [Parameter(Mandatory=$true)]
 [string]
 $BackendHttpHostName
 )

Set-Item Env:\SuppressAzurePowerShellBreakingChangeWarnings "true"

# Check if app Gateway is present or not
$AppGW = Get-AzApplicationGateway -Name $AppGatewayName -ResourceGroupName $ResourceGroupName -ErrorAction SilentlyContinue
if(!$AppGW)
{
	Write-Output "App Gateway Not Found"
	exit
}

$testpath = Test-Path -Path $CertFilepath
if(!$testpath)
{
    Write-Output "File Not Found"
	exit
}


#Adding Certificate to App Gateway
$AppGW = Get-AzApplicationGateway -Name $AppGatewayName -ResourceGroupName $ResourceGroupName -ErrorAction SilentlyContinue
$password = ConvertTo-SecureString $CertPwd -AsPlainText -Force #pragma: allowlist secret

$CertName = (Get-PfxData -Password $password -FilePath $CertFilepath).EndEntityCertificates.Thumbprint

$Cert = Get-AzApplicationGatewaySslCertificate -ApplicationGateway $AppGW -Name $CertName -ErrorAction SilentlyContinue

if(!$Cert)
{
    $AppGW = Add-AzApplicationGatewaySslCertificate -ApplicationGateway $AppGW -Name $CertName -CertificateFile $CertFilepath -Password $password
    Set-AzApplicationGateway -ApplicationGateway $AppGw
}


#Create Backend Pool with FQDN
$AppGw = Get-AzApplicationGateway -Name $AppGatewayName -ResourceGroupName $ResourceGroupName -ErrorAction SilentlyContinue

$backendPool = Get-AzApplicationGatewayBackendAddressPool -ApplicationGateway $AppGw -Name $backendPoolName.ToUpper() -ErrorAction SilentlyContinue

if(!$backendPool)
{
    $AppGw = Add-AzApplicationGatewayBackendAddressPool -ApplicationGateway $AppGw -Name $backendPoolName.ToUpper() -BackendFqdns $BackendFqdns
    Set-AzApplicationGateway -ApplicationGateway $AppGw
}


#FrontEnd IP Config
$AppGw = Get-AzApplicationGateway -Name $AppGatewayName -ResourceGroupName $ResourceGroupName -ErrorAction SilentlyContinue
$frontendIpConfig = Get-AzApplicationGatewayFrontendIPConfig -ApplicationGateway $AppGw -ErrorAction SilentlyContinue | Where-Object {$_.PublicIPAddress -ne $null}

if(!$frontendIpConfig)
{
    exit
}


#Front end port
$AppGw = Get-AzApplicationGateway -Name $AppGatewayName -ResourceGroupName $ResourceGroupName -ErrorAction SilentlyContinue
$frontendPort = Get-AzApplicationGatewayFrontendPort -ApplicationGateway $AppGw -ErrorAction SilentlyContinue | Where-Object {$_.Port -eq "443"}
if(!$frontendPort)
{
    $AppGw = Add-AzApplicationGatewayFrontendPort -ApplicationGateway $AppGw -Name $frontendportName -Port 443
    Set-AzApplicationGateway -ApplicationGateway $AppGw
}

#Http Listener
$AppGw = Get-AzApplicationGateway -Name $AppGatewayName -ResourceGroupName $ResourceGroupName -ErrorAction SilentlyContinue

$httpListener= Get-AzApplicationGatewayHttpListener -ApplicationGateway $AppGw -Name $httpListenerName -ErrorAction SilentlyContinue
if(!$httpListener)
{
    $Cert = Get-AzApplicationGatewaySslCertificate -ApplicationGateway $AppGW -Name $CertName -ErrorAction Stop
    $frontendPort = Get-AzApplicationGatewayFrontendPort -ApplicationGateway $AppGw -ErrorAction Stop | Where-Object {$_.Port -eq "443"}

    #Multi site
    $AppGw = Add-AzApplicationGatewayHttpListener -ApplicationGateway $AppGw -Name $httpListenerName -Protocol "Https" -FrontendIpConfiguration $frontendIpConfig -FrontendPort $frontendPort -SslCertificate $Cert -HostName $httpListenerHostName
    
    Set-AzApplicationGateway -ApplicationGateway $AppGw
}



#Custom Probe
# Define the status codes to match for the probe
$AppGw = Get-AzApplicationGateway -Name $AppGatewayName -ResourceGroupName $ResourceGroupName -ErrorAction SilentlyContinue
$match = New-AzApplicationGatewayProbeHealthResponseMatch -StatusCode 200-399

$probe = Get-AzApplicationGatewayProbeConfig -ApplicationGateway $AppGw -Name $probeConfigName -ErrorAction SilentlyContinue
if(!$probe)
{
    $AppGw =Add-AzApplicationGatewayProbeConfig -ApplicationGateway $AppGw -Name $probeConfigName -Protocol Https -Path / -Interval 30 -Timeout 120 -UnhealthyThreshold 3 -Match $match -PickHostNameFromBackendHttpSettings
    Set-AzApplicationGateway -ApplicationGateway $AppGw
}


#Backend http
$AppGw = Get-AzApplicationGateway -Name $AppGatewayName -ResourceGroupName $ResourceGroupName -ErrorAction SilentlyContinue

$httpBackendSettings = Get-AzApplicationGatewayBackendHttpSettings -ApplicationGateway $AppGW -Name $backendHttpSettingsName -ErrorAction SilentlyContinue
if(!$httpBackendSettings){
    $probe = Get-AzApplicationGatewayProbeConfig -ApplicationGateway $AppGw -Name $probeConfigName -ErrorAction Stop
    $AppGw = Add-AzApplicationGatewayBackendHttpSettings -ApplicationGateway $AppGw -Name $backendHttpSettingsName -Port 443 -Protocol Https -CookieBasedAffinity "Disabled" -RequestTimeout 60 -Probe $probe -HostName $BackendHttpHostName
    Set-AzApplicationGateway -ApplicationGateway $AppGw
}



# Create a new rule
$AppGw = Get-AzApplicationGateway -Name $AppGatewayName -ResourceGroupName $ResourceGroupName -ErrorAction SilentlyContinue
# Get all required details
$httpBackendSettings = Get-AzApplicationGatewayBackendHttpSettings -ApplicationGateway $AppGW -Name $backendHttpSettingsName -ErrorAction Stop
$httpListener= Get-AzApplicationGatewayHttpListener -ApplicationGateway $AppGw -Name $httpListenerName -ErrorAction Stop
$backendPool = Get-AzApplicationGatewayBackendAddressPool -ApplicationGateway $AppGw -Name $backendPoolName -ErrorAction Stop

$rule = Get-AzApplicationGatewayRequestRoutingRule -ApplicationGateway $AppGW -Name $RuleName -ErrorAction SilentlyContinue

if(!$rule){
    $AppGw = Add-AzApplicationGatewayRequestRoutingRule -ApplicationGateway $AppGW -Name $RuleName -RuleType Basic -BackendHttpSettings $httpBackendSettings -HttpListener $httpListener -BackendAddressPool $backendPool
    Set-AzApplicationGateway -ApplicationGateway $AppGw
}
