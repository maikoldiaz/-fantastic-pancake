/*-- =================================================================================================
-- Author:		Microsoft
-- Created date: Oct-25-2019
-- Updated date: Mar-20-2020
--				 Apr-09-2020  -- Removed(BlockchainStatus = 1)   
--				 Jun-29-2021  -- Add NodeOrigin and NodeDestination columns
-- <Description>:	This View is to Fetch view_CalculationErrors Data </Description>:
-- ===================================================================================================*/

CREATE VIEW [Admin].[view_CalculationErrors]
AS
SELECT DISTINCT
		OwnErr.OwnershipNodeErrorId AS [OperationId]
		,'Inventory' AS [Type]
		,OwnNode.OwnershipNodeId -- Additional column for filtering purpose
        ,NULL AS [Operation]
        ,Inv.InventoryDate As [OperationDate]
        ,OwnErr.ExecutionDate As [ExecutionDate]
        ,CE.Name As [Segment]
        ,Inv.ProductVolume As [NetVolume]
        ,SrcProd.Name AS [ProductOrigin]
        ,NULL AS [ProductDestination]
		,SrcNode.Name AS [NodeOrigin]
		,NULL AS [NodeDestination]
        ,OwnErr.[ErrorMessage] AS [ErrorMessage]
FROM [Admin].[OwnershipNodeError] OwnErr
INNER JOIN [Admin].[view_InventoryInformation] Inv
ON OwnErr.InventoryProductId = Inv.InventoryProductId
INNER JOIN [Admin].[Product] SrcProd
ON SrcProd.ProductId = Inv.ProductId
INNER JOIN [Admin].[Node] SrcNode
ON SrcNode.NodeId = Inv.NodeId
INNER JOIN [Admin].[OwnershipNode] OwnNode
ON OwnNode.OwnershipNodeId = OwnErr.OwnershipNodeId
INNER JOIN [Admin].[CategoryElement] CE
ON CE.ElementId = Inv.SegmentId
UNION
SELECT DISTINCT
		OwnErr.OwnershipNodeErrorId AS [OperationId]
		,'Movement' AS [Type]
		,OwnNode.OwnershipNodeId  -- Additional column for filtering purpose
        ,Mov.EventType AS [Operation]
        ,Mov.OperationalDate As [OperationDate]
        ,OwnErr.ExecutionDate As [ExecutionDate]
        ,CE.Name As [Segment]
        ,Mov.NetStandardVolume As [NetVolume]
        ,SrcProd.Name AS [ProductOrigin]
        ,DestProd.Name AS [ProductDestination]
		,SrcNode.Name AS [NodeOrigin]
		,DestNode.Name AS [NodeDestination]
        ,OwnErr.[ErrorMessage] AS [ErrorMessage]
FROM [Admin].[OwnershipNodeError] OwnErr
INNER JOIN [Offchain].[Movement] Mov
	ON OwnErr.MovementTransactionId = Mov.MovementTransactionId
LEFT JOIN [Offchain].[MovementSource] MovSrc
	ON Mov.MovementTransactionId = MovSrc.MovementTransactionId
LEFT JOIN [Offchain].[MovementDestination] MovDest
	ON Mov.MovementTransactionId = MovDest.MovementTransactionId
LEFT JOIN [Admin].[Product] SrcProd
	ON SrcProd.ProductId = MovSrc.SourceProductId
LEFT JOIN [Admin].[Product] DestProd
	ON DestProd.ProductId = MovDest.DestinationProductId
LEFT JOIN [Admin].[Node] SrcNode
	ON SrcNode.NodeId = MovSrc.SourceNodeId
LEFT JOIN [Admin].[Node] DestNode
	ON DestNode.NodeId = MovDest.DestinationNodeId
INNER JOIN [Admin].[OwnershipNode] OwnNode
	ON OwnNode.OwnershipNodeId = OwnErr.OwnershipNodeId
INNER JOIN [Admin].[CategoryElement] CE
	ON CE.ElementId = Mov.SegmentId

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch view_CalculationErrors Data',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_CalculationErrors',
    @level2type = NULL,
    @level2name = NULL