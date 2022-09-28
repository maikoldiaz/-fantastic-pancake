param(
    [Parameter(Mandatory=$true)]
	[string]$resourceGroupName,
    [Parameter(Mandatory=$true)]
	[string]$apiManagementServiceName
	)

	$apimContext = New-AzApiManagementContext -ResourceGroupName $resourceGroupName -ServiceName $apiManagementServiceName
	$apimProducts = Get-AzApiManagementProduct -Context $apimContext

	$starterProductToDelete = $apimProducts |
	Where-Object {$_.Title -Match "Starter"}

	$unlimitedProductToDelete = $apimProducts |
	Where-Object {$_.Title -Match "Unlimited"}

	$apimApis = Get-AzApiManagementApi -Context $apimContext

	$apimApiToDelete = $apimApis |
	Where-Object {$_.ApiId -Match "echo-api"}

	if (!($null -eq $starterProductToDelete))
	{
		Remove-AzApiManagementProduct -Context $apimContext -ProductId "Starter" -DeleteSubscriptions
	}

	if (!($null -eq $unlimitedProductToDelete))
	{
		Remove-AzApiManagementProduct -Context $apimContext -ProductId "Unlimited" -DeleteSubscriptions
	}

	if (!($null -eq $apimApiToDelete))
	{
		Remove-AzApiManagementApi -Context $apimContext -ApiId "echo-api"
	}






