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

IF EXISTS (Select 'X' from sys.schemas Where name = 'Analytics')
BEGIN
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Analytics' AND  TABLE_NAME = 'OwnershipAnalytics'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='B5037ADE-7AC5-4ECC-BE3F-7AEFA699F479' AND Status = 1)
		BEGIN
			BEGIN TRY
				DROP TABLE [Analytics].[OwnershipAnalytics];
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('B5037ADE-7AC5-4ECC-BE3F-7AEFA699F479', 1, 'POST');
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('B5037ADE-7AC5-4ECC-BE3F-7AEFA699F479', 0, 'POST');
			END CATCH
		END
	END
END