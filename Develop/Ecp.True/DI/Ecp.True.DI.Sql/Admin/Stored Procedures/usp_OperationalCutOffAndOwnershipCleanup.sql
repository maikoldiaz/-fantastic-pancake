/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date: 	April-06-2020
-- Updated Date:	April-08-2020
					Modified Code to Delete the Relevant records from MovementRelated tables( Offchain.Movement
																						     ,Offchain.MovementSource
																							 ,Offchain.MovementDestination
																							 ,Admin.MovementPeriod
																							 ,Admin.MovementContract
																							 ,Admin.MovementEvent
																							)
					August-28-2020 Modified Procedure code to Delete data from [MovementsByProductWithoutOwner]
-- <Description>:	This Procedure is used to clean records from Ownership, OwnershipNode, OwnershipNodeError based on TicketId. </Description>
-- EXEC [Admin].[usp_OperationalCutOffAndOwnershipCleanup] 6
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_OperationalCutOffAndOwnershipCleanup]
(
	    @TicketId			INT 
)
AS
BEGIN
       BEGIN TRY
             BEGIN TRANSACTION

				IF NOT EXISTS(SELECT 1 FROM [Admin].[Ticket] WHERE TicketId = @TicketId)
				BEGIN
					RAISERROR ('No such ticket exists',16,1)
					RETURN
				END

				--Check for the Matching
				DELETE FROM [Admin].[OwnershipNodeError]--TicketID
				WHERE EXISTS (SELECT 1 
							  FROM [Admin].[OwnershipNode] 
							  WHERE OwnershipNodeError.OwnershipNodeId = OwnershipNode.OwnershipNodeId
							  AND [OwnershipNode].TicketId = @TicketId
							 );

				DELETE FROM [Admin].OwnershipNode
				WHERE TicketId = @TicketId;

				DELETE FROM [Offchain].[Ownership]
				WHERE TicketId = @TicketId;

				IF OBJECT_ID('tempdb..#TempContractIdCleanUp')IS NOT NULL
				DROP TABLE #TempContractIdCleanUp

				IF OBJECT_ID('tempdb..#TempEventIdCleanUp')IS NOT NULL
				DROP TABLE #TempEventIdCleanUp

				/*****Delete the data from Movement Related tables Start*****/
				DELETE FROM Offchain.MovementSource
				WHERE EXISTS (SELECT 1 
							  FROM Offchain.Movement 
							  WHERE Movement.MovementTransactionId = MovementSource.MovementTransactionId
							  AND TicketId = @TicketId
							  AND IsSystemGenerated = 1
							 );

				DELETE FROM Offchain.MovementDestination
				WHERE EXISTS (SELECT 1 
							  FROM Offchain.Movement 
							  WHERE Movement.MovementTransactionId = MovementDestination.MovementTransactionId
							  AND   Movement.TicketId = @TicketId
							  AND   Movement.IsSystemGenerated = 1
							 );

				DELETE Own
				FROM Offchain.Movement Mov
				INNER JOIN offchain.Ownership Own
				ON Mov.MovementTransactionId = Own.MovementTransactionId
				WHERE Mov.TicketId = @TicketId
				AND Mov.IsSystemGenerated = 1

				SELECT [MovementEventId]
				INTO #TempEventIdCleanUp
				FROM Offchain.Movement
				WHERE TicketId = @TicketId
				AND IsSystemGenerated = 1

				SELECT [MovementContractId]
				INTO #TempContractIdCleanUp
				FROM Offchain.Movement
				WHERE TicketId = @TicketId
				AND IsSystemGenerated = 1

				DELETE MovPrd
				FROM Offchain.Movement Mov
				INNER JOIN offchain.MovementPeriod MovPrd
				ON Mov.MovementTransactionId = MovPrd.MovementTransactionId
				WHERE Mov.TicketId = @TicketId
				AND Mov.IsSystemGenerated = 1

				DELETE Mov
				FROM Offchain.Movement Mov
				WHERE Mov.TicketId = @TicketId
				AND Mov.IsSystemGenerated = 1

				DELETE FROM [Admin].MovementContract
				WHERE MovementContractId IN (SELECT MovementContractId From #TempContractIdCleanUp)

				DELETE FROM [Admin].MovementEvent
				WHERE MovementEventId IN (SELECT MovementEventId From #TempEventIdCleanUp)
				/*****Delete the data from Movement Related tables End*****/

				--Delete the data from UnBalance Table
				DELETE FROM Offchain.Unbalance
				WHERE TicketId = @TicketId

				DELETE FROM [Admin].[MovementsByProductWithoutOwner] 
				WHERE TicketId = @TicketId

				DELETE FROM [Admin].[KPIDataByCategoryElementNode]
				WHERE TicketId = @TicketId

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
    @value = N'This Procedure is used to clean records from Ownership, OwnershipNode, OwnershipNodeError based on TicketId.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_OperationalCutOffAndOwnershipCleanup',
    @level2type = NULL,
    @level2name = NULL