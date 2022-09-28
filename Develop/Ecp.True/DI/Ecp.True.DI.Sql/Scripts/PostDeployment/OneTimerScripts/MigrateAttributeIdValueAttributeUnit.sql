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

/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Feb-14-2020
-- Description:     To migrate category elements of categories - Attribute, AtrributeUnit, Operator, System Origin
-- ==============================================================================================================================*/


IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Offchain' AND  TABLE_NAME = 'Attribute'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='8447cc35-fb04-430e-8cf8-6a271d70265c' AND Status = 1)
		BEGIN
			BEGIN TRY

				DECLARE @AttributeUpdateQuery NVARCHAR (4000);
				SET @AttributeUpdateQuery = N'UPDATE Offchain.Attribute SET AttributeId = ''169'' WHERE AttributeId =  N''API'';
				UPDATE Offchain.Attribute SET AttributeId = ''167'' WHERE AttributeId = ''S'';
				UPDATE Offchain.Attribute SET AttributeId=CE.Elementid 
				FROM Offchain.Attribute Attr
				INNER JOIN Admin.CategoryElement CE
				ON CE.Name = Attr.AttributeId AND CE.CategoryId=20;
				UPDATE Offchain.Attribute SET ValueAttributeUnit = ''175'' WHERE ValueAttributeUnit = ''Grados API'';
				UPDATE Offchain.Attribute SET ValueAttributeUnit = ''178'' WHERE ValueAttributeUnit = N''Partes por Mill�n'' OR ValueAttributeUnit = N''Partes por Millon'';
				UPDATE Offchain.Attribute SET ValueAttributeUnit=CE.Elementid 
				FROM Offchain.Attribute Attr
				INNER JOIN Admin.CategoryElement CE
				ON CE.Name = Attr.ValueAttributeUnit AND CE.CategoryId=6;';
				EXEC sp_executesql @AttributeUpdateQuery

				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('8447cc35-fb04-430e-8cf8-6a271d70265c', 1, 'Post');
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('8447cc35-fb04-430e-8cf8-6a271d70265c', 0, 'Post');
			END CATCH
		END
	END
END