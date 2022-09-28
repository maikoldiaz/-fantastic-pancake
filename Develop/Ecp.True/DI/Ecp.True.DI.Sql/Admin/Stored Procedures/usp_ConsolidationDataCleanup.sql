/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:    July-08-2020
-- Updated Date:    July-13-2020 Added list of tables to delete from based on movementtransactionid
--                  
-- <Description>:   This Procedure is used to delete records from ConsolidatedMovement, ConsolidatedInventoryProduct based on TicketId and ErrorMessage. </Description>
-- EXEC [Admin].[usp_ConsolidationDataCleanup] 6 , ''
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[Usp_consolidationdatacleanup] 
(
    @TicketId                        INT, 
    @ErrorMessage                    NVARCHAR(MAX),
	@MovementTransactionIdList       [Admin].[KeyType] READONLY--TableType
) 
AS 
  BEGIN 
      BEGIN TRY 
          BEGIN TRANSACTION 

          IF NOT EXISTS(SELECT 1 
                        FROM   [Admin].[ticket] 
                        WHERE  ticketid = @TicketId) 
            BEGIN 
                RAISERROR ('No such ticket exists',16,1) 

                RETURN 
            END 

			IF OBJECT_ID('tempdb..#TempMovementConsolidation')IS NOT NULL
			DROP TABLE #TempMovementConsolidation
			IF OBJECT_ID('tempdb..#TempInventoryConsolidation')IS NOT NULL
			DROP TABLE #TempInventoryConsolidation
			IF OBJECT_ID('tempdb..#TempConsolidatedOwnerMovement')IS NOT NULL
			DROP TABLE #TempConsolidatedOwnerMovement
			IF OBJECT_ID('tempdb..#TempConsolidatedOwnerInventory')IS NOT NULL
			DROP TABLE #TempConsolidatedOwnerInventory

			SELECT CM.ConsolidatedMovementId
			INTO #TempMovementConsolidation
			FROM Admin.ConsolidatedMovement CM
			WHERE CM.Ticketid = @TicketId AND IsActive = 0; 
					
			SELECT ConsolidatedInventoryProductId
			INTO #TempInventoryConsolidation
			FROM Admin.ConsolidatedInventoryProduct
			WHERE ticketid = @TicketId AND IsActive = 0; 

			SELECT COM.ConsolidatedOwnerId
			INTO #TempConsolidatedOwnerMovement
			FROM admin.ConsolidatedOwner COM
			WHERE ConsolidatedMovementId IN(SELECT ConsolidatedMovementId FROM #TempMovementConsolidation)

			SELECT COI.ConsolidatedOwnerId
			INTO #TempConsolidatedOwnerInventory
			FROM admin.ConsolidatedOwner COI
			WHERE ConsolidatedInventoryProductId IN(SELECT ConsolidatedInventoryProductId FROM #TempInventoryConsolidation)

			DELETE admin.ConsolidatedOwner
			WHERE ConsolidatedOwnerId IN(SELECT ConsolidatedOwnerId FROM #TempConsolidatedOwnerMovement)

			DELETE admin.ConsolidatedOwner
			WHERE ConsolidatedOwnerId IN(SELECT ConsolidatedOwnerId FROM  #TempConsolidatedOwnerInventory)

			UPDATE Offchain.Movement 
				SET ConsolidatedInventoryProductId = NULL
				WHERE ConsolidatedInventoryProductId IN (SELECT ConsolidatedInventoryProductId FROM  #TempInventoryConsolidation)

			UPDATE Offchain.Movement 
				SET ConsolidatedMovementTransactionId = NULL
				WHERE ConsolidatedMovementTransactionId IN (SELECT ConsolidatedMovementId FROM  #TempMovementConsolidation)

			DELETE Admin.ConsolidatedMovement 
			WHERE ConsolidatedMovementId IN (SELECT ConsolidatedMovementId FROM #TempMovementConsolidation)

			DELETE Admin.ConsolidatedInventoryProduct
			WHERE ConsolidatedInventoryProductId IN (SELECT ConsolidatedInventoryProductId FROM  #TempInventoryConsolidation)

          --Update ticket table  
          UPDATE [Admin].[ticket] 
          SET    [status] = 2, 
                 errormessage = @ErrorMessage, 
                 lastmodifiedby = 'System', 
                 lastmodifieddate = admin.Udf_gettruedate() 
          WHERE  [status] = 1 
                 AND ticketid = @TicketId 

	      --Update delta node table
          UPDATE [Admin].[deltanode]
          SET    [status] = 3
          WHERE  [status] = 1
                 AND ticketid = @TicketId
		
          --Delete Movements generated as part of current cycle  
          EXEC [Admin].[Usp_deletemovements] @MovementTransactionIdList 

          COMMIT TRANSACTION 
      END TRY 

      BEGIN catch 
          IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION; 

          THROW 
      END catch 
  END 

GO 
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to delete records from ConsolidatedMovement and ConsolidatedInventoryProduct based on TicketId and ErrorMessage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_ConsolidationDataCleanup',
    @level2type = NULL,
    @level2name = NULL



