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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='516E4E6B-5F46-4A43-A280-9208988E6C3C' AND Status = 1)
	BEGIN
		BEGIN TRY
			
			DECLARE @PTUnitsUpdate NVARCHAR(4000)
			SET @PTUnitsUpdate = N'UPDATE Admin.PendingTransaction
			SET Units =  CASE WHEN b.ElementId IS NOT NULL THEN b.ElementId ELSE NULL END,
			    LastModifiedBy = ''System''
			FROM Admin.PendingTransaction a
			LEFT JOIN Admin.CategoryElement b
			ON a.Units = b.Name
			WHERE ISNUMERIC(Units)=0;'

			IF EXISTS
			(
			SELECT 1
			FROM SYS.COLUMNS C
			JOIN SYS.TYPES T
				 ON C.USER_TYPE_ID=T.USER_TYPE_ID
			WHERE C.OBJECT_ID=OBJECT_ID('Admin.PendingTransaction')
			AND C.NAME = 'Units' AND TYPE_NAME(C.USER_TYPE_ID) LIKE '%char%'
			)
		    BEGIN
				EXEC (@PTUnitsUpdate)
			END
			
			SET @PTUnitsUpdate = N'UPDATE Admin.PendingTransaction
			SET Units =  CASE WHEN b.ElementId IS NOT NULL THEN b.ElementId ELSE NULL END,
			    LastModifiedBy = ''System''
			FROM Admin.PendingTransaction a
			LEFT JOIN Admin.CategoryElement b
			ON a.Units = b.ElementId
			WHERE ISNUMERIC(Units)=1;'

			IF EXISTS
			(
			SELECT 1
			FROM SYS.COLUMNS C
			JOIN SYS.TYPES T
				 ON C.USER_TYPE_ID=T.USER_TYPE_ID
			WHERE C.OBJECT_ID=OBJECT_ID('Admin.PendingTransaction')
			AND C.NAME = 'Units' AND TYPE_NAME(C.USER_TYPE_ID) LIKE '%char%'
			)
		    BEGIN
				EXEC (@PTUnitsUpdate)
			END		
	          

			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('516E4E6B-5F46-4A43-A280-9208988E6C3C', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('516E4E6B-5F46-4A43-A280-9208988E6C3C', 0);
		END CATCH
	END
END