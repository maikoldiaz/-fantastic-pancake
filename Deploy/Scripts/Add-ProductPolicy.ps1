param(
    [string]$resourceGroupName,
    [Parameter(Mandatory=$true)]
    [string]$resourceName,
    [Parameter(Mandatory=$true)]
    [string]$policyPath,
    [Parameter(Mandatory=$true)]
    [string]$productId
)

Write-Output 'Deploying the product policy'
$apimContext = New-AzApiManagementContext -ResourceGroupName $resourceGroupName -ServiceName $resourceName
Set-AzApiManagementPolicy -Context $apimContext -PolicyFilePath $policyPath -ProductId $productId