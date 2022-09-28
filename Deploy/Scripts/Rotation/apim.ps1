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
    $clientSecret
)

$context = New-AzApiManagementContext -ResourceGroupName $resourceGroupName -ServiceName $apimServiceName

Set-AzApiManagementIdentityProvider -Context $context -Type Aad -ClientSecret $clientSecret