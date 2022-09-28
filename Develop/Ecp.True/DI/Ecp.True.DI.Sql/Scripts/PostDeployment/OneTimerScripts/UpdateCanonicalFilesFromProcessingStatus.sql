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
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND  TABLE_NAME = 'FileRegistration'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='cd881a3d-073f-4235-bb82-756d0ae1ab0e' AND Status = 1)
		BEGIN
			BEGIN TRY
				
                UPDATE [Admin].[FileRegistration]
                SET STATUS = 2, IsParsed = 0
                WHERE FileRegistrationId IN (136501, 136498, 136497, 136492)

				INSERT [Admin].[ControlScript] ([Id], [DeploymentType], [Status]) VALUES ('cd881a3d-073f-4235-bb82-756d0ae1ab0e', 'POST', 1);
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [DeploymentType], [Status]) VALUES ('cd881a3d-073f-4235-bb82-756d0ae1ab0e', 'POST', 0);
			END CATCH
		END
	END
END