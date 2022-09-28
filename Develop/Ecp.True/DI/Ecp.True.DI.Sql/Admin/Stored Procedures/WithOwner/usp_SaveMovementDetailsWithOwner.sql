/*-- =================================================================================================
-- Author:		Microsoft
-- Created date: Jul-24-2020
-- Updated date: Aug-12-2020 -- Removed unnecessary join
-- Updated date: Nov-18-2020 -- Changed logic to use ElementId of Ticket
-- Updated date: May-20-2021  add NodeId Validation
-- <Description>:	This procedure is to Fetch MovementDetailsWithOwner Data For PowerBi Report From
				Tables(Movement, Product, Ticket, Node, NodeTag, CategoryElement, Category, VariableType)</Description>
   EXEC [Admin].[usp_SaveMovementDetailsWithOwner] @OwnershipTicketId = -1(full load)
   EXEC [Admin].[usp_SaveMovementDetailsWithOwner] @OwnershipTicketId = 31352(ticketid load)
   SELECT * FROM Admin.MovementDetailsWithOwner
-- ===================================================================================================*/

CREATE PROCEDURE [Admin].[usp_SaveMovementDetailsWithOwner]
(
	@OwnershipTicketId INT,
	@NodeId INT = NULL
)
AS
BEGIN
  SET NOCOUNT ON

IF OBJECT_ID('tempdb..#segment')IS NOT NULL
			DROP TABLE #segment
IF OBJECT_ID('tempdb..#system')IS NOT NULL
			DROP TABLE #system
IF OBJECT_ID('tempdb..#Source')IS NOT NULL
			DROP TABLE #Source
IF OBJECT_ID('tempdb..#MovementConciDeleted')IS NOT NULL
			DROP TABLE #MovementConciDeleted


CREATE TABLE #MovementConciDeleted
(movementid VARCHAR(50)  COLLATE SQL_Latin1_General_CP1_CS_AS,
)


DROP INDEX IF EXISTS ix_Movementid ON #MovementConciDeleted;
CREATE NONCLUSTERED INDEX ix_Movementid ON #MovementConciDeleted ([movementid]);

INSERT INTO #MovementConciDeleted 
SELECT DISTINCT m2.movementid 
FROM Admin.view_MovementInformation [Mov]
INNER JOIN offchain.movement m2 on [Mov].movementid = m2.movementid and m2.isdeleted =1
INNER JOIN [offchain].[Ownership] [Ownership]
				ON [Mov].[MovementTransactionId] = [Ownership].[MovementTransactionId]
				AND [Mov].[OwnershipTicketId] = [Ownership].[TicketId]
WHERE mov.MovementTypeId in (228,229,230,231)  and ([Ownership].[TicketId]= @OwnershipTicketId OR @OwnershipTicketId =-1)
				AND (@NodeId IS  NULL OR  ([Mov].DestinationNodeId = @NodeId OR [Mov].SourceNodeId = @NodeId))


