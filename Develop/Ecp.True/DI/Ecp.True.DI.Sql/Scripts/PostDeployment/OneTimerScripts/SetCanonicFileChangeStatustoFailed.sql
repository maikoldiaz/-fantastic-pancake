/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='9CCFBF73-81B0-4584-B241-A7A5951CBF7D' AND Status = 1)
		BEGIN
			BEGIN TRY

				UPDATE [Admin].FileRegistration 
				SET IsParsed = 0
				where FileRegistrationId in (111056,111057,111058,111059,111153,111154,111155,111156,111157,111158,111159,111622,111743,111880)

				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('9CCFBF73-81B0-4584-B241-A7A5951CBF7D', 1, 'POST');
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('9CCFBF73-81B0-4584-B241-A7A5951CBF7D', 0, 'POST');
			END CATCH
		END
END