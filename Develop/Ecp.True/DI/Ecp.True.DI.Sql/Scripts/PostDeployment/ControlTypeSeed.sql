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

IF	OBJECT_ID('Admin.ControlType') IS NOT NULL
BEGIN

-- Clearing any junk data from the reserved space
--DELETE FROM [Admin].[Rule] WHERE RuleId > 30 AND RuleId < 1001

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Admin].[ControlType] ON 

IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = 1)	BEGIN	INSERT [Admin].[ControlType] ([ControlTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (1, 'InitialInventory', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = '1' AND ([Name] <> 'InitialInventory'))	BEGIN	UPDATE [Admin].[ControlType] SET [Name] = 'InitialInventory' WHERE [ControlTypeId] = '1'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = 2)	BEGIN	INSERT [Admin].[ControlType] ([ControlTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (2, 'FinalInventory', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = '2' AND ([Name] <> 'FinalInventory'))	BEGIN	UPDATE [Admin].[ControlType] SET [Name] = 'FinalInventory' WHERE [ControlTypeId] = '2'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = 3)	BEGIN	INSERT [Admin].[ControlType] ([ControlTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (3, 'Input', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = '3' AND ([Name] <> 'Input'))	BEGIN	UPDATE [Admin].[ControlType] SET [Name] = 'Input' WHERE [ControlTypeId] = '3'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = 4)	BEGIN	INSERT [Admin].[ControlType] ([ControlTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (4, 'Ouput', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = '4' AND ([Name] <> 'Output'))	BEGIN	UPDATE [Admin].[ControlType] SET [Name] = 'Output' WHERE [ControlTypeId] = '4'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = 5)	BEGIN	INSERT [Admin].[ControlType] ([ControlTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (5, 'IdentifiedLosses', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = '5' AND ([Name] <> 'IdentifiedLosses'))	BEGIN	UPDATE [Admin].[ControlType] SET [Name] = 'IdentifiedLosses' WHERE [ControlTypeId] = '5'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = 6)	BEGIN	INSERT [Admin].[ControlType] ([ControlTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (6, 'Unbalance', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = '6' AND ([Name] <> 'Unbalance'))	BEGIN	UPDATE [Admin].[ControlType] SET [Name] = 'Unbalance' WHERE [ControlTypeId] = '6'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = 7)	BEGIN	INSERT [Admin].[ControlType] ([ControlTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (7, 'Interface', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = '7' AND ([Name] <> 'Interface'))	BEGIN	UPDATE [Admin].[ControlType] SET [Name] = 'Interface' WHERE [ControlTypeId] = '7'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = 8)	BEGIN	INSERT [Admin].[ControlType] ([ControlTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (8, 'Tolerance', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = '8' AND ([Name] <> 'Tolerance'))	BEGIN	UPDATE [Admin].[ControlType] SET [Name] = 'Tolerance' WHERE [ControlTypeId] = '8'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = 9)	BEGIN	INSERT [Admin].[ControlType] ([ControlTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (9, 'UnidentifiedLosses', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = '9' AND ([Name] <> 'UnidentifiedLosses'))	BEGIN	UPDATE [Admin].[ControlType] SET [Name] = 'UnidentifiedLosses' WHERE [ControlTypeId] = '9'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = 10)	BEGIN	INSERT [Admin].[ControlType] ([ControlTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (10, 'InterfaceUnbalance', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = '10' AND ([Name] <> 'InterfaceUnbalance'))	BEGIN	UPDATE [Admin].[ControlType] SET [Name] = 'Output' WHERE [ControlTypeId] = '10'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = 11)	BEGIN	INSERT [Admin].[ControlType] ([ControlTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (11, 'ToleranceUnbalance', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = '11' AND ([Name] <> 'ToleranceUnbalance'))	BEGIN	UPDATE [Admin].[ControlType] SET [Name] = 'ToleranceUnbalance' WHERE [ControlTypeId] = '11'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = 12)	BEGIN	INSERT [Admin].[ControlType] ([ControlTypeId], [Name], [CreatedBy], [CreatedDate]) VALUES (12, 'UnidentifiedLossesUnbalance', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ControlType] WHERE [ControlTypeId] = '12' AND ([Name] <> 'IdentifiedLossesUnbalance'))	BEGIN	UPDATE [Admin].[ControlType] SET [Name] = 'IdentifiedLossesUnbalance' WHERE [ControlTypeId] = '12'	END

SET IDENTITY_INSERT [Admin].[ControlType] OFF
END	

