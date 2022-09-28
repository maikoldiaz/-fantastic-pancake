param(
    [Parameter(Mandatory=$true)]
    [string]$apimServiceName,
	[Parameter(Mandatory=$true)]
    [string]$resourceGroupName,
	[Parameter(Mandatory=$true)]
    [string]$certificateFilePath,
	[Parameter(Mandatory=$true)]
    [string]$certificatePassword,
    [Parameter(Mandatory=$true)]
    [string]$dnsName
)

# Api Management service specific details
$Secure_String_Pwd = ConvertTo-SecureString $certificatePassword -AsPlainText -Force

$proxyHostnameConfig = New-AzApiManagementCustomHostnameConfiguration -Hostname $dnsName -HostnameType Proxy -PfxPath $certificateFilePath -PfxPassword $Secure_String_Pwd
$apim = Get-AzApiManagement -ResourceGroupName $resourceGroupName -Name $apimServiceName
$apim.ProxyCustomHostnameConfiguration = $proxyHostnameConfig
Set-AzApiManagement -InputObject $apim

