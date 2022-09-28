/*-- =================================================================================================================================
-- Author:          Microsoft  
-- Created Date:    August-26-2020
-- <Description>:	This Procedure is used to get first time nodes  </Description>
-- EXEC [Admin].[usp_GetFirstTimeNodes] 66791,'2020-05-31','2020-06-03'
-- ==================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetFirstTimeNodes] 
(
       @SegmentId          INT,
	   @StartDate		   DATETIME,
	   @EndDate			   DATETIME
)
AS 
  BEGIN 
		SELECT NodeId FROM [Admin].[NodeTag] NT
WHERE NT.[ElementId] = @SegmentId AND NT.[StartDate] <= @StartDate AND NT.[EndDate] >= @StartDate
AND NOT EXISTS (SELECT 1 FROM [Admin].[view_MovementInformation] WHERE ([SourceNodeId] = NT.NodeId OR [DestinationNodeId] = NT.NodeId) AND TicketId IS NOT NULL)
AND NOT EXISTS (SELECT 1 FROM [Admin].[view_InventoryInformation] WHERE [NodeId] = NT.NodeId AND TicketId IS NOT NULL)
  END
GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get first time nodes',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetFirstTimeNodes',
							@level2type = NULL,
							@level2name = NULL