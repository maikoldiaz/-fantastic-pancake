param(
   [string][parameter(Mandatory = $true)]
   $resourceGroupName,

   [string][parameter(Mandatory = $true)]
   $resourceName,

   [string][parameter(Mandatory = $true)]
   $PipelineVariablesKeyName
)

$msiServicePrincipal=(Get-AzDataFactoryV2 -ResourceGroupName $resourceGroupName -Name $resourceName).Identity

$key= $PipelineVariablesKeyName
$value = $msiServicePrincipal.PrincipalId.Guid
Write-Output "##vso[task.setvariable variable=$key;]$value"