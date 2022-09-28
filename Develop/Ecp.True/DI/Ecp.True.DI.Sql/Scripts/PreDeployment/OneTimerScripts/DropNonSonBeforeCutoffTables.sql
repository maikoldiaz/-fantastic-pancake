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
	
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='2F461981-1CE3-4FB6-8EB7-3BDBED28A946' AND Status = 1)
		BEGIN
			BEGIN TRY

				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OperationalNonSon'))
				BEGIN
					DROP TABLE [Admin].[OperationalNonSon];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OperationalMovementQualityNonSon'))
				BEGIN
					DROP TABLE [Admin].[OperationalMovementQualityNonSon];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OperationalMovementOwnerNonSon'))
				BEGIN
					DROP TABLE [Admin].[OperationalMovementOwnerNonSon];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OperationalInventoryQualityNonSon'))
				BEGIN
					DROP TABLE [Admin].[OperationalInventoryQualityNonSon];
				END


				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('2F461981-1CE3-4FB6-8EB7-3BDBED28A946', 1);
			END TRY
			

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('2F461981-1CE3-4FB6-8EB7-3BDBED28A946', 0);
			END CATCH
		END
END