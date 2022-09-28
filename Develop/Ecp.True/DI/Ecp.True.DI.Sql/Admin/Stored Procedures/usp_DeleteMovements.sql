/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:    July-13-2020
-- Updated Date:    Sep-21-2020   Changed code to improve the performance
                                  1. Instead of "IN" operator used inner join to delete the records
-- <Description>:   This Procedure is used to delete records the tables based on MovementTransactionId list as input. </Description>
-- EXEC [Admin].[usp_DeleteMovements] 23716
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_DeleteMovements]
(
          @MovementTransactionIdList            [Admin].[KeyType] READONLY--TableType        
)
AS
BEGIN
    BEGIN TRY
         BEGIN TRANSACTION
                           
                --Based On Input List Delete
				DELETE DE
                FROM [Admin].DeltaError	DE
				JOIN @MovementTransactionIdList Temp
				  ON MovementTransactionId = [Key]

				DELETE Att
                FROM [Admin].Attribute Att
				JOIN @MovementTransactionIdList Temp
				  ON Att.MovementTransactionId = Temp.[Key]
                
				DELETE MD
                FROM [Offchain].MovementDestination	MD
				JOIN @MovementTransactionIdList Temp
                  ON MD.MovementTransactionId = Temp.[Key]

				DELETE MP
                FROM [Offchain].MovementPeriod MP
				JOIN @MovementTransactionIdList Temp
				  ON MP.MovementTransactionId = Temp.[Key]

				DELETE MS
                FROM [Offchain].MovementSource MS
				JOIN @MovementTransactionIdList Temp
				  ON MS.MovementTransactionId = Temp.[Key]

				DELETE OW
                FROM [Offchain].[Owner] OW
				JOIN @MovementTransactionIdList Temp
				  ON OW.MovementTransactionId = Temp.[Key]

				DELETE OS
                FROM [Offchain].[Ownership] OS
				JOIN @MovementTransactionIdList Temp
				  ON OS.MovementTransactionId = Temp.[Key]

				DELETE Mov
                FROM [Offchain].[Movement] Mov
				JOIN @MovementTransactionIdList Temp
				  ON Mov.MovementTransactionId = Temp.[Key]

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
    @value = N'This Procedure is used to delete records from ConsolidatedMovement and ConsolidatedInventoryProduct based on TicketId and ErrorMessage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_DeleteMovements',
    @level2type = NULL,
    @level2name = NULL
