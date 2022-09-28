 /*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Aug-06-2020
-- Updated date: Sep-15-2020  Removing multiplication with "-1" as per BUG 78346
-- Updated date: May-20-2021  add NodeId Validation
-- Description:     This Procedure is to upsert the KPI With owner data into KPIDataByCategoryElementNode, 
                    KPIPreviousDateDataByCategoryElementNode tables.
   EXEC [Admin].[usp_SaveKPIDataByCategoryElementNode] -1
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveKPIDataByCategoryElementNode]
( 
 @TicketId INT,
 @NodeId INT = NULL
)
AS
BEGIN
  SET NOCOUNT ON

 -- VARIABLE DECLARATION
     DECLARE @Todaysdate	   DATETIME 

  -- SETTING VALUE TO VARIABLES
     SET @Todaysdate  =  [Admin].[udf_GetTrueDate] ()

  -- CREATEING TEMP TABLES
	 IF OBJECT_ID('tempdb..#UnBalance')IS NOT NULL
	 DROP TABLE #UnBalance

	 IF OBJECT_ID('tempdb..#SegmentUnbalance')IS NOT NULL
	 DROP TABLE #SegmentUnbalance

	 IF OBJECT_ID('tempdb..#SystemUnbalance')IS NOT NULL
	 DROP TABLE #SystemUnbalance

	 IF OBJECT_ID('tempdb..#MainTemp')IS NOT NULL
	 DROP TABLE #MainTemp

   
  SELECT * 
    INTO #UnBalance 
    FROM(
         SELECT DISTINCT  
    			 'Unbalance'                               AS FilterType
    			,Cat.[Name]                                AS Category
                ,Element.[Name]                            AS Element
                ,ND.[Name]                                 AS NodeName
                ,Ubal.NodeId                               AS NodeId
                ,Ubal.[CalculationDate]                    AS [CalculationDate]
                ,Prd.[Name]                                AS Product                           
                ,Ubal.TicketId			        		   AS OwnershipTicketId
                ,SUM(Ubal.[Interface]) * -1                AS InterfaceVolume
    			,SUM(Ubal.[IdentifiedLosses])              AS IdentifiedLossesVolume
    			,CASE WHEN SUM(Ubal.ToleranceUnbalance) > 0 
                      THEN ABS(SUM(Ubal.[Tolerance])) * -1
                      ELSE ABS(SUM(Ubal.[Tolerance]))
                 END                                       AS ToleranceVolume
    			,SUM(Ubal.[UnidentifiedLosses]) * -1        AS UnidentifiedLossesVolume
          FROM [Offchain].Unbalance Ubal
          INNER JOIN [Admin].Product Prd
          On Ubal.ProductId = Prd.ProductId
          INNER JOIN [Admin].Ticket Tick
          ON Tick.TicketId = Ubal.TicketId
		  AND (Tick.TicketId = @TicketId OR @TicketId = -1) -- KPI WITH OWNER DETAILS FOR A GIVEN TICKETID
          INNER JOIN [Admin].[Node] ND
          ON ND.NodeId = UBal.NodeId
          INNER JOIN [Admin].NodeTag NT
          ON NT.NodeId = Nd.NodeId
          INNER JOIN [Admin].CategoryElement Element
          ON NT.ElementId = Element.ElementId
          INNER JOIN [Admin].Category  Cat
          ON Element.CategoryId = Cat.CategoryId
          WHERE Tick.[Status] = 0             --> 0 = successfully processed
          AND Tick.ErrorMessage IS NULL
	      AND [Tick].[TicketTypeId] = 1 -- Cutoff 
	      AND [Cat].[Name] IN ('Segmento','Sistema')
          AND (@NodeId IS  NULL OR  (Ubal.NodeId = @NodeId))
		  GROUP BY Cat.[Name]
    	          ,Element.[Name]
    	          ,Ubal.[CalculationDate]    
    	          ,Prd.[Name]
    	          ,Ubal.TicketId
		          ,Ubal.NodeId
				  ,ND.[Name]
             ) SQ
      
   
    SELECT * 
      INTO #SegmentUnbalance
      FROM (
	        SELECT DISTINCT			 
				    'SegmentUnbalance'                        AS FilterType
				   ,Cat.[Name]                                AS Category
                   ,Element.[Name]                            AS Element
                   ,''                                        AS NodeName
                   ,''                                        AS NodeId    
                   ,Ubal.[Date]                               AS CalculationDate
                   ,Prd.[Name]                                AS Product                           
                   ,SUM(Ubal.[IdentifiedLossesVolume])        AS IdentifiedLossesVolume 
				   ,SUM(Ubal.[InterfaceVolume])               AS InterfaceVolume
				   ,SUM(Ubal.[ToleranceVolume])               AS ToleranceVolume
				   ,SUM(Ubal.[UnidentifiedLossesVolume])      AS UnidentifiedLossesVolume
                   ,Ubal.TicketId                             AS OwnershipTicketId
              FROM [Admin].SegmentUnbalance Ubal
              INNER JOIN [Admin].Product Prd
              On Ubal.ProductId = Prd.ProductId
              INNER JOIN [Admin].Ticket Tick
              ON Tick.TicketId = Ubal.TicketId
			  AND (Tick.TicketId = @TicketId OR @TicketId = -1) -- KPI WITH OWNER DETAILS FOR A GIVEN TICKETID
	          INNER JOIN [Admin].[CategoryElement] Element
	          ON Ubal.[SegmentId] = [Element].[ElementId]
	          AND [Element].[CategoryId] = 2 -- Segmento
              INNER JOIN [Admin].Category  Cat
              ON Element.CategoryId = Cat.CategoryId
              WHERE Tick.[Status] = 0             --> 0 = successfully processed
              AND Tick.ErrorMessage IS NULL
	          AND [Tick].[TicketTypeId] = 1 -- Cutoff
			   GROUP BY Cat.[Name]
    				   ,Element.[Name]
    				   ,Ubal.[Date]    
    				   ,Prd.[Name]
    				   ,Ubal.TicketId
             ) SQ
    
        SELECT * 
          INTO #SystemUnbalance
          FROM (
                SELECT DISTINCT			 
    				   'SystemUnbalance'                          AS FilterType
    				   ,Cat.[Name]                                AS Category
                       ,Element.[Name]                            AS Element
                       ,''                                        AS NodeName
                       ,''                                        AS NodeId
                       ,Ubal.[Date]                               AS [CalculationDate] 
                       ,Prd.[Name]                                AS Product 
                       ,SUM(Ubal.[IdentifiedLossesVolume])        AS IdentifiedLossesVolume 
    				   ,SUM(Ubal.[InterfaceVolume])               AS InterfaceVolume
    				   ,SUM(Ubal.[ToleranceVolume])               AS ToleranceVolume
    				   ,SUM(Ubal.[UnidentifiedLossesVolume])      AS UnidentifiedLossesVolume
                       ,Ubal.TicketId                             AS OwnershipTicketId
                  FROM [Admin].SystemUnbalance Ubal
                  INNER JOIN [Admin].Product Prd
                  On Ubal.ProductId = Prd.ProductId
                  INNER JOIN [Admin].Ticket Tick
                  ON Tick.TicketId = Ubal.TicketId
				  AND (Tick.TicketId = @TicketId OR @TicketId = -1) -- KPI WITH OWNER DETAILS FOR A GIVEN TICKETID
	              INNER JOIN [Admin].[CategoryElement] Element
	              ON Ubal.[SystemId] = [Element].[ElementId]
	              AND [Element].[CategoryId] = 8 -- Sistema
                  INNER JOIN [Admin].Category  Cat
                  ON Element.CategoryId = Cat.CategoryId
                  WHERE Tick.[Status] = 0             --> 0 = successfully processed
                  AND Tick.ErrorMessage IS NULL
	              AND [Tick].[TicketTypeId] = 1 -- Cutoff
			      GROUP BY Cat.[Name]
    		              ,Element.[Name]
    		              ,Ubal.[Date]    
    		              ,Prd.[Name]
    		              ,Ubal.TicketId
                   ) SQ
    
              -- INSERT INTO #MAINTEMP
                 SELECT  FilterType
    	                ,OrderToDisplay
    	                ,Category
    	                ,Element
    	                ,NodeName
    	                ,NodeId
    	                ,CAST(CalculationDate AS DATE) AS CalculationDate
    	                ,Product
    	                ,Indicator
    	                ,CurrentValue
    	                ,OwnershipTicketId AS TicketId 
    				INTO #MainTemp FROM (
                 -- CALCULATING IDENTIFIED LOSSES KPI
                    SELECT  FilterType
                           ,1                        AS OrderToDisplay
                    	   ,Category
                    	   ,Element
                    	   ,NodeName
                    	   ,NodeId
                    	   ,CalculationDate
                    	   ,Product 
                    	   ,'PÉRDIDAS IDENTIFICADAS' AS Indicator
                    	   ,[IdentifiedLossesVolume] AS CurrentValue
    					   ,OwnershipTicketId
                    FROM #UnBalance 
                    UNION
                    SELECT  FilterType
                           ,1                        AS OrderToDisplay
                    	   ,Category
                    	   ,Element
                    	   ,NodeName
                    	   ,NodeId
                    	   ,CalculationDate
                    	   ,Product 
                    	   ,'PÉRDIDAS IDENTIFICADAS' AS Indicator
                    	   ,[IdentifiedLossesVolume] AS CurrentValue
    					   ,OwnershipTicketId
                    FROM #SegmentUnbalance 
                    UNION
                    SELECT  FilterType
                           ,1                        AS OrderToDisplay
                    	   ,Category
                    	   ,Element
                    	   ,NodeName
                    	   ,NodeId
                    	   ,CalculationDate
                    	   ,Product 
                    	   ,'PÉRDIDAS IDENTIFICADAS' AS Indicator
                    	   ,[IdentifiedLossesVolume] AS CurrentValue
    					   ,OwnershipTicketId
                      FROM #SystemUnbalance
    				  UNION
                 -- CALCULATING INTERFACEVOLUME  KPI
                    SELECT  FilterType
                           ,2                        AS OrderToDisplay
                    	   ,Category
                    	   ,Element
                    	   ,NodeName
                    	   ,NodeId
                    	   ,CalculationDate
                    	   ,Product 
                    	   ,'INTERFASES'             AS Indicator
                    	   ,[InterfaceVolume]        AS CurrentValue
    					   ,OwnershipTicketId
                    FROM #UnBalance 
                    UNION
                    SELECT  FilterType
                           ,2                        AS OrderToDisplay
                    	   ,Category
                    	   ,Element
                    	   ,NodeName
                    	   ,NodeId
                    	   ,CalculationDate
                    	   ,Product 
                    	   ,'INTERFASES'             AS Indicator
                    	   ,[InterfaceVolume]        AS CurrentValue
    					   ,OwnershipTicketId
                    FROM #SegmentUnbalance 
                    UNION
                    SELECT  FilterType
                           ,2                        AS OrderToDisplay
                    	   ,Category
                    	   ,Element
                    	   ,NodeName
                    	   ,NodeId
                    	   ,CalculationDate
                    	   ,Product 
                    	   ,'INTERFASES'             AS Indicator
                    	   ,[InterfaceVolume]        AS CurrentValue
    					   ,OwnershipTicketId
                      FROM #SystemUnbalance
    				  UNION
                 -- CALCULATING TOLERANCIA  KPI
                    SELECT  FilterType
                           ,3                        AS OrderToDisplay
                    	   ,Category
                    	   ,Element
                    	   ,NodeName
                    	   ,NodeId
                    	   ,CalculationDate
                    	   ,Product 
                    	   ,'TOLERANCIA'             AS Indicator
                    	   ,[ToleranceVolume]        AS CurrentValue
    					   ,OwnershipTicketId
                    FROM #UnBalance 
                    UNION
                    SELECT  FilterType
                           ,3                        AS OrderToDisplay
                    	   ,Category
                    	   ,Element
                    	   ,NodeName
                    	   ,NodeId
                    	   ,CalculationDate
                    	   ,Product 
                    	   ,'TOLERANCIA'             AS Indicator
                    	   ,[ToleranceVolume]        AS CurrentValue
    					   ,OwnershipTicketId
                    FROM #SegmentUnbalance 
                    UNION
                    SELECT  FilterType
                           ,3                        AS OrderToDisplay
                    	   ,Category
                    	   ,Element
                    	   ,NodeName
                    	   ,NodeId
                    	   ,CalculationDate
                    	   ,Product 
                    	   ,'TOLERANCIA'             AS Indicator
                    	   ,[ToleranceVolume]        AS CurrentValue
    					   ,OwnershipTicketId
                      FROM #SystemUnbalance
    				  UNION
                 -- CALCULATING UNIDENTIFIED LOSSES  KPI
                    SELECT  FilterType
                           ,4                              AS OrderToDisplay
                    	   ,Category
                    	   ,Element
                    	   ,NodeName
                    	   ,NodeId
                    	   ,CalculationDate
                    	   ,Product 
                    	   ,'PÉRDIDAS NO IDENTIFICADAS'    AS Indicator
                    	   ,[UnidentifiedLossesVolume]     AS CurrentValue
    					   ,OwnershipTicketId
                    FROM #UnBalance 
                    UNION
                    SELECT  FilterType
                           ,4                              AS OrderToDisplay
                    	   ,Category
                    	   ,Element
                    	   ,NodeName
                    	   ,NodeId
                    	   ,CalculationDate
                    	   ,Product 
                    	   ,'PÉRDIDAS NO IDENTIFICADAS'    AS Indicator
                    	   ,[UnidentifiedLossesVolume]     AS CurrentValue
    					   ,OwnershipTicketId
                    FROM #SegmentUnbalance 
                    UNION
                    SELECT  FilterType
                           ,4                              AS OrderToDisplay
                    	   ,Category
                    	   ,Element
                    	   ,NodeName
                    	   ,NodeId
                    	   ,CalculationDate
                    	   ,Product 
                    	   ,'PÉRDIDAS NO IDENTIFICADAS'    AS Indicator
                    	   ,[UnidentifiedLossesVolume]     AS CurrentValue
    					   ,OwnershipTicketId
                      FROM #SystemUnbalance
					  
    				  )A

              -- UPSERT DATA INTO CURRENT KPI MAIN TABLE FROM TEMP TABLE
    		     MERGE INTO [Admin].[KPIDataByCategoryElementNode]  DEST
    			 USING #MainTemp SRC
    			    ON  ISNULL(SRC.FilterType       ,'')  = ISNULL(DEST.FilterType      ,'')
    	            AND ISNULL(SRC.OrderToDisplay   ,'')  = ISNULL(DEST.OrderToDisplay	,'')
    	            AND ISNULL(SRC.Category		    ,'')  = ISNULL(DEST.Category		,'')
    	            AND ISNULL(SRC.Element			,'')  = ISNULL(DEST.Element			,'')
    	            AND ISNULL(SRC.NodeName	  	    ,'')  = ISNULL(DEST.NodeName		,'')
    	            AND ISNULL(SRC.NodeId			,'')  = ISNULL(DEST.NodeId			,'')
    	            AND ISNULL(SRC.CalculationDate  ,'')  = ISNULL(DEST.CalculationDate	,'')
    	            AND ISNULL(SRC.Product			,'')  = ISNULL(DEST.Product			,'')
    	            AND ISNULL(SRC.Indicator		,'')  = ISNULL(DEST.Indicator		,'')
    	            AND ISNULL(SRC.TicketId		    ,'')  = ISNULL(DEST.TicketId		,'')
               -- WHEN MATCHED
    		      WHEN MATCHED 
    			   AND DEST.CurrentValue <> SRC.CurrentValue
    			  THEN
    			  UPDATE SET DEST.CurrentValue       = SRC.CurrentValue
    			            ,DEST.LastModifiedBy     = 'ReportUser'
    						,DEST.LastModifiedDate   = @Todaysdate
               -- WHEN NOT MATCHED
    		      WHEN NOT MATCHED THEN
    			  INSERT (FilterType,OrderToDisplay,Category,Element,NodeName,NodeId,CalculationDate,Product,Indicator,CurrentValue,TicketId
    			         ,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate
    					 )
    			  VALUES (SRC.FilterType,SRC.OrderToDisplay,SRC.Category,SRC.Element,SRC.NodeName,SRC.NodeId,SRC.CalculationDate,SRC.Product
    			         ,SRC.Indicator,SRC.CurrentValue,SRC.TicketId,'ReportUser',@Todaysdate,'ReportUser',@Todaysdate
    					 );
    
              -- UPSERT DATA INTO PREVIOUS KPI MAIN TABLE FROM TEMP TABLE
    		     MERGE INTO [Admin].[KPIPreviousDateDataByCategoryElementNode]  DEST
    			 USING #MainTemp SRC
    			    ON  ISNULL(SRC.FilterType       ,'')  = ISNULL(DEST.FilterType          ,'')
    	            AND ISNULL(SRC.OrderToDisplay   ,'')  = ISNULL(DEST.OrderToDisplay	    ,'')
    	            AND ISNULL(SRC.Category		    ,'')  = ISNULL(DEST.CategoryPrev	    ,'')
    	            AND ISNULL(SRC.Element			,'')  = ISNULL(DEST.ElementPrev		    ,'')
    	            AND ISNULL(SRC.NodeName	  	    ,'')  = ISNULL(DEST.NodeNamePrev	    ,'')
    	            AND ISNULL(SRC.NodeId			,'')  = ISNULL(DEST.NodeId			    ,'')
    	            AND ISNULL(SRC.CalculationDate  ,'')  = ISNULL(DEST.CalculationDatePrev	,'')
    	            AND ISNULL(SRC.Product			,'')  = ISNULL(DEST.Product			    ,'')
    	            AND ISNULL(SRC.Indicator		,'')  = ISNULL(DEST.Indicator		    ,'')
    	            AND ISNULL(SRC.TicketId		    ,'')  = ISNULL(DEST.TicketId		    ,'')	
               -- WHEN MATCHED
    		      WHEN MATCHED 
    			   AND DEST.CurrentValuePrev <> SRC.CurrentValue
    			  THEN
    			  UPDATE SET DEST.CurrentValuePrev   = SRC.CurrentValue
    			            ,DEST.LastModifiedBy     = 'ReportUser'
    						,DEST.LastModifiedDate   = @Todaysdate
               -- WHEN NOT MATCHED
    		      WHEN NOT MATCHED THEN
    			  INSERT (FilterType,OrderToDisplay,CategoryPrev,ElementPrev,NodeNamePrev,NodeId,CalculationDatePrev,Product,Indicator
    			         ,CurrentValuePrev,TicketId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate
    					 )
    			  VALUES (SRC.FilterType,SRC.OrderToDisplay,SRC.Category,SRC.Element,SRC.NodeName,SRC.NodeId,SRC.CalculationDate,SRC.Product
    			         ,SRC.Indicator,SRC.CurrentValue,SRC.TicketId,'ReportUser',@Todaysdate,'ReportUser',@Todaysdate
    					 );

END

GO

EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is to upsert the KPI With owner data into KPIDataByCategoryElementNode, KPIPreviousDateDataByCategoryElementNode tables.',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_SaveKPIDataByCategoryElementNode',
							@level2type = NULL,
							@level2name = NULL




