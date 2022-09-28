/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	FEB-04-2020
-- Updated Date:	
-- Updated date: 	Apr-08-2020  -- Added BatchId, SystemName as per PBI 28962. Also replaced Inventory tables with Generic view.
--				    Apr-09-2020  -- Removed(BlockchainStatus = 1)   
-- Updated date: 	Apr-22-2020  -- Added EventType Column
-- Updated date: 	Apr-23-2020  -- Removed Distinct to get all the Inventories
-- Updated date: 	May-05-2020  -- Added new Column "InventoryProductId" and changed Join Criteria 
-- Updated date: 	May-12-2020  -- Removing unnecessary casting and conditions
-- Updated date: 	May-12-2020  -- Splitted deleting the data from target table condition to improve the performance
-- Updated date:    Jun-15-2020  -- Added GrossStandardQuantity as per PBI 31874
-- Updated date:    Jun-15-2020  -- Changed sorting to InventoryId,CalculationDate as per PBI 31874
-- Updated date:    Jun-15-2020  -- Filtered data based on ScenarioId = 1 as per PBI 31874
-- Updated date:    Jun-25-2020  -- Updated systemname column mapping to sourcesystem
-- Updated date:    Jul-02-2020   -- Modified for parallel Execution
-- Updated date:    Aug-04-2020  -- Changed input paramters from STRING to INTEGER AND removed delete old data
-- Updated date:    Aug-07-2020  -- Removing unrequired columns and conditions
-- Description:     This Procedure is to Get TimeOne Inventory Data for System Category, Element, Node, StartDate, EndDate.
-- EXEC  [Admin].[usp_SaveInventoryDetailsWithoutCutOffForSystem] 'Sistema','SystemForData','ALL','2019-12-13','2020-01-27','738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'
   SELECT * FROM [Admin].[OperationalInventory] WHERE InputCategory = 'Sistema' AND ExecutionId = '738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveInventoryDetailsWithoutCutOffForSystem]
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
	
  				DECLARE @Previousdate  DATETIME =  [Admin].[udf_GetTrueDate] ()-1,
						@Todaysdate	   DATETIME =  [Admin].[udf_GetTrueDate] ()

		  -- Variables Declaration
	         DECLARE  @NoOfDays			          INT
	         		 ,@PreviousDayOfStartDate     DATE
            
           
            SELECT @NoOfDays = DATEDIFF(DAY,@StartDate,@EndDate)
			   SET @PreviousdayOfStartDate = DATEADD(DAY,-1,@StartDate)


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
		[GrossStandardQuantity],
		[EventType],
		[SystemName],
		[PercentStandardUnCertainty],
		[ExecutionId],               
		[CreatedBy],
		[CreatedDate]
	)
	SELECT ROW_NUMBER() OVER (ORDER BY [InventoryId], CalculationDate ASC) AS RNo
	      ,*
      FROM (
			SELECT	--DISTINCT
					[Inv].[InventoryId]										AS InventoryId,
					[Inv].[InventoryProductId]                              AS InventoryProductId,
					Inv.InventoryDate										AS CalculationDate,
					[Inv].[NodeName]										AS NodeName,
					[Inv].[TankName]										AS TankName,
					[Inv].[BatchId]											AS BatchId,
					[Inv].[ProductName] 									AS ProductName,
					[Inv].[ProductId]										AS ProductId,
					[Inv].[ProductVolume]									AS NetVolume,
					CEUnit.[Name]											AS MeasurementUnit,
					[Inv].GrossStandardQuantity                             AS GrossStandardQuantity,
					[Inv].[EventType]                                       AS EventType,
					[Inv].[SystemName]										AS SystemName,
					Inv.[UncertaintyPercentage]								AS UncertaintyPercentage,
					@ExecutionId											AS ExecutionId,
					'ReportUser'											AS CreatedBy,
					@Todaysdate                                             AS CreatedDate
    			FROM [Admin].[view_InventoryInformation] Inv
    			INNER JOIN [Admin].[CategoryElement] CEUnit
    			ON [CEUnit].[ElementId] = Inv.[MeasurementUnit]
				AND Inv.InventoryDate BETWEEN @PreviousdayOfStartDate AND @EndDate
    			AND Inv.ProductId IS NOT NULL
    			AND Inv.SegmentId IS NOT NULL--Segment Should not be NULL Even for System as System is part of Segment
    			AND Inv.SourceSystem != 'TRUE' -- Excluding the inventories where source system is "TRUE'
				AND (Inv.NodeId = @NodeId OR @NodeId = 0)
    			INNER JOIN [Admin].[NodeTagCalculationDateForSystemReport] NTSystem
				ON  NTSystem.NodeId = Inv.NodeId
				AND NTSystem.CalculationDate  = Inv.InventoryDate
				AND NTSystem.ExecutionId      = @ExecutionId
				WHERE Inv.ScenarioId = 1
				) SubQ
END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne Inventory Data for System Category, Element, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveInventoryDetailsWithoutCutOffForSystem',
    @level2type = NULL,
    @level2name = NULL