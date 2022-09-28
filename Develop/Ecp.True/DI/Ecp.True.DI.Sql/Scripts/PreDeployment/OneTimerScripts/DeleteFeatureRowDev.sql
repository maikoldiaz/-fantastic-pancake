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


-- Deleting nodeGraphicalConnection
IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='14b4e906-bac4-41d3-8df5-f1cfd2c3e4b8' AND Status = 1)
	BEGIN
		BEGIN TRY
			DELETE FROM [Admin].[FeatureRole] WHERE [FeatureId] = 22;
			DELETE FROM [Admin].[Feature] WHERE [FeatureId] = 22;
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('14b4e906-bac4-41d3-8df5-f1cfd2c3e4b8', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('14b4e906-bac4-41d3-8df5-f1cfd2c3e4b8', 0);
		END CATCH
	END
END