/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Sep-17-2020
-- Updated Date:    Sep 30-2020
                    Updated LastModifiedBy and LastModifiedDate. Changed the InputDatatable to TEMP table and used it.
-- <Description>:	This Procedure is used to update movement records OfficialDeltaTicketId based on MovementTransactionId list as input. </Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_UpdateMovementWithOfficialDeltaTicket]
(
          @TicketId                             INT,
          @UserIdValue							NVARCHAR(260) NULL,
          @MovementTransactionIdList            [Admin].[KeyType] READONLY--TableType        
)
AS
BEGIN
    BEGIN TRY
         BEGIN TRANSACTION
               DECLARE @TodaysDateTime     DATETIME = ADMIN.udf_GetTrueDate()
               
               IF OBJECT_ID('tempdb..#MovIds') IS NOT NULL
               DROP TABLE #MovIds
                
			   CREATE TABLE #MovIds ([Key] INT)
			   INSERT INTO #MovIds ([Key])
			   SELECT * FROM @MovementTransactionIdList

               UPDATE Offchain.Movement
               SET OfficialDeltaTicketId = @TicketId
                   ,LastModifiedBy       = @UserIdValue
                   ,LastModifiedDate     = @TodaysDateTime
			   WHERE MovementTransactionId IN (SELECT [Key] FROM #MovIds)

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
							@value		= N'This Procedure is used to update movement records with OfficialDeltaTicketId based on MovementTransactionId list as input',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_UpdateMovementWithOfficialDeltaTicket',
							@level2type = NULL,
							@level2name = NULL