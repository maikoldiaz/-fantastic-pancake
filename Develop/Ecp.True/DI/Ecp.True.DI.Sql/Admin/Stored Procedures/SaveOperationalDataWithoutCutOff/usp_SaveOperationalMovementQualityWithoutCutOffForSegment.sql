/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jan-28-2020
-- Updated Date:	Mar-31-2020 (Changed the logic to get the nodes which fall under node startdate/enddate based on startdate&enddate passed as inputs)
-- Updated date: 	Apr-08-2020  -- Added SystemName as per PBI 28962
-- Updated date: 	Apr-22-2020  -- Added EventType Column
-- Updated date: 	Apr-23-2020  -- Removed Distinct to get all the Movement Details
-- Updated date: 	May-05-2020  -- Added new Column "MovementTransactionId", changed Join Criteria and added temp table
-- Updated date:    Jun-15-2020  -- Added AttributeId column as part of #31874
--                               -- Added batchid column as part of #31874
--                               -- Modified sorting order as part of #31874
                                 -- Updated scenarioid = 1 as part of #31874
								 -- Modified ValueAttributeUnit logic as part of #31874
-- Updated date:    Jun-26-2020  -- AttributeId homologated to categoryElement table. (AttributeId = ElementName)
								 -- Added condition categoryid = 20 for generating attribute name
								 -- SELECT  ROW_NUMBER() OVER (ORDER BY MovementId , [CalculationDate] ASC) AS RNo 
-- Updated date:    Jul-24-2020  -- Added NoLock
-- Updated date:    Aug-04-2020  -- Changed input paramters from STRING to INTEGER AND removed delete old data
-- Updated date:    Aug-07-2020  -- Removing unrequired columns and conditions
-- Description:     This Procedure is to Get TimeOne Movement Quality Details Data for Segment Category, Element, Node, StartDate, EndDate.
-- EXEC [Admin].[usp_SaveOperationalMovementQualityWithoutCutOffForSegment] 'Segmento','Transporte Test','GALAN','2020-04-17','2020-04-17','49CA1512-8ACD-4105-9271-01648C1155CC'
-- EXEC [Admin].[usp_SaveOperationalMovementQualityWithoutCutOffForSegment] 'Segmento','Transporte Test','TODOS','2020-04-17','2020-04-17','49CA1512-8ACD-4105-9271-01648C1155CC'
   SELECT * FROM [Admin].[OperationalMovementQuality] WHERE InputCategory = 'Segmento' AND ExecutionId = '49CA1512-8ACD-4105-9271-01648C1155CC' 
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveOperationalMovementQualityWithoutCutOffForSegment]
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


			INSERT INTO [Admin].[OperationalMovementQuality]
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
					,[Movement]
					,[PercentStandardUnCertainty]
					,[Uncertainty]
					,[AttributeId]
					,[AttributeValue]
					,[ValueAttributeUnit]
					,[AttributeDescription]
					,[ProductId]
					,[ExecutionId]
					,[CreatedDate]
					,[CreatedBy]
			   )
			SELECT  ROW_NUMBER() OVER (ORDER BY MovementId , [CalculationDate] ASC) AS RNo 
				  ,OM.[BatchId]
				  ,OM.[MovementId]
				  ,OM.[MovementTransactionId]
				  ,OM.[CalculationDate]
				  ,OM.[MovementTypeName]
				  ,OM.[SourceNode]
				  ,OM.[DestinationNode]
				  ,OM.[SourceProduct]
				  ,OM.[DestinationProduct]
				  ,OM.[NetStandardVolume]
				  ,OM.[GrossStandardVolume]
				  ,OM.[MeasurementUnit]
				  ,OM.[EventType]
				  ,OM.[SystemName]
				  ,OM.[Movement]
				  ,OM.[PercentStandardUnCertainty]
				  ,ISNULL([Uncertainty],0)  AS [Uncertainty]
				  ,[AttributeId].[Name]		AS [AttributeId]
				  ,Att.[AttributeValue]
				  ,ValAttUnit.[Name]		AS [ValueAttributeUnit]
				  ,Att.[AttributeDescription]
				  ,OM.[ProductID]
				  ,OM.[ExecutionId]  
				  ,OM.[CreatedDate]
				  ,OM.[CreatedBy]				  
			FROM [Admin].[OperationalMovement](NOLOCK) OM
			INNER JOIN [Admin].[Attribute] Att
			ON Att.MovementTransactionId = OM.MovementTransactionId
			INNER JOIN [Admin].[CategoryElement] ValAttUnit
            ON Att.ValueAttributeUnit = ValAttUnit.ElementId
			INNER JOIN [Admin].[CategoryElement] AttributeId
			ON AttributeId.ElementId = Att.AttributeId
			WHERE OM.[ExecutionId]    		= @ExecutionId
			AND	  AttributeId.CategoryId    = 20
END
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne Movement Quality Details Data for Segment Category, Element, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveOperationalMovementQualityWithoutCutOffForSegment',
    @level2type = NULL,
    @level2name = NULL
