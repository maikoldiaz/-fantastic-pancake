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
    $apiNames
)

$apiNameList = $apiNames.split(",").Trim()
$context = New-AzApiManagementContext -ResourceGroupName $resourceGroupName -ServiceName $apimServiceName
foreach($apiName in $apiNameList){
    $existingApi = Get-AzApiManagementApi -Context $context -Name $apiName -ErrorAction SilentlyContinue

    if($existingApi){
        Remove-AzApiManagementApi -Context $context -ApiId $existingApi.ApiId
        Write-Output "Removed $apiName from APIM."
    }
}




