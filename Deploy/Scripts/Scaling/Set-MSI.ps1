param
(
    [Parameter(Mandatory = $false)]
    [String]$resourceGroup,

    [Parameter(Mandatory = $false)]
    [String]$resourceName,

    [Parameter(Mandatory = $false)]
    [String]$outputKeyName
)


$webapp= Get-AzWebApp -ResourceGroupName $resourceGroup -Name $resourceName
$data= $webapp.Identity.PrincipalId

##Set Output Variable.
$key="identity"
$key = $key+ $outputKeyName

$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"