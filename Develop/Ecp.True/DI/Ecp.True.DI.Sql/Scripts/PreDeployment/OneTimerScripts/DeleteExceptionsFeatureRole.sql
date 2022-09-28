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

IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='503dd563-e9f3-4506-a6f3-001a8170a9b7' AND Status = 1)
	BEGIN
		BEGIN TRY
			DELETE FROM [Admin].[FeatureRole] WHERE [FeatureRoleId] = 109;
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('503dd563-e9f3-4506-a6f3-001a8170a9b7', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('503dd563-e9f3-4506-a6f3-001a8170a9b7', 0);
		END CATCH
	END
END