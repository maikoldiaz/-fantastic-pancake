##Parameters
param(
    [Parameter(Mandatory=$true)]
	[string]$accountName
	)

$data = '{\"StorageSettings.AccountName\":\"'+$accountName+'\"}'

Write-Output $data
##Set Output Variable.
$key="storageConfig"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"