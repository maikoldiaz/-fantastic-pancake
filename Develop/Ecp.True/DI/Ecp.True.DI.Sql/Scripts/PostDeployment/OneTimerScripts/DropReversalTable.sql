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
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND  TABLE_NAME = 'Reversal'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='bee66260-1a61-4821-b7ca-3159e1e06903' AND Status = 1)
		BEGIN
			BEGIN TRY
				DROP TABLE [Admin].[Reversal];
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('bee66260-1a61-4821-b7ca-3159e1e06903', 1, 'POST');
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('bee66260-1a61-4821-b7ca-3159e1e06903', 0, 'POST');
			END CATCH
		END
	END
END