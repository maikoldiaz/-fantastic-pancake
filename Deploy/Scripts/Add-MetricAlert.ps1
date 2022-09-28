param
(
    [Parameter(Mandatory = $true)]
    [string]
    $alertName,

    [Parameter(Mandatory = $true)]
    [string]
    $resourceGroupName,

    [Parameter(Mandatory = $true)]
    [string]
    $scope,

    [Parameter(Mandatory = $true)]
    [string]
    $condition,

    [Parameter(Mandatory = $true)]
    [string]
    $windowSizeMins,

    [Parameter(Mandatory = $true)]
    [string]
    $frequencyMins,

    [Parameter(Mandatory = $true)]
    [string]
    $actionGroupName,

    [Parameter(Mandatory = $true)]
    [string]
    $subscriptionId,

    [Parameter(Mandatory = $true)]
    [string]
    $description,

    [Parameter(Mandatory = $true)]
    [string]
    $status,

    [Parameter(Mandatory = $true)]
    [string]
    $acrId
)

Set-Item Env:\SuppressAzurePowerShellBreakingChangeWarnings "true"
$alertName = $alertName.ToUpper()
$scope = "/subscriptions/$subscriptionId/resourceGroups/$resourceGroupName/providers/$scope"
$action = "/subscriptions/$subscriptionId/resourceGroups/$resourceGroupName/providers/Microsoft.Insights/actionGroups/$actionGroupName"

Write-Output "Setting the process Id as $acrId"
Set-Variable AZURE_HTTP_USER_AGENT= $acrId

Write-Output "Creating the alert $alertName"
az monitor metrics alert create -n $alertName -g $resourceGroupName --scopes $scope --condition $condition --window-size $windowSizeMins --evaluation-frequency $frequencyMins --action $action --description $description --disabled $status

Write-Output "Created the alert $alertName"
