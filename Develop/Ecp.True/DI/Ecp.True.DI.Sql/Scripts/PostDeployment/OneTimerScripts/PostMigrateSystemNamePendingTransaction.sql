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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='48DFCD78-F231-4636-94D8-81FDBF292F9B' AND Status = 1)
	BEGIN
		BEGIN TRY

			--Update SystemName with corresponding Id
			DECLARE @PTUpdateQuery NVARCHAR(4000)
			SET @PTUpdateQuery = N'	UPDATE PT
			SET PT.SystemName = CASE WHEN CE.ElementId IS NULL THEN PT.SystemName ELSE CE.ElementId END
			FROM Admin.PendingTransaction PT
			LEFT JOIN Admin.CategoryElement CE
			ON CE.Name = CASE WHEN PT.SystemName IN (''EXCEL - OCENSA'' , ''EXCEL CENIT'') THEN ''EXCEL'' ELSE PT.SystemName END'
			EXEC (@PTUpdateQuery)
	          

			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('48DFCD78-F231-4636-94D8-81FDBF292F9B', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('48DFCD78-F231-4636-94D8-81FDBF292F9B', 0, 'POST');
		END CATCH
	END
END