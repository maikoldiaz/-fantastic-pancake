/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Aug-02-2020
-- Update date: 	Sep-17-2020  Changing the logic to get non son segment details 
                                 (instead of IsOperationalSegment = 0 using IsOperationalSegment != 1)
-- Update date: 	Sep-22-2020  Need to show only movements where ScenarioId = 1 as per BUG 79451
-- Update date: 	Oct-01-2020  Calculating the data for given period
-- Update date: 	Oct-07-2020  Removed messagetypeid condition for interface, tolerance as per bug 83220.
-- Update date: 	Oct-09-2020  Modified code to populate data into newly added columns in table
-- Update date: 	Oct-20-2020  Added Temp tables and indexes to improve the performance 
-- Description:     This Procedure is to Get TimeOne Movement Details Data for Non Segment , Element, Node, StartDate, EndDate.
-- EXEC admin.usp_SaveNonSonSegmentMovementDetails 183411,43843,'2020-07-27','2020-08-01','4C1B89FC-B66D-4E1E-B1AA-FD4A6D6108C5'
   SELECT * FROM [Admin].[OperationalMovementOwner_NonSon]   ExecutionId = '4C1B89FC-B66D-4E1E-B1AA-FD4A6D6108C5'
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveNonSonSegmentMovementDetails]
(
	    
	     @ElementId		INT
		,@NodeId	    INT
		,@StartDate		DATE		  
		,@EndDate		DATE		  
		,@ExecutionId	INT 
)
AS
BEGIN

SET NOCOUNT ON
	        DECLARE  @NoOfDays			     INT
					,@PreviousDayOfStartDate DATE
					,@IdentifiedLosses       VARCHAR (100)
			DECLARE @PeridasIdentificadas NVARCHAR(50) = N'Pérdidas Identificadas';
	
			IF OBJECT_ID('tempdb..#MovTempNodeTagCalculationDate')IS NOT NULL
			DROP TABLE #MovTempNodeTagCalculationDate

			IF OBJECT_ID('tempdb..#MovTempNodeTagSegment')IS NOT NULL
			DROP TABLE #MovTempNodeTagSegment

			DROP TABLE IF EXISTS #CategoryElement
			DROP TABLE IF EXISTS #Movements

			DECLARE @Todaysdate	   DATETIME =  [Admin].[udf_GetTrueDate] ()
			SELECT @NoOfDays = DATEDIFF(DAY,@StartDate,@EndDate)			
			SET @PreviousdayOfStartDate = DATEADD(DAY,-1,@StartDate)


			SELECT * 
			  INTO #CategoryElement 
			  FROM [Admin].[CategoryElement]

            CREATE CLUSTERED INDEX IX_CategoryElement_ElementId ON #CategoryElement (ElementId)

			SELECT * 
			  INTO #Movements
			  FROM [Admin].[view_MovementInformation] [Mov] 
			 WHERE Mov.ScenarioId = 1
			   AND OperationalDate BETWEEN @StartDate AND @EndDate

			SELECT * INTO #MovTempNodeTagSegment 
			  FROM (
					SELECT DISTINCT NT.NodeId,
									NT.ElementId,
									Ce.Name AS ElementName,
									CASE WHEN NT.StartDate < @StartDate 
									     THEN @StartDate
									     ELSE NT.StartDate END
									       AS StartDate,
									CASE WHEN (   CAST(NT.EndDate AS DATE) > (CAST(GETDATE() AS DATE)) 
									           OR CAST(NT.EndDate AS DATE) > (DATEADD(DAY,@NoOfDays,@StartDate))
											  )
										 THEN @EndDate
										 ELSE NT.EndDate 
										 END AS EndDate 
					FROM Admin.NodeTag NT
					INNER JOIN #CategoryElement CE
					ON CE.ElementId = NT.ElementId
					WHERE NT.ElementId = @ElementId
					AND (NT.NodeId = @NodeId) 
					AND CE.CategoryId = 2 
					) A                   	
		
			SELECT  NT.NodeId
				   ,NT.ElementId 
				   ,NT.ElementName
				   ,Ca.CalculationDate 
			INTO #MovTempNodeTagCalculationDate
			FROM #MovTempNodeTagSegment NT
			CROSS APPLY (SELECT	dates	AS  CalculationDate
						 FROM [Admin].[udf_GetAllDates]( NT.StartDate, 
												         NT.EndDate, 
												         NT.NodeId, 
												         NT.ElementId)
						) CA
			WHERE CA.CalculationDate BETWEEN @StartDate AND @EndDate


