param(
 [Parameter(Mandatory=$True)]
 [string]
 $resourceGroupName,

 [Parameter(Mandatory=$True)]
 [string]
 $sharedResourceGroupName,

 [Parameter(Mandatory=$True)]
 [string]
 $serverName,

 [Parameter(Mandatory=$True)]
 [string]
 $subscriptionID,

 [Parameter(Mandatory=$True)]
 [string]
 $logAnalyticsResourceId,

 [Parameter(Mandatory=$True)]
 [string]
 $databaseName,

 [Parameter(Mandatory=$True)]
 [string]
 $securityGroup,

 [Parameter(Mandatory=$True)]
 [string]
 $storageAccountName
 )


#******************************************************************************
# Script body
# Execution begins here
#******************************************************************************
Set-AzSqlServerAudit -ResourceGroupName $sharedResourceGroupName -ServerName $serverName -LogAnalyticsTargetState Enabled -WorkspaceResourceId $logAnalyticsResourceId

Write-Output "SQL SERVER AUDIT HAS BEEN SET - Log Analytics"

Set-AzSqlServerAudit -ResourceGroupName $sharedResourceGroupName -ServerName $serverName -StorageAccountResourceId "/subscriptions/$subscriptionID/resourceGroups/$resourceGroupName/providers/Microsoft.Storage/storageAccounts/$storageAccountName" -BlobStorageTargetState 'Enabled' -RetentionInDays 365

Write-Output "SQL SERVER AUDIT HAS BEEN SET - Storage Account"

Update-AzSqlDatabaseAdvancedThreatProtectionSetting -ResourceGroupName $sharedResourceGroupName -ServerName $serverName -StorageAccountName $storageAccountName -EmailAdmins $true -ExcludedDetectionType 'None' -DatabaseName $databaseName

Write-Output "SQL DB Threat Detection Enabled."

Update-AzSqlServerAdvancedThreatProtectionSetting -ResourceGroupName $sharedResourceGroupName -ServerName $serverName -StorageAccountName $storageAccountName -EmailAdmins $true -ExcludedDetectionType 'None'

Write-Output "SQL SERVER Threat Detection Enabled."

Update-AzSqlDatabaseVulnerabilityAssessmentSetting -DatabaseName $databaseName -ServerName $serverName -ResourceGroupName $sharedResourceGroupName -StorageAccountName $storageAccountName -EmailAdmins $true

Write-Output "SQL DB Vulnerability Assessment Enabled."

Update-AzSqlServerVulnerabilityAssessmentSetting -ResourceGroupName $sharedResourceGroupName -ServerName $serverName -StorageAccountName $storageAccountName -EmailAdmins $true

Write-Output "SQL SERVER Vulnerability Assessment Enabled."

Set-AzSqlServerActiveDirectoryAdministrator -ResourceGroupName $sharedResourceGroupName -ServerName $serverName -DisplayName $securityGroup

Write-Output "SQL Server Admin Enabled."

#Set-AzSqlDatabaseAudit -ResourceGroupName $resourceGroupName -ServerName $serverName -DatabaseName $databaseName -LogAnalyticsTargetState Enabled -WorkspaceResourceId "/subscriptions/$subscriptionID/resourceGroups/$resourceGroupName/providers/Microsoft.OperationalInsights/workspaces/$logAnalyticsWorkspaceName"

#Write-Output "SQL DATABASE AUDIT HAS BEEN SET"