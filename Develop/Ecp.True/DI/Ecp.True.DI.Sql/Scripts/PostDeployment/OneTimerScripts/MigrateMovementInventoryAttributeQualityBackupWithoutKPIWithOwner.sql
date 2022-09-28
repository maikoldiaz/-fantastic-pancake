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


IF EXISTS (Select 'X' from sys.schemas Where name = 'Audit')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='e69ce0db-ce4e-4112-8aa9-8292fb2b4481' AND Status = 1)
	BEGIN
		BEGIN TRY
			EXEC [Admin].[usp_SaveMovementDetails] -1
			EXEC [Admin].[usp_SaveAttributeDetails] -1
			EXEC [Admin].[usp_SaveInventoryDetails] -1
			EXEC [Admin].[usp_SaveQualityDetails] -1
			EXEC [Admin].[usp_SaveBackupMovementDetails] -1
			EXEC [Admin].[usp_SaveKPIDataByCategoryElementNodeWithOwnership] -1
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('e69ce0db-ce4e-4112-8aa9-8292fb2b4481', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('e69ce0db-ce4e-4112-8aa9-8292fb2b4481', 0, 'POST');
		END CATCH
	END
END