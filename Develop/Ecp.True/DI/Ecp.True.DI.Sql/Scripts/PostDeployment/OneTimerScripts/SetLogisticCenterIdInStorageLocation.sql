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



IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin'  AND  TABLE_NAME = 'StorageLocation'))
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='425b7b8b-e5c8-4801-acfd-da307d9a8864' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE Admin.StorageLocation SET LogisticCenterId = SUBSTRING(StorageLocationId, 0, CHARINDEX(':', StorageLocationId));
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('425b7b8b-e5c8-4801-acfd-da307d9a8864', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('425b7b8b-e5c8-4801-acfd-da307d9a8864', 0, 'POST');
		END CATCH
	END
END