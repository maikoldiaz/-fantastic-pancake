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
	
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='83CB658D-CF85-49C9-8631-A16448D7ED2B' AND Status = 1)
		BEGIN
			BEGIN TRY

				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'AttributeDetailsWithOwner'))
				BEGIN
					DROP VIEW [Admin].[AttributeDetailsWithOwner];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'QualityDetailsWithOwner'))
				BEGIN
					DROP VIEW [Admin].[QualityDetailsWithOwner];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'MovementsByProductWithOwner'))
				BEGIN
					DROP VIEW [Admin].[MovementsByProductWithOwner];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'view_MovementsByProduct'))
				BEGIN
					DROP VIEW [Admin].[view_MovementsByProduct];
				END

				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'view_KPIDataByCategoryElementNode'))
				BEGIN
					DROP VIEW [Admin].[view_KPIDataByCategoryElementNode];
				END
				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'view_KPIPreviousDateDataByCategoryElementNode'))
				BEGIN
					DROP VIEW [Admin].[view_KPIPreviousDateDataByCategoryElementNode];
				END

				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'view_RelKPI'))
				BEGIN
					DROP VIEW [Admin].[view_RelKPI];
				END

				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('83CB658D-CF85-49C9-8631-A16448D7ED2B', 1);
			END TRY
			

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('83CB658D-CF85-49C9-8631-A16448D7ED2B', 0);
			END CATCH
		END
END