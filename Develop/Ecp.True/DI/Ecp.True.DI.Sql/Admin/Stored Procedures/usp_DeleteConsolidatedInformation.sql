/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:    NOV-18-2021
-- <Description>:   This Procedure is used to delete records the tables movements conciliation </Description>
-- EXEC [Admin].[usp_DeleteConsolidatedInformation]
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_DeleteConsolidatedInformation]
(
	@SegmentName NVARCHAR(50),
	@StartDate DATE,
	@EndDate DATE
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

			  DECLARE @SegmentId INT
			  DECLARE @MovementCount INT = 0
			  DECLARE @InventoryCount INT = 0
			  DECLARE @MovementOwnerCount INT = 0
			  DECLARE @InventoryOwnerCount INT = 0

				SET @SegmentId = (SELECT ElementId FROM Admin.CategoryElement WHERE Name = @SegmentName AND CategoryId = 2)

				IF OBJECT_ID('tempdb..#TempMovementConsolidation')IS NOT NULL
				DROP TABLE #TempMovementConsolidation
				IF OBJECT_ID('tempdb..#TempInventoryConsolidation')IS NOT NULL
				DROP TABLE #TempInventoryConsolidation
				IF OBJECT_ID('tempdb..#TempConsolidatedOwnerMovement')IS NOT NULL
				DROP TABLE #TempConsolidatedOwnerMovement
				IF OBJECT_ID('tempdb..#TempConsolidatedOwnerInventory')IS NOT NULL
				DROP TABLE #TempConsolidatedOwnerInventory
				 
				IF(@SegmentId IS NULL)
				BEGIN
					RAISERROR('Segment does not exist', 15, 1)
				END

				-- Get nodes in state different from deltas
				SELECT
					Node.Name                                   AS Nodo
				, OwnershipNodeStatusType.Name              AS Estado
				, CONVERT(VARCHAR, Ticket.StartDate, 106)   AS 'Fecha de incicio'
				, CONVERT(VARCHAR, Ticket.EndDate, 106)     AS 'Fecha de fin'
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
				WHERE NodeTag.ElementId = @SegmentId
					AND (
						DeltaNode.Status NOT IN (12, 3) -- Valid states: 12 Deltas, 3 Fallido
						OR 
						DeltaNode.LastApprovedDate IS NOT NULL)
					AND Ticket.StartDate >= @StartDate
					AND Ticket.EndDate <= @EndDate

				IF((SELECT COUNT(*)
				FROM #TempConsolidationNodes) > 0)
				BEGIN
					RAISERROR('Se presento un error en el segmento seleccionado porque un nodo/s esta o estuvo en un estado diferente a deltas para el periodo seleccionado', 16, 1)
				END


				 SELECT CM.ConsolidatedMovementId
				 INTO #TempMovementConsolidation
				 FROM Admin.ConsolidatedMovement CM
						WHERE 
							SegmentId = @SegmentId AND
							StartDate >= @StartDate AND 
							EndDate <= @EndDate
					
				SELECT ConsolidatedInventoryProductId
				INTO #TempInventoryConsolidation
				FROM Admin.ConsolidatedInventoryProduct
						WHERE
							SegmentId = @SegmentId AND 
							InventoryDate BETWEEN @StartDate AND @EndDate

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

				SELECT @MovementCount = COUNT(*) FROM #TempMovementConsolidation
				SELECT @InventoryCount = COUNT(*) FROM #TempInventoryConsolidation
				SELECT @MovementOwnerCount = COUNT(*) FROM #TempConsolidatedOwnerMovement
				SELECT @InventoryOwnerCount = COUNT(*) FROM #TempConsolidatedOwnerInventory

				 SELECT @MovementCount AS MovementCount
				, @InventoryCount AS InventoryCount
				, @MovementOwnerCount AS MovementOwnerCount
				, @InventoryOwnerCount AS InventoryOwnerCount

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
    @value = N'This Procedure is used to delete records from ConsolidatedMovement and ConsolidatedInventoryProduct based on SegmentID and date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_DeleteConsolidatedInformation',
    @level2type = NULL,
    @level2name = NULL