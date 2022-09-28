/* ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jun-16-2020
-- Updated Date:    Jun-18-2020  Added Sorting based on InventoryId,CalculationDate
-- Updated date:    Jun-26-2020  Removing Ownership join coz that join is not required to bring Owner details
-- Updated date:    Jul-07-2020  -- Modified Ownershippercentage formula
-- Updated date:    Aug-04-2020  -- Changed input paramters from STRING to INTEGER AND removed delete old data
-- Updated date:    Aug-07-2020  -- Removing unrequired columns and conditions
-- Updated date: 	Sep-18-2020  Changing Percentage Calculation
-- Description:     This Procedure is to Get TimeOne Inventory Owner Details Data for Segment Category, Element, Node, StartDate, EndDate.
-- EXEC  [Admin].[usp_SaveInventoryOwnerDetailsWithoutCutOffForSystem] 'Sistema','SystemForData','ALL','2019-12-13','2020-01-27','738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'
   SELECT * FROM [Admin].[OperationalInventoryOwner] WHERE InputCategory = 'Sistema' AND ExecutionId = '738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveInventoryOwnerDetailsWithoutCutOffForSystem]
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
					SELECT DISTINCT  
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
			 FROM [Admin].[OperationalInventory] OI
             INNER JOIN [Offchain].[Owner] O
             ON OI.InventoryProductId = O.InventoryProductId
             INNER JOIN [Admin].[CategoryElement] Own
             ON Own.ElementId = O.OwnerId
			 WHERE OI.[ExecutionId]    	    = @ExecutionId
	        )SubQ
			
			
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne InventoryOwners Data for System  Category, Element,, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveInventoryOwnerDetailsWithoutCutOffForSystem',
    @level2type = NULL,
    @level2name = NULL