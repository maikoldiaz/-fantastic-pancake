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

IF	OBJECT_ID('Admin.Role') IS NOT NULL
BEGIN

-- Clearing any junk data from the reserved space
--DELETE FROM [Admin].[Role] WHERE RoleId > 5 AND RoleId < 101

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Admin].[Role] ON 

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Role] WHERE [RoleId] = 1)	BEGIN	INSERT [Admin].[Role] ([RoleId], [RoleName], [CreatedBy], [CreatedDate]) VALUES (1, N'ADMINISTRADOR', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Role] WHERE [RoleId] = '1' AND ([RoleName] <> 'ADMINISTRADOR'))	BEGIN	UPDATE [Admin].[Role] SET [RoleName] = 'ADMINISTRADOR' WHERE [RoleId] = '1'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Role] WHERE [RoleId] = 2)	BEGIN	INSERT [Admin].[Role] ([RoleId], [RoleName], [CreatedBy], [CreatedDate]) VALUES (2, N'APROBADOR', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Role] WHERE [RoleId] = '2' AND ([RoleName] <> 'APROBADOR'))	BEGIN	UPDATE [Admin].[Role] SET [RoleName] = 'APROBADOR' WHERE [RoleId] = '2'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Role] WHERE [RoleId] = 3)	BEGIN	INSERT [Admin].[Role] ([RoleId], [RoleName], [CreatedBy], [CreatedDate]) VALUES (3, N'PROFESIONAL BALANCES DEL SEGMENTO', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Role] WHERE [RoleId] = '3' AND ([RoleName] <> 'PROFESIONAL BALANCES DEL SEGMENTO'))	BEGIN	UPDATE [Admin].[Role] SET [RoleName] = 'PROFESIONAL BALANCES DEL SEGMENTO' WHERE [RoleId] = '3'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Role] WHERE [RoleId] = 4)	BEGIN	INSERT [Admin].[Role] ([RoleId], [RoleName], [CreatedBy], [CreatedDate]) VALUES (4, N'PROGRAMADOR', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Role] WHERE [RoleId] = '4' AND ([RoleName] <> 'PROGRAMADOR'))	BEGIN	UPDATE [Admin].[Role] SET [RoleName] = 'PROGRAMADOR' WHERE [RoleId] = '4'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Role] WHERE [RoleId] = 5)	BEGIN	INSERT [Admin].[Role] ([RoleId], [RoleName], [CreatedBy], [CreatedDate]) VALUES (5, N'CONSULTA', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Role] WHERE [RoleId] = '5' AND ([RoleName] <> 'CONSULTA'))	BEGIN	UPDATE [Admin].[Role] SET [RoleName] = 'CONSULTA' WHERE [RoleId] = '5'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Role] WHERE [RoleId] = 6)	BEGIN	INSERT [Admin].[Role] ([RoleId], [RoleName], [CreatedBy], [CreatedDate]) VALUES (6, N'AUDITOR', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Role] WHERE [RoleId] = '6' AND ([RoleName] <> 'AUDITOR'))	BEGIN	UPDATE [Admin].[Role] SET [RoleName] = 'AUDITOR' WHERE [RoleId] = '6'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Role] WHERE [RoleId] = 7)	BEGIN	INSERT [Admin].[Role] ([RoleId], [RoleName], [CreatedBy], [CreatedDate]) VALUES (7, N'USUARIO DE CADENA', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Role] WHERE [RoleId] = '7' AND ([RoleName] <> 'USUARIO DE CADENA'))	BEGIN	UPDATE [Admin].[Role] SET [RoleName] = 'USUARIO DE CADENA' WHERE [RoleId] = '7'	END

SET IDENTITY_INSERT [Admin].[Role] OFF
END	