##Parameters
    param(
	[Parameter(Mandatory=$true)]
	[string]$BasePath
	)

	$data = '{\"SystemSettings.BasePath\":\"https://'+$BasePath+'\"}'


	Write-Output $data
	##Set Output Variable.
	$key="systemSettingsConfig"
	$value = $data
	Write-Output "##vso[task.setvariable variable=$key;]$value"