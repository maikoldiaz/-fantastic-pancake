-- ======================================================================================================================
-- Author: Microsoft
-- Created date: Jan-11-2019
-- Updated date: Mar-20-2020
--				 Apr-09-2020  -- Removed(BlockchainStatus = 1)
--               May-12-2020  -- Converting InventoryDate to DATE
--               Jun-11-2020  -- Added GrossStandardQuantity as per PBI 31874
--               Jun-15-2020  -- Added ScenarioId as per PBI 31874
-- Updated date: Jun-25-2020  -- SourceSystemName homologated into category Element. Same implemented to get the name from categoryelement based on sourcesystemId
--                            -- Updated systemname column mapping to sourcesystem from category element table
--               Jul-07-2020  -- Added Null check condition for sourcesystem column
--				 Jul-10-2020  -- Added InvPrd.[Version]
--				 Jul-22-2020  -- Added join to get systemname using systemid	
-- <Description>: This View is to Fetch Data [Admin].[view_InventoryInformation] For PowerBi Report From Tables(Inventory, InventoryProduct, Attribute, Unbalance, Ownership, Ticket, Product, Node, CategoryElement,Category)</Description>
-- =========================================================================================================================

CREATE VIEW [Admin].[view_InventoryInformation]
AS
SELECT  DISTINCT
		InvPrd.InventoryProductId AS InventoryTransactionId
	   ,SysType.[Name] AS SystemTypeName
	   ,InvPrd.SystemTypeId
	   ,ISNULL(SrcType.[Name],'')  AS SourceSystem
	   ,ISNULL(SystName.[Name], '') AS SystemName
	   ,InvPrd.DestinationSystem
	   ,InvPrd.EventType
	   ,InvPrd.TankName
	   ,InvPrd.InventoryId
	   ,InvPrd.TicketId
	   ,InvPrd.DeltaTicketId
	   ,InvPrd.InventoryProductUniqueId
	   ,InvPrd.InventoryDate AS InventoryDate
	   ,InvPrd.NodeId
	   ,ND.[Name] AS NodeName
	   ,Nd.SendToSAP AS NodeSendToSAP
	   ,InvPrd.SegmentId
	   ,CatEle.[Name] AS SegmentName
	   ,InvPrd.IsDeleted
	   ,InvPrd.ProductId
	   ,Prd.[Name] AS ProductName
	   ,ProductType.[Name] AS ProductType
	   ,InvPrd.ProductVolume
       ,CEUnits.[Name] AS MeasurmentUnit
	   ,InvPrd.UncertaintyPercentage
	   ,InvPrd.OwnershipTicketId
	   ,InvPrd.ReasonId
	   ,InvPrd.BlockchainStatus
	   ,InvPrd.InventoryProductId
	   ,InvPrd.MeasurementUnit
	   ,InvPrd.[LastModifiedDate]
	   ,InvPrd.[CreatedDate]
	   ,InvPrd.[LastModifiedBy]
	   ,InvPrd.Comment
	   ,InvPrd.BatchId
	   ,InvPrd.GrossStandardQuantity
	   ,InvPrd.ScenarioId
	   ,InvPrd.SourceSystemId
	   ,InvPrd.[Version]
FROM Offchain.InventoryProduct InvPrd
INNER JOIN [Admin].[SystemType] SysType
ON SysType.SystemTypeId = InvPrd.SystemTypeId
INNER JOIN [Admin].[Node] ND
ON ND.NodeId = InvPrd.NodeId
INNER JOIN [Admin].[CategoryElement] CatEle
ON CatEle.ElementId = InvPrd.SegmentId
INNER JOIN [Admin].Product Prd
ON Prd.ProductId = InvPrd.ProductId
INNER JOIN [Admin].[CategoryElement] CEUnits
ON CEUnits.ElementId = InvPrd.MeasurementUnit
LEFT JOIN [Admin].[CategoryElement] ProductType
ON ProductType.ElementId = InvPrd.ProductType
LEFT JOIN [Admin].[CategoryElement] SrcType
ON InvPrd.SourceSystemId = SrcType.ElementId
LEFT JOIN [Admin].[CategoryElement] SystName
ON SystName.ElementId = InvPrd.SystemId

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data [Admin].[view_InventoryInformation] For PowerBi Report From Tables(Inventory, InventoryProduct, Attribute, Unbalance, Ownership, Ticket, Product, Node, CategoryElement,Category)',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_InventoryInformation',
    @level2type = NULL,
    @level2name = NULL