DELETE FROM Admin.MovementDetailsWithOwner  
WHERE movementid  IN (SELECT movementid FROM #MovementConciDeleted)


DECLARE @Todaysdate	 DATETIME =  [Admin].[udf_GetTrueDate] ()

SELECT       -- DISTINCT          
						   Mov.MovementId																									
                          ,Mov.MovementTransactionId
						  ,Mov.OperationalDate															AS [OperationalDate]				
                          ,Mov.MovementTypeName  														AS [Operacion]											
                          ,Mov.SourceNodeName															AS [SourceNode]											
                          ,Mov.DestinationNodeName														AS DestinationNode										
                          ,Mov.SourceProductName														AS [SourceProduct]										
                          ,Mov.DestinationProductName													AS [DestinationProduct]									
                          ,Mov.NetStandardVolume 														AS [NetStandardVolume]						            
                          ,Mov.GrossStandardVolume														AS [GrossStandardVolume]					            
                          ,CEUnits.Name																	AS [MeasurementUnit]									
						  ,Mov.SourceSystem																AS [SourceSystem]
						  ,CASE 
								WHEN [Mov].SystemName = 'FICO' AND Mov.MovementTypeName IN ('Compra','Venta') 
										THEN Mov.SourceMovementId
								WHEN Mov.MovementTypeName IN ('ACE Entrada','ACE Salida')
										THEN Mov.SourceMovementId							
								ELSE NULL
							END										AS [SourceMovementId]							
						  ,CASE 
								WHEN [Mov].SystemName IN ('FICO','TRUE') AND Mov.MovementTypeName IN ('Compra','Venta') 
										THEN [Cont].DocumentNumber 
								ELSE NULL
							END										AS [Order]										
						  ,CASE 
								WHEN [Mov].SystemName IN ('FICO','TRUE') AND Mov.MovementTypeName IN ('Compra','Venta') 
										THEN [Cont].Position 
								WHEN [Mov].SystemName = 'TRUE' AND Mov.MovementTypeName IN ('Compra','Venta')
										THEN [Cont].Position
								ELSE NULL
							END										AS [Position]									

						  ,Mov.SystemName                                                               AS [SystemName]
						  ,Mov.MovementTypeName															AS [CETypeName]
						  ,Mov.EventType															    AS [EventType]                              
						  ,[Cont].DocumentNumber														AS [Contract_DocumentNumber]
						  ,[Cont].Position                                                              AS [Contract_Position]
                          ,ElementOwner.[Name]															AS [OwnerName]								
                          ,[Ownership].OwnershipVolume												    AS [OwnershipVolume]					    	  
                          ,[Ownership].ExecutionDate													AS [OwnershipProcessDate]					
                          ,[Ownership].AppliedRule													    AS [Rule]	
						  ,Vt.[VariableTypeId]															AS [VariableTypeId]
						  ,[Mov].MessageTypeId															AS [MessageTypeId]
						  ,Mov.[Classification]															AS [Classification]							
						  ,Mov.DestinationNodeId
						  ,Mov.SourceNodeId

						   ,CASE 
		                    WHEN VT.[VariableTypeId] = 1 AND Mov.MessageTypeId NOT IN (1,2)
		                     AND Mov.MovementTypeName = 'Traslado de productos'
		                    THEN 'Interfases'
		                    WHEN VT.[VariableTypeId] = 2 AND Mov.MessageTypeId NOT IN (1,2) 
		                     AND Mov.MovementTypeName = 'Tolerancia' 
		                    THEN 'Tolerancia'
		                    WHEN VT.[VariableTypeId] = 3 AND Mov.MessageTypeId NOT IN (1,2) 
		                    	 AND Mov.MovementTypeName = 'Pérdida no identificada' 
		                    		THEN 'Pérdidas No Identificadas'
		                    WHEN [Classification] = 'PerdidaIdentificada' AND Mov.MessageTypeId = 2 
		                    		THEN 'Pérdidas Identificadas'
                         END AS [Movements]

                          ,Mov.UncertaintyPercentage   AS [UncertaintyPercentage]			                            
                          ,ISNULL(Mov.NetStandardVolume,1)*ISNULL(Mov.UncertaintyPercentage,1)  AS [Uncertainty]		
						  ,Mov.[BackupMovementId]
                          ,Mov.[GlobalMovementId]
                          ,[Prod].[ProductId] AS [SourceProductId]
                          ,[Ownership].OwnershipPercentage  AS [OwnershipPercentage]
                          ,Cat.[Name] AS [Category]
                          ,Element.[Name] AS [Element]
                          ,Mov.BatchId
						  ,Element.ElementID
						  ,Element.CategoryId
						  ,[Ownership].[TicketId] AS [OwnershipTicketId]
						  ,St.[Name]															AS [ScenarioName]
				INTO #Segment
				FROM Admin.view_MovementInformation [Mov]
				INNER JOIN [Admin].[Product] Prod
				ON (Prod.ProductId = Mov.SourceProductId
				OR Prod.ProductId = Mov.DestinationProductId)
				INNER JOIN [offchain].[Ownership] [Ownership]
				ON [Mov].[MovementTransactionId] = [Ownership].[MovementTransactionId]
				AND [Mov].[OwnershipTicketId] = [Ownership].[TicketId]
				INNER JOIN [Admin].[Ticket] T
				ON T.TicketId = Mov.[OwnershipTicketId]
				INNER JOIN [Admin].[CategoryElement] [Element] 
				ON [T].[CategoryElementId] = [Element].[ElementId]
				INNER JOIN [Admin].[Category]  [Cat]
				ON [Element].[CategoryId] = [Cat].[CategoryId]
				INNER JOIN [Admin].[CategoryElement] [ElementOwner] 
				ON [Ownership].[OwnerId] = [ElementOwner].[ElementId]
				AND [ElementOwner].[CategoryID] = 7 -- 'Propietario' From name Column in Category
				INNER JOIN [Admin].[CategoryElement] CEUnits
				ON CEUnits.ElementId = Mov.MeasurementUnit 
				AND CEUnits.CategoryID = 6 --'Unidad de Medida' From name Column in Category
				LEFT JOIN [Admin].[VariableType] Vt
				ON Vt.VariableTypeId = Mov.VariableTypeId
				LEFT JOIN [Admin].[MovementContract] [Cont]
				ON [Mov].ContractId = [Cont].[MovementContractId]
				AND	[Cont].IsDeleted = 0
				LEFT JOIN [Admin].ScenarioType St
				ON St.ScenarioTypeId = Mov.scenarioId
				WHERE [Ownership].IsDeleted = 0 and mov.movementid not in (select movementid from #MovementConciDeleted)
				AND ([Ownership].[TicketId]= @OwnershipTicketId OR @OwnershipTicketId =-1)
				AND (@NodeId IS  NULL OR  ([Mov].DestinationNodeId = @NodeId OR [Mov].SourceNodeId = @NodeId))

				DROP INDEX IF EXISTS ix_ElementId ON #Segment;
				DROP INDEX IF EXISTS ix_SourceNodeId ON #Segment;
				DROP INDEX IF EXISTS ix_SourceNodeId ON #Segment;
				DROP INDEX IF EXISTS ix_CategoryId ON #Segment;

				CREATE NONCLUSTERED INDEX ix_ElementId ON #Segment ([ElementId]);
				CREATE NONCLUSTERED INDEX ix_SourceNodeId ON #Segment ([SourceNodeId]);
				CREATE NONCLUSTERED INDEX ix_DestinationNodeId ON #Segment ([DestinationNodeId]);
				CREATE NONCLUSTERED INDEX ix_CategoryId ON #Segment ([CategoryId]);

				 SELECT * INTO #System  
				 FROM
				(SELECT DISTINCT
				
				 SQ.MovementId																			
				,SQ.MovementTransactionId
				,SQ.OperationalDate														AS [OperationalDate]					
				,SQ.Operacion														
				,SQ.SourceNode															AS [SourceNode]								
				,SQ.DestinationNode														AS DestinationNode									
				,SQ.SourceProduct														AS [SourceProduct]							
				,SQ.DestinationProduct													AS [DestinationProduct]						
				,SQ.NetStandardVolume 													AS [NetStandardVolume]					
				,SQ.GrossStandardVolume													AS [GrossStandardVolume]				
				,SQ.MeasurementUnit														AS [MeasurementUnit]			
				,SQ.[Order]
				,SQ.[Position]
				,SQ.EventType															AS [EventType]  
				,SQ.SourceSystem														AS [SourceSystem]
				,SQ.SystemName                                                          AS [SystemName]
				,SQ.[CETypeName]														AS [CETypeName]
				,SQ.SourceMovementId													AS [SourceMovementId]               
				,SQ.Contract_DocumentNumber												AS [Contract_DocumentNumber]
				,SQ.Contract_Position                                                   AS [Contract_Position]
				,SQ.[OwnerName]															AS [OwnerName]								
				,SQ.OwnershipVolume														AS [OwnershipVolume]					        
				,SQ.[OwnershipProcessDate]												AS [OwnershipProcessDate]				
				,SQ.[Rule]																AS [Rule]	
				,SQ.[VariableTypeId]												    AS [VariableTypeId]
				,SQ.MessageTypeId														AS [MessageTypeId]
				,SQ.[Classification]													AS [Classification]
				,SQ.DestinationNodeId
				,SQ.SourceNodeId
				,SQ.Movements
				, SQ.[UncertaintyPercentage]			                            -- % Incertidumbre Estándar
				,SQ.[Uncertainty]		-- Incertidumbre
				,SQ.[BackupMovementId]
				,SQ.[GlobalMovementId]
				,SQ.[SourceProductId]
				,SQ.OwnershipPercentage													AS [OwnershipPercentage]
				,Cat1.[Name]															AS [Category]
				,CatEle.[Name]														    AS [Element]
				,SQ.BatchId
				,SQ.[OwnershipTicketId]
				,SQ.[ScenarioName]
				FROM #Segment SQ
		 INNER JOIN [Admin].NodeTag NT            
         ON NT.ElementId = SQ.ElementId  and (SQ.SourceNodeId = NT.NodeId or SQ.DestinationNodeId = NT.NodeId)           
         INNER JOIN [Admin].Category  Cat            
         ON SQ.CategoryId = Cat.CategoryId            
         INNER JOIN [Admin].NodeTag NT1            
         ON NT.NodeId = NT1.NodeId            
         INNER JOIN [Admin].CategoryElement CatEle            
         ON CatEle.ElementId = NT1.ElementId AND CatEle.CategoryId = 8            
         INNER JOIN [Admin].Category  Cat1            
         ON CatEle.CategoryId = Cat1.CategoryId )P   
		 
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
                    ,[SystemName]
					,[SourceMovementId]
					,[Order]
					,[Position]
					,[OwnerName]
					,[OwnershipVolume]
					,[OwnershipProcessDate]
					,[Rule]
                    ,ISNULL([Movement],'') AS [Movement]
                    ,[% Standard Uncertainty]
                    ,[Uncertainty]
                    ,[BackupMovementId]
                    ,[GlobalMovementId]
                    ,[SourceProductId]
					,[OwnershipPercentage]
                    ,[Category]
                    ,[Element]
					,[BatchId]
                    ,[NodeName]
                    ,[OperationalDate] AS [CalculationDate]
					,[OwnershipTicketId]
					,ScenarioName
					,'ReportUser' AS [CreatedBy]
					,@TodaysDate  AS [CreatedDate]
					,NULL         AS [LastModifiedBy]
					,NULL         AS [LastModifiedDate]

					INTO #Source
			 FROM (
		     SELECT
						   MovementId									
                          ,MovementTransactionId
						  ,[OperationalDate]							
                          ,[Operacion]									
                          ,[SourceNode]									
                          ,DestinationNode								
                          ,[SourceProduct]								
                          ,[DestinationProduct]							
                          ,[NetStandardVolume]						    
                          ,[GrossStandardVolume]					    
                          ,[MeasurementUnit]							
						  ,[EventType]
						  ,SourceSystem AS [SystemName]                 
						  , [SourceMovementId]							
						  , Mov.[Order]									
						  , [Position]									
                          , [OwnerName]									
                          ,[OwnershipVolume]					        
                          , [OwnershipProcessDate]						
                          , [Rule]										
                                   ,CASE 
		                        WHEN [Classification] <> 'PerdidaIdentificada' AND MessageTypeId = 1
		                        	 AND CETypeName NOT IN ('Traslado de productos','Tolerancia','Pérdida no identificada') 
		                        	 AND DestinationNodeId = ND.NodeId
		                        		THEN  'Entradas'
		                        WHEN [Classification] <> 'PerdidaIdentificada' AND MessageTypeId = 1
		                        	 AND CETypeName NOT IN ('Traslado de productos','Tolerancia','Pérdida no identificada') 
		                        	 AND SourceNodeId = ND.NodeId
		                        		THEN  'Salidas'
				   	 		ELSE [Movements]
				   	 		END AS [Movement]								    -- Movimiento
                          ,UncertaintyPercentage   AS [% Standard Uncertainty]			                            -- % Incertidumbre Estándar
                          ,[Uncertainty]		-- Incertidumbre
						  ,[BackupMovementId]
                          ,[GlobalMovementId]
                          ,[SourceProductId] AS [SourceProductId]
                          ,OwnershipPercentage  AS [OwnershipPercentage]
                          ,[Category] AS [Category]
                          ,[Element] AS [Element]
                          ,BatchId
						  ,CONCAT('-_-', ND.[Name], '-_-')   AS NodeName
						  ,[OwnershipTicketId]
						  ,mov.ScenarioName
                     FROM #Segment Mov
					 INNER JOIN Admin.[Node] ND
				 	  ON (ND.NodeId = Mov.SourceNodeId 
				 	  OR ND.NodeId = Mov.DestinationNodeId)

				UNION

				SELECT					  
						   MovementId									
                          ,MovementTransactionId
						  ,[OperationalDate]							
                          ,[Operacion]									
                          ,[SourceNode]									
                          ,DestinationNode								
                          ,[SourceProduct]								
                          ,[DestinationProduct]							
                          ,[NetStandardVolume]						    
                          ,[GrossStandardVolume]					    
                          ,[MeasurementUnit]							
						  ,[EventType]
						  ,SourceSystem AS [SystemName]                 
						  , [SourceMovementId]							
						  , Mov.[Order]									
						  , [Position]									
                          , [OwnerName]									
                          ,[OwnershipVolume]					        
                          , [OwnershipProcessDate]						
                          , [Rule]										
                          ,CASE 
		                   WHEN [Classification] <> 'PerdidaIdentificada' AND MessageTypeId = 1
		                        AND CETypeName NOT IN ('Traslado de productos','Tolerancia','Pérdida no identificada')
		                        	THEN  ''
				   	 		ELSE [Movements]
				   	 		END AS [Movement]								    -- Movimiento
                          ,UncertaintyPercentage   AS [% Standard Uncertainty]			                            -- % Incertidumbre Estándar
                          ,[Uncertainty]		-- Incertidumbre
						  ,[BackupMovementId]
                          ,[GlobalMovementId]
                          ,[SourceProductId] AS [SourceProductId]
                          ,OwnershipPercentage  AS [OwnershipPercentage]
                          ,[Category] AS [Category]
                          ,[Element] AS [Element]
                          ,BatchId
						  ,CONCAT('-_-', 'Todos', '-_-')   AS NodeName
						  ,[OwnershipTicketId]
						  ,ScenarioName
                     FROM #Segment Mov
			
			UNION
		   SELECT		
							MovementId									
                          ,MovementTransactionId
						  ,[OperationalDate]							
                          ,[Operacion]									
                          ,[SourceNode]									
                          ,DestinationNode								
                          ,[SourceProduct]								
                          ,[DestinationProduct]							
                          ,[NetStandardVolume]						    
                          ,[GrossStandardVolume]					    
                          ,[MeasurementUnit]							
						  ,[EventType]
						  ,SourceSystem AS [SystemName]                 
						  , [SourceMovementId]							
						  , Mov.[Order]									
						  , [Position]									
                          , [OwnerName]									
                          ,[OwnershipVolume]					        
                          , [OwnershipProcessDate]						
                          , [Rule]										
                                   ,CASE 
		                        WHEN [Classification] <> 'PerdidaIdentificada' AND MessageTypeId = 1
		                        	 AND CETypeName NOT IN ('Traslado de productos','Tolerancia','Pérdida no identificada') 
		                        	 AND DestinationNodeId = ND.NodeId
		                        		THEN  'Entradas'
		                        WHEN [Classification] <> 'PerdidaIdentificada' AND MessageTypeId = 1
		                        	 AND CETypeName NOT IN ('Traslado de productos','Tolerancia','Pérdida no identificada') 
		                        	 AND SourceNodeId = ND.NodeId
		                        		THEN  'Salidas'
				   	 		ELSE [Movements]
				   	 		END AS [Movement]								    -- Movimiento
                          ,UncertaintyPercentage   AS [% Standard Uncertainty]			                            -- % Incertidumbre Estándar
                          ,[Uncertainty]		-- Incertidumbre
						  ,[BackupMovementId]
                          ,[GlobalMovementId]
                          ,[SourceProductId] AS [SourceProductId]
                          ,OwnershipPercentage  AS [OwnershipPercentage]
                          ,[Category] AS [Category]
                          ,[Element] AS [Element]
                          ,BatchId
						  ,CONCAT('-_-', ND.[Name], '-_-')   AS NodeName
						  ,[OwnershipTicketId]
						  ,Mov.ScenarioName
                     FROM #System Mov
					 INNER JOIN Admin.[Node] ND
				 	  ON (ND.NodeId = Mov.DestinationNodeId)
					 INNER JOIN Admin.[Node] NS
				 	  ON (NS.NodeId = Mov.SourceNodeId)

				

				UNION

				SELECT					   
						   MovementId								
                          ,MovementTransactionId
						  ,[OperationalDate]						
                          ,[Operacion]								
                          ,[SourceNode]								
                          ,DestinationNode							
                          ,[SourceProduct]							
                          ,[DestinationProduct]						
                          ,[NetStandardVolume]						
                          ,[GrossStandardVolume]					
                          ,[MeasurementUnit]						
						  ,[EventType]
						  ,SourceSystem AS [SystemName]             
						  , [SourceMovementId]						
						  , Mov.[Order]								
						  , [Position]								
                          , [OwnerName]								
                          ,[OwnershipVolume]					    
                          , [OwnershipProcessDate]					
                          , [Rule]									
                          ,CASE 
		                   WHEN [Classification] <> 'PerdidaIdentificada' AND MessageTypeId = 1
		                        AND CETypeName NOT IN ('Traslado de productos','Tolerancia','Pérdida no identificada')
		                        	THEN  ''
				   	 		ELSE [Movements]
				   	 		END AS [Movement]								    -- Movimiento
                          ,UncertaintyPercentage   AS [% Standard Uncertainty]			                            -- % Incertidumbre Estándar
                          ,[Uncertainty]		-- Incertidumbre
						  ,[BackupMovementId]
                          ,[GlobalMovementId]
                          ,[SourceProductId] AS [SourceProductId]
                          ,OwnershipPercentage  AS [OwnershipPercentage]
                          ,[Category] AS [Category]
                          ,[Element] AS [Element]
                          ,BatchId
						  ,CONCAT('-_-', 'Todos', '-_-')   AS NodeName
						  ,[OwnershipTicketId]
						  ,ScenarioName
                     FROM #System Mov
			)SubQ


			
				MERGE Admin.MovementDetailsWithOwner AS TARGET 
				USING #Source AS SOURCE 
				ON  ISNULL(TARGET.[MovementTransactionId]	,'')=	ISNULL(SOURCE.[MovementTransactionId]	,'')
				AND ISNULL(TARGET.MovementId				,'')=	ISNULL(SOURCE.MovementId				,'')
				AND ISNULL(TARGET.[OperationalDate]			,'')=	ISNULL(SOURCE.[OperationalDate]			,'')
				AND ISNULL(TARGET.[SourceNode]				,'')=	ISNULL(SOURCE.[SourceNode]				,'')
				AND ISNULL(TARGET.[DestinationNode]			,'')=	ISNULL(SOURCE.[DestinationNode]			,'')
				AND ISNULL(TARGET.[SourceProduct]			,'')=	ISNULL(SOURCE.[SourceProduct]			,'')
				AND ISNULL(TARGET.[DestinationProduct]		,'')=	ISNULL(SOURCE.[DestinationProduct]		,'')
				AND ISNULL(TARGET.[MeasurementUnit]			,'')=	ISNULL(SOURCE.[MeasurementUnit]			,'')
				AND ISNULL(TARGET.[SourceProduct]			,'')=	ISNULL(SOURCE.[SourceProduct]			,'')
				AND ISNULL(TARGET.[OwnerName]				,'')=	ISNULL(SOURCE.[OwnerName]				,'')
				AND ISNULL(TARGET.[Operacion]				,'')=	ISNULL(SOURCE.[Operacion]				,'')
				AND ISNULL(TARGET.[Movement]				,'')=	ISNULL(SOURCE.[Movement]				,'')
				AND ISNULL(TARGET.[Category]				,'')=	ISNULL(SOURCE.[Category]				,'')
				AND ISNULL(TARGET.[Element]					,'')=	ISNULL(SOURCE.[Element]					,'')
				AND ISNULL(TARGET.[NodeName]				,'')=	ISNULL(SOURCE.[NodeName]				,'')
				AND ISNULL(TARGET.[SourceProductId]         ,'')=   ISNULL(SOURCE.[SourceProductId]         ,'')
				AND ISNULL(TARGET.[OwnershipTicketId]       ,'')=   ISNULL(SOURCE.[OwnershipTicketId]       ,'')
				AND ISNULL(TARGET.[scenarioname]       ,'')=   ISNULL(SOURCE.[scenarioname]       ,'')


				WHEN MATCHED  AND ( TARGET.[NetStandardVolume] <> SOURCE.[NetStandardVolume]		OR
					  TARGET.[GrossStandardVolume]			   <> SOURCE.[GrossStandardVolume]		OR
					  TARGET.[SystemName]					   <> SOURCE.[SystemName]				OR
					  TARGET.[OwnershipVolume]				   <> SOURCE.[OwnershipVolume]			OR
					  TARGET.[OwnershipPercentage]             <> SOURCE.[OwnershipPercentage]		OR
					  TARGET.[% Standard Uncertainty]		   <> SOURCE.[% Standard Uncertainty]   OR
					  TARGET.[Uncertainty]					   <> SOURCE.[Uncertainty]				OR		
					  TARGET.[BackupMovementId]				   <> SOURCE.[BackupMovementId]			OR	
					  TARGET.[GlobalMovementId]				   <> SOURCE.[GlobalMovementId]			OR
					  TARGET.[Order]                           <> SOURCE.[Order]					OR
					  TARGET.[Position]                        <> SOURCE.[Position]					OR
					  TARGET.[EventType]                       <> SOURCE.[EventType]				OR
					  TARGET.[SourceMovementId]                <>SOURCE.[SourceMovementId]			OR
					  TARGET.[Rule]							   <>SOURCE.[Rule]						OR
					  TARGET.[BatchId]						   <>SOURCE.[BatchId]
					  )


				THEN UPDATE 
				  SET TARGET.[NetStandardVolume]	   =  SOURCE.[NetStandardVolume],
					  TARGET.[GrossStandardVolume]	   =  SOURCE.[GrossStandardVolume],
					  TARGET.[SystemName]			   =  SOURCE.[SystemName],
					  TARGET.[OwnershipVolume]		   =  SOURCE.[OwnershipVolume],
					  TARGET.[OwnershipPercentage]	   =  SOURCE.[OwnershipPercentage],
					  TARGET.[% Standard Uncertainty]  =  SOURCE.[% Standard Uncertainty],
					  TARGET.[Uncertainty]             =  SOURCE.[Uncertainty],			
					  TARGET.[BackupMovementId]		   =  SOURCE.[BackupMovementId],		
					  TARGET.[GlobalMovementId]		   =  SOURCE.[GlobalMovementId],
					  TARGET.[Order]                   =  SOURCE.[Order],
					  TARGET.[Position]                =  SOURCE.[Position],
					  TARGET.[EventType]               =  SOURCE.[EventType] ,
					  TARGET.[SourceMovementId]        =  SOURCE.[SourceMovementId] ,
					  TARGET.[Rule]                    =  SOURCE.[Rule] ,
					  TARGET.[BatchId]                 =  SOURCE.[BatchId],
					  TARGET.LastModifiedBy            =  'ReportUser',
					  TARGET.[LastModifiedDate]		   =  @TodaysDate
					  
       
				/* 2. Performing the INSERT operation */ 
  
				/* When no records are matched with TARGET table 
				   Then insert the records in the target table */ 
				WHEN NOT MATCHED BY TARGET  
				THEN INSERT ([MovementId]  
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
							 ,[SystemName]
							 ,[SourceMovementId]
							 ,[Order]
							 ,[Position]
							 ,[OwnerName]
							 ,[OwnershipVolume]
							 ,[OwnershipProcessDate]
							 ,[Rule]
							 ,[Movement]
							 ,[% Standard Uncertainty]
							 ,[Uncertainty]
							 ,[BackupMovementId]
							 ,[GlobalMovementId]
							 ,[SourceProductId]
							 ,[OwnershipPercentage]
							 ,[Category]
							 ,[Element]
							 ,[BatchId]
							 ,[NodeName]
							 ,[CalculationDate]
							 ,[OwnershipTicketId]
							 ,[scenarioname]
							 ,[CreatedBy]
							 ,[CreatedDate] 
							 ,[LastModifiedBy]
							 ,[LastModifiedDate]
							 )

			 VALUES (SOURCE.[MovementId]
                    ,SOURCE.[MovementTransactionId]
                    ,SOURCE.[OperationalDate]
                    ,SOURCE.[Operacion]
                    ,SOURCE.[SourceNode]
                    ,SOURCE.[DestinationNode]
                    ,SOURCE.[SourceProduct]
                    ,SOURCE.[DestinationProduct]
                    ,SOURCE.[NetStandardVolume]
                    ,SOURCE.[GrossStandardVolume]
                    ,SOURCE.[MeasurementUnit]
                    ,SOURCE.[EventType]
                    ,SOURCE.[SystemName]
					,SOURCE.[SourceMovementId]
					,SOURCE.[Order]
					,SOURCE.[Position]
					,SOURCE.[OwnerName]
					,SOURCE.[OwnershipVolume]
					,SOURCE.[OwnershipProcessDate]
					,SOURCE.[Rule]
                    ,SOURCE.[Movement]
                    ,SOURCE.[% Standard Uncertainty]
                    ,SOURCE.[Uncertainty]
                    ,SOURCE.[BackupMovementId]
                    ,SOURCE.[GlobalMovementId]
                    ,SOURCE.[SourceProductId]
					,SOURCE.[OwnershipPercentage]
                    ,SOURCE.[Category]
                    ,SOURCE.[Element]
					,SOURCE.[BatchId]
                    ,SOURCE.[NodeName]
                    ,SOURCE.[CalculationDate]
					,SOURCE.[OwnershipTicketId]
					,SOURCE.[scenarioname]
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
    @level1name = N'usp_SaveMovementDetailsWithOwner',
    @level2type = NULL,
    @level2name = NULL