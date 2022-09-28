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
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND  TABLE_NAME = 'AlgorithmList'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='36d62e06-0bfd-4be0-8d08-be5e19d6c44e' AND Status = 1)
		BEGIN
			BEGIN TRY
				DROP TABLE [Admin].[AlgorithmList]
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('36d62e06-0bfd-4be0-8d08-be5e19d6c44e', 1);
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('36d62e06-0bfd-4be0-8d08-be5e19d6c44e', 0);
			END CATCH
		END
	END
END