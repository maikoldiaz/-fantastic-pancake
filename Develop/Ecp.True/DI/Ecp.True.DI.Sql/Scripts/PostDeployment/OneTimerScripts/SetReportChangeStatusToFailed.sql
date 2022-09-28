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
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='489D9184-2308-4409-8035-F0EEF34DB0AE' AND Status = 1)
		BEGIN
			BEGIN TRY

				UPDATE admin.ReportExecution 
				SET StatusTypeId = 2
				WHERE ExecutionId in (10531,10530,10525,10524,10529,10522,10523,10526,10527,10528,10532)

				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('489D9184-2308-4409-8035-F0EEF34DB0AE', 1, 'POST');
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('489D9184-2308-4409-8035-F0EEF34DB0AE', 0, 'POST');
			END CATCH
		END
END