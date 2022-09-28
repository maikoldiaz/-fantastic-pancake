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

IF	OBJECT_ID('Admin.KPIRelationship') IS NOT NULL
BEGIN

-- Clearing any junk data from the reserved space
--DELETE FROM [Admin].[RegisterFileActionType] WHERE [ActionTypeId] > 4 AND [ActionTypeId] < 101

-- Inserting Seed data with Identity
--SET IDENTITY_INSERT [Admin].[RegisterFileActionType] ON 
 DECLARE @Todaysdate	   DATETIME =  [Admin].[udf_GetTrueDate] ();

IF NOT EXISTS (SELECT 'X' FROM [Admin].[KPIRelationship] WHERE [OrderToDisplay] = 1)	BEGIN	INSERT [Admin].[KPIRelationship] (OrderToDisplay, Indicator, [CreatedBy], [CreatedDate]) VALUES (1, N'PÉRDIDAS IDENTIFICADAS', N'System', @Todaysdate)	END	    ELSE IF EXISTS (SELECT 'X' FROM [Admin].[KPIRelationship]	WHERE [OrderToDisplay] = '1' AND (Indicator <> N'PÉRDIDAS IDENTIFICADAS'   ))	BEGIN	UPDATE [Admin].[KPIRelationship] SET Indicator = N'PÉRDIDAS IDENTIFICADAS'     WHERE [OrderToDisplay] = '1'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[KPIRelationship] WHERE [OrderToDisplay] = 2)	BEGIN	INSERT [Admin].[KPIRelationship] (OrderToDisplay, Indicator, [CreatedBy], [CreatedDate]) VALUES (2, N'INTERFASES', N'System', @Todaysdate)	END	                ELSE IF EXISTS (SELECT 'X' FROM [Admin].[KPIRelationship]	WHERE [OrderToDisplay] = '2' AND (Indicator <> N'INTERFASES'               ))	BEGIN	UPDATE [Admin].[KPIRelationship] SET Indicator = N'INTERFASES'                 WHERE [OrderToDisplay] = '2'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[KPIRelationship] WHERE [OrderToDisplay] = 3)	BEGIN	INSERT [Admin].[KPIRelationship] (OrderToDisplay, Indicator, [CreatedBy], [CreatedDate]) VALUES (3, N'TOLERANCIA', N'System', @Todaysdate)	END	                ELSE IF EXISTS (SELECT 'X' FROM [Admin].[KPIRelationship]	WHERE [OrderToDisplay] = '3' AND (Indicator <> N'TOLERANCIA'               ))	BEGIN	UPDATE [Admin].[KPIRelationship] SET Indicator = N'TOLERANCIA'                 WHERE [OrderToDisplay] = '3'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[KPIRelationship] WHERE [OrderToDisplay] = 4)	BEGIN	INSERT [Admin].[KPIRelationship] (OrderToDisplay, Indicator, [CreatedBy], [CreatedDate]) VALUES (4, N'PÉRDIDAS NO IDENTIFICADAS', N'System', @Todaysdate)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[KPIRelationship]	WHERE [OrderToDisplay] = '4' AND (Indicator <> N'PÉRDIDAS NO IDENTIFICADAS'))	BEGIN	UPDATE [Admin].[KPIRelationship] SET Indicator = N'PÉRDIDAS NO IDENTIFICADAS'  WHERE [OrderToDisplay] = '4'	END


SET IDENTITY_INSERT [Admin].[RegisterFileActionType] OFF
END	
