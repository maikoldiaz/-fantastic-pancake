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
    $productId
)

$context = New-AzApiManagementContext -ResourceGroupName $resourceGroupName -ServiceName $apimServiceName

# import api from Url
$existing = Get-AzApiManagementProduct -Context $context -ProductId $productId -ErrorAction SilentlyContinue

if(!$existing){
    New-AzApiManagementProduct -Context $context -ProductId $productId -Title $productId -Description "TRUE Product" -SubscriptionRequired $False -State "Published"
}
