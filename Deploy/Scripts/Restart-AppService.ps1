param(
    [Parameter(Mandatory = $true)]
    [string]$action,
	[Parameter(Mandatory = $true)]
    [string]$resourceGroupName,
	[Parameter(Mandatory = $true)]
    [string]$appName

)

switch($action) {
    "STOP" {
        Stop-AzWebApp -ResourceGroupName $resourceGroupName -Name $appName -ErrorAction SilentlyContinue
    }
    "START" {
        Start-AzWebApp -ResourceGroupName $resourceGroupName -Name $appName -ErrorAction SilentlyContinue
    }
    "RESTART" {
        Stop-AzWebApp -ResourceGroupName $resourceGroupName -Name $appName -ErrorAction SilentlyContinue
        Start-Sleep -s 10
        Start-AzWebApp -ResourceGroupName $resourceGroupName -Name $appName -ErrorAction SilentlyContinue
    }
 }
