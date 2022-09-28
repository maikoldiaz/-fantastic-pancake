/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Aug-05-2020
-- Update date: 	Oct-20-2020  Removed unused variables to improve the performance 
-- Description:     This Procedure is to Get TimeOne Inventory Quality Details Data for Non Sons Segment , Element, Node, StartDate, EndDate.
-- exec [Admin].[usp_SaveNonSonSegmentInventoryQualityDetails] 183411,43843,'2020-07-27','2020-08-01','4C1B89FC-B66D-4E1E-B1AA-FD4A6D6108C5'
   SELECT * FROM [Admin].[OperationalInventoryQuality_NonSon] WHERE  ExecutionId = '4C1B89FC-B66D-4E1E-B1AA-FD4A6D6108C5'
-- ==============================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_SaveNonSonSegmentInventoryQualityDetails]
(
	     
	     @ElementId	    INT
		,@NodeId	    INT
		,@StartDate		DATE		  
		,@EndDate		DATE		  
		,@ExecutionId	INT 
)
AS
BEGIN
  SET NOCOUNT ON

	        DECLARE @Todaysdate	   DATETIME =  [Admin].[udf_GetTrueDate] ()

			INSERT INTO [Admin].[OperationalInventoryQualityNonSon]
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
					,[GrossStandardQuantity]  
					,[MeasurementUnit]		                  
					,[SystemName]			
					,[AttributeId]            
					,[AttributeValue]         
					,[ValueAttributeUnit]     
					,[AttributeDescription]            
					,[ExecutionId]            
					,[CreatedBy]              
					,[CreatedDate]            
					)
			SELECT 
				   OPI.[RNo] 
				  ,OPI.[InventoryId]
				  ,OPI.[InventoryProductId]
				  ,OPI.[CalculationDate]
				  ,OPI.[NodeName]
				  ,OPI.[TankName]
				  ,ISNUll([BatchId],'') AS [BatchId]
				  ,OPI.[Product]
				  ,OPI.[ProductId]
				  ,OPI.[NetStandardVolume]
				  ,OPI.[GrossStandardQuantity]
				  ,OPI.[MeasurementUnit]
				  ,OPI.[SystemName]
				  ,AttributeId.Name AS [AttributeId]
				  ,Att.[AttributeValue] 
				  ,AttributeUnitElement.[Name] AS [ValueAttributeUnit]
				  ,Att.[AttributeDescription]
				  ,OPI.[ExecutionId]
				  ,OPI.[CreatedBy]
				  ,@Todaysdate
			FROM [Admin].[OperationalInventoryOwnerNonSon] OPI
			INNER JOIN [Admin].[Attribute] Att
			ON Att.[InventoryProductId]		= OPI.[InventoryProductId]
		   AND OPI.[ExecutionId]    		= @ExecutionId
			INNER JOIN [Admin].[CategoryElement] AttributeUnitElement
			ON AttributeUnitElement.ElementId = Att.ValueAttributeUnit
			INNER JOIN [Admin].[CategoryElement] AttributeId
			ON AttributeId.ElementId = Att.AttributeId
		   AND AttributeId.CategoryId = 20
END
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne Inventory Quality Details Data for NON SONS Segment , Element, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveNonSonSegmentInventoryQualityDetails',
    @level2type = NULL,
    @level2name = NULL