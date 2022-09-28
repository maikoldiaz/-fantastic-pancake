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
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND  TABLE_NAME = 'Unbalance'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='273a33c5-33e1-405d-9ae7-2bd6c27819b0' AND Status = 1)
		BEGIN
			BEGIN TRY
				
                UPDATE [Admin].[CategoryElement]
                SET IsActive = 0
                WHERE ElementId = 1

				INSERT [Admin].[ControlScript] ([Id], [DeploymentType], [Status]) VALUES ('273a33c5-33e1-405d-9ae7-2bd6c27819b0', 'POST', 1);
				
				SELECT TOP 10 * FROM [Admin].[CategoryElement]
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [DeploymentType], [Status]) VALUES ('273a33c5-33e1-405d-9ae7-2bd6c27819b0', 'POST', 0);
			END CATCH
		END
	END
END