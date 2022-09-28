param(
	[string][parameter(Mandatory = $true)] $msiDisplayName
)

$ServicePrincipal = Get-AzADServicePrincipal -DisplayName $msiDisplayName

	##Set Output Variable.
	$key="msiAksServicePrincipal"
	$value = $ServicePrincipal.Id
	Write-Output "##vso[task.setvariable variable=$key;]$value"