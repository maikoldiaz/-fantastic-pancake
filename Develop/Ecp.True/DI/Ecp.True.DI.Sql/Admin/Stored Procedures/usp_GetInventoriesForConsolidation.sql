/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jul-06-2020
-- <Description>:	This Procedure is used to get the Consolidated Movements on SegmentId , StartDate, EndDate. </Description>
EXEC [Admin].[usp_GetInventoriesForConsolidation] 121054,'2020-06-29','2020-07-06'
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetInventoriesForConsolidation]
(
	     @SegmentId				INT
		,@InventoryDate			DATE
)
AS
BEGIN
		IF OBJECT_ID('tempdb..#TempSegmentInventory')IS NOT NULL
		DROP TABLE #TempSegmentInventory

		SELECT  Inv.InventoryProductId
			   ,Inv.NodeId
			   ,Inv.ProductId
			   ,Inv.MeasurementUnit
			   ,Inv.ProductVolume
			   ,Inv.GrossStandardQuantity
			   ,Inv.OwnershipTicketId
			   ,Inv.InventoryDate
		INTO #TempSegmentInventory
		FROM Admin.view_InventoryInformation Inv
		WHERE Inv.SegmentId = @SegmentId
		AND Inv.InventoryDate = @InventoryDate
		AND Inv.ScenarioId = 1

		DECLARE @IsSegmentSON BIT

		SELECT @IsSegmentSON = IsOperationalSegment 
		FROM Admin.CategoryElement 
		WHERE CategoryID = 2
		AND ElementId = @SegmentId

		IF @IsSegmentSON = 1
		BEGIN
			SELECT   Inv.InventoryProductId
					,Inv.NodeId
					,Inv.ProductId
					,Inv.MeasurementUnit
					,Inv.ProductVolume
					,Inv.GrossStandardQuantity
					,Own.OwnerId
					,Own.OwnershipPercentage
					,Own.OwnershipVolume
					,NULL AS OwnershipValueUnit
					,Inv.InventoryDate
			FROM #TempSegmentInventory Inv 
			INNER JOIN Offchain.[Ownership] OWN 
			ON Inv.InventoryProductId = OWN.InventoryProductId
			WHERE Inv.OwnershipTicketId IS NOT NULL
		END
		ELSE
		BEGIN
			SELECT   Inv.InventoryProductId
					,Inv.NodeId
					,Inv.ProductId
					,Inv.MeasurementUnit
					,Inv.ProductVolume
					,Inv.GrossStandardQuantity
					,Own.OwnerId  AS OwnerId
					,NULL AS OwnershipPercentage
					,Own.OwnershipValue AS OwnershipVolume
					,Own.OwnershipValueUnit
					,Inv.InventoryDate
			FROM #TempSegmentInventory Inv 
			INNER JOIN Offchain.[Owner] OWN 
			ON Inv.InventoryProductId = OWN.InventoryProductId
		END
END
GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Consolidated Movements on SegmentId , StartDate, EndDate.',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetInventoriesForConsolidation',
							@level2type = NULL,
							@level2name = NULL