param(
 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory=$true)]
 [string]
 $storageAccountName,

 [Parameter(Mandatory=$true)]
 [string]
 $retentionDays
 )

 Write-Output "Adding the retention policy for the storage account."
 $storageAccount = Get-AzStorageAccount -ResourceGroupName $resourceGroupName -Name $storageAccountName -ErrorAction SilentlyContinue
 if ($storageAccount)
 {
    $storageContext = $storageAccount.Context
    $serviceTypesForLoggingProperty = @('Blob', 'Queue', 'Table')
    ForEach($serviceType in $serviceTypesForLoggingProperty)
    {
        Set-AzStorageServiceLoggingProperty -ServiceType $serviceType -LoggingOperations 'All' -Context $storageContext -RetentionDays $retentionDays -PassThru
    }

    $serviceTypesForMetricsProperty = @('Blob', 'Queue', 'Table', 'File')
    ForEach($serviceType in $serviceTypesForMetricsProperty)
    {
        Set-AzStorageServiceMetricsProperty -MetricsType 'Hour' -ServiceType $serviceType -Context $storageContext -MetricsLevel 'ServiceAndApi' -RetentionDays $retentionDays -PassThru
    }
}
else {
    Write-Output "The storage account $storageAccountName was not found in the resource Group $resourceGroupName"
}