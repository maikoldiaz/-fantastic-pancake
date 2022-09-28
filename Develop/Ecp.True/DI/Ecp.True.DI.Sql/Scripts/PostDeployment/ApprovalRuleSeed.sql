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

IF	OBJECT_ID('Admin.ApprovalRule') IS NOT NULL
BEGIN

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Admin].[ApprovalRule] ON 

IF NOT EXISTS (SELECT 'X' FROM [Admin].[ApprovalRule] WHERE [ApprovalRuleId] = '1')	BEGIN	INSERT [Admin].[ApprovalRule] ([ApprovalRuleId], [Rule], [CreatedBy], [CreatedDate]) VALUES (1, N'PI/E < 0.2', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ApprovalRule] WHERE [ApprovalRuleId] = '1' AND ([Rule] <> 'PI/E < 0.2'))	BEGIN	UPDATE [Admin].[ApprovalRule] SET [Rule] = 'PI/E < 0.2' WHERE [ApprovalRuleId] = 1	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ApprovalRule] WHERE [ApprovalRuleId] = '2')	BEGIN	INSERT [Admin].[ApprovalRule] ([ApprovalRuleId], [Rule], [CreatedBy], [CreatedDate]) VALUES (2, N'PI/E < 0.2 AND PNI/E < 0.18', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ApprovalRule] WHERE [ApprovalRuleId] = '2' AND ([Rule] <> 'PI/E < 0.2 AND PNI/E < 0.18'))	BEGIN	UPDATE [Admin].[ApprovalRule] SET [Rule] = 'PI/E < 0.2 AND PNI/E < 0.18' WHERE [ApprovalRuleId] = 2	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ApprovalRule] WHERE [ApprovalRuleId] = '3')	BEGIN	INSERT [Admin].[ApprovalRule] ([ApprovalRuleId], [Rule], [CreatedBy], [CreatedDate]) VALUES (3, N'(PI/E < 0.2 AND PNI/E < 0.18) OR PNI <= 100000', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ApprovalRule] WHERE [ApprovalRuleId] = '3' AND ([Rule] <> '(PI/E < 0.2 AND PNI/E < 0.18) OR PNI <= 100000'))	BEGIN	UPDATE [Admin].[ApprovalRule] SET [Rule] = '(PI/E < 0.2 AND PNI/E < 0.18) OR PNI <= 100000' WHERE [ApprovalRuleId] = 3	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[ApprovalRule] WHERE [ApprovalRuleId] = '4')	BEGIN	INSERT [Admin].[ApprovalRule] ([ApprovalRuleId], [Rule], [CreatedBy], [CreatedDate]) VALUES (4, N'((PI+PNI)*0.5)/(E+Io) <= 0.24', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[ApprovalRule] WHERE [ApprovalRuleId] = '4' AND ([Rule] <> '((PI+PNI)*0.5)/(E+Io) <= 0.24'))	BEGIN	UPDATE [Admin].[ApprovalRule] SET [Rule] = '((PI+PNI)*0.5)/(E+Io) <= 0.24' WHERE [ApprovalRuleId] = 4	END

SET IDENTITY_INSERT [Admin].[ApprovalRule] OFF
END	

