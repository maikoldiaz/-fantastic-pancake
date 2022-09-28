   
/*-- ===============================================================================================================================
-- Author:			Microsoft
-- Created Date:	Nov-22-2019
-- Updated Date:	Mar-20-2020
				    Apr-09-2020--> Modified the Code to Update Inventory Details in InventoryProductTable and Removed( BlockchainStatus = 1)
					Jun-17-2020	   Added batchId in the Partition By Class 
					Jun-17-2020	   Modified Order By Class
--					Jun-24-2020	   Modified Code Of adding(AND Inv.TicketId  IS NOT NULL) for the bug 57068
--					Jul-16-2020	   Made Changes to Improve Performance
                    July-29-2020   Removed JOIN [Offchain].[Owner] JOIN from CTE and added it to the next SELECT.
					Aug-06-2020	   Removed CAST on Owner.OwnerId
-- <Description>:	This Procedure is used to get the Inventory details for the Excel file based on the Ticket Id. </Description>
-- ===============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetInventoryDetails] 
(
	@TicketId INT
)
AS 
  BEGIN 
	 
	 IF OBJECT_ID('tempdb..#TempGetInventoryDetails')IS NOT NULL
	 DROP TABLE #TempGetInventoryDetails

     ;WITH CTE
	 AS
	 (
		SELECT 
		       [Tic].[TicketId]							AS Ticket,
		       Inv.[InventoryProductId]					AS InventoryProductId,
		       Inv.[InventoryDate]					    AS OperationalDate,
		       Inv.[NodeId]								AS NodeId,
		       Inv.[ProductId]							AS ProductId,
		       Inv.[TankName]							AS Tank,
		       Inv.[ProductVolume]						AS NetVolume,
			   Inv.[EventType],
		       ROW_NUMBER()OVER(PARTITION BY Inv.[ProductId]
											,Inv.InventoryID
											,Inv.NodeId
											,Inv.[InventoryDate]
											,ISNULL(Inv.[TankName],'')
											,ISNULL(Inv.BatchId,'')
								  ORDER BY Inv.[InventoryProductId] DESC  
								  )AS Rnum
		FROM   [Admin].[Ticket] Tic
		INNER JOIN [Admin].[NodeTag] NT
		ON [Tic].[CategoryElementId] = [NT].[ElementId]		
		INNER JOIN Offchain.InventoryProduct Inv
        ON Inv.NodeId = NT.NodeId
		WHERE [Tic].[TicketId]=@TicketId 
		AND Inv.[InventoryDate]  Between CAST([Tic].[StartDate] AS DATE) AND CAST([Tic].[EndDate] AS DATE)
		AND [Tic].[TicketTypeId]=2			 -- TicketTypeID 2 represents Ownership
		AND Inv.TicketId  IS NOT NULL
	  )
	  SELECT Ticket,
			 CTE.InventoryProductId		As InventoryId,
			 OperationalDate,
			 NodeId,
			 ProductId,
			 Tank,
			 NetVolume,
			 [o].[OwnerId]				AS OwnerId,
			 CatEle.[Name]              AS [Owner],
			 [o].[OwnershipValue]		As OwnershipValue,
			 [o].[OwnershipValueUnit]	AS OwnershipUnit
	  INTO #TempGetInventoryDetails
	  FROM CTE
		LEFT JOIN [Offchain].[Owner] o
		ON CTE.InventoryProductId = [o].[InventoryProductId]
		LEFT JOIN Admin.CategoryElement CatEle
		ON [o].[OwnerId] = CatEle.ElementId	
	  WHERE Rnum = 1
      AND NetVolume > 0

	  UPDATE InvPrd
	  SET OwnershipTicketId = @TicketId
	  FROM OffChain.InventoryProduct InvPrd  
	  INNER JOIN #TempGetInventoryDetails InvDtls
	  ON InvDtls.InventoryId = InvPrd.InventoryProductId

	  SELECT Ticket,
			 InventoryId,
			 OwnerId,
			 NodeId,
			 ProductId,
			 NetVolume,
			 OwnershipValue,
			 OwnershipUnit,
			 OperationalDate,
			 Tank
	  FROM #TempGetInventoryDetails
  END 
  GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
							@value = N'This Procedure is used to get the Inventory details for the Excel file based on the Ticket Id.',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetInventoryDetails',
							@level2type = NULL,
							@level2name = NULL