INSERT INTO [Admin].[OperationalMovementOwnerNonSon]
(
 [RNo]                    
,[MovementId]             
,[BatchId]                
,[MovementTransactionId]  
,[CalculationDate]        
,[TypeMovement]           
,[SourceNode]             
,[DestinationNode]
,SourceProductId
,[SourceProduct]   
,DestinationProductId
,[DestinationProduct]     
,[NetQuantity]            
,[GrossQuantity]          
,[MeasurementUnit]        
,[EventType]              
,[Order]                  
,[Position]               
,[SystemName]             
,[SourceMovementId]       
,[OwnerName]              
,[Ownershipvolume]        
,[ExecutionDate]          
,[Rule]                   
,[Movement]               
,[UncertaintyPercentage]  
,[Uncertainty]		       
,[BackupMovementId]       
,[GlobalMovementId]       
,[ProductID] 
,[ProductName] 
,[Ownershippercentage]    
,[ExecutionId]   

--Internal Common Columns
,[CreatedBy]              
,[CreatedDate]            
)
SELECT
ROW_NUMBER() OVER (ORDER BY [MovementId],[OperationalDate] ASC) AS RNo
,[MovementId]
,[BatchId]
,[MovementTransactionId]
,[OperationalDate]
,[Operacion]
,[SourceNode]
,[DestinationNode]
,[SourceProductId]
,[SourceProduct]
,[DestinationProductId]
,[DestinationProduct]
,[NetStandardVolume]
,[GrossStandardVolume]
,[MeasurementUnit]
,[EventType]
,[Order]
,[Position]
,[SystemName]
,[SourceMovementId]
,[OwnerName]
,[OwnershipVolume]
,[OwnershipProcessDate]
,[Rule]
,[Movement]
,[% Standard Uncertainty]
,[Uncertainty]
,[BackupMovementId]
,[GlobalMovementId]
,[ProductId]
,[ProductName] 
,[OwnershipPercentage]
,@ExecutionId	 
,'ReportUser'	 
,@Todaysdate	 



