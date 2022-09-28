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



IF	OBJECT_ID('Offchain.NodeStateType') IS NOT NULL
BEGIN

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Offchain].[NodeStateType] ON 

IF NOT EXISTS (SELECT 'X' FROM [Offchain].[NodeStateType] WHERE [NodeStateTypeId] = 1)	BEGIN	INSERT [Offchain].[NodeStateType] ([NodeStateTypeId], [Name], [CreatedBy]) VALUES (1, 'CreatedNode', 'System')	END	ELSE IF EXISTS (SELECT 'X' FROM [Offchain].[NodeStateType] WHERE [NodeStateTypeId] = '1' AND ([Name] <> 'CreatedNode'))		BEGIN	UPDATE [Offchain].[NodeStateType] SET [Name] = 'CreatedNode' WHERE [NodeStateTypeId] = '1'	END
IF NOT EXISTS (SELECT 'X' FROM [Offchain].[NodeStateType] WHERE [NodeStateTypeId] = 2)	BEGIN	INSERT [Offchain].[NodeStateType] ([NodeStateTypeId], [Name], [CreatedBy]) VALUES (2, 'UpdatedNode', 'System')	END	ELSE IF EXISTS (SELECT 'X' FROM [Offchain].[NodeStateType] WHERE [NodeStateTypeId] = '2' AND ([Name] <> 'UpdatedNode'))		BEGIN	UPDATE [Offchain].[NodeStateType] SET [Name] = 'UpdatedNode' WHERE [NodeStateTypeId] = '2'	END
IF NOT EXISTS (SELECT 'X' FROM [Offchain].[NodeStateType] WHERE [NodeStateTypeId] = 3)	BEGIN	INSERT [Offchain].[NodeStateType] ([NodeStateTypeId], [Name], [CreatedBy]) VALUES (3, 'OperativeBalanceCalculated', 'System')	END	ELSE IF EXISTS (SELECT 'X' FROM [Offchain].[NodeStateType] WHERE [NodeStateTypeId] = '3' AND ([Name] <> 'OperativeBalanceCalculated'))		BEGIN	UPDATE [Offchain].[NodeStateType] SET [Name] = 'OperativeBalanceCalculated' WHERE [NodeStateTypeId] = '3'	END
IF NOT EXISTS (SELECT 'X' FROM [Offchain].[NodeStateType] WHERE [NodeStateTypeId] = 4)	BEGIN	INSERT [Offchain].[NodeStateType] ([NodeStateTypeId], [Name], [CreatedBy]) VALUES (4, 'OperativeBalanceCalculatedWithOwnership', 'System')	END	ELSE IF EXISTS (SELECT 'X' FROM [Offchain].[NodeStateType] WHERE [NodeStateTypeId] = '4' AND ([Name] <> 'OperativeBalanceCalculatedWithOwnership'))		BEGIN	UPDATE [Offchain].[NodeStateType] SET [Name] = 'OperativeBalanceCalculatedWithOwnership' WHERE [NodeStateTypeId] = '4'	END

SET IDENTITY_INSERT [Offchain].[NodeStateType] OFF
END	
