/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jul-07-2020
					Jul-14-2020     Modified Filter Logic
					Jul-16-2020		Renamed SP to usp_UpdateNodeAndGetDeltaMovements and Added new login for transaction as per PBI 32131
					Jul-20-2020		Added 'GO' at the end
					Sep-18-2020     Changed code to improve the performance
					Sep-19-2020     Reverting the code
-- <Description>:	This Procedure is used to get the Pending Movements Based on TicketId and NodeList</Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_UpdateNodeAndGetDeltaMovements]
(
	   @TicketId				INT,
	   @NodeList				[Admin].[NodeListType]	READONLY
)
AS
BEGIN

		IF OBJECT_ID('tempdb..#tempdeltanodescenario1')IS NOT NULL
		DROP TABLE #tempdeltanodescenario1

		IF OBJECT_ID('tempdb..#tempdeltanodescenario2')IS NOT NULL
		DROP TABLE #tempdeltanodescenario2

		IF OBJECT_ID('tempdb..#tempdeltanodescenario3')IS NOT NULL
		DROP TABLE #tempdeltanodescenario3

		IF OBJECT_ID('tempdb..#MovementsToDelete')IS NOT NULL
		DROP TABLE #MovementsToDelete

		IF OBJECT_ID('tempdb..#tempdeltanodewithapproval')IS NOT NULL
		DROP TABLE #tempdeltanodewithapproval

		IF OBJECT_ID('tempdb..#tempdeltanodewithoutapproval')IS NOT NULL
		DROP TABLE #tempdeltanodewithoutapproval

		DECLARE  @TicketStartDate		DATE
				,@TicketEndDate			DATE
				,@PrevTicketStartDate	DATE
				,@PrevTicketEndDate		DATE
				,@TikcetSegmentId		INT

		SELECT   @TicketStartDate = StartDate
				,@TicketEndDate   = EndDate
				,@TikcetSegmentId = CategoryElementId
		FROM Admin.Ticket
		WHERE TicketId = @TicketId

		SELECT TOP 1 @PrevTicketStartDate = [StartDate]
			       , @PrevTicketEndDate = [EndDate]
		FROM Admin.Ticket 
		WHERE [StartDate] != @TicketStartDate AND [EndDate] != @TicketEndDate AND TicketTypeId = 5
		ORDER BY TicketId DESC

	BEGIN TRY

		BEGIN TRANSACTION

			-- If there are nodes in Deltas or Rejected state that have not been previously approved (Nodes without approvals):
			SELECT * INTO #tempdeltanodescenario1
			FROM
			(			
				SELECT  d.DeltaNodeId,
						d.LastModifiedDate,
						d.NodeId,
						d.Status,
						d.TicketId
				FROM Admin.DeltaNode d
				INNER JOIN Admin.Ticket t 
				ON t.TicketId = d.TicketId
				WHERE CONVERT(DATE, t.StartDate)= @TicketStartDate
				AND CONVERT(DATE, t.EndDate)= @TicketEndDate
				AND t.CategoryElementId = @TikcetSegmentId
				AND t.TicketId <> @TicketId
				AND (d.Status = 12 OR d.Status = 10)
				AND d.LastApprovedDate IS NULL
				AND d.NodeId IN (SELECT NodeId FROM @NodeList)
			) a

			-- Find nodes with approvals in the previous period
			SELECT dn.[NodeId], dn.LastApprovedDate
			INTO #tempdeltanodewithapproval
			FROM [Admin].[DeltaNode] dn
			INNER JOIN [Admin].[Ticket] t
			ON dn.TicketId = t.TicketId
			WHERE dn.NodeId IN (SELECT NodeId FROM #tempdeltanodescenario1) AND
			t.StartDate = @PrevTicketStartDate AND t.EndDate = @PrevTicketEndDate
			AND dn.LastApprovedDate IS NOT NULL

			SELECT NodeId INTO #tempdeltanodewithoutapproval
			FROM #tempdeltanodescenario1 
			WHERE [NodeId] NOT IN (SELECT NODEId FROM #tempdeltanodewithapproval)

			-- Nodes with approvals in current period
			SELECT * INTO #tempdeltanodeswithapprovaltobeexcluded
			FROM
			(			
				SELECT d.DeltaNodeId,
				d.LastModifiedDate,
				d.NodeId,
				d.Status,
				d.TicketId,
				d.LastApprovedDate
				FROM Admin.DeltaNode d
				INNER JOIN Admin.Ticket t 
				ON t.TicketId = d.TicketId
				WHERE
				Convert(DATE, t.StartDate)= @TicketStartDate
				AND Convert(DATE, t.EndDate)= @TicketEndDate
				AND t.CategoryElementId = @TikcetSegmentId
				AND t.TicketId <> @TicketId
				AND (d.Status = 12 OR d.Status = 10 OR d.Status = 3 OR d.Status = 11)
				AND d.LastApprovedDate IS NOT NULL
				AND d.NodeId IN (SELECT NodeId FROM @NodeList)
			) a

			--Movements originated by inventory deltas (OfficialDeltaMessageTypeId equal to "OfficialInventoryDelta" or "ConsolidatedInventoryDelta"),
			--		where the source or destination node is equal to one of the nodes without approvals,
			--		the operational date is equal to end date of the period.
			--Movements originated by inventory deltas (OfficialDeltaMessageTypeId equal to "OfficialInventoryDelta" or "ConsolidatedInventoryDelta"),
			--		where the source or destination node is equal to one of the nodes without approvals, 
			--		the operational date is equal to start date of the period minus one day, 
			--		and the creation date is greater than the node's approval date in the previous period (if the node was approved in the previous period). 

			SELECT  Mov.[MovementTransactionID]	AS [MovementTransactionID] INTO #MovementsToDelete
			FROM Admin.view_MovementInformation Mov
			LEFT JOIN #tempdeltanodewithapproval sm
			ON sm.NodeId = Mov.SourceNodeId
			LEFT JOIN #tempdeltanodewithapproval dm
			ON dm.NodeId = Mov.DestinationNodeId
			WHERE ((Mov.OfficialDeltaMessageTypeId IN (1,2) AND Mov.IsSystemGenerated = 1 AND Mov.OfficialDeltaTicketId IS NOT NULL)  --OfficialInventoryDelta,ConsolidatedInventoryDelta
					OR Mov.SourceSystemId = 189) --ManualInvOficial
			AND Mov.SegmentId = @TikcetSegmentId
			AND ((Mov.OperationalDate = @TicketEndDate
					AND (Mov.SourceNodeId IN (SELECT NodeId FROM #tempdeltanodewithoutapproval) OR Mov.DestinationNodeId IN (SELECT NodeId FROM #tempdeltanodewithoutapproval))
					AND (Mov.SourceNodeId NOT IN (SELECT NodeId from #tempdeltanodeswithapprovaltobeexcluded) AND Mov.DestinationNodeId NOT IN (SELECT NodeId from #tempdeltanodeswithapprovaltobeexcluded))) 
						OR 
					((Mov.OperationalDate = DATEADD(DAY,-1,@TicketStartDate) AND 
					(Mov.SourceNodeId IN (SELECT NodeId FROM #tempdeltanodewithapproval) OR Mov.DestinationNodeId IN (SELECT NodeId FROM #tempdeltanodewithapproval))
					AND (Mov.SourceNodeId NOT IN (SELECT NodeId from #tempdeltanodeswithapprovaltobeexcluded) AND Mov.DestinationNodeId NOT IN (SELECT NodeId from #tempdeltanodeswithapprovaltobeexcluded)))
					AND (Mov.CreatedDate > sm.LastApprovedDate OR Mov.CreatedDate > dm.LastApprovedDate))
						OR
					(Mov.OperationalDate = DATEADD(DAY,-1,@TicketStartDate) AND 
					(Mov.SourceNodeId IN (SELECT NodeId FROM #tempdeltanodewithoutapproval) OR Mov.DestinationNodeId IN (SELECT NodeId FROM #tempdeltanodewithoutapproval))
					AND (Mov.SourceNodeId NOT IN (SELECT NodeId from #tempdeltanodeswithapprovaltobeexcluded) AND Mov.DestinationNodeId NOT IN (SELECT NodeId from #tempdeltanodeswithapprovaltobeexcluded))))

			INSERT INTO  #MovementsToDelete
			SELECT  Mov.[MovementTransactionID]	AS [MovementTransactionID]
			FROM Admin.view_MovementInformation Mov
			INNER JOIN Offchain.MovementPeriod MovPeriod
			ON Mov.[MovementTransactionId] = MovPeriod.[MovementTransactionId]
			WHERE ((Mov.OfficialDeltaMessageTypeId IN (3,4) AND Mov.IsSystemGenerated = 1 AND Mov.OfficialDeltaTicketId IS NOT NULL)--OfficialMovementDelta,ConsolidatedMovementDelta
					OR Mov.SourceSystemId = 190) --ManualMovOficial
			AND Mov.SegmentId = @TikcetSegmentId
			AND MovPeriod.StartTime = @TicketStartDate
			AND MovPeriod.EndTime = @TicketEndDate
			AND (Mov.SourceNodeId IN (SELECT NodeId FROM #tempdeltanodescenario1) OR Mov.DestinationNodeId IN (SELECT NodeId FROM #tempdeltanodescenario1))
			AND (Mov.SourceNodeId NOT IN (SELECT NodeId from #tempdeltanodeswithapprovaltobeexcluded) AND Mov.DestinationNodeId NOT IN (SELECT NodeId from #tempdeltanodeswithapprovaltobeexcluded))

			--Delete nodes without approvals from previous tickets from the same period.

			DELETE FROM Admin.DeltaNodeError 
			WHERE DeltaNodeId IN (SELECT DeltaNodeId FROM #tempdeltanodescenario1)

			DELETE FROM Admin.DeltaNode
			WHERE DeltaNodeId IN (SELECT DeltaNodeId FROM #tempdeltanodescenario1)


			--If there are Nodes in the Deltas or Rejected state, which have been previously approved (Nodes with approvals)

			-- Nodes with approvals in current period
			SELECT * INTO #tempdeltanodescenario2
			FROM
			(			
				SELECT d.DeltaNodeId,
				d.LastModifiedDate,
				d.NodeId,
				d.Status,
				d.TicketId,
				d.LastApprovedDate
				FROM Admin.DeltaNode d
				INNER JOIN Admin.Ticket t 
				ON t.TicketId = d.TicketId
				WHERE
				Convert(DATE, t.StartDate)= @TicketStartDate
				AND Convert(DATE, t.EndDate)= @TicketEndDate
				AND t.CategoryElementId = @TikcetSegmentId
				AND t.TicketId <> @TicketId
				AND (d.Status = 12 OR d.Status = 10 OR d.Status = 3)
				AND d.LastApprovedDate IS NOT NULL
				AND d.NodeId IN (SELECT NodeId FROM @NodeList)
			) a

			--Delete movements originated by inventory deltas (OfficialDeltaMessageTypeId equal to "OfficialInventoryDelta" or "ConsolidatedInventoryDelta"),
			--which were registered after the last approval date of the node, where the source or destination node is equal to the node with approvals,
			--the operational date is equal to the initial date of the period minus one day or equal to the end date of the period.
			--Delete movements originated by manual inventory deltas (Source system ID equal to System ID: "ManualInvOficial"), 
			--which were registered after the last approval date of the node, where the source or destination node is equal to the node with approvals,
			--the operational date is equal to the initial date of the period minus one day or equal to the end date of the period.
			INSERT INTO #MovementsToDelete
			SELECT  Mov.[MovementTransactionID]	AS [MovementTransactionID]
			FROM Admin.view_MovementInformation Mov
			LEFT JOIN #tempdeltanodescenario2 sm
			ON sm.NodeId = Mov.SourceNodeId
			LEFT JOIN #tempdeltanodescenario2 dm
			ON dm.NodeId = Mov.DestinationNodeId
			WHERE ((Mov.OfficialDeltaMessageTypeId IN (1,2) AND Mov.IsSystemGenerated = 1 AND Mov.OfficialDeltaTicketId IS NOT NULL) --OfficialInventoryDelta,ConsolidatedInventoryDelta
					OR Mov.SourceSystemId = 189) --ManualInvOficial
			AND Mov.SegmentId = @TikcetSegmentId
			AND (
					(Mov.OperationalDate = @TicketEndDate OR Mov.OperationalDate = DATEADD(DAY,-1,@TicketStartDate))
					AND (Mov.SourceNodeId IN (SELECT NodeId FROM #tempdeltanodescenario2) OR Mov.DestinationNodeId IN (SELECT NodeId FROM #tempdeltanodescenario2))
					AND (Mov.CreatedDate > ISNULL(sm.LastApprovedDate, cast('9999-12-31 00:00:00.000' as datetime)) OR Mov.CreatedDate > ISNULL(dm.LastApprovedDate, cast('9999-12-31 00:00:00.000' as datetime)))
				)

			INSERT INTO  #MovementsToDelete
			SELECT  Mov.[MovementTransactionID]	AS [MovementTransactionID]
			FROM Admin.view_MovementInformation Mov
			INNER JOIN Offchain.MovementPeriod MovPeriod
			ON Mov.[MovementTransactionId] = MovPeriod.[MovementTransactionId]
			LEFT JOIN #tempdeltanodescenario2 sm
			ON sm.NodeId = Mov.SourceNodeId
			LEFT JOIN #tempdeltanodescenario2 dm
			ON dm.NodeId = Mov.DestinationNodeId
			WHERE ((Mov.OfficialDeltaMessageTypeId IN (3,4) AND Mov.IsSystemGenerated = 1 AND Mov.OfficialDeltaTicketId IS NOT NULL)--OfficialMovementDelta,ConsolidatedMovementDelta
				OR Mov.SourceSystemId = 190) --ManualMovOficial
			AND Mov.SegmentId = @TikcetSegmentId
			AND MovPeriod.StartTime = @TicketStartDate
			AND MovPeriod.EndTime = @TicketEndDate
			AND (Mov.SourceNodeId IN (SELECT NodeId FROM #tempdeltanodescenario2) OR Mov.DestinationNodeId IN (SELECT NodeId FROM #tempdeltanodescenario2))
			AND (Mov.CreatedDate > ISNULL(sm.LastApprovedDate, cast('9999-12-31 00:00:00.000' as datetime)) OR Mov.CreatedDate > ISNULL(dm.LastApprovedDate, cast('9999-12-31 00:00:00.000' as datetime)))

			UPDATE [Admin].DeltaNode SET [Status] = 1, [TicketId] = @TicketId, LastModifiedDate = Admin.udf_GetTrueDate(), LastModifiedBy = 'System'
			WHERE DeltaNodeId IN (SELECT DeltaNodeId FROM #tempdeltanodescenario2)

			--If there are nodes in reopened state:
				--Register nodes in reopened state in the state historical table. Store the node, its status, its ticket number, and the status date.
				--Update the node ticket by the new official delta ticket of the segment and update the status of the nodes to processing.

			SELECT * INTO #tempdeltanodescenario3
			FROM
			(			
				SELECT d.DeltaNodeId,
				d.LastModifiedDate,
				d.NodeId,
				d.Status,
				d.TicketId,
				d.LastApprovedDate
				FROM Admin.DeltaNode d
				INNER JOIN Admin.Ticket t 
				ON t.TicketId = d.TicketId
				WHERE CONVERT(DATE, t.StartDate)= @TicketStartDate
				AND CONVERT(DATE, t.EndDate)= @TicketEndDate
				AND t.CategoryElementId = @TikcetSegmentId
				AND t.TicketId <> @TicketId
				AND d.Status = 11
				AND d.NodeId IN (SELECT NodeId FROM @NodeList)
			) a

			UPDATE [Admin].DeltaNode SET [Status] = 1, [TicketId] = @TicketId, LastModifiedDate = Admin.udf_GetTrueDate(), LastModifiedBy = 'System'
			WHERE DeltaNodeId IN (SELECT DeltaNodeId FROM #tempdeltanodescenario3)

			--Insert new records only for nodes which have not been previously approved, and not in reopened state
			INSERT INTO Admin.DeltaNode 
			(
				NodeId, 
				TicketId, 
				Status,
				CreatedDate,
				CreatedBy
			) 
			SELECT NodeId, 
				   @TicketId AS TicketId, 
				   1		 AS [Status],
				   [Admin].udf_GetTrueDate(),
				   'System'
			FROM @NodeList
			WHERE NodeId NOT IN (SELECT NodeId FROM #tempdeltanodescenario3)
			AND NodeId NOT IN (SELECT NodeId FROM #tempdeltanodescenario2)

			SELECT [MovementTransactionID] FROM #MovementsToDelete

		COMMIT TRANSACTION

	END TRY

	
    BEGIN CATCH
			IF @@TRANCOUNT > 0
			       ROLLBACK TRANSACTION;
			THROW
    END CATCH

END

GO

EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Pending Movements Based on TicketId and NodeList',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_UpdateNodeAndGetDeltaMovements',
							@level2type = NULL,
							@level2name = NULL