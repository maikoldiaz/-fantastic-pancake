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
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Offchain' AND  TABLE_NAME = 'Owner'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='cee19898-6d3d-406f-a38b-55d1dac54891' AND Status = 1)
		BEGIN
			BEGIN TRY

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 44511

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 44518

                update Offchain.owner
                Set OwnershipValue = 1406.52
                Where Id = 44514

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47901

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47903

                update Offchain.owner
                Set OwnershipValue = -1406.52
                Where Id = 47905
                ---------------------------------------------------------


                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 44612

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 44615

                update Offchain.owner
                Set OwnershipValue = 1406.52
                Where Id = 44613

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47918

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47920

                update Offchain.owner
                Set OwnershipValue = -1406.52
                Where Id = 47924

                ---------------------------------------------

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 44565

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 44569

                update Offchain.owner
                Set OwnershipValue = 1406.52
                Where Id = 44568

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47922

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47926

                update Offchain.owner
                Set OwnershipValue = -1406.52
                Where Id = 47928

                ---------------------------------------------


                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46193

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46208

                update Offchain.owner
                Set OwnershipValue = 1416.71
                Where Id = 46202

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47907

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47909

                update Offchain.owner
                Set OwnershipValue = -1416.71
                Where Id = 47911

                ----------------------------------------------------------------------------------

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46224

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46234

                update Offchain.owner
                Set OwnershipValue = 1416.71
                Where Id = 46231

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47895

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47897

                update Offchain.owner
                Set OwnershipValue = -1416.71
                Where Id = 47898
                ----------------------------------------------------------------------------


                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46130

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46135

                update Offchain.owner
                Set OwnershipValue = 1416.71
                Where Id = 46133

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47906

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47908

                update Offchain.owner
                Set OwnershipValue = -1416.71
                Where Id = 47910

                ---------------------------------------------


                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46216

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46221

                update Offchain.owner
                Set OwnershipValue = 1416.71
                Where Id = 46220

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47919

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47921

                update Offchain.owner
                Set OwnershipValue = -1416.71
                Where Id = 47925
                -----------------------------------------------------------------------------


                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46110

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46113

                update Offchain.owner
                Set OwnershipValue = 1416.71
                Where Id = 46111

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47884

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47887

                update Offchain.owner
                Set OwnershipValue = -1416.71
                Where Id = 47891

                ---------------------------------------------


                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46316

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46318

                update Offchain.owner
                Set OwnershipValue = 1416.71
                Where Id = 46317

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47912

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47913

                update Offchain.owner
                Set OwnershipValue = -1416.71
                Where Id = 47914

                ---------------------------------------------

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46294

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46296

                update Offchain.owner
                Set OwnershipValue = 1079.55
                Where Id = 46295

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47885

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47886

                update Offchain.owner
                Set OwnershipValue = -1079.55
                Where Id = 47889
                ---------------------------------------------------------------------------------


                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46373

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 46375

                update Offchain.owner
                Set OwnershipValue = 1079.55
                Where Id = 46374

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47195

                update Offchain.owner
                Set OwnershipValue = 0
                Where Id = 47196

                update Offchain.owner
                Set OwnershipValue = -1079.55
                Where Id = 47197

				INSERT [Admin].[ControlScript] ([Id], [DeploymentType], [Status]) VALUES ('cee19898-6d3d-406f-a38b-55d1dac54891', 'POST', 1);
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [DeploymentType], [Status]) VALUES ('cee19898-6d3d-406f-a38b-55d1dac54891', 'POST', 0);
			END CATCH
		END
	END
END