/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	FEB-04-2020
-- Updated Date:	Mar-31-2020 (Added a check to get the nodes where the given input start and end dates should fall under NodeTag table)
-- Updated date: 	Apr-08-2020  -- Added BatchId, SystemName as per PBI 28962. Also replaced Inventory tables with Generic view.
--					Apr-09-2020  -- Removed(BlockchainStatus = 1)  
-- Updated date: 	Apr-22-2020  -- Added EventType Column
-- Updated date: 	Apr-23-2020  -- Removed Distinct to get all the Inventories
-- Updated date: 	May-05-2020  -- Added new Column "InventoryProductId", changed Join Criteria
-- Updated date: 	May-12-2020  -- Splitted deleting the data from target table condition to improve the performance
-- Updated date:    Jun-15-2020  -- Added GrossStandardQuantity as Per 31874
-- Updated date:    Jun-15-2020  -- Changed sorting to InventoryId,CalculationDate as per PBI 31874
-- Updated date:    Jun-15-2020  -- Filtered data based on ScenarioId = 1 as per PBI 31874
-- Updated date:    Jun-25-2020  -- Updated systemname column mapping to sourcesystem
-- Updated date:    Jul-24-2020  -- Added NoLock
-- Updated date:    Aug-04-2020  -- Changed input paramters from STRING to INTEGER AND removed delete old data
-- Updated date:    Aug-07-2020  -- Removing unrequired columns and conditions
-- Description:     This Procedure is to Get TimeOne Inventory Data for Segment Category, Element, Node, StartDate, EndDate.
-- EXEC [Admin].[usp_SaveInventoryDetailsWithoutCutOffForSegment] 'Segmento','Transporte Test','GALAN', '2020-04-17', '2020-04-17','49CA1512-8ACD-4105-9271-01648C1155CC'
   SELECT * FROM [Admin].[OperationalInventory] WHERE InputCategory = 'Segmento' AND ExecutionId = '49CA1512-8ACD-4105-9271-01648C1155CC'
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveInventoryDetailsWithoutCutOffForSegment]
(
        @CategoryId                 INT 
       ,@ElementId                  INT 
       ,@NodeId                     INT 
       ,@StartDate                  DATE                      
       ,@EndDate                    DATE                      
       ,@ExecutionId                INT
)
AS
BEGIN
	SET NOCOUNT ON

	
	--Variables Declaration
	DECLARE  @NoOfDays			          INT
			,@PreviousDayOfStartDate      DATE
			,@Todaysdate	 DATETIME =  [Admin].[udf_GetTrueDate] ()

			
			SELECT @NoOfDays = DATEDIFF(DAY,@StartDate,@EndDate)
			   SET @PreviousDayOfStartDate = DATEADD(DAY,-1,@StartDate)


	INSERT INTO [Admin].[OperationalInventory]
	(
		[RNo],
		[InventoryId],
		[InventoryProductId],
		[CalculationDate],			
		[NodeName],                
		[TankName],
		[BatchId],
		[Product],
		[ProductId],
		[NetStandardVolume],         
		[MeasurementUnit],
		[EventType],
		[SystemName],
		[GrossStandardQuantity],
		[PercentStandardUnCertainty],
		[ExecutionId],               
		[CreatedBy] ,
		[CreatedDate]             
	)
	SELECT ROW_NUMBER() OVER (ORDER BY [InventoryId], CalculationDate ASC) AS RNo
	      ,*
	  FROM (
			SELECT	--DISTINCT
					[Inv].[InventoryId]										AS InventoryId,
					[Inv].[InventoryProductId]                              AS InventoryProductId,
					Inv.InventoryDate							    		AS CalculationDate,
					[Inv].[NodeName]  										AS NodeName,
					[Inv].[TankName]										AS TankName,
					[Inv].[BatchId]											AS BatchId,
					[Inv].ProductName     									AS ProductName,
					[Inv].[ProductId]										AS ProductId,
					Inv.[ProductVolume]										AS NetVolume,
					CEUnit.[Name]											AS MeasurementUnit,
					[Inv].[EventType]                                       AS EventType,
					[Inv].[SourceSystem]									AS SystemName,
					[Inv].[GrossStandardQuantity]                           AS GrossStandardQuantity,
					Inv.[UncertaintyPercentage]								AS UncertaintyPercentage,
					@ExecutionId											AS ExecutionId,
					'ReportUser'											AS CreatedBy,
					@Todaysdate												AS CreatedDate
			FROM [Admin].[view_InventoryInformation] Inv
			INNER JOIN [Admin].[CategoryElement] CEUnit
			ON CEUnit.[ElementId] = Inv.MeasurementUnit
			AND Inv.SegmentId = @ElementId
			AND Inv.InventoryDate BETWEEN @PreviousDayOfStartDate AND @EndDate
			AND Inv.ProductId IS NOT NULL
			AND Inv.SegmentId IS NOT NULL--Segment Should not be NULL
			AND Inv.SourceSystem != 'TRUE' -- Excluding the inventories where source system is "TRUE'
			AND (Inv.NodeId = @NodeId OR @NodeId = 0)
			INNER JOIN [Admin].[NodeTagCalculationDateForSegmentReport](NOLOCK) NTSegment
			ON NTSegment.NodeId            = Inv.NodeId
			AND NTSegment.CalculationDate  = Inv.InventoryDate
			AND NTSegment.ElementId        = Inv.SegmentId
			AND NTSegment.ExecutionId      = @ExecutionId
			WHERE Inv.ScenarioId = 1
			)SubQ

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne Inventory Data for Segement Category, Element, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveInventoryDetailsWithoutCutOffForSegment',
    @level2type = NULL,
    @level2name = NULL