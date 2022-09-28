param(
    [Parameter(Mandatory=$true)]
	[string]$accountName,
    [Parameter(Mandatory=$true)]
	[string]$resourceGroupName
	)

try {
	$existingAutomationAccount = Get-AzAutomationAccount -Name $accountName -ResourceGroupName $resourceGroupName

	if ($existingAutomationAccount)
	{
		Remove-AzAutomationAccount -Name $accountName -Force -ResourceGroupName $resourceGroupName
		Write-Output "Automation account deleted."
	}
}
catch {
		Write-Output "Automation account not present. Moving on."
}






