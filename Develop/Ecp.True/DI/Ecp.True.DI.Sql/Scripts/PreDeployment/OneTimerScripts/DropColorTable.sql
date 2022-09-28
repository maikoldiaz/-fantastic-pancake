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
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND  TABLE_NAME = 'Color'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='99c22129-7db5-4707-8e8e-260a22d003a9' AND Status = 1)
		BEGIN
			BEGIN TRY
				DROP TABLE [Admin].[Color]
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('99c22129-7db5-4707-8e8e-260a22d003a9', 1);
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('99c22129-7db5-4707-8e8e-260a22d003a9', 0);
			END CATCH
		END
	END
END