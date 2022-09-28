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

IF	OBJECT_ID('Admin.TicketType') IS NOT NULL
BEGIN

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Admin].[TicketType] ON 

IF NOT EXISTS (SELECT 'X' FROM [Admin].[TicketType] WHERE [TicketTypeId] = 1)	BEGIN	INSERT [Admin].[TicketType] ([TicketTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (1, N'Cutoff', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[TicketType] WHERE [TicketTypeId] = '1' AND ([Name] <> 'Cutoff'))	BEGIN	UPDATE [Admin].[TicketType] SET [Name] = 'Cutoff' WHERE [TicketTypeId] = '1'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[TicketType] WHERE [TicketTypeId] = 2)	BEGIN	INSERT [Admin].[TicketType] ([TicketTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (2, N'Ownership', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[TicketType] WHERE [TicketTypeId] = '2' AND ([Name] <> 'Ownership'))	BEGIN	UPDATE [Admin].[TicketType] SET [Name] = 'Ownership' WHERE [TicketTypeId] = '2'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[TicketType] WHERE [TicketTypeId] = 3)	BEGIN	INSERT [Admin].[TicketType] ([TicketTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (3, N'Logistics', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[TicketType] WHERE [TicketTypeId] = '3' AND ([Name] <> 'Logistics'))	BEGIN	UPDATE [Admin].[TicketType] SET [Name] = 'Logistics' WHERE [TicketTypeId] = '3'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[TicketType] WHERE [TicketTypeId] = 4)	BEGIN	INSERT [Admin].[TicketType] ([TicketTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (4, N'Delta', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[TicketType] WHERE [TicketTypeId] = '4' AND ([Name] <> 'Delta'))	BEGIN	UPDATE [Admin].[TicketType] SET [Name] = 'Delta' WHERE [TicketTypeId] = '4'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[TicketType] WHERE [TicketTypeId] = 5)	BEGIN	INSERT [Admin].[TicketType] ([TicketTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (5, N'OfficialDelta', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[TicketType] WHERE [TicketTypeId] = '5' AND ([Name] <> 'OfficialDelta'))	BEGIN	UPDATE [Admin].[TicketType] SET [Name] = 'OfficialDelta' WHERE [TicketTypeId] = '5'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[TicketType] WHERE [TicketTypeId] = 6)	BEGIN	INSERT [Admin].[TicketType] ([TicketTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (6, N'OfficialLogistics', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[TicketType] WHERE [TicketTypeId] = '6' AND ([Name] <> 'OfficialLogistics'))	BEGIN	UPDATE [Admin].[TicketType] SET [Name] = 'OfficialLogistics' WHERE [TicketTypeId] = '6'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[TicketType] WHERE [TicketTypeId] = 7)	BEGIN	INSERT [Admin].[TicketType] ([TicketTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (7, N'LogisticMovements', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[TicketType] WHERE [TicketTypeId] = '7' AND ([Name] <> 'LogisticMovements'))	BEGIN	UPDATE [Admin].[TicketType] SET [Name] = 'LogisticMovements' WHERE [TicketTypeId] = '7'	END

SET IDENTITY_INSERT [Admin].[TicketType] OFF
END	
