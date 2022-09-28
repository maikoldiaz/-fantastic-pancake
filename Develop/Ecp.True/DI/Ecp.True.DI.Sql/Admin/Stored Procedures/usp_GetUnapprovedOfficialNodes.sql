/*-- =============================================================================================================================================================================
-- Author:          Microsoft
-- Created Date:	July-28-2020
-- Update 1 Date:   July-28-2020 -- order by DeltaNodeId and select top 1 for every node, remove ticket status check.
-- Update 2 Date:   Aug-03-2020  -- update status check logic, add operation date column
-- <Description>:	This Procedure is used to get unapproved official delta nodes for selected period. </Description>
-- ==============================================================================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_GetUnapprovedOfficialNodes] 
(
		 @SegmentId			INT
        ,@NodeId            INT
		,@StartDate			DATE
		,@EndDate			DATE
)
AS 
BEGIN

    ;WITH CteUnapprovedNodes
	AS
	(
		SELECT   ND.Name            AS [NodeName]
                ,OWN.Name           AS [NodeStatus]
				,DN.Status          AS [StatusId]
                ,DN.CreatedDate     AS [OperationDate]    
			    ,ROW_NUMBER()OVER(PARTITION BY ND.NodeId
								  ORDER     BY DN.DeltaNodeId DESC)Rnum
		FROM Admin.Ticket TIC
        INNER JOIN Admin.DeltaNode DN
        ON DN.TicketId = TIC.TicketId
        INNER JOIN Admin.Node ND
        ON DN.NodeId = ND.NodeId
        INNER JOIN Admin.OwnershipNodeStatusType OWN
        ON OWN.OwnershipNodeStatusTypeId = DN.Status
        WHERE TIC.CategoryElementId = @SegmentId AND TIC.TicketTypeId = 5 AND 
        TIC.StartDate >= @StartDate AND TIC.EndDate <= @EndDate
        AND (@NodeId = 0 OR DN.NodeId = @NodeId) -- @NodeId = 0 for Todos
	)

    SELECT NodeName
          ,NodeStatus
          ,OperationDate
    FROM CteUnapprovedNodes
	WHERE Rnum = 1  AND StatusId != 9
    ORDER BY NodeName, NodeStatus
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get unapproved official delta nodes for selected period.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetUnapprovedOfficialNodes',
    @level2type = NULL,
    @level2name = NULL