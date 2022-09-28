
#Parameters
param(
    [Parameter(Mandatory=$true)]
	[string]$subscriptionId,
	[Parameter(Mandatory=$true)]
	[string]$resourceGroupName,
	[Parameter(Mandatory=$true)]
	[string]$resourceType,
	[Parameter(Mandatory=$true)]
	[string]$resourceName,
	[string]$logAnalyticsWorkspaceId
	)
Set-Item Env:\SuppressAzurePowerShellBreakingChangeWarnings "true"

$resourceId = "/subscriptions/$subscriptionId/resourceGroups/$resourceGroupName/providers/$resourceType/$resourceName"

Set-AzDiagnosticSetting -ResourceId $resourceId -Enabled $True -WorkspaceId $logAnalyticsWorkspaceId