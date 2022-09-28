param
(
    [Parameter(Mandatory = $true)]
    [string]
    $resourceGroupName,

    [Parameter(Mandatory = $true)]
    [string]
    $apimServiceName,

    [Parameter(Mandatory = $true)]
    [string]
    $swaggerUrl,

    [Parameter(Mandatory = $true)]
    [string]
    $apiPath,

    [Parameter(Mandatory = $true)]
    [string]
    $policyFilePath,

	[Parameter(Mandatory = $true)]
    [string]
    $apiName,

	[Parameter(Mandatory = $true)]
    [string]
    $apiServiceUrl,

    [Parameter(Mandatory = $true)]
    [string]
    $productId
)

$context = New-AzApiManagementContext -ResourceGroupName $resourceGroupName -ServiceName $apimServiceName

# import api from Url
$existing = Get-AzApiManagementApi -Context $context -Name $apiName -ErrorAction SilentlyContinue

if($existing){
    $api = Import-AzApiManagementApi -Context $context -SpecificationUrl $swaggerUrl -SpecificationFormat Swagger -Path $apiPath -ApiId $existing.ApiId
}else{
$api = Import-AzApiManagementApi -Context $context -SpecificationUrl $swaggerUrl -SpecificationFormat Swagger -Path $apiPath
}

Set-AzApiManagementApi -Context $context -ServiceUrl $apiServiceUrl -ApiId $api.ApiId

Write-Output "Setting product as $productId for analytics API"
add-AzApiManagementApiToProduct -context $context -ProductId $productId -ApiId $api.ApiId

Write-Output "Setting policy for analytics API"
Set-AzApiManagementPolicy -Context $context -PolicyFilePath $policyFilePath -ApiId $api.ApiId