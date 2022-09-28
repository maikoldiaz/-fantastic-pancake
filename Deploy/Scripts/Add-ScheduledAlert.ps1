param
(
    [Parameter(Mandatory = $true)]
    [string]
    $resourceGroupName,

    [Parameter(Mandatory = $true)]
    [int]
    $consecutiveThresholdCount,

    [Parameter(Mandatory = $true)]
    [int]
    $frequency,

    [Parameter(Mandatory = $true)]
    [int]
    $windowSize,

    [Parameter(Mandatory = $true)]
    [int]
    $percentageThreshold,

    [Parameter(Mandatory = $true)]
    [string]
    $query,

    [Parameter(Mandatory = $true)]
    [string]
    $dataSourceId,

    [Parameter(Mandatory = $true)]
    [string]
    $actionGroupId,

    [Parameter(Mandatory = $true)]
    [string]
    $deployLocation,

    [Parameter(Mandatory = $true)]
    [string]
    $description,

    [Parameter(Mandatory = $true)]
    [string]
    $alertName,

    [Parameter(Mandatory = $true)]
    [string]
    $severity,

    [Parameter(Mandatory = $true)]
    [string]
    $status,

    [Parameter(Mandatory = $true)]
    [string]
    $acrId
)

Set-Item Env:\SuppressAzurePowerShellBreakingChangeWarnings "true"
[Microsoft.Azure.Common.Authentication.AzureSession]::ClientFactory.AddUserAgent($acrId)

Write-Output "Starting alert creation..."
$enabled = !([System.Convert]::ToBoolean($status))

$source = New-AzScheduledQueryRuleSource -Query $query -DataSourceId $dataSourceId
$schedule = New-AzScheduledQueryRuleSchedule -FrequencyInMinutes $frequency -TimeWindowInMinutes $windowSize
$metricTrigger = New-AzScheduledQueryRuleLogMetricTrigger -ThresholdOperator "GreaterThan" -Threshold $consecutiveThresholdCount -MetricTriggerType "Consecutive" -MetricColumn "_ResourceId"

$triggerCondition = New-AzScheduledQueryRuleTriggerCondition -ThresholdOperator "GreaterThan" -Threshold $percentageThreshold -MetricTrigger $metricTrigger
$aznsActionGroup = New-AzScheduledQueryRuleAznsActionGroup -ActionGroup $actionGroupId

$alertingAction = New-AzScheduledQueryRuleAlertingAction -AznsAction $aznsActionGroup -Severity $severity -Trigger $triggerCondition

Write-Output "Creating alert rule..."
New-AzScheduledQueryRule -ResourceGroupName $resourceGroupName -Location $deployLocation -Action $alertingAction -Enabled $enabled -Description $description -Schedule $schedule -Source $source -Name $alertName.ToUpper()