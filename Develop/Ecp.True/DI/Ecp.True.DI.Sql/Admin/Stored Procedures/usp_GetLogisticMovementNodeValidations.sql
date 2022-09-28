/*-- =================================================================================================================================
-- Author: IG Service
-- Created Date: Marzo-19-2021
-- Update Date: April-14-2021 Add generic node validation
				June-21-2021 Add Status ticket validation
				June-28-2021 Filter movements according to received dates
				August-20-2021 Add  start and end dates
			    October-19-2021  Add owner validation
-- <Description>: This Procedure is used to validate that the nodes for the next case.
* Validating available nodes.
* Validating nodes with submission to SAP.
* Approved nodes.
* Predecessor nodes with submission to SAP.
</Description>
-- ==================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetLogisticMovementNodeValidations]
(
	   @SegmentId							INT,
       @StartDate							DATE,
       @EndDate								DATE,
	   @ScenarioTypeId						INT,
	   @OwnerId							    INT = NULL,
	   @DtNodes								[Admin].[NodeListType] READONLY
)
AS
BEGIN
	   IF @StartDate <= @EndDate   
	   BEGIN
			DECLARE @Cnt							INT  = 0,			 
					@ElementId						INT, 
					@NodeID							INT, 
					@NodeName						NVARCHAR (MAX),
					@newStartDate					DATE,
					@newEndDate						DATE,
					@IsSelectedAllSegment			BIT,
					@IsEnabledForSendToSap			BIT;

			
			IF OBJECT_ID('tempdb..#TempNodes')IS NOT NULL
			DROP TABLE #TempNodes

			IF OBJECT_ID('tempdb..#TempMovement')IS NOT NULL
			DROP TABLE #TempMovement

			IF OBJECT_ID('tempdb..#TempNodeFinal')IS NOT NULL
			DROP TABLE #TempNodeFinal

			IF OBJECT_ID('tempdb..#TempNodeTagForSegment') IS NOT NULL
			DROP TABLE #TempNodeTagForSegment

			IF OBJECT_ID('tempdb..#TempNodesForSegment') IS NOT NULL 
			DROP TABLE #TempNodesForSegment
			
			CREATE TABLE #TempNodeTagForSegment
			(
				NodeId						INT,
				NodeName					NVARCHAR (150),
				ElementId					INT,
				StartDate					DATE,
				EndDate						DATE,
				IsEnabledForSendToSap		INT,
				RowNumber					INT
			)
			
			CREATE TABLE #TempNodesForSegment
			(
				SegmentId					INT,
				NodeId						INT,
				NodeName					NVARCHAR (150),
				OperationDate				DATE,
				StartDate					DATE,
				EndDate						DATE,
				StatusId					INT,
				StatusName					NVARCHAR (150),
				IsEnabledForSendToSap		BIT,
				IsApproved					BIT,
				IsActiveInBatch				BIT,
				PredecessorIsApproved		BIT,
				TicketStatusName			NVARCHAR (150),
			)
			
			-- Validate if all the nodes of a segment are being used
			SET   @IsSelectedAllSegment = CASE WHEN (SELECT COUNT(NodeId) FROM @DtNodes) = 0 THEN 1 ELSE 0 END;

			-- Search movements in processing(1), viewing(5) and sent(3)
			SELECT mvt.MovementTransactionId, (SELECT S.StatusType FROM Admin.StatusType S WHERE S.StatusTypeId = T.Status) as TicketStatusName
			INTO #TempMovement
			FROM Offchain.LogisticMovement as MVT
			INNER JOIN Offchain.Movement M  ON MVT.MovementTransactionId = M.MovementTransactionId
			INNER JOIN Admin.Ticket T  ON T.TicketId = MVT.TicketId
			WHERE MVT.TicketId IN (SELECT TC.TicketId FROM admin.Ticket TC WHERE CategoryElementId = @SegmentId  
				  AND TC.OwnerId = @OwnerId
				  AND TC.Status IN (1,3,5) -- Status of the ticket  processing(1), viewing(5) and sent(3)
			      AND TC.TicketTypeId = 7
				  AND TC.ScenarioTypeId = @ScenarioTypeId) -- CASE WHEN @ScenarioTypeId = 1 THEN 3 ELSE 6 END) -- TicketTypeId of the ticket Logistic(3), OficialLogistic(6)
				  AND M.OperationalDate BETWEEN @StartDate AND @EndDate
			
			
			-- A temporary table is created with the initial and destination nodes of each movement
			-- found in bash with processing, viewing and sent states
			SELECT MovementTransactionId, SourceNodeId as nodeId, 
			(SELECT TOP 1 m.TicketStatusName FROM #TempMovement m WHERE m.MovementTransactionId = MovementTransactionId) as TicketStatusName
			INTO #TempNodeFinal
			FROM Offchain.MovementSource
			WHERE MovementTransactionId IN (SELECT m.MovementTransactionId FROM #TempMovement m)
			UNION ALL
			SELECT MovementTransactionId, DestinationNodeId as nodeId, 
			(SELECT TOP 1 m.TicketStatusName FROM #TempMovement m WHERE m.MovementTransactionId = MovementTransactionId) as TicketStatusName
			FROM Offchain.MovementDestination
			WHERE MovementTransactionId IN (SELECT m.MovementTransactionId FROM #TempMovement m)


			-- Consult nodes used by segment
			SELECT  NT.NodeId
					,N.Name as NodeName
					,NT.ElementId
					,CASE WHEN @StartDate >= NT.StartDate 
						  THEN @StartDate
						  ELSE CAST(NT.StartDate		AS DATE) 
						  END AS StartDate
					,CASE WHEN Nt.EndDate = '9999-12-31' 
						   OR  @EndDate   <= Nt.EndDate
						  THEN @EndDate
						  ELSE CAST(NT.EndDate AS DATE)
						  END AS EndDate
					,N.SendToSAP as IsEnabledForSendToSap
			INTO #TempNodes
			FROM Admin.NodeTag NT JOIN Admin.Node N ON NT.NodeId = N.NodeId 
			WHERE NT.ElementId = @SegmentId
			AND ((@StartDate BETWEEN NT.StartDate  AND Nt.EndDate )
				  OR (@EndDate   BETWEEN NT.StartDate  AND Nt.EndDate ))

			-- Consult nodes
			IF(@IsSelectedAllSegment = 1)
			BEGIN
				INSERT INTO #TempNodeTagForSegment 
				SELECT *, ROW_NUMBER()OVER(ORDER BY NT.NodeId)RowNumber FROM #TempNodes NT
			END
			ELSE
			BEGIN
				INSERT INTO #TempNodeTagForSegment 
				SELECT *, ROW_NUMBER()OVER(ORDER BY NT.NodeId)RowNumber FROM #TempNodes NT WHERE NT.NodeId IN (SELECT NodeId FROM @DtNodes)
			END
			
			SET	  @Cnt  = (SELECT COUNT(1) FROM #TempNodeTagForSegment)

			WHILE @Cnt>0
			BEGIN 
				SELECT TOP 1  @NodeID = NodeId, 
							  @NodeName = NodeName,
							  @ElementId = ElementId, 
							  @newStartDate = StartDate, 
							  @newEndDate = EndDate,
							  @IsEnabledForSendToSap = IsEnabledForSendToSap
				FROM #TempNodeTagForSegment
				WHERE RowNumber = @Cnt

				SELECT @Cnt = @Cnt-1

				-- Generate all dates between @StartDate and @EndDate
				INSERT INTO #TempNodesForSegment
				(
						SegmentId,
						NodeId,
						NodeName,
						OperationDate,
						StartDate,
						EndDate,
						StatusId,
						StatusName,
						IsEnabledForSendToSap,
						IsApproved,
						TicketStatusName,
						IsActiveInBatch
				)
				SELECT   Elementid		AS 	SegmentId
						,NodeId			AS  NodeId
						,@NodeName
						,dates			AS  OperationDate
						,@StartDate
						,@EndDate
						,null
						,null
						,@IsEnabledForSendToSap
						,0
						,(SELECT TOP 1 m.TicketStatusName FROM #TempNodeFinal m WHERE m.nodeId = NodeId) as TicketStatusName
						,CASE WHEN NodeId IN (SELECT DISTINCT TF.nodeId FROM #TempNodeFinal TF) THEN 1 ELSE 0 END
				FROM [Admin].[udf_GetAllDates](@newStartDate, 
												@newEndDate, 
												@NodeID, 
												@ElementId) C

				UPDATE #TempNodesForSegment
				SET PredecessorIsApproved = (SELECT [Admin].[udf_PrecessorsIsApproved](A.OperationDate,
																	A.NodeId, 
																	@SegmentId, 
																	@IsSelectedAllSegment, 
																	@ScenarioTypeId))
				FROM (SELECT * FROM #TempNodesForSegment) AS B INNER JOIN #TempNodesForSegment A 
				ON A.NodeId = B.NodeId AND A.OperationDate = B.OperationDate

			END
			-- Validate if nodes are approved
			IF @ScenarioTypeId = 1
			BEGIN 
				UPDATE TempNodes
				SET 
				TempNodes.StatusId = Ticket.OwnershipStatusId, 
				TempNodes.StatusName = (SELECT OST.Name FROM Admin.OwnershipNodeStatusType OST 
												 WHERE OST.OwnershipNodeStatusTypeId= Ticket.OwnershipStatusId),
				TempNodes.IsApproved = CASE WHEN Ticket.OwnershipStatusId = 9 THEN 1 ELSE 0 END
				FROM #TempNodesForSegment TempNodes
				INNER JOIN
			   (SELECT T.StartDate, ONode.* FROM Admin.Ticket T inner join Admin.OwnershipNode ONode ON T.TicketId = ONode.TicketId
											WHERE T.TicketTypeId = 2 AND T.Status=0) Ticket
				ON TempNodes.NodeId = Ticket.NodeId and Ticket.StartDate = TempNodes.OperationDate

				SELECT SegmentId, NodeId, NodeName, OperationDate, StartDate, EndDate, StatusId, StatusName,
					   IsEnabledForSendToSap,IsApproved,IsActiveInBatch,PredecessorIsApproved,TicketStatusName 
				FROM #TempNodesForSegment TNS
				WHERE (Lower(TNS.NodeName) NOT LIKE '%generico%' AND Lower(TNS.NodeName) NOT LIKE '%genérico%')
			END
			ELSE IF @ScenarioTypeId = 2
			BEGIN 
				UPDATE TempNodes
				SET 
				TempNodes.StatusId = Ticket.Status, 
				TempNodes.StatusName = (SELECT OST.Name FROM Admin.OwnershipNodeStatusType OST 
												 WHERE OST.OwnershipNodeStatusTypeId= Ticket.Status),
				TempNodes.IsApproved = CASE WHEN Ticket.Status = 9 THEN 1 ELSE 0 END
				FROM #TempNodesForSegment TempNodes
				INNER JOIN
			   (SELECT T.StartDate, DNode.* FROM Admin.Ticket T inner join Admin.DeltaNode DNode ON T.TicketId = DNode.TicketId
											WHERE T.TicketTypeId = 5 AND T.Status = 4) Ticket
				ON TempNodes.NodeId = Ticket.NodeId and (Month(Ticket.StartDate) = Month(TempNodes.OperationDate) and  
				Year(Ticket.StartDate) = Year(TempNodes.OperationDate))
				
				SELECT SegmentId, NodeId, NodeName, StartDate as OperationDate, StartDate, EndDate, StatusId, StatusName,
					   IsEnabledForSendToSap,IsApproved,IsActiveInBatch,PredecessorIsApproved,TicketStatusName 
				FROM #TempNodesForSegment
				WHERE (Lower(NodeName) NOT LIKE '%generico%' AND Lower(NodeName) NOT LIKE '%genérico%')
				group by SegmentId, NodeId, NodeName, StatusId, StatusName,IsEnabledForSendToSap,IsApproved,IsActiveInBatch,PredecessorIsApproved,TicketStatusName, StartDate, EndDate
			END
		END
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', 
	@value=N'This Procedure is used to validate that the nodes for the next case. 
	* Validating available nodes.
	* Validating nodes with submission to SAP.
	* Approved nodes.
	* Predecessor nodes with submission to SAP.' , 
	@level0type=N'SCHEMA',
	@level0name=N'Admin', 
	@level1type=N'PROCEDURE',
	@level1name=N'usp_GetLogisticMovementNodeValidations'
GO