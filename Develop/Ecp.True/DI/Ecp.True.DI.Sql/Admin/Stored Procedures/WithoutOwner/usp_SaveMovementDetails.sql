/*-- =================================================================================================
-- Author      :	Microsoft
-- Created date:    Jul-31-2019
-- Updated date:    Aug-12-2020 -- Removed unnecessary join
-- Updated date:    Nov-18-2020 -- Changed logic to use ElementId of Ticket
-- Updated date:    Jun-06-2021 -- Se Modifica Filtro a nivel de sistema, para que se puede filtrar por el nodo.
-- Updated date:    Jun-06-2021 -- Se Modifica Filtro a nivel de sistema, para que se puede filtrar por el nodo.
-- Updated date:    Jun-25-2021 -- excluir los tipos de movimientos Anulacion delta em evacuacion y Anulacion delta sm evacuacion.
-- Updated date:    Jul-28-2021 -- incluir los tipos de movimientos Anulacion delta em evacuacion y Anulacion delta sm evacuacion.
-- <Description>:	This procedure is to Fetch view_MovementDetails Data For PowerBi Report From
				Tables(Movement, Product, Ticket, Node, NodeTag, CategoryElement, Category, VariableType)</Description>
   EXEC [Admin].[usp_SaveMovementDetails] @TicketId = -1
-- ===================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveMovementDetails] 
(
  @TicketId INT
)
AS
BEGIN

DROP TABLE IF EXISTS #Segement;
DROP TABLE IF EXISTS #System;
DROP TABLE IF EXISTS #Source;

DECLARE @Todaysdate	 DATETIME =  [Admin].[udf_GetTrueDate] ()

        		 SELECT --DISTINCT			  
				 	    Mov.MovementId                                                      AS [MovementId]																
				       ,Mov.MovementTransactionId                                           AS [MovementTransactionId]
				 	   ,Mov.OperationalDate                                                 AS [OperationalDate]						
				       ,Mov.MovementTypeName                                                 AS [Operacion]								
				       ,Mov.SourceNodeName                                                  AS [SourceNode]							
				       ,Mov.DestinationNodeName                                             AS [DestinationNode]						
				       ,Mov.SourceProductName                                               AS [SourceProduct]							
				       ,Mov.DestinationProductName                                          AS [DestinationProduct]					
				       ,Mov.NetStandardVolume                                               AS [NetStandardVolume]						
				       ,Mov.GrossStandardVolume                                             AS [GrossStandardVolume]					
				       ,CEUnits.[Name]                                                      AS [MeasurementUnit]
				 	   ,Mov.EventType													    AS [EventType]                                                               
				 	   ,Mov.BatchId                                                         AS [BatchId]
				 	   ,[Mov].SourceSystem                                                  AS [SystemName]	
				       ,Vt.[VariableTypeId]												    AS [VariableTypeId]
				 	   ,[Mov].MessageTypeId												    AS [MessageTypeId]
				 	   ,[Mov].[Classification]                                              AS [Classification]
					   ,CASE 
		                    WHEN VT.[VariableTypeId] = 1 AND MessageTypeId NOT IN (1,2)
		                     AND Mov.MovementTypeName = 'Traslado de productos'
		                    THEN 'Interfases'
		                    WHEN VT.[VariableTypeId] = 2 AND MessageTypeId NOT IN (1,2) 
		                     AND Mov.MovementTypeName = 'Tolerancia' 
		                    THEN 'Tolerancia'
		                    WHEN VT.[VariableTypeId] = 3 AND MessageTypeId NOT IN (1,2) 
		                    	 AND Mov.MovementTypeName = 'Pérdida no identificada' 
		                    		THEN 'Pérdidas No Identificadas'
		                    WHEN [Classification] = 'PerdidaIdentificada' AND MessageTypeId = 2 
		                    		THEN 'Pérdidas Identificadas'
                         END AS [Movements]
				 	   ,Mov.UncertaintyPercentage                                           AS [PercentStandardUnCertainty]
				       ,ISNULL(Mov.NetStandardVolume,1)*ISNULL(Mov.UncertaintyPercentage,1) AS [Uncertainty]
				 	   ,Mov.[BackupMovementId]                                              AS [BackupMovementId]
				       ,Mov.[GlobalMovementId]                                              AS [GlobalMovementId]
				       ,Prod.ProductId                                                      AS [SourceProductId]
					   ,Mov.SourceNodeId													AS SourceNodeId
					   ,Mov.DestinationNodeId												AS DestinationNodeId
				       ,Cat.[Name]                                                          AS [Category]
				       ,Element.[Name]                                                      AS [Element]     
					   ,Element.ElementId
					   ,Tick.TicketId
					   INTO #Segement
				 FROM [Admin].[view_MovementInformation] Mov
				 INNER JOIN [Admin].[Product] Prod
				         ON (Prod.ProductId = Mov.SourceProductId
				 		OR Prod.ProductId = Mov.DestinationProductId)
				 INNER JOIN [Admin].Ticket Tick
						ON Mov.TicketId = Tick.TicketId
				 INNER JOIN [Admin].CategoryElement Element
						ON Tick.CategoryElementId = Element.ElementId
				 INNER JOIN [Admin].Category  Cat
				 		ON Element.CategoryId = Cat.CategoryId
				 INNER JOIN [Admin].[CategoryElement] CEUnits
				 		ON CEUnits.ElementId = Mov.MeasurementUnit 
				 LEFT JOIN [Admin].[VariableType] Vt
				 		ON Vt.VariableTypeId = Mov.VariableTypeId
				 WHERE CEUnits.[CategoryId] = 6--'Unidad de Medida' From name Column in Category
				 AND   Tick.[Status] = 0		--> 0 = successfully processed
				 AND   Tick.ErrorMessage IS NULL 
				 AND  (Tick.[TicketId]= @TicketId OR @TicketId = -1 )

				 DROP INDEX IF EXISTS ix_ElementId ON #Segement;
				 DROP INDEX IF EXISTS ix_SourceNodeId ON #Segement;
				 DROP INDEX IF EXISTS ix_SourceNodeId ON #Segement;
				 DROP INDEX IF EXISTS ix_CategoryId ON #Segement;

				 CREATE NONCLUSTERED INDEX ix_ElementId ON #Segement ([ElementId]);
				 CREATE NONCLUSTERED INDEX ix_SourceNodeId ON #Segement ([SourceNodeId]);
				 CREATE NONCLUSTERED INDEX ix_DestinationNodeId ON #Segement ([DestinationNodeId]);
				 CREATE NONCLUSTERED INDEX ix_CategoryId ON #Segement ([Category]);

				SELECT      SG.[MovementId]																
				           ,SG.[MovementTransactionId]
				     	   ,SG.[OperationalDate]						
				           ,SG.[Operacion]								
				           ,SG.[SourceNode]							
				           ,SG.[DestinationNode]						
				           ,SG.[SourceProduct]							
				           ,SG.[DestinationProduct]					
				           ,SG.[NetStandardVolume]						
				           ,SG.[GrossStandardVolume]					
				           ,SG.[MeasurementUnit]
				     	   ,SG.[EventType]                                                               
				     	   ,SG.[BatchId]
				     	   ,SG.[SystemName]	
				           ,SG.[Movements]
				     	   ,SG.[PercentStandardUnCertainty]
				           ,SG.[Uncertainty]
				     	   ,SG.[BackupMovementId]
				           ,SG.[GlobalMovementId]
				           ,SG.[SourceProductId]
				           ,Cat1.[Name] AS [Category]
				           ,CatEle.[Name]  AS [Element]               
						   ,SourceNodeId	
					       ,DestinationNodeId
						   ,[MessageTypeId]
						   ,[Classification]
						   ,[TicketId]
				 INTO #System 
				 FROM #Segement SG
				 INNER JOIN Admin.NodeTag NT
		             ON NT.ElementId = SG.ElementId and (SG.SourceNodeId = NT.NodeId or SG.DestinationNodeId = NT.NodeId) 
				INNER JOIN Admin.NodeTag NT1
		             ON NT.NodeId = NT1.NodeId
               INNER JOIN Admin.CategoryElement CatEle
		             ON CatEle.ElementId = NT1.ElementId AND CatEle.CategoryId = 8
               INNER JOIN Admin.Category  Cat1
		             ON CatEle.CategoryId = Cat1.CategoryId

				DROP INDEX IF EXISTS ix_DestinationNodeIdS ON #System;
				DROP INDEX IF EXISTS ix_SourceNodeIdS ON #System;

				CREATE NONCLUSTERED INDEX ix_DestinationNodeIdS ON #System ([DestinationNodeId]);
				CREATE NONCLUSTERED INDEX ix_SourceNodeIdS ON #System ([SourceNodeId]);

		     SELECT  [MovementId]
                    ,[MovementTransactionId]
                    ,[OperationalDate]
                    ,[Operacion]
                    ,[SourceNode]
                    ,[DestinationNode]
                    ,[SourceProduct]
                    ,[DestinationProduct]
                    ,[NetStandardVolume]
                    ,[GrossStandardVolume]
                    ,[MeasurementUnit]
                    ,[EventType]
                    ,[BatchId]
                    ,[SystemName]
                    ,ISNULL([Movement],'') AS [Movement]
                    ,[PercentStandardUnCertainty]
                    ,[Uncertainty]
                    ,[BackupMovementId]
                    ,[GlobalMovementId]
                    ,[SourceProductId]
                    ,[Category]
                    ,[Element]
                    ,[NodeName]
                    ,[OperationalDate] AS [CalculationDate]
					,[TicketId]
					,'ReportUser' as [CreatedBy]
					,@TodaysDate as [CreatedDate]
					,NULL as [LastModifiedBy]
					,NULL as [LastModifiedDate]
					INTO #Source
			 FROM (
		     SELECT 
				   [MovementId]																
			      ,[MovementTransactionId]
				  ,[OperationalDate]						
			      ,[Operacion]								
			      ,[SourceNode]							
			      ,[DestinationNode]						
			      ,[SourceProduct]							
			      ,[DestinationProduct]					
			      ,[NetStandardVolume]						
			      ,[GrossStandardVolume]					
			      ,[MeasurementUnit]
				  ,[EventType]                                                               
				  ,[BatchId]
				  ,[SystemName]	
			      ,CASE 
		               WHEN [Classification] <> 'PerdidaIdentificada' AND MessageTypeId = 1
		               	 AND [Operacion] NOT IN ('Traslado de productos','Tolerancia','Pérdida no identificada') 
		               	 AND DestinationNodeId = ND.NodeId
		               		THEN  'Entradas'
		               WHEN [Classification] <> 'PerdidaIdentificada' AND MessageTypeId = 1
		               	 AND [Operacion] NOT IN ('Traslado de productos','Tolerancia','Pérdida no identificada') 
		               	 AND SourceNodeId = ND.NodeId
		               		THEN  'Salidas'
					ELSE [Movements]
					END AS [Movement]
				   ,[PercentStandardUnCertainty]
			      ,[Uncertainty]
				   ,[BackupMovementId]
			      ,[GlobalMovementId]
			      ,[SourceProductId]
			      ,[Category]
			      ,[Element]                
			      ,CONCAT('-_-', ND.[Name], '-_-')   AS NodeName
				  ,[TicketId]
            FROM #Segement Mov
			INNER JOIN Admin.[Node] ND
			 ON (ND.NodeId = Mov.SourceNodeId 
			 OR ND.NodeId = Mov.DestinationNodeId)

			UNION

			SELECT 
				   [MovementId]																
			      ,[MovementTransactionId]
				  ,[OperationalDate]						
			      ,[Operacion]								
			      ,[SourceNode]							
			      ,[DestinationNode]						
			      ,[SourceProduct]							
			      ,[DestinationProduct]					
			      ,[NetStandardVolume]						
			      ,[GrossStandardVolume]					
			      ,[MeasurementUnit]
				  ,[EventType]                                                               
				  ,[BatchId]
				  ,[SystemName]	
			      ,[Movements]AS [Movement]
				  ,[PercentStandardUnCertainty]
			      ,[Uncertainty]
				  ,[BackupMovementId]
			      ,[GlobalMovementId]
			      ,[SourceProductId]
			      ,[Category]
			      ,[Element]                
			      ,CONCAT('-_-','Todos', '-_-')  AS NodeName
				  ,[TicketId]
            FROM #Segement

			UNION

			SELECT 
				   [MovementId]																
			      ,[MovementTransactionId]
				  ,[OperationalDate]						
			      ,[Operacion]								
			      ,[SourceNode]							
			      ,[DestinationNode]						
			      ,[SourceProduct]							
			      ,[DestinationProduct]					
			      ,[NetStandardVolume]						
			      ,[GrossStandardVolume]					
			      ,[MeasurementUnit]
				  ,[EventType]                                                               
				  ,[BatchId]
				  ,[SystemName]	
			      ,CASE 
		               WHEN [Classification] <> 'PerdidaIdentificada' AND MessageTypeId = 1
		               	 AND [Operacion] NOT IN ('Traslado de productos','Tolerancia','Pérdida no identificada') 
		               	 AND DestinationNodeId = ND.NodeId
		               		THEN  'Entradas'
		               WHEN [Classification] <> 'PerdidaIdentificada' AND MessageTypeId = 1
		               	 AND [Operacion] NOT IN ('Traslado de productos','Tolerancia','Pérdida no identificada') 
		               	 AND SourceNodeId = ND.NodeId
		               		THEN  'Salidas'
					ELSE [Movements]
					END AS [Movement]
				   ,[PercentStandardUnCertainty]
			      ,[Uncertainty]
				  ,[BackupMovementId]
			      ,[GlobalMovementId]
			      ,[SourceProductId]
			      ,[Category]
			      ,[Element]                
			      ,CONCAT('-_-', ND.[Name], '-_-')   AS NodeName
				  ,[TicketId]
            FROM #System Mov
			INNER JOIN Admin.[Node] ND
			 ON (ND.NodeId = Mov.SourceNodeId 
			 OR ND.NodeId = Mov.DestinationNodeId)

			UNION

			SELECT 
				   [MovementId]																
			      ,[MovementTransactionId]
				  ,[OperationalDate]						
			      ,[Operacion]								
			      ,[SourceNode]							
			      ,[DestinationNode]						
			      ,[SourceProduct]							
			      ,[DestinationProduct]					
			      ,[NetStandardVolume]						
			      ,[GrossStandardVolume]					
			      ,[MeasurementUnit]
				  ,[EventType]                                                               
				  ,[BatchId]
				  ,[SystemName]	
			      ,[Movements] AS [Movement]
				  ,[PercentStandardUnCertainty]
			      ,[Uncertainty]
				  ,[BackupMovementId]
			      ,[GlobalMovementId]
			      ,[SourceProductId]
			      ,[Category]
			      ,[Element]                
			      ,CONCAT('-_-','Todos', '-_-')   AS NodeName
				  ,[TicketId]
            FROM #System
			)SubQ

			MERGE [Admin].[MovementDetailsWithoutOwner] AS TARGET 
				USING #Source AS SOURCE  
				ON  ISNULL(TARGET.[MovementTransactionId],'') = ISNULL(SOURCE.[MovementTransactionId],'')
				AND ISNULL(TARGET.[MovementId]           ,'') = ISNULL(SOURCE.[MovementId]           ,'')
				AND ISNULL(TARGET.[OperationalDate]      ,'') = ISNULL(SOURCE.[OperationalDate]      ,'')
				AND ISNULL(TARGET.[Operacion]            ,'') = ISNULL(SOURCE.[Operacion]            ,'')
				AND ISNULL(TARGET.[SourceNode]           ,'') = ISNULL(SOURCE.[SourceNode]           ,'')
				AND ISNULL(TARGET.[DestinationNode]      ,'') = ISNULL(SOURCE.[DestinationNode]      ,'')
				AND ISNULL(TARGET.[SourceProduct]        ,'') = ISNULL(SOURCE.[SourceProduct]        ,'')
				AND ISNULL(TARGET.[DestinationProduct]   ,'') = ISNULL(SOURCE.[DestinationProduct]   ,'')
				AND ISNULL(TARGET.[MeasurementUnit]      ,'') = ISNULL(SOURCE.[MeasurementUnit]      ,'')
				AND ISNULL(TARGET.[Category]             ,'') = ISNULL(SOURCE.[Category]             ,'')
				AND ISNULL(TARGET.[Element]              ,'') = ISNULL(SOURCE.[Element]              ,'')
				AND ISNULL(TARGET.[NodeName]             ,'') = ISNULL(SOURCE.[NodeName]             ,'')
				AND ISNULL(TARGET.[TicketId]             ,'') = ISNULL(SOURCE.[TicketId]             ,'')
				
				WHEN MATCHED  AND ( 
				                    TARGET.[NetStandardVolume]             <> SOURCE.[NetStandardVolume] OR
					                TARGET.[GrossStandardVolume]           <> SOURCE.[GrossStandardVolume] OR
					                TARGET.[SystemName]                    <> SOURCE.[SystemName] OR
					                TARGET.[Movement]                      <> SOURCE.[Movement] OR
					                TARGET.[PercentStandardUnCertainty]    <> SOURCE.PercentStandardUnCertainty OR
					                TARGET.[EventType]                     <> SOURCE.[EventType] OR
					                TARGET.[Uncertainty]                   <> SOURCE.[Uncertainty] OR		
					                TARGET.[BackupMovementId]              <> SOURCE.[BackupMovementId] OR	
					                TARGET.[GlobalMovementId]              <> SOURCE.[GlobalMovementId] OR		
					                TARGET.[SourceProductId]               <> SOURCE.[SourceProductId] 
									)


				THEN UPDATE 
					  SET TARGET.[NetStandardVolume]             = SOURCE.[NetStandardVolume]
					     ,TARGET.[GrossStandardVolume]           = SOURCE.[GrossStandardVolume]
					     ,TARGET.[SystemName]                    = SOURCE.[SystemName]
					     ,TARGET.[Movement]                      = SOURCE.[Movement] 
					     ,TARGET.[PercentStandardUnCertainty]    = SOURCE.PercentStandardUnCertainty
					     ,TARGET.[EventType]                     = SOURCE.[EventType]
					     ,TARGET.[Uncertainty]                   = SOURCE.[Uncertainty] 		
					     ,TARGET.[BackupMovementId]              = SOURCE.[BackupMovementId] 	
					     ,TARGET.[GlobalMovementId]              = SOURCE.[GlobalMovementId] 		
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
                             ,[OperationalDATE]
                             ,[Operacion]
                             ,[SourceNode]
                             ,[DestinationNode]
                             ,[SourceProduct]
                             ,[DestinationProduct]
                             ,[NetStandardVolume]
                             ,[GrossStandardVolume]
                             ,[MeasurementUnit]
                             ,[EventType]
                             ,[BatchId]
                             ,[SystemName]
                             ,[Movement]
                             ,[PercentStandardUnCertainty]
                             ,[Uncertainty]
                             ,[BackupMovementId]
                             ,[GlobalMovementId]
                             ,[SourceProductId]
                             ,[Category]
                             ,[Element]
                             ,[NodeName]
                             ,[CalculationDATE]
							 ,[TicketId]
                             ,[CreatedBy]
                             ,[CreatedDate]
                             ,[LastModifiedBy]
                             ,[LastModifiedDate]
							)

			 VALUES (
			          SOURCE.[MovementId]
                     ,SOURCE.[MovementTransactionId]
                     ,SOURCE.[OperationalDATE]
                     ,SOURCE.[Operacion]
                     ,SOURCE.[SourceNode]
                     ,SOURCE.[DestinationNode]
                     ,SOURCE.[SourceProduct]
                     ,SOURCE.[DestinationProduct]
                     ,SOURCE.[NetStandardVolume]
                     ,SOURCE.[GrossStandardVolume]
                     ,SOURCE.[MeasurementUnit]
                     ,SOURCE.[EventType]
                     ,SOURCE.[BatchId]
                     ,SOURCE.[SystemName]
                     ,SOURCE.[Movement]
                     ,SOURCE.[PercentStandardUnCertainty]
                     ,SOURCE.[Uncertainty]
                     ,SOURCE.[BackupMovementId]
                     ,SOURCE.[GlobalMovementId]
                     ,SOURCE.[SourceProductId]
                     ,SOURCE.[Category]
                     ,SOURCE.[Element]
                     ,SOURCE.[NodeName]
                     ,SOURCE.[CalculationDATE]
					 ,SOURCE.[TicketId]
                     ,SOURCE.[CreatedBy]
                     ,SOURCE.[CreatedDate]
                     ,SOURCE.[LastModifiedBy]
                     ,SOURCE.[LastModifiedDate]
					);
END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PBI 32275. This procedure is to validate the Nodes send to approval',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveMovementDetails',
    @level2type = NULL,
    @level2name = NULL