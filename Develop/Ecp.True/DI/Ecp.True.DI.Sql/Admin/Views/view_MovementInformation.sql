/*-- ================================================================================================
-- Author:		Microsoft
-- Create date: Oct-21-2019
-- Updated date: Mar-20-2020
-- Updated date: Apr-06-2020  -- Added SystemName as per PBI 28962
--				 Apr-09-2020  -- Removed(BlockchainStatus = 1)   
-- Updated date: Jun-11-2020  -- Added batch id column as part of #PBI31874
-- Updated date: Jun-11-2020  -- Added BackupMovementId,GlobalMovementId as part of #PBI25459
-- Updated date: Jun-15-2020  -- Logic changed for SourceSystem. Name from CategoryElement table joining with Movement based on SourceSystemId
-- Updated date: Jun-15-2020  -- Added Scenarioid column as part of #PBI31874
-- Updated date: Jun-16-2020  -- Added DeltaTicketId column as part of #PBI6379
-- Updated date: Jul-07-2020  -- Added Null check condition for sourcesystem column
-- Updated date: Jul-08-2020  -- Added Version column from Movement Table
-- Updated date: Jul-10-2020  -- Added OfficialDeltaMessageTypeId column to get delta movement/ inventory details.
-- Updated date: Jul-10-2020  -- Added ConsolidatedMovementTransactionId,ConsolidatedInventoryProductId column to get Consolidated
                                 movement/ inventory details.
-- Updated date: Jul-20-2020  -- Updated System Name logic
--				 Jul-22-2020  -- Added join to get systemname using systemid	
--				 Ago-06-2021  -- Added IsReconciled
-- <Description>:This View is to Fetch Data related to movement and related elements From Tables(Movement,MovementDestination,MovementSource,Node,Product,CategoryElemnet)</Description>
-- ==================================================================================================*/
CREATE VIEW [Admin].[view_MovementInformation]
AS
SELECT  DISTINCT
        Mov.MovementID,
		Mov.[MovementTypeId],
		MoveType.[Name] AS MovementTypeName,
        Mov.MovementTransactionId,
        Mov.[MessageTypeId],
        Mov.[OfficialDeltaMessageTypeId],
        Mov.[ConsolidatedMovementTransactionId],
        Mov.[ConsolidatedInventoryProductId],    
        Mov.[Classification],
        Mov.[SystemTypeId],
        Mov.SourceMovementId,
        Mov.SourceSystemId,
		ISNULL(SrcSys.[Name],'') AS SourceSystem,
        ISNULL(SystName.[Name], '') AS SystemName,
		Mov.EventType,
        Mov.TicketId,  
        Mov.DeltaTicketId,
        Mov.OwnershipTicketId,
        Mov.OperationalDate AS OperationalDate,
        Mov.SegmentId,
		Mov.MeasurementUnit,
		Mov.GrossStandardVolume,
		Mov.NetStandardVolume,
		Mov.UncertaintyPercentage,
        Mov.VariableTypeId,
        Mov.ReasonId,
        Mov.Comment,
        Mov.MovementContractId AS ContractId,
		Mov.[Version],
        MovSrc.SourceNodeId,
        MovSrc.SourceNodeName,
        MovDest.DestinationNodeId,
        MovDest.DestinationNodeName,
        MovSrc.SourceProductId,
        MovSrc.SourceProductName,
		MovSrc.SourceProductTypeId AS SourceProductTypeId,
        MovDest.DestinationProductId,
        MovDest.DestinationProductName,
		MovDest.DestinationProductTypeId,
		MovSrc.SourceStorageLocationId,
		MovDest.DestinationStorageLocationId,
        Mov.IsDeleted,
        Mov.BatchId,
		Mov.BackupMovementId,
		Mov.GlobalMovementId,
        Mov.IsTransferPoint,
	    Mov.ScenarioId,
        Mov.IsOfficial,
        Mov.SourceMovementTransactionId,
        MOv.SourceInventoryProductId,
        Mov.IsSystemGenerated,
        Mov.OfficialDeltaTicketId,
        Mov.OriginalMovementTransactionId,
        Mov.OwnershipTicketConciliationId,
        Mov.SystemId,
        Mov.CreatedDate,
        Mov.LastModifiedDate,
        Mov.IsReconciled
FROM [OffChain].[Movement] Mov
LEFT JOIN (SELECT  MovDest.MovementTransactionId
				  ,MovDest.DestinationNodeId
				  ,DesNd.[Name] AS DestinationNodeName
				  ,MovDest.DestinationProductId		
				  ,DestPrd.[Name] AS DestinationProductName				  
				  ,MovDest.DestinationProductTypeId
				  ,MovDest.DestinationStorageLocationId				  
		   FROM [Offchain].[MovementDestination] MovDest
		   INNER JOIN [Admin].[Node] DesNd
		   ON DesNd.NodeId = MovDest.DestinationNodeId 
		   INNER JOIN [Admin].Product DestPrd
		   ON DestPrd.ProductId = MovDest.DestinationProductId
		  )MovDest
ON Mov.MovementTransactionId = MovDest.MovementTransactionId
LEFT JOIN (SELECT  MovSrc.MovementTransactionId
				  ,MovSrc.SourceNodeId
				  ,SrcNd.[Name] AS SourceNodeName
				  ,MovSrc.SourceProductId		
				  ,SrcPrd.[Name] AS SourceProductName				  
				  ,MovSrc.SourceProductTypeId
				  ,MovSrc.SourceStorageLocationId				  
		   FROM [Offchain].[MovementSource] MovSrc
		   INNER JOIN [Admin].[Node] SrcNd
		   ON SrcNd.NodeId = MovSrc.SourceNodeId 
		   INNER JOIN [Admin].Product SrcPrd
		   ON SrcPrd.ProductId = MovSrc.SourceProductId
		  )MovSrc
ON Mov.MovementTransactionId = MovSrc.MovementTransactionId
LEFT JOIN [Admin].CategoryElement MoveType
ON MoveType.ElementId = Mov.MovementTypeId
AND MoveType.CategoryId = 9 --MovementType
LEFT JOIN [Admin].[CategoryElement] SrcSys
ON SrcSys.ElementId = Mov.SourceSystemId
LEFT JOIN [Admin].[SystemType] SysType
ON SysType.SystemTypeId = Mov.SystemTypeId
LEFT JOIN [Admin].[CategoryElement] SystName
ON SystName.ElementId = Mov.SystemId
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data related to movement and related elements From Tables(Movement,MovementDestination,MovementSource,Node,Product,CategoryElemnet)',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_MovementInformation',
    @level2type = NULL,
    @level2name = NULL
