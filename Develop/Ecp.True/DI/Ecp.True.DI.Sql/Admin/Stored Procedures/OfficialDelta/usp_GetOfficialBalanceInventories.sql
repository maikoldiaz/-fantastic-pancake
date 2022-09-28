/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jul-07-2020
-- Updated Date:    Jul-09-2020		Added SegmentId Filter
					Jul-14-2020     Modified Logic for column [OwnershipValue]
					Sep-21-2020     Modified code to improve the performance
					                1. Declared a variable instead using the function directly
									2. Applied NOLOCK to avoid deadlocks
					Oct-29-2020     Perf fix and Removed unnecessary casting
-- <Description>:	This Procedure is used to get the Delta Inventories Data based on the input of SegmentId, Start Date, End Date</Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetOfficialBalanceInventories]
(
	   @TicketId				INT	 	   
)
AS
BEGIN
	DECLARE  @TicketStartDate		DATE
			,@TicketEndDate			DATE
			,@TicketSegmentId		INT
			,@PrevLastModifiedDate	DATE
			,@TodaysDate			DATE = Admin.udf_GetTrueDate()
			,@GetPreviousLastModifiedDate DATETIME
     
	DROP TABLE IF EXISTS #Ticket
	DROP TABLE IF EXISTS #TempStepOneOfficialDeltaInventories
	DROP TABLE IF EXISTS #TempStepThreeOfficialDeltaInventories
	DROP TABLE IF EXISTS #TempNodeList

	SELECT CAST([StartDate]	AS DATE) AS StartDate
	      ,CAST([EndDate]   AS DATE) AS EndDate
		  ,[CategoryElementId]
		  ,[TicketId]
		  ,[Status]
	  INTO #Ticket 
	  FROM [Admin].[Ticket] WITH (NOLOCK)

	SELECT   @TicketStartDate = StartDate
			,@TicketEndDate   = EndDate
			,@TicketSegmentId = CategoryElementId
	FROM #Ticket
	WHERE TicketId = @TicketId

	--Step 1 :SELECCT * FROM inv Where ScenarioId = 2
	--Inventory Date Should be Equal to StartDate or EndDate of the TicketDates
	--Get the Latest record for Each InventoryProductUniqueID based on InventoryProductId
	--Only If Positive Into temp table
	;With CTE
	AS
	(
		SELECT  InvPrd.[InventoryProductId]
			   ,InvPrd.[InventoryProductUniqueId]
			   ,InvPrd.[InventoryDate]
			   ,InvPrd.[NodeId]
			   ,InvPrd.[ProductId]
			   ,InvPrd.[ProductVolume]
			   ,InvPrd.[OfficialDeltaTicketId]
			   ,InvPrd.[MeasurementUnit]
			   ,InvPrd.[SegmentId]
			   ,ISNULL(LastModifiedDate,CreatedDate) AS DateValue
			   ,ROW_NUMBER()OVER(PARTITION BY InvPrd.InventoryProductUniqueId
		   						 ORDER     BY InvPrd.InventoryProductId DESC)Rnum
		FROM Offchain.InventoryProduct InvPrd 
		WHERE ScenarioId = 2
		AND InvPrd.SegmentId = @TicketSegmentId
		AND (
				 InvPrd.InventoryDate = @TicketStartDate
			  OR InvPrd.InventoryDate = @TicketEndDate
			)
	)
	SELECT	 InvPrd.[InventoryProductId]
			,InvPrd.[InventoryProductUniqueId]
			,InvPrd.[InventoryDate]
			,InvPrd.[NodeId]
			,InvPrd.[ProductId]
			,InvPrd.[ProductVolume]
			,InvPrd.[DateValue]
			,InvPrd.[OfficialDeltaTicketId]
			,InvPrd.[MeasurementUnit]
			,InvPrd.[SegmentId]
	INTO #TempStepOneOfficialDeltaInventories
	FROM CTE InvPrd
	WHERE Rnum = 1
	AND ProductVolume > 0

	--Step 2:
	--Delete the records From Step 1 if DeltaTicketID is not NULL
	DELETE s1
	FROM #TempStepOneOfficialDeltaInventories s1
	LEFT JOIN [Admin].[Ticket] t
	ON s1.[OfficialDeltaTicketId] = t.[TicketId]
	WHERE s1.[OfficialDeltaTicketId] IS NOT NULL
	AND t.[Status] NOT IN (1, 2)

	--Step 3:
	--CTE WITH NodeId, ProductId,Inventorydate get the Latest based on InventoryProductID Into temp
	;WITH CTEStep3
	AS
	(
		SELECT 	 InvPrd.[InventoryProductId]
				,InvPrd.[InventoryProductUniqueId]
				,InvPrd.[InventoryDate]
				,InvPrd.[NodeId]
				,InvPrd.[ProductId]
				,InvPrd.[ProductVolume]
				,InvPrd.[DateValue]
				,InvPrd.[MeasurementUnit]
				,InvPrd.[SegmentId]
				,ROW_NUMBER()OVER(PARTITION BY   InvPrd.[NodeId]
												,InvPrd.[ProductId]
												,InvPrd.[InventoryDate]
								  ORDER BY InvPrd.[InventoryProductId] DESC)Rnum
		FROM #TempStepOneOfficialDeltaInventories InvPrd
	)
	SELECT *
	INTO #TempStepThreeOfficialDeltaInventories 
	FROM CTEStep3
	WHERE Rnum = 1

	--Step 5 : If Step 4 has Data: based on Step 4 --(SELECT [Admin].[udf_GetPreviousLastModifiedDate] (@TicketId,@TicketSegmentId,@TicketStartDate,@TicketEndDate))date 
	--remove all the Inventories 
	--from Step 3 CreationDate is Before or less than step 4 Date(LastModifiedDate) 
	--Else step 3 
    SELECT @GetPreviousLastModifiedDate = [Admin].[udf_GetPreviousLastModifiedDate] (@TicketId
                                                                                    ,@TicketSegmentId
																			        ,@TicketStartDate
																			        ,@TicketEndDate)
	
	DELETE 
	FROM #TempStepThreeOfficialDeltaInventories
	WHERE DateValue < (@GetPreviousLastModifiedDate)

	--Step 6 : DeltaNode table JOIN with ticket on ticketID 
	--WHERE Segment = InputTicketsSegmentId Start and endDate Input tickets and Status = 0 from ticket table
	--And Status from DeltaNode table should be Delta--
	--Will give us NodeList from this Step
	SELECT DNode.NodeId
	INTO #TempNodeList
	FROM Admin.DeltaNode DNode  WITH (NOLOCK)
	INNER JOIN #Ticket Tic
	ON Dnode.TicketId = Tic.TicketId
	WHERE Tic.CategoryElementId = @TicketSegmentId
	AND Tic.StartDate = @TicketStartDate
	AND Tic.EndDate   = @TicketEndDate
	AND Dnode.[Status] IN (8,9)--submitted for approval,Approved,Rejected

	--Step 7 : Remove from Inventories from Step 5 based on if NodeId belong to Step 6 NodeList
	DELETE InvPrd
	FROM #TempStepThreeOfficialDeltaInventories InvPrd
	INNER JOIN #TempNodeList NodeList
	ON InvPrd.NodeId = NodeList.NodeId


	SELECT InvPrd.[InventoryProductId]				AS [InventoryProductId]
		  ,InvPrd.[InventoryProductUniqueId]		AS [InventoryProductUniqueId]
		  ,InvPrd.[InventoryDate]					AS [InventoryDate]
		  ,InvPrd.[NodeId]							AS [NodeId]
		  ,InvPrd.[ProductId]						AS [ProductId]
		  ,InvPrd.[MeasurementUnit]					AS [MeasurementUnit]
		  ,InvPrd.[SegmentId]						AS [SegmentId]
		  ,Own.[OwnerId]							AS [OwnerId]
		   ,CASE WHEN Own.OwnershipValueUnit = '%'
				 THEN (Own.[OwnershipValue]*InvPrd.ProductVolume)/100	
				 ELSE Own.[OwnershipValue]								END AS [OwnershipValue]
	FROM #TempStepThreeOfficialDeltaInventories InvPrd
	INNER JOIN Offchain.[Owner] Own
	ON InvPrd.[InventoryProductId] = Own.[InventoryProductId]
END

GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Delta Inventories Data based on the input of SegmentId, Start Date, End Date',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetOfficialBalanceInventories',
							@level2type = NULL,
							@level2name = NULL