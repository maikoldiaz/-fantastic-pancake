/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Sep-17-2020
-- Updated Date:    Sep 30-2020
                    Updated LastModifiedBy and LastModifiedDate. Changed the InputDatatable to TEMP table and used it.
-- <Description>:	This Procedure is used to update InventoryProduct records OfficialDeltaTicketId based on InventoryProductId list as input. </Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_UpdateInventoryProductWithOfficialDeltaTicket]
(
          @TicketId                             INT,
          @UserIdValue							NVARCHAR(260) NULL,
          @InventoryProductIdList               [Admin].[KeyType] READONLY--TableType        
)
AS
BEGIN
    BEGIN TRY
         BEGIN TRANSACTION
               DECLARE @TodaysDateTime     DATETIME = ADMIN.udf_GetTrueDate()

               IF OBJECT_ID('tempdb..#InvIds') IS NOT NULL
               DROP TABLE #InvIds
                
			   CREATE TABLE #InvIds ([Key] INT)
			   INSERT INTO #InvIds ([Key])
			   SELECT * FROM @InventoryProductIdList
               
               UPDATE Offchain.InventoryProduct
               SET OfficialDeltaTicketId = @TicketId
                   ,LastModifiedBy       = @UserIdValue
                   ,LastModifiedDate     = @TodaysDateTime
			   WHERE InventoryProductId IN (SELECT [Key] FROM #InvIds)

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
							@value		= N'This Procedure is used to update InventoryProduct records OfficialDeltaTicketId based on InventoryProductId list as input',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_UpdateInventoryProductWithOfficialDeltaTicket',
							@level2type = NULL,
							@level2name = NULL