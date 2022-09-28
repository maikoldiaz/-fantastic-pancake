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
	
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='21655B5A-226E-4292-868B-551FF336EC70' AND Status = 1)
		BEGIN
			BEGIN TRY

				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'ConsolidatedDeltaMovementInformation'))
				BEGIN
					DROP TABLE [Admin].[ConsolidatedDeltaMovementInformation];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'ConsolidatedNodeTagCalculationDate'))
				BEGIN
					DROP TABLE [Admin].[ConsolidatedNodeTagCalculationDate];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'ConsolidatedDeltaMovements'))
				BEGIN
					DROP TABLE [Admin].[ConsolidatedDeltaMovements];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'ConsolidatedDeltaInventory'))
				BEGIN
					DROP TABLE [Admin].[ConsolidatedDeltaInventory];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'ConsolidatedDeltaBalance'))
				BEGIN
					DROP TABLE [Admin].[ConsolidatedDeltaBalance];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OfficialMovementInformation'))
				BEGIN
					DROP TABLE [Admin].[OfficialMovementInformation];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OfficialNodeTagCalculationDate'))
				BEGIN
					DROP TABLE [Admin].[OfficialNodeTagCalculationDate];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OfficialMonthlyMovementDetails'))
				BEGIN
					DROP TABLE [Admin].[OfficialMonthlyMovementDetails];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OfficialMonthlyMovementQualityDetails'))
				BEGIN
					DROP TABLE [Admin].[OfficialMonthlyMovementQualityDetails];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OfficialMonthlyInventoryDetails'))
				BEGIN
					DROP TABLE [Admin].[OfficialMonthlyInventoryDetails];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OfficialMonthlyInventoryQualityDetails'))
				BEGIN
					DROP TABLE [Admin].[OfficialMonthlyInventoryQualityDetails];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OfficialMonthlyBalance'))
				BEGIN
					DROP TABLE [Admin].[OfficialMonthlyBalance];
				END


				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('21655B5A-226E-4292-868B-551FF336EC70', 1);
			END TRY
			

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('21655B5A-226E-4292-868B-551FF336EC70', 0);
			END CATCH
		END
END