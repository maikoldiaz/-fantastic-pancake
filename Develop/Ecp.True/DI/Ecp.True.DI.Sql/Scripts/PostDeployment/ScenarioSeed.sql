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

IF	OBJECT_ID('Admin.Scenario') IS NOT NULL
BEGIN

-- Clearing any junk data from the reserved space
--DELETE FROM [Admin].[Scenario] WHERE ScenarioId > 4 AND ScenarioId < 101

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Admin].[Scenario] ON 

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Scenario] WHERE [ScenarioId] = 1)	BEGIN	INSERT [Admin].[Scenario] ([ScenarioId], [Name], [Sequence], [CreatedBy], [CreatedDate]) VALUES (1, N'administration', 10, N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Scenario] WHERE [ScenarioId] = '1' AND ([Sequence] <> '10' OR [Name] <> 'administration'))	BEGIN	UPDATE [Admin].[Scenario] SET [Sequence] = '10', [Name] = 'administration' WHERE [ScenarioId] = '1'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Scenario] WHERE [ScenarioId] = 2)	BEGIN	INSERT [Admin].[Scenario] ([ScenarioId], [Name], [Sequence], [CreatedBy], [CreatedDate]) VALUES (2, N'balanceTransporters', 20, N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Scenario] WHERE [ScenarioId] = '2' AND ([Sequence] <> '20' OR [Name] <> 'balanceTransporters'))	BEGIN	UPDATE [Admin].[Scenario] SET [Sequence] = '20', [Name] = 'balanceTransporters' WHERE [ScenarioId] = '2'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Scenario] WHERE [ScenarioId] = 3)	BEGIN	INSERT [Admin].[Scenario] ([ScenarioId], [Name], [Sequence], [CreatedBy], [CreatedDate]) VALUES (3, N'supplyChain', 40, N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Scenario] WHERE [ScenarioId] = '3' AND ([Sequence] <> '40' OR [Name] <> 'supplyChain'))	BEGIN	UPDATE [Admin].[Scenario] SET [Sequence] = '40', [Name] = 'supplyChain' WHERE [ScenarioId] = '3'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Scenario] WHERE [ScenarioId] = 4)	BEGIN	INSERT [Admin].[Scenario] ([ScenarioId], [Name], [Sequence], [CreatedBy], [CreatedDate]) VALUES (4, N'balanceIntegration', 50, N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Scenario] WHERE [ScenarioId] = '4' AND ([Sequence] <> '50' OR [Name] <> 'balanceIntegration'))	BEGIN	UPDATE [Admin].[Scenario] SET [Sequence] = '50', [Name] = 'balanceIntegration' WHERE [ScenarioId] = '4'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Scenario] WHERE [ScenarioId] = 5)	BEGIN	INSERT [Admin].[Scenario] ([ScenarioId], [Name], [Sequence], [CreatedBy], [CreatedDate]) VALUES (5, N'reports', 30, N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Scenario] WHERE [ScenarioId] = '5' AND ([Sequence] <> '30' OR [Name] <> 'reports'))	BEGIN	UPDATE [Admin].[Scenario] SET [Sequence] = '30', [Name] = 'reports' WHERE [ScenarioId] = '5'	END

SET IDENTITY_INSERT [Admin].[Scenario] OFF
END	