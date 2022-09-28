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

IF	OBJECT_ID('Admin.ReportType') IS NOT NULL
BEGIN

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Admin].[ReportType] ON 

IF NOT EXISTS (SELECT 'X' FROM [Admin].[ReportType] WHERE [ReportTypeId] = 1)	BEGIN	INSERT [Admin].[ReportType] ([ReportTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (1, N'BeforeCutOff', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ReportType] WHERE [ReportTypeId] = '1' AND ([Name] <> 'BeforeCutOff'))	BEGIN	UPDATE [Admin].[ReportType] SET [Name] = 'BeforeCutOff' WHERE [ReportTypeId] = '1'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ReportType] WHERE [ReportTypeId] = 2)	BEGIN	INSERT [Admin].[ReportType] ([ReportTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (2, N'OfficialNodeBalance', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ReportType] WHERE [ReportTypeId] = '2' AND ([Name] <> 'OfficialNodeBalance'))	BEGIN	UPDATE [Admin].[ReportType] SET [Name] = 'OfficialNodeBalance' WHERE [ReportTypeId] = '2'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ReportType] WHERE [ReportTypeId] = 3)	BEGIN	INSERT [Admin].[ReportType] ([ReportTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (3, N'OfficialInitialBalance', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ReportType] WHERE [ReportTypeId] = '3' AND ([Name] <> 'OfficialInitialBalance'))	BEGIN	UPDATE [Admin].[ReportType] SET [Name] = 'OfficialInitialBalance' WHERE [ReportTypeId] = '3'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ReportType] WHERE [ReportTypeId] = 4)	BEGIN	INSERT [Admin].[ReportType] ([ReportTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (4, N'OperativeBalance', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ReportType] WHERE [ReportTypeId] = '4' AND ([Name] <> 'OperativeBalance'))	BEGIN	UPDATE [Admin].[ReportType] SET [Name] = 'OperativeBalance' WHERE [ReportTypeId] = '4'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ReportType] WHERE [ReportTypeId] = 5)	BEGIN	INSERT [Admin].[ReportType] ([ReportTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (5, N'SapBalance', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ReportType] WHERE [ReportTypeId] = '5' AND ([Name] <> 'SapBalance'))	BEGIN	UPDATE [Admin].[ReportType] SET [Name] = 'SapBalance' WHERE [ReportTypeId] = '5'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ReportType] WHERE [ReportTypeId] = 6)	BEGIN	INSERT [Admin].[ReportType] ([ReportTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (6, N'UserRolesAndPermissions', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ReportType] WHERE [ReportTypeId] = '6' AND ([Name] <> 'UserRolesAndPermissions'))	BEGIN	UPDATE [Admin].[ReportType] SET [Name] = 'UserRolesAndPermissions' WHERE [ReportTypeId] = '6'	END

SET IDENTITY_INSERT [Admin].[ReportType] OFF
END	
