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
-- Updated date:    Jun-15-2020  -- Added AttributeId,GrossStandardQuantity column as per PBI 31874
-- Updated date:    Jun-15-2020  -- Changed sorting to InventoryId,CalculationDate as per PBI 31874
-- Updated date:    Jun-15-2020  -- Filtered data based on ScenarioId = 1 as per PBI 31874
-- Updated date:    Jun-15-2020  -- Changed the logic of ValueAttributeUnit as per PBI 31874
-- Updated date:    Jun-26-2020  -- Added condition categoryid = 20 for generating attribute name 
-- Updated date:    Jul-02-2020  -- Updated parallel execution logic
-- Updated date:    Aug-04-2020  -- Changed input paramters from STRING to INTEGER AND removed delete old data
-- Updated date:    Aug-07-2020  -- Removing unrequired columns and conditions
-- Description:     This Procedure is to Get TimeOne InventoryQuality Data for System Category, Element, Node, StartDate, EndDate.
-- EXEC  [Admin].[usp_SaveInventoryQualityDetailsWithoutCutOffForSystem] 'Sistema','SystemForData','ALL','2019-12-13','2020-01-27','738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'
   SELECT * FROM [Admin].[OperationalInventoryQuality] WHERE InputCategory = 'Sistema' AND ExecutionId = '738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveInventoryQualityDetailsWithoutCutOffForSystem]
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
			@Todaysdate	   DATETIME =  [Admin].[udf_GetTrueDate] ()
        
	
			INSERT INTO ADMIN.OperationalInventoryQuality
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
			,[EventType]
			,[SystemName]
			,[GrossStandardQuantity]
			,[PercentStandardUnCertainty]
			,[AttributeId]
			,[AttributeValue]            
			,[ValueAttributeUnit]        
			,[AttributeDescription]      
			,[ExecutionId]               
			,[CreatedBy]   
			,[CreatedDate]
			)
			
					SELECT   OPI.[RNo]
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
							,OPI.[EventType]
			                ,OPI.[SystemName]
			                ,OPI.[GrossStandardQuantity]
			                ,OPI.[PercentStandardUnCertainty]
			                ,AttributeId.[Name] AS [AttributeId]
			                ,Att.[AttributeValue]
			                ,AttributeId.[Name] AS [ValueAttributeUnit]
			                ,Att.[AttributeDescription]
			                ,OPI.[ExecutionId]
			                ,OPI.[CreatedBy]
			                ,@Todaysdate
					FROM [Admin].[OperationalInventory] OPI
					INNER JOIN [Admin].[Attribute] Att
					ON [Att].[InventoryProductId] = OPI.[InventoryProductId]
					INNER JOIN [Admin].[CategoryElement] VallAttUnit
					ON VallAttUnit.[ElementId] = Att.ValueAttributeUnit
					INNER JOIN [Admin].[CategoryElement] AttributeId
			        ON AttributeId.ElementId = Att.AttributeId
					AND AttributeId.CategoryId = 20
				    WHERE OPI.[ExecutionId]    		= @ExecutionId
END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne InventoryQuality Data for System Category, Element, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveInventoryQualityDetailsWithoutCutOffForSystem',
    @level2type = NULL,
    @level2name = NULL