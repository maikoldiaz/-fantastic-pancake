/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Current Bogota Time
DECLARE @CurrentTime DATETIME;
SET @CurrentTime = (SELECT GETDATE() AT TIME ZONE 'UTC' AT TIME ZONE 'SA Pacific Standard Time');


:r .\VersionSeed.sql


:r .\SystemTypeSeed.sql


:r .\MessageTypeSeed.sql


:r .\HomologationObjectTypeSeed.sql


:r .\RegisterFileActionTypeSeed.sql


:r .\FileUploadStateSeed.sql


:r .\CategorySeed.sql


:r .\IconSeed.sql


:r .\CategoryElementSeed.sql


:r .\ScenarioSeed.sql


:r .\FeatureSeed.sql


:r .\RoleSeed.sql


:r .\VariableTypeSeed.sql


:r .\FeatureRoleSeed.sql


:r .\StatusTypeSeed.sql


:r .\TicketTypeSeed.sql


:r .\ControlTypeSeed.sql


:r .\AlgorithmSeed.sql


:r .\AuditStatusSeed.sql


:r .\AnalyticsAuditStatusSeed.sql


:r .\OwnershipNodeStatusTypeSeed.sql


:r .\ApprovalRuleSeed.sql


:r .\ReportTemplateConfigurationSeed.sql


:r .\NodeStatusIconUrlSeed.sql


:r .\OriginTypeSeed.sql


:r .\ScenarioTypeSeed.sql


:r .\NodeStateTypeSeed.sql


:r .\OfficialDeltaMessageTypeSeed.sql


:r .\ReportTypeSeed.sql


--This script is to insert seed data into KPIRelationship table.
:r .\KPIRelationshipSeed.sql


-- This is a special script which will always run to migrate any user entered manual CategoryElement which becomes the part of seed script..
:r .\CategoryElementMigration.sql

-- This script is to insert seed dates into DimDate table.
:r .\DimDateSeed.sql

-- This script is to update exportation null.
:r .\NodeIsExportation.sql

-- ########################### Calling One Timer Scripts after Post Deployment Scripts has run ###########################

DECLARE @ServerName VARCHAR(90);

IF ('$(serverName)' <> 'null')
BEGIN
	SET @ServerName = '$(serverName)'
END 
ELSE
BEGIN
	SET @ServerName = (SELECT @@SERVERNAME)
END



/*----------------------------- CALLING SetLogisticCenterIdInStorageLocation ----------------------------------------
	This script Sets the newly added LogisticCenterId column value to make relationship with StorageLocation and LogisticCenter.
	It should run on all the environments.
*/
:r .\OneTimerScripts\SetLogisticCenterIdInStorageLocation.sql



