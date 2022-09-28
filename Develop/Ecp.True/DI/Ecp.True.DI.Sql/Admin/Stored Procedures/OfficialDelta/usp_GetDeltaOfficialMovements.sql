/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jul-30-2020
-- Updated Date: 	Aug-25-2020  Removed alias
-- Updated Date:    Sep-18-2020  Changed code to improve the performance
-- Updated Date:    Sep-19-2020  Reverting the code
-- Updated Date:    Sep-21-2020  Changed code to improve the performance
                                 1. Instead of "IN" operator used inner join to delete the records
								 2. Instead of using tables repeatedly used temp tables
								 3. Used to NOLOCK to avoid dead locks
-- <Description>:	This Procedure is used to get the Delta Official Movements Data based on the input of TicketId & NodeList</Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetDeltaOfficialMovements]
(
	   @TicketId				INT,
	   @NodeList				[Admin].[NodeListType]	READONLY	   
)
AS
BEGIN

		IF OBJECT_ID('tempdb..#TempDeltaNode')IS NOT NULL
		DROP TABLE #TempDeltaNode

		IF OBJECT_ID('tempdb..#TempOfficialMovementOutputData')IS NOT NULL
		DROP TABLE #TempOfficialMovementOutputData

		DROP TABLE IF EXISTS #Ticket;

		DECLARE  @TicketStartDate		DATE
				,@TicketEndDate			DATE
				,@TikcetSegmentId		INT

		SELECT StartDate
			  ,EndDate
			  ,CategoryElementId
			  ,TicketId
		 INTO #Ticket
		 FROM [Admin].[Ticket] WITH (NOLOCK)

		SELECT @TicketStartDate = StartDate
			  ,@TicketEndDate   = EndDate
			  ,@TikcetSegmentId = CategoryElementId
		 FROM #Ticket
		WHERE TicketId = @TicketId


		SELECT Dnode.NodeId
			  ,Dnode.TicketId
			  ,Dnode.LastApprovedDate
		INTO #TempDeltaNode
		FROM [Admin].DeltaNode Dnode WITH (NOLOCK)
		INNER JOIN #Ticket Tic
		ON Dnode.TicketId = Tic.TicketId
		WHERE Tic.StartDate         = @TicketStartDate
		  AND Tic.EndDate           = @TicketEndDate
		  AND Tic.CategoryElementId = @TikcetSegmentId
		  AND Dnode.LastApprovedDate IS NOT NULL
		  AND Dnode.[Status] IN (10,11,12,3)--Reopened, Rejected, Delta, Failed
		

		SELECT  Mov.MovementTransactionId
			   ,Own.OwnerId	AS MovementOwnerID
			   ,Mov.SourceNodeId
			   ,Mov.DestinationNodeId	   
			   ,Mov.SourceProductId
			   ,Mov.DestinationProductId
			   ,Mov.MovementTypeId
			   ,Own.OwnerId AS OwnerId
			   ,CASE WHEN Own.OwnershipValueUnit = '%'
						THEN (Own.[OwnershipValue]*Mov.NetStandardVolume)/100	
						ELSE Own.[OwnershipValue]
					END AS OwnershipVolume
			   ,Mov.SegmentId
			   ,Mov.MeasurementUnit
			   ,Mov.OperationalDate
			   ,MovPrd.StartTime AS StartDate
			   ,MovPrd.EndTime AS EndDate
			   ,Mov.SourceProductTypeId
			   ,Mov.DestinationProductTypeId
		INTO #TempOfficialMovementOutputData
		FROM [Admin].view_MovementInformation Mov  WITH (NOLOCK)
		INNER JOIN Offchain.[Owner] Own  WITH (NOLOCK)
		ON Own.MovementTransactionId = Mov.MovementTransactionID
		AND Mov.SegmentId    = @TikcetSegmentId
		INNER JOIN Offchain.MovementPeriod MovPrd  WITH (NOLOCK)
		ON MovPrd.MovementTransactionID = Mov.MovementTransactionID
		AND MovPrd.StartTime = @TicketStartDate
		AND MovPrd.EndTime   = @TicketEndDate
		LEFT JOIN #TempDeltaNode Sn
		ON Mov.SourceNodeId = Sn.NodeId
		LEFT JOIN #TempDeltaNode Dn
		ON Mov.DestinationNodeId = Dn.NodeId
		WHERE (   Mov.CreatedDate < Sn.LastApprovedDate 
		       OR Mov.CreatedDate < Dn.LastApprovedDate
			  )
		  AND (
				(    Mov.OfficialDeltaTicketId IS NOT NULL
				 AND Mov.IsSystemGenerated = 1 
				 AND Mov.OfficialDeltaMessageTypeId IN (3,4) --OfficialMovementDelta,ConsolidatedMovementDelta
				)
			   OR Mov.SourceSystemId = 190
			  ) --ManualMovOficial
		  AND (   Mov.SourceNodeId      IN (SELECT NodeId FROM #TempDeltaNode)
		  	   OR Mov.DestinationNodeId IN (SELECT NodeId FROM #TempDeltaNode)
			   )
		  
		UPDATE Mov 
		   SET OfficialDeltaTicketId = @TicketId
          FROM [Offchain].[Movement] Mov
		  JOIN #TempOfficialMovementOutputData Temp
		    ON Mov.MovementTransactionId = Temp.MovementTransactionId

		SELECT  MovementTransactionId
			   ,MovementOwnerID
			   ,SourceNodeId
			   ,DestinationNodeId	   
			   ,SourceProductId
			   ,DestinationProductId
			   ,MovementTypeId
			   ,OwnerId
			   ,OwnershipVolume
			   ,SegmentId
			   ,MeasurementUnit
			   ,OperationalDate
			   ,StartDate
			   ,EndDate
			   ,SourceProductTypeId
			   ,DestinationProductTypeId
		FROM #TempOfficialMovementOutputData

END

GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Delta Official Movements Data based on the input of TicketId & NodeList',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetDeltaOfficialMovements',
							@level2type = NULL,
							@level2name = NULL