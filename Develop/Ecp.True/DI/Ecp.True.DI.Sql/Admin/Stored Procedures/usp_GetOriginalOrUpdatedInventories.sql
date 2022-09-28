/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jun-11-2020
--					Jun-17-2020	Added Extra Columns for 6381 PBI and Removed UnWanted Checks
--					Jun-18-2020	Modified delta Ticket Logic
--					Jun-19-2020	Modified Code for the ticketID Condition
--                  July-1-2020 Added UPPER function for EventType
-- <Description>:	This Procedure is used to get the Original Inventories on the input of TicketId. </Description>
					@IsOriginal -->0-->Updated--Actual--1-->Original
EXEC [Admin].[usp_GetOriginalORUpdatedInventories] 65637,'2020-04-30','2020-06-30',0
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetOriginalOrUpdatedInventories]
(
	     @SegmentId				INT
		,@StartDate				DATE
		,@EndDate				DATE
	    ,@IsOriginal			BIT-->0-->Updated--Actual--1-->Original 
)
AS
BEGIN
		IF OBJECT_ID('tempdb..#TempSegmentInventories')IS NOT NULL
		DROP TABLE #TempSegmentInventories

		IF OBJECT_ID('tempdb..#TempInventoriesDetails')IS NOT NULL
		DROP TABLE #TempInventoriesDetails

		IF OBJECT_ID('tempdb..#TempnewInventories')IS NOT NULL
		DROP TABLE #TempnewInventories

		IF OBJECT_ID('tempdb..#TempOriginalUpdatedInventoryDetails')IS NOT NULL
		DROP TABLE #TempOriginalUpdatedInventoryDetails

		CREATE TABLE #TempOriginalUpdatedInventoryDetails
		(
			 InventoryId				VARCHAR(50) collate SQL_Latin1_General_CP1_CS_AS
			,NodeName					NVARCHAR(150)	
			,ProductName				NVARCHAR(150)	
			,ProductVolume				DECIMAL(18,2)
			,InventoryProductId			INT		
			,EventType					NVARCHAR (25)
			,MeasurementUnit			NVARCHAR(150)
			,TicketId					INT
			,InventoryProductUniqueId	NVARCHAR(150)
			,InventoryDate				DATETIME
		)

		--InventoryDetails based on Ticket(Segment, StartDate & EndDate)
		SELECT   InvPrd.InventoryId
				,InvPrd.NodeName
				,InvPrd.ProductName
				,InvPrd.ProductVolume
				,InvPrd.InventoryProductId
				,InvPrd.EventType
				,InvPrd.TicketId	
				,InvPrd.InventoryProductUniqueId
				,InvPrd.InventoryDate
				,InvPrd.MeasurementUnit
				,InvPrd.DeltaTicketId
		INTO #TempSegmentInventories
		FROM Admin.view_InventoryInformation InvPrd
		WHERE InvPrd.SegmentId = @SegmentId AND InvPrd.ScenarioId = 1
		AND InvPrd.InventoryDate BETWEEN @StartDate AND @EndDate
		AND EXISTS (SELECT 1
					FROM Admin.view_InventoryInformation InvPrdinn
					WHERE InvPrdinn.InventoryProductUniqueId = InvPrd.InventoryProductUniqueId
					AND InvPrdinn.DeltaTicketId IS NULL)

		SELECT Inv1.*
		INTO #TempInventoriesDetails
		FROM #TempSegmentInventories Inv1
		WHERE EXISTS     ( SELECT 1 
						   FROM #TempSegmentInventories Inv3 
						   WHERE Inv1.InventoryProductUniqueId = Inv3.InventoryProductUniqueId 
						   AND Inv3.TicketId IS NOT NULL)
		AND   EXISTS     ( SELECT 1 
						   FROM #TempSegmentInventories Inv4 
						   WHERE Inv1.InventoryProductUniqueId = Inv4.InventoryProductUniqueId 
						   AND Inv4.TicketId IS NULL) 


		IF @IsOriginal = 1
		BEGIN
			--OriginalInventories
			;WITH CTE
			AS
			(
				SELECT   Inv1.InventoryId
						,Inv1.NodeName
						,Inv1.ProductName
						,Inv1.ProductVolume
						,Inv1.InventoryProductId
						,Inv1.EventType
						,Inv1.TicketId	
						,Inv1.InventoryProductUniqueId
						,Inv1.InventoryDate
						,Inv1.MeasurementUnit
					    ,ROW_NUMBER()OVER(PARTITION BY Inv1.InventoryProductUniqueId 
										  ORDER BY Inv1.InventoryProductId DESC) AS RNUM
				FROM #TempInventoriesDetails Inv1
				WHERE TicketId IS NOT NULL
			)
			INSERT INTO #TempOriginalUpdatedInventoryDetails
			(
				    InventoryId
				   ,NodeName
				   ,ProductName
				   ,ProductVolume
				   ,InventoryProductId
				   ,EventType
				   ,TicketId	
				   ,InventoryProductUniqueId
				   ,InventoryDate
				   ,MeasurementUnit
			)
			SELECT  InventoryId
				   ,NodeName
				   ,ProductName
				   ,ProductVolume
				   ,InventoryProductId
				   ,EventType
				   ,TicketId	
				   ,InventoryProductUniqueId
				   ,InventoryDate
				   ,MeasurementUnit
			FROM CTE
			WHERE RNUM = 1 
			AND EventType <> 'DELETE'	
		END
		ELSE
		BEGIN
			--New MovementDetails(Doubt need to Validate)
			;WITH CTE
			AS
			(
			SELECT * ,
			ROW_NUMBER()OVER( PARTITION BY Inv1.InventoryProductUniqueId 
							  ORDER BY InventoryProductID DESC) AS RNUM
			FROM #TempSegmentInventories Inv1 
			WHERE NOT EXISTS ( SELECT 1 
							 FROM #TempSegmentInventories Inv3 
							 WHERE Inv1.InventoryProductUniqueId = Inv3.InventoryProductUniqueId 
							 AND Inv3.TicketId IS NOT NULL)
			AND EXISTS ( SELECT 1 
							 FROM #TempSegmentInventories Inv4 
							 WHERE Inv1.InventoryProductUniqueId = Inv4.InventoryProductUniqueId 
							 AND Inv4.DeltaTicketId IS NULL)
			)
			SELECT  Inv.InventoryId
				   ,Inv.NodeName
				   ,Inv.ProductName
				   ,Inv.ProductVolume
				   ,Inv.InventoryProductId
				   ,Inv.EventType
				   ,Inv.TicketId	
				   ,Inv.InventoryProductUniqueId
				   ,Inv.InventoryDate
				   ,Inv.MeasurementUnit
			INTO #TempNewInventories
			FROM CTE Inv
			WHERE RNUM = 1 
			AND EventType <> 'DELETE'

			--UpdatedInventories
			;WITH CTE
			AS
			(
				SELECT   Inv1.InventoryId
						,Inv1.NodeName
						,Inv1.ProductName
						,Inv1.ProductVolume
						,Inv1.InventoryProductId
						,Inv1.EventType
						,Inv1.TicketId	
						,Inv1.InventoryProductUniqueId
						,Inv1.InventoryDate
						,Inv1.MeasurementUnit
					    ,ROW_NUMBER()OVER(PARTITION BY Inv1.InventoryProductUniqueId 
										  ORDER BY InventoryProductId DESC) AS RNUM
				FROM #TempInventoriesDetails Inv1
			)
			INSERT INTO #TempOriginalUpdatedInventoryDetails
			(
				    InventoryId
				   ,NodeName
				   ,ProductName
				   ,ProductVolume
				   ,InventoryProductId
				   ,EventType
				   ,TicketId	
				   ,InventoryProductUniqueId
				   ,InventoryDate
				   ,MeasurementUnit
			)
			SELECT  InventoryId
				   ,NodeName
				   ,ProductName
				   ,ProductVolume
				   ,InventoryProductId
				   ,EventType
				   ,TicketId	
				   ,InventoryProductUniqueId
				   ,InventoryDate
				   ,MeasurementUnit
			FROM CTE
			WHERE RNUM = 1 AND TicketId IS NULL
			UNION
			SELECT  InventoryId
				   ,NodeName
				   ,ProductName
				   ,ProductVolume
				   ,InventoryProductId
				   ,EventType
				   ,TicketId	
				   ,InventoryProductUniqueId
				   ,InventoryDate
				   ,MeasurementUnit
			FROM #TempNewInventories

		END

		SELECT	 Inv.NodeName							AS Node
				,Inv.ProductName						AS Product
				,ABS(Inv.ProductVolume)					AS Amount
				,CASE WHEN Inv.EventType = 'Insert'
					  THEN 1
					  WHEN Inv.EventType = 'Update'
					  THEN 2
					  WHEN Inv.EventType = 'Delete'
					  THEN 3
					  WHEN Inv.EventType = 'ReInject'
					  THEN 4		END					AS Action
				,'Bbl' AS Unit
				,Inv.InventoryProductId				    AS InventoryProductId
				,Inv.InventoryId						AS InventoryId
				,Inv.TicketId							AS TicketId	
				,Inv.InventoryProductUniqueId			AS InventoryProductUniqueId
				,Inv.InventoryDate						AS InventoryDate
				,Inv.EventType       				    AS EventType
				,ABS(Inv.ProductVolume)                 AS ProductVolume
		FROM #TempOriginalUpdatedInventoryDetails Inv
		INNER JOIN Admin.CategoryElement CE
		ON Inv.MeasurementUnit = CE.ElementId 

END
GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Original Inventories on the input of TicketId',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetOriginalORUpdatedInventories',
							@level2type = NULL,
							@level2name = NULL