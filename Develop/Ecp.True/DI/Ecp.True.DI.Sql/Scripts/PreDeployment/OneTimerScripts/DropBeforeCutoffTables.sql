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
	
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='5026C0EC-68F2-4804-9AD3-71B56990CBF0' AND Status = 1)
		BEGIN
			BEGIN TRY

				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'MovementInformationMovforSegmentReport'))
				BEGIN
					TRUNCATE TABLE [Admin].[MovementInformationMovforSegmentReport];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'MovementInformationMovforSystemReport'))
				BEGIN
					TRUNCATE TABLE [Admin].[MovementInformationMovforSystemReport];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'NodeTagCalculationDateForSegmentReport'))
				BEGIN
					TRUNCATE TABLE [Admin].[NodeTagCalculationDateForSegmentReport];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'NodeTagCalculationDateForSystemReport'))
				BEGIN
					TRUNCATE TABLE [Admin].[NodeTagCalculationDateForSegmentReport];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OperationalInventory'))
				BEGIN
					TRUNCATE TABLE [Admin].[OperationalInventory];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OperationalInventoryOwner'))
				BEGIN
					TRUNCATE TABLE [Admin].[OperationalInventoryOwner];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OperationalInventoryQuality'))
				BEGIN
					TRUNCATE TABLE [Admin].[OperationalInventoryQuality];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OperationalMovement'))
				BEGIN
					TRUNCATE TABLE [Admin].[OperationalMovement];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OperationalMovementOwner'))
				BEGIN
					TRUNCATE TABLE [Admin].[OperationalMovementOwner];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'OperationalMovementQuality'))
				BEGIN
					TRUNCATE TABLE [Admin].[OperationalMovementQuality];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'Operational'))
				BEGIN
					TRUNCATE TABLE [Admin].[Operational];
				END


				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('5026C0EC-68F2-4804-9AD3-71B56990CBF0', 1);
			END TRY
			

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('5026C0EC-68F2-4804-9AD3-71B56990CBF0', 0);
			END CATCH
		END
END