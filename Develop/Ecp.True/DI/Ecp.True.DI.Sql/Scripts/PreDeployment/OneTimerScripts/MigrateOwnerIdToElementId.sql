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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='BE685C21-DDC0-4A1F-9798-6B08CC13BF33' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE tbl
			SET tbl.OwnerId = catel.ElementId
			FROM Offchain.Owner tbl
			INNER JOIN Admin.CategoryElement catel
				ON tbl.OwnerId = catel.Name
				WHERE catel.CategoryId = 7
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('BE685C21-DDC0-4A1F-9798-6B08CC13BF33', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('BE685C21-DDC0-4A1F-9798-6B08CC13BF33', 0);
		END CATCH
	END
END