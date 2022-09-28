/*-- ======================================================================================================================================================================================
-- Author:          Microsoft
-- Created Date:	Feb-17-2020
-- Updated Date:	Mar-20-2020
-- <Description>:	This Procedure is used to get the Logistic Inventory details for the Excel file based on the Segment Id, Owner Id, Node Id, Start Date and End Date.  </Description> 
-- ========================================================================================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_GetLogisticInventoryDetails] 
(
		 @SegmentId			INT
		,@StartDate			DATE
		,@EndDate			DATE
		,@OwnerId			INT
		,@NodeId			INT = NULL
)
AS 
BEGIN

		
		IF OBJECT_ID('tempdb..#TempInvPartnerOwnerId')IS NOT NULL
		DROP TABLE #TempInvPartnerOwnerId		

		SELECT PartnerOwnerId
	    INTO #TempInvPartnerOwnerId
	    FROM Admin.PartnerOwnerMapping
	    WHERE GrandOwnerId = @OwnerId

		IF NOT EXISTS (SELECT 1 FROM #TempInvPartnerOwnerId WHERE PartnerOwnerId = @OwnerId)
		BEGIN
			INSERT INTO #TempInvPartnerOwnerId VALUES(@OwnerId)
		END


		SELECT   'Inventario Físico'								AS Inventory 
		--in case the node has more than one storage location, the first return store must be sent.
				,SubQ.StorageLocationName							AS StorageLocation
				,Inv.ProductId										AS ProductId
				,Inv.ProductName									AS Product
				,SUM(Own.OwnershipVolume)							AS Value
				,Inv.MeasurmentUnit									AS Uom--MeasurementUnit
				,'LOGISTIC_FILE_STATIC_MESSAGE'						AS Finding
				,'LOGISTIC_FILE_STATIC_MESSAGE'						AS Diagnostic
				,'LOGISTIC_FILE_STATIC_MESSAGE'						AS Impact
				,'LOGISTIC_FILE_STATIC_MESSAGE'						AS Solution
				,''													AS [Status]
				,''													AS [Order]
				,Inv.InventoryDate									AS DateOperation
				,Inv.NodeId											AS NodeId
				,Inv.NodeName										AS NodeName
		FROM Admin.view_InventoryInformation Inv
		LEFT JOIN (SELECT  StrgLoc.StorageLocationId,
						   StrgLoc.Name AS StorageLocationName,
						   NSL.NodeId,
						   StrgLoc.LogisticCenterId, 
						   ROW_NUMBER()OVER(PARTITION BY NSL.NodeId 
											ORDER     BY StrgLoc.[CreatedDate]) Rnum
					FROM Admin.NodeStorageLocation NSL
					LEFT JOIN Admin.StorageLocation StrgLoc
					ON StrgLoc.StorageLocationId = NSL.StorageLocationId
					WHERE NSL.NodeId = ISNULL(@NodeId,NSL.NodeId) 
				 )SubQ
		ON  SubQ.NodeId = Inv.NodeId
		AND SubQ.Rnum   = 1
		INNER JOIN Offchain.Ownership Own
		ON Own.InventoryProductId = Inv.InventoryProductId
		LEFT JOIN Admin.LogisticCenter Logic
		ON SubQ.LogisticCenterId = Logic.LogisticCenterId
		INNER JOIN #TempInvPartnerOwnerId tempowner
		ON tempowner.PartnerOwnerId = Own.OwnerId
		WHERE Inv.NodeSendToSAP = 1
		AND Inv.SegmentId  = @SegmentId
		AND Inv.NodeId     = ISNULL(@NodeId,Inv.NodeId) 
		AND Inv.InventoryDate BETWEEN @StartDate AND @EndDate
		GROUP BY  SubQ.StorageLocationName
				  ,Inv.ProductId								
				  ,Inv.ProductName								
				  ,Inv.ProductVolume							
				  ,Inv.MeasurmentUnit										
				  ,Inv.InventoryDate
				  ,Inv.NodeId									
				  ,Inv.NodeName									
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get the Logistic Inventory details for the Excel file based on the Segment Id, Owner Id, Node Id, Start Date and End Date.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetLogisticInventoryDetails',
    @level2type = NULL,
    @level2name = NULL