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
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'ModelEvaluationProperty'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='0e3c22ce-5664-4cba-b7f5-a69bd254133e' AND Status = 1)
		BEGIN
			BEGIN TRY
				DROP VIEW [Admin].[ModelEvaluationProperty];
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('0e3c22ce-5664-4cba-b7f5-a69bd254133e', 1);
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('0e3c22ce-5664-4cba-b7f5-a69bd254133e', 0);
			END CATCH
		END
	END
END