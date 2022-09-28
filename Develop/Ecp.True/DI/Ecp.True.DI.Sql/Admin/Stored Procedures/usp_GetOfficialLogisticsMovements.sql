/*-- =============================================================================================================================================================================
-- Author:          Microsoft
-- Created Date:	July-30-2020
-- Updated Date:	Jan-29-2020 
--					May-31-2020  Reconciliation and Delta Inventories movements are excluded
--					Jun-22-2020  change order type from int to nvarchar
--					Jul-09-2021  Add validation Node for segment
--					Aug-05-2021  add validation for ownershipvalue
--					Aug-11-2021  TicketType SIV-SAP is included
--					Oct-19-2021  Add owner validation
-- <Description>:	This Procedure is used to get movements for official logistics ticket for selected period. </Description>
-- ==============================================================================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_GetOfficialLogisticsMovements] 
(
         @TicketId         INT
)
AS 
BEGIN
		IF OBJECT_ID('tempdb..#TempNodeSegment')IS NOT NULL
		DROP TABLE #TempNodeSegment

		IF OBJECT_ID('tempdb..#TempGrandOwnersTable')IS NOT NULL
		DROP TABLE #TempGrandOwnersTable 

		IF OBJECT_ID('tempdb..#TempTicketNode')IS NOT NULL
		DROP TABLE #TempTicketNode 

		IF OBJECT_ID('tempdb..#TempNodeGeneric')IS NOT NULL
		DROP TABLE #TempNodeGeneric 

		

    DECLARE @SegmentId                      INT,
            @OwnerId                        INT,
            @NodeId                         INT,
            @StartDate                      DATETIME,
            @EndDate                        DATETIME,
            @OfficialMovementDeltaId        INT,
            @ConsolidatedMovementDeltaId    INT,
            @ManualMovSourceSystemId        INT,
            @OfficialScenarioId             INT,
			@TicketTypeId					INT


	IF NOT EXISTS(SELECT 1 FROM Admin.Ticket WHERE TicketId = @TicketId AND [Status] = 1)							
	BEGIN
		RAISERROR ('INVALID_STATUS_TO_LOGISTICT_TICKET',16,1) 
	END
    ELSE
    BEGIN
        SELECT @SegmentId = CategoryElementId
              ,@OwnerId   = OwnerId
              ,@NodeId    = NodeId
              ,@StartDate = StartDate
              ,@EndDate   = EndDate
			  ,@TicketTypeId = TicketTypeId
        FROM Admin.Ticket 
        WHERE TicketId = @TicketId AND [Status] = 1 -- Procesando
   

		SELECT TOP 0 NodeId 
		INTO #TempTicketNode
		FROM Admin.TicketNode 
        
		IF @TicketTypeId in(3,6,7) AND @NodeId IS NULL
			BEGIN

				INSERT INTO #TempTicketNode
				SELECT NodeId 
				FROM Admin.TicketNode 
				WHERE TicketId = @TicketId
				UNION 
				SELECT Nt.NodeID
				FROM Admin.NodeTag NT
					INNER JOIN Admin.CategoryElement CatEle
					ON CatEle.ElementId = NT.ElementId
					INNER JOIN Admin.Node ND
					ON Nd.NodeId = NT.NodeId
				WHERE NT.ElementId = @SegmentId
				AND (SELECT COUNT(*) FROM Admin.TicketNode WHERE TicketId = @TicketId) = 0
		END


		SELECT PartnerOwnerId 
		INTO #TempGrandOwnersTable
		FROM Admin.PartnerOwnerMapping
		WHERE GrandOwnerId = @OwnerId 

		IF NOT EXISTS (SELECT 1 FROM #TempGrandOwnersTable WHERE PartnerOwnerId = @OwnerId)
		BEGIN
			INSERT INTO #TempGrandOwnersTable VALUES(@OwnerId)
		END

		SELECT Nt.NodeID
			INTO #TempNodeSegment
				FROM Admin.NodeTag NT
					INNER JOIN Admin.CategoryElement CatEle
					ON CatEle.ElementId = NT.ElementId
					INNER JOIN Admin.Node ND
					ON Nd.NodeId = NT.NodeId
				WHERE NT.ElementId = @SegmentId

		SELECT 
			Nt.NodeId,
			Nt.StartDate,
			Nt.EndDate
			INTO #TempNodeGeneric
				FROM Admin.NodeTag Nt
					INNER Join Admin.CategoryElement c on nt.ElementId = c.ElementId
				WHERE  c.Name like'%Gen[eé]rico%' and c.CategoryId = 1

		SELECT @OfficialMovementDeltaId = OfficialDeltaMessageTypeId
		FROM Admin.OfficialDeltaMessageType
		WHERE Name = 'OfficialMovementDelta' 
    
		SELECT @ConsolidatedMovementDeltaId = OfficialDeltaMessageTypeId
		FROM Admin.OfficialDeltaMessageType
		WHERE Name = 'ConsolidatedMovementDelta' 

		SELECT @ManualMovSourceSystemId = ElementId
		FROM Admin.CategoryElement
		WHERE Name = 'ManualMovOficial' AND CategoryId = 22 

		SELECT @OfficialScenarioId = ScenarioTypeId
		FROM Admin.ScenarioType
		WHERE Name = 'Oficial' 

		;WITH CteMovements
		AS
		(
			SELECT  Mov.MovementTransactionId                               AS  [MovementTransactionId]
					,Mov.SourceNodeId                                        AS  [SourceNodeId]
					,Mov.SourceNodeName                                      AS  [SourceNode]
					,Mov.DestinationNodeId                                   AS  [DestinationNodeId]
					,Mov.DestinationNodeName                                 AS  [DestinationNode]
					,Mov.SourceProductId                                     AS  [SourceProductId]
					,Mov.SourceProductName                                   AS  [SourceProduct]
					,Mov.DestinationProductId                                AS  [DestinationProductId]
					,Mov.DestinationProductName                              AS  [DestinationProduct]
					,SrcStrgLoc.StorageLocationId                            AS  [SourceStorageLocationId]
					,SrcStrgLoc.Name                                         AS  [SourceStorageLocation]
					,DestStrgLoc.StorageLocationId                           AS  [DestinationStorageLocationId]
					,DestStrgLoc.Name                                        AS  [DestinationStorageLocation]
					,SrcLogCtr.LogisticCenterId                              AS  [SourceLogisticCenterId]
					,SrcLogCtr.Name                                          AS  [SourceLogisticCenter]
					,DestLogCtr.LogisticCenterId                             AS  [DestinationLogisticCenterId]
					,DestLogCtr.Name                                         AS  [DestinationLogisticCenter]
					,Mov.MovementTypeId                                      AS  [MovementTypeId]
					,Mov.NetStandardVolume                                   AS  [NetStandardVolume]
					,Mov.MeasurementUnit                                     AS  [MeasurementUnit]
					,CASE WHEN Mov.SourceNodeId IS NULL 
							THEN DestNd.[Order]
							ELSE SrcNd.[Order]
							END                                              AS  [Order]
					,MovPrd.StartTime                                        AS  [StartDate]
					,MovPrd.EndTime                                          AS  [EndDate]
					,Mov.OperationalDate                                         AS  [OperationDate]
					,MovCont.DocumentNumber							         AS  [OrderPurchase]
					,MovCont.Position								         AS  [PosPurchase]
					,Mov.MovementId                                          AS  [MovementId]
					,NodCost.CostCenterId                                    AS  [CostCenter]
					,DestNd.[Order]                                          AS  [DestinationNodeOrder]
					,SrcNd.[Order]                                           AS  [SourceNodeOrder]
					,Mov.MovementTypeName                                    AS  [MovementTypeName]
					,DestNd.[IsExportation]                                  AS  [DestinationNodeExportation]
					,SrcNd.[IsExportation]                                   AS  [SourceNodeExportation]
					,Mov.[Classification]                                    AS  [Classification]
					,SrcNd.SendToSAP								            AS [SourceNodeSendToSap]
					,DestNd.SendToSAP								        AS [DestinationNodeSendtoSap]
					,ROW_NUMBER()OVER(PARTITION BY Mov.MovementId 
										ORDER     BY Mov.MovementTransactionId DESC) AS Rnum
					,NodCostName.Name										AS [CostCenterName]
			FROM Admin.[view_MovementInformation] Mov
			INNER JOIN Offchain.MovementPeriod MovPrd
			ON Mov.MovementTransactionId = MovPrd.MovementTransactionId
			LEFT JOIN Admin.Node SrcNd
			ON Mov.SourceNodeId = SrcNd.NodeId
			LEFT JOIN Admin.Node DestNd
			ON Mov.DestinationNodeId = DestNd.NodeId
			LEFT JOIN Admin.NodeStorageLocation SrcNodeStrgLoc
			ON SrcNd.NodeId = SrcNodeStrgLoc.NodeId
			LEFT JOIN Admin.NodeStorageLocation DestNodeStrgLoc
			ON  DestNd.NodeId = DestNodeStrgLoc.NodeId
			LEFT JOIN (SELECT * FROM Admin.StorageLocation WHERE IsActive = 1) SrcStrgLoc
			ON SrcNodeStrgLoc.StorageLocationId = SrcStrgLoc.StorageLocationId
			LEFT JOIN (SELECT * FROM Admin.StorageLocation WHERE IsActive = 1) DestStrgLoc
			ON DestNodeStrgLoc.StorageLocationId = DestStrgLoc.StorageLocationId
			LEFT JOIN (SELECT * FROM Admin.LogisticCenter WHERE IsActive = 1) SrcLogCtr
			ON SrcStrgLoc.LogisticCenterId = SrcLogCtr.LogisticCenterId
			LEFT JOIN (SELECT * FROM Admin.LogisticCenter WHERE IsActive = 1) DestLogCtr
			ON DestStrgLoc.LogisticCenterId = DestLogCtr.LogisticCenterId
			LEFT JOIN [Admin].[MovementContract] MovCont
			ON MovCont.MovementContractId = Mov.ContractId
			LEFT JOIN Admin.NodeCostCenter NodCost 
			ON NodCost.IsActive = 1 AND SrcNd.NodeId = NodCost.SourceNodeId AND DestNd.NodeId = NodCost.DestinationNodeId AND NodCost.MovementTypeId  = Mov.MovementTypeId 
			LEFT JOIN Admin.CategoryElement NodCostName
			ON NodCost.CostCenterId = NodCostName.ElementId
			LEFT JOIN [Offchain].LogisticMovement LogMov 
			ON LogMov.MovementTransactionId = Mov.MovementTransactionId
			AND (CONCAT(LogMov.MovementTransactionId,logMov.StatusProcessId,LogMov.CreatedDate)IN
			(SELECT Movement FROM (SELECT TOP(1) 
				CONCAT(Logistic.MovementTransactionId,Logistic.StatusProcessId,Logistic.CreatedDate) AS Movement, 
				Logistic.TicketId, 
				Logistic.StatusProcessId
				FROM [Offchain].LogisticMovement Logistic
				WHERE Logistic.MovementTransactionId = Mov.MovementTransactionId
				ORDER BY  Logistic.CreatedDate desc) LM
				))
			LEFT JOIN [Admin].Ticket Ticket 
			ON LogMov.TicketId = Ticket.TicketId AND Ticket.OwnerId = @OwnerId  
			WHERE
			Mov.SegmentId = @SegmentId
			AND (LogMov.LogisticMovementId IS NULL 
				OR LogMov.StatusProcessId = 8
				OR Ticket.[Status] IN (6,8)
				OR (Ticket.[Status] IN (2) AND LogMov.StatusProcessId NOT IN (0,1,2,3)))
			AND Mov.ScenarioId = @OfficialScenarioId 
			AND ((Mov.OfficialDeltaMessageTypeId = @OfficialMovementDeltaId OR Mov.OfficialDeltaMessageTypeId = @ConsolidatedMovementDeltaId) OR Mov.SourceSystemId = @ManualMovSourceSystemId) 
			AND  ((@NodeId IS NULL OR (SrcNd.NodeId = @NodeId) OR (DestNd.NodeId = @NodeId) 
			OR  ((SrcNd.NodeId IN (SELECT NodeId FROM #TempTicketNode))
					OR (DestNd.NodeId IN (SELECT NodeId FROM #TempTicketNode )))))
			AND MovPrd.StartTime >= @StartDate 
			AND MovPrd.EndTime < DATEADD("dd", 1, @EndDate)
			AND Mov.OfficialDeltaTicketId IS NOT NULL
			AND Mov.MovementTransactionId not in (SELECT Mov.MovementTransactionId  
					WHERE Mov.[Classification] = 'Conciliación' and Mov.DestinationNodeId IN (SELECT NodeId FROM #TempNodeSegment))
			AND Mov.MovementTypeId != 187 -- Movimientos Deltas Inventarios y Anulacion Delta Inventarios

			AND (((SrcNd.NodeId IS NULL OR SrcNd.Name LIKE'%Gen[eé]rico%' 
			OR (SrcNd.NodeId IN (SELECT NodeId FROM #TempNodeGeneric))) 
			AND DestNd.NodeId IN (SELECT NodeId FROM #TempNodeSegment))
			OR (SrcNd.NodeId IN (SELECT NodeId FROM #TempNodeSegment)))
		)

		SELECT  SourceNodeId
				,SourceNode
				,DestinationNodeId
				,DestinationNode
				,SourceProductId
				,SourceProduct
				,DestinationProductId
				,DestinationProduct
				,SourceStorageLocationId
				,SourceStorageLocation
				,DestinationStorageLocationId
				,DestinationStorageLocation
				,SourceLogisticCenterId
				,SourceLogisticCenter
				,DestinationLogisticCenterId
				,DestinationLogisticCenter
				,MovementTypeId
				,NetStandardVolume
				,MeasurementUnit
				,ISNULL(Own.OwnershipValue, OwnerS.OwnershipVolume) as OwnershipValue
				,Own.OwnershipValueUnit
				,IIF([Order] IS NULL, NULL,CONVERT(NVARCHAR, [Order])) AS 'Order'
				,StartDate
				,EndDate
				,OperationDate
				,OrderPurchase
				,PosPurchase
				,MovementId 
				,CostCenter
				,DestinationNodeOrder
				,SourceNodeOrder
				,MovementTypeName
				,DestinationNodeExportation
				,SourceNodeExportation
				,[Classification]
				,SourceNodeSendToSap						
				,DestinationNodeSendtoSap	
				,CteMovements.MovementTransactionId
				,[CostCenterName]
		FROM CteMovements
		LEFT JOIN Offchain.Owner Own
		ON CteMovements.MovementTransactionId = Own.MovementTransactionId
		LEFT JOIN Offchain.Ownership OwnerS
		ON CteMovements.MovementTransactionId = OwnerS.MovementTransactionId
		INNER JOIN #TempGrandOwnersTable GrndOwn
		ON GrndOwn.PartnerOwnerId = Own.OwnerId
		WHERE Rnum = 1
	 END
	  END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get movements for official logistics ticket for selected period.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetOfficialLogisticsMovements',
    @level2type = NULL,
    @level2name = NULL