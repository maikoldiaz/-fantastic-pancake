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

-- Old GUID: 0482a534-79ca-4115-a4d2-53576aed3556
-- Old GUID: CB6CA147-C3FA-4490-B43B-75342E717466


IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='24F8B3BB-D351-4F24-90BF-1276D047C649' AND Status = 1)
	BEGIN
		BEGIN TRY
			TRUNCATE TABLE [Admin].BackupMovementDetailsWithoutOwner
			TRUNCATE TABLE [Admin].BackupMovementDetailsWithOwner
			EXEC [Admin].[usp_SaveBackupMovementDetails] -1
			EXEC [Admin].[usp_SaveBackupMovementDetailsWithOwner] -1
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('24F8B3BB-D351-4F24-90BF-1276D047C649', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('24F8B3BB-D351-4F24-90BF-1276D047C649', 0, 'POST');
		END CATCH
	END
END