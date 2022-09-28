
/*-- ==================================================================================================================================================== 
-- Author:          Microsoft   
-- Created Date:    Nov-22-2019  
-- Updated Date:    Mar-20-2020
--				    Apr-09-2020  -- Removed(BlockchainStatus = 1)   
                    Apr-22-2020 If there any tickets with type 2 and status 0 then get the data from Ownership table else from owner table
					May-22-2020 update ownership ticketid all records that are Sent from this SP with the ticket id you get as an input,
								if the record doesn't already have one
					Jun-18-2020 Added  BatchId and Inventory Date in Partition by  as per OnSite team Suggestion
--					Jun-24-2020	   Modified Code Of adding(AND Inv.TicketId  IS NOT NULL) for the bug 57068
--					Jul-16-2020	   Made Changes to Improve Performance
--					Sep-07-2020 Modified Partition By Logic
--					Sep-10-2020 Added DISTINCT As Part of Bug Fix 78166
-- <Description>:   This Procedure is used to get the Initial Inventory Property details for the Excel file based on the Ticket Id.   </Description>
-- ====================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetInitialInventoryPropertyDetails] 
(
	@TicketId INT
) 
AS 
  BEGIN 
 
		   IF OBJECT_ID('tempdb..#TempResultOfInventoryPropertyDetails')IS NOT NULL
		   DROP TABLE #TempResultOfInventoryPropertyDetails 

          ;WITH CTE
           AS
           (
				 SELECT			 Inv.InventoryProductId											AS InventoryId, 
				                 Inv.[NodeId]													AS NodeId, 
				                 Inv.[ProductId]												AS ProductId,  
				                 Inv.[ProductVolume]											AS NetVolume,
				                  ROW_NUMBER()OVER(PARTITION BY  Inv.InventoryProductUniqueId
													ORDER BY Inv.[InventoryProductId] DESC
													)AS Rnum
				FROM   [Admin].[Ticket] Tic 
				INNER JOIN [Admin].[NodeTag] NT 
				ON [Tic].[CategoryElementId] = [NT].[ElementId]
				INNER JOIN Offchain.InventoryProduct Inv
				ON Inv.NodeId = NT.NodeId				
				WHERE  Inv.[InventoryDate] = DATEADD(DAY,-1,CAST([Tic].[StartDate] AS DATE))
				AND [Tic].[TicketId] = @TicketId 
				AND [Tic].[TicketTypeId]=2       -- TicketTypeID 2 represents Ownership
				AND Inv.TicketId IS NOT NULL             
             )
             SELECT InventoryId, 
                    NodeId, 
                    ProductId, 
				                 ISNULL([Own].[OwnerId],[Owner].OwnerId)						AS OwnerId,
				                 CASE   WHEN [Own].[OwnershipVolume] IS NOT NULL
										THEN [Own].[OwnershipVolume] 
										WHEN CHARINDEX('%',[Owner].OwnershipValueUnit)> 0 OR [Owner].OwnershipValueUnit = '159'
				                        THEN ([Owner].OwnershipValue*C.NetVolume)/100 --When %
				                        ELSE [Owner].OwnershipValue --When No %
				                        END														AS OwnershipVolume,
				                 CASE   WHEN [Own].[OwnershipPercentage] IS NOT NULL
									    THEN [Own].[OwnershipPercentage] 
									    WHEN CHARINDEX('%',[Owner].OwnershipValueUnit)> 0 OR [Owner].OwnershipValueUnit = '159'
				                        THEN  [Owner].OwnershipValue
				                        ELSE ([Owner].OwnershipValue/C.NetVolume)*100 
				                        END														AS OwnershipPercentage,
								  CASE WHEN [Own].[OwnerId] IS NOT NULL 
										THEN 1 
										ELSE 0 END AS IsOwnershipCalculated,
								 C.NetVolume													AS NetStandardVolume
			 INTO #TempResultOfInventoryPropertyDetails
             FROM CTE C
			 LEFT JOIN [Offchain].[Ownership] Own 
			 ON [Own].[InventoryProductId] = C.InventoryId  
			 AND Own.IsDeleted = 0
			 LEFT JOIN [Offchain].[Owner] [Owner] 
			 ON [Owner].[InventoryProductId] = C.InventoryId	
             WHERE Rnum = 1
             AND NetVolume > 0

			 UPDATE InvPrd   
             SET OwnershipTicketId = @TicketId   
             FROM Offchain.InventoryProduct InvPrd   
             INNER JOIN #TempResultOfInventoryPropertyDetails Temp    
             ON InvPrd.InventoryProductId = Temp.InventoryId       
             AND InvPrd.OwnershipTicketId IS NULL   
   	         
              SELECT DISTINCT
					 InventoryId							    AS InventoryId,    
                     NodeId									    AS NodeId,    
                     ProductId								    AS ProductId,    
                     OwnerId								    AS OwnerId, 
                     CAST(OwnershipVolume     AS DECIMAL(18,2)) AS OwnershipVolume, 
                     CAST(OwnershipPercentage AS DECIMAL(18,2)) AS OwnershipPercentage,
					 CAST(IsOwnershipCalculated AS BIT)		    AS IsOwnershipCalculated,
					 CAST(NetStandardVolume   AS DECIMAL(18,2)) AS NetStandardVolume
             FROM #TempResultOfInventoryPropertyDetails

  END
  GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
							@value = N'This Procedure is used to get the Initial Inventory Property details for the Excel file based on the Ticket Id.',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetInitialInventoryPropertyDetails',
							@level2type = NULL,
							@level2name = NULL