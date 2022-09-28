/*-- ===============================================================================================================================================
-- Author:          Intergrupo
-- Created Date: 	Jun-03-2021
-- Updated Date: 	Jun-21-2021 Change status ticket and remove SourceSystemId validation
--				 	Jun-25-2021 Add MovementId and delete ownership volume field
--					Jul-02-2021 Modified filter date to comparate between official date
--					Sep-30-2021 Add columns OfficialDeltaTicketId, TicketId, SourceSystemId
--					Oct-8-2021 Add date validation
-- <Description>:	This Procedure is used to get the Delta Official Movements Data based on the input of TicketId & NodeList</Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetDeltaOfficialMovementsForNodes]
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
				,@TicketCreatedDate		DATETIME
				,@TicketSegmentId		INT

		SELECT StartDate
			  ,EndDate
			  ,CategoryElementId
			  ,TicketId
			  ,CreatedDate
		 INTO #Ticket
		 FROM [Admin].[Ticket] WITH (NOLOCK)

		SELECT @TicketStartDate = StartDate
			  ,@TicketEndDate   = EndDate
			  ,@TicketSegmentId = CategoryElementId
			  ,@TicketCreatedDate = CreatedDate
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
		  AND Tic.CategoryElementId = @TicketSegmentId
		  AND Dnode.LastApprovedDate IS NOT NULL
		  AND Dnode.[Status] IN (9)--Approved
		  AND Dnode.NodeId IN (SELECT n.NodeId FROM @NodeList n)
		

		SELECT  Mov.MovementTransactionId
			   ,Own.OwnerId	AS MovementOwnerID
			   ,Mov.SourceNodeId
			   ,Mov.DestinationNodeId	   
			   ,Mov.SourceProductId
			   ,Mov.DestinationProductId
			   ,Mov.MovementTypeId
			   ,Own.OwnerId AS OwnerId
			   ,own.OwnershipValue
			   ,own.OwnershipValueUnit
			   ,Mov.SegmentId
			   ,Mov.MeasurementUnit
			   ,Mov.OperationalDate
			   ,MovPrd.StartTime AS StartDate
			   ,MovPrd.EndTime AS EndDate
			   ,Mov.SourceProductTypeId
			   ,Mov.DestinationProductTypeId
			   ,Mov.GrossStandardVolume
			   ,Mov.NetStandardVolume
			   ,Mov.Version
			   ,Mov.MovementID as 'MovementId'
			   ,Mov.OfficialDeltaTicketId
			   ,Mov.TicketId
			   ,Mov.SourceSystemId
		INTO #TempOfficialMovementOutputData
		FROM [Admin].view_MovementInformation Mov  WITH (NOLOCK)
		INNER JOIN Offchain.[Owner] Own  WITH (NOLOCK)
		ON Own.MovementTransactionId = Mov.MovementTransactionID
		AND Mov.SegmentId    = @TicketSegmentId
		INNER JOIN Offchain.MovementPeriod MovPrd  WITH (NOLOCK)
		ON MovPrd.MovementTransactionID = Mov.MovementTransactionID
		AND MovPrd.StartTime BETWEEN @TicketStartDate AND @TicketEndDate
		AND MovPrd.EndTime BETWEEN @TicketStartDate AND @TicketEndDate
		LEFT JOIN #TempDeltaNode Sn
		ON Mov.SourceNodeId = Sn.NodeId
		LEFT JOIN #TempDeltaNode Dn
		ON Mov.DestinationNodeId = Dn.NodeId
		WHERE (   Mov.CreatedDate < Sn.LastApprovedDate 
		       OR Mov.CreatedDate < Dn.LastApprovedDate
			  )
		  AND  Mov.OfficialDeltaTicketId = @TicketId
		  AND (   Mov.SourceNodeId      IN (SELECT NodeId FROM #TempDeltaNode)
		  	   OR Mov.DestinationNodeId IN (SELECT NodeId FROM #TempDeltaNode)
			   )
		  AND Mov.CreatedDate > @TicketCreatedDate

		SELECT  MovementTransactionId
			   ,MovementOwnerID
			   ,SourceNodeId
			   ,DestinationNodeId	   
			   ,SourceProductId
			   ,DestinationProductId
			   ,MovementTypeId
			   ,OwnerId
			   ,OwnershipValue
			   ,OwnershipValueUnit
			   ,SegmentId
			   ,MeasurementUnit
			   ,OperationalDate
			   ,StartDate
			   ,EndDate
			   ,SourceProductTypeId
			   ,DestinationProductTypeId
			   ,GrossStandardVolume
			   ,NetStandardVolume
			   ,Version
			   ,MovementId
			   ,OfficialDeltaTicketId
			   ,TicketId
			   ,SourceSystemId
		FROM #TempOfficialMovementOutputData
END
GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Delta Official Movements Data based on the input of TicketId & NodeList',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetDeltaOfficialMovementsForNodes',
							@level2type = NULL,
							@level2name = NULL