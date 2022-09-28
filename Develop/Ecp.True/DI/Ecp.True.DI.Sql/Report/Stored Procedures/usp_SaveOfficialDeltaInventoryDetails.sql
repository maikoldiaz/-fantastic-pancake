/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date:	Aug-21 2020 
-- Updated date:	Aug-25 2020  Instead of considering the ticket id's to do inventory calculation, 
                                 using distint segment and nodeid from deltabalance table.
-- Update date: 	Aug-26-2020  Inserting Start and End date values into table
-- Update date: 	Sep-23-2020  Moved the conditions into right position to get all the data correctly (Unique combination in MERGE)
-- Update date: 	Sep-30-2020  Need to consider only the inventories where source system is "TRUE" 
                                 from ConsolidatedInventoryProduct table
-- Update date: 	Oct-08-2020  Always Scenario value will be "Operativo"
-- Update date: 	Oct-09-2020  Passing ElementId and NodeId as inputs to SP
-- Description:     This Procedure is to populate official consolidated delta inventory Data into [Report].[OfficialDeltaInventory].
   
   EXEC [Report].[usp_SaveOfficialDeltaInventoryDetails] 1528,'1900-12-31','1900-12-31'
   SELECT * FROM [Report].[OfficialDeltaInventory] 
-- ==============================================================================================================================*/
CREATE PROCEDURE [Report].[usp_SaveOfficialDeltaInventoryDetails]
(
 @SegmentId INT 
,@StartDate DATE
,@EndDate   DATE
)

