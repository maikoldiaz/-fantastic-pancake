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

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin'  AND  TABLE_NAME in ('LogisticCenter','StorageLocation')))
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='FDB1641D-4897-40F3-944A-12F4FBDB0CD8' AND Status = 1)
	BEGIN
		BEGIN TRY			
			 --Se agrega Centro logistico
			 INSERT INTO [Admin].[LogisticCenter] ([LogisticCenterId],[Name],[IsActive],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
			 VALUES (1134,'PR EL MORRO-ARAGUANEY-EMA',-1,'System','2022-03-14 00:00:00.000','System','2022-03-14 00:00:00.000')

			 INSERT INTO [Admin].[StorageLocation] ([StorageLocationId],[Name],[IsActive],[LogisticCenterId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
			 VALUES ('1134:F001','PR EL MORRO-ARAGUANEY-EMA: INV TRANSITO',-1,1134,'System','2022-03-14 00:00:00.000','System','2022-03-14 00:00:00.000')


			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('FDB1641D-4897-40F3-944A-12F4FBDB0CD8', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('FDB1641D-4897-40F3-944A-12F4FBDB0CD8', 0, 'POST');
		END CATCH
	END
END