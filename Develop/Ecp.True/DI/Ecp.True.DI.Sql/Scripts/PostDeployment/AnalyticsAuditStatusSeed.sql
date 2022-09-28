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

IF	OBJECT_ID('Analytics.AuditStatus') IS NOT NULL
BEGIN

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Analytics].[AuditStatus] ON 

IF NOT EXISTS (SELECT 'X' FROM [Analytics].[AuditStatus] WHERE [StatusId] = 1)	BEGIN	INSERT INTO [Analytics].[AuditStatus] ([StatusId],[Status]) VALUES (1,'InProgress')	END	ELSE IF EXISTS (SELECT 'X' FROM [Analytics].[AuditStatus] WHERE [StatusId] = '1' AND([Status] <> 'InProgress'))	BEGIN	UPDATE [Analytics].[AuditStatus] SET [Status] = 'InProgress' WHERE [StatusId] = '1'	END
IF NOT EXISTS (SELECT 'X' FROM [Analytics].[AuditStatus] WHERE [StatusId] = 2)	BEGIN	INSERT INTO [Analytics].[AuditStatus] ([StatusId],[Status]) VALUES (2,'Failed')	END	ELSE IF EXISTS (SELECT 'X' FROM [Analytics].[AuditStatus] WHERE [StatusId] = '2' AND([Status] <> 'Failed'))	BEGIN	UPDATE [Analytics].[AuditStatus] SET [Status] = 'Failed' WHERE [StatusId] = '2'	END
IF NOT EXISTS (SELECT 'X' FROM [Analytics].[AuditStatus] WHERE [StatusId] = 3)	BEGIN	INSERT INTO [Analytics].[AuditStatus] ([StatusId],[Status]) VALUES (3,'Successful')	END	ELSE IF EXISTS (SELECT 'X' FROM [Analytics].[AuditStatus] WHERE [StatusId] = '3' AND([Status] <> 'Successful'))	BEGIN	UPDATE [Analytics].[AuditStatus] SET [Status] = 'Successful' WHERE [StatusId] = '3'	END

SET IDENTITY_INSERT [Analytics].[AuditStatus] OFF
END	
