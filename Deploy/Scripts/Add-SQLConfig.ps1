##Parameters
param
(
	[Parameter(Mandatory = $true)]
	[string]$TenantId
)
$data = '{\"SqlConnectionSettings.TenantId\":\"'+$TenantId+'\"}'

Write-Output $data
##Set Output Variable.
$key = "sqlconfig"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"