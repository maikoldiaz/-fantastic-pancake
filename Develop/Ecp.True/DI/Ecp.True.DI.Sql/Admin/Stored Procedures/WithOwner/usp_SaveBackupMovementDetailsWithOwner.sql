/*-- =================================================================================================
-- Author:			Microsoft
-- Created date:	July-30-2020
-- Updated date:	August-12-2020 -- Removed unnecessary join
-- Updated date:    Sep-16-2020  Filter condition added in start of SP
-- Updated date:    Sep-16-2020  Changed logic of SP, to fetch the details of backup movements irrespective of segement and node
-- Updated date:    Nov-18-2020  Changed logic to use ElementId of Ticket
-- Updated date: May-20-2021  add NodeId Validation
-- <Description>:   This SP is to Fetch Backup Movement Details With Owner Data For PowerBi Report From Tables
					(Unbalance, Product, Node, NodeTag, CategoryElement, Category, Ownership)</Description>
  SELECT COUNT(*) FROM [Admin].[BackupMovementDetailsWithOwner] 
  EXEC [Admin].[usp_SaveBackupMovementDetailsWithOwner] -1
-- ===================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveBackupMovementDetailsWithOwner]
(
	@OwnershipTicketId             INT,
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
               IF OBJECT_ID('tempdb..#TempBackupMovements')IS NOT NULL
               DROP TABLE #TempBackupMovements
               
               
               IF OBJECT_ID('tempdb..#TempSegmentDetails')IS NOT NULL
               DROP TABLE #TempSegmentDetails
               
               
               IF OBJECT_ID('tempdb..#TempSystemDetails')IS NOT NULL
               DROP TABLE #TempSystemDetails
               
               IF OBJECT_ID('tempdb..#TempStagingTable')IS NOT NULL
               DROP TABLE #TempStagingTable
			   

                SELECT * 
				  INTO #TempBackupMovements
				  FROM (
                          SELECT DISTINCT Mov.BackupMovementId      AS MovementId
    						             ,Mov.MovementTransactionID AS MovementTransactionID
    									 ,T.CategoryElementId		AS SegmentId
										 ,Mov.OperationalDate       AS OperationalDate
										 ,Mov.SourceNodeName        AS NodeName
                                         ,[Ownership].[TicketId]    AS OwnershipTicketId
                    	    FROM [Admin].[View_MovementInformation] Mov
                                  JOIN [offchain].[Ownership] [Ownership]
                                    ON [Mov].[MovementTransactionId] = [Ownership].[MovementTransactionId]
                                   AND [Mov].[OwnershipTicketId] = [Ownership].[TicketId]
    					           AND ([Ownership].[TicketId] = @OwnershipTicketId OR @OwnershipTicketId = -1)  -- BACKUP MOVEMENT DETAILS FOR A GIVEN TICKETID
								  JOIN Admin.Ticket T 
                                    ON T.TicketId = [Ownership].[TicketId]
    					    WHERE BackupMovementId IS NOT NULL
    					      AND [Ownership].IsDeleted = 0
                              AND (@NodeId IS  NULL OR ([Mov].DestinationNodeId = @NodeId OR [Mov].SourceNodeId = @NodeId))
                          UNION
                          SELECT DISTINCT Mov.BackupMovementId      AS MovementId
    						             ,Mov.MovementTransactionID AS MovementTransactionID
    									 ,T.CategoryElementId		AS SegmentId
										 ,Mov.OperationalDate       AS OperationalDate
										 ,Mov.DestinationNodeName   AS NodeName
                                         ,[Ownership].[TicketId]    AS OwnershipTicketId
                    	    FROM [Admin].[View_MovementInformation] Mov
                                  JOIN [offchain].[Ownership] [Ownership]
                                    ON [Mov].[MovementTransactionId] = [Ownership].[MovementTransactionId]
                                   AND [Mov].[OwnershipTicketId] = [Ownership].[TicketId]
    					           AND ([Ownership].[TicketId] = @OwnershipTicketId OR @OwnershipTicketId = -1)  -- BACKUP MOVEMENT DETAILS FOR A GIVEN TICKETID
								  JOIN Admin.Ticket T 
                                    ON T.TicketId = [Ownership].[TicketId]
    					    WHERE BackupMovementId IS NOT NULL
    					      AND [Ownership].IsDeleted = 0
                              AND (@NodeId IS NOT NULL AND ([Mov].DestinationNodeId = @NodeId OR [Mov].SourceNodeId = @NodeId))
                    	) TempBackupMovements



                  SELECT * 
				    INTO #TempSegmentDetails
                    FROM (
                          SELECT                   
                           BM.MovementId																			-- Identificación del movimiento
                          ,Mov.BatchId
                          ,BM.MovementTransactionId
						  ,BM.OperationalDate AS [OperationalDate]													-- Fecha (operacional)
                          ,Mov.OperationalDate AS [Date]
                          ,Mov.MovementTypeName AS [Operacion]														-- Operación
                          ,Mov.SourceNodeName AS [SourceNode]														-- Nodo Origen
                          ,Mov.DestinationNodeName AS DestinationNode												-- Nodo Destino
                          ,Mov.SourceProductName AS [SourceProduct]												    -- Producto Origen
                          ,Mov.DestinationProductName AS [DestinationProduct]										-- Producto Destino
                          ,Mov.NetStandardVolume 	AS [NetStandardVolume]						                    -- Volumen Neto
                          ,Mov.GrossStandardVolume  AS [GrossStandardVolume]					                    -- Volumen Bruto
                          ,CEUnits.[Name] AS [MeasurementUnit]														-- Unidad
						  ,Mov.EventType                                                                            -- Acción
                          ,Mov.SourceSystem AS [SystemName]
						  ,Cat.[Name] AS [Category]
                          ,Element.[Name] AS [Element]
						  ,Mov.BackupMovementId
						  ,Mov.GlobalMovementId
                          ,[Prod].[ProductId] AS [ProductId]
						  ,CONCAT('-_-', BM.[NodeName], '-_-') AS [NodeName]
						  ,[Mov].[SegmentId] AS SegmentId
                          ,[BM].[OwnershipTicketId] AS [OwnershipTicketId]
                    FROM [Admin].view_MovementInformation [Mov]
                    INNER JOIN #TempBackupMovements BM
                            ON Mov.MovementID = BM.MovementId
                    INNER JOIN [Admin].[Product] Prod
                            ON (Prod.ProductId = Mov.SourceProductId
                    		OR Prod.ProductId = Mov.DestinationProductId)
                    INNER JOIN [Admin].[CategoryElement] [Element] 
                            ON [BM].[SegmentId] = [Element].[ElementId]
                    INNER JOIN [Admin].[Category]  [Cat]
                            ON [Element].[CategoryId] = [Cat].[CategoryId]
                    --INNER JOIN [Admin].[CategoryElement] [ElementOwner] 
                    --        ON [Ownership].[OwnerId] = [ElementOwner].[ElementId]
                    --       AND [ElementOwner].[CategoryID] = 7 -- 'Propietario' From name Column in Category
                    INNER JOIN [Admin].[CategoryElement] CEUnits
                            ON CEUnits.ElementId = Mov.MeasurementUnit 
                           AND CEUnits.CategoryID = 6 --'Unidad de Medida' From name Column in Category
                    LEFT JOIN [Admin].[VariableType] Vt
                           ON Vt.VariableTypeId = Mov.VariableTypeId
                    LEFT JOIN [Admin].[MovementContract] [Cont]
                           ON [Mov].ContractId = [Cont].ContractId
                          AND [Cont].IsDeleted = 0
                    INNER JOIN [Admin].[Node] ND
                           ON (   ND.NodeId = Mov.SourceNodeId 
                               OR ND.NodeId = Mov.DestinationNodeId
							   )
                    ) TempSegmentDetails


                     SELECT * 
                       INTO #TempSystemDetails
                       FROM (
                             SELECT 
							 Seg.MovementId										-- Identificación del movimiento
							,Seg.BatchId
							,Seg.MovementTransactionId
							,Seg.[OperationalDate]								-- Fecha (operacional)
                            ,Seg.[Date]
							,Seg.[Operacion]									-- Operación
							,Seg.[SourceNode]									-- Nodo Origen
							,Seg.DestinationNode								-- Nodo Destino
							,Seg.[SourceProduct]								-- Producto Origen
							,Seg.[DestinationProduct]							-- Producto Destino
							,Seg.[NetStandardVolume]						    -- Volumen Neto
							,Seg.[GrossStandardVolume]					        -- Volumen Bruto
							,Seg.[MeasurementUnit]								-- Unidad
							,Seg.EventType                                      -- Acción
							,Seg.[SystemName]
							,Cat1.[Name]					AS [Category]
							,CatEle.[Name]					AS [Element]
							,Seg.BackupMovementId
							,Seg.GlobalMovementId
							,Seg.[ProductId]
							,Seg.[NodeName]
							,Seg.SegmentId
                            ,Seg.OwnershipTicketId
                       FROM #TempSegmentDetails Seg
                       INNER JOIN [Admin].[CategoryElement] [Element] 
                               ON [Seg].[SegmentId] = [Element].[ElementId]
                       INNER JOIN [Admin].NodeTag NT
                               ON NT.ElementId = Element.ElementId
                       INNER JOIN [Admin].NodeTag NT1
                               ON NT.NodeId = NT1.NodeId
                       INNER JOIN [Admin].CategoryElement CatEle
                               ON CatEle.ElementId = NT1.ElementId 
							  AND CatEle.CategoryId = 8
                       INNER JOIN [Admin].Category  Cat1
                               ON CatEle.CategoryId = Cat1.CategoryId
                       ) SystemDetails


              SELECT * 
                INTO #TempStagingTable
                FROM (
                      SELECT
                            [MovementId]                                                         -- Id movimiento
                           ,[BatchId] 						                                     -- Id batch
                           ,[MovementTransactionId]		                                         
                           ,[OperationalDate]				                                     -- Fecha
                           ,[Date]
                           ,[Operacion]					                                         -- Operación
                           ,[SourceNode]					                                     -- Nodo origen
                           ,[DestinationNode]				                                     -- Nodo destino
                           ,[SourceProduct]				                                         -- Producto origen
                           ,[DestinationProduct]			                                     -- Producto destino
                           ,[NetStandardVolume]			                                         -- Volumen neto
                           ,[GrossStandardVolume]			                                     -- Volumen bruta
                           ,[MeasurementUnit]				                                     -- Unidad
                           ,[EventType]					                                         -- Acción
                           ,[SystemName]                                                         -- Origen
                           ,[Category]						                                        
                           ,[Element]						                                        
                           ,[NodeName]						                                        
                           ,[OperationalDate] AS [CalculationDate]									
                           ,[BackupmovementId]                                                      -- Id movimiento respaldo
                           ,[GlobalmovementId]                                                      -- Id movimiento global
                           ,[ProductId]
                           ,[OwnershipTicketId]
                           --,ROW_NUMBER() OVER (ORDER BY [MovementId],[OperationalDate] ASC) AS RNo	 --
                      FROM 
                          (
                            SELECT * FROM #TempSegmentDetails
                            UNION 
                            SELECT * FROM #TempSystemDetails
						  ) IQ 
                      )SQ ORDER BY [MovementId],[OperationalDate] ASC

                   MERGE [Admin].[BackupMovementDetailsWithOwner] Dest 
                   USING #TempStagingTable Src
                      ON ISNULL(Dest.[MovementId]				,'')  =	ISNULL(Src.[MovementId]			    ,'')
                     AND ISNULL(Dest.[BatchId] 				    ,'')  =	ISNULL(Src.[BatchId] 				,'')
                     AND ISNULL(Dest.[MovementTransactionId]	,'')  =	ISNULL(Src.[MovementTransactionId]	,'')
                     AND ISNULL(Dest.[OperationalDate]			,'')  =	ISNULL(Src.[OperationalDate]		,'')
                     AND ISNULL(Dest.[Date]			            ,'')  =	ISNULL(Src.[Date]		            ,'')
                     AND ISNULL(Dest.[Operacion]				,'')  =	ISNULL(Src.[Operacion]				,'')
                     AND ISNULL(Dest.[SourceNode]				,'')  =	ISNULL(Src.[SourceNode]				,'')
                     AND ISNULL(Dest.[DestinationNode]			,'')  =	ISNULL(Src.[DestinationNode]		,'')
                     AND ISNULL(Dest.[SourceProduct]			,'')  =	ISNULL(Src.[SourceProduct]			,'')
                     AND ISNULL(Dest.[DestinationProduct]		,'')  =	ISNULL(Src.[DestinationProduct]		,'')
                     AND ISNULL(Dest.[MeasurementUnit]			,'')  =	ISNULL(Src.[MeasurementUnit]		,'')
                     AND ISNULL(Dest.[EventType]				,'')  =	ISNULL(Src.[EventType]				,'')
                     AND ISNULL(Dest.[SystemName]				,'')  =	ISNULL(Src.[SystemName]				,'')
                     AND ISNULL(Dest.[Category]				    ,'')  =	ISNULL(Src.[Category]				,'')
                     AND ISNULL(Dest.[Element]					,'')  =	ISNULL(Src.[Element]				,'')
                     AND ISNULL(Dest.[NodeName]				    ,'')  =	ISNULL(Src.[NodeName]				,'')
                     AND ISNULL(Dest.[CalculationDate]			,'')  =	ISNULL(Src.[CalculationDate]		,'')
                     AND ISNULL(Dest.[ProductId]			  	,'')  =	ISNULL(Src.[ProductId]				,'')
					 AND ISNULL(Dest.[BackupmovementId]			,'')  = ISNULL(Src.[BackupmovementId]	    ,'')
					 AND ISNULL(Dest.[GlobalmovementId]			,'')  = ISNULL(Src.[GlobalmovementId]       ,'')
					 AND ISNULL(Dest.[OwnershipTicketId]		,'')  = ISNULL(Src.[OwnershipTicketId]      ,'')
                                      
                   WHEN MATCHED AND (
				                        Dest.[NetStandardVolume]	<>	Src.[NetStandardVolume]	
                   					OR	Dest.[GrossStandardVolume]	<>	Src.[GrossStandardVolume]
                   				)
                   THEN UPDATE 
				           SET  Dest.[NetStandardVolume]		= Src.[NetStandardVolume]	
                   			   ,Dest.[GrossStandardVolume]		= Src.[GrossStandardVolume]
							   --,Dest.[BackupmovementId]			= Src.[BackupmovementId]	
							   --,Dest.[GlobalmovementId]			= Src.[GlobalmovementId]
                   			   ,Dest.[LastModifiedBy]				= 'ReportUser'
                   			   ,Dest.[LastModifiedDate]				= @Todaysdate
                   
                   WHEN NOT MATCHED BY TARGET 
                       THEN INSERT (
                   				    [MovementId]					
                   				   ,[BatchId] 						
                   				   ,[MovementTransactionId]		
                   				   ,[OperationalDate]
                                   ,[Date]
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
                   				   ,[Category]						
                   				   ,[Element]						
                   				   ,[NodeName]						
                   				   ,[CalculationDate]				
                   				   ,[BackupmovementId]				
                   				   ,[GlobalmovementId]				
                   				   ,[ProductId]
                                   ,[OwnershipTicketId]
                   				   ,[CreatedBy]					
                   				   ,[CreatedDate]					
                   				   )			
                   	      VALUES
                   	      	   (
                   	      	    Src.[MovementId]                            
                   	      	   ,Src.[BatchId] 						         
                   	      	   ,Src.[MovementTransactionId]		         
                   	      	   ,Src.[OperationalDate]
                               ,Src.[Date]
                   	      	   ,Src.[Operacion]					         
                   	      	   ,Src.[SourceNode]					         
                   	      	   ,Src.[DestinationNode]				         
                   	      	   ,Src.[SourceProduct]				         
                   	      	   ,Src.[DestinationProduct]			         
                   	      	   ,Src.[NetStandardVolume]			         
                   	      	   ,Src.[GrossStandardVolume]			         
                   	      	   ,Src.[MeasurementUnit]				         
                   	      	   ,Src.[EventType]					         
                   	      	   ,Src.[SystemName]                            
                   	      	   ,Src.[Category]						         
                   	      	   ,Src.[Element]						         
                   	      	   ,Src.[NodeName]						         
                   	      	   ,Src.[CalculationDate]	
                   	      	   ,Src.[BackupmovementId]                      
                   	      	   ,Src.[GlobalmovementId]                      
                   	      	   ,Src.[ProductId]
                               ,Src.[OwnershipTicketId]
                   	      	   ,'ReportUser'
                   	      	   ,@Todaysdate
                   	      	   );
END

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This SP is to Fetch Backup Movement Details With Owner Data For PowerBi Report From Tables (Unbalance, Product, Node, NodeTag, CategoryElement, Category, Ownership)',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveBackupMovementDetailsWithOwner',
    @level2type = NULL,
    @level2name = NULL