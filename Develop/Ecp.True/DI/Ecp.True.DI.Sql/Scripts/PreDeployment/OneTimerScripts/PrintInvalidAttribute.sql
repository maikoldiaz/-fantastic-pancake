/*
 Post-Deployment Script Template							
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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='d7884e26-f441-42df-abff-c7a1b7041f05' AND Status = 1)
	BEGIN
		BEGIN TRY
			DECLARE @txt NVARCHAR(MAX)
			IF EXISTS (SELECT 'X' FROM [Offchain].[Attribute] WHERE ISNUMERIC(AttributeId) = 0 OR ISNUMERIC(ValueAttributeUnit) = 0)
			BEGIN
				PRINT 'Non-numeric AttributeId, ValueAttribute rows in Attribute table'
				SET @txt = 'Id, AttributeId, ValueAttributeUnit' + CHAR(13)
				SELECT @txt = @txt +CAST(Id AS NVARCHAR) + ',' + CAST(AttributeId AS NVARCHAR) + ',' + CAST(ValueAttributeUnit AS NVARCHAR) + CHAR(13) FROM [Offchain].[Attribute] WHERE ISNUMERIC(AttributeId) = 0 OR ISNUMERIC(ValueAttributeUnit) = 0
				PRINT @txt
			END
			ELSE
			BEGIN
				PRINT 'No non-numeric AttributeId, ValueAttribute rows found in Attribute table.'
			END
			IF OBJECT_ID('tempdb..#TempAttributes') IS NOT NULL
				DROP TABLE #TempAttributes
			SELECT Att.* INTO #TempAttributes
			FROM [Offchain].Attribute Att
			INNER JOIN [Admin].CategoryElement CE
			ON CE.ElementId = Att.ValueAttributeUnit
			WHERE CE.CategoryId = 21
			IF EXISTS (SELECT 'X' FROM #TempAttributes)
			BEGIN
				PRINT 'ValueAttributeUnits of Unidades Atributos category in Attribute table'
				SET @txt = 'Id, ValueAttributeUnit' + CHAR(13)
				SELECT @txt = @txt + CAST(Id AS NVARCHAR) + ',' + CAST(ValueAttributeUnit AS NVARCHAR) + CHAR(13) FROM #TempAttributes
				PRINT @txt
			END
			ELSE
			BEGIN
				PRINT 'No valueattributeunits of Unidades Atributos category found in Attribute table.'
			END
			IF OBJECT_ID('tempdb..#TempAttributes') IS NOT NULL
				DROP TABLE #TempAttributes
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('d7884e26-f441-42df-abff-c7a1b7041f05', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('d7884e26-f441-42df-abff-c7a1b7041f05', 0);
		END CATCH
	END
END