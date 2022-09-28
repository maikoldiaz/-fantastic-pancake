/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jul-30-2020
-- Updated Date:	Aug-07-2020 -- Included CategoryElementId while fetching previous ticket
-- Updated Date: 	Aug-25-2020    Included OwnerId
-- Updated Date:    Sep-18-2020  Added a variable
-- Updated Date:    Sep-21-2020  Changed code to improve performace
                                 1. Instead of "IN" operator used inner join to delete the records
								 2. Instead of using tables repeatedly used temp tables
								 3. Used to NOLOCK to avoid dead locks
-- Updated Date:	Oct-23-2020 Fixed the issue for failed ticket status
-- <Description>:	This Procedure is used to get the Delta Official Inventories Data based on the input of TicketId & NodeList</Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetDeltaOfficialInventory]
(
	   @TicketId				INT,
	   @NodeList				[Admin].[NodeListType]	READONLY	   
)
AS
BEGIN

		IF OBJECT_ID('tempdb..#TempDeltaNode')IS NOT NULL
		DROP TABLE #TempDeltaNode

		IF OBJECT_ID('tempdb..#TempDeltaNodePrevTicket')IS NOT NULL
		DROP TABLE #TempDeltaNodePrevTicket

		IF OBJECT_ID('tempdb..#TempOutputData')IS NOT NULL
		DROP TABLE #TempOutputData

		DECLARE  @TicketStartDate		  DATE
				,@TicketEndDate			  DATE
				,@PrevTicketStartDate	  DATE
				,@PrevTicketEndDate		  DATE
				,@TikcetSegmentId		  INT
				,@PreviousTicketEndDate DATE 
				
    
	    DROP TABLE IF EXISTS #Ticket;
		DROP TABLE IF EXISTS #TempTicketDeltaNode;

		SELECT StartDate
		      ,EndDate
			  ,CategoryElementId
			  ,TicketTypeId
			  ,TicketId
		  INTO #Ticket
		  FROM [Admin].[Ticket] WITH (NOLOCK)
		
		
		SELECT  @TicketStartDate = StartDate
			   ,@TicketEndDate   = EndDate
			   ,@TikcetSegmentId = CategoryElementId
		FROM #Ticket
		WHERE TicketId = @TicketId

	    SET @PreviousTicketEndDate = DATEADD(DAY,-1,@TicketStartDate)

		SELECT TOP 1  @PrevTicketStartDate = [StartDate]
					, @PrevTicketEndDate   = [EndDate]			
		FROM #Ticket
		WHERE TicketTypeId      = 5
		  AND [StartDate]      != @TicketStartDate 
		  AND [EndDate]        != @TicketEndDate
		  AND TicketId          < @TicketId
		  AND CategoryElementId = @TikcetSegmentId
		ORDER BY TicketId DESC

		SELECT Dnode.NodeId
			  ,Dnode.TicketId
			  ,Dnode.LastApprovedDate
			  ,Dnode.[Status]
			  ,Tic.StartDate
			  ,Tic.EndDate
			  ,Tic.CategoryElementId
		INTO #TempTicketDeltaNode
		FROM [Admin].[DeltaNode] Dnode  WITH (NOLOCK)
		INNER JOIN #Ticket Tic
		ON Dnode.TicketId = Tic.TicketId
		INNER JOIN @NodeList NL
		ON Dnode.NodeId = NL.NodeId
		WHERE Tic.CategoryElementId = @TikcetSegmentId
		  AND Dnode.[Status] IN (3,9,10,11,12,13)
		
		SELECT Dnode.NodeId
			  ,Dnode.TicketId
			  ,Dnode.LastApprovedDate
		INTO #TempDeltaNodePrevTicket
		FROM #TempTicketDeltaNode Dnode
		WHERE Dnode.StartDate         = @PrevTicketStartDate
		  AND Dnode.EndDate           = @PrevTicketEndDate
		  AND Dnode.CategoryElementId = @TikcetSegmentId
		  AND Dnode.[Status]          = 9--Approved
		
		SELECT Dnode.NodeId
			  ,Dnode.TicketId
			  ,Dnode.LastApprovedDate
		INTO #TempDeltaNode
		FROM #TempTicketDeltaNode Dnode
	   WHERE Dnode.LastApprovedDate IS NOT NULL
		 AND Dnode.[Status] IN (10,11,12,3)--Reopened, Rejected, Delta, Failed
		

		SELECT  Mov.MovementTransactionId
			   ,Own.OwnerId	AS MovementOwnerId
			   ,Mov.SourceNodeId
			   ,Mov.DestinationNodeId	   
			   ,Mov.SourceProductId
			   ,Mov.DestinationProductId
			   ,Mov.MeasurementUnit
			   ,Mov.SegmentId
			   ,Mov.OperationalDate
			   ,ISNULL(Mov.SourceNodeId,Mov.DestinationNodeId) AS NodeId
			   ,ISNULL(Mov.SourceProductId,Mov.DestinationProductId) AS ProductID
			   ,Own.OwnerId AS OwnerId
			   ,CASE WHEN Mov.SourceNodeId IS NOT NULL
					THEN
						CASE WHEN Own.OwnershipValueUnit = '%'
							THEN ((Own.[OwnershipValue]*Mov.NetStandardVolume) /100)* -1
							ELSE Own.[OwnershipValue] * -1
							END
					ELSE
						CASE WHEN Own.OwnershipValueUnit = '%'
							THEN (Own.[OwnershipValue]*Mov.NetStandardVolume)/100	
							ELSE Own.[OwnershipValue]
							END
					END AS OwnershipVolume
		INTO #TempOutputData
		FROM [Admin].[view_MovementInformation] Mov WITH (NOLOCK)
		INNER JOIN Offchain.[Owner] Own WITH (NOLOCK)
		ON Own.MovementTransactionId = Mov.MovementTransactionID
		LEFT JOIN #TempDeltaNode Sn
		ON Mov.SourceNodeId = Sn.NodeId
		LEFT JOIN #TempDeltaNode Dn
		ON Mov.DestinationNodeId = Dn.NodeId
		LEFT JOIN #TempDeltaNodePrevTicket Psn
		ON Mov.SourceNodeId = Psn.NodeId
		LEFT JOIN #TempDeltaNodePrevTicket Pdn
		ON Mov.DestinationNodeId = Pdn.NodeId
		WHERE (
				(   Mov.OfficialDeltaMessageTypeId IN (1,2)--Need to Text As Comment   
				AND Mov.IsSystemGenerated = 1 
				AND Mov.OfficialDeltaTicketId IS NOT NULL
				)
				OR Mov.SourceSystemId = 189
			  )
		AND (
		     (
			   (   Mov.SourceNodeId IN (SELECT NodeId FROM #TempDeltaNode) 
				OR Mov.DestinationNodeId IN (SELECT NodeId FROM #TempDeltaNode)
			   )
				AND (Mov.CreatedDate < Sn.LastApprovedDate OR Mov.CreatedDate < Dn.LastApprovedDate)
				AND (Mov.OperationalDate = @PreviousTicketEndDate OR Mov.OperationalDate = @TicketEndDate)
			  ) 
			 OR
				((Mov.SourceNodeId IN (SELECT NodeId FROM #TempDeltaNodePrevTicket)
				OR Mov.DestinationNodeId IN (SELECT NodeId FROM #TempDeltaNodePrevTicket))
				AND Mov.OperationalDate = @PreviousTicketEndDate
				AND (Mov.CreatedDate < Psn.LastApprovedDate OR Mov.CreatedDate < Pdn.LastApprovedDate))
			 )

		UPDATE Mov 
		   SET OfficialDeltaTicketId = @TicketId
		  FROM [Offchain].[Movement] Mov
		  JOIN #TempOutputData Temp
		    ON Mov.MovementTransactionId = Temp.MovementTransactionId

		SELECT MovementTransactionId
		      ,MovementOwnerId
		      ,SourceNodeId
		      ,DestinationNodeId	   
		      ,SourceProductId
		      ,DestinationProductId
		      ,MeasurementUnit
		      ,SegmentId
		      ,OperationalDate
		      ,NodeId
		      ,ProductID
		      ,OwnerId
		      ,OwnershipVolume
		FROM #TempOutputData
		
END

GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Delta Official Inventories Data based on the input of TicketId & NodeList',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetDeltaOfficialInventory',
							@level2type = NULL,
							@level2name = NULL