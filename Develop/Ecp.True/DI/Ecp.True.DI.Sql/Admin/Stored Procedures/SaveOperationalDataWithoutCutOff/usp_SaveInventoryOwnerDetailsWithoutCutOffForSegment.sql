/* ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jun-16-2020
-- Updated Date:    Jun-18-2020  Added Sorting based on InventoryId,CalculationDate
-- Updated date:    Jun-26-2020  Removing Ownership join coz that join is not required to bring Owner details
-- Updated date:    Jul-07-2020  -- Modified Ownershippercentage formula
-- Updated date:    Jul-24-2020  -- Added NoLock
-- Updated date:    Aug-04-2020  -- Changed input paramters from STRING to INTEGER AND removed delete old data
-- Updated date:    Aug-07-2020  -- Removing unrequired columns and conditions
-- Updated date: 	Sep-18-2020  Changing Percentage Calculation
-- Description:     This Procedure is to Get TimeOne Inventory Owner Details Data for Segment Category, Element, Node, StartDate, EndDate.
-- EXEC [Admin].[usp_SaveInventoryOwnerDetailsWithoutCutOffForSegment] 'Segmento','Transporte Test','GALAN','2020-04-17','2020-04-17','49CA1512-8ACD-4105-9271-01648C1155CC'
-- EXEC [Admin].[usp_SaveInventoryOwnerDetailsWithoutCutOffForSegment] 'Segmento','Transporte Test','TODOS','2020-04-17','2020-04-17','49CA1512-8ACD-4105-9271-01648C1155CC'
   SELECT * FROM [Admin].[OperationalInventoryOwner] WHERE InputCategory = 'Segmento' AND ExecutionId = '49CA1512-8ACD-4105-9271-01648C1155CC' 
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveInventoryOwnerDetailsWithoutCutOffForSegment]
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
      
	  
         
	INSERT INTO [Admin].[OperationalInventoryOwner]
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
	,[Owner]                
	,[OwnershipVolume]      
	,[OwnershipPercentage]  
	,[ExecutionId]    
	,[CreatedBy]  
	,[CreatedDate]
	)
	SELECT ROW_NUMBER() OVER (ORDER BY InventoryId,CalculationDate ASC) AS RNo
	               ,*
              FROM (
					SELECT  DISTINCT 
					[OI].[InventoryId]										AS InventoryId,
					[OI].[InventoryProductId]                               AS InventoryProductId,
					[OI].CalculationDate									AS CalculationDate,
					[OI].[NodeName]  										AS NodeName,
					[OI].[TankName]									    	AS TankName,
					[OI].[BatchId]											AS BatchId,
					[OI].[Product]        						     		AS ProductName,
					[OI].[ProductId]										AS ProductID,
					[OI].[NetStandardVolume]								AS NetStandardVolume,
					[OI].[MeasurementUnit]					                AS MeasurementUnit,
					[OI].GrossStandardQuantity                              AS GrossStandardQuantity,
					[OI].[EventType]                                        AS EventType,
					[OI].[SystemName]										AS SystemName,
					[Own].[Name]                                            AS [Owner],
					CASE WHEN CHARINDEX('%',OwnershipValueUnit) > 0
                         THEN (OI.NetStandardVolume * O.OwnershipValue) /100
                         ELSE O.OwnershipValue
                    END AS OwnershipVolume,    
					CASE WHEN OI.NetStandardVolume = 0
					     THEN 0
						 WHEN CHARINDEX('%',OwnershipValueUnit) > 0
                         THEN O.OwnershipValue
                         ELSE (O.OwnershipValue / OI.NetStandardVolume) * 100	 
                    END AS OwnershipPercentage,   
					[OI].ExecutionId										AS ExecutionId,
					[OI].[CreatedBy]										AS CreatedBy,
					@Todaysdate                                             AS CreatedDate
			 FROM [Admin].[OperationalInventory](NOLOCK) OI
             INNER JOIN [Offchain].[Owner] O
             ON OI.InventoryProductId = O.InventoryProductId
             JOIN [Admin].[CategoryElement] Own
             ON Own.ElementId = O.OwnerId
			 WHERE OI.[ExecutionId]    	    = @ExecutionId
	         )SubQ
			
			
END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne InventoryOwners Data for Segment  Category, Element,, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveInventoryOwnerDetailsWithoutCutOffForSegment',
    @level2type = NULL,
    @level2name = NULL