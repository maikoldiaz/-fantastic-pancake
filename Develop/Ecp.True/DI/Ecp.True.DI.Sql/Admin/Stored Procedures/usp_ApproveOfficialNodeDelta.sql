/*-- ========================================================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	July-08-2020
-- Updated Date:	July-15-2020 Changed status to 8 for 'Enviado a aprobación'
-- Updated Date:	July-21-2020 Corrected logic for send for approval check
-- Updated Date:	July-22-2020 Included [Order] as per UAT bug 67369
-- Updated Date:	July-30-2020 Changed status to 'Approved' '8' as per PBI 31879
-- Updated Date:	Aug-13-2020	 Included logic to only consider predecessors with Ticket status as 'Delta' (Status=4)
-- Updated Date:	Oct-14-2020	 Added period validation to check predecessor nodes based on delta node period
-- <Description>:   PBI 32275. This procedure is to validate the Nodes send to approval
--EXEC [Admin].[usp_ApproveOfficialNodeDelta] 208598, 51599
-- ========================================================================================================================================================================*/


CREATE PROCEDURE [Admin].[usp_ApproveOfficialNodeDelta]
(
	@SegmentID INT,
	@NodeID INT,
	@DeltaNodeID INT
)
AS
BEGIN

	IF OBJECT_ID('tempdb..#TempNodePredecessors')IS NOT NULL
			DROP TABLE #TempNodePredecessors;
	
	IF OBJECT_ID('tempdb..#TempNodeSegmentIntersect')IS NOT NULL
			DROP TABLE #TempNodeSegmentIntersect;
	
	
--Table to store predecessors of input node wih their segment
	CREATE TABLE #TempNodePredecessors
	(
	NodeId INT,
	SegmentId INT
	);
		
	--Table to store predecessor nodes having same segment as input segment
	CREATE TABLE #TempNodeSegmentIntersect
	(
	NodeIdPre INT,
	SegmentId INT,
	TicketId INT
	);
	
	DECLARE @StartDate	DATETIME
			,@EndDate	DATETIME

	SELECT @StartDate = StartDate
		  ,@EndDate = EndDate
	FROM [Admin].[view_DeltaNode]
	WHERE DeltaNodeId = @DeltaNodeID;
	
	WITH Predecessors 
	AS
	(
    SELECT  SourceNodeId,
			DestinationNodeId--Get the All the
	FROM Admin.NodeConnection C
	INNER JOIN Admin.Node NDest On C.DestinationNodeId=NDest.NodeId
	INNER JOIN Admin.Node NSrc On C.SourceNodeId=NSrc.NodeId
	WHERE	NDest.[Order] > NSrc.[Order]
	AND		DestinationNodeId = @NodeID 
    
    UNION ALL

    SELECT   N.SourceNodeId,
			 N.DestinationNodeId
    FROM   Admin.NodeConnection N
	INNER JOIN Admin.Node NDest On N.DestinationNodeId=NDest.NodeId
	INNER JOIN Admin.Node NSrc On N.SourceNodeId=NSrc.NodeId
    INNER JOIN Predecessors P ON P.SourceNodeId = N.DestinationNodeId
	WHERE NDest.[Order] > NSrc.[Order]
	)

	--Predecessors for input node and their segment
	INSERT INTO  #TempNodePredecessors (NodeId, SegmentId)
	SELECT  p.SourceNodeId, ISNULL(n.ElementId,0) 
	FROM  Predecessors p 
	JOIN admin.NodeTag n on p.SourceNodeId=n.NodeId; -- Nodes only having segment
	
	--Check if Segment of Predecessor nodes are present in Segment of Input node
	INSERT INTO #TempNodeSegmentIntersect(NodeIdPre,SegmentId, TicketId)
	Select a.NodeId, a.SegmentId, t.TicketId 
	from #TempNodePredecessors a
	JOIN Admin.view_DeltaNode b
	ON a.NodeId = b.NodeId
	JOIN Admin.Ticket t
	ON b.TicketId = t.TicketId
	AND t.Status=4
	WHERE a.SegmentId = @SegmentID
	AND t.StartDate = @StartDate AND t.EndDate = @EndDate

	DECLARE 
		@CNTPre INT,
		@CNTSegInt INT,
		@ReturnFlag INT
	
	SET @CNTPre = (SELECT COUNT(NodeId) FROM #TempNodePredecessors)
	SET @CNTSegInt = (SELECT COUNT(SegmentId) FROM #TempNodeSegmentIntersect)
	

	--If(Ticket+Node Has No Predecessor)
	IF @CNTPre=0
	BEGIN
	  SET @ReturnFlag = 1
	END

	--If(Ticket+Node Has Predecessor which all are of Different Segment)
	ELSE IF @CNTPre>0 AND @CNTSegInt=0
	BEGIN
		SET @ReturnFlag=1
	END

	--If(Ticket+Node Has Predecessor of which any one is of same Segment and all State is Send to Approval)
	--For the NodeList Verify If  the Segment matches with Input Node Segment then ONly for those 
	--Check in DeltaNode table in the Column SendToApproval--All the NodeList Should be Flagged with Send to Approval
	ELSE IF @CNTPre>0 AND @CNTSegInt>0 AND 
	NOT EXISTS (	
		SELECT  1
		FROM  #TempNodeSegmentIntersect Nd 
		JOIN Admin.DeltaNode DN 	
		ON DN.NodeId=Nd.NodeIdPre
		AND Nd.TicketId = DN.TicketId
		WHERE DN.Status<>9 -- Changed to 'Approved' status

		)
	BEGIN
	  	SET @ReturnFlag=1	 
	END

	--The node cannot be sent for approval because there are nodes in the chain that must be approved first.
	ELSE
	BEGIN
		SET @ReturnFlag= 0
	END

	IF @ReturnFlag = 0
	BEGIN
		RAISERROR('Approve Official Node Delta Fail',15,1)
	END
 END


 GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PBI 32275. This procedure is to validate the Nodes send to approval',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_ApproveOfficialNodeDelta',
    @level2type = NULL,
    @level2name = NULL

 
