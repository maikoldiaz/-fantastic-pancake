/*-- ============================================================================================================================
-- Author:          IG Services
-- Created Date:    Ene-02-2022
-- <Description>:   This Procedure is used to find nodes in state different from deltas </Description>
-- EXEC [Admin].[usp_ValidateNodesStateDifferentDeltas]
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_ValidateNodesStateDifferentDeltas]
(
	@TicketId INT
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

				IF OBJECT_ID('tempdb..#TempTicket')IS NOT NULL
				DROP TABLE #TempTicket
				
				SELECT 
					Admin.Ticket.CategoryElementId,
					Admin.Ticket.StartDate,
					Admin.Ticket.EndDate
				INTO #TempTicket 
				FROM Admin.Ticket
				WHERE TicketId = @TicketId

				IF((SELECT COUNT(*)
				FROM #TempTicket) <= 0)
				BEGIN
					RAISERROR('El Ticket no es valido', 16, 1)
				END

				-- Get nodes in state different from deltas
				SELECT
					Node.Name                               AS NodeName
				, OwnershipNodeStatusType.Name              AS NodeStatus
				INTO #TempConsolidationNodes
				FROM Admin.DeltaNode DeltaNode
					LEFT JOIN Admin.NodeTag NodeTag
					ON NodeTag.NodeId = DeltaNode.NodeId
					LEFT JOIN Admin.Node Node
					ON Node.NodeId = DeltaNode.NodeId
					LEFT JOIN Admin.OwnershipNodeStatusType OwnershipNodeStatusType
					ON OwnershipNodeStatusType.OwnershipNodeStatusTypeId = DeltaNode.Status
					LEFT JOIN Admin.Ticket Ticket
					ON DeltaNode.TicketId = Ticket.TicketId
				WHERE NodeTag.ElementId = (SELECT #TempTicket.CategoryElementId FROM #TempTicket)
					AND (
						DeltaNode.Status NOT IN (12, 3) -- Valid states: 12 Deltas, 3 Fallido
						OR 
						DeltaNode.LastApprovedDate IS NOT NULL)
					AND Ticket.StartDate >= (SELECT #TempTicket.StartDate FROM #TempTicket)
					AND Ticket.EndDate <= (SELECT #TempTicket.EndDate FROM #TempTicket)

				SELECT * FROM #TempConsolidationNodes

		COMMIT TRANSACTION
	END TRY
  BEGIN CATCH
             IF @@TRANCOUNT > 0
                    ROLLBACK TRANSACTION;
             THROW
       END CATCH
END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to find nodes in state different from deltas',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_ValidateNodesStateDifferentDeltas',
    @level2type = NULL,
    @level2name = NULL