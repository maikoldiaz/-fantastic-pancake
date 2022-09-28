param(
    [Parameter(Mandatory=$true)]
	[string]$ResourceGroupName,
    [Parameter(Mandatory=$true)]
	[string]$resourceName,
    [Parameter(Mandatory=$true)]
	[string]$key
	)

	# Check Current Status
	$status = Get-AzResource -ResourceGroupName $ResourceGroupName -Name $resourceName

	if ($status) {
		$value = "false"
	}
	else {
		$value = "true"
	}

	Write-Output "##vso[task.setvariable variable=$key;]$value"