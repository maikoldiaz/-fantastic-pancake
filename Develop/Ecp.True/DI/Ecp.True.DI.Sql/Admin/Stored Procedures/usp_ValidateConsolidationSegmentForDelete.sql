/*-- ========================================================================================================================================================================
-- Author:          Intergrupo
-- Created Date: 	November-16-2021
-- <Description>:   PBI 164634. This procedure is to validate ticket whose conciliation can be deleted.
--EXEC [Admin].[usp_ValidateConsolidationSegmentForDelete]
-- ========================================================================================================================================================================*/


CREATE PROCEDURE [Admin].[usp_ValidateConsolidationSegmentForDelete]
    (
    @SegmentName NVARCHAR(50),
    @StartDate DATE,
    @EndDate DATE
)
AS
BEGIN
    DECLARE @MovementCount INT = 0
    DECLARE @InventoryCount INT = 0
    DECLARE @MovementOwnerCount INT = 0
    DECLARE @InventoryOwnerCount INT = 0
    DECLARE @SegmentId INT = NULL

    IF OBJECT_ID('tempdb..#TempConsolidationNodes')IS NOT NULL
    DROP TABLE #TempConsolidationNodes
    
    IF OBJECT_ID('tempdb..#TempConsolidatedMovementIds')IS NOT NULL
    DROP TABLE #TempConsolidatedMovementIds

	IF OBJECT_ID('tempdb..#TempConsolidatedInventoryProductIds')IS NOT NULL
	DROP TABLE #TempConsolidatedInventoryProductIds

    -- Select Segment Id
    SET @SegmentId = (SELECT ElementId FROM Admin.CategoryElement WHERE Name = @SegmentName AND CategoryId = 2)

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

    SELECT ConsolidatedMovementId
    INTO #TempConsolidatedMovementIds
    FROM Admin.ConsolidatedMovement ConsolidatedMovement
    WHERE SegmentId = @SegmentId
        AND ConsolidatedMovement.StartDate >= @StartDate
        AND ConsolidatedMovement.EndDate <= @EndDate

    SELECT @MovementCount = COUNT(*) FROM #TempConsolidatedMovementIds
    
    SELECT @MovementOwnerCount = COUNT(*)
    FROM Admin.ConsolidatedOwner ConsolidatedOwner
    WHERE ConsolidatedMovementId IN (SELECT ConsolidatedMovementId FROM #TempConsolidatedMovementIds)
    
    SELECT ConsolidatedInventoryProductId
    INTO #TempConsolidatedInventoryProductIds
    FROM Admin.ConsolidatedInventoryProduct ConsolidatedInventoryProduct
    WHERE SegmentId = @SegmentId
        AND ConsolidatedinventoryProduct.InventoryDate >= @StartDate
        AND ConsolidatedinventoryProduct.InventoryDate <= @EndDate
        
    SELECT @InventoryCount = COUNT(*) FROM #TempConsolidatedInventoryProductIds

    SELECT @InventoryOwnerCount = COUNT(*)
    FROM Admin.ConsolidatedOwner ConsolidatedOwner
    WHERE ConsolidatedInventoryProductId IN(SELECT ConsolidatedInventoryProductId FROM #TempConsolidatedInventoryProductIds)

    -- Output count of movements that pass the validation
    SELECT @MovementCount AS MovementCount
        , @InventoryCount AS InventoryCount
        , @MovementOwnerCount AS MovementOwnerCount
        , @InventoryOwnerCount AS InventoryOwnerCount

END

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This procedure is to validate ticket whose conciliation can be deleted.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_ValidateConsolidationSegmentForDelete',
    @level2type = NULL,
    @level2name = NULL