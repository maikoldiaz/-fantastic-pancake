/* =================================================================================================
-- Author: Microsoft
-- Created date: Nov-28-2019
-- Updated date: Mar-20-2020
-- Updated date: Apr-06-2020  -- Added BatchID,SystemName as per PBI#28962.
--				 Apr-09-2020  -- Removed(BlockchainStatus = 1)
-- Updated date: Apr-23-2020  -- Added EventType Column as per PBI#25056.
--               Apr-23-2020  -- Removed DISTINCT to get all the inventories quality details
--               Apr-30-2020  -- Added Inventory Product Id column to make the records as unique while showing it in reports
--               May-06-2020  -- Removed Casting for AttributeValue Column as it is in all other places
--               Jun-11-2020  -- Added GrossStandardQuantity as per PBI#31874
--               Jun-11-2020  -- Added AttributeId as per PBI#31874
--               Jun-11-2020  -- Modified OrderBy from NodeName to InventoryId
--               Jun-15-2020  -- Changed the logic of ValueAttributeUnit as per PBI 31874
-- Updated date: Jun-25-2020  -- AttributeId homologated to categoryElement table. (AtributeId = ElementName)
--                            -- Updated systemname column mapping to sourcesytem
-- Updated date: Jun-26-2020  -- Added condition categoryid = 20 for generating attribute name 
				 Aug-06-2020  -- Removed Casting on CEUnits.ElementId= Inv.MeasurementUnit
-- Updated date: May-20-2021  add NodeId Validation
-- <Description>: This procedure is to Fetch Data [Admin].[QualityDetailsWithoutOwner] For PowerBi Report From Tables</Description>
    EXEC [Admin].[usp_SaveQualityDetails] @TicketId = -1
-- =================================================================================================== */
CREATE PROCEDURE [Admin].[usp_SaveQualityDetails]
(
  @TicketId INT,
  @NodeId INT = NULL
)
AS
BEGIN

DROP TABLE IF EXISTS #Segement;
DROP TABLE IF EXISTS #System;
DROP TABLE IF EXISTS #Source;

DECLARE @Todaysdate	 DATETIME =  [Admin].[udf_GetTrueDate] ()

CREATE TABLE #Source (
	[InventoryId]              VARCHAR(50)	collate SQL_Latin1_General_CP1_CS_AS NOT NULL,
	[InventoryProductId]       INT                NOT NULL,
	[CalculationDate]          DATE               NULL,
	[NodeId]                   INT                NOT NULL,
	[NodeName]                 NVARCHAR (150)     NOT NULL,
	[TankName]                 NVARCHAR (20)      NULL,
	[BatchId]                  NVARCHAR (150)     NULL,
	[ProductName]              NVARCHAR (150)     NOT NULL,
	[ProductVolume]            DECIMAL  (29, 2)   NULL,
	[MeasurmentUnit]           NVARCHAR (150)     NOT NULL,
	[EventType]                NVARCHAR (10)      NOT NULL,
	[SystemName]               NVARCHAR (150)     NOT NULL,
	[AttributeValue]           NVARCHAR (150)     NOT NULL,
	[ValueAttributeUnit]       NVARCHAR (150)     NOT NULL,
	[AttributeDescription]     NVARCHAR (150)     NULL,
	[ProdutId]                 NVARCHAR (20)      NOT NULL,
	[Category]                 NVARCHAR (150)     NOT NULL,
	[Element]                  NVARCHAR (150)     NOT NULL,
	[GrossStandardQuantity]    DECIMAL  (18, 2)   NULL,
	[AttributeId]              NVARCHAR (150)     NOT NULL,
	[TicketId]				   INT				  NULL,

	--Internal Common Columns
	[CreatedBy]			       NVARCHAR (260)	  NULL,
	[CreatedDate]			   DATETIME			  NULL,
	LastModifiedBy			   NVARCHAR (260)	  NULL,
	LastModifiedDate           DATETIME           NULL
    );

