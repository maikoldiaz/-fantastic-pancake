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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='E4F428DF-BF0B-49BC-885E-24D568B3FA6B' AND Status = 1)
	BEGIN
		BEGIN TRY
			
          -- Backup Commercial Id values along with PK into temp table
			IF (OBJECT_ID('TempDB..#Analytics_OperativeNodeRelationshipWithOwnership') IS NULL)
              BEGIN
                  -- BACKUP DATA TO A TEMP TABLE
                   SELECT * INTO #Analytics_OperativeNodeRelationshipWithOwnership 
				            FROM Analytics.OperativeNodeRelationshipWithOwnership
              END

			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('E4F428DF-BF0B-49BC-885E-24D568B3FA6B', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('E4F428DF-BF0B-49BC-885E-24D568B3FA6B', 0);
		END CATCH
	END
END