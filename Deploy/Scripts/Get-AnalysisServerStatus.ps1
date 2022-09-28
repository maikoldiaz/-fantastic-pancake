param(
    [Parameter(Mandatory=$true)]
	[string]$ResourceGroupName,
    [Parameter(Mandatory=$true)]
	[string]$AnalysisServerName
	)

try {
	# Check Current Status
	$analysisServer = Get-AzAnalysisServicesServer -ResourceGroupName $ResourceGroupName -Name $AnalysisServerName

	if ($analysisServer.State -eq "Paused") {
		Write-Output "Resuming Analysis Server..."
		Resume-AzAnalysisServicesServer -ResourceGroupName $ResourceGroupName -Name $AnalysisServerName
		Write-Output "Analysis Service Resumed successfully"
	}
	else {
		Write-Output "Analysis Service is already in resumed state"
	}
}
catch {
	Write-Output "Unable to get status of $AnalysisServerName. $Error[0]"
}