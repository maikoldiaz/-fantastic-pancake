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
	
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='c111b02b-daee-49ad-9a49-78be7b3c165a' AND Status = 1)
		BEGIN
			BEGIN TRY

				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'InventoryDetailsWithoutOwner'))
				BEGIN
					DROP VIEW [Admin].[InventoryDetailsWithoutOwner];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'QualityDetailsWithoutOwner'))
				BEGIN
					DROP VIEW [Admin].[QualityDetailsWithoutOwner];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'view_MovementDetails'))
				BEGIN
					DROP VIEW [Admin].[view_MovementDetails];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'view_AttributeDetails'))
				BEGIN
					DROP VIEW [Admin].[view_AttributeDetails];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'BackupMovementDetailsWithoutOwner'))
				BEGIN
					DROP VIEW [Admin].[BackupMovementDetailsWithoutOwner];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'KPIDataByCategoryElementNodeWithOwnership'))
				BEGIN
					DROP VIEW [Admin].[KPIDataByCategoryElementNodeWithOwnership];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'KPIPreviousDateDataByCategoryElementNodeWithOwner'))
				BEGIN
					DROP VIEW [Admin].[KPIPreviousDateDataByCategoryElementNodeWithOwner];
				END
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('c111b02b-daee-49ad-9a49-78be7b3c165a', 1);
			END TRY
			

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('c111b02b-daee-49ad-9a49-78be7b3c165a', 0);
			END CATCH
		END
END