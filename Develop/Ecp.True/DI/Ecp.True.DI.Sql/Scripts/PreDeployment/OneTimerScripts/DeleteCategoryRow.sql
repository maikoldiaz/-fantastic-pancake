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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='eca8ea10-5946-4f2b-9899-9c3828176b2e' AND Status = 1)
	BEGIN
		BEGIN TRY
			DELETE FROM [Admin].[Category] WHERE [CategoryId] = 12;
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('eca8ea10-5946-4f2b-9899-9c3828176b2e', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('eca8ea10-5946-4f2b-9899-9c3828176b2e', 0);
		END CATCH
	END
END