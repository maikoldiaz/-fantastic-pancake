/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jul-31-2020
--Updated date:     Aug-06-2020  Separated the logic to calculate the KPI values. Removed Common code. 
-- Updated date: May-20-2021  add NodeId Validation
-- Description:     This Procedure is to upsert the KPI With owner data into KPIDataByCategoryElementNodeWithOwnership, 
                    KPIPreviousDateDataByCategoryElementNodeWithOwner tables.
   EXEC [Admin].[usp_SaveKPIDataByCategoryElementNodeWithOwnership] -1
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveKPIDataByCategoryElementNodeWithOwnership]
( 
 @OwnershipTicketId INT,
 @NodeId INT = null
)
AS
BEGIN
  SET NOCOUNT ON

 -- VARIABLE DECLARATION
     DECLARE @Todaysdate	   DATETIME 

  -- SETTING VALUE TO VARIABLES
     SET @Todaysdate  =  [Admin].[udf_GetTrueDate] ()

  -- CREATEING TEMP TABLES
	 IF OBJECT_ID('tempdb..#Ownership')IS NOT NULL
	 DROP TABLE #Ownership

	 IF OBJECT_ID('tempdb..#SegementOwnership')IS NOT NULL
	 DROP TABLE #SegementOwnership

	 IF OBJECT_ID('tempdb..#SystemOwnership')IS NOT NULL
	 DROP TABLE #SystemOwnership

	 IF OBJECT_ID('tempdb..#MainTemp')IS NOT NULL
	 DROP TABLE #MainTemp

   
  SELECT * 
    INTO #Ownership 
    FROM(
         SELECT DISTINCT  
    			 'Ownership'                               AS FilterType
    			,Cat.[Name]                                AS Category
                ,Element.[Name]                            AS Element
                ,ND.[Name]                                 AS NodeName
                ,Ubal.NodeId                               AS NodeId
                ,Ubal.[Date]                               AS [CalculationDate]
                ,Prd.[Name]                                AS Product                           
                ,Ubal.OwnershipTicketId					   AS OwnershipTicketId
                ,SUM(Ubal.[InterfaceVolume])               AS InterfaceVolume
    			,SUM(Ubal.[IdentifiedLossesVolume])        AS IdentifiedLossesVolume
    			,SUM(Ubal.[ToleranceVolume]) * -1          AS ToleranceVolume
    			,SUM(Ubal.[UnidentifiedLossesVolume]) * -1 AS UnidentifiedLossesVolume
               FROM [Admin].OwnershipCalculation Ubal
               INNER JOIN [Admin].Product Prd
               On Ubal.ProductId = Prd.ProductId
               INNER JOIN [Admin].Ticket Tick
               ON Tick.TicketId = Ubal.OwnershipTicketId
			   AND (Tick.TicketId = @OwnershipTicketId OR @OwnershipTicketId = -1) -- KPI WITH OWNER DETAILS FOR A GIVEN TICKETID
               INNER JOIN [Admin].[Node] ND
               ON ND.NodeId = UBal.NodeId
               INNER JOIN [Admin].NodeTag NT
               ON NT.NodeId = Nd.NodeId
               INNER JOIN [Admin].[CategoryElement] ElementOwner 
               ON [Ubal].[OwnerId] = [ElementOwner].[ElementId]
               INNER JOIN [Admin].CategoryElement Element
               ON NT.ElementId = Element.ElementId
               INNER JOIN [Admin].Category  Cat
               ON Element.CategoryId = Cat.CategoryId
               WHERE Tick.[Status] = 0             --> 0 = successfully processed
               AND Tick.ErrorMessage IS NULL
               AND [Tick].[TicketTypeId] = 2 --> Here ticket type id 2 is "OWNERSHIP"
               AND [ElementOwner].[CategoryId] = 7 --> Here categoryId 7 is "Propietario"
               AND [Cat].[Name] IN ('Segmento','Sistema') 
               AND (@NodeId IS  NULL OR  (Ubal.NodeId = @NodeId))
			   GROUP BY Cat.[Name]
    				   ,Element.[Name]
    				   ,Ubal.[Date]    
    				   ,Prd.[Name]
    				   ,Ubal.OwnershipTicketId
					   ,Ubal.NodeId
					   ,ND.[Name]
             ) SQ
      
   
    SELECT * 
      INTO #SegementOwnership
      FROM (
	        SELECT DISTINCT			 
				    'SegmentOwnership'                        AS FilterType
				   ,Cat.[Name]                                AS Category
                   ,Element.[Name]                            AS Element
                   ,''                                        AS NodeName
                   ,''                                        AS NodeId    
                   ,Ubal.[Date]                               AS CalculationDate
                   ,Prd.[Name]                                AS Product                           
                   ,SUM(Ubal.[IdentifiedLossesVolume])        AS IdentifiedLossesVolume 
				   ,SUM(Ubal.[InterfaceVolume])               AS InterfaceVolume
				   ,SUM(Ubal.[ToleranceVolume]) * -1          AS ToleranceVolume
				   ,SUM(Ubal.[UnidentifiedLossesVolume]) * -1 AS UnidentifiedLossesVolume
                   ,Ubal.OwnershipTicketId                    AS OwnershipTicketId
               FROM [Admin].SegmentOwnershipCalculation Ubal
               INNER JOIN [Admin].Product Prd
               On Ubal.ProductId = Prd.ProductId
               INNER JOIN [Admin].Ticket Tick
               ON Tick.TicketId = Ubal.OwnershipTicketId
			   AND (Tick.TicketId = @OwnershipTicketId OR @OwnershipTicketId = -1) -- KPI WITH OWNER DETAILS FOR A GIVEN TICKETID
               INNER JOIN [Admin].[CategoryElement] ElementOwner 
               ON [Ubal].[OwnerId] = [ElementOwner].[ElementId]
               INNER JOIN [Admin].[CategoryElement] Element
               ON Ubal.[SegmentId] = [Element].[ElementId]
               AND [Element].[CategoryId] = 2 -- Segmento
               INNER JOIN [Admin].Category  Cat
               ON Element.CategoryId = Cat.CategoryId
               WHERE Tick.[Status] = 0             --> 0 = successfully processed
               AND Tick.ErrorMessage IS NULL
               AND [Tick].[TicketTypeId] = 2 --> Here ticket type id 2 is "OWNERSHIP"
               AND [ElementOwner].[CategoryId] = 7 --> Here categoryId 7 is "Propietario"
			   GROUP BY Cat.[Name]
    				   ,Element.[Name]
    				   ,Ubal.[Date]    
    				   ,Prd.[Name]
    				   ,Ubal.OwnershipTicketId
             ) SQ
    
        SELECT * 
          INTO #SystemOwnership
          FROM (
                SELECT DISTINCT			 
    				   'SystemOwnership'                          AS FilterType
    				   ,Cat.[Name]                                AS Category
                       ,Element.[Name]                            AS Element
                       ,''                                        AS NodeName
                       ,''                                        AS NodeId
                       ,Ubal.[Date]                               AS [CalculationDate] 
                       ,Prd.[Name]                                AS Product 
                       ,SUM(Ubal.[IdentifiedLossesVolume])        AS IdentifiedLossesVolume 
    				   ,SUM(Ubal.[InterfaceVolume])               AS InterfaceVolume
    				   ,SUM(Ubal.[ToleranceVolume]) * -1          AS ToleranceVolume
    				   ,SUM(Ubal.[UnidentifiedLossesVolume]) * -1 AS UnidentifiedLossesVolume
                       ,Ubal.OwnershipTicketId                    AS OwnershipTicketId
                  FROM [Admin].SystemOwnershipCalculation Ubal
                  INNER JOIN [Admin].Product Prd
                  On Ubal.ProductId = Prd.ProductId
                  INNER JOIN [Admin].Ticket Tick
                  ON Tick.TicketId = Ubal.OwnershipTicketId
				  AND (Tick.TicketId = @OwnershipTicketId OR @OwnershipTicketId = -1) -- KPI WITH OWNER DETAILS FOR A GIVEN TICKETID
	              INNER JOIN [Admin].[CategoryElement] ElementOwner 
                  ON [Ubal].[OwnerId] = [ElementOwner].[ElementId]
	              INNER JOIN [Admin].[CategoryElement] Element
                  ON Ubal.[SystemId] = [Element].[ElementId]
	              AND [Element].[CategoryId] = 8 -- Sistema
                  INNER JOIN [Admin].Category  Cat
                  ON Element.CategoryId = Cat.CategoryId
                  WHERE Tick.[Status] = 0             --> 0 = successfully processed
                  AND Tick.ErrorMessage IS NULL
	              AND [Tick].[TicketTypeId] = 2 --> Here ticket type id 2 is "OWNERSHIP"
	              AND [ElementOwner].[CategoryId] = 7 --> Here categoryId 7 is "Propietario"
			      GROUP BY Cat.[Name]
    		              ,Element.[Name]
    		              ,Ubal.[Date]    
    		              ,Prd.[Name]
    		              ,Ubal.OwnershipTicketId
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
    	                ,OwnershipTicketId 
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
                    FROM #Ownership 
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
                    FROM #SegementOwnership 
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
                      FROM #SystemOwnership
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
                    FROM #Ownership 
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
                    FROM #SegementOwnership 
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
                      FROM #SystemOwnership
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
                    FROM #Ownership 
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
                    FROM #SegementOwnership 
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
                      FROM #SystemOwnership
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
                    FROM #Ownership 
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
                    FROM #SegementOwnership 
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
                      FROM #SystemOwnership
					  
    				  )A
    				  
              -- UPSERT DATA INTO CURRENT KPI MAIN TABLE FROM TEMP TABLE
    		     MERGE INTO [Admin].[KPIDataByCategoryElementNodeWithOwnership]  DEST
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
    	            AND ISNULL(SRC.OwnershipTicketId ,'')  = ISNULL(DEST.OwnershipTicketId ,'')
               -- WHEN MATCHED
    		      WHEN MATCHED 
    			   AND DEST.CurrentValue <> SRC.CurrentValue
    			  THEN
    			  UPDATE SET DEST.CurrentValue       = SRC.CurrentValue
    			            ,DEST.LastModifiedBy     = 'ReportUser'
    						,DEST.LastModifiedDate   = @Todaysdate
               -- WHEN NOT MATCHED
    		      WHEN NOT MATCHED THEN
    			  INSERT (FilterType,OrderToDisplay,Category,Element,NodeName,NodeId,CalculationDate,Product,Indicator,CurrentValue,OwnershipTicketId
    			         ,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate
    					 )
    			  VALUES (SRC.FilterType,SRC.OrderToDisplay,SRC.Category,SRC.Element,SRC.NodeName,SRC.NodeId,SRC.CalculationDate,SRC.Product
    			         ,SRC.Indicator,SRC.CurrentValue,SRC.OwnershipTicketId,'ReportUser',@Todaysdate,'ReportUser',@Todaysdate
    					 );
    
              -- UPSERT DATA INTO PREVIOUS KPI MAIN TABLE FROM TEMP TABLE
    		     MERGE INTO [Admin].[KPIPreviousDateDataByCategoryElementNodeWithOwner]  DEST
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
    	            AND ISNULL(SRC.OwnershipTicketId ,'')  = ISNULL(DEST.OwnershipTicketId	,'')	
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
    			         ,CurrentValuePrev,OwnershipTicketId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate
    					 )
    			  VALUES (SRC.FilterType,SRC.OrderToDisplay,SRC.Category,SRC.Element,SRC.NodeName,SRC.NodeId,SRC.CalculationDate,SRC.Product
    			         ,SRC.Indicator,SRC.CurrentValue,SRC.OwnershipTicketId,'ReportUser',@Todaysdate,'ReportUser',@Todaysdate
    					 );
        
END

GO

EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is to upsert the KPI With owner data into KPIDataByCategoryElementNodeWithOwnership, KPIPreviousDateDataByCategoryElementNodeWithOwner tables.',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_SaveKPIDataByCategoryElementNodeWithOwnership',
							@level2type = NULL,
							@level2name = NULL