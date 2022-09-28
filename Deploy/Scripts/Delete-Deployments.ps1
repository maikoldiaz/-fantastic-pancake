param(
    [Parameter(Mandatory=$true)]
	[string]$resourceGroupName,
    [Parameter(Mandatory=$true)]
	[string]$deploymentsLimit
	)


    $deployments = Get-AzResourceGroupDeployment $resourceGroupName
    $count = $deployments.Count
    $limit = $deploymentsLimit
    $deployments | Where-Object {$count -ge $limit} | ForEach-Object {
      Remove-AzResourceGroupDeployment -Name $_.DeploymentName -ResourceGroupName $resourceGroupName
      $count--
	  Write-Output "Deployment Deleted :" $_.DeploymentName - "Current Count:" $count
     }