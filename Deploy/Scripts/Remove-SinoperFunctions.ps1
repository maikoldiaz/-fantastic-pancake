param(
 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory=$true)]
 [string]
 $functionAppName
 )

function Remove-FunctionAppSetting {
    param (
        [string]$resourceGroupName,
        [string]$functionAppName,
        [string]$settingName
    )

    $settingDetails = az functionapp config appsettings list --resource-group $resourceGroupName --name $functionAppName -o json | ConvertFrom-Json
    if ($settingDetails.name -like "*$settingName") {
        Write-Output "Deleting settings $settingName"
        az functionapp config appsettings delete --name $functionAppName --resource-group $resourceGroupName --setting-names $settingName
    } else {
        Write-Output "Function Setting $settingName not found"
    }
}

Remove-FunctionAppSetting -resourceGroupName $resourceGroupName -functionAppName $functionAppName -settingName 'MovementQueue'
Remove-FunctionAppSetting -resourceGroupName $resourceGroupName -functionAppName $functionAppName -settingName 'LossesQueue'
Remove-FunctionAppSetting -resourceGroupName $resourceGroupName -functionAppName $functionAppName -settingName 'SpecialMovementQueue'
Remove-FunctionAppSetting -resourceGroupName $resourceGroupName -functionAppName $functionAppName -settingName 'InventoryQueue'