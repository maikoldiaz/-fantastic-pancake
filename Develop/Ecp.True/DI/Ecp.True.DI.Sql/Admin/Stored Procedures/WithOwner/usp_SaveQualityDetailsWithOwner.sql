/*-- =================================================================================================
-- Author:		Microsoft
-- Created date: Aug-05-2020
-- Updated date: May-20-2021  add NodeId Validation
-- -- <Description>:	This procedure is to Fetch InventoryDetailsWithOwner Data For PowerBi Report From
				From Tables(Inventory, InventoryProduct, Unbalance, Ownership, Ticket, Product, Node, 
--				CategoryElement,Category)</Description></Description>
   EXEC [Admin].[usp_SaveQualityDetailsWithOwner] @TicketId = -1(full load)
   EXEC [Admin].[usp_SaveInventoryDetailsWithOwner] @OwnershipTicketId = 31352(ticketid load)
   SELECT * FROM ADMIN.QualityDetailsWithOwner
-- ===================================================================================================*/

CREATE PROCEDURE [Admin].[usp_SaveQualityDetailsWithOwner]
(
	      @OwnershipTicketId INT,
		  @NodeId INT = NULL
)
AS
BEGIN
  SET NOCOUNT ON

IF OBJECT_ID('tempdb..#segment')IS NOT NULL
			DROP TABLE #segment
IF OBJECT_ID('tempdb..#Source')IS NOT NULL
			DROP TABLE #Source


DECLARE @Todaysdate	 DATETIME =  [Admin].[udf_GetTrueDate] ()

--temp table for segment data
SELECT   --DISTINCT
             Cat.[Name]                      AS Category
            ,Element.[Name]                  AS Element
            ,Inv.NodeName                       AS NodeName
            ,CAST(Inv.InventoryDate AS DATE) AS CalculationDate
            ,Inv.ProductName                      AS Product
            ,Inv.ProductId                   AS ProductId
            ,Inv.InventoryId
			,Inv.InventoryProductId
            ,Inv.InventoryDate
            ,Inv.TankName
			,Inv.BatchId
            ,Inv.NodeId AS NodeId
            ,Inv.ProductVolume  AS ProductVolume                       --ProductVolume,
            ,CEUnits.Name AS MeasurmentUnit                                                  --MeasurmentUnit,
			,Inv.EventType                                                                   --Acción
			,Inv.SourceSystem AS SystemName
            ,ATT.AttributeValue  AS AttributeValue
            ,ValAttUnit.Name AS ValueAttributeUnit
            ,ATT.AttributeDescription
            ,Inv.OwnershipTicketId
            ,Inv.GrossStandardQuantity
			,[AttributeId].[Name]                                    AS AttributeId
			,Element.ElementId
			,Element.CategoryId
			INTO #Segment
    FROM [Admin].[view_InventoryInformation] Inv
    INNER JOIN [Admin].Ticket Tick
            ON Tick.TicketId = Inv.OwnershipTicketId
    INNER JOIN Offchain.[Ownership] Own--This table is Joined to Filter data (To Fetch the data only when ownership is calculated)
            ON Own.InventoryProductId = Inv.InventoryProductId
    INNER JOIN [Admin].[CategoryElement] CEUnits
            ON CEUnits.ElementId = Inv.MeasurementUnit
    INNER JOIN [Admin].CategoryElement Element 
            ON Element.[ElementId] = Inv.SegmentId
    INNER JOIN [Admin].Category  Cat
            ON Element.CategoryId = Cat.CategoryId
    INNER JOIN [Admin].[Attribute] ATT
            ON Inv.InventoryProductId = ATT.InventoryProductId
	INNER JOIN [Admin].[CategoryElement] ValAttUnit
            ON Att.ValueAttributeUnit = ValAttUnit.ElementId
    INNER JOIN [Admin].[CategoryElement] AttributeId
			ON AttributeId.ElementId = Att.AttributeId
            AND AttributeId.CategoryId = 20
   WHERE Tick.[Status] = 0        --> 0 = successfully processed
                    AND Tick.ErrorMessage IS NULL
                    AND Tick.[TicketTypeId] = 2 --> Here ticket type id 2 is "OWNERSHIP"
                    AND Own.IsDeleted = 0
					AND  (Tick.[TicketId]= @OwnershipTicketId OR @OwnershipTicketId = -1 )
					AND (@NodeId IS NULL OR (Inv.NodeId = @NodeId))
	 

	SELECT --DISTINCT
			 Category
			,Element
			,NodeName	
			,CalculationDate
			,Product
			,ProductId			
			,InventoryId
			,InventoryProductId
			,InventoryDate
			,TankName
			,BatchId
			,NodeId
			,ProductVolume		
			,MeasurmentUnit 		
			,EventType            
			,SystemName
			,AttributeValue
			,ValueAttributeUnit
			,AttributeDescription
			,OwnershipTicketId
			,GrossStandardQuantity
			,AttributeId
			,'ReportUser'   as [CreatedBy]
			,@TodaysDate    as [CreatedDate]
			,NULL			as [LastModifiedBy]
			,NULL           as [LastModifiedDate]
					INTO #Source
			 FROM (
		     SELECT 
						 SG.Category
						,SG.Element
						,SG.NodeName	
						,SG.CalculationDate
					    ,SG.Product
						,SG.ProductId					
						,SG.InventoryId
						,SG.InventoryProductId
						,SG.InventoryDate
						,SG.TankName
						,SG.BatchId
						,SG.NodeId
						,SG.ProductVolume				
						,SG.MeasurmentUnit 				
						,SG.EventType                              
						,SG.SystemName
						,SG.AttributeValue
						,SG.ValueAttributeUnit
						,SG.AttributeDescription
						,SG.OwnershipTicketId
						,SG.GrossStandardQuantity	
						,SG.AttributeId
						FROM 
						#Segment SG

				UNION

				SELECT DISTINCT
                     Cat1.[Name]					AS Category
                    ,CatEle.[Name]					AS Element
                    ,SG.NodeName					AS NodeName
					,SG.CalculationDate
                    ,SG.Product
                    ,SG.ProductId					
					,SG.InventoryId
					,SG.InventoryProductId
					,SG.InventoryDate
					,SG.TankName
					,SG.BatchId
					,SG.NodeId
					,SG.ProductVolume				
					,SG.MeasurmentUnit 				
					,SG.EventType                                                              
					,SG.SystemName
					,SG.AttributeValue
					,SG.ValueAttributeUnit
					,SG.AttributeDescription
					,SG.OwnershipTicketId
					,SG.GrossStandardQuantity	
					,SG.AttributeId

					FROM #Segment SG
					INNER JOIN Admin.NodeTag NT
							ON NT.ElementId = SG.ElementId
					INNER JOIN Admin.Category CA
							ON CA.CategoryId = SG.CategoryId
					INNER JOIN Admin.NodeTag NT1
							ON NT.NodeId = NT1.NodeId
					INNER JOIN Admin.CategoryElement CatEle
							ON CatEle.ElementId = NT1.ElementId AND CatEle.CategoryId = 8
					INNER JOIN Admin.Category  Cat1
							ON CatEle.CategoryId = Cat1.CategoryId   
					)SubQ
				
				MERGE [Admin].[QualityDetailsWithOwner] AS TARGET 
				USING #Source AS SOURCE  
				ON  ISNULL(TARGET.[Category]			 ,'')=  ISNULL(  SOURCE.[Category]			 ,'')
				AND ISNULL(TARGET.[Element]			     ,'')=  ISNULL(  SOURCE.[Element]			 ,'')
				AND ISNULL(TARGET.[NodeName]			 ,'')=  ISNULL(  SOURCE.[NodeName]			 ,'')
				AND ISNULL(TARGET.[CalculationDate]		 ,'')=  ISNULL(  SOURCE.[CalculationDate]	 ,'')
				AND ISNULL(TARGET.[Product]				 ,'')=  ISNULL(  SOURCE.[Product]			 ,'')
				AND ISNULL(TARGET.[ProductId]			 ,'')=  ISNULL(  SOURCE.[ProductId]			 ,'')
				AND ISNULL(TARGET.[InventoryId]			 ,'')=  ISNULL(  SOURCE.[InventoryId]		 ,'')
				AND ISNULL(TARGET.[InventoryProductId]	 ,'')=  ISNULL(  SOURCE.[InventoryProductId] ,'')
				AND ISNULL(TARGET.[InventoryDate]		 ,'')=  ISNULL(  SOURCE.[InventoryDate]		 ,'')
				AND ISNULL(TARGET.[TankName]			 ,'')=  ISNULL(  SOURCE.[TankName]			 ,'')
				AND ISNULL(TARGET.[NodeId]			     ,'')=  ISNULL(  SOURCE.[NodeId]			 ,'')
				AND ISNULL(TARGET.[OwnershipTicketId]	 ,'')=  ISNULL(  SOURCE.[OwnershipTicketId]	 ,'')

				/*Update records in Target Table using source*/
				WHEN MATCHED  AND ( 
						 TARGET.BatchId                   <> SOURCE.BatchId 
					  OR TARGET.[ProductVolume]			  <> SOURCE.[ProductVolume]						
					  OR TARGET.[EventType]				  <> SOURCE.[EventType]				
					  OR TARGET.[SystemName]			  <> SOURCE.[SystemName]			
					  OR TARGET.[AttributeValue]          <> SOURCE.[AttributeValue]      
					  OR TARGET.[ValueAttributeUnit]      <> SOURCE.[ValueAttributeUnit]  
					  OR TARGET.[AttributeDescription]    <> SOURCE.[AttributeDescription]		
					  OR TARGET.[GrossStandardQuantity]   <> SOURCE.[GrossStandardQuantity])

				THEN UPDATE 
					 SET 
					  	TARGET.BatchId                   = SOURCE.BatchId 
					  , TARGET.[ProductVolume]			 = SOURCE.[ProductVolume]						
					  , TARGET.[EventType]				 = SOURCE.[EventType]				
					  , TARGET.[SystemName]			     = SOURCE.[SystemName]			
					  , TARGET.[AttributeValue]          = SOURCE.[AttributeValue]      
					  , TARGET.[ValueAttributeUnit]      = SOURCE.[ValueAttributeUnit]  
					  , TARGET.[AttributeDescription]    = SOURCE.[AttributeDescription]		
					  , TARGET.[GrossStandardQuantity]   = SOURCE.[GrossStandardQuantity]
					  ,TARGET.LastModifiedBy             = 'ReportUser'
					  ,TARGET.[LastModifiedDate]         = @TodaysDate
					  
       
				/* 2. Performing the INSERT operation */ 
  
				/* When no records are matched with TARGET table 
				   Then insert the records in the target table */ 
				WHEN NOT MATCHED BY TARGET 
				
				THEN INSERT (
				 Category
				,Element
				,NodeName	
				,CalculationDate
				,Product
				,ProductId			
				,InventoryId
				,InventoryProductId
				,InventoryDate
				,TankName
				,BatchId
				,NodeId
				,ProductVolume		
				,MeasurmentUnit 		
				,EventType            
				,SystemName
				,AttributeValue
				,ValueAttributeUnit
				,AttributeDescription
				,OwnershipTicketId
				,GrossStandardQuantity
				,AttributeId
				,[CreatedBy]
				,[CreatedDate]
				,[LastModifiedBy]
				,[LastModifiedDate]
			     )

			 VALUES 
			 (
			 SOURCE.Category
			,SOURCE.Element
			,SOURCE.NodeName	
			,SOURCE.CalculationDate
			,SOURCE.Product
			,SOURCE.ProductId			
			,SOURCE.InventoryId
			,SOURCE.InventoryProductId
			,SOURCE.InventoryDate
			,SOURCE.TankName
			,SOURCE.BatchId
			,SOURCE.NodeId
			,SOURCE.ProductVolume		
			,SOURCE.MeasurmentUnit 		
			,SOURCE.EventType            
			,SOURCE.SystemName
			,SOURCE.AttributeValue
			,SOURCE.ValueAttributeUnit
			,SOURCE.AttributeDescription
			,SOURCE.OwnershipTicketId
			,SOURCE.GrossStandardQuantity
			,SOURCE.AttributeId
			,SOURCE.[CreatedBy]
			,SOURCE.[CreatedDate]
			,SOURCE.[LastModifiedBy]
			,SOURCE.[LastModifiedDate]
			);		


END
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This procedure is to Fetch MovementDetailsWithOwner Data',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveQualityDetailsWithOwner',
    @level2type = NULL,
    @level2name = NULL