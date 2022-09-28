/*
 Pre-Deployment Script Template							
 --------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
 SELECT * FROM [$(TableName)]					
 --------------------------------------------------------------------------------------
 */


  BEGIN 
	 DECLARE @TicketId INT
	 SET @TicketId = 25929
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