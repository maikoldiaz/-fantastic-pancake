##Parameters
param(
    [Parameter(Mandatory=$true)]
	[string]$ResourceGroupName,
    [Parameter(Mandatory=$true)]
	[string]$dataFactoryName
)
Get-AzureRmDataFactory -ResourceGroupName $resourceGroupName -Name $dataFactoryName -ErrorAction SilentlyContinue -ErrorVariable ProcessError;
if($ProcessError) {
    Write-Output "Data factory doesn't exist"
} else {
    Write-Output "Deleting Data Factory $dataFactoryName..."
    Remove-AzureRmResource -ResourceGroupName  $ResourceGroupName -ResourceName $dataFactoryName -ResourceType "Microsoft.DataFactory/factories"
}
