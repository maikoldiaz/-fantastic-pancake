param(
	[Parameter(Mandatory = $true)]
    [string]$resourceGroupName,
	[Parameter(Mandatory = $true)]
    [string]$name

)

Restart-AzSignalR -ResourceGroupName $resourceGroupName -Name $name -PassThru