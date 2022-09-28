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
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND  TABLE_NAME = 'Rule'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='3752ac12-501a-4be2-b1fe-7d4cc777bf46' AND Status = 1)
		BEGIN
			BEGIN TRY
				DROP TABLE [Admin].[Rule];
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('3752ac12-501a-4be2-b1fe-7d4cc777bf46', 1);
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('3752ac12-501a-4be2-b1fe-7d4cc777bf46', 0);
			END CATCH
		END
	END
END

IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND  TABLE_NAME = 'RuleName'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='65c88044-4130-4e9c-b45a-bb9aa2068d2d' AND Status = 1)
		BEGIN
			BEGIN TRY
				DROP TABLE [Admin].[RuleName];
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('65c88044-4130-4e9c-b45a-bb9aa2068d2d', 1);
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('65c88044-4130-4e9c-b45a-bb9aa2068d2d', 0);
			END CATCH
		END
	END
END

IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND  TABLE_NAME = 'RuleType'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='7286f52b-6144-44ad-a909-9ecc6d0a050d' AND Status = 1)
		BEGIN
			BEGIN TRY
				DROP TABLE [Admin].[RuleType];
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('7286f52b-6144-44ad-a909-9ecc6d0a050d', 1);
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('7286f52b-6144-44ad-a909-9ecc6d0a050d', 0);
			END CATCH
		END
	END
END