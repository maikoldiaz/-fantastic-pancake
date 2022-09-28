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

IF	OBJECT_ID('Admin.MessageType') IS NOT NULL
BEGIN

-- Clearing any junk data from the reserved space
--DELETE FROM [Admin].[MessageType] WHERE MessageTypeId > 4 AND MessageTypeId < 101

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Admin].[MessageType] ON 

IF NOT EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = 1)	BEGIN	INSERT [Admin].[MessageType] ([MessageTypeId], [Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (1, N'Movement', N'System', @CurrentTime, NULL, NULL)			END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = '1' AND ([Name] <> 'Movement'))				BEGIN	UPDATE [Admin].[MessageType] SET [Name] = 'Movement', [LastModifiedBy] = 'System', [LastModifiedDate] = @CurrentTime WHERE [MessageTypeId] = '1'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = 2)	BEGIN	INSERT [Admin].[MessageType] ([MessageTypeId], [Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (2, N'Loss', N'System', @CurrentTime, NULL, NULL)				END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = '2' AND ([Name] <> 'Loss'))					BEGIN	UPDATE [Admin].[MessageType] SET [Name] = 'Loss', [LastModifiedBy] = 'System', [LastModifiedDate] = @CurrentTime WHERE [MessageTypeId] = '2'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = 3)	BEGIN	INSERT [Admin].[MessageType] ([MessageTypeId], [Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (3, N'SpecialMovement', N'System', @CurrentTime, NULL, NULL)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = '3' AND ([Name] <> 'SpecialMovement'))		BEGIN	UPDATE [Admin].[MessageType] SET [Name] = 'SpecialMovement', [LastModifiedBy] = 'System', [LastModifiedDate] = @CurrentTime WHERE [MessageTypeId] = '3'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = 4)	BEGIN	INSERT [Admin].[MessageType] ([MessageTypeId], [Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (4, N'Inventory', N'System', @CurrentTime, NULL, NULL)			END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = '4' AND ([Name] <> 'Inventory'))				BEGIN	UPDATE [Admin].[MessageType] SET [Name] = 'Inventory', [LastModifiedBy] = 'System', [LastModifiedDate] = @CurrentTime WHERE [MessageTypeId] = '4'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = 5)	BEGIN	INSERT [Admin].[MessageType] ([MessageTypeId], [Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (5, N'MovementAndInventory', N'System', @CurrentTime, NULL, NULL)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = '5' AND ([Name] <> 'MovementAndInventory'))	BEGIN	UPDATE [Admin].[MessageType] SET [Name] = 'MovementAndInventory', [LastModifiedBy] = 'System', [LastModifiedDate] = @CurrentTime WHERE [MessageTypeId] = '5'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = 6)	BEGIN	INSERT [Admin].[MessageType] ([MessageTypeId], [Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (6, N'MovementOwnership', N'System', @CurrentTime, NULL, NULL)		END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = '6' AND ([Name] <> 'MovementOwnership'))		BEGIN	UPDATE [Admin].[MessageType] SET [Name] = 'MovementOwnership', [LastModifiedBy] = 'System', [LastModifiedDate] = @CurrentTime WHERE [MessageTypeId] = '6'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = 7)	BEGIN	INSERT [Admin].[MessageType] ([MessageTypeId], [Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (7, N'InventoryOwnership', N'System', @CurrentTime, NULL, NULL)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = '7' AND ([Name] <> 'InventoryOwnership'))		BEGIN	UPDATE [Admin].[MessageType] SET [Name] = 'InventoryOwnership', [LastModifiedBy] = 'System', [LastModifiedDate] = @CurrentTime WHERE [MessageTypeId] = '7'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = 8)	BEGIN	INSERT [Admin].[MessageType] ([MessageTypeId], [Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (8, N'Contract', N'System', @CurrentTime, NULL, NULL)				END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = '8' AND ([Name] <> 'Contract'))				BEGIN	UPDATE [Admin].[MessageType] SET [Name] = 'Contract', [LastModifiedBy] = 'System', [LastModifiedDate] = @CurrentTime WHERE [MessageTypeId] = '8'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = 9)	BEGIN	INSERT [Admin].[MessageType] ([MessageTypeId], [Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (9, N'Events', N'System', @CurrentTime, NULL, NULL)				END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[MessageType] WHERE [MessageTypeId] = '9' AND ([Name] <> 'Events'))					BEGIN	UPDATE [Admin].[MessageType] SET [Name] = 'Events', [LastModifiedBy] = 'System', [LastModifiedDate] = @CurrentTime WHERE [MessageTypeId] = '9'	END

SET IDENTITY_INSERT [Admin].[MessageType] OFF
END	
