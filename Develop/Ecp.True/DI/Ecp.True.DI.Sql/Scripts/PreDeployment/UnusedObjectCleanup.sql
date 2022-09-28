/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/



--------------------- UnUsed Tables Cleanup  - START ------------------------------------------


IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Admin' 
                 AND  TABLE_NAME = 'CutOffInformation'))
BEGIN
	PRINT 'Dropping TABLE [Admin].[CutOffInformation]'
	DROP TABLE [Admin].[CutOffInformation]
END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Admin' 
                 AND  TABLE_NAME = 'NodeCategoryGroup'))
BEGIN
	PRINT 'Dropping TABLE [Admin].[NodeCategoryGroup]'
	DROP TABLE [Admin].[NodeCategoryGroup]
END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Admin' 
                 AND  TABLE_NAME = 'OperationalCutOff'))
BEGIN
	PRINT 'Dropping TABLE [Admin].[OperationalCutOff]'
	DROP TABLE [Admin].[OperationalCutOff]
END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Admin' 
                 AND  TABLE_NAME = 'OperationalCutOffDate'))
BEGIN
	PRINT 'Dropping TABLE [Admin].[OperationalCutOffDate]'
	DROP TABLE [Admin].[OperationalCutOffDate]
END

IF EXISTS(select * FROM sys.views where name = 'view_NodeCategoryGroupWithCategoryId')
BEGIN
	PRINT 'Dropping View [Admin].[view_NodeCategoryGroupWithCategoryId]'
	DROP View [Admin].[view_NodeCategoryGroupWithCategoryId]
END

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Admin].[usp_NodeCategoryGroup]'))
BEGIN
	PRINT 'Dropping Procedure [Admin].[usp_NodeCategoryGroup]'
	DROP Procedure [Admin].[usp_NodeCategoryGroup]
END


IF EXISTS (SELECT *
           FROM   sys.objects
           WHERE  object_id = OBJECT_ID(N'[Admin].[func_NodeCategoryGroupWithCategoryId]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))
BEGIN
	PRINT 'Dropping FUNCTION [Admin].[func_NodeCategoryGroupWithCategoryId]'
	DROP FUNCTION [Admin].[func_NodeCategoryGroupWithCategoryId]
END


IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'NodeCategoryGroupType')
BEGIN
	PRINT 'Dropping TYPE [Admin].[NodeCategoryGroupType]'
	DROP TYPE [Admin].[NodeCategoryGroupType]
END

IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'NodeTagCategoryType')
BEGIN
	PRINT 'Dropping TYPE [Admin].[NodeTagCategoryType]'
	DROP TYPE [Admin].[NodeTagCategoryType]
END

		------------- Deletion of UnUsed Admin Tables -----------------


IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Admin' 
                 AND  TABLE_NAME = 'Attribute'))
BEGIN
	PRINT 'Dropping TABLE [Admin].[Attribute]'
	DROP TABLE [Admin].[Attribute]
END



IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Admin' 
                 AND  TABLE_NAME = 'Owner'))
BEGIN
	PRINT 'Dropping TABLE [Admin].[Owner]'
	DROP TABLE [Admin].[Owner]
END


IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Admin' 
                 AND  TABLE_NAME = 'GroupType'))
BEGIN
	PRINT 'Dropping TABLE [Admin].[GroupType]'
	DROP TABLE [Admin].[GroupType]
END



IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Admin' 
                 AND  TABLE_NAME = 'InventoryProduct'))
BEGIN
	PRINT 'Dropping TABLE [Admin].[InventoryProduct]'
	DROP TABLE [Admin].[InventoryProduct]
END


IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Admin' 
                 AND  TABLE_NAME = 'Inventory'))
BEGIN
	PRINT 'Dropping TABLE [Admin].[Inventory]'
	DROP TABLE [Admin].[Inventory]
END



IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Admin' 
                 AND  TABLE_NAME = 'MovementDestination'))
BEGIN
	PRINT 'Dropping TABLE [Admin].[MovementDestination]'
	DROP TABLE [Admin].[MovementDestination]
END


IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Admin' 
                 AND  TABLE_NAME = 'MovementPeriod'))
BEGIN
	PRINT 'Dropping TABLE [Admin].[MovementPeriod]'
	DROP TABLE [Admin].[MovementPeriod]
END


IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Admin' 
                 AND  TABLE_NAME = 'MovementSource'))
BEGIN
	PRINT 'Dropping TABLE [Admin].[MovementSource]'
	DROP TABLE [Admin].[MovementSource]
END



IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Admin' 
                 AND  TABLE_NAME = 'Movement'))
BEGIN
	PRINT 'Dropping TABLE [Admin].[Movement]'
	DROP TABLE [Admin].[Movement]
END

--------------------- UnUsed Tables Cleanup  - END ------------------------------------------


---------------------UnUsed Stored Procedure Cleanup  - Start--------------------------------
IF EXISTS( SELECT 'X'
		   FROM INFORMATION_SCHEMA.ROUTINES
		   WHERE SPECIFIC_NAME = 'usp_SaveCutOffInformation'
		 )
BEGIN
		DROP PROCEDURE [Admin].usp_SaveCutOffInformation
END
---------------------UnUsed Stored Procedure Cleanup  - End--------------------------------



---------------------UnUsed Stored Procedure Cleanup  - Start--------------------------------
IF EXISTS( SELECT 'X'
		   FROM INFORMATION_SCHEMA.ROUTINES
		   WHERE SPECIFIC_NAME = 'usp_SaveNode'
		 )
BEGIN
		DROP PROCEDURE [Admin].usp_SaveNode
END
---------------------UnUsed Stored Procedure Cleanup  - End--------------------------------


---------------------UnUsed Stored Procedure Cleanup  - Start--------------------------------
IF EXISTS( SELECT 'X'
		   FROM INFORMATION_SCHEMA.ROUTINES
		   WHERE SPECIFIC_NAME = 'usp_SaveHomologation'
		 )
BEGIN
		DROP PROCEDURE [Admin].usp_SaveHomologation
END
---------------------UnUsed Stored Procedure Cleanup  - End--------------------------------

---------------------UnUsed Stored Procedure Cleanup  - Start--------------------------------
IF EXISTS( SELECT 'X'
		   FROM INFORMATION_SCHEMA.ROUTINES
		   WHERE SPECIFIC_NAME = 'usp_GetExcelDataSetMaster'
		 )
BEGIN
		DROP PROCEDURE [Admin].usp_GetExcelDataSetMaster
END
---------------------UnUsed Stored Procedure Cleanup  - End--------------------------------


-------------------------- UnUsed View Cleanup  - Start----------------------------------
IF EXISTS(select * FROM sys.views where name = 'view_UnablanceOutput')
BEGIN
	PRINT 'Dropping View [Admin].[view_UnablanceOutput]'
	DROP View [Admin].[view_UnablanceOutput]
END
-------------------------- UnUsed View Cleanup  - End----------------------------------