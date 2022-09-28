
/*-- ============================================================================================================================
-- Author:          Intergrupo
-- Created Date:	Mar-28-2021
-- Update Date: April-14-2021 Add generic node validation
-- <Description>:   This function takes information from a node and returns if its predecessor node is approved </Description>
-- Update Date: April-14-2021
* Add generic node validation
-- Update Date: June-17-2021
				June-21-2021 Add Status node validation
				June-23-2021 Add conciliation node validation
--				Sep-21-2021 Change CONCILIATION validation
* Change ticketStatus to ownershipNodeStatus and deltaNodeStatus
-- ==============================================================================================================================*/

CREATE FUNCTION [Admin].[udf_PrecessorsIsApproved](@operationDate date,
													@nodeId Int, 
													@elementId Int, 
													@allSegment bit, 
													@ScenarioTypeId Int)
RETURNS BIT
AS
BEGIN
		DECLARE @Approved BIT,
			@CNTPre INT,
			@CNTPreApproved INT,
			@CNTSegInt INT,
			@ReturnFlag INT;

	-- if scenarioTypeId is operative(1) and all segment is selected then no apply  predecessor validation 
	IF(@ScenarioTypeId = 1 AND @allSegment = 1)
	BEGIN
		SET @Approved = 1
	END
	ELSE
	BEGIN
		DECLARE @TempNodePredecessors TABLE(NodeId INT, SegmentId INT);
		DECLARE @TempNodeSegmentIntersect TABLE(NodeIdPre INT, SegmentId INT, TicketId INT);

		WITH Predecessors 
		AS
		(
		SELECT  SourceNodeId,
				DestinationNodeId--Get the All the
		FROM Admin.NodeConnection C
		INNER JOIN Admin.Node NDest On C.DestinationNodeId=NDest.NodeId
		INNER JOIN Admin.Node NSrc On C.SourceNodeId=NSrc.NodeId
		WHERE	NDest.[Order] > NSrc.[Order]
		AND		DestinationNodeId = @nodeId 
		AND		(Lower(NDest.Name) NOT LIKE '%gen[eé]rico%' AND Lower(NDest.Name) <> 'CONCILIACION' )
		AND		(Lower(NSrc.Name) NOT LIKE '%gen[eé]rico%' AND Lower(NSrc.Name) <> 'CONCILIACION' )
    
		UNION ALL

		SELECT   N.SourceNodeId,
				 N.DestinationNodeId
		FROM   Admin.NodeConnection N
		INNER JOIN Admin.Node NDest On N.DestinationNodeId=NDest.NodeId
		INNER JOIN Admin.Node NSrc On N.SourceNodeId=NSrc.NodeId
		INNER JOIN Predecessors P ON P.SourceNodeId = N.DestinationNodeId
		WHERE NDest.[Order] > NSrc.[Order] AND @nodeId IS NULL
		AND		(Lower(NDest.Name) NOT LIKE '%gen[eé]rico%' AND Lower(NDest.Name) <> 'CONCILIACION' )
		AND		(Lower(NSrc.Name) NOT LIKE '%gen[eé]rico%' AND Lower(NSrc.Name) <> 'CONCILIACION' )
		)

		--Predecessors for input node and their segment
		INSERT INTO  @TempNodePredecessors (NodeId, SegmentId)
		SELECT  p.SourceNodeId, ISNULL(n.ElementId,0) 
		FROM  Predecessors p 
		JOIN admin.NodeTag n on p.SourceNodeId=n.NodeId; -- Nodes only having segment
	
		--Check if Segment of Predecessor nodes are present in Segment of Input node
		IF @ScenarioTypeId = 1
		BEGIN
		    SET @CNTPre = (SELECT DISTINCT(COUNT(TP.NodeId)) FROM @TempNodePredecessors TP WHERE TP.SegmentId = @elementId)

			-- Check if not exist node predecessor
			If @CNTPre = 0
			BEGIN
				SET @Approved = 1
			END
			ELSE
			BEGIN
				INSERT INTO @TempNodeSegmentIntersect(NodeIdPre,SegmentId, TicketId)
				Select a.NodeId, a.SegmentId, t.TicketId 
				from @TempNodePredecessors a
				JOIN Admin.OwnershipNode b
				ON a.NodeId = b.NodeId
				JOIN Admin.Ticket t
				ON b.TicketId = t.TicketId
				AND b.OwnershipStatusId = 9
				WHERE a.SegmentId = @elementId
				AND @operationDate BETWEEN t.StartDate AND t.EndDate

				SET @CNTPreApproved = (SELECT DISTINCT(COUNT(P.NodeId)) FROM @TempNodePredecessors P WHERE P.SegmentId = @elementId AND P.NodeId IN (SELECT SI.NodeIdPre FROM @TempNodeSegmentIntersect SI)) 
				SET @Approved = CASE WHEN @CNTPreApproved = @CNTPre THEN 1 ELSE 0 END;
			END 
		END
		ELSE
		BEGIN
			IF @allSegment = 1
			BEGIN
				SET @CNTPre = (SELECT DISTINCT(COUNT(TP.NodeId)) FROM @TempNodePredecessors TP WHERE TP.SegmentId <> @elementId)

				-- Check if not exist node predecessor
				If @CNTPre = 0
				BEGIN
					SET @Approved = 1
				END
				ELSE
					INSERT INTO @TempNodeSegmentIntersect(NodeIdPre,SegmentId, TicketId)
					Select a.NodeId, a.SegmentId, t.TicketId 
					from @TempNodePredecessors a
					JOIN Admin.view_DeltaNode b
					ON a.NodeId = b.NodeId
					JOIN Admin.Ticket t
					ON b.TicketId = t.TicketId
					AND b.DeltaNodeStatus = 9
					WHERE a.SegmentId <> @elementId
					AND @operationDate BETWEEN t.StartDate AND t.EndDate 

					SET @CNTPreApproved = (SELECT DISTINCT(COUNT(P.NodeId)) FROM @TempNodePredecessors P 
											WHERE P.SegmentId <> @elementId AND P.NodeId IN (SELECT SI.NodeIdPre FROM @TempNodeSegmentIntersect SI)) 

					SET @Approved = CASE WHEN @CNTPreApproved = @CNTPre THEN 1 ELSE 0 END;
				END
			ELSE
			BEGIN
			SET @CNTPre = (SELECT DISTINCT(COUNT(TP.NodeId)) FROM @TempNodePredecessors TP)

				-- Check if not exist node predecessor
				If @CNTPre = 0
				BEGIN
					SET @Approved = 1
				END
				ELSE
				BEGIN
					INSERT INTO @TempNodeSegmentIntersect(NodeIdPre,SegmentId, TicketId)
					Select a.NodeId, a.SegmentId, t.TicketId 
					from @TempNodePredecessors a
					JOIN Admin.view_DeltaNode b
					ON a.NodeId = b.NodeId
					JOIN Admin.Ticket t
					ON b.TicketId = t.TicketId
					AND b.DeltaNodeStatus = 9
					WHERE @operationDate BETWEEN t.StartDate AND t.EndDate 

					SET @CNTPreApproved = (SELECT DISTINCT(COUNT(P.NodeId)) FROM @TempNodePredecessors P 
											WHERE P.NodeId IN (SELECT SI.NodeIdPre FROM @TempNodeSegmentIntersect SI)) 

					SET @Approved = CASE WHEN @CNTPreApproved = @CNTPre THEN 1 ELSE 0 END;
				END
			END
		END
	END
    RETURN CASE WHEN @Approved IS NULL THEN 0 ELSE @Approved END; 
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', 
	@value=N'This function takes information from a node and returns if its predecessor node is approved' , 
	@level0type=N'SCHEMA',
	@level0name=N'Admin', 
	@level1type=N'FUNCTION',
	@level1name=N'udf_PrecessorsIsApproved'
GO


