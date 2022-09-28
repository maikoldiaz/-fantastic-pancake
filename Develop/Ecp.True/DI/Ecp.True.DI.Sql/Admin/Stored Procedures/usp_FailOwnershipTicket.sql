/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date: 	May-14-2020	Modified Code to Delete the Relevant records for ownership ticket
					Jun-03-2020	Added LastModifiedBy,LastModifiedDate In ticket Update Stmt
-- <Description>:	This Procedure is used to delete the relevant records for ownership ticket based on TicketId, ErrorMessage and OtherErrorMessage. </Description>
-- EXEC [Admin].[usp_FailOwnershipTicket] 6
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_FailOwnershipTicket]
(
	    @TicketId			INT,
		@ErrorMessage		NVARCHAR(MAX),
		@OtherErrorMessage	NVARCHAR(Max)		
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

				UPDATE Offchain.Movement
				SET BlockchainStatus = null
				WHERE OwnershipTicketId = @TicketId AND IsSystemGenerated = 1 AND BlockchainStatus = 1

				--Update ticket table
				UPDATE [Admin].[Ticket] 
				SET	  [Status]			= 2, 
					  ErrorMessage		= @ErrorMessage,
					  LastModifiedBy	= 'System',
					  LastModifiedDate	= Admin.udf_GetTrueDate()
				WHERE [Status] = 1 
				AND TicketId = @TicketId

				UPDATE [Admin].[Ticket] 
				SET	  [Status]			= 2, 
					  ErrorMessage		= @OtherErrorMessage,
					  LastModifiedBy	= 'System',
					  LastModifiedDate	= Admin.udf_GetTrueDate() 
				WHERE [Status] = 1 
				AND TicketGroupId = (SELECT TicketGroupId 
								     FROM [Admin].[Ticket] 
									 WHERE TicketId = @TicketId)

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
    @value = N'This Procedure is used to delete the relevant records for ownership ticket based on TicketId, ErrorMessage and OtherErrorMessage.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_FailOwnershipTicket',
    @level2type = NULL,
    @level2name = NULL