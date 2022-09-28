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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='ACB5EE8F-A18A-402A-833A-19C9728F20C3' AND Status = 1)
	BEGIN
		BEGIN TRY
			
			UPDATE tbl
			SET tbl.ProductType = catel.ElementId
			FROM Offchain.InventoryProduct tbl
			INNER JOIN Admin.CategoryElement catel
				ON tbl.ProductType = catel.Name
				WHERE catel.CategoryId = 11


			UPDATE tbl
			SET tbl.DestinationProductTypeId = catel.ElementId
			FROM Offchain.MovementDestination tbl
			INNER JOIN Admin.CategoryElement catel
				ON tbl.DestinationProductTypeId = catel.Name
				WHERE catel.CategoryId = 11


			UPDATE tbl
			SET tbl.SourceProductTypeId = catel.ElementId
			FROM Offchain.MovementSource tbl
			INNER JOIN Admin.CategoryElement catel
				ON tbl.SourceProductTypeId = catel.Name
				WHERE catel.CategoryId = 11

			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('ACB5EE8F-A18A-402A-833A-19C9728F20C3', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('ACB5EE8F-A18A-402A-833A-19C9728F20C3', 0);
		END CATCH
	END
END