param(
    [Parameter(Mandatory=$true)]
    [string]$resourceId,
    [Parameter(Mandatory=$true)]
    [string]$workspaceId
)
Set-Item Env:\SuppressAzurePowerShellBreakingChangeWarnings "true"

Set-AzDiagnosticSetting -ResourceId $resourceId -Enabled $True -WorkspaceId $workspaceId