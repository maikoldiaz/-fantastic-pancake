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
	
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='0a75c2de-0900-4973-9c13-f296b47a96b2' AND Status = 1)
		BEGIN
			BEGIN TRY

				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'BackupMovementDetailsWithOwner'))
				BEGIN
					DROP VIEW [Admin].[BackupMovementDetailsWithOwner];
				END

				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('0a75c2de-0900-4973-9c13-f296b47a96b2', 1);
			END TRY
			

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('0a75c2de-0900-4973-9c13-f296b47a96b2', 0);
			END CATCH
		END
END