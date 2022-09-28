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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='ea26ded9-ecb9-40a9-b050-a249b163cd3f' AND Status = 1)
	BEGIN
		BEGIN TRY
			EXEC [Admin].[usp_Cleanup_OperationalDataWithoutCutOff] @Hour=0;
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('ea26ded9-ecb9-40a9-b050-a249b163cd3f', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('ea26ded9-ecb9-40a9-b050-a249b163cd3f', 0, 'POST');
		END CATCH
	END
END