/*-- =================================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Jul-08-2020  
-- Updated Date:    Jul-30-2020		
                    Added condition to delete existing records from ConsolidatedInventoryProducts with matching NodeId, ProductId 
					and InventoryDate for NonSON segment.
-- Updated Date:    Aug-14-2020		
                    Removed condition to delete existing records from ConsolidatedInventoryProducts with matching NodeId, ProductId 
					and InventoryDate for NonSON segment.
-- <Description>:	This Procedure is used to complete the consolidation for movements and inventory products for a given Ticket Id.  </Description>
-- EXEC [Admin].[usp_CompleteConsolidation] 25285
-- ==================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_CompleteConsolidation] 
(
       @TicketId          INT
)
AS 
  BEGIN
	SET NOCOUNT ON
   			
	BEGIN TRY
		DECLARE @lnvErrorMessage NVARCHAR(MAX)
		BEGIN TRANSACTION
			--Update IsActive to true for all consolidated movements
			UPDATE [Admin].[ConsolidatedMovement]
			SET [IsActive] = 1 -- SUCCESS
			WHERE TicketId = @TicketId

			--Update IsActive to true for all consolidated inventory products except the ones which already processed as part of last period
			UPDATE C 
			SET C.[IsActive] = 1 
			FROM [Admin].[ConsolidatedInventoryProduct] C
			WHERE TicketId = @TicketId AND NOT EXISTS (SELECT 1 FROM Admin.ConsolidatedInventoryProduct T
			    WHERE C.NodeId         = T.NodeId
			    AND C.ProductId      = T.ProductId
			    AND C.InventoryDate  = T.InventoryDate
			    AND T.TicketId <> @TicketId AND T.IsActive = 1)

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		SELECT
			@lnvErrorMessage = ERROR_MESSAGE()
		ROLLBACK TRANSACTION
		
		RAISERROR (@lnvErrorMessage, 16, 1)
	END CATCH
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description'
						   ,@value = N'This Procedure is used to complete the consolidation for movements and inventory products for a given Ticket Id.'
						   ,@level0type = N'SCHEMA'
						   ,@level0name = N'Admin'
						   ,@level1type = N'PROCEDURE'
						   ,@level1name = N'usp_CompleteConsolidation'
						   ,@level2type = NULL
						   ,@level2name = NULL