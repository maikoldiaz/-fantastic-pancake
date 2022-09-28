param(
    [Parameter(Mandatory=$true)]
    [string]$resourceGroupName,
    [Parameter(Mandatory=$true)]
    [string]$dataFactoryName
)

Get-AzureRmDataFactory -ResourceGroupName $resourceGroupName -Name $dataFactoryName -ErrorAction SilentlyContinue -ErrorVariable ProcessError;
if($ProcessError) {
    Write-Output "Data factory doesn't exist"
    Write-Output "##vso[task.setvariable variable=createAdf]true"
} else {
    Write-Output "Data factory exists"
    Write-Output "##vso[task.setvariable variable=createAdf]false"
}
