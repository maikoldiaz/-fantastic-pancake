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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='625af68a-83f4-435e-8d06-91f92bbaa029' AND Status = 1)
	BEGIN
		BEGIN TRY

			ALTER DATABASE CURRENT
			MODIFY FILE (NAME = 'data_0', FILEGROWTH = 50240MB);
			ALTER DATABASE CURRENT
			MODIFY FILE (NAME = 'log', FILEGROWTH = 50240MB);

			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('625af68a-83f4-435e-8d06-91f92bbaa029', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('625af68a-83f4-435e-8d06-91f92bbaa029', 0);
		END CATCH
	END
END