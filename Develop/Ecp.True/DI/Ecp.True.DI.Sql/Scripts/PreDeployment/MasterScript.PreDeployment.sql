/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DECLARE @ServerName VARCHAR(90);

IF ('$(serverName)' <> 'null')
BEGIN
	SET @ServerName = '$(serverName)'
	PRINT '$(serverName) is the servername'
END 
ELSE
BEGIN
	SET @ServerName = (SELECT @@SERVERNAME)
	PRINT @ServerName + ' is the servername'
END



/*----------------------------- CALLING HandleNodeOrderFix ----------------------------------------
	This script is handle order fix.
*/
	:r .\OneTimerScripts\HandleNodeOrderFix.sql



/*----------------------------- CALLING UnusedObjectCleanup ----------------------------------------
	This script cleans up any leftover objects (tables/view/procs) after they got renamed or no longer used.
*/
IF (@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat')
BEGIN
	:r .\UnusedObjectCleanup.sql
END


/*----------------------------- CALLING TableSchemaUpdate ----------------------------------------
	This script updates the table structure if any.
*/
IF (@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat')
BEGIN
	:r .\TableSchemaUpdate.sql
END



/*----------------------------- CALLING MasterTableDataUpdate ----------------------------------------
	This script updates the seed data of master tables if any.
*/
IF (@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat')
BEGIN
	:r .\MasterTableDataUpdate.sql
END




/*------------------------------ CALLING OneTimerScripts Specific to Environment ----------------------
	These scripts will run one time and regiter itself into ControlScript table.
*/



IF (@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat')
BEGIN
	:r .\OneTimerScripts\HomologationObjectTypeCleanup.sql
END



IF (@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat')
BEGIN
:r .\OneTimerScripts\DeleteFeatureRoleRow.sql
END



IF (@ServerName = 'sq-aeu-ecp-dev')
BEGIN
	:r .\OneTimerScripts\DeleteCategoryRow.sql
END



IF (@ServerName = 'sq-aeu-ecp-dev')
BEGIN
	:r .\OneTimerScripts\DeleteFeatureRow.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-qat' OR @ServerName = 'sq-aeu-ecp-tst')
BEGIN
	:r .\OneTimerScripts\DropHomologationVersionTable.sql
END



IF (@ServerName = 'sq-aeu-ecp-dev')
BEGIN
	:r .\OneTimerScripts\TempDacpacIssueFix.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-qat' OR @ServerName = 'sq-aeu-ecp-tst')
BEGIN
	:r .\OneTimerScripts\DropHomologationVersionTable.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-qat' OR @ServerName = 'sq-aeu-ecp-tst')
BEGIN
	:r .\OneTimerScripts\MigrateHomologationGroupTypeToCategoryElement.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev')
BEGIN
	:r .\OneTimerScripts\DropAlgorithmListTable.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat')
BEGIN
	:r .\OneTimerScripts\MeasurementUnitMigration.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat')
BEGIN
	:r .\OneTimerScripts\OffchainMeasurementUnitMigration.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev')
BEGIN
	:r .\OneTimerScripts\DeleteExceptionsFeatureRole.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev')
BEGIN
	:r .\OneTimerScripts\DeleteNodeApprovalFlowSettingsMenu.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev')
BEGIN
	:r .\OneTimerScripts\RestoreOwnershipNodeStatus.sql
END



IF(@ServerName = 'sq-aeu-ecp-qat')
BEGIN
	:r .\OneTimerScripts\MigrateOwnerIdToElementId.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat')
BEGIN
	:r .\OneTimerScripts\MigrateProductTypeToCategoryElement.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst')
BEGIN
	:r .\OneTimerScripts\DeleteFeatureRow2.sql
END


IF(@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat')
BEGIN
	:r .\OneTimerScripts\MigrateFileRegistrationTable.sql
END



IF(@ServerName = 'sq-aeu-ecp-qat' OR @ServerName = 'mi-asc-ecp-qas-mainsqlmiqas.57a2d062a971.database.windows.net')
BEGIN
	:r .\OneTimerScripts\RenameExistingOwnerOtros.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst')
BEGIN
	:r .\OneTimerScripts\DropColorTable.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat')
BEGIN
	:r .\OneTimerScripts\DecimalChange.sql
END



IF(@ServerName = 'sq-aeu-ecp-dev')
BEGIN
	:r .\OneTimerScripts\DeleteFeatureRowDev.sql
END



--IF(@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'mi-asc-ecp-dev-mainsqlmidev.c105ff79c574.database.windows.net')
--BEGIN
:r .\OneTimerScripts\ModifyFeatureTablesAll.sql
--END



IF(@ServerName = 'sq-aeu-ecp-dev' OR @ServerName = 'sq-aeu-ecp-tst')
BEGIN
	:r .\OneTimerScripts\ColumnRenameCommercialIdToOwner1IdForContract.sql
END



/*----------------------------- CALLING ColumnRenameOwnerIdToOwner1IdForEvent ----------------------------------------
	This script is to ename OwnerId to Owner1Id in Event Table
	Since this table is already existing in Production, it should run on all the environments.
*/
	:r .\OneTimerScripts\ColumnRenameOwnerIdToOwner1IdForEvent.sql



/*----------------------------- CALLING DropModelEvaluationView ----------------------------------------
	This script is to drop ModelEvaluationProperty view
	Since this view is no longer being used by 10.10.03 report
*/
	:r .\OneTimerScripts\DropModelEvaluationView.sql

/*----------------------------- CALLING ColumnRenameInOperativeNodeRelationshipWithOwnership ----------------------------------------
	This script will rename columns in operativenoderelationshipwithownership table
*/
IF (  @ServerName = 'sq-aeu-ecp-tst' OR @ServerName = 'sq-aeu-ecp-qat' 
   OR @ServerName = 'mi-asc-ecp-qas-mainsqlmiqas.57a2d062a971.database.windows.net'
   OR @ServerName = 'mi-asc-ecp-uat-mainsqlmiuat.public.cb2315d1702c.database.windows.net')
BEGIN
	:r .\OneTimerScripts\ColumnRenameInOperativeNodeRelationshipWithOwnership.sql
END



/*----------------------------- CALLING DropRuleTables ----------------------------------------
	This script is to drop Rule tables (Rule, RuleName, RuleType)
*/
	:r .\OneTimerScripts\DropRuleTables.sql



/*----------------------------- CALLING MigrateInventoryToInventorProduct ----------------------------------------
	This script is to migrate the data from Inventory table to InventoryProduct table.
	It should run on all the environments.
*/
	:r .\OneTimerScripts\MigrateInventoryToInventoryProductPre.sql



/*----------------------------- CALLING ColumnRenameAuditLogTable ----------------------------------------
	This script is to migrate values from NodeCode, StoreLocationCode columns, PK to Identity
*/
	:r .\OneTimerScripts\ColumnRenameAuditLogTable.sql

/*----------------------------- CALLING ColumnRenameOperatorInMovement ----------------------------------------
	This script is to migrate values from Operator to OperatorId
*/
	:r .\OneTimerScripts\ColumnRenameOperatorToOperatorIdForMovement.sql

/*----------------------------- CALLING ColumnRenameOperatorInInventory ----------------------------------------
	This script is to migrate values from Operator to OperatorId
*/
	:r .\OneTimerScripts\ColumnRenameOperatorToOperatorIdForInventory.sql



/*----------------------------- CALLING PreMigrateSourceSystemAndSystemNameMovAndInvPrd ----------------------------------------
	This script is to migrate values for sourcesystem and systemname in Movement and InventoryProduct
*/
	IF (@ServerName <> 'mi-asc-ecp-dev-mainsqlmidev.c105ff79c574.database.windows.net')
	BEGIN
		:r .\OneTimerScripts\PreMigrateSourceSystemAndSystemNameMovAndInvPrd.sql
	END


/*----------------------------- CALLING PostMigrateSystemNamePendingTransaction ----------------------------------------
	This script is migrate SystemName with Id in PendingTransaction Table one time.
*/
	:r .\OneTimerScripts\PreMigrateSystemNamePendingTransaction.sql


/*----------------------------- CALLING DropMovementDetailsWithOwnerView ----------------------------------------
	This script is to drop movement, inventory details with owner view
*/
	:r .\OneTimerScripts\DropMovementInventoryDetailsWithOwnerViews.sql


/*----------------------------- CALLING DropBackupMovementDetailsWithOwnerView ----------------------------------------
	This script is to drop backup movement details with owner view
*/
	:r .\OneTimerScripts\DropBackupMovementDetailsWithOwnerView.sql

/*----------------------------- CALLING DropMovementInventoryAttributeQualityWithoutOwnerViews ----------------------------------------
	This script is to drop movement, inventory, attribute, quality, backup details without owner kpi with owner views
*/
	:r .\OneTimerScripts\DropMovementInventoryAttributeQualityBackupWithoutKPIWithOwner.sql

/*----------------------------- CALLING DropAttributeQualityMovementsByProductWithOwnerViews ----------------------------------------
	This script is to drop , attribute, quality, MovementsByProduct with owner views
*/
	:r .\OneTimerScripts\DropAttributeQualityMovementsByProductWithOwnerViews.sql

/*----------------------------- CALLING DropBalancePerNodeandOfficialpernodeTables ----------------------------------------
	This script is to drop Balance Per Node and Official Balance per node Tables
*/
	:r .\OneTimerScripts\DropBalancePerNodeandOfficialpernodeTables.sql

/*----------------------------- CALLING DropBeforeCutoffTables ----------------------------------------
	This script is to drop Before Cutoff Tables
*/
	:r .\OneTimerScripts\DropBeforeCutoffTables.sql

/*----------------------------- CALLING DropNonSonBeforeCutoffTables ----------------------------------------
	This script is to drop NonSon Before Cutoff Tables
*/
	:r .\OneTimerScripts\DropNonSonBeforeCutoffTables.sql


/*----------------------------- CALLING DropBalanceControlReportHeaderTemplateViews ----------------------------------------
	This script is to BalnceControl , ReportHeader, ReportTemplate views
*/
	:r .\OneTimerScripts\DropBalanceControlReportHeaderTemplateViews.sql


/*----------------------------- CALLING DeleteAttributeUnitCategoryAndRemoveDupElements ----------------------------------------
	This script is to delete attribute unit category and remove duplicate element between category 6 and 21.
*/
	:r .\OneTimerScripts\DeleteAttributeUnitCategoryAndRemoveDupElements.sql
	

/*----------------------------- CALLING PreMigrateUnitsPendingTransaction ----------------------------------------
	This script is to update Units column in prending transaction to get INT values from CategoryElement
*/
	:r .\OneTimerScripts\PreMigrateUnitsPendingTransaction.sql


/*----------------------------- CALLING PreMigrateUnitsPendingTransaction ----------------------------------------
	This script is to update foreign keys data which are not present in CategoryElement 
*/
	:r .\OneTimerScripts\PreMigrateUpdateForeignKey.sql


/*----------------------------- CALLING CorrectUnbalanceCommentAudits ----------------------------------------
	This script is to correct UnbalanceComment Audits (Change Entity from Unbalance to UnbalanceComment)
*/
	:r .\OneTimerScripts\CorrectUnbalanceCommentAudits.sql


/*----------------------------- CALLING MigrateBlockchainStatus ----------------------------------------
	This script is to update Blockchain status (Value from 0 to 1 and vice versa)
*/
	:r .\OneTimerScripts\MigrateBlockchainStatus.sql

/*----------------------------- CALLING MigrateRetryCountForNodeAndMovement ----------------------------------------
This script is to migrate retry count pending synchronization records for node and movement in production
*/
IF (@ServerName = 'mi-asc-ecp-prd-mainsqlmiprd.3835c682cbdd.database.windows.net')
	BEGIN
	:r .\OneTimerScripts\MigrateRetryCountForNodeAndMovement.sql
END