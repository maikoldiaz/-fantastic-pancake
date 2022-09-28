/*
 Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='bf1f8faa-5eec-41b6-9cdb-0a653a236e19' AND Status = 1)
	BEGIN
		BEGIN TRY
			TRUNCATE TABLE [Admin].AttributeDetailsWithOwner
			TRUNCATE TABLE [Admin].QualityDetailsWithOwner
			TRUNCATE TABLE [Admin].BackupMovementDetailsWithOwner
			TRUNCATE TABLE [Admin].AttributeDetailsWithoutOwner
			TRUNCATE TABLE [Admin].QualityDetailsWithoutOwner
			TRUNCATE TABLE [Admin].BackupMovementDetailsWithoutOwner
			TRUNCATE TABLE [Admin].BalanceControl

			EXEC [Admin].[usp_SaveAttributeDetailsWithOwner] -1
			EXEC [Admin].[usp_SaveQualityDetailsWithOwner] -1
			EXEC [Admin].[usp_SaveBackupMovementDetailsWithOwner] -1
			EXEC [Admin].[usp_SaveAttributeDetails] -1
			EXEC [Admin].[usp_SaveQualityDetails] -1
			exec [Admin].[usp_SaveBackupMovementDetails] -1
			EXEC [Admin].[usp_SaveBalanceControl] -1

			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('bf1f8faa-5eec-41b6-9cdb-0a653a236e19', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('bf1f8faa-5eec-41b6-9cdb-0a653a236e19', 0, 'POST');
		END CATCH
	END
END