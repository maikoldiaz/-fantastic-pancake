/*-- =================================================================================================
-- Author:		Microsoft
-- Created date: Aug-05-2020
-- Updated date: Aug-12-2020 -- Removed unnecessary join
-- Updated date: May-20-2021  add NodeId Validation
-- <Description>:	This View is to Fetch AttributeDetailsWithOwner Data For PowerBi Report From
				Tables(Unbalance, Product, Ticket, Node, NodeTag, CategoryElement,  Category, Ownership)</Description>
   EXEC [Admin].[usp_SaveAttributeDetailsWithOwner] @OwnershipTicketId = -1(full load)
   EXEC [Admin].[usp_SaveInventoryDetailsWithOwner] @OwnershipTicketId = 31352(ticketid load)
   SELECT * FROM Admin.[AttributeDetailsWithOwner]
-- ===================================================================================================*/

CREATE PROCEDURE [Admin].[usp_SaveAttributeDetailsWithOwner]
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
			SELECT      
                           [Mov].[MovementId]																						 -- Identificación del movimiento
                          ,[Mov].[MovementTransactionId]
						  ,[Mov].[OperationalDate]					AS OperationalDate												 -- Fecha (operacional)
                          ,Mov.MovementTypeName						AS Operacion                                                     -- Operación
                          ,[Mov].[SourceNodeName]					AS SourceNodeName											     -- Nodo Origen
                          ,[Mov].[DestinationNodeName]				AS DestinationNodeName											 -- Nodo Destino
                          ,[Mov].[SourceProductName]				AS SourceProductName											 -- Producto Origen
                          ,[Mov].[DestinationProductName]			AS DestinationProductName									     -- Producto Destino
                          ,[Mov].NetStandardVolume  AS NetStandardVolume										                     -- Volumen Neto
                          ,[Mov].GrossStandardVolume AS GrossStandardVolume									                         -- Volumen Bruto
                          ,CEUnits.[Name] AS MeasurementUnit																			 -- Unidad
						  ,Mov.EventType                                                                                             --Acción
						  ,Mov.SourceSystem AS [SystemName]
						  ,CASE 
								WHEN [Mov].SystemName = 'FICO' AND Mov.MovementTypeName IN ('Compra','Venta') 
										THEN mov.SourceMovementId
								WHEN Mov.MovementTypeName IN ('ACE Entrada','ACE Salida')
										THEN mov.SourceMovementId								
								ELSE NULL
							END										AS [SourceMovementId]											 -- Mov. Origen
						  ,CASE 
								WHEN [Mov].SystemName IN( 'FICO','TRUE') AND Mov.MovementTypeName IN ('Compra','Venta') 
										THEN [Cont].DocumentNumber 
								ELSE NULL
							END										AS [Order]														 -- Pedido
						  ,CASE 
								WHEN [Mov].SystemName IN( 'FICO','TRUE') AND Mov.MovementTypeName IN ('Compra','Venta') 
										THEN [Cont].Position 
								ELSE NULL
							END										AS [Position]													 -- Posición
                          ,ElementOwner.[Name] AS OwnerName																			 -- Propietario
                          ,[Ownership].OwnershipVolume	 AS OwnershipVolume									                         -- Volumen Propiedad
						  ,Attr.AttributeValue																						 -- Valor Atributo
     				      ,ValAttUnit.Name AS ValueAttributeUnit																					 -- Unidad Atributo
     				      ,Attr.AttributeDescription																				 -- Descripción Atributo
                          ,[Ownership].ExecutionDate AS OwnershipProcessDate														 -- Fecha Ejecución Propiedad
						  ,Mov.UncertaintyPercentage
                          ,[Prod].[ProductId]    AS [SourceProductId]
                          ,Cat.[Name] AS [Category]
                          ,Element.[Name] AS [Element]
						  ,[AttributeId].[Name]  AS AttributeId
						  ,Mov.BatchId
						  ,Element.ElementId
						  ,[Ownership].[TicketId] as [OwnershipTicketId]
						  INTO #Segment
			FROM [Admin].[view_MovementInformation] [Mov]
			INNER JOIN [offchain].[Ownership] [Ownership]
					ON [Mov].[MovementTransactionId] = [Ownership].[MovementTransactionId]
					AND [Mov].[OwnershipTicketId] = [Ownership].[TicketId]
					AND [Mov].[OwnershipTicketId] IS NOT NULL
			INNER JOIN [Admin].[Product] Prod
					ON (Prod.ProductId = Mov.SourceProductId
					OR Prod.ProductId = Mov.DestinationProductId)
			INNER JOIN Admin.CategoryElement Element
					ON Mov.SegmentId = Element.ElementId
			INNER JOIN [Admin].Category  Cat
					ON Element.CategoryId = Cat.CategoryId
			INNER JOIN [Admin].[CategoryElement] CEUnits
					ON CEUnits.ElementId = Mov.MeasurementUnit 
					AND CEUnits.CategoryID = 6 --'Unidad de Medida' From name Column in Category
			INNER JOIN [Admin].CategoryElement ElementOwner -- JOINING THIS TABLE WITH OWNERSHIP TABLE TO GET THE OWNER'S INFORMATION
					ON [Ownership].OwnerId = ElementOwner.ElementId 
					AND ElementOwner.CategoryID = 7 -- 'Propietario' From name Column in Category
			INNER JOIN [Admin].[Attribute] Attr
					ON Attr.MovementTransactionId = Mov.MovementTransactionId
			INNER JOIN [Admin].[CategoryElement] ValAttUnit
					ON Attr.ValueAttributeUnit = ValAttUnit.ElementId
			INNER JOIN [Admin].[CategoryElement] AttributeId
					ON AttributeId.ElementId = Attr.AttributeId
					AND AttributeId.CategoryId = 20
			LEFT JOIN [Admin].[MovementContract] [Cont]
					ON [Mov].ContractId = [Cont].ContractId
					AND	[Cont].IsDeleted = 0
			WHERE [Ownership].IsDeleted = 0
			AND [Mov].[OwnershipTicketId] IS NOT NULL
		    AND ([Ownership].[TicketId]= @OwnershipTicketId OR @OwnershipTicketId =-1)
			
			AND (@NodeId IS  NULL OR  ([Mov].DestinationNodeId = @NodeId OR [Mov].SourceNodeId = @NodeId))
	 


	SELECT --DISTINCT
			 [MovementId]
			 ,MovementTransactionId
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
			 ,SystemName
			 ,SourceMovementId
			 ,[Order]
			 ,Position
			 ,OwnerName
			 ,OwnershipVolume
			 ,AttributeValue
			 ,ValueAttributeUnit
			 ,AttributeDescription
			 ,OwnershipProcessDate
			 ,CAST(ISNULL(NetStandardVolume,1)*ISNULL(UncertaintyPercentage,1) AS DECIMAL(29,2)) AS [Uncertainty]
			 ,[SourceProductId]
			 ,[Category]
			 ,[Element]
			 ,CONCAT('-_-', SourceNodeName, '-_-', DestinationNodeName, '-_-') AS [NodeName]
			 ,OperationalDate AS [CalculationDate]
			 ,AttributeId
			 ,BatchId
			 ,OwnershipTicketId
			 ,'ReportUser'   as [CreatedBy]
			 ,@TodaysDate    as [CreatedDate]
			 ,NULL			as [LastModifiedBy]
			 ,NULL           as [LastModifiedDate]
					INTO #Source
			 FROM (
					SELECT   SG.MovementID
							,MovementTransactionId
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
							,[SystemName]
							,[SourceMovementId]										
							,[Order]												
							,[Position]												
							,OwnerName												
							,OwnershipVolume									    
     						,AttributeValue											
     						,ValueAttributeUnit										
     						,AttributeDescription									
							,OwnershipProcessDate									
							,UncertaintyPercentage
							,[SourceProductId]
							,[Category]
							,[Element]
							,AttributeId
							,BatchId
							,OwnershipTicketId
						FROM 
						#Segment SG

				    UNION

					SELECT   SG.MovementID
							,SG.MovementTransactionId
							,SG.OperationalDate
							,SG.Operacion
							,SG.SourceNodeName
							,SG.DestinationNodeName
							,SG.SourceProductName
							,SG.DestinationProductName                           
							,SG.NetStandardVolume								
							,SG.GrossStandardVolume								
							,SG.MeasurementUnit									
							,SG.EventType                                        
							,SG.[SystemName]
							,SG.[SourceMovementId]								
							,SG.[Order]											
							,SG.[Position]										
							,SG.OwnerName										
							,SG.OwnershipVolume									 
     						,SG.AttributeValue									
     						,SG.ValueAttributeUnit								
     						,SG.AttributeDescription							
							,SG.OwnershipProcessDate							
							,SG.UncertaintyPercentage
							,SG.[SourceProductId]
							,Cat1.[Name] AS [Category]
							,CatEle.[Name] AS [Element]
							,SG.AttributeId
							,SG.BatchId
							,OwnershipTicketId
					FROM 
					#Segment SG
					INNER JOIN Admin.NodeTag NT
							ON NT.ElementId = SG.ElementId
					INNER JOIN Admin.NodeTag NT1
							ON NT.NodeId = NT1.NodeId
					INNER JOIN Admin.CategoryElement CatEle
							ON CatEle.ElementId = NT1.ElementId AND CatEle.CategoryId = 8
					INNER JOIN Admin.Category  Cat1
							ON CatEle.CategoryId = Cat1.CategoryId 
					)SubQ
				

				MERGE Admin.[AttributeDetailsWithOwner] AS TARGET 
				USING #Source AS SOURCE 
				ON  ISNULL(TARGET.[MovementTransactionId]		,'')=	ISNULL(SOURCE.[MovementTransactionId]	,'')
				AND ISNULL(TARGET.[MovementId]					,'')=	ISNULL(SOURCE.MovementId				,'')
				AND ISNULL(TARGET.[OperationalDate]				,'')=	ISNULL(SOURCE.[OperationalDate]			,'')
				AND ISNULL(TARGET.[SourceNodeName]				,'')=	ISNULL(SOURCE.[SourceNodeName]			,'')
				AND ISNULL(TARGET.[DestinationNodeName]			,'')=	ISNULL(SOURCE.[DestinationNodeName]		,'')
				AND ISNULL(TARGET.[SourceProductName]			,'')=	ISNULL(SOURCE.[SourceProductName]		,'')
				AND ISNULL(TARGET.[DestinationProductName]		,'')=	ISNULL(SOURCE.[DestinationProductName]	,'')
				AND ISNULL(TARGET.[MeasurementUnit]				,'')=	ISNULL(SOURCE.[MeasurementUnit]			,'')
				AND ISNULL(TARGET.[SourceProductId]				,'')=	ISNULL(SOURCE.[SourceProductId]			,'')
				AND ISNULL(TARGET.[OwnerName]					,'')=	ISNULL(SOURCE.[OwnerName]				,'')
				AND ISNULL(TARGET.[Operacion]					,'')=	ISNULL(SOURCE.[Operacion]				,'')
				AND ISNULL(TARGET.AttributeId					,'')=	ISNULL(SOURCE.AttributeId				,'')
				AND ISNULL(TARGET.[Category]					,'')=	ISNULL(SOURCE.[Category]				,'')
				AND ISNULL(TARGET.[Element]						,'')=	ISNULL(SOURCE.[Element]					,'')
				AND ISNULL(TARGET.[NodeName]					,'')=	ISNULL(SOURCE.[NodeName]				,'')
				AND ISNULL(TARGET.[OwnershipTicketId]			,'')=	ISNULL(SOURCE.[OwnershipTicketId]       ,'')
				



				WHEN MATCHED  AND ( 
					  TARGET.[NetStandardVolume]               <>  SOURCE.[NetStandardVolume]		OR
					  TARGET.[GrossStandardVolume]			   <>  SOURCE.[GrossStandardVolume]		OR
					  TARGET.[SystemName]					   <>  SOURCE.[SystemName]				OR
					  TARGET.[OwnershipVolume]				   <>  SOURCE.[OwnershipVolume]			OR
					  TARGET.[Uncertainty]					   <>  SOURCE.[Uncertainty]				OR		
					  TARGET.[SourceProductId]				   <>  SOURCE.[SourceProductId]			OR
					  TARGET.[Order]                           <>  SOURCE.[Order]					OR
					  TARGET.[Position]                        <>  SOURCE.[Position]			    OR
					  TARGET.[EventType]                       <>  SOURCE.[EventType]				OR
					  TARGET.[SourceMovementId]                <>  SOURCE.[SourceMovementId]		OR
					  TARGET.[BatchId]						   <>  SOURCE.[BatchId]                 OR     
					  TARGET.[AttributeValue]				   <>  SOURCE.[AttributeValue]		    OR
				      TARGET.[ValueAttributeUnit]			   <>  SOURCE.[ValueAttributeUnit]	    OR
				      TARGET.[AttributeDescription]		       <>  SOURCE.[AttributeDescription]	
					  )


				THEN UPDATE 
					 SET 
					  	    TARGET.[NetStandardVolume]       = SOURCE.[NetStandardVolume]	
					  	   ,TARGET.[GrossStandardVolume]     = SOURCE.[GrossStandardVolume]	
					  	   ,TARGET.[SystemName]				 = SOURCE.[SystemName]			
					  	   ,TARGET.[OwnershipVolume]		 = SOURCE.[OwnershipVolume]		
					  	   ,TARGET.[Uncertainty]			 = SOURCE.[Uncertainty]			
					  	   ,TARGET.[SourceProductId]		 = SOURCE.[SourceProductId]		
					  	   ,TARGET.[Order]                   = SOURCE.[Order]				
					  	   ,TARGET.[Position]                = SOURCE.[Position]				
					  	   ,TARGET.[EventType]               = SOURCE.[EventType]			
					  	   ,TARGET.[SourceMovementId]        = SOURCE.[SourceMovementId]		
						   ,TARGET.[BatchId]				 = SOURCE.[BatchId]   
						   ,TARGET.[AttributeValue]			 = SOURCE.[AttributeValue]		    
				           ,TARGET.[ValueAttributeUnit]		 = SOURCE.[ValueAttributeUnit]	    
				           ,TARGET.[AttributeDescription]	 = SOURCE.[AttributeDescription]	
						   ,TARGET.LastModifiedBy            = 'ReportUser'
						   ,TARGET.[LastModifiedDate]        = @TodaysDate
					  
       
				/* 2. Performing the INSERT operation */ 
  
				/* When no records are matched with TARGET table 
				   Then insert the records in the target table */ 
				WHEN NOT MATCHED BY TARGET 
				
				THEN INSERT (
				[MovementId]
				,MovementTransactionId
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
				,SystemName
				,SourceMovementId
				,[Order]
				,Position
				,OwnerName
				,OwnershipVolume
				,AttributeValue
				,ValueAttributeUnit
				,AttributeDescription
				,OwnershipProcessDate
				,[Uncertainty]
				,[SourceProductId]
				,[Category]
				,[Element]
				,[NodeName]
				,[CalculationDate]
				,AttributeId
				,BatchId
				,OwnershipTicketId
				,[CreatedBy]
				,[CreatedDate]
				,[LastModifiedBy]
				,[LastModifiedDate])

			 VALUES 
			 (
			 SOURCE.[MovementId]
			,SOURCE.MovementTransactionId
			,SOURCE.OperationalDate
			,SOURCE.Operacion
			,SOURCE.SourceNodeName
			,SOURCE.DestinationNodeName
			,SOURCE.SourceProductName
			,SOURCE.DestinationProductName
			,SOURCE.NetStandardVolume
			,SOURCE.GrossStandardVolume
			,SOURCE.MeasurementUnit
			,SOURCE.EventType
			,SOURCE.SystemName
			,SOURCE.SourceMovementId
			,SOURCE.[Order]
			,SOURCE.Position
			,SOURCE.OwnerName
			,SOURCE.OwnershipVolume
			,SOURCE.AttributeValue
			,SOURCE.ValueAttributeUnit
			,SOURCE.AttributeDescription
			,SOURCE.OwnershipProcessDate
			,SOURCE.[Uncertainty]
			,SOURCE.[SourceProductId]
			,SOURCE.[Category]
			,SOURCE.[Element]
			,SOURCE.[NodeName]
			,SOURCE.[CalculationDate]
			,SOURCE.AttributeId
			,SOURCE.BatchId
			,SOURCE.OwnershipTicketId
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
    @level1name = N'usp_SaveAttributeDetailsWithOwner',
    @level2type = NULL,
    @level2name = NULL