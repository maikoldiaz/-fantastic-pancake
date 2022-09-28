##Parameters
param(
	[Parameter(Mandatory=$true)]
	[AllowEmptyString()]
	[string]$Username,

	[Parameter(Mandatory=$true)]
	[AllowEmptyString()]
	[string]$BasePath
	)

	$data = '{\"SapSettings.Username\":\"'+$Username+'\",\"SapSettings.BasePath\":\"'+$BasePath+'\"}'

	Write-Output $data
	##Set Output Variable.
	$key="sapConfig"
	$value = $data
	Write-Output "##vso[task.setvariable variable=$key;]$value"