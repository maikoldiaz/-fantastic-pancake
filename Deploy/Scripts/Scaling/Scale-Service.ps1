param(
	[Parameter(Mandatory = $true)]
	[string]$resourceGroupName,

	[Parameter(Mandatory = $true)]
	[string]$resourceName,

	[Parameter(Mandatory = $false)]
	[string]$type,

	[Parameter(Mandatory = $false)]
	[string]$tier,

	[Parameter(Mandatory = $false)]
	[string]$workerSize,

	[Parameter(Mandatory = $false)]
	[string]$numberOfworkers
)

if($type -eq "webapp")
{
	$app = Get-AzWebApp -ResourceGroupName $resourceGroupName -Name $resourceName

	# Modify the NumberOfWorkers setting to the desired value.
	$app.SiteConfig.NumberOfWorkers = $numberOfworkers

	# Post updated app back to azure
	Set-AzWebApp $app
}

if($type -eq "appserviceplan")
{
	Set-AzAppServicePlan -Name $resourceName -ResourceGroupName $resourceGroupName -Tier $tier -WorkerSize $workerSize -PerSiteScaling $true -NumberofWorkers $numberOfworkers
}