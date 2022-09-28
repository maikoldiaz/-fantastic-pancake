##Parameters
param(
    [Parameter(Mandatory=$true)]
	[string]$TenantId,
	[Parameter(Mandatory=$true)]
	[string]$ClientID,
	[Parameter(Mandatory=$true)]
	[string]$AnalysisServerName,
    [Parameter(Mandatory=$true)]
	[string]$AnalysisServerModelName,
    [Parameter(Mandatory=$true)]
	[string]$AnalysisServerAuditModelName,
	[Parameter(Mandatory=$true)]
	[string]$Region
	)

	$data = '{\"AnalysisSettings.TenantId\":\"'+$TenantId+'\",\"AnalysisSettings.ClientId\":\"'+$ClientID+'\",\"AnalysisSettings.ServerName\":\"'+$AnalysisServerName+'\",\"AnalysisSettings.ModelName\":\"'+$AnalysisServerModelName+'\",\"AnalysisSettings.AuditModelName\":\"'+$AnalysisServerAuditModelName+'\",\"AnalysisSettings.Region\":\"'+$Region+'\"}'

	Write-Output $data
	##Set Output Variable.
	$key="analysisServerConfig"
	$value = $data
	Write-Output "##vso[task.setvariable variable=$key;]$value"