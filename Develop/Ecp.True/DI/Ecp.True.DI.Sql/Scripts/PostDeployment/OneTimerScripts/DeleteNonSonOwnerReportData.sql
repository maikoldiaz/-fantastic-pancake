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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='472f3241-4f67-4dc3-93c7-964927e3c442' AND Status = 1)
	BEGIN
		BEGIN TRY
			EXEC [Admin].[usp_Cleanup_NonSonSegmentDataWithoutCutOff] @Hour=0;
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('472f3241-4f67-4dc3-93c7-964927e3c442', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('472f3241-4f67-4dc3-93c7-964927e3c442', 0, 'POST');
		END CATCH
	END
END