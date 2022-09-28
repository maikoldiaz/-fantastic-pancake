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

IF	OBJECT_ID('Admin.OriginType') IS NOT NULL
BEGIN

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Admin].[OriginType] ON 

IF NOT EXISTS (SELECT 'X' FROM [Admin].[OriginType] WHERE [OriginTypeId] = '1')	BEGIN	INSERT [Admin].[OriginType] ([OriginTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (1, N'Origen', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[OriginType] WHERE [OriginTypeId] = '1' AND ([Name] <> 'Origen'))	BEGIN	UPDATE [Admin].[OriginType] SET [Name] = 'Origen' WHERE [OriginTypeId] = 1	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[OriginType] WHERE [OriginTypeId] = '2')	BEGIN	INSERT [Admin].[OriginType] ([OriginTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (2, N'Destino', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[OriginType] WHERE [OriginTypeId] = '2' AND ([Name] <> 'Destino'))	BEGIN	UPDATE [Admin].[OriginType] SET [Name] = 'Destino' WHERE [OriginTypeId] = 2	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[OriginType] WHERE [OriginTypeId] = '3')	BEGIN	INSERT [Admin].[OriginType] ([OriginTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (3, N'Ninguno', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[OriginType] WHERE [OriginTypeId] = '3' AND ([Name] <> 'Ninguno'))	BEGIN	UPDATE [Admin].[OriginType] SET [Name] = 'Ninguno' WHERE [OriginTypeId] = 3	END

SET IDENTITY_INSERT [Admin].[OriginType] OFF
END	

