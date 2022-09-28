/******************************************************************************
-- Type = Execute stored procedure by using the given paramters and load the data into [Admin].[Opertional]
-- INSERT --> Insert into [Admin].[Operational] table.
Scenario: Category  -------> Sistema
           Element  -------> SystemForData
		   NodeName -------> Automation_ct8ze
		   StartDate-------> '2019-12-13'
		   EndDate  -------> '2020-01-27'
		   ExecutionID-----> 738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37R (Should be different for every run)

Expectation: Stored procedure has to calculate input,output,intitalinventory,finalinvenetoyand unabalance based on given parameters
             "Sistema" , given node and element and load into Operational table
********************************************************************************/
 
	EXEC [Admin].[usp_SaveOperationalDataWithoutCutOffForReport] 'Sistema','SystemForData','Automation_ct8ze','2019-12-13','2020-01-27','738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37R'

	-- CHECK OPERATIONAL TABLE FOR VALUES RETURNED BY ABOVE EXECUTED SP
	select * from [Admin].[Operational] where inputelementname='SystemForData' and inputnodename <>'ALL'
----------------------------------------------------------------
-- VALIDATE DATA FROM CORRESPONDING TABLES 
	-- Movement data for all System =SystemForData , Nodes= Automation_ct8ze
	-- Cehck inputs 
	-- If for one day multiple entries, summarise the netstandardvolume field
	select * from admin.view_MovementInformation where SourceSystem <>'True'and  DestinationNodeId=4346 and SourceNodeId<>4346
	-- Check output 
	select * from admin.view_MovementInformation where SourceSystem <>'True'and SourceNodeId=4346 and DestinationNodeId<>4346
	-- Check identified losses 
	select * from admin.view_MovementInformation where SourceSystem <>'True'and SourceNodeId=4346 and MessageTypeId=2
	and SourceProductId in ('10000002318','10000002372')
	-- check for inventory information 
	-- INITIAL INVENTORY: INVENTORY VALUE OF CURRENTDAY-1
	-- FINAL INVENTORY : INVENTORY VALUE OF CURRENTDAY
	declare @StartDate		DATE		  
		,@EndDate		DATE	
		,@SystemId int
		,@ElementName NVARCHAR(250) 
		,@NodeId Int

	set @StartDate='2019-12-13'
	set @EndDate='2020-01-27'
	set @SystemId=10853
	set @ElementName='SystemForData'
	set @NodeId = 4346

	CREATE TABLE #TempGetAllNodesInASystem
			(
				SystemID		INT,
				SystemName		NVARCHAR(250) NULL,
				NodeId			INT,
				NodeName		NVARCHAR(250),
				CalculationDate DATE
			)
	INSERT INTO #TempGetAllNodesInASystem (NodeId,NodeName,CalculationDate )
	EXEC [Admin].[usp_GetAllNodesInASystem] 10853,@StartDate,@EndDate
	UPDATE #TempGetAllNodesInASystem SET  SystemID   =@SystemId ,SystemName = @ElementName
	--select * from #TempGetAllNodesInASystem
	SELECT				 CAST(Inv.InventoryDate AS DATE) AS CalculationDate,
								 InvPrd.ProductId				 AS ProductID,
								 Prd.Name						 AS ProductName,
								 Inv.SegmentId					 AS SegmentID,						
								 Inv.NodeId						 AS NodeId,
								 Nd.NodeName					 AS NodeName,
								 InvPrd.ProductVolume			 AS ProductVolume
			INTO #TempInventoryDataForSystem
			FROM [Offchain].[Inventory] Inv
			INNER JOIN [Offchain].InventoryProduct InvPrd
			ON Inv.[InventoryTransactionId] = InvPrd.InventoryProductId
			INNER JOIN Admin.Product Prd
			ON Prd.ProductId = InvPrd.ProductId
			INNER JOIN #TempGetAllNodesInASystem ND
			ON ND.NodeId = Inv.NodeId
			AND CAST(Inv.InventoryDate AS DATE) = ND.CalculationDate
			WHERE Inv.InventoryDate  BETWEEN @StartDate AND @EndDate
			AND (Inv.NodeId = @NodeId OR @NodeId IS NULL)
			AND InvPrd.ProductId IS NOT NULL
			AND Inv.SegmentId IS NOT NULL--Segement Should not be NULL Even for System as System is part of Segment
			AND Inv.SourceSystem != 'TRUE' -- Exclusing the inventories where source syste is "TRUE'
	SELECT * FROM #TempInventoryDataForSystem   -- NO INVENTORY DATA


