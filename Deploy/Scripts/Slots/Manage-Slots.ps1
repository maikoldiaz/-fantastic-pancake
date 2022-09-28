param(
    [Parameter(Mandatory = $true)]
    [string]
    $resourceGroupName,

    [Parameter(Mandatory = $true)]
    [string]
    $appServiceName,

    [Parameter(Mandatory = $true)]
    [string]
    $appServicePlanName,

    [Parameter(Mandatory = $true)]
    [string]
    $slotName,

    [Parameter(Mandatory = $true)]
    [string]
    $action

)

#Variables
$prodSlot = "production"

if ($action -eq "CREATE") {

    $exists = Get-AzWebAppSlot -ResourceGroupName $resourceGroupName -Name $appServiceName -Slot $slotName -ErrorAction SilentlyContinue

    if ($exists) {
        Write-Output "Slot Already Exists"
    }
    else {
        New-AzWebAppSlot -ResourceGroupName $resourceGroupName -Name $appServiceName -AppServicePlan $appServicePlanName -Slot $slotName -ErrorAction SilentlyContinue
    }
}
elseif ($action -eq "SWITCH") {
    Start-AzWebAppSlot -ResourceGroupName $resourceGroupName -Name $appServiceName -Slot $prodSlot -ErrorAction SilentlyContinue
    Start-AzWebAppSlot -ResourceGroupName $resourceGroupName -Name $appServiceName -Slot $slotName -ErrorAction SilentlyContinue
    Switch-AzWebAppSlot -Name $appServiceName -ResourceGroupName $resourceGroupName -SourceSlotName $slotName -DestinationSlotName $prodSlot
    Stop-AzWebAppSlot -ResourceGroupName $resourceGroupName -Name $appServiceName -Slot $slotName -ErrorAction SilentlyContinue
}
elseif ($action -eq "START") {
    Start-AzWebAppSlot -ResourceGroupName $resourceGroupName -Name $appServiceName -Slot $slotName -ErrorAction SilentlyContinue
}
elseif ($action -eq "STOP") {
    Stop-AzWebAppSlot -ResourceGroupName $resourceGroupName -Name $appServiceName -Slot $slotName -ErrorAction SilentlyContinue
}
elseif ($action -eq "DELETE") {
    Remove-AzWebAppSlot -ResourceGroupName $resourceGroupName -Name $appServiceName -Slot $slotName -ErrorAction SilentlyContinue
}
else {
    Write-Output "NO ACTION SELECTED"
}