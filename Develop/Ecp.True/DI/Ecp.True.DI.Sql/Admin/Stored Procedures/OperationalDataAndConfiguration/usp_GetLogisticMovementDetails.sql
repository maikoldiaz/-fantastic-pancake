/*-- ======================================================================================================================================================================================
-- Author:          Microsoft
-- Created Date:	Dec-12-2019
-- Updated Date:	Jan-29-2020; [BugFix-27772]: SP now return multiple rows in case of date range; Now partition is happening over the MovementId.
--								 And now we are using [Value] as OwnerShipVolume instead of NetStandardVolume. This is captured in PBI-4957.
					Mar-27-2020  Modified Procedure to get the data for Columns (OrderPurchase, PosPurchase) from Contract table
--				    Apr-09-2020  Removed(BlockchainStatus = 1)   
--					Aug-12-2020  Removed cast on OperationalDate
--					May-31-2020  Reconciliation and Delta Inventories movements are excluded
--					Jul-09-2021  Add validation Node for segment
--					Aug-05-2021  add validation for ownershipvalue
--					Aug-11-2021  TicketType SIV-SAP is included
--					Oct-02-2021  Cost Center Name is included
--					Oct-19-2021  Add owner validation
--					Ago-30-2022  Add temp table for concat two movements duplicates of annulation and union all movements
-- <Description>:	This Procedure is used to get the Logistic Movement details for the Excel file based on the Segment Id, Owner Id, Node Id, Start Date and End Date. </Description>
-- =======================================================================================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_GetLogisticMovementDetails] 
(
		 @TicketId			INT
)
AS 
BEGIN

		IF OBJECT_ID('tempdb..#TempPartnerOwnerId')IS NOT NULL
		DROP TABLE #TempPartnerOwnerId

		IF OBJECT_ID('tempdb..#TempTicketNode')IS NOT NULL
		DROP TABLE #TempTicketNode

		IF OBJECT_ID('tempdb..#TempNodeSegment')IS NOT NULL
		DROP TABLE #TempNodeSegment

		IF OBJECT_ID('tempdb..#TempNodeGeneric')IS NOT NULL
		DROP TABLE #TempNodeGeneric 

		IF OBJECT_ID('tempdb..#TempNodeSourceNullAndNodeDestinationNull')IS NOT NULL
		DROP TABLE #TempNodeSourceNullAndNodeDestinationNull
									
		DECLARE @SegmentId                  INT,
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
			FROM TicketNode 

			IF @TicketTypeId in (3,6,7) AND @NodeId IS NULL
			BEGIN

				INSERT INTO #TempTicketNode
				SELECT NodeId 
				FROM TicketNode 
				WHERE TicketId = @TicketId
				UNION 
				SELECT Nt.NodeID
				FROM Admin.NodeTag NT
					INNER JOIN Admin.CategoryElement CatEle
					ON CatEle.ElementId = NT.ElementId
					INNER JOIN Admin.Node ND
					ON Nd.NodeId = NT.NodeId
				WHERE NT.ElementId = @SegmentId
				AND (SELECT COUNT(*) FROM TicketNode WHERE TicketId = @TicketId) = 0
			END


			SELECT PartnerOwnerId
			INTO #TempPartnerOwnerId
			FROM Admin.PartnerOwnerMapping
			WHERE GrandOwnerId = @OwnerId

			IF NOT EXISTS (SELECT 1 FROM #TempPartnerOwnerId WHERE PartnerOwnerId = @OwnerId)
			BEGIN
				INSERT INTO #TempPartnerOwnerId VALUES(@OwnerId)
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

			
			SELECT DISTINCT
				 Mov.SourceNodeId				                AS SourceNodeId
				,Mov.SourceNode                                 AS SourceNode
				,Mov.DestinationNodeId							AS DestinationNodeId
				,Mov.DestinationNode							AS DestinationNode
				,Mov.SourceProductId							AS SourceProductId
				,Mov.SourceProductName							AS [SourceProduct]
				,Mov.DestinationProductId						AS DestinationProductId
				,Mov.DestinationProduct							AS DestinationProduct
				,Mov.SourceMovementTransactionId				AS SourceMovementTransactionId
				,Mov.OriginalMovementTransactionId				AS OriginalMovementTransactionId
				,Mov.SrcStorageLocationId						AS SourceStorageLocationId
				,Mov.SourceStorageLocation                      AS SourceStorageLocation
				,Mov.DestStorageLocationId						AS DestinationStorageLocationId
				,Mov.DestinationStorageLocation					AS DestinationStorageLocation
				,Mov.SrcLogisticCenterId						AS SourceLogisticCenterId
				,Mov.SourceLogisticCenter                       AS SourceLogisticCenter
				,Mov.DestLogisticCenterId						AS DestinationLogisticCenterId
				,Mov.DestinationLogisticCenter                  AS DestinationLogisticCenter
				,Mov.MovementTypeId                             AS MovementTypeId
				,Mov.NetStandardVolume	     					AS NetStandardVolume
				,Mov.MeasurementUnit                            AS MeasurementUnit
				,Mov.OwnershipValue								AS OwnershipValue
				,Mov.OwnershipValueUnit							AS OwnershipValueUnit
				,''												AS [Order]-- Always empty field.
				,Mov.StartDate                                  AS StartDate
				,Mov.EndDate                                    AS EndDate
				,Mov.OperationDate                              AS OperationDate
				,Mov.OrderPurchase								AS OrderPurchase
				,Mov.PosPurchase								AS PosPurchase
				,Mov.MovementID									AS [MovementID]
				,Mov.CostCenterId								AS CostCenter
				,Mov.DestinationNodeOrder                       AS DestinationNodeOrder
				,Mov.SourceNodeOrder                            AS SourceNodeOrder									
				,Mov.MovementTypeName							AS MovementTypeName												  
				,Mov.DestinationNodeExportation                 AS DestinationNodeExportation
				,Mov.SourceNodeExportation                      AS SourceNodeExportation
				,Mov.[Classification]                           AS [Classification]			
				,Mov.SourceNodeSendToSap						AS [SourceNodeSendToSap]
				,Mov.DestinationNodeSendtoSap					AS [DestinationNodeSendtoSap]
				,Mov.MovementTransactionId						AS MovementTransactionId
				,Mov.CostCenterName								AS CostCenterName
			INTO #TempNodeSourceNullAndNodeDestinationNull
			FROM
			(
				SELECT  DISTINCT
					Mov.MovementID									AS [MovementID]
					,Mov.MovementTypeName							AS [MovementTypeName]
					,Mov.SourceProductId							AS [SourceProductId]
					,Mov.DestinationProductId						AS [DestinationProductId]
					,Mov.SourceNodeId								AS [SourceNodeId]
					,Mov.DestinationNodeId							AS [DestinationNodeId]
					,SrcNd.LogisticCenterId 						AS [SrcLogisticCenterId]
					,DestNd.LogisticCenterId						AS [DestLogisticCenterId]
					,SrcNSL.StorageLocationId						AS [SrcStorageLocationId]
					,DestNSL.StorageLocationId						AS [DestStorageLocationId]
					,Mov.SourceProductName							AS [SourceProductName]
					,Mov.DestinationProductName                     AS [DestinationProduct]--Destination Product Of Movement
					,Mov.SourceMovementTransactionId				AS [SourceMovementTransactionId]
					,mov.OriginalMovementTransactionId				AS [OriginalMovementTransactionId]
					,CAST(Mov.NetStandardVolume	AS DECIMAL(18, 2))	AS [NetStandardVolume]
					,Mov.MeasurementUnit							as [MeasurementUnit]
					,CAST(Own.OwnershipVolume	AS DECIMAL(18, 2))	AS [OwnershipVolume]
					,MUnit.Name										AS [UOM]
					,MovCont.DocumentNumber							AS [OrderPurchase]
					,MovCont.Position								AS [PosPurchase]
					,ROW_NUMBER()OVER(PARTITION BY Mov.MovementId 
										ORDER     BY Mov.MovementTransactionId DESC) AS Rnum
					,NodCost.CostCenterId							AS [CostCenterId]
					,Mov.SourceNodeName                             AS [SourceNode]
					,Mov.DestinationNodeName                        AS [DestinationNode]
					,SrcNSL.Name                                    AS [SourceStorageLocation]
					,DestNSL.Name									AS [DestinationStorageLocation]
					,SrcNSL.Name                                    AS [SourceLogisticCenter]
					,DestNSL.Name                                   AS [DestinationLogisticCenter]
					,Mov.MovementTypeId                             AS [MovementTypeId]
					,Mov.OperationalDate                                AS [OperationDate]
					,ISNULL(Owne.OwnershipValue, Own.OwnershipVolume)AS [OwnershipValue]
					,Owne.OwnershipValueUnit						AS [OwnershipValueUnit]
					,Mov.OperationalDate                            AS  [StartDate]
					,Mov.OperationalDate                            AS  [EndDate]
					,DestNd.[Order]                                 AS [DestinationNodeOrder]
					,SrcNd.[Order]                                  AS [SourceNodeOrder]
					,DestNd.[IsExportation]                         AS [DestinationNodeExportation]
					,SrcNd.[IsExportation]                          AS [SourceNodeExportation]
					,Mov.[Classification]                           AS [Classification]
					,SrcNd.SendToSAP								AS [SourceNodeSendToSap]
					,DestNd.SendToSAP								AS [DestinationNodeSendtoSap]
					,Mov.MovementTransactionId						AS [MovementTransactionId]
					,NodCostName.Name								AS [CostCenterName]
					,Mov.IsDeleted									AS [IsDeleted]
				FROM [Admin].[view_MovementInformation] Mov
				INNER JOIN Offchain.Ownership Own
				ON Own.MovementTransactionId = Mov.MovementTransactionId
				LEFT JOIN [Admin].[MovementContract] MovCont
				ON MovCont.MovementContractId = Mov.ContractId
				LEFT JOIN Admin.Node SrcNd
				ON SrcNd.NodeId = Mov.SourceNodeId
				LEFT JOIN Admin.NodeStorageLocation SrcNSL
				ON SrcNSL.NodeId = Mov.SourceNodeId
				LEFT JOIN Admin.Node DestNd
				ON DestNd.NodeId = Mov.DestinationNodeId
				LEFT JOIN Admin.NodeStorageLocation DestNSL
				ON DestNSL.NodeId = Mov.DestinationNodeId
				LEFT JOIN Admin.CategoryElement MUnit
				ON MUnit.ElementId = Mov.MeasurementUnit
				AND MUnit.CategoryId = 6 --Unit of measurement
				INNER JOIN #TempPartnerOwnerId tempowner
				ON tempowner.PartnerOwnerId = Own.OwnerId
				LEFT JOIN Admin.NodeCostCenter NodCost
				ON NodCost.IsActive = 1 AND SrcNd.NodeId = NodCost.SourceNodeId AND DestNd.NodeId = NodCost.DestinationNodeId AND NodCost.MovementTypeId = Mov.MovementTypeId
				LEFT JOIN Admin.CategoryElement NodCostName
				on NodCost.CostCenterId = NodCostName.ElementId
				LEFT JOIN Offchain.Owner Owne
				ON Mov.MovementTransactionId = Owne.MovementTransactionId
				LEFT JOIN Offchain.MovementPeriod MovPrd
				ON Mov.MovementTransactionId = MovPrd.MovementTransactionId
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
				AND Mov.OperationalDate BETWEEN @StartDate AND @EndDate
				AND ((@TicketTypeId<>7 AND (SrcNd.NodeId = ISNULL(@NodeId,SrcNd.NodeId) OR DestNd.NodeId = ISNULL(@NodeId,DestNd.NodeId)))
				OR ((SrcNd.NodeId IN (SELECT NodeId FROM #TempTicketNode))
				OR (DestNd.NodeId IN (SELECT NodeId FROM #TempTicketNode))))
				AND Own.IsDeleted = 0
				AND Mov.MovementTransactionId not in (select Mov.MovementTransactionId  
				WHERE Mov.[Classification] = 'Conciliación' and Mov.DestinationNodeId IN (SELECT NodeId FROM #TempNodeSegment))
				AND Mov.MovementTypeId != 187 -- Movimientos Deltas Inventarios y Anulacion Delta Inventarios
				AND concat('2',' ',Mov.SourceMovementTransactionId, ' ', Mov.OriginalMovementTransactionId)  IN (
					select Concat (count(*),' ', SourceMovementTransactionId, ' ', OriginalMovementTransactionId) from Offchain.Movement
					where  OriginalMovementTransactionId is not null
					and SourceMovementTransactionId is not null
					group by SourceMovementTransactionId, OriginalMovementTransactionId
					having count(*) > 1)
				AND (DestNd.Name IS NULL OR SrcNd.Name IS NULL)
				AND (((SrcNd.NodeId IS NULL OR SrcNd.Name LIKE'%Gen[eé]rico%' 
				OR (SrcNd.NodeId IN (SELECT NodeId FROM #TempNodeGeneric))) 
				AND DestNd.NodeId IN (SELECT NodeId FROM #TempNodeSegment))
				OR (SrcNd.NodeId IN (SELECT NodeId FROM #TempNodeSegment)))
			)Mov
			LEFT JOIN Admin.LogisticCenter SrcLogic
			ON Mov.SrcLogisticCenterId = SrcLogic.LogisticCenterId
			LEFT JOIN Admin.StorageLocation SrcSloc
			ON Mov.SrcStorageLocationId = SrcSloc.StorageLocationId
			LEFT JOIN Admin.LogisticCenter DestLogic
			ON Mov.DestLogisticCenterId = DestLogic.LogisticCenterId
			LEFT JOIN Admin.StorageLocation DestSloc
			ON Mov.DestStorageLocationId = DestSloc.StorageLocationId 
			WHERE Mov.Rnum = 1
					AND Mov.NetStandardVolume > 0 AND Mov.[IsDeleted] = 0


			(
				SELECT DISTINCT
					NULL											AS ConcatMovementId
					,Mov.SourceNodeId				                AS SourceNodeId
					,Mov.SourceNode                                 AS SourceNode
					,Mov.DestinationNodeId							AS DestinationNodeId
					,Mov.DestinationNode							AS DestinationNode
					,Mov.SourceProductId							AS SourceProductId
					,Mov.SourceProductName							AS [SourceProduct]
					,Mov.DestinationProductId						AS DestinationProductId
					,Mov.DestinationProduct							AS DestinationProduct
					,Mov.SrcStorageLocationId						AS SourceStorageLocationId
					,Mov.SourceStorageLocation                      AS SourceStorageLocation
					,Mov.DestStorageLocationId						AS DestinationStorageLocationId
					,Mov.DestinationStorageLocation					AS DestinationStorageLocation
					,Mov.SrcLogisticCenterId						AS SourceLogisticCenterId
					,Mov.SourceLogisticCenter                       AS SourceLogisticCenter
					,Mov.DestLogisticCenterId						AS DestinationLogisticCenterId
					,Mov.DestinationLogisticCenter                  AS DestinationLogisticCenter
					,Mov.MovementTypeId                             AS MovementTypeId
					,Mov.NetStandardVolume	     					AS NetStandardVolume
					,Mov.MeasurementUnit                            AS MeasurementUnit
					,Mov.OwnershipValue								AS OwnershipValue
					,Mov.OwnershipValueUnit							AS OwnershipValueUnit
					,''												AS [Order]-- Always empty field.
					,Mov.StartDate                                  AS StartDate
					,Mov.EndDate                                    AS EndDate
					,Mov.OperationDate                              AS OperationDate
					,Mov.OrderPurchase								AS OrderPurchase
					,Mov.PosPurchase								AS PosPurchase
					,Mov.MovementID									AS [MovementID]
					,Mov.CostCenterId								AS CostCenter
					,Mov.DestinationNodeOrder                       AS DestinationNodeOrder
					,Mov.SourceNodeOrder                            AS SourceNodeOrder									
					,Mov.MovementTypeName							AS MovementTypeName												  
					,Mov.DestinationNodeExportation                 AS DestinationNodeExportation
					,Mov.SourceNodeExportation                      AS SourceNodeExportation
					,Mov.[Classification]                           AS [Classification]			
					,Mov.SourceNodeSendToSap						AS [SourceNodeSendToSap]
					,Mov.DestinationNodeSendtoSap					AS [DestinationNodeSendtoSap]
					,Mov.MovementTransactionId						AS MovementTransactionId
					,Mov.CostCenterName								AS CostCenterName
				FROM
				(
					SELECT  DISTINCT
						Mov.MovementID									AS [MovementID]
						,Mov.MovementTypeName							AS [MovementTypeName]
						,Mov.SourceProductId							AS [SourceProductId]
						,Mov.DestinationProductId						AS [DestinationProductId]
						,Mov.SourceNodeId								AS [SourceNodeId]
						,Mov.DestinationNodeId							AS [DestinationNodeId]
						,SrcNd.LogisticCenterId 						AS [SrcLogisticCenterId]
						,DestNd.LogisticCenterId						AS [DestLogisticCenterId]
						,SrcNSL.StorageLocationId						AS [SrcStorageLocationId]
						,DestNSL.StorageLocationId						AS [DestStorageLocationId]
						,Mov.SourceProductName							AS [SourceProductName]
						,Mov.DestinationProductName                     AS [DestinationProduct]--Destination Product Of Movement
						,CAST(Mov.NetStandardVolume	AS DECIMAL(18, 2))	AS [NetStandardVolume]
						,Mov.MeasurementUnit							as [MeasurementUnit]
						,CAST(Own.OwnershipVolume	AS DECIMAL(18, 2))	AS [OwnershipVolume]
						,MUnit.Name										AS [UOM]
						,MovCont.DocumentNumber							AS [OrderPurchase]
						,MovCont.Position								AS [PosPurchase]
						,ROW_NUMBER()OVER(PARTITION BY Mov.MovementId 
											ORDER     BY Mov.MovementTransactionId DESC) AS Rnum
						,NodCost.CostCenterId							AS [CostCenterId]
						,Mov.SourceNodeName                             AS [SourceNode]
						,Mov.DestinationNodeName                        AS [DestinationNode]
						,SrcNSL.Name                                    AS [SourceStorageLocation]
						,DestNSL.Name									AS [DestinationStorageLocation]
						,SrcNSL.Name                                    AS [SourceLogisticCenter]
						,DestNSL.Name                                   AS [DestinationLogisticCenter]
						,Mov.MovementTypeId                             AS [MovementTypeId]
						,Mov.OperationalDate                                AS [OperationDate]
						,ISNULL(Owne.OwnershipValue, Own.OwnershipVolume)AS [OwnershipValue]
						,Owne.OwnershipValueUnit						AS [OwnershipValueUnit]
						,Mov.OperationalDate                            AS  [StartDate]
						,Mov.OperationalDate                            AS  [EndDate]
						,DestNd.[Order]                                 AS [DestinationNodeOrder]
						,SrcNd.[Order]                                  AS [SourceNodeOrder]
						,DestNd.[IsExportation]                         AS [DestinationNodeExportation]
						,SrcNd.[IsExportation]                          AS [SourceNodeExportation]
						,Mov.[Classification]                           AS [Classification]
						,SrcNd.SendToSAP								AS [SourceNodeSendToSap]
						,DestNd.SendToSAP								AS [DestinationNodeSendtoSap]
						,Mov.MovementTransactionId						AS [MovementTransactionId]
						,NodCostName.Name								AS [CostCenterName]
						,Mov.IsDeleted									AS [IsDeleted]
					FROM [Admin].[view_MovementInformation] Mov
					INNER JOIN Offchain.Ownership Own
					ON Own.MovementTransactionId = Mov.MovementTransactionId
					LEFT JOIN [Admin].[MovementContract] MovCont
					ON MovCont.MovementContractId = Mov.ContractId
					LEFT JOIN Admin.Node SrcNd
					ON SrcNd.NodeId = Mov.SourceNodeId
					LEFT JOIN Admin.NodeStorageLocation SrcNSL
					ON SrcNSL.NodeId = Mov.SourceNodeId
					LEFT JOIN Admin.Node DestNd
					ON DestNd.NodeId = Mov.DestinationNodeId
					LEFT JOIN Admin.NodeStorageLocation DestNSL
					ON DestNSL.NodeId = Mov.DestinationNodeId
					LEFT JOIN Admin.CategoryElement MUnit
					ON MUnit.ElementId = Mov.MeasurementUnit
					AND MUnit.CategoryId = 6 --Unit of measurement
					INNER JOIN #TempPartnerOwnerId tempowner
					ON tempowner.PartnerOwnerId = Own.OwnerId
					LEFT JOIN Admin.NodeCostCenter NodCost
					ON NodCost.IsActive = 1 AND SrcNd.NodeId = NodCost.SourceNodeId AND DestNd.NodeId = NodCost.DestinationNodeId AND NodCost.MovementTypeId = Mov.MovementTypeId
					LEFT JOIN Admin.CategoryElement NodCostName
					on NodCost.CostCenterId = NodCostName.ElementId
					LEFT JOIN Offchain.Owner Owne
					ON Mov.MovementTransactionId = Owne.MovementTransactionId
					LEFT JOIN Offchain.MovementPeriod MovPrd
					ON Mov.MovementTransactionId = MovPrd.MovementTransactionId
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
					AND Mov.OperationalDate BETWEEN @StartDate AND @EndDate
					AND ((@TicketTypeId<>7 AND (SrcNd.NodeId = ISNULL(@NodeId,SrcNd.NodeId) OR DestNd.NodeId = ISNULL(@NodeId,DestNd.NodeId)))
					OR ((SrcNd.NodeId IN (SELECT NodeId FROM #TempTicketNode))
					OR (DestNd.NodeId IN (SELECT NodeId FROM #TempTicketNode))))
					AND Own.IsDeleted = 0
					AND Mov.MovementTransactionId not in (select Mov.MovementTransactionId  
					WHERE Mov.[Classification] = 'Conciliación' and Mov.DestinationNodeId IN (SELECT NodeId FROM #TempNodeSegment))
					AND Mov.MovementTypeId != 187 -- Movimientos Deltas Inventarios y Anulacion Delta Inventarios
					AND (concat('1',' ',Mov.SourceMovementTransactionId, ' ', Mov.OriginalMovementTransactionId)  IN (
						select Concat (count(*),' ', SourceMovementTransactionId, ' ', OriginalMovementTransactionId) from Offchain.Movement
						where  OriginalMovementTransactionId is not null
						AND SourceMovementTransactionId is not null
						group by SourceMovementTransactionId, OriginalMovementTransactionId
						having count(*) = 1)
						or Mov.OriginalMovementTransactionId is null
						or Mov.SourceMovementTransactionId is null)
					AND (((SrcNd.NodeId IS NULL OR SrcNd.Name LIKE'%Gen[eé]rico%' 
					OR (SrcNd.NodeId IN (SELECT NodeId FROM #TempNodeGeneric))) 
					AND DestNd.NodeId IN (SELECT NodeId FROM #TempNodeSegment))
					OR (SrcNd.NodeId IN (SELECT NodeId FROM #TempNodeSegment)))
				)Mov
				LEFT JOIN Admin.LogisticCenter SrcLogic
				ON Mov.SrcLogisticCenterId = SrcLogic.LogisticCenterId
				LEFT JOIN Admin.StorageLocation SrcSloc
				ON Mov.SrcStorageLocationId = SrcSloc.StorageLocationId
				LEFT JOIN Admin.LogisticCenter DestLogic
				ON Mov.DestLogisticCenterId = DestLogic.LogisticCenterId
				LEFT JOIN Admin.StorageLocation DestSloc
				ON Mov.DestStorageLocationId = DestSloc.StorageLocationId 
				WHERE Mov.Rnum = 1
						AND Mov.NetStandardVolume > 0 AND Mov.[IsDeleted] = 0
			)

			UNION ALL

			(
				SELECT DISTINCT
					CONCAT(NdSrc.MovementId,'-', NdDstn.MovementId) 	AS ConcatMovementId
					,NdDstn.SourceNodeId				                AS SourceNodeId
					,NdDstn.SourceNode                                  AS SourceNode
					,NdSrc.DestinationNodeId							AS DestinationNodeId
					,NdSrc.DestinationNode								AS DestinationNode
					,NdDstn.SourceProductId								AS SourceProductId
					,NdDstn.SourceProduct								AS [SourceProduct]
					,NdSrc.DestinationProductId							AS DestinationProductId
					,NdSrc.DestinationProduct							AS DestinationProduct
					,NdDstn.SourceStorageLocationId						AS SourceStorageLocationId
					,NdDstn.SourceStorageLocation                      	AS SourceStorageLocation
					,NdSrc.DestinationStorageLocationId					AS DestinationStorageLocationId
					,NdSrc.DestinationStorageLocation					AS DestinationStorageLocation
					,NdDstn.SourceLogisticCenterId						AS SourceLogisticCenterId
					,NdDstn.SourceLogisticCenter                       	AS SourceLogisticCenter
					,NdSrc.DestinationLogisticCenterId					AS DestinationLogisticCenterId
					,NdSrc.DestinationLogisticCenter                  	AS DestinationLogisticCenter
					,NdDstn.MovementTypeId                             	AS MovementTypeId
					,NdDstn.NetStandardVolume	     					AS NetStandardVolume
					,NdDstn.MeasurementUnit                            	AS MeasurementUnit
					,NdDstn.OwnershipValue								AS OwnershipValue
					,NdDstn.OwnershipValueUnit							AS OwnershipValueUnit
					,''													AS [Order]-- Always empty field.
					,NdDstn.StartDate                                  	AS StartDate
					,NdDstn.EndDate                                    	AS EndDate
					,NdDstn.OperationDate                              	AS OperationDate
					,NdDstn.OrderPurchase								AS OrderPurchase
					,NdDstn.PosPurchase									AS PosPurchase
					,NdDstn.MovementID									AS [MovementID]
					,NdDstn.CostCenter									AS CostCenter
					,NdSrc.DestinationNodeOrder                       	AS DestinationNodeOrder
					,NdDstn.SourceNodeOrder                            	AS SourceNodeOrder									
					,NdDstn.MovementTypeName							AS MovementTypeName												  
					,NdSrc.DestinationNodeExportation                 	AS DestinationNodeExportation
					,NdDstn.SourceNodeExportation                      	AS SourceNodeExportation
					,NdDstn.[Classification]                           	AS [Classification]			
					,NdDstn.SourceNodeSendToSap							AS [SourceNodeSendToSap]
					,NdSrc.DestinationNodeSendtoSap						AS [DestinationNodeSendtoSap]
					,NdDstn.MovementTransactionId						AS MovementTransactionId
					,NdDstn.CostCenterName								AS CostCenterName
				FROM (SELECT * FROM #TempNodeSourceNullAndNodeDestinationNull WHERE DestinationNodeId IS NULL) NdDstn
				INNER JOIN (SELECT * FROM #TempNodeSourceNullAndNodeDestinationNull WHERE SourceNodeId IS NULL) NdSrc
				ON NdDstn.SourceMovementTransactionId = NdSrc.SourceMovementTransactionId
				AND NdDstn.OriginalMovementTransactionId = NdSrc.OriginalMovementTransactionId
			)

		END
					
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get the Logistic Movement details for the Excel file based on the Segment Id, Owner Id, Node Id, Start Date and End Date.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetLogisticMovementDetails',
    @level2type = NULL,
    @level2name = NULL