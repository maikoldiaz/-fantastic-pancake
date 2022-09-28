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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='99a49191-bf9b-4c74-b915-4bc9a29b4ec9' AND Status = 1)
	BEGIN
		BEGIN TRY
			DELETE FROM [Admin].[FeatureRole] WHERE [FeatureId] = '19';
			DELETE FROM [Admin].[FeatureRole] WHERE [FeatureId] = '20';
			DELETE FROM [Admin].[Feature] WHERE [FeatureId] = '19';
			DELETE FROM [Admin].[Feature] WHERE [FeatureId] = '20';		
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('99a49191-bf9b-4c74-b915-4bc9a29b4ec9', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('99a49191-bf9b-4c74-b915-4bc9a29b4ec9', 0);
		END CATCH
	END
END