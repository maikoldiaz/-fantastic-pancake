##Parameters
param
(
	[Parameter(Mandatory = $true)]
	[string]$EnvironmentId
)

$data = '{\"FlowSettings.EnvironmentId\":\"' + $EnvironmentId + '\"}'

Write-Output $data
##Set Output Variable.
$key = "flowconfig"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"