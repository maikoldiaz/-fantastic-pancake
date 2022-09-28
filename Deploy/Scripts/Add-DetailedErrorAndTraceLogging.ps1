param(
 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory=$true)]
 [string]
 $resourceName
 )

 Write-Output "Adding the detailed diagnostics for the resource $resourceName."

Set-AzWebApp -Name $resourceName -ResourceGroupName $resourceGroupName -DetailedErrorLoggingEnabled $true -HttpLoggingEnabled $true -RequestTracingEnabled $true