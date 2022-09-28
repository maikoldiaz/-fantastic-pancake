/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Dec-18-2020
-- <Description>:	This Procedure is used to mark the blockchain status of the relevant records in movement and ownership in case of failure based on TicketId, and mark the ticket as failed with ErrorMessage. </Description>
-- EXEC [Admin].[usp_FailCutoffTicket] 6
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_FailCutoffTicket]
(
	    @TicketId			INT,
		@ErrorMessage		NVARCHAR(MAX)
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

				UPDATE Offchain.Movement
				SET BlockchainStatus = null
				WHERE TicketId = @TicketId AND IsSystemGenerated = 1 AND BlockchainStatus = 1

				UPDATE Offchain.Unbalance
				SET BlockchainStatus = null
				WHERE TicketId = @TicketId AND BlockchainStatus = 1

				--Update ticket table
				UPDATE [Admin].[Ticket] 
				SET	  [Status]			= 2, 
					  ErrorMessage		= @ErrorMessage,
					  LastModifiedBy	= 'System',
					  LastModifiedDate	= Admin.udf_GetTrueDate()
				WHERE [Status] = 1 
				AND TicketId = @TicketId

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
    @value = N'his Procedure is used to mark the blockchain status of the relevant records in movement and ownership in case of failure based on TicketId, and mark the ticket as failed with ErrorMessage..',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_FailCutoffTicket',
    @level2type = NULL,
    @level2name = NULL