FROM 
(
SELECT                     DISTINCT
						   Mov.MovementId																			-- Identificación del movimiento
						  ,Mov.MovementTransactionId
						  ,Mov.OperationalDate        AS [OperationalDate]											-- Fecha (operacional)
                          ,CEType.[Name]              AS [Operacion]									     		-- Operación
                          ,Mov.SourceNodeName         AS [SourceNode]								    			-- Nodo Origen
                          ,Mov.DestinationNodeName    AS DestinationNode											-- Nodo Destino
                          ,Mov.SourceProductId        AS SourceProductId
						  ,Mov.SourceProductName      AS [SourceProduct]										    -- Producto Origen
                          ,Mov.DestinationProductId   AS [DestinationProductId]
						  ,Mov.DestinationProductName AS [DestinationProduct]										-- Producto Destino
                          ,Mov.NetStandardVolume 	  AS [NetStandardVolume]						                -- Volumen Neto
                          ,Mov.GrossStandardVolume    AS [GrossStandardVolume]					      
						  -- Volumen Bruto
                          ,CEUnits.[Name]             AS [MeasurementUnit]		     								-- Unidad
						  ,Mov.EventType                                                                            -- Acción
						  ,Mov.SourceSystem AS [SystemName]                                                         -- Origen
						  ,CASE 
								WHEN [Mov].SystemName = 'FICO' AND CEType.[Name] IN ('Compra','Venta') 
										THEN Mov.SourceMovementId
								WHEN CEType.[Name] IN ('ACE Entrada','ACE Salida')
										THEN Mov.SourceMovementId							
								ELSE NULL
							END										AS [SourceMovementId]							-- Mov. Origen
						  ,''										AS [Order]										-- Pedido
						  ,''										AS [Position]									-- Posición
                          ,ElementOwner.[Name] AS [OwnerName]														-- Propietario
                          ,CASE WHEN CHARINDEX('%',OwnershipValueUnit) > 0 
						        THEN ([Mov].NetStandardVolume * [Ownership].OwnershipValue) /100 
						        ELSE [Ownership].OwnershipValue 
						   END AS OwnershipVolume					                    -- Volumen Propiedad						  
                          ,'' AS [OwnershipProcessDate]--[Ownership].ExecutionDate AS [OwnershipProcessDate]										-- Fecha Ejecución Propiedad
                          ,'' AS [Rule]--[Ownership].AppliedRule AS [Rule]														-- Regla
                          ,CASE 
								WHEN CEType.[ElementId] = 42 --'Traslado de productos' 
								THEN 'Interfases'
								WHEN CEType.[ElementId] = 43--'Tolerancia' 
								THEN 'Tolerancia'
								WHEN Mov.[Classification] = 'PerdidaIdentificada' 
								AND [Mov].MessageTypeId = 2 
								THEN @PeridasIdentificadas
								WHEN Mov.[Classification] <> 'PerdidaIdentificada' 
								AND [Mov].MessageTypeId = 1
								AND CEType.[ElementId] NOT IN (42,43,44) --('Traslado de productos','Tolerancia','Pérdida no identificada')
								AND Mov.DestinationNodeId = ND.NodeId
								THEN 'Entradas'
								WHEN Mov.[Classification] <> 'PerdidaIdentificada' 
								AND [Mov].MessageTypeId = 1
								AND CEType.[ElementId] NOT IN (42,43,44)--('Traslado de productos','Tolerancia','Pérdida no identificada')
								AND Mov.SourceNodeId = ND.NodeId
								THEN 'Salidas'
						   END AS [Movement]																		    -- Movimiento
                          ,Mov.UncertaintyPercentage   AS [% Standard Uncertainty]			                            -- % Incertidumbre Estándar
                          ,ISNULL(Mov.NetStandardVolume,1)*ISNULL(Mov.UncertaintyPercentage,1)  AS [Uncertainty]		-- Incertidumbre
						  ,Mov.[BackupMovementId]
                          ,Mov.[GlobalMovementId]
                          ,[Prod].[ProductId] AS [ProductId]
						  ,[Prod].[Name]      AS [ProductName]
                          ,CASE WHEN [Mov].NetStandardVolume = 0
						        THEN 0
								WHEN CHARINDEX('%',OwnershipValueUnit) > 0
								THEN [Ownership].OwnershipValue
						        ELSE ([Ownership].OwnershipValue / [Mov].NetStandardVolume) * 100 
						   END AS OwnershipPercentage
                          ,Mov.BatchId
FROM #Movements [Mov]
INNER JOIN [Admin].[Product] Prod
ON (   Prod.ProductId = Mov.SourceProductId
	 OR Prod.ProductId = Mov.DestinationProductId)
INNER JOIN [offchain].[Owner] [Ownership]
ON [Mov].[MovementTransactionId] = [Ownership].[MovementTransactionId]
INNER JOIN [Admin].[CategoryElement] [Element] 
ON [Mov].[SegmentId] = [Element].[ElementId]
AND ISNULL(Element.IsOperationalSegment,0) != 1 -- CONSIDER ONLY NON SON SEGMENTS   
INNER JOIN [Admin].[Category]  [Cat]
ON [Element].[CategoryId] = [Cat].[CategoryId]
INNER JOIN [Admin].[CategoryElement] [ElementOwner] 
ON [Ownership].[OwnerId] = [ElementOwner].[ElementId]
AND [ElementOwner].[CategoryID] = 7 -- 'Propietario' From name Column in Category
INNER JOIN [Admin].[CategoryElement] CEUnits
ON CEUnits.ElementId = Mov.MeasurementUnit 
AND CEUnits.CategoryID = 6 --'Unidad de Medida' From name Column in Category
INNER JOIN [Admin].[CategoryElement] CEType
ON CEType.ElementId = Mov.MovementTypeId 
AND CEType.CategoryID = 9 --'Tipo Movimiento'  From name Column in Category
LEFT JOIN [Admin].[MovementContract] [Cont]
ON [Mov].ContractId = [Cont].ContractId
AND	[Cont].IsDeleted = 0
INNER JOIN #MovTempNodeTagCalculationDate ND
ON (   ND.NodeId = Mov.SourceNodeId 
    OR ND.NodeId = Mov.DestinationNodeId
	) 
)SQ

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne Movement Owner Details Data for NON SONS Segment , Element, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveNonSonSegmentMovementDetails',
    @level2type = NULL,
    @level2name = NULL