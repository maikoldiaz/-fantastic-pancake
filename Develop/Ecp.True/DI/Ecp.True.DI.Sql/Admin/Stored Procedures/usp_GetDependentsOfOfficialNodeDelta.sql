/*-- ========================================================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	July-30-2020
-- Updated Date:	Oct-14-2020 -- Added period validation to get successors.
-- <Description>:   PBI 31879. This procedure is to get information of all the dependent successor nodes
-- ========================================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetDependentsOfOfficialNodeDelta]
(
	@DeltaNodeID INT
)
AS
BEGIN	
	DECLARE @NodeId    INT,
			@SegmentId INT,
			@StartDate DATETIME,
			@EndDate   DATETIME

	--Get Nodeid and SegmentId corresponding to input deltanode id
	SELECT DISTINCT @NodeId = NodeId, 
					@SegmentId = SegmentId,
					@StartDate = StartDate,
					@EndDate = EndDate
	FROM [Admin].[view_DeltaNode]
	WHERE DeltaNodeId = @DeltaNodeID	
	
	--Get Only 1 level successor for input's node,successor's state should be 'approved',get for higher or equal order
	--active nodes,no self connection

	--Return Dependent successor Node information
     SELECT DISTINCT view_DN.Segment		AS 'Segment', 
					 view_DN.NodeName		AS 'Node', 
					 view_DN.DeltaNodeId    AS 'DeltaNode'
	FROM Admin.NodeConnection C
	INNER JOIN Admin.Node NDest 
	ON C.DestinationNodeId=NDest.NodeId
	INNER JOIN Admin.Node NSrc 
	ON C.SourceNodeId=NSrc.NodeId
	INNER JOIN [Admin].[view_DeltaNode] view_DN --to get destination node details
	ON view_DN.NodeId = C.DestinationNodeId
	WHERE NDest.[Order] >= NSrc.[Order] --higher or equal order of successor node
	AND	SourceNodeId = @NodeId --source node
	AND	view_DN.SegmentId = @SegmentId --successor should belong to same source segment
	AND SourceNodeId <> DestinationNodeId --no self connection
	And view_DN.DeltaNodeStatus = 9 --Successor deltanode status is 'Approved'
	AND NDest.IsActive =1 --active successor node
	AND view_DN.StartDate = @StartDate AND view_DN.EndDate = @EndDate	
END				

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PBI 32275. This procedure is to get dependent Successor nodes',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetDependentsOfOfficialNodeDelta',
    @level2type = NULL,
    @level2name = NULL

 
