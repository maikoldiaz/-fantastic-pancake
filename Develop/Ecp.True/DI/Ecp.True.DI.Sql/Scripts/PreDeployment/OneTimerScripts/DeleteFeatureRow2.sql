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

-- Deleting TransportLogistics
IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='aa5915b7-4789-47e0-98ec-b9e4218bb0e0' AND Status = 1)
	BEGIN
		BEGIN TRY
			DELETE FROM [Admin].[FeatureRole] WHERE [FeatureId] = 22;
			DELETE FROM [Admin].[Feature] WHERE [FeatureId] = 22;
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('aa5915b7-4789-47e0-98ec-b9e4218bb0e0', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('aa5915b7-4789-47e0-98ec-b9e4218bb0e0', 0);
		END CATCH
	END
END