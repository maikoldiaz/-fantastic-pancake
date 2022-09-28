/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF	OBJECT_ID('Admin.ScenarioType') IS NOT NULL
BEGIN

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Admin].[ScenarioType] ON 

IF NOT EXISTS (SELECT 'X' FROM [Admin].[ScenarioType] WHERE [ScenarioTypeId] = '1')	BEGIN	INSERT [Admin].[ScenarioType] ([ScenarioTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (1, N'Operativo', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ScenarioType] WHERE [ScenarioTypeId] = '1' AND ([Name] <> 'Operativo'))	BEGIN	UPDATE [Admin].[ScenarioType] SET [Name] = 'Operativo' WHERE [ScenarioTypeId] = 1	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ScenarioType] WHERE [ScenarioTypeId] = '2')	BEGIN	INSERT [Admin].[ScenarioType] ([ScenarioTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (2, N'Oficial', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ScenarioType] WHERE [ScenarioTypeId] = '2' AND ([Name] <> 'Oficial'))	BEGIN	UPDATE [Admin].[ScenarioType] SET [Name] = 'Oficial' WHERE [ScenarioTypeId] = 2	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ScenarioType] WHERE [ScenarioTypeId] = '3')	BEGIN	INSERT [Admin].[ScenarioType] ([ScenarioTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (3, N'Consolidado', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ScenarioType] WHERE [ScenarioTypeId] = '3' AND ([Name] <> 'Consolidado'))	BEGIN	UPDATE [Admin].[ScenarioType] SET [Name] = 'Consolidado' WHERE [ScenarioTypeId] = 3	END

SET IDENTITY_INSERT [Admin].[ScenarioType] OFF
END	

