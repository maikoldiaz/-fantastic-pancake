/*-- =================================================================================================
-- Author:		Microsoft
-- Created date: Jun-16-2019
-- Updated date: Jun-18-2019 1. Removed "Todos" case as we are not passing that value as parameter from UI.
                             2. Changed logic to get the System related data
							 3. Logic changed for System movement details
-- Updated date: Jun-25-2020  Update origin column logic 
-- Updated date: Aug-12-2020  Removed Unnecessary join
-- Updated date: Sep-16-2020  Filter condition added in start of SP
-- Updated date: Sep-16-2020  Changed logic of SP, to fetch the details of backup movements irrespective of segement and node
-- Updated date: Nov-18-2020  Changed logic to use ElementId of Ticket
-- Updated date: Ago-03-2021  Add temporal table
-- <Description>:	This procedure is to Fetch Backup Movement Details Without Owner Data For PowerBi Report From
				    Tables(Movement, Product, Ticket, Node, NodeTag, CategoryElement, Category, VariableType)</Description>
EXEC [Admin].[usp_SaveBackupMovementDetails]  @TicketId = -1

SELECT DISTINCT MovementId,BatchId,MovementTransactionId,OperationalDate,Operacion,SourceNode,DestinationNode,SourceProduct,DestinationProduct
               ,NetStandardVolume,GrossStandardVolume,MeasurementUnit,EventType,SystemName,BackupMovementId,GlobalMovementId
  FROM [Admin].[BackupMovementDetailsWithoutOwner] 
 WHERE Element = 'Automation_5Fyo1' AND NodeName LIKE '%Automation_miyee%' AND OperationalDate = '2020-09-16'
-- ===================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveBackupMovementDetails] 
(
 @TicketId INT
)
AS	  
BEGIN 
SET NOCOUNT ON
DROP TABLE IF EXISTS #SegementInitial;
DROP TABLE IF EXISTS #Segement;
DROP TABLE IF EXISTS #System;
DROP TABLE IF EXISTS #Source;
DROP TABLE IF EXISTS #BackupMovements;

DECLARE @Todaysdate	 DATETIME =  [Admin].[udf_GetTrueDate] ()

-- GET BACKUP MOVEMENTS

SELECT *
 INTO #BackupMovements 
 FROM(
      SELECT DISTINCT BackupMovementId          AS MovementId
                     ,MovementTransactionID     AS MovementTransactionID
      				 ,Tick.CategoryElementId    AS SegmentId
					 ,OperationalDate           AS OperationalDate
					 ,SourceNodeName            AS NodeName
                     ,[Tick].[TicketId]         AS TicketId
		FROM [Admin].[view_MovementInformation] Mov
        JOIN [Admin].Ticket Tick
          ON Mov.TicketId = Tick.TicketId
       WHERE BackupMovementId IS NOT NULL 
         AND  ([Tick].[TicketId]= @TicketId OR @TicketId = -1) 
	     AND   Tick.[Status] = 0		--> 0 = successfully processed
         AND   Tick.ErrorMessage IS NULL

		 UNION
		SELECT DISTINCT BackupMovementId          AS MovementId
                       ,MovementTransactionID     AS MovementTransactionID
      				   ,Tick.CategoryElementId    AS SegmentId
					   ,OperationalDate           AS OperationalDate
					   ,DestinationNodeName       AS NodeName
                       ,[Tick].[TicketId]         AS TicketId
		FROM [Admin].[view_MovementInformation] Mov
        JOIN [Admin].Ticket Tick
          ON Mov.TicketId = Tick.TicketId
       WHERE BackupMovementId IS NOT NULL 
         AND  ([Tick].[TicketId]= @TicketId OR @TicketId = -1) 
	     AND   Tick.[Status] = 0		--> 0 = successfully processed
         AND   Tick.ErrorMessage IS NULL
	  )SQ

SELECT --DISTINCT			  
	    BM.MovementId																-- Identificación del movimiento
	   ,Mov.BatchId
       ,BM.MovementTransactionId
	   ,BM.OperationalDate  AS [OperationalDate]           		     				-- Fecha (operacional)
	   ,Mov.OperationalDate  AS [Date]           		     				
       ,Mov.MovementTypeName AS [Operacion]												-- Operación
       ,Mov.SourceNodeName   AS [SourceNode]											-- Nodo Origen
       ,Mov.DestinationNodeName AS [DestinationNode]								-- Nodo Destino
       ,Mov.SourceProductName AS [SourceProduct]									-- Producto Origen
       ,Mov.DestinationProductName AS [DestinationProduct]							-- Producto Destino
       ,Mov.NetStandardVolume AS [NetStandardVolume]			                    -- Volumen Neto
       ,Mov.GrossStandardVolume AS [GrossStandardVolume]		                    -- Volumen Bruto
       ,CEUnits.[Name] AS [MeasurementUnit]											-- Unidad
	   ,Mov.EventType                                                               --Acción
	  ,[Mov].SourceSystem AS [SystemName]
	  ,Mov.BackupMovementId AS BackupMovementId                                     -- Id movimiento respaldo
	  ,Mov.GlobalMovementId AS GlobalMovementId                                     -- Id movimiento global
      ,Prod.ProductId AS [ProductId]
      ,Cat.[Name] AS [Category]
      ,Element.[Name] AS [Element]                
      ,CONCAT('-_-', BM.[NodeName], '-_-') AS [NodeName]
	  ,Element.ElementId 
      ,[BM].TicketId AS [TicketId]
	  ,Mov.SourceNodeId 
	  ,Mov.DestinationNodeId
	  INTO #SegementInitial
FROM [Admin].[view_MovementInformation] Mov
INNER JOIN #BackupMovements BM
        ON Mov.MovementId = BM.MovementId
INNER JOIN [Admin].[Product] Prod
        ON (Prod.ProductId = Mov.SourceProductId
		OR Prod.ProductId = Mov.DestinationProductId)
INNER JOIN [Admin].CategoryElement Element
		ON BM.SegmentId = Element.ElementId 
INNER JOIN [Admin].Category  Cat
		ON Element.CategoryId = Cat.CategoryId
INNER JOIN [Admin].[CategoryElement] CEUnits
		ON CEUnits.ElementId = Mov.MeasurementUnit 
LEFT JOIN [Admin].[VariableType] Vt
		ON Vt.VariableTypeId = Mov.VariableTypeId
WHERE CEUnits.[CategoryId] = 6--'Unidad de Medida' From name Column in Category


SELECT * INTO #Segement from #SegementInitial Mov
INNER JOIN [Admin].[Node] ND
		ON (ND.NodeId = Mov.SourceNodeId 
		OR ND.NodeId = Mov.DestinationNodeId)

SELECT 
 [MovementId]                            -- Id movimiento
,[BatchId]                               -- Id batch
,[MovementTransactionId]		         -- 
,[OperationalDate]				         -- Fecha
,[Date]
,[Operacion]					         -- Operación
,[SourceNode]					         -- Nodo origen
,[DestinationNode]				         -- Nodo destino
,[SourceProduct]				         -- Producto origen
,[DestinationProduct]			         -- Producto destino
,[NetStandardVolume]			         -- Cantidad neta
,[GrossStandardVolume]			         -- Cantidad bruta
,[MeasurementUnit]				         -- Unidad
,[EventType]					         -- Acción
,[SystemName]					         -- Origen
,[BackupMovementId]                      -- Id movimiento respaldo
,[GlobalMovementId]                      -- Id movimiento global
,[ProductId]				         
,Cat1.[Name] AS [Category]
,CatEle.[Name]  AS [Element] 						         
,[NodeName]
,[TicketId]
INTO #System
FROM #Segement SG
INNER JOIN [Admin].NodeTag NT
		ON NT.ElementId = SG.ElementId
INNER JOIN [Admin].NodeTag NT1
		ON NT.NodeId = NT1.NodeId
INNER JOIN [Admin].CategoryElement CatEle
		ON CatEle.ElementId = NT1.ElementId 
	   AND CatEle.CategoryId = 8
INNER JOIN [Admin].Category  Cat1
		ON CatEle.CategoryId = Cat1.CategoryId


-- SELECT CODE
SELECT 
 [MovementId]                            -- Id movimiento
,[BatchId]                               -- Id batch
,[MovementTransactionId]		         -- 
,[OperationalDate]				         -- Fecha
,[Date]
,[Operacion]					         -- Operación
,[SourceNode]					         -- Nodo origen
,[DestinationNode]				         -- Nodo destino
,[SourceProduct]				         -- Producto origen
,[DestinationProduct]			         -- Producto destino
,[NetStandardVolume]			         -- Cantidad neta
,[GrossStandardVolume]			         -- Cantidad bruta
,[MeasurementUnit]				         -- Unidad
,[EventType]					         -- Acción
,[SystemName]					         -- Origen
,[BackupMovementId]                      -- Id movimiento respaldo
,[GlobalMovementId]                      -- Id movimiento global
,[ProductId]				         
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
FROM
(
 SELECT [MovementId]    
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
,[BackupMovementId]     
,[GlobalMovementId]     
,[ProductId]				         
,[Category]
,[Element] 						         
,[NodeName]		
,[TicketId]
FROM #Segement
 UNION
 SELECT [MovementId]    
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
,[BackupMovementId]     
,[GlobalMovementId]     
,[ProductId]				         
,[Category]
,[Element] 						         
,[NodeName]		
,[TicketId]
FROM #System
) A

MERGE [Admin].[BackupMovementDetailsWithoutOwner] AS TARGET 
				USING #Source AS SOURCE  
				ON  ISNULL(TARGET.[MovementTransactionId],'') = ISNULL(SOURCE.[MovementTransactionId],'')
				AND ISNULL(TARGET.[MovementId]           ,'') = ISNULL(SOURCE.[MovementId]           ,'')
				AND ISNULL(TARGET.[OperationalDate]      ,'') = ISNULL(SOURCE.[OperationalDate]      ,'')
				AND ISNULL(TARGET.[Date]                 ,'') = ISNULL(SOURCE.[Date]                 ,'')
				AND ISNULL(TARGET.[Operacion]            ,'') = ISNULL(SOURCE.[Operacion]            ,'')
				AND ISNULL(TARGET.[SourceNode]           ,'') = ISNULL(SOURCE.[SourceNode]           ,'')
				AND ISNULL(TARGET.[DestinationNode]      ,'') = ISNULL(SOURCE.[DestinationNode]      ,'')
				AND ISNULL(TARGET.[SourceProduct]        ,'') = ISNULL(SOURCE.[SourceProduct]        ,'')
				AND ISNULL(TARGET.[DestinationProduct]   ,'') = ISNULL(SOURCE.[DestinationProduct]   ,'')
				AND ISNULL(TARGET.[MeasurementUnit]      ,'') = ISNULL(SOURCE.[MeasurementUnit]      ,'')
				AND ISNULL(TARGET.[Category]             ,'') = ISNULL(SOURCE.[Category]             ,'')
				AND ISNULL(TARGET.[Element]              ,'') = ISNULL(SOURCE.[Element]              ,'')
				AND ISNULL(TARGET.[NodeName]             ,'') = ISNULL(SOURCE.[NodeName]             ,'')
				AND ISNULL(TARGET.[ProductId]            ,'') = ISNULL(SOURCE.[ProductId]            ,'')
				AND ISNULL(TARGET.[BatchId]              ,'') = ISNULL(SOURCE.[BatchId]              ,'')
				AND ISNULL(TARGET.[BackupMovementId]     ,'') = ISNULL(SOURCE.[BackupMovementId]     ,'')
				AND ISNULL(TARGET.[GlobalMovementId]     ,'') = ISNULL(SOURCE.[GlobalMovementId]     ,'')
                AND ISNULL(TARGET.[TicketId]             ,'') = ISNULL(SOURCE.[TicketId]             ,'')

				WHEN MATCHED  AND ( 
				                    TARGET.[NetStandardVolume]             <> SOURCE.[NetStandardVolume] OR
					                TARGET.[GrossStandardVolume]           <> SOURCE.[GrossStandardVolume] OR
					                TARGET.[SystemName]                    <> SOURCE.[SystemName] OR
					                TARGET.[EventType]                     <> SOURCE.[EventType] 
					                --TARGET.[BackupMovementId]              <> SOURCE.[BackupMovementId] OR	
					                --TARGET.[GlobalMovementId]              <> SOURCE.[GlobalMovementId] 
									)


				THEN UPDATE 
					  SET TARGET.[NetStandardVolume]             = SOURCE.[NetStandardVolume]
					     ,TARGET.[GrossStandardVolume]           = SOURCE.[GrossStandardVolume]
					     ,TARGET.[SystemName]                    = SOURCE.[SystemName]
					     ,TARGET.[EventType]                     = SOURCE.[EventType]
					     --,TARGET.[BackupMovementId]              = SOURCE.[BackupMovementId]	
					     --,TARGET.[GlobalMovementId]              = SOURCE.[GlobalMovementId] 
					     ,TARGET.LastModifiedBy                  ='ReportUser'
					     ,TARGET.[LastModifiedDate]              = @TodaysDate
					  
       
				/* 2. Performing the INSERT operation */ 
  
				/* When no records are matched with TARGET table 
				   Then insert the records in the target table */ 
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
                              ,[BackupMovementId]                    
                              ,[GlobalMovementId]                    
                              ,[ProductId]				         
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
                      ,SOURCE.[BatchId]                             
                      ,SOURCE.[MovementTransactionId]		         
                      ,SOURCE.[OperationalDate]	
					  ,SOURCE.[Date]
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
                      ,SOURCE.[BackupMovementId]                    
                      ,SOURCE.[GlobalMovementId]                    
                      ,SOURCE.[ProductId]				         
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
    @value = N'This View is to Fetch Data [Admin].[usp_SaveBackupMovementDetails] For PowerBi Report From Tables',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveBackupMovementDetails',
    @level2type = NULL,
    @level2name = NULL
GO