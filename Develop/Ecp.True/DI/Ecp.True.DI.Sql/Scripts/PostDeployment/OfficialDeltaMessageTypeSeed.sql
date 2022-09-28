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

IF	OBJECT_ID('Admin.OfficialDeltaMessageType') IS NOT NULL
BEGIN

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Admin].[OfficialDeltaMessageType] ON 

IF NOT EXISTS (SELECT 'X' FROM [Admin].[OfficialDeltaMessageType] WHERE [OfficialDeltaMessageTypeId] = 1)	BEGIN	INSERT [Admin].[OfficialDeltaMessageType] ([OfficialDeltaMessageTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (1, N'OfficialInventoryDelta', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[OfficialDeltaMessageType] WHERE [OfficialDeltaMessageTypeId] = '1' AND ([Name] <> 'OfficialInventoryDelta'))	BEGIN	UPDATE [Admin].[OfficialDeltaMessageType] SET [Name] = 'OfficialInventoryDelta' WHERE [OfficialDeltaMessageTypeId] = '1'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[OfficialDeltaMessageType] WHERE [OfficialDeltaMessageTypeId] = 2)	BEGIN	INSERT [Admin].[OfficialDeltaMessageType] ([OfficialDeltaMessageTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (2, N'ConsolidatedInventoryDelta', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[OfficialDeltaMessageType] WHERE [OfficialDeltaMessageTypeId] = '2' AND ([Name] <> 'ConsolidatedInventoryDelta'))	BEGIN	UPDATE [Admin].[OfficialDeltaMessageType] SET [Name] = 'ConsolidatedInventoryDelta' WHERE [OfficialDeltaMessageTypeId] = '2'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[OfficialDeltaMessageType] WHERE [OfficialDeltaMessageTypeId] = 3)	BEGIN	INSERT [Admin].[OfficialDeltaMessageType] ([OfficialDeltaMessageTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (3, N'OfficialMovementDelta', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[OfficialDeltaMessageType] WHERE [OfficialDeltaMessageTypeId] = '3' AND ([Name] <> 'OfficialMovementDelta'))	BEGIN	UPDATE [Admin].[OfficialDeltaMessageType] SET [Name] = 'OfficialMovementDelta' WHERE [OfficialDeltaMessageTypeId] = '3'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[OfficialDeltaMessageType] WHERE [OfficialDeltaMessageTypeId] = 4)	BEGIN	INSERT [Admin].[OfficialDeltaMessageType] ([OfficialDeltaMessageTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (4, N'ConsolidatedMovementDelta', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[OfficialDeltaMessageType] WHERE [OfficialDeltaMessageTypeId] = '4' AND ([Name] <> 'ConsolidatedMovementDelta'))	BEGIN	UPDATE [Admin].[OfficialDeltaMessageType] SET [Name] = 'ConsolidatedMovementDelta' WHERE [OfficialDeltaMessageTypeId] = '4'	END

SET IDENTITY_INSERT [Admin].[OfficialDeltaMessageType] OFF
END	
