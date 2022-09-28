param(
 [string]  $resourceGroupName,
 [Parameter(Mandatory=$False)]
 [string]  $outputKeySuffix,
 [Parameter(Mandatory=$False)]
 [string]  $outputKeyName,
 [string]  $deploymentName
)

Start-Sleep -s 5
if($deploymentName)
{
    $lastDeployment = Get-AzResourceGroupDeployment -ResourceGroupName $resourceGroupName -Name $deploymentName
}else{
    $lastDeployment = Get-AzResourceGroupDeployment -ResourceGroupName $resourceGroupName | Sort-Object Timestamp -Descending | Where-Object {$_.DeploymentName -Like "deploy-*"} | Select-Object -First 1
}
if(!$lastDeployment) {
    throw "Deployment could not be found for Resource Group '$resourceGroupName'."
}

if(!$lastDeployment.Outputs) {
    throw "No output parameters could be found for the last deployment of Resource Group '$resourceGroupName'."
}

foreach ($key in $lastDeployment.Outputs.Keys){
    $type = $lastDeployment.Outputs.Item($key).Type
    $value = $lastDeployment.Outputs.Item($key).Value

	if($outputKeyName -eq $key){
		$key = $key+ $outputKeySuffix
	}

    if ($type -eq "secureString") {
        Write-Output "##vso[task.setvariable variable=$key;issecret=true;]$value" #pragma: allowlist secret
    }
    else {
        Write-Output "##vso[task.setvariable variable=$key;]$value"
    }
}