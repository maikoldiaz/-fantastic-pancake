/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Aug-02-2020
-- Update date: 	Sep-17-2020  Changing the logic to get non son segment details 
                                 (instead of IsOperationalSegment = 0 using IsOperationalSegment != 1)
-- Update date: 	Sep-18-2020  Changing Percentage Calculation
-- Update date: 	Sep-22-2020  Need to show only movements where ScenarioId = 1 as per BUG 79451
-- Update date: 	Oct-01-2020  Calculating the data for given period and removed commented code
-- Update date: 	Oct-05-2020  Used ProductVolume instead of GrossStandardQuantity as per BUG 83220
-- Update date: 	Oct-06-2020  Fixed startdate as previousstartdate for initial inventories
-- Update date: 	Oct-20-2020  Added Temp tables, removed unused variables and indexes to improve the performance 
-- Description:     This Procedure is to Get TimeOne Inventory Details Data for Non Segment , Element, Node, StartDate, EndDate.
-- EXEC admin.usp_SaveNonSonSegmentInventoryDetails 183411,43843,'2020-07-27','2020-08-01','4C1B89FC-B66D-4E1E-B1AA-FD4A6D6108C5'
   SELECT * FROM [Admin].[[OperationalInventoryOwner_NonSons]]   ExecutionId = '4C1B89FC-B66D-4E1E-B1AA-FD4A6D6108C5'
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveNonSonSegmentInventoryDetails]
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
					,@Todaysdate	         DATETIME =  [Admin].[udf_GetTrueDate] ()

			SELECT @NoOfDays = DATEDIFF(DAY,@StartDate,@EndDate)			
			SET @PreviousdayOfStartDate = DATEADD(DAY,-1,@StartDate)

			IF OBJECT_ID('tempdb..#InvTempNodeTagCalculationDate')IS NOT NULL
			DROP TABLE #InvTempNodeTagCalculationDate

			IF OBJECT_ID('tempdb..#InvTempNodeTagSegment')IS NOT NULL
			DROP TABLE #InvTempNodeTagSegment
				 
            DROP TABLE IF EXISTS #CategoryElement

			SELECT * 
			  INTO #CategoryElement 
			  FROM [Admin].[CategoryElement]

            CREATE CLUSTERED INDEX IX_CategoryElement_ElementId ON #CategoryElement (ElementId)

			SELECT * INTO #InvTempNodeTagSegment 
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
			INTO #InvTempNodeTagCalculationDate
			FROM #InvTempNodeTagSegment NT
			CROSS APPLY (SELECT	dates	AS  CalculationDate
						 FROM [Admin].[udf_GetAllDates]( NT.StartDate, 
												         NT.EndDate, 
												         NT.NodeId, 
												         NT.ElementId)
						) CA
			WHERE CA.CalculationDate BETWEEN @PreviousdayOfStartDate AND @EndDate   -- INVENTORY IS CALCULATED FROM PREVIOUS OF START DATE TO END DATE

INSERT INTO Admin.[OperationalInventoryOwnerNonSon]
(
 RNo
,Product
,ProductId
,OwnerName
,InventoryId
,InventoryProductId
,CalculationDate
,Nodename 
,TankName
,BatchId
,[NetStandardVolume]
,[MeasurementUnit]
,EventType
,SystemName
,[PercentStandardUnCertainty]
,[Uncertainty]
,[OwnershipVolume]
,[OwnershipPercentage]
,[AppliedRule]
,[GrossStandardQuantity]     
,[ExecutionId]
,createdby
,createddate
)



SELECT --DISTINCT
ROW_NUMBER() OVER (ORDER BY CalculationDate,CreatedDate ASC) AS RNo
,Product
,ProductId
,OwnerName
,InventoryId
,InventoryProductId
,CalculationDate
,Nodename
,TankName
,BatchId
,[NetStandardVolume]
,[MeasurementUnit]
,EventType
,SystemName
,[PercentStandardUnCertainty]
,[Uncertainty]
,[OwnershipVolume]
,[OwnershipPercentage]
,[AppliedRule]
,[GrossStandardQuantity]
,@ExecutionId	
,'ReportUser'	
,@Todaysdate	

FROM
(
SELECT              DISTINCT
                     Inv.ProductName          AS Product
                    ,Inv.ProductId       AS ProductId
                    ,ElementOwner.[Name] AS OwnerName											 -- Propietario
					,Inv.InventoryId
					,Inv.InventoryProductId
					,CAST(Inv.InventoryDate AS DATE) AS CalculationDate
					,Inv.NodeName
					,Inv.TankName
					,Inv.BatchId
					,Inv.ProductVolume  AS [NetStandardVolume]											--ProductVolume,
					,CEUnits.Name AS [MeasurementUnit] 												--MeasurementUnit,
					,Inv.EventType                                                              --Acción
					,Inv.SourceSystem AS SystemName
					,Inv.UncertaintyPercentage  AS [PercentStandardUnCertainty]
					,ISNULL(Inv.ProductVolume,1)*ISNULL(Inv.UncertaintyPercentage,1) AS [Uncertainty]
					,CASE WHEN CHARINDEX('%',OwnershipValueUnit) > 0 
					      THEN (Inv.ProductVolume * [Ownership].OwnershipValue) /100 
					      ELSE [Ownership].OwnershipValue 
					 END AS OwnershipVolume					                    -- Volumen Propiedad						  
					,CASE WHEN Inv.ProductVolume = 0
					      THEN 0
						  WHEN CHARINDEX('%',OwnershipValueUnit) > 0
					      THEN [Ownership].OwnershipValue
					      ELSE ([Ownership].OwnershipValue / Inv.ProductVolume) * 100 END AS OwnershipPercentage
					,'' AS AppliedRule--Owner.[AppliedRule]	
					,Inv.GrossStandardQuantity
					,Inv.CreatedDate AS [CreatedDate]
	FROM [Admin].[view_InventoryInformation] Inv
    INNER JOIN [offchain].[Owner] [Ownership]
			ON [Ownership].[InventoryProductId] = Inv.[InventoryProductId] 
		   AND Inv.ScenarioId = 1
		   AND Inv.InventoryDate BETWEEN @PreviousDayOfStartDate AND @EndDate
    INNER JOIN #CategoryElement ElementOwner  
			ON [Ownership].OwnerId=  ElementOwner.ElementId
    INNER JOIN #CategoryElement CEUnits
		  ON CEUnits.ElementId = Inv.MeasurementUnit
    INNER JOIN #CategoryElement Element 
			ON Element.[ElementId] = inv.segmentid
			AND ElementOwner.CategoryId = 7  --> Here categoryId 7 is "Propietario" i.e. Ownership;
			AND ISNULL(Element.IsOperationalSegment,0) != 1  -- CONSIDER ONLY NON SON SEGMENTS
    INNER JOIN #InvTempNodeTagCalculationDate ND 
	        ON ND.NodeId = Inv.NodeId                       -- CONDITION FOR INVENTORY JOINED ON NODEID

 ) AS invt
END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne Inventory Owner Details Data for NON SONS Segment , Element, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveNonSonSegmentInventoryDetails',
    @level2type = NULL,
    @level2name = NULL