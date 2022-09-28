/*-- =================================================================================================
-- Author:		Microsoft
-- Create date: Oct-21-2019
-- Updated date: Apr-06-2020  -- Added SystemName as per PBI#28962.
-- Updated date: Apr-23-2020  -- Added EventType Column as per PBI#25056.
--               Apr-23-2020  -- Removed DISTINCT to get all the Movement Attribute details
--               Apr-30-2020  -- Added Movement Transaction Id column to make the records as unique while showing it in reports
--               Jun-11-2020  -- Added AttributeId column as per #PBI31874
--               Jun-11-2020  -- Added BatchId column as per #PBI31874
--               Jun-11-2020  -- Modified OrderBy from ConcatenatedNodeName to MovementId
--               Jun-15-2020  -- Changed the logic of ValueAttributeUnit as per PBI 31874
-- Updated date: Jun-25-2020  -- Update origin column logic 
-- Updated date: Jun-25-2020  -- AttributeId homologated to categoryElement table. (AttributeId = ElementName)
-- Updated date: Jun-26-2020  -- Added condition categoryid = 20 for generating attribute name
-- Updated date: Jun-26-2020  -- Removed unnecessary joins
-- <Description>:	This SP is to Fetch  AttributeDetails  Data For PowerBi Report From
				Tables(Movement, Product, Ticket, Node, NodeTag, CategoryElement, Category, Attribute) </Description>
	Test Script: EXEC [Admin].[usp_SaveAttributeDetails] @TicketId = -1
-- ===================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveAttributeDetails] 
(
  @TicketId INT
)
AS
BEGIN

DECLARE @Todaysdate	 DATETIME =  [Admin].[udf_GetTrueDate] ()

DROP TABLE IF EXISTS #Segment;
DROP TABLE IF EXISTS #System;
DROP TABLE IF EXISTS #Source;

                        SELECT      
                           [Mov].[MovementId]																						 -- Identificación del movimiento
                          ,[Mov].[MovementTransactionId]
                          ,[Mov].[OperationalDate]	                AS OperationalDate												 -- Fecha (operacional)
                          ,Mov.MovementTypeName  					AS Operacion                                                     -- Operación
                          ,[Mov].[SourceNodeName]					AS SourceNodeName											     -- Nodo Origen
                          ,[Mov].[DestinationNodeName]				AS DestinationNodeName											 -- Nodo Destino
                          ,[Mov].[SourceProductName]				AS SourceProductName											 -- Producto Origen
                          ,[Mov].[DestinationProductName]			AS DestinationProductName									     -- Producto Destino
                          ,[Mov].NetStandardVolume	                AS NetStandardVolume										     -- Volumen Neto
                          ,[Mov].GrossStandardVolume               	AS GrossStandardVolume 									         -- Volumen Bruto
                          ,CEUnits.Name AS MeasurementUnit																			 -- Unidad
						  ,Mov.EventType                                                                                             --Acción
                          ,Mov.BatchId
                          --,CASE 
		                  --     WHEN [Mov].MovementTypeId IN (42,43,44) 
		 	              --       THEN [Mov].SourceSystem
		                  --     WHEN ISNULL([Mov].SystemName,'') = ''
			              --      THEN [Mov].SourceSystem
		 	              --       ELSE [Mov].SystemName	
		                  --END	AS [SystemName]															                             -- Origen
                          ,[Mov].SourceSystem AS [SystemName]
                          ,[AttributeId].[Name]  AS AttributeId                                                                     -- Id atributo
					      ,Attr.AttributeValue																						 -- Valor Atributo
     				      ,ValAttUnit.[Name]                      AS ValueAttributeUnit										         -- Unidad Atributo
     				      ,Attr.AttributeDescription																				 -- Descripción Atributo
                          ,[Prod].[ProductId]    				  AS [SourceProductId]
                          ,Cat.[Name] AS [Category]
                          ,Element.[Name] AS [Element]   
						  ,CONCAT('-_-', [Mov].SourceNodeName, '-_-', Mov.DestinationNodeName, '-_-') AS [NodeName]
                          ,ISNULL([Mov].SourceNodeName,'') + ISNULL([Mov].DestinationNodeName,'') AS [ConcatenatedNodeName]
						  ,Mov.OperationalDate  AS [CalculationDate]
						  ,Element.ElementId
                          ,Tick.TicketId AS [TicketId]
                     INTO #Segment
                     FROM [Admin].[view_MovementInformation] [Mov]
                     INNER JOIN [Admin].[Product] Prod
                             ON (Prod.ProductId = Mov.SourceProductId
                     		OR Prod.ProductId = Mov.DestinationProductId)
                     INNER JOIN [Admin].Ticket Tick
                     		ON Mov.TicketId = Tick.TicketId
                     INNER JOIN [Admin].CategoryElement Element
                     		ON Mov.SegmentId = Element.ElementId
                     INNER JOIN [Admin].Category  Cat
                             ON Element.CategoryId = Cat.CategoryId
                     INNER JOIN [Admin].[CategoryElement] CEUnits
                             ON CEUnits.ElementId= Mov.MeasurementUnit 
                     INNER JOIN [Admin].[Attribute] Attr
                     	   ON Attr.MovementTransactionId = Mov.MovementTransactionId
                     INNER JOIN [Admin].[CategoryElement] ValAttUnit
                            ON Attr.ValueAttributeUnit = ValAttUnit.ElementId
                     INNER JOIN [Admin].[CategoryElement] AttributeId
                             ON AttributeId.ElementId = Attr.AttributeId
                             AND AttributeId.CategoryId = 20
                     WHERE CEUnits.CategoryID = 6 --'Unidad de Medida' From name Column in Category
                       AND Tick.Status = 0		--> 0 = successfully processed
                       AND Tick.ErrorMessage IS NULL
					   AND  (Tick.[TicketId]= @TicketId OR @TicketId = -1 )

					   SELECT
                           [MovementId]
                          ,[MovementTransactionId]
                          ,OperationalDate
                          ,Operacion
                          ,SourceNodeName
                          ,DestinationNodeName
                          ,SourceProductName
                          ,DestinationProductName
                          ,NetStandardVolume
                          ,GrossStandardVolume
                          ,MeasurementUnit
                          ,EventType
                          ,BatchId
                          ,SystemName
                          ,AttributeId
                          ,AttributeValue
                          ,ValueAttributeUnit
                          ,AttributeDescription
                          ,[SourceProductId]
                          ,Cat1.[Name] AS [Category]
                          ,CatEle.[Name] AS [Element]
                          ,[NodeName]
                          ,[CalculationDate]
                          ,[TicketId]
					      INTO #System
					  FROM #Segment SG
                      INNER JOIN Admin.NodeTag NT
		              ON NT.ElementId = SG.ElementId
		              INNER JOIN Admin.NodeTag NT1
		              ON NT.NodeId = NT1.NodeId
                      INNER JOIN Admin.CategoryElement CatEle
		              ON CatEle.ElementId = NT1.ElementId AND CatEle.CategoryId = 8
                      INNER JOIN Admin.Category  Cat1
		              ON CatEle.CategoryId = Cat1.CategoryId;


					 SELECT
                     [MovementId]
                    ,[MovementTransactionId]
                    ,OperationalDate
                    ,Operacion
                    ,SourceNodeName
                    ,DestinationNodeName
                    ,SourceProductName
                    ,DestinationProductName
                    ,NetStandardVolume
                    ,GrossStandardVolume
                    ,MeasurementUnit
                    ,EventType
                    ,BatchId
                    ,SystemName
                    ,AttributeId
                    ,AttributeValue
                    ,ValueAttributeUnit
                    ,AttributeDescription
                    ,[SourceProductId]
                    ,[Category]
                    ,[Element]
                    ,[NodeName]
                    ,[CalculationDate]
                    ,[TicketId]
					,'ReportUser' as [CreatedBy]
					,@TodaysDate as [CreatedDate]
					,NULL as [LastModifiedBy]
					,NULL as [LastModifiedDate]
					INTO #Source
					FROM
					(
					 SELECT [MovementId]
                    ,[MovementTransactionId]
                    ,OperationalDate
                    ,Operacion
                    ,SourceNodeName
                    ,DestinationNodeName
                    ,SourceProductName
                    ,DestinationProductName
                    ,NetStandardVolume
                    ,GrossStandardVolume
                    ,MeasurementUnit
                    ,EventType
                    ,BatchId
                    ,SystemName
                    ,AttributeId
                    ,AttributeValue
                    ,ValueAttributeUnit
                    ,AttributeDescription
                    ,[SourceProductId]
                    ,[Category]
                    ,[Element]
                    ,[NodeName]
                    ,[CalculationDate]
                    ,[TicketId]
					FROM #Segment
					UNION
					SELECT [MovementId]
                    ,[MovementTransactionId]
                    ,OperationalDate
                    ,Operacion
                    ,SourceNodeName
                    ,DestinationNodeName
                    ,SourceProductName
                    ,DestinationProductName
                    ,NetStandardVolume
                    ,GrossStandardVolume
                    ,MeasurementUnit
                    ,EventType
                    ,BatchId
                    ,SystemName
                    ,AttributeId
                    ,AttributeValue
                    ,ValueAttributeUnit
                    ,AttributeDescription
                    ,[SourceProductId]
                    ,[Category]
                    ,[Element]
                    ,[NodeName]
                    ,[CalculationDate]
                    ,[TicketId]
					FROM #System
					) SubQ
				
				MERGE [Admin].[AttributeDetailsWithOutOwner] AS TARGET 
				USING #Source AS SOURCE  
				ON  ISNULL(TARGET.[MovementTransactionId] ,'') = ISNULL(SOURCE.[MovementTransactionId] ,'')
				AND ISNULL(TARGET.[MovementId]            ,'') = ISNULL(SOURCE.[MovementId]            ,'')
				AND ISNULL(TARGET.[OperationalDate]       ,'') = ISNULL(SOURCE.[OperationalDate]       ,'')
				AND ISNULL(TARGET.[Operacion]             ,'') = ISNULL(SOURCE.[Operacion]             ,'')
				AND ISNULL(TARGET.[SourceNodeName]        ,'') = ISNULL(SOURCE.[SourceNodeName]        ,'')
				AND ISNULL(TARGET.[DestinationNodeName]   ,'') = ISNULL(SOURCE.[DestinationNodeName]   ,'')
				AND ISNULL(TARGET.[SourceProductName]     ,'') = ISNULL(SOURCE.[SourceProductName]     ,'')
				AND ISNULL(TARGET.[DestinationProductname],'') = ISNULL(SOURCE.[DestinationProductname],'')
				AND ISNULL(TARGET.[MeasurementUnit]       ,'') = ISNULL(SOURCE.[MeasurementUnit]       ,'')
				AND ISNULL(TARGET.[EventType]             ,'') = ISNULL(SOURCE.[EventType]             ,'')
				AND ISNULL(TARGET.[Category]              ,'') = ISNULL(SOURCE.[Category]              ,'')
				AND ISNULL(TARGET.[Element]               ,'') = ISNULL(SOURCE.[Element]               ,'')
				AND ISNULL(TARGET.[NodeName]              ,'') = ISNULL(SOURCE.[NodeName]              ,'')
				AND ISNULL(TARGET.AttributeId             ,'') = ISNULL(SOURCE.AttributeId             ,'')
                AND ISNULL(TARGET.[TicketId]              ,'') = ISNULL(SOURCE.[TicketId]              ,'')

				WHEN MATCHED  AND ( 
				                    TARGET.[NetStandardVolume]             <> SOURCE.[NetStandardVolume] OR
					                TARGET.[GrossStandardVolume]           <> SOURCE.[GrossStandardVolume] OR
					                TARGET.[SystemName]                    <> SOURCE.[SystemName] OR
					                TARGET.BatchId                         <> SOURCE.BatchId OR
					                TARGET.AttributeValue                  <> SOURCE.AttributeValue OR
					                TARGET.ValueAttributeUnit              <> SOURCE.ValueAttributeUnit OR		
					                TARGET.AttributeDescription            <> SOURCE.AttributeDescription OR	
					                TARGET.[SourceProductId]               <> SOURCE.[SourceProductId]  
									)


				THEN UPDATE 
					  SET TARGET.[NetStandardVolume]             = SOURCE.[NetStandardVolume] 
					     ,TARGET.[GrossStandardVolume]           = SOURCE.[GrossStandardVolume] 
					     ,TARGET.[SystemName]                    = SOURCE.[SystemName] 
					     ,TARGET.BatchId                         = SOURCE.BatchId 
					     ,TARGET.AttributeValue                  = SOURCE.AttributeValue 
					     ,TARGET.ValueAttributeUnit              = SOURCE.ValueAttributeUnit 		
					     ,TARGET.AttributeDescription            = SOURCE.AttributeDescription	
					     ,TARGET.[SourceProductId]               = SOURCE.[SourceProductId]	
					     ,TARGET.LastModifiedBy                  ='ReportUser'
					     ,TARGET.[LastModifiedDate]              = @TodaysDate
					  
       
				/* 2. Performing the INSERT operation */ 
  
				/* When no records are matched with TARGET table 
				   Then insert the records in the target table */ 
				WHEN NOT MATCHED BY TARGET  
				THEN INSERT ( 
				               [MovementId]
                              ,[MovementTransactionId]
                              ,[OperationalDate]
                              ,[Operacion]
                              ,[SourceNodeName]
                              ,[DestinationNodeName]
                              ,[SourceProductName]
                              ,[DestinationProductName]
                              ,[NetStandardVolume]
                              ,[GrossStandardVolume]
                              ,[MeasurementUnit]
                              ,[EventType]
                              ,[BatchId]
                              ,[SystemName]
                              ,[AttributeId]
                              ,[AttributeValue]
                              ,[ValueAttributeUnit]
                              ,[AttributeDescription]
                              ,[SourceProductId]
                              ,[Category]
                              ,[Element]
                              ,[NodeName]
                              ,[CalculationDate]
                              ,[TicketId]
                              ,[CreatedBy]
                              ,[CreatedDate]
                              ,[LastModifiedBy]
                              ,[LastModifiedDate]
							)

			 VALUES (
			          SOURCE.[MovementId]
                     ,SOURCE.[MovementTransactionId]
                     ,SOURCE.[OperationalDate]
                     ,SOURCE.[Operacion]
                     ,SOURCE.[SourceNodeName]
                     ,SOURCE.[DestinationNodeName]
                     ,SOURCE.[SourceProductName]
                     ,SOURCE.[DestinationProductName]
                     ,SOURCE.[NetStandardVolume]
                     ,SOURCE.[GrossStandardVolume]
                     ,SOURCE.[MeasurementUnit]
                     ,SOURCE.[EventType]
                     ,SOURCE.[BatchId]
                     ,SOURCE.[SystemName]
                     ,SOURCE.[AttributeId]
                     ,SOURCE.[AttributeValue]
                     ,SOURCE.[ValueAttributeUnit]
                     ,SOURCE.[AttributeDescription]
                     ,SOURCE.[SourceProductId]
                     ,SOURCE.[Category]
                     ,SOURCE.[Element]
                     ,SOURCE.[NodeName]
                     ,SOURCE.[CalculationDate]
                     ,SOURCE.[TicketId]
                     ,SOURCE.[CreatedBy]
                     ,SOURCE.[CreatedDate]
                     ,SOURCE.[LastModifiedBy]
                     ,SOURCE.[LastModifiedDate]
					);

END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data [Admin].[usp_SaveAttributeDetails] For PowerBi Report From Tables',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveAttributeDetails',
    @level2type = NULL,
    @level2name = NULL
GO