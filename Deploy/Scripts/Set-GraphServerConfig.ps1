##Parameters
param(
    [Parameter(Mandatory=$true)]
	[string]$TenantId,
	[Parameter(Mandatory=$true)]
	[string]$ClientID
	)

	$data = '{\"GraphSettings.TenantId\":\"'+$TenantId+'\",\"GraphSettings.ClientId\":\"'+$ClientID+'\"}'

	Write-Output $data
	##Set Output Variable.
	$key="graphServerConfig"
	$value = $data
	Write-Output "##vso[task.setvariable variable=$key;]$value"