/*----------------------------- CALLING SetSegmentIdValueInFileRegistration ----------------------------------------
	This scripts does two things -->
		1. Migrating all the existing [SystemTypeId] value from 2 to 1.
		2. Setting all the existing [SegmentId] to value of 10 which is CategoryElemnt for Transport.
	It should not run in Production.
*/
IF (@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat')
BEGIN
	:r .\OneTimerScripts\SetSegmentIdValueInFileRegistration.sql
END


/*----------------------------- CALLING CorrectOwnerIdInOwnerTable ----------------------------------------

*/
IF (@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst')
BEGIN
	:r .\OneTimerScripts\CorrectOwnerIdInOwnerTable.sql
END


/*----------------------------- CALLING MigrateOwnershipNodeOwnershipStatusId ----------------------------------------

*/
IF (@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat')
BEGIN
	:r .\OneTimerScripts\MigrateOwnershipNodeOwnershipStatusId.sql
END


/*----------------------------- CALLING OwnershipNodeStatusTypeMigration ----------------------------------------

*/
IF(@ServerName = 'sq-aeu-ecp-dev')
BEGIN
	:r .\OneTimerScripts\OwnershipNodeStatusTypeMigration.sql
END



/*----------------------------- CALLING SeedDataToBlockchainStatusColumn ----------------------------------------

*/
IF(@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat')
BEGIN
	:r .\OneTimerScripts\SeedDataToBlockchainStatusColumn.sql
END



/*----------------------------- CALLING MigrateOwnerIdForOtros ----------------------------------------
	This script is for migrating OwnerIds for OTROS in all related tables.
	It should run only in qat and qas.
*/
IF (@ServerName = 'sq-aeu-ecp-qat' OR @ServerName = 'mi-asc-ecp-qas-mainsqlmiqas.57a2d062a971.database.windows.net')
BEGIN
	:r .\OneTimerScripts\MigrateOwnerIdForOtros.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst')
BEGIN
	:r .\OneTimerScripts\BackupContractDataAndTempDelete.sql
END



/*----------------------------- CALLING BackupEventDataAndTempDelete ----------------------------------------
	This script is to migrate OwnerId to Owner1Id in Admin.Event Tables
	It should run on all the environments.
*/
:r .\OneTimerScripts\BackupEventDataAndTempDelete.sql



IF (  @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat' 
   OR @ServerName = 'mi-asc-ecp-qas-mainsqlmiqas.57a2d062a971.database.windows.net'
   OR @ServerName = 'mi-asc-ecp-uat-mainsqlmiuat.public.cb2315d1702c.database.windows.net')
BEGIN
	:r .\OneTimerScripts\BackupOperativeNodeRelationshipWithOwnershipAndTempDelete.sql
END



/*----------------------------- CALLING MigrateInventoryToInventorProduct ----------------------------------------
	This script is to migrate the data from Inventory table to InventoryProduct table.
	It should run on all the environments.
*/
	:r .\OneTimerScripts\MigrateInventoryToInventoryProductPost.sql


/*----------------------------- CALLING DropRegistrationProgress sctipt ----------------------------------------
	This script is to drop RegistrationProgress table.
*/
	:r .\OneTimerScripts\DropRegistrationProgress.sql


/*----------------------------- CALLING DropOwnershipAnalytics sctipt ----------------------------------------
	This script is to drop DropOwnershipAnalytics table.
*/
	:r .\OneTimerScripts\DropOwnershipAnalytics.sql



/*----------------------------- CALLING RenameInventoryTable ----------------------------------------
	This script is to rename Inventory table to Obsolete_Inventory.
	It should run on all the environments.
*/
	:r .\OneTimerScripts\RenameInventoryTable.sql

/*----------------------------- CALLING BackupAuditLogDataAndTempDelete ----------------------------------------
	This script is to migrate backup data of NodeCode, StoreLocationCode, PK column data to insert into [Identity]
	column. It should run on all the environments.
*/
	:r .\OneTimerScripts\BackupAuditLogDataAndTempDelete.sql


/*----------------------------- CALLING SetAutogrowthValue ----------------------------------------
	This script is to set autogrowth of database to 50240 MB.
*/
	:r .\OneTimerScripts\SetAutogrowthValue.sql


/*----------------------------- CALLING SetScenarioIdInMovementAndInventory ----------------------------------------
	This script is to initialize scenario id to 1 in existing records of Movement and InventoryProduct table.
	It should run on all the environments.
*/
	:r .\OneTimerScripts\SetScenarioIdInMovementAndInventory.sql

/*----------------------------- CALLING BackupOperatorInMovement ----------------------------------------
	This script is to migrate operator to operatorId.
	It should run on all the environments.
*/

	:r .\OneTimerScripts\BackupOperatorInMovement.sql

/*----------------------------- CALLING BackupOperatorInInventory ----------------------------------------
	This script is to migrate operator to operatorId.
	It should run on all the environments.
*/

	:r .\OneTimerScripts\BackupOperatorInInventory.sql

/*----------------------------- CALLING BackupOperatorInInventory ----------------------------------------
	This script is to drop Reversal table in Admin schema.
	It should run on all the environments.
*/

	:r .\OneTimerScripts\DropReversalTable.sql

/*----------------------------- CALLING BackupOperatorInInventoryInDev ----------------------------------------
	This script is to drop Reversal table in Admin schema in dev.
	It should run on all the environments.
*/
IF (@ServerName = 'mi-asc-ecp-dev-mainsqlmidev.c105ff79c574.database.windows.net')
BEGIN
	:r .\OneTimerScripts\BackupOperatorInInventoryInDev.sql
END


/*----------------------------- CALLING MigrateAttributeIdValueAttributeUnit ----------------------------------------
	This script is to migrate attributeid and valueattributeunit in Attribute Table
*/
	:r .\OneTimerScripts\MigrateAttributeIdValueAttributeUnit.sql


/*----------------------------- CALLING MigrateMovementClassification ----------------------------------------
	This script is to migrate blank or cls Classification value to Movimiento
*/
	:r .\OneTimerScripts\MigrateMovementClassification.sql


/*----------------------------- CALLING InsertModelEvaluationValues ----------------------------------------
	This script is to insert values in model evaluation table only one time.
*/
	:r .\OneTimerScripts\InsertModelEvaluationValues.sql

/*----------------------------- CALLING PostMigrateSourceSystemAndSystemNameMovAndInvPrd ----------------------------------------
	This script is migrate SystemName and SourceSystem with Id in Movement and InventoryProduct Table one time.
*/
	IF (@ServerName <> 'mi-asc-ecp-dev-mainsqlmidev.c105ff79c574.database.windows.net')
	BEGIN
		:r .\OneTimerScripts\PostMigrateSourceSystemAndSystemNameMovAndInvPrd.sql
	END



/*----------------------------- CALLING MigrateMovementDetailsWithOwner ----------------------------------------
	This script is to insert existing movement, inventory details with owner data in the table.
*/
	:r .\OneTimerScripts\MigrateMovementInventoryOwnerViews.sql


/*----------------------------- CALLING MigrateBackupMovementWithOwnerView ----------------------------------------
	This script is to insert existing backup movement details with owner data in the table.
*/
	:r .\OneTimerScripts\MigrateBackupMovementWithOwnerView.sql


/*----------------------------- CALLING MigrateMovementInventoryAttributeQualityBackupWithoutKPIWithOwner ----------------------------------------
	This script is to insert existing movement, attribute, inventory, quality, backup details without owner and kpi with owner data in the table.
*/
	:r .\OneTimerScripts\MigrateMovementInventoryAttributeQualityBackupWithoutKPIWithOwner.sql

/*----------------------------- CALLING MigrateAttributeQualityMovementsByProductViews ----------------------------------------
	This script is to insert existing movement, quality, movementsbyproduct with owner and  movementsbyproductwithoutowner,in the table.
*/
	:r .\OneTimerScripts\MigrateAttributeQualityMovementsByProductViews.sql

/*----------------------------- CALLING MigrateBalanceControlReportHeaderTemplateViews ----------------------------------------
	This script is to BalnceControl , ReportHeader, ReportTemplate views
*/
	:r .\OneTimerScripts\MigrateBalanceControlReportHeaderTemplateViews.sql


/*----------------------------- CALLING DeleteAttributeUnitCategory ----------------------------------------
	This script is to delete attribute unit category
*/
	:r .\OneTimerScripts\DeleteAttributeUnitCategory.sql


/*----------------------------- CALLING DropAdminUnbalanceOffchainAttributeTable ----------------------------------------
	This script is to drop tables Admin.Unbalance and Offchain.Attribute and migrate their data to
	Offchain.Unbalance and Admin.Attribute respectively.
*/
	:r .\OneTimerScripts\DropAdminUnbalanceOffchainAttributeTable.sql



/*----------------------------- CALLING DropAdminUnbalanceOffchainAttributeTable ----------------------------------------
	This script is to truncate backupmovement tables and execute again SPs in dev, uat and qas.
*/
IF (@ServerName = 'mi-asc-ecp-dev-mainsqlmidev.c105ff79c574.database.windows.net' 
OR @ServerName = 'mi-asc-ecp-uat-mainsqlmiuat.cb2315d1702c.database.windows.net' 
OR @ServerName = 'mi-asc-ecp-qas-mainsqlmiqas.57a2d062a971.database.windows.net')
BEGIN
	:r .\OneTimerScripts\TruncateAndExecuteBackupMovementsDevUatQas.sql
END


/*----------------------------- CALLING DropAdminUnbalanceOffchainAttributeTable ----------------------------------------
	This script is to truncate report tables for a bug fix
*/
IF (@ServerName = 'mi-asc-ecp-dev-mainsqlmidev.c105ff79c574.database.windows.net' 
OR @ServerName = 'mi-asc-ecp-uat-mainsqlmiuat.cb2315d1702c.database.windows.net' 
OR @ServerName = 'mi-asc-ecp-qas-mainsqlmiqas.57a2d062a971.database.windows.net')
BEGIN
	:r .\OneTimerScripts\TruncateReportTables.sql
END


/*----------------------------- CALLING TruncateOfficialDeltaReportTables ----------------------------------------
	This script is to truncate official delta report tables.
*/
IF (@ServerName = 'mi-asc-ecp-dev-mainsqlmidev.c105ff79c574.database.windows.net' 
OR @ServerName = 'mi-asc-ecp-uat-mainsqlmiuat.cb2315d1702c.database.windows.net' 
OR @ServerName = 'mi-asc-ecp-qas-mainsqlmiqas.57a2d062a971.database.windows.net')
BEGIN
	:r .\OneTimerScripts\TruncateOfficialDeltaReportTables.sql
END


/*----------------------------- CALLING TruncateOfficialDeltaReportTables ----------------------------------------
	This script is to drop report intermittent table
*/
IF (@ServerName = 'mi-asc-ecp-dev-mainsqlmidev.c105ff79c574.database.windows.net' 
OR @ServerName = 'mi-asc-ecp-uat-mainsqlmiuat.cb2315d1702c.database.windows.net' 
OR @ServerName = 'mi-asc-ecp-qas-mainsqlmiqas.57a2d062a971.database.windows.net')
BEGIN
	:r .\OneTimerScripts\DropReportIntermitttentTables.sql
END


/*----------------------------- CALLING TruncateOfficialDeltaReportTables ----------------------------------------
	This script is to fill data in attribute, quality and balance control report tables.
*/
	:r .\OneTimerScripts\FillAttributeQualityBalanceControlReportTables.sql


/*----------------------------- CALLING DropUnusedReportObjects ----------------------------------------
	This script is to drop objects which are no longer required for reporting.
*/
	:r .\OneTimerScripts\DropUnusedReportObjects.sql

/*----------------------------- CALLING RemigrateCutoffReportBalanceSheetTables ----------------------------------------
	This script is to again migrate report tables used in balance sheet of cutoff report
*/
IF (@ServerName = 'mi-asc-ecp-stg-mainsqlmistg.4218c20078d1.database.windows.net' 
OR @ServerName = 'mi-asc-ecp-prd-mainsqlmiprd.3835c682cbdd.database.windows.net')
BEGIN
	:r .\OneTimerScripts\RemigrateCutoffReportBalanceSheetTables.sql
END


/*----------------------------- CALLING DeleteBeforeCutoffReportData ----------------------------------------
	This script is to delete all existing before cutoff report generated data
*/
	:r .\OneTimerScripts\DeleteBeforeCutoffReportData.sql


/*----------------------------- CALLING DeleteNonSonOwnerReportData ----------------------------------------
	This script is to delete all existing nonson ownership report generated data
*/
	:r .\OneTimerScripts\DeleteNonSonOwnerReportData.sql


/*----------------------------- CALLING RemigrateMovementsByProductWithOwner ----------------------------------------
	This script is to truncate and remigrate movements by product with owner data.
*/
	:r .\OneTimerScripts\RemigrateMovementsByProductWithOwner.sql


/*----------------------------- CALLING SetIsDeletedMovementInventory ----------------------------------------
	This script is to set IsDeleted values in Movement, InventoryProduct tables.
*/	
	:r .\OneTimerScripts\SetIsDeletedMovementInventory.sql


/*----------------------------- CALLING RemigrateOrCleanupToFixIsDeleted ----------------------------------------
	This script is to truncate and remigrate inventory, quality details for without owner, with owner reports
	and truncate existing before cutoff, non-son ownership, official balance loaded reports to fix IsDeleted data.
*/
	:r .\OneTimerScripts\RemigrateOrCleanupToFixIsDeleted.sql


/*----------------------------- CALLING FixScenarioIdThreeSystemUnbalance ----------------------------------------
    This script is to set ticketid, ownershipticketid to null of a particular movement record with scenarioid=3
    and correct it's systemunbalance.
*/
IF (@ServerName = 'mi-asc-ecp-prd-mainsqlmiprd.3835c682cbdd.database.windows.net')
BEGIN
	:r .\OneTimerScripts\FixScenarioIdThreeSystemUnbalance.sql
END


/*----------------------------- CALLING FixBlockchainStatusBlockNumber ----------------------------------------
    This script is to update blockchain statuses and block numbers.
*/
	:r .\OneTimerScripts\FixBlockchainStatusBlockNumber.sql

/*----------------------------- CALLING UpdateDataHistoryContract ----------------------------------------
    This script is to update historical order records.
*/
	:r .\OneTimerScripts\UpdateDataHistoryContract.sql

/*----------------------------- CALLING SetNameValueInSystemTypeTable ----------------------------------------
    This script is to update the name of system type (to 'Pedidos')
*/
	:r .\OneTimerScripts\SetNameValueInSystemTypeTable.sql

/*----------------------------- CALLING UpdateStatusTicket ----------------------------------------
    This script is to update the id status of ticket 
*/
	:r .\OneTimerScripts\UpdateStatusTicket.sql


/*----------------------------- CALLING UpdateReportExecutionAuditoriaRolMenus ----------------------------------------
    This script is to update the id status of ReportExecution for report  Auditoria Rol Menús
*/
	:r .\OneTimerScripts\UpdateReportExecutionAuditoriaRolMenus.sql