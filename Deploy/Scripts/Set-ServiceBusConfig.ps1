##Parameters
param(
    [Parameter(Mandatory=$true)]
	[string]$Namespace,
    [Parameter(Mandatory=$true)]
	[string]$TenantId
	)

$data = '{\"ServiceBusSettings.Namespace\":\"'+$Namespace+'\",\"ServiceBusSettings.TenantId\":\"'+$TenantId+'\"}'

Write-Output $data
##Set Output Variable.
$key="serviceBusConfig"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"