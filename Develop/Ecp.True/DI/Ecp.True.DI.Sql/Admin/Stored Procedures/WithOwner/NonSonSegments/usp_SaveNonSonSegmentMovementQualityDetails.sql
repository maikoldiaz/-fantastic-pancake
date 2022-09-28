/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Aug-05-2020
-- Update date: 	Oct-20-2020  Removed unused variables to improve the performance 
-- Description:     This Procedure is to Get TimeOne Movement Quality Details Data for Non Sons Segment , Element, Node, StartDate, EndDate.
-- EXEC  [Admin].[usp_SaveNonSonSegmentMovementQualityDetails] 183411,43843,'2020-07-27','2020-08-01','4C1B89FC-B66D-4E1E-B1AA-FD4A6D6108C5'
-- SELECT * fROM [Admin].OperationalMovementQuality_NonSon
-- ==============================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_SaveNonSonSegmentMovementQualityDetails]
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

			INSERT INTO [Admin].OperationalMovementQualityNonSon
			   (
					 [RNo]
					,[BatchId]
					,[MovementId]
					,[MovementTransactionId]
					,[CalculationDate]
					,[MovementTypeName]
					,[SourceNode]
					,[DestinationNode]
					,[SourceProduct]
					,[DestinationProduct]
					,[NetStandardVolume]
					,[GrossStandardVolume]
					,[MeasurementUnit]
					,[EventType]
					,[SystemName]
					,[SourceMovementId]	
					,[Order]               
					,[Position]			
					,[Movement]
					,[AttributeId]
					,[AttributeValue]
					,[ValueAttributeUnit]
					,[AttributeDescription]
					,[ProductId]
					,[ExecutionId]
					,[CreatedDate]
					,[CreatedBy]
			   )
			SELECT  
				   OM.RNo 
				  ,OM.[BatchId]
				  ,OM.[MovementId]
				  ,OM.[MovementTransactionId]
				  ,OM.[CalculationDate]
				  ,OM.[TypeMovement]           
				  ,OM.[SourceNode]
				  ,OM.[DestinationNode]
				  ,OM.[SourceProduct]
				  ,OM.[DestinationProduct]
				  ,OM.[NetQuantity]            
				  ,OM.[GrossQuantity]          
				  ,OM.[MeasurementUnit]
				  ,OM.[EventType]
				  ,OM.[SystemName]									-- origen
				  ,OM.[SourceMovementId]							-- Mov. Origen
				  ,OM.[Order]                                       -- Pedido
				  ,OM.[Position]									-- Posición
				  ,OM.[Movement]
				  ,[AttributeId].[Name]		AS [AttributeId]
				  ,Att.[AttributeValue]
				  ,ValAttUnit.[Name]		AS [ValueAttributeUnit]
				  ,Att.[AttributeDescription]
				  ,OM.[ProductID]
				  ,OM.[ExecutionId]  
				  ,OM.[CreatedDate]
				  ,OM.[CreatedBy]				  
			FROM [Admin].[OperationalMovementOwnerNonSon] OM
			INNER JOIN [Admin].[Attribute] Att
			ON Att.MovementTransactionId = OM.MovementTransactionId
           AND OM.[ExecutionId]    		= @ExecutionId
			INNER JOIN [Admin].[CategoryElement] ValAttUnit
            ON Att.ValueAttributeUnit = ValAttUnit.ElementId
			INNER JOIN [Admin].[CategoryElement] AttributeId
			ON AttributeId.ElementId = Att.AttributeId
		   AND AttributeId.CategoryId    = 20
END


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne Movement Quality Details Data for NON SONS Segment , Element, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveNonSonSegmentMovementQualityDetails',
    @level2type = NULL,
    @level2name = NULL