AS 
BEGIN
  SET NOCOUNT ON
			   
  -- Variables Declaration
       DECLARE @Todaysdate   DATETIME =  [Admin].[udf_GetTrueDate] ()
	          ,@PreviousDayOfStartDate DATE     = DATEADD(DAY,-1,@StartDate)

	-- Temp Tables
       IF OBJECT_ID('tempdb..#ConsolidatedInventory')IS NOT NULL
       DROP TABLE #ConsolidatedInventory

       IF OBJECT_ID('tempdb..#DeltaBalance')IS NOT NULL
       DROP TABLE #DeltaBalance

       DROP TABLE IF EXISTS #CategoryElement

     -- Fetching DISTINCT Segment and NodeId for a given period from DeltaBalance table
        SELECT DISTINCT SegmentId,NodeId 
		  INTO #DeltaBalance
		  FROM [Report].[DeltaBalance]
		  WHERE SegmentId = @SegmentId
		    AND StartDate = @StartDate
			AND EndDate   = @EndDate   	


          SELECT * 
	        INTO #CategoryElement 
	        FROM [Admin].[CategoryElement] 

        SELECT DISTINCT	
                ND.[NodeId]                                     AS NodeId,
                Element.ElementId                               AS SegmentId,
    		    [CIP].[ConsolidatedInventoryProductId]          AS InventoryProductId,
    		    [CIP].[InventoryDate]                           AS [Date],
    			[ND].[Name]  						    		AS [NodeName],
    			[P].[Name]     					        	    AS [Product],
    			[CIP].[ProductVolume]							AS [NetQuantity], 
    			[CIP].[GrossStandardQuantity]                   AS [GrossQuantity],
    			[CEUnits].[Name]     							AS [MeasurementUnit],
    			[Own].[Name]									AS [Owner],
    			[CO].[OwnershipVolume]							AS [OwnershipVolume],			
    			[CO].[OwnershipPercentage]						AS [OwnershipPercentage],
    			'Operativo' 									AS [Scenario],
    			ISNULL(SrcType.[Name],'')						AS [Origin],	 
    			[CIP].[ExecutionDate]                           AS [ExecutionDate]
    	   INTO #ConsolidatedInventory
    	   FROM [Admin].[ConsolidatedInventoryProduct] CIP 
    	   INNER JOIN [Admin].[Ticket] Tic
		   ON Tic.[TicketId] = CIP.TicketId
		   AND CIP.SegmentId = @SegmentId
		   AND CIP.ProductId IS NOT NULL
    	   AND CIP.SegmentId IS NOT NULL--Segment Should not be NULL
		   AND CIP.[InventoryDate] BETWEEN @PreviousDayOfStartDate AND @EndDate
		   INNER JOIN #CategoryElement Element
           ON Element.[ElementId] = Tic.CategoryElementId
		   INNER JOIN [Admin].[Node] ND 
		   ON CIP.NodeId = ND.NodeId
		   INNER JOIN #DeltaBalance DB 
		   ON DB.SegmentId = Element.ElementId
		   AND DB.NodeId = ND.NodeId 
		   INNER JOIN [Admin].[Product] P 
		   ON P.ProductId = CIP.ProductId
    	   INNER JOIN #CategoryElement CEUnits
    	   ON CEUnits.ElementId = CIP.MeasurementUnit
    	   AND CEUnits.[CategoryId] = 6 --'Unidad de Medida'
    	   LEFT JOIN [Admin].[ConsolidatedOwner] CO
    	   ON CIP.ConsolidatedInventoryProductId = CO.ConsolidatedInventoryProductId
    	   LEFT JOIN #CategoryElement SrcType
    	   ON CIP.SourceSystemId = SrcType.ElementId
    	   LEFT JOIN #CategoryElement Own -- Owner
    	   ON Own.ElementId = CO.OwnerId
    	   WHERE SrcType.[Name] = 'TRUE' -- Excluding the inventories where source system is "TRUE'

		 MERGE [Report].[OfficialDeltaInventory] AS TARGET 
		 USING #ConsolidatedInventory AS SOURCE  
		 ON (
		         TARGET.[StartDate]                               =  @StartDate 
			 AND TARGET.[EndDate]								  =  @EndDate
			 AND ISNULL(TARGET.[InventoryProductId],0)            =  ISNULL(SOURCE.[InventoryProductId],0)           
    		 AND ISNULL(TARGET.[Date],'1900-12-31')	              =  ISNULL(SOURCE.[Date],'1900-12-31')	          
    		 AND ISNULL(TARGET.[NodeName],'')			          =  ISNULL(SOURCE.[NodeName],'')			          
    		 AND ISNULL(TARGET.[Product],'') 			          =  ISNULL(SOURCE.[Product],'') 			          
    		 AND ISNULL(TARGET.[MeasurementUnit],'')	          =  ISNULL(SOURCE.[MeasurementUnit],'')	          
    		 AND ISNULL(TARGET.[Owner],'')				          =  ISNULL(SOURCE.[Owner],'')				          
    		 AND ISNULL(TARGET.[Scenario],'')			          =  ISNULL(SOURCE.[Scenario],'')			          
    		 AND ISNULL(TARGET.[Origin],'') 			          =  ISNULL(SOURCE.[Origin],'') 			          
    		 AND ISNULL(TARGET.[ExecutionDate],'1900-12-31')	  =  ISNULL(SOURCE.[ExecutionDate],'1900-12-31')
             AND ISNULL(TARGET.[SegmentId],0)                     =  ISNULL(SOURCE.[SegmentId],0)
             AND ISNULL(TARGET.[NodeId],0)                        =  ISNULL(SOURCE.[NodeId],0)

		    )

         WHEN MATCHED 
		  AND   (
		             TARGET.[NetQuantity]           <> SOURCE.[NetQuantity]        
                  OR TARGET.[GrossQuantity]		    <> SOURCE.[GrossQuantity]		
                  OR TARGET.[OwnershipVolume]		<> SOURCE.[OwnershipVolume]		   
    	    	  OR TARGET.[OwnershipPercentage]	<> SOURCE.[OwnershipPercentage]
		    	 )
         THEN UPDATE 
		        SET  TARGET.[NetQuantity]          =  TARGET.[NetQuantity]        
                    ,TARGET.[GrossQuantity]		   =  TARGET.[GrossQuantity]		
                    ,TARGET.[OwnershipVolume]	   =  TARGET.[OwnershipVolume]		   
    		        ,TARGET.[OwnershipPercentage]  =  TARGET.[OwnershipPercentage] 
					,TARGET.LastModifiedBy         =  'ReportUser'
					,TARGET.[LastModifiedDate]     =  @TodaysDate
          
		 WHEN NOT MATCHED BY TARGET 
         THEN INSERT (
                       StartDate
					  ,EndDate
					  ,SegmentId
                      ,NodeId
					  ,InventoryProductId 
					  ,[Date]
					  ,[NodeName]                   
					  ,[Product]		             
					  ,[NetQuantity]
					  ,[GrossQuantity]      
					  ,[MeasurementUnit]			 
					  ,[Owner]                      
					  ,[OwnershipVolume]            
					  ,[OwnershipPercentage]     
					  ,[Scenario]
					  ,[Origin]   
					  ,[ExecutionDate]
                      ,[CreatedBy]                  
					  ,[CreatedDate] 
					  ,[LastModifiedBy]
					  ,[LastModifiedDate]
					)

            VALUES  (  
                       @StartDate
					  ,@EndDate 
					  ,SOURCE.SegmentId
                      ,SOURCE.NodeId
                      ,SOURCE.InventoryProductId 
					  ,SOURCE.[Date]
					  ,SOURCE.[NodeName]                   
					  ,SOURCE.[Product]		             
					  ,SOURCE.[NetQuantity]
					  ,SOURCE.[GrossQuantity]      
					  ,SOURCE.[MeasurementUnit]			 
					  ,SOURCE.[Owner]                      
					  ,SOURCE.[OwnershipVolume]            
					  ,SOURCE.[OwnershipPercentage]     
					  ,SOURCE.[Scenario]
					  ,SOURCE.[Origin]   
					  ,SOURCE.[ExecutionDate]
                      ,'ReportUser'
					  ,@TodaysDate
					  ,'ReportUser'
					  ,@TodaysDate
					);
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to populate official consolidated delta inventory Data into [Report].[OfficialDeltaInventory].',
	@level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveOfficialDeltaInventoryDetails',
    @level2type = NULL,
    @level2name = NULL