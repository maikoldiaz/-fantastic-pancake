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
IF	OBJECT_ID('Admin.Node') IS NOT NULL
BEGIN

    IF EXISTS (SELECT IsExportation FROM [Admin].[Node] WHERE IsExportation IS NULL )	
        BEGIN	
        UPDATE  [Admin].[Node] 
        SET IsExportation = 0
        WHERE IsExportation IS NULL
    END

END	