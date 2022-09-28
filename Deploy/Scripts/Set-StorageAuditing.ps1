param(
 [Parameter(Mandatory=$True)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory=$True)]
 [string]
 $storageAccountName
 )


#******************************************************************************
# Script body
# Execution begins here
#******************************************************************************

$storageAccount = Get-AzStorageAccount -ResourceGroupName $resourceGroupName -Name $storageAccountName -ErrorAction SilentlyContinue
Set-AzStorageServiceLoggingProperty -ServiceType 'Blob' -LoggingOperations 'All' -Context $storageAccount.Context -RetentionDays '365' -PassThru

Set-AzStorageAccount -ResourceGroupName $resourceGroupName -Name $storageAccountName -EnableHttpsTrafficOnly $true

Write-Output "Storage Account only Https Enabled."

#Set-AzSqlDatabaseAudit -ResourceGroupName $resourceGroupName -ServerName $serverName -DatabaseName $databaseName -LogAnalyticsTargetState Enabled -WorkspaceResourceId "/subscriptions/$subscriptionID/resourceGroups/$resourceGroupName/providers/Microsoft.OperationalInsights/workspaces/$logAnalyticsWorkspaceName"

#Write-Output "SQL DATABASE AUDIT HAS BEEN SET"