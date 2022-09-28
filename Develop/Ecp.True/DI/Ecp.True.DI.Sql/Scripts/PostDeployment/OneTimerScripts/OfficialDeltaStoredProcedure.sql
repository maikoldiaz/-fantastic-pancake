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
IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='4D000BED-973B-4E8B-98A5-2AEE9FA8AABF' AND Status = 1)
	BEGIN
		BEGIN TRY
			EXEC [Report].[usp_SaveOfficialDeltaBalance] '1900-12-31','1900-12-31'
			EXEC [Report].[usp_SaveOfficialDeltaInventoryDetails] '1900-12-31','1900-12-31'
			EXEC [Report].[usp_SaveOfficialDeltaMovementDetails] '1900-12-31','1900-12-31'
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('4D000BED-973B-4E8B-98A5-2AEE9FA8AABF', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('4D000BED-973B-4E8B-98A5-2AEE9FA8AABF', 0, 'POST');
		END CATCH
	END
END