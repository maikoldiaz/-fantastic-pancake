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


IF EXISTS (Select 'X' from sys.schemas Where name = 'Audit')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='7621659f-7f56-462b-9cf2-d7fbea9be0c7' AND Status = 1)
	BEGIN
		BEGIN TRY
			EXEC [Admin].[usp_SaveBackupMovementDetailsWithOwner] -1
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('7d523cd4-b43e-4998-9ba0-d8956a0d8c55', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('7d523cd4-b43e-4998-9ba0-d8956a0d8c55', 0, 'POST');
		END CATCH
	END
END