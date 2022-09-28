/* =================================================================================================
-- Author: Microsoft
-- Created date: Nov-28-2019
-- Updated date: Mar-20-2020
-- Updated date: Apr-06-2020  -- Added BatchID,SystemName as per PBI#28962.
-- Updated date: Apr-23-2020  -- Added EventType Column as per PBI#25056.
--               Apr-23-2020  -- Removed DISTINCT to get all the inventories
--               Apr-30-2020  -- Added Inventory Product Id column to make the records as unique while showing it in reports    
--               Jun-11-2020  -- Added GrossStandardQuantity column as per PBI#31874
--               Jun-11-2020  -- Modified OrderBy from NodeName to InventoryId
-- Updated date  Jun-25-2020  -- Updated systemname column mapping to sourcesystem
-- <Description>: This View is to Fetch Data [Admin].[InventoryDetailsWithOutOwner] For PowerBi Report From Tables(Inventory, Product, Node, NodeConnectionProduct, NodeConnection, CategoryElement,Category)</Description>
EXEC [Admin].[usp_SaveInventoryDetails] @TicketId = -1
-- =================================================================================================== */
CREATE PROCEDURE [Admin].[usp_SaveInventoryDetails] 
(
  @TicketId INT
)
AS
BEGIN

DECLARE @Todaysdate	 DATETIME =  [Admin].[udf_GetTrueDate] ()

DROP TABLE IF EXISTS #Segment;
DROP TABLE IF EXISTS #System;
DROP TABLE IF EXISTS #Source;

CREATE TABLE #Source(
	[InventoryId]              VARCHAR(50) collate SQL_Latin1_General_CP1_CS_AS NOT NULL,
	[InventoryProductId]       INT                NOT NULL,
	[CalculationDate]          DATE               NULL,
	[NodeId]                   INT                NOT NULL,
	[NodeName]                 NVARCHAR(150)      NOT NULL,
	[TankName]                 NVARCHAR(20)       NULL,
	[BatchId]                  NVARCHAR(150)      NULL,
	[ProductName]              NVARCHAR(150)      NOT NULL,
	[ProductVolume]            DECIMAL (29, 2)    NULL,
	[MeasurmentUnit]           NVARCHAR(150)      NOT NULL,
	[EventType]                NVARCHAR(10)       NOT NULL,
	[SystemName]               NVARCHAR(150)      NOT NULL,
	[UncertainityPercentage]   DECIMAL(29, 2)     NULL,
	[Incertidumbre]            DECIMAL(29, 2)     NULL,
	[TicketId]                 INT                NULL,
	[ProdutId]                 NVARCHAR(20)       NOT NULL,
	[Category]                 NVARCHAR(150)      NOT NULL,
	[Element]                  NVARCHAR(150)      NOT NULL,
	[GrossStandardQuantity]    DECIMAL (18, 2)    NULL

	--Internal Common Columns
	,[CreatedBy]			   NVARCHAR (260)	  NULL
	,[CreatedDate]			   DATETIME			  NULL
	,LastModifiedBy			   NVARCHAR (260)	  NULL
	,LastModifiedDate          DATETIME           NULL
);

SELECT  --DISTINCT
        Inv.InventoryId,
		Inv.InventoryProductId,
        Inv.InventoryDate,
        Inv.NodeId AS NodeId,        
        Inv.TankName,
		Inv.BatchId,
        Inv.ProductId ProdutId,
        Inv.[ProductName] AS ProductName,
        CAST(Inv.ProductVolume AS DECIMAL(29,2)) AS  ProductVolume,
        CEUnits.Name AS MeasurmentUnit,
		Inv.EventType,
		Inv.SourceSystem AS SystemName,
        CAST(Inv.UncertaintyPercentage AS DECIMAL(29,2)) AS UncertainityPercentage,
        CAST(ISNULL(Inv.ProductVolume,1)*ISNULL(Inv.UncertaintyPercentage,1)  AS DECIMAL(29,2)) AS Incertidumbre,
        Inv.TicketId,
        CA.Name AS Category,
        CE.Name AS Element,
        Inv.NodeName AS NodeName,	
		Inv.GrossStandardQuantity as GrossStandardQuantity,
        Inv.InventoryDate  AS CalculationDate,
		CE.ElementId AS ElementId
		INTO #Segment
  FROM [Admin].[view_InventoryInformation] Inv
  INNER JOIN Admin.Ticket Tic
  ON Inv.TicketId = Tic.TicketId 
  INNER JOIN  [Admin].[CategoryElement] CE
  ON CE.[ElementId] = Inv.SegmentId
  INNER JOIN Admin.Category CA
  ON CA.CategoryId = CE.CategoryId
  INNER JOIN [Admin].[CategoryElement] CEUnits
  ON CEUnits.ElementId= Inv.MeasurementUnit
  WHERE Tic.Status = 0            --> 0 = successfully processed
  AND Tic.ErrorMessage IS NULL
  AND  (Tic.[TicketId]= @TicketId OR @TicketId = -1 )

  SELECT --DISTINCT 
	 InventoryId
	,InventoryProductId
	,CalculationDate
	,SG.NodeId
	,NodeName
	,TankName
	,BatchId
	,ProductName
	,ProductVolume
	,MeasurmentUnit
	,EventType
	,SystemName
	,UncertainityPercentage
	,Incertidumbre
	,TicketId
	,ProdutId
	,Cat1.Name AS [Category]
    ,CatEle.Name AS Element
	,GrossStandardQuantity
	INTO #System
  FROM #Segment SG
  INNER JOIN Admin.NodeTag NT
  ON NT.ElementId = SG.ElementId
  INNER JOIN Admin.NodeTag NT1
  ON NT.NodeId = NT1.NodeId
  INNER JOIN Admin.CategoryElement CatEle
  ON CatEle.ElementId = NT1.ElementId AND CatEle.CategoryId = 8
  INNER JOIN Admin.Category  Cat1
  ON CatEle.CategoryId = Cat1.CategoryId

  INSERT INTO #Source
  (  
     InventoryId
	,InventoryProductId
	,CalculationDate
	,NodeId
	,NodeName
	,TankName
	,BatchId
	,ProductName
	,ProductVolume
	,MeasurmentUnit
	,EventType
	,SystemName
	,UncertainityPercentage
	,Incertidumbre
	,TicketId
	,ProdutId
	,Category
	,Element
	,GrossStandardQuantity
   ,[CreatedBy]
   ,[CreatedDate]
   ,[LastModifiedBy]
   ,[LastModifiedDate]
     )
  SELECT InventoryId
	,InventoryProductId
	,CalculationDate
	,NodeId
	,NodeName
	,TankName
	,BatchId
	,ProductName
	,ProductVolume
	,MeasurmentUnit
	,EventType
	,SystemName
	,UncertainityPercentage
	,Incertidumbre
	,TicketId
	,ProdutId
	,Category
	,Element
	,GrossStandardQuantity
	,'ReportUser' as [CreatedBy]
	,@TodaysDate as [CreatedDate]
	,NULL as [LastModifiedBy]
	,NULL as [LastModifiedDate]
	FROM ( 
	       SELECT 	 InventoryId
	,InventoryProductId
	,CalculationDate
	,NodeId
	,NodeName
	,TankName
	,BatchId
	,ProductName
	,ProductVolume
	,MeasurmentUnit
	,EventType
	,SystemName
	,UncertainityPercentage
	,Incertidumbre
	,TicketId
	,ProdutId
	,Category
	,Element
	,GrossStandardQuantity
	FROM #Segment
	UNION 
	SELECT 	 InventoryId
	,InventoryProductId
	,CalculationDate
	,NodeId
	,NodeName
	,TankName
	,BatchId
	,ProductName
	,ProductVolume
	,MeasurmentUnit
	,EventType
	,SystemName
	,UncertainityPercentage
	,Incertidumbre
	,TicketId
	,ProdutId
	,Category
	,Element
	,GrossStandardQuantity
	FROM #System
	)SubQ

  MERGE [Admin].[InventoryDetailsWithOutOwner] AS TARGET 
				USING #Source AS SOURCE  
				ON (
				    ISNULL(TARGET.InventoryId       ,'')  =     ISNULL(SOURCE.InventoryId		 ,'')
				AND ISNULL(TARGET.InventoryProductId,'')  =     ISNULL(SOURCE.InventoryProductId ,'')
				AND ISNULL(TARGET.CalculationDate   ,'')  =     ISNULL(SOURCE.CalculationDate	 ,'')
				AND ISNULL(TARGET.NodeId            ,'')  =     ISNULL(SOURCE.NodeId			 ,'')
				AND ISNULL(TARGET.TankName          ,'')  =     ISNULL(SOURCE.TankName			 ,'')
				AND ISNULL(TARGET.NodeName          ,'')  =     ISNULL(SOURCE.NodeName			 ,'')
				AND ISNULL(TARGET.ProductName       ,'')  =     ISNULL(SOURCE.ProductName		 ,'')
				AND ISNULL(TARGET.ProdutId          ,'')  =     ISNULL(SOURCE.ProdutId			 ,'')
				AND ISNULL(TARGET.[EventType]       ,'')  =     ISNULL(SOURCE.[EventType]		 ,'')
				AND ISNULL(TARGET.[Category]        ,'')  =     ISNULL(SOURCE.[Category]		 ,'')
				AND ISNULL(TARGET.[Element]         ,'')  =     ISNULL(SOURCE.[Element]			 ,'')
				AND ISNULL(TARGET.TicketId          ,'')  =     ISNULL(SOURCE.TicketId			 ,'')
				)

				WHEN MATCHED AND ( 
				                    TARGET.BatchId                 <> SOURCE.BatchId OR
					                TARGET.ProductVolume           <> SOURCE.ProductVolume OR
					                TARGET.[SystemName]            <> SOURCE.[SystemName] OR
					                TARGET.UncertainityPercentage  <> SOURCE.UncertainityPercentage OR
					                TARGET.Incertidumbre           <> SOURCE.Incertidumbre OR		
					                TARGET.GrossStandardQuantity   <> SOURCE.GrossStandardQuantity 
									)
                     THEN UPDATE 
					  SET TARGET.BatchId                 = SOURCE.BatchId 
					     ,TARGET.ProductVolume           = SOURCE.ProductVolume 
					     ,TARGET.[SystemName]            = SOURCE.[SystemName] 
					     ,TARGET.UncertainityPercentage  = SOURCE.UncertainityPercentage 
					     ,TARGET.Incertidumbre           = SOURCE.Incertidumbre 		
					     ,TARGET.GrossStandardQuantity   = SOURCE.GrossStandardQuantity 
					     ,TARGET.LastModifiedBy          ='ReportUser'
					     ,TARGET.[LastModifiedDate]      = @TodaysDate
    				  
          
				/* 2. Performing the INSERT operation */ 
    --            WHEN NOT MATCHED BY SOURCE  
				/* When no records are matched with TARGET table 
				   Then insert the records in the target table */ 
				WHEN NOT MATCHED BY TARGET 
				THEN INSERT ( 
				               InventoryId
	                          ,InventoryProductId
	                          ,CalculationDate
	                          ,NodeId
	                          ,NodeName
	                          ,TankName
	                          ,BatchId
	                          ,ProductName
	                          ,ProductVolume
	                          ,MeasurmentUnit
	                          ,EventType
	                          ,SystemName
	                          ,UncertainityPercentage
	                          ,Incertidumbre
	                          ,TicketId
	                          ,ProdutId
	                          ,Category
	                          ,Element
	                          ,GrossStandardQuantity
                              ,[CreatedBy]
                              ,[CreatedDate]
                              ,[LastModifiedBy]
                              ,[LastModifiedDate]
							)

			 VALUES (
			                   SOURCE.InventoryId
	                          ,SOURCE.InventoryProductId
	                          ,SOURCE.CalculationDate
	                          ,SOURCE.NodeId
	                          ,SOURCE.NodeName
	                          ,SOURCE.TankName
	                          ,SOURCE.BatchId
	                          ,SOURCE.ProductName
	                          ,SOURCE.ProductVolume
	                          ,SOURCE.MeasurmentUnit
	                          ,SOURCE.EventType
	                          ,SOURCE.SystemName
	                          ,SOURCE.UncertainityPercentage
	                          ,SOURCE.Incertidumbre
	                          ,SOURCE.TicketId
	                          ,SOURCE.ProdutId
	                          ,SOURCE.Category
	                          ,SOURCE.Element
	                          ,SOURCE.GrossStandardQuantity
                              ,SOURCE.[CreatedBy]
                              ,SOURCE.[CreatedDate]
                              ,SOURCE.[LastModifiedBy]
                              ,SOURCE.[LastModifiedDate]
					);
				
END
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PBI 32275. This procedure is generate inventory details without owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveInventoryDetails',
    @level2type = NULL,
    @level2name = NULL