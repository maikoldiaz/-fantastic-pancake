##Parameters
    param(
	[Parameter(Mandatory=$true)]
	[string]$AdministratorGroupId,
	[Parameter(Mandatory=$true)]
	[string]$ApproverGroupId,
	[Parameter(Mandatory=$true)]
	[string]$ProfessionalSegmentBalancesGroupId,
	[Parameter(Mandatory=$true)]
	[string]$ProgrammerGroupId,
	[Parameter(Mandatory=$true)]
	[string]$QueryGroupId,
	[Parameter(Mandatory=$true)]
	[string]$AuditorGroupId,
	[Parameter(Mandatory=$true)]
	[string]$ChainGroupId
	)

	$data = '{\"UserRoleSettings.Mapping\":{\"Administrator\":\"'+$AdministratorGroupId+'\",\"Approver\":\"'+$ApproverGroupId+'\",\"ProfessionalSegmentBalances\":\"'+$ProfessionalSegmentBalancesGroupId+'\",\"Programmer\":\"'+$ProgrammerGroupId+'\",\"Query\":\"'+$QueryGroupId+'\",\"Auditor\":\"'+$AuditorGroupId+'\",\"Chain\":\"'+$ChainGroupId+'\"}}'

	Write-Output $data
	##Set Output Variable.
	$key="roleGroupConfig"
	$value = $data
	Write-Output "##vso[task.setvariable variable=$key;]$value"