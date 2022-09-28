/*-- =================================================================================================================================
-- Author: IG Service
-- Created Date: March-26-2021
-- Updated Date: June-22-2021 update no confirmed movement to canceled   
-- <Description>: This Procedure is used for Update Confirm Movements Send To Sap. </Description>
-- ==================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_UpdateConfirmedLogisticMovements]
(
	   @TicketId							INT,
	   @DtMovement							[Admin].[MovementListType] READONLY
)
AS
BEGIN
    BEGIN TRY
         BEGIN TRANSACTION
			--Capture Ticket Status
			DECLARE @Result int = 0
			DECLARE @TicketStatus int = (SELECT 
											TC.Status 
										 FROM admin.Ticket TC INNER JOIN admin.StatusType ST
															ON TC.status = ST.StatusTypeId
										 WHERE TC.TicketId = @TicketId)

			IF @TicketStatus = 5 -- Verify Status same Visualizacion 
				BEGIN
					-- UPDATE State of ticket to Procesando
					UPDATE admin.Ticket SET status = 1 WHERE TicketId = @TicketId

					-- UPDATE movement to used by ticket for update ischeck.
					UPDATE offchain.LogisticMovement 
					SET IsCheck = 1 
					WHERE TicketId = @TicketId 
					AND MovementTransactionId IN (SELECT MovementTransactionId FROM @DtMovement)

					-- UPDATE no confirmed movement to canceled  
					UPDATE offchain.LogisticMovement 
					SET IsCheck = 0, StatusProcessId = 8  -- Canceled
					WHERE TicketId = @TicketId 
					AND MovementTransactionId NOT IN (SELECT MovementTransactionId FROM @DtMovement)
				END
			ELSE
				BEGIN
					PRINT'BEFORE THROW';    
					THROW 50000,'Operacion Invalida, el batch se encuentra en estado confirmado.',1    
				END

        COMMIT TRANSACTION
       END TRY

       BEGIN CATCH
             IF @@TRANCOUNT > 0
                    ROLLBACK TRANSACTION;
             THROW
       END CATCH
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', 
	@value=N'This Procedure is used for Update Confirm Movements Send To Sap.' , 
	@level0type=N'SCHEMA',
	@level0name=N'Admin', 
	@level1type=N'PROCEDURE',
	@level1name=N'usp_UpdateConfirmedLogisticMovements'
GO