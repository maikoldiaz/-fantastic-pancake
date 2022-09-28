param(
[Parameter(Mandatory=$true)]
[String]$resourceGroup,
[Parameter(Mandatory=$true)]
[String]$swaggerUrl,
[Parameter(Mandatory=$true)]
[String]$apimName,
[Parameter(Mandatory=$true)]
[String]$apimApiDisplayName,
[Parameter(Mandatory=$true)]
[String]$apimApiPath,
[Parameter(Mandatory=$true)]
[String]$serviceUrl
)

Write-Output "Getting context"
$azcontext = New-AzApiManagementContext -ResourceGroupName $resourceGroup -ServiceName $apimName

Write-Output "Getting API details"
$existing = Get-AzApiManagementApi -Context $azcontext -Name $apimApiDisplayName -ErrorAction SilentlyContinue

Write-Output "Importing API"
Import-AzApiManagementApi -Context $azcontext -SpecificationFormat OpenApi -SpecificationUrl $swaggerUrl -ApiId $existing.ApiId -Path $apimApiPath

Write-Output "Setting API"
Set-AzApiManagementApi -Context $azcontext -ApiId $existing.ApiId -Protocols @("https") -Name $apimApiDisplayName -ServiceUrl $serviceUrl