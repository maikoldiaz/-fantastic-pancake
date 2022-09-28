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
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND  TABLE_NAME = 'Contract'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='20560CAD-3BBC-422F-A826-D6ABB548D56F' AND Status = 1)
		BEGIN
			BEGIN TRY
				  update [Admin].[Contract] set Frequency = 'Diario'
				  update [Admin].[Contract] set SourceSystem = 'Excel'
				  update [Admin].[Contract] set PurchaseOrderType = null
				  update [Admin].[Contract] set Status = 'Activo'
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('20560CAD-3BBC-422F-A826-D6ABB548D56F', 1, 'POST');
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('20560CAD-3BBC-422F-A826-D6ABB548D56F', 0, 'POST');
			END CATCH
		END
	END
END