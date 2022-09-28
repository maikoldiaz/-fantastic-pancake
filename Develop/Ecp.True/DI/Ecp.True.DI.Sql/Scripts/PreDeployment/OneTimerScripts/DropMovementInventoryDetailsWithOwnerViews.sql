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
	
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='f81f8fa7-5e40-4c31-aabc-574ac9c3c5bc' AND Status = 1)
		BEGIN
			BEGIN TRY

				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'MovementDetailsWithOwner'))
				BEGIN
					DROP VIEW [Admin].[MovementDetailsWithOwner];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'InventoryDetailsWithOwner'))
				BEGIN
					DROP VIEW [Admin].[InventoryDetailsWithOwner];
				END

				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('f81f8fa7-5e40-4c31-aabc-574ac9c3c5bc', 1);
			END TRY
			

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('f81f8fa7-5e40-4c31-aabc-574ac9c3c5bc', 0);
			END CATCH
		END
END