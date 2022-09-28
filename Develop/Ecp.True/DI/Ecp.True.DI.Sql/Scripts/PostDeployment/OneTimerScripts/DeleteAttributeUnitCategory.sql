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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='3fdaece4-77c4-4bf2-9d44-475a29068c12' AND Status = 1)
	BEGIN
		BEGIN TRY
			DELETE FROM Admin.Category WHERE CategoryId=21
			INSERT [Admin].[ControlScript] ([Id], [DeploymentType], [Status]) VALUES ('3fdaece4-77c4-4bf2-9d44-475a29068c12', 'POST', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [DeploymentType], [Status]) VALUES ('3fdaece4-77c4-4bf2-9d44-475a29068c12', 'POST', 0);
		END CATCH
	END
END