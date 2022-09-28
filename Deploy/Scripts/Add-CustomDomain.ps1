param(
    [Parameter(Mandatory=$true)]
    [string]$WebAppName,
	[Parameter(Mandatory=$true)]
	[string]$ResourceGroupName,
	[Parameter(Mandatory=$true)]
	[string]$CustomHostName
)

Set-AzWebApp -Name $WebAppName -ResourceGroupName $ResourceGroupName -HostNames @("$CustomHostName","$WebAppName.azurewebsites.net")