SELECT  --DISTINCT
        Inv.InventoryId,
		Inv.InventoryProductId,
        Inv.InventoryDate,
        Inv.NodeId AS NodeId,        
        Inv.TankName,
		Inv.BatchId,
        Inv.ProductId ProdutId,
        Inv.ProductName AS ProductName,
        CAST(Inv.ProductVolume  AS DECIMAL(29,2)) ProductVolume,
        CEUnits.Name AS MeasurmentUnit,
		Inv.EventType,
		Inv.SourceSystem AS SystemName,
        ATT.AttributeValue  AS AttributeValue,
        ValAttUnit.[Name]   AS ValueAttributeUnit,
        ATT.AttributeDescription,
        CA.Name AS Category,
        CE.Name AS Element,
        Inv.NodeName AS NodeName,
        Inv.GrossStandardQuantity as GrossStandardQuantity,
        [AttributeId].[Name]  AS AttributeId,
        Inv.InventoryDate AS CalculationDate,
		CE.ElementId AS ElementId,
		Tic.TicketId AS TicketId
		INTO #Segment
  FROM [Admin].[view_InventoryInformation] Inv
  INNER JOIN  [Admin].[CategoryElement] CE
  ON CE.[ElementId] = Inv.[SegmentId]
  INNER JOIN Admin.Ticket Tic
  ON Inv.TicketId = Tic.TicketId 
  INNER JOIN Admin.Category CA
  ON CA.CategoryId = CE.CategoryId
  INNER JOIN [Admin].[CategoryElement] CEUnits
  ON CEUnits.ElementId= Inv.MeasurementUnit
  INNER JOIN [Admin].[Attribute] ATT
  ON Inv.InventoryProductId = ATT.InventoryProductId 
  INNER JOIN [Admin].[CategoryElement] ValAttUnit
  ON ATT.ValueAttributeUnit = ValAttUnit.ElementId
  INNER JOIN [Admin].[CategoryElement] AttributeId
  ON AttributeId.ElementId = Att.AttributeId
  AND AttributeId.CategoryId = 20
  WHERE Tic.[Status] = 0            --> 0 = successfully processed
    AND Tic.ErrorMessage IS NULL
	AND  (Tic.[TicketId]= @TicketId OR @TicketId = -1 )
	AND (@NodeId IS  NULL OR  (Inv.NodeId = @NodeId))

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
	,AttributeValue
	,ValueAttributeUnit
	,AttributeDescription
	,ProdutId
	,Cat1.Name AS Category
    ,CatEle.Name AS Element
    ,GrossStandardQuantity
    ,AttributeId
	,TicketId
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

   INSERT INTO #Source(
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
	,AttributeValue
	,ValueAttributeUnit
	,AttributeDescription
	,ProdutId
	,Category
	,Element
    ,GrossStandardQuantity
    ,AttributeId
	,TicketId
	,[CreatedBy]
	,[CreatedDate]
	,[LastModifiedBy]
	,[LastModifiedDate]
   )
   SELECT --DISTINCT 
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
	,AttributeValue
	,ValueAttributeUnit
	,AttributeDescription
	,ProdutId
	,Category
	,Element
    ,GrossStandardQuantity
    ,AttributeId
	,TicketId
	,'ReportUser' as [CreatedBy]
	,@TodaysDate as [CreatedDate]
	,NULL as [LastModifiedBy]
	,NULL as [LastModifiedDate]
    FROM
    (
	 SELECT --DISTINCT 
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
	,AttributeValue
	,ValueAttributeUnit
	,AttributeDescription
	,ProdutId
	,Category
	,Element
    ,GrossStandardQuantity
    ,AttributeId
	,TicketId
	FROM #Segment
	UNION
	SELECT --DISTINCT 
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
	,AttributeValue
	,ValueAttributeUnit
	,AttributeDescription
	,ProdutId
	,Category
	,Element
    ,GrossStandardQuantity
    ,AttributeId
	,TicketId
	FROM #System
	) SubQ

	MERGE [Admin].[QualityDetailsWithoutOwner] AS TARGET 
				USING #Source AS SOURCE  
				ON (
				    ISNULL(TARGET.InventoryId       ,'')  =     ISNULL(SOURCE.InventoryId		 ,'')
				AND ISNULL(TARGET.InventoryProductId,'')  =     ISNULL(SOURCE.InventoryProductId ,'')
				AND ISNULL(TARGET.CalculationDate   ,'')  =     ISNULL(SOURCE.CalculationDate	 ,'')
				AND ISNULL(TARGET.NodeId            ,'')  =     ISNULL(SOURCE.NodeId			 ,'')
				AND ISNULL(TARGET.NodeName          ,'')  =     ISNULL(SOURCE.NodeName			 ,'')
				AND ISNULL(TARGET.TankName          ,'')  =     ISNULL(SOURCE.TankName			 ,'')
				AND ISNULL(TARGET.NodeName          ,'')  =     ISNULL(SOURCE.NodeName			 ,'')
				AND ISNULL(TARGET.ProductName       ,'')  =     ISNULL(SOURCE.ProductName		 ,'')
				AND ISNULL(TARGET.ProdutId          ,'')  =     ISNULL(SOURCE.ProdutId			 ,'')
				AND ISNULL(TARGET.[EventType]       ,'')  =     ISNULL(SOURCE.[EventType]		 ,'')
				AND ISNULL(TARGET.[Category]        ,'')  =     ISNULL(SOURCE.[Category]		 ,'')
				AND ISNULL(TARGET.[Element]         ,'')  =     ISNULL(SOURCE.[Element]			 ,'')
				AND ISNULL(TARGET.MeasurmentUnit    ,'')  =     ISNULL(SOURCE.MeasurmentUnit	 ,'')
				AND ISNULL(TARGET.AttributeId       ,'')  =     ISNULL(SOURCE.AttributeId	     ,'')
				AND ISNULL(TARGET.TicketId          ,'')  =     ISNULL(SOURCE.TicketId	         ,'')
				)

				WHEN MATCHED AND ( 
				                    TARGET.BatchId                 <> SOURCE.BatchId OR
					                TARGET.ProductVolume           <> SOURCE.ProductVolume OR
					                TARGET.[SystemName]            <> SOURCE.[SystemName] OR
					                TARGET.AttributeValue          <> SOURCE.AttributeValue OR
					                TARGET.ValueAttributeUnit      <> SOURCE.ValueAttributeUnit OR	
									TARGET.AttributeDescription    <> SOURCE.AttributeDescription OR
					                TARGET.GrossStandardQuantity   <> SOURCE.GrossStandardQuantity 
									)
                     THEN UPDATE 
					  SET TARGET.BatchId                 = SOURCE.BatchId
					     ,TARGET.ProductVolume           = SOURCE.ProductVolume
					     ,TARGET.[SystemName]            = SOURCE.[SystemName]
					     ,TARGET.AttributeValue          = SOURCE.AttributeValue
					     ,TARGET.ValueAttributeUnit      = SOURCE.ValueAttributeUnit	
						 ,TARGET.AttributeDescription    = SOURCE.AttributeDescription
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
	                          ,AttributeValue
	                          ,ValueAttributeUnit
	                          ,AttributeDescription
	                          ,ProdutId
	                          ,Category
	                          ,Element
                              ,GrossStandardQuantity
                              ,AttributeId
							  ,TicketId
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
	                          ,SOURCE.AttributeValue
	                          ,SOURCE.ValueAttributeUnit
	                          ,SOURCE.AttributeDescription
	                          ,SOURCE.ProdutId
	                          ,SOURCE.Category
	                          ,SOURCE.Element
                              ,SOURCE.GrossStandardQuantity
                              ,SOURCE.AttributeId
							  ,SOURCE.TicketId
                              ,SOURCE.[CreatedBy]
                              ,SOURCE.[CreatedDate]
                              ,SOURCE.[LastModifiedBy]
                              ,SOURCE.[LastModifiedDate]
					);
END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This procedure is to Fetch Data [Admin].[QualityDetailsWithoutOwner] For PowerBi Report From Tables',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveQualityDetails',
    @level2type = NULL,
    @level2name = NULL
GO