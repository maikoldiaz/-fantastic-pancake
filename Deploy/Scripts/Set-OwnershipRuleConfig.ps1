##Parameters
param(
	[Parameter(Mandatory=$true)]
	[string]$BasePath,

	[Parameter(Mandatory=$true)]
	[string]$RegistrationPath,

	[Parameter(Mandatory=$true)]
	[string]$OwnershipRulePath,

	[Parameter(Mandatory=$true)]
	[string]$OwnershipClientId,

	[Parameter(Mandatory=$true)]
	[string]$DeltaBasePath,

	[Parameter(Mandatory=$true)]
	[string]$DeltaApiPath,

	[Parameter(Mandatory=$true)]
	[string]$OfficialDeltaBasePath,

	[Parameter(Mandatory=$true)]
	[string]$OfficialDeltaApiPath
	)

	$data = '{\"OwnershipRuleSettings.ClientId\":\"'+$OwnershipClientId+'\",\"OwnershipRuleSettings.BasePath\":\"'+$BasePath+'\",\"OwnershipRuleSettings.RegistrationPath\":\"'+$RegistrationPath+'\",\"OwnershipRuleSettings.OwnershipRulePath\":\"'+$OwnershipRulePath+'\",\"OwnershipRuleSettings.DeltaBasePath\":\"'+$DeltaBasePath+'\",\"OwnershipRuleSettings.DeltaApiPath\":\"'+$DeltaApiPath+'\",\"OwnershipRuleSettings.OfficialDeltaBasePath\":\"'+$OfficialDeltaBasePath+'\",\"OwnershipRuleSettings.OfficialDeltaApiPath\":\"'+$OfficialDeltaApiPath+'\"}'


	Write-Output $data
	##Set Output Variable.
	$key="ownershipRuleConfig"
	$value = $data
	Write-Output "##vso[task.setvariable variable=$key;]$value"