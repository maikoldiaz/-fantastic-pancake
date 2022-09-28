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

IF EXISTS (Select 'X' from sys.schemas Where name = 'Audit')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='252d3e61-2d88-4047-a643-7c6877851927' AND Status = 1)
	BEGIN
		BEGIN TRY
			IF (OBJECT_ID('TempDB..#Audit_AuditLog') IS NULL)
            BEGIN
				-- Backup AuditLogs in temp table
				SELECT * INTO #Audit_AuditLog FROM [Audit].[AuditLog];
            END
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('252d3e61-2d88-4047-a643-7c6877851927', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('252d3e61-2d88-4047-a643-7c6877851927', 0);
		END CATCH
	END
END