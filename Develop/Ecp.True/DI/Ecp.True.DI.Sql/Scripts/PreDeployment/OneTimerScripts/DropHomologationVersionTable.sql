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
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND  TABLE_NAME = 'HomologationVersion'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='fab1de7e-6e5d-4b5e-a964-61f333e3cce3' AND Status = 1)
		BEGIN
			BEGIN TRY
				DROP TABLE [Admin].[HomologationVersion];
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('fab1de7e-6e5d-4b5e-a964-61f333e3cce3', 1);
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('fab1de7e-6e5d-4b5e-a964-61f333e3cce3', 0);
			END CATCH
		END
	END
END