##Parameters
param(
    [Parameter(Mandatory=$true)]
	[string]$TenantId,
	[Parameter(Mandatory=$true)]
	[string]$ClientId,
	[Parameter(Mandatory=$true)]
	[string]$Scope
	)



	$data = '{\"AnalyticsSettings.TenantId\":\"'+$TenantId+'\",\"AnalyticsSettings.ClientId\":\"'+$ClientId+'\",\"AnalyticsSettings.Scope\":\"'+$Scope+'\"}'

	Write-Output $data
	##Set Output Variable.
	$key="analyticsConfig"
	$value = $data
	Write-Output "##vso[task.setvariable variable=$key;]$value"