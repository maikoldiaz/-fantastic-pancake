/* ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jun-16-2020
-- Updated date:    Jun-26-2020  -- Removing Ownership join coz that join is not required to bring Owner details
-- Updated date:    Jul-07-2020  -- Modified Ownershippercentage formula
-- Updated date:    Jul-24-2020  -- Added NoLock
-- Updated date:    Aug-04-2020  -- Changed input paramters from STRING to INTEGER AND removed delete old data
-- Updated date:    Aug-07-2020  -- Removing unrequired columns and conditions
-- Updated date: 	Sep-18-2020  -- Changing Percentage Calculation
-- Description:     This Procedure is to Get TimeOne Movement Owner Details Data for Segment Category, Element, Node, StartDate, EndDate.
-- EXEC [Admin].[usp_SaveOperationalMovementOwnerWithoutCutOffForSegment] 'Segmento','Transporte Test','GALAN','2020-04-17','2020-04-17','49CA1512-8ACD-4105-9271-01648C1155CC'
-- EXEC [Admin].[usp_SaveOperationalMovementOwnerWithoutCutOffForSegment] 'Segmento','Automation_8srjq','Todos','2020-06-01','2020-06-10','9e67b8af-3d22-400f-bdb5-85a41a7a245a'
   SELECT * FROM [Admin].[OperationalMovementOwner] WHERE InputCategory = 'Segmento' AND ExecutionId = '49CA1512-8ACD-4105-9271-01648C1155CC' 
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveOperationalMovementOwnerWithoutCutOffForSegment]
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


       INSERT INTO [Admin].[OperationalMovementOwner]
			       (
			         RNo
                    ,MovementId
                    ,BatchId
					,MovementTransactionId
                    ,CalculationDate
                    ,TypeMovement
                    ,SourceNode
                    ,DestinationNode
                    ,SourceProduct
                    ,DestinationProduct
                    ,NetQuantity
                    ,GrossQuantity
                    ,MeasurementUnit
                    ,EventType
                    ,SystemName
                    ,[Owner]
                    ,Ownershipvolume
                    ,Ownershippercentage
                    ,ProductID
                    ,ExecutionId
                    ,CreatedBy
                    ,CreatedDate
                   )
			 SELECT ROW_NUMBER() OVER (ORDER BY MovementId,CalculationDate ASC) AS RNo
	               ,*
             FROM (
                     SELECT DISTINCT OM.MovementId
                      ,OM.BatchId
					  ,OM.MovementTransactionId
                      ,OM.CalculationDate
                      ,OM.MovementTypeName AS TypeMoment
                      ,OM.SourceNode
                      ,OM.DestinationNode
                      ,OM.SourceProduct
                      ,OM.DestinationProduct
                      ,OM.NetStandardVolume
                      ,OM.GrossStandardVolume
                      ,OM.MeasurementUnit
                      ,OM.EventType
                      ,OM.SystemName
			          ,Own.[Name] AS [Owner]
			          ,CASE WHEN  CHARINDEX('%',OwnershipValueUnit) > 0
			                THEN (OM.NetStandardVolume * O.OwnershipValue) /100
			                ELSE O.OwnershipValue
			          END AS OwnershipVolume
                      ,CASE WHEN OM.NetStandardVolume = 0 
					        THEN 0
							WHEN CHARINDEX('%',OwnershipValueUnit) > 0
			                THEN O.OwnershipValue
			                ELSE (O.OwnershipValue / OM.NetStandardVolume) * 100	 
			          END AS OwnershipPercentage
                      ,OM.ProductID
                      ,@ExecutionId AS ExecutionId
                      ,'ReportUser' AS CreatedBy
                      ,@Todaysdate AS CreatedDate
                     FROM [Admin].[OperationalMovement](NOLOCK)  OM
                     INNER JOIN [Offchain].[Owner] O
                     ON  OM.MovementTransactionId  = O.MovementTransactionId
					 AND OM.[ExecutionId]    	   = @ExecutionId
	                 INNER JOIN [Admin].[CategoryElement] Own
                     ON Own.ElementId = O.OwnerId
			 )SubQ			
END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne Movement Owner Details Data for Segment Category, Element, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveOperationalMovementOwnerWithoutCutOffForSegment',
    @level2type = NULL,
    @level2name = NULL