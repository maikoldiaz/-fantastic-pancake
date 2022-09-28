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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='D312FDFF-7F76-4830-9B42-DAF20F65345C' AND Status = 1)
	BEGIN
		BEGIN TRY

			--Update ProductType in InventoryProduct 
			DECLARE @PTUpdateInvPrdQuery NVARCHAR(4000)
			SET @PTUpdateInvPrdQuery = N'UPDATE Offchain.InventoryProduct
										SET ProductType = NULL
										WHERE NOT EXISTS (Select 1 from Admin.CategoryElement CE where CE.ElementId=ProductType)
										AND ISNUMERIC(ProductType) = 1;
									
										UPDATE Offchain.InventoryProduct
										SET MeasurementUnit = NULL
										WHERE NOT EXISTS (Select 1 from Admin.CategoryElement CE where CE.ElementId=MeasurementUnit)
										AND ISNUMERIC(MeasurementUnit) = 1;

										UPDATE Offchain.InventoryProduct
										SET ProductType =  CASE WHEN b.ElementId IS NOT NULL THEN b.ElementId ELSE NULL END,
											LastModifiedBy = ''System''
										FROM Offchain.InventoryProduct a
										LEFT JOIN Admin.CategoryElement b
										ON a.ProductType = b.Name
										WHERE ISNUMERIC(ProductType)=0;

										UPDATE Offchain.InventoryProduct
										SET MeasurementUnit =  CASE WHEN b.ElementId IS NOT NULL THEN b.ElementId ELSE NULL END,
											LastModifiedBy = ''System''
										FROM Offchain.InventoryProduct a
										LEFT JOIN Admin.CategoryElement b
										ON a.MeasurementUnit = b.Name
										WHERE ISNUMERIC(MeasurementUnit)=0;'			
			
			EXEC (@PTUpdateInvPrdQuery)

			--Update ProductType in Movement 
			DECLARE @PTUpdateMovQuery NVARCHAR(4000)
			SET @PTUpdateMovQuery = N'UPDATE Offchain.Movement
									SET MovementTypeId = NULL
									WHERE NOT EXISTS (Select 1 from Admin.CategoryElement CE where CE.ElementId=MovementTypeId)
									AND ISNUMERIC(MovementTypeId) = 1;
									
									UPDATE Offchain.Movement
									SET MeasurementUnit = NULL
									WHERE NOT EXISTS (Select 1 from Admin.CategoryElement CE where CE.ElementId=MeasurementUnit)
									AND ISNUMERIC(MeasurementUnit) = 1;

									UPDATE Offchain.Movement
									SET MovementTypeId =  CASE WHEN b.ElementId IS NOT NULL THEN b.ElementId ELSE NULL END,
										LastModifiedBy = ''System''
									FROM Offchain.Movement a
									LEFT JOIN Admin.CategoryElement b
									ON a.MovementTypeId = b.Name
									WHERE ISNUMERIC(MovementTypeId)=0;

									UPDATE Offchain.Movement
									SET MeasurementUnit =  CASE WHEN b.ElementId IS NOT NULL THEN b.ElementId ELSE NULL END,
										LastModifiedBy = ''System''
									FROM Offchain.Movement a
									LEFT JOIN Admin.CategoryElement b
									ON a.MeasurementUnit = b.Name
									WHERE ISNUMERIC(MeasurementUnit)=0;'			

			EXEC (@PTUpdateMovQuery)

			--Update DestinationProductType in MovementDestination 
			DECLARE @PTUpdateMovDestQuery NVARCHAR(4000)
			SET @PTUpdateMovDestQuery = N'UPDATE Offchain.MovementDestination
										SET DestinationProductTypeId = NULL
									    WHERE NOT EXISTS (Select 1 from Admin.CategoryElement CE where CE.ElementId=DestinationProductTypeId)
										AND ISNUMERIC(DestinationProductTypeId) = 1;

										UPDATE Offchain.MovementDestination
										SET DestinationProductTypeId =  CASE WHEN b.ElementId IS NOT NULL THEN b.ElementId ELSE NULL END,
											LastModifiedBy = ''System''
										FROM Offchain.MovementDestination a
										LEFT JOIN Admin.CategoryElement b
										ON a.DestinationProductTypeId = b.Name
										WHERE ISNUMERIC(DestinationProductTypeId)=0;'			

			EXEC (@PTUpdateMovDestQuery)

			--Update SourceProductType in MovementSource 
			DECLARE @PTUpdateMovSrcQuery NVARCHAR(4000)
			SET @PTUpdateMovSrcQuery = N'UPDATE Offchain.MovementSource
										SET SourceProductTypeId = NULL
									    WHERE NOT EXISTS (Select 1 from Admin.CategoryElement CE where CE.ElementId=SourceProductTypeId)
										AND ISNUMERIC(SourceProductTypeId) = 1;

										UPDATE Offchain.MovementSource
										SET SourceProductTypeId =  CASE WHEN b.ElementId IS NOT NULL THEN b.ElementId ELSE NULL END,
											LastModifiedBy = ''System''
										FROM Offchain.MovementSource a
										LEFT JOIN Admin.CategoryElement b
										ON a.SourceProductTypeId = b.Name
										WHERE ISNUMERIC(SourceProductTypeId)=0;'			

			EXEC (@PTUpdateMovSrcQuery)

			--Update OwnerId in Owner 
			DECLARE @PTUpdateOwnerQuery NVARCHAR(4000)
			SET @PTUpdateOwnerQuery = N'UPDATE Offchain.Owner
										SET OwnerId = NULL
									    WHERE NOT EXISTS (Select 1 from Admin.CategoryElement CE where CE.ElementId=OwnerId)
										AND ISNUMERIC(OwnerId) = 1;

										UPDATE Offchain.Owner
										SET OwnerId =  CASE WHEN b.ElementId IS NOT NULL THEN b.ElementId ELSE NULL END,
											LastModifiedBy = ''System''
										FROM Offchain.Owner a
										LEFT JOIN Admin.CategoryElement b
										ON a.OwnerId = b.Name
										WHERE ISNUMERIC(OwnerId)=0; '			

			EXEC (@PTUpdateOwnerQuery)
	          

			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('D312FDFF-7F76-4830-9B42-DAF20F65345C', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('D312FDFF-7F76-4830-9B42-DAF20F65345C', 0);
		END CATCH
	END
END