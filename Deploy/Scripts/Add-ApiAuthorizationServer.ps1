param(
[Parameter(Mandatory=$true)]
[String]$resourceGroup,
[Parameter(Mandatory=$true)]
[String]$apiName,
[Parameter(Mandatory=$true)]
[String]$apimName,
[Parameter(Mandatory=$true)]
[String]$path,
[Parameter(Mandatory=$true)]
[String]$authorizationServerName,
[Parameter(Mandatory=$true)]
[String]$serviceUrl
)

Write-Output "Getting Context"
$azcontext = New-AzApiManagementContext -ResourceGroupName $resourceGroup -ServiceName $apimName

Write-Output "Getting Api"
$api = Get-AzApiManagementApi -Context $azcontext -Name $apiName

Write-Output "Setting API Settings"
Set-AzApiManagementApi -Context $azcontext -Name $apiName -Path $path -Protocols "https" -ApiId $api.ApiId -ServiceUrl $serviceUrl -AuthorizationServerId $authorizationServerName

Write-Output "Api settings update Successful."