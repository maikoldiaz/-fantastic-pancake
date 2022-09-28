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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='8E9FC943-85C2-48B5-887A-D78836F51414' AND Status = 1)
	BEGIN
		BEGIN TRY

			--Update SystemName with corresponding Id
			DECLARE @PTUpdateQuery NVARCHAR(4000)
			SET @PTUpdateQuery = N'	UPDATE Admin.PendingTransaction
			SET SystemName = CASE WHEN SystemName IN (''EXCEL - OCENSA'' , ''EXCEL CENIT'', ''EXCEL SERGIO'', ''EXCEL INVENTARIOS'', ''EXCEL'', ''EXCEL SINOPER'') THEN ''164''
								WHEN SystemName IN (''SINOPER'') THEN ''166''
								WHEN SystemName IN (''1611234567890123456789'',''16112345678901234567893'') THEN NULL
								WHEN CE.ElementId IS NULL THEN SystemName
								ELSE CE.ElementId END
			FROM Admin.PendingTransaction PT
			LEFT JOIN Admin.CategoryElement CE
			ON CE.Name = PT.SystemName'
			
			IF EXISTS
			(
			SELECT 1
			FROM SYS.COLUMNS C
			JOIN SYS.TYPES T
				 ON C.USER_TYPE_ID=T.USER_TYPE_ID
			WHERE C.OBJECT_ID=OBJECT_ID('Admin.PendingTransaction')
			AND C.NAME = 'SystemName' AND TYPE_NAME(C.USER_TYPE_ID) LIKE '%char%'
			)
			BEGIN
				EXEC (@PTUpdateQuery)			
			END

	          

			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('8E9FC943-85C2-48B5-887A-D78836F51414', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('8E9FC943-85C2-48B5-887A-D78836F51414', 0);
		END CATCH
	END
END