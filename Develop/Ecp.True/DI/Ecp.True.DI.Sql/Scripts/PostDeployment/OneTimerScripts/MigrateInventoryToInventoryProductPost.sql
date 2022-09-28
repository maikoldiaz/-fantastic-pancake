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
	IF (OBJECT_ID('TempDB..#Offchain_Inventory') IS NOT NULL) 
	BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='AEF794D7-2FD2-4132-B80B-2F8DB4B8B264' AND Status = 1)
	BEGIN
		BEGIN TRY
			DECLARE @InventoryproductUpdateQuery NVARCHAR (4000)
	                  SET @InventoryproductUpdateQuery = N'Update InvPrd 
										SET  InvPrd.SystemTypeId = Inv.SystemTypeId
											,InvPrd.SystemName = Inv.SystemName
											,InvPrd.SourceSystem = Inv.SourceSystem
											,InvPrd.DestinationSystem = Inv.DestinationSystem
											,InvPrd.EventType = Inv.EventType
											,InvPrd.TankName = Inv.TankName
											,InvPrd.InventoryId = Inv.InventoryId
											,InvPrd.TicketId = Inv.TicketId
											,InvPrd.InventoryDate = Inv.InventoryDate
											,InvPrd.NodeId = Inv.NodeId
											,InvPrd.SegmentId = Inv.SegmentId
											,InvPrd.Observations = Inv.Observations
											,InvPrd.Scenario = Inv.Scenario
											,InvPrd.IsDeleted = Inv.IsDeleted
											,InvPrd.FileRegistrationTransactionId = Inv.FileRegistrationTransactionId
											,InvPrd.Operator = Inv.Operator
										FROM
										Offchain.InventoryProduct InvPrd
										Inner Join #Offchain_InventoryProduct tempInvProd
										On tempInvProd.InventoryProductId = InvPrd.InventoryProductId
										Inner Join #Offchain_Inventory Inv
										On tempInvProd.InventoryPrimaryKeyId = Inv.InventoryTransactionId;

										Update Offchain.InventoryProduct SET InventoryProductUniqueId = InventoryProductId;'

	            EXEC sp_executesql @InventoryproductUpdateQuery

				Delete From Admin.Attribute Where InventoryProductId IN (Select InventoryProductId from Offchain.InventoryProduct Where NodeId = 0);
				Delete From Offchain.Owner Where InventoryProductId IN (Select InventoryProductId from Offchain.InventoryProduct Where NodeId = 0);
				Delete from Offchain.InventoryProduct Where NodeId = 0;

				IF OBJECT_ID('tempdb..#Offchain_InventoryProduct')IS NOT NULL
					DROP TABLE #Offchain_InventoryProduct
				IF OBJECT_ID('tempdb..#Offchain_Inventory')IS NOT NULL
					DROP TABLE #Offchain_Inventory

			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('AEF794D7-2FD2-4132-B80B-2F8DB4B8B264', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('AEF794D7-2FD2-4132-B80B-2F8DB4B8B264', 0, 'POST');
		END CATCH
	END
	END
END