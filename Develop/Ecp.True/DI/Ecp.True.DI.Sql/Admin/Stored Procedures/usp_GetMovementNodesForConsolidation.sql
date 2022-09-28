/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jul-13-2020
-- <Description>:	This Procedure is used to get the Consolidated Movement Nodes on SegmentId , StartDate, EndDate. </Description>
EXEC [Admin].[usp_GetMovementNodesForConsolidation] 145391,'2020-05-01', '2020-05-31'
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetMovementNodesForConsolidation]
(
	     @SegmentId				INT
		,@StartDate				DATE
		,@EndDate				DATE
)
AS
BEGIN
		SELECT DISTINCT 
				Mov.SourceNodeId
			   ,Mov.DestinationNodeId
		FROM Admin.view_MovementInformation Mov
		WHERE Mov.SegmentId = @SegmentId
		AND Mov.OperationalDate BETWEEN @StartDate AND @EndDate
		AND Mov.ScenarioId = 1 AND Mov.SourceMovementTransactionId IS NULL
END
GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Consolidated Movement Nodes on SegmentId , StartDate, EndDate.',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetMovementNodesForConsolidation',
							@level2type = NULL,
							@level2name = NULL