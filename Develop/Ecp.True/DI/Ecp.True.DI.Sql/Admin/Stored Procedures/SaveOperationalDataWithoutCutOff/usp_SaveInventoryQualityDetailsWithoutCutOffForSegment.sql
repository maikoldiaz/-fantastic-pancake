/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	FEB-04-2020
-- Updated Date:	MAR-31-2020 (Added a check to get the nodes where the given input start and end dates should fall under NodeTag table)
-- Updated date: 	Apr-08-2020  -- Added BatchId, SystemName as per PBI 28962. Also replaced Inventory tables with Generic view.
--				    Apr-09-2020  -- Removed(BlockchainStatus = 1)   
-- Updated date: 	Apr-23-2020  -- Removed Distinct to get all the Inventories
-- Updated date: 	May-05-2020  -- Added new Column "InventoryProductId", changed Join Criteria
-- Updated date: 	May-12-2020  -- Removing unnecessary casting and conditions
-- Updated date: 	May-12-2020  -- Splitted deleting the data from target table condition to improve the performance
-- updated date:    Jun-10-2020  -- Separated Common Code Portion and changed logic
-- Updated date:    Jun-15-2020  -- Added new column AttributeId,GrossStandardQuantity as per PBI 31874
-- Updated date:    Jun-15-2020  -- Changed sorting to InventoryId,CalculationDate as per PBI 31874
-- Updated date:    Jun-15-2020  -- Filtered data based on ScenarioId = 1 as per PBI 31874
-- Updated date:    Jun-15-2020  -- Changed the logic of ValueAttributeUnit as per PBI 31874
-- Updated date:    Jun-25-2020  -- Updated systemname column mapping to sourcesystem
-- Updated date:    Jun-26-2020  -- Added condition categoryid = 20 for generating attribute name 
-- Updated date:    Jul-24-2020  -- Added NoLock
-- Updated date:    Aug-04-2020  -- Changed input paramters from STRING to INTEGER AND removed delete old data
-- Updated date:    Aug-07-2020  -- Removing unrequired columns and conditions
-- Description:     This Procedure is to Get TimeOne InventoryQuality Data for Segment Category, Element, Node, StartDate, EndDate.
-- EXEC [Admin].[usp_SaveInventoryQualityDetailsWithoutCutOffForSegment] 'Segmento','Transporte Test','GALAN','2020-04-17','2020-04-17','49CA1512-8ACD-4105-9271-01648C1155CC'
   SELECT * FROM [Admin].[OperationalInventoryQuality] WHERE InputCategory = 'Segmento' AND ExecutionId = '49CA1512-8ACD-4105-9271-01648C1155CC' 
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveInventoryQualityDetailsWithoutCutOffForSegment]
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
  
   -- Variables Declaration
	  DECLARE @Previousdate  DATETIME =  [Admin].[udf_GetTrueDate] ()-1,
			  @Todaysdate	 DATETIME =  [Admin].[udf_GetTrueDate] ()


		INSERT INTO [Admin].[OperationalInventoryQuality]
		(
			 [RNo]
			,[InventoryId]	
			,[InventoryProductId]
			,[CalculationDate]			
			,[NodeName]                  
			,[TankName]
			,[BatchId]
			,[Product]
			,[ProductId]
			,[NetStandardVolume]         
			,[MeasurementUnit]
			,[GrossStandardQuantity]
			,[EventType]
			,[SystemName]
			,[PercentStandardUnCertainty]
			,[AttributeId]
			,[AttributeValue]            
			,[ValueAttributeUnit]        
			,[AttributeDescription]      
			,[ExecutionId]               
			,[CreatedBy]            
			,[CreatedDate]
		 )
	   SELECT  OPI.[RNo]
			  ,OPI.[InventoryId]
			  ,OPI.[InventoryProductId]
			  ,OPI.[CalculationDate]
			  ,OPI.[NodeName]
			  ,OPI.[TankName]
			  ,ISNUll([BatchId],'') AS [BatchId]
			  ,OPI.[Product]
			  ,OPI.[ProductId]
			  ,OPI.[NetStandardVolume]
			  ,OPI.[MeasurementUnit]
			  ,OPI.[GrossStandardQuantity]
			  ,OPI.[EventType]
			  ,OPI.[SystemName]
			  ,OPI.[PercentStandardUnCertainty]
			  ,AttributeId.[Name] AS [AttributeId]
			  ,Att.[AttributeValue]
			  ,AttributeUnitElement.[Name] AS [ValueAttributeUnit]
			  ,Att.[AttributeDescription]
			  ,OPI.[ExecutionId]
			  ,OPI.[CreatedBy]
			  ,@Todaysdate
	 FROM [Admin].[OperationalInventory](NOLOCK) OPI
	INNER JOIN [Admin].[Attribute] Att
	ON Att.[InventoryProductId]		  = OPI.[InventoryProductId]
	INNER JOIN [Admin].[CategoryElement] AttributeUnitElement
	ON AttributeUnitElement.ElementId = Att.ValueAttributeUnit
    INNER JOIN [Admin].[CategoryElement] AttributeId
    ON AttributeId.ElementId = Att.AttributeId
    AND AttributeId.CategoryId = 20
	WHERE OPI.[ExecutionId]    		= @ExecutionId
END


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne InventoryQuality Data for Segment  Category, Element,, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveInventoryQualityDetailsWithoutCutOffForSegment',
    @level2type = NULL,
    @level2name = NULL