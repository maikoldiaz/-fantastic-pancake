/******************************************************************************
-- Type = Execute stored procedure by using the given paramters and load the data into [Admin].[Opertional]
-- INSERT --> Insert into [Admin].[Operational] table.
Scenario: Category  -------> Sistema
           Element  -------> SystemName
		   NodeName -------> ALL
		   StartDate-------> '2019-12-13'
		   EndDate  -------> '2020-01-27'
		   ExecutionID-----> 738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37R (Should be different for every run)

Expectation: Stored procedure has to calculate input,output,intitalinventory,finalinvenetoyand unabalance based on given parameters
             "Sistema" , all nodes (Since Node name parameter value is "ALL")  and element, and load data into operational table
********************************************************************************/

        -- EXECUTING MAIN STORED PROCEDURE, IT SHOULD INTERNALL CALL usp_SaveOperationalDataWithoutCutOffForSystem 
		EXEC [Admin].[usp_SaveOperationalDataWithoutCutOffForReport] 'Sistema','SystemName','ALL','2019-12-13','2020-01-27','738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37R'
	  
	    -- CHECK OPERATIONAL TABLE FOR DATA ENTERED BY SP 
		select * from [Admin].[Operational] where inputelementname='SystemName'  
		-- IMP OBSERVATION: This element has 2 nodes :Automation_ivhki, Automation_h3z06 ; 
		-- Only values of movements for Automation_ivhki is observed as for node Automation_h3z06 the calculation date doesn't match the movement operational date
	
		---- VALIDATING DATE WITH CORRESPONDING TABLES 
		-- check input
		-- If for one day multiple entries, summarise them
		select * from admin.view_MovementInformation where SourceSystem <>'True'and  DestinationNodeId IN (4116,4117) and SourceNodeId NOT IN (4116,4117)
		-- Check output 
		select * from admin.view_MovementInformation where SourceSystem <>'True'and SourceNodeId IN (4116,4117)and DestinationNodeId NOT IN (4116,4117)
		-- Check identified losses 
		select * from admin.view_MovementInformation where SourceSystem <>'True'and SourceNodeId IN (4116,4117) and MessageTypeId=2
		and SourceProductId in ('10000002318','10000002372')
		
		-- Inventory Data information 
		-- INITIAL INVENTORY: INVENTORY VALUE OF CURRENTDAY-1
		-- FINAL INVENTORY : INVENTORY VALUE OF CURRENTDAY
		declare @StartDate		DATE		  
				,@EndDate		DATE	
				,@SystemId int
				,@ElementName NVARCHAR(250) 
				,@NodeId Int
		
		set @StartDate='2019-12-13'
		set @EndDate='2020-01-27'
		set @SystemId=9888
		set @ElementName='SystemName'
		set @NodeId = NULL
		
		CREATE TABLE #TempGetAllNodesInASystem
					(
						SystemID		INT,
						SystemName		NVARCHAR(250) NULL,
						NodeId			INT,
						NodeName		NVARCHAR(250),
						CalculationDate DATE
					)
		
		INSERT INTO #TempGetAllNodesInASystem (NodeId,NodeName,CalculationDate )
		EXEC [Admin].[usp_GetAllNodesInASystem] @SystemId,@StartDate,@EndDate
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
		SELECT * FROM #TempInventoryDataForSystem  