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

IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='3cd010ba-1010-4ac2-b220-1f9dda45d05a' AND Status = 1)
	BEGIN
		BEGIN TRY
			
          -- Backup Commercial Id values along with PK into temp table
			IF (OBJECT_ID('TempDB..#Admin_Event') IS NULL)
              BEGIN
                  -- BACKUP DATA TO A TEMP TABLE
                  SELECT * INTO #Admin_Event FROM [Admin].[Event]       
              END
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('3cd010ba-1010-4ac2-b220-1f9dda45d05a', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('3cd010ba-1010-4ac2-b220-1f9dda45d05a', 0);
		END CATCH
	END
END