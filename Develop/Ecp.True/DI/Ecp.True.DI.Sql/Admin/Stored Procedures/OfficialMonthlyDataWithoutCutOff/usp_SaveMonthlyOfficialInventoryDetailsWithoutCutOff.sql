/* ============================================================================================================================
-- Author		:		Microsoft
-- Create date	: 		July-10-2020
-- Modified date:		July-13-2020
						Modified logic of Ownership Volume and Ownership Percentage
						Applied DISTINCT
-- Modified date: 	Sep-18-2020  Changing Percentage Calculation
-- Modified date: 	Sep-25-2020 Removed delete statements, CZ deletes are part of Cleanup SP's
-- Description	:		This Procedure is to feed the OfficialMonthlyInventoryDetails table.
						EXEC [Admin].[usp_SaveMonthlyOfficialDataWithoutCutOffReport] 137290,30824,'2020-07-03' ,'2020-07-06' ,'B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EFuser5'
						EXEC [Admin].[usp_SaveMonthlyOfficialInventoryDetailsWithoutCutOff] 137290,30824,'2020-07-03' ,'2020-07-06' ,'B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EFuser5'
						SELECT * FROM  [Admin].[OfficialNodeTagCalculationDate] WHERE ExecutionId = 'B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EFuser5'
						SELECT * FROM  [Admin].[OfficialMonthlyInventoryDetails]  WHERE ExecutionId = 'B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EFuser5'
-- ==============================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_SaveMonthlyOfficialInventoryDetailsWithoutCutOff]
(
                    @ElementId             INT
                   ,@NodeId                INT
                   ,@StartDate             DATE                      
                   ,@EndDate               DATE                      
                   ,@ExecutionId           INT
)
AS 
BEGIN
  SET NOCOUNT ON
			   
    --Variables Declaration
    DECLARE   @PreviousDayOfStartDate       DATE
             ,@Previousdate                 DATE =  [Admin].[udf_GetTrueDate] ()-1
             ,@Todaysdate                   DATE =  [Admin].[udf_GetTrueDate] ()


	 
	 SET @PreviousDayOfStartDate = DATEADD(DAY,-1,@StartDate)

	 INSERT INTO [Admin].[OfficialMonthlyInventoryDetails] 
						(
						[RNo]						
						,[System]                     
						,[Version]                    
						,[InventoryId]				
						,[NodeName]                   
						,[Product]		             
						,[NetStandardVolume]          
						,[GrossStandardQuantity]      
						,[MeasurementUnit]			 
						,[Owner]                      
						,[OwnershipVolume]            
						,[OwnershipPercentage]                       
						,[Origin]                     
						,[RegistrationDate]           
						,[InventoryProductId]		           
						,[ExecutionId]    
						,[CreatedBy]                  
						,[CreatedDate]                
						)
				SELECT ROW_NUMBER() OVER
								(
										ORDER BY [System]
										,[Version]
										,[Product]
										,[Owner]
										ASC
								) Rno
							,* FROM
							(
							SELECT DISTINCT							
																			
							[Inv].[SystemName]								AS [System],
							[Inv].[Version]									AS [Version],					
							[Inv].[InventoryId]								AS [InventoryId],
							[Inv].[NodeName]  								AS [NodeName],
							[Inv].[ProductName]     						AS [Product],
							[Inv].[ProductVolume]							AS [NetStandardVolume], 
							[Inv].[GrossStandardQuantity]                   AS [GrossStandardQuantity],
							[Inv].[MeasurmentUnit]							AS [MeasurementUnit],
							[Own].[Name]									AS [Owner],
							CASE 
							WHEN CHARINDEX('%',OwnershipValueUnit) > 0
							THEN ([Inv].[ProductVolume] * O.OwnershipValue) /100
							ELSE O.OwnershipValue
							END 
																			AS OwnershipVolume,   
							CASE 
							WHEN [Inv].[ProductVolume] = 0
							THEN 0
							WHEN CHARINDEX('%',OwnershipValueUnit) > 0
							THEN O.OwnershipValue
							ELSE (O.OwnershipValue / [Inv].[ProductVolume]) * 100    
							END 
																			AS OwnershipPercentage,  		
							[Inv].[SourceSystem]							AS [Origin],
							[Inv].[InventoryDate]							AS [RegistrationDate],
							[Inv].[InventoryProductId]						AS [InventoryProductId],           
							@ExecutionId									AS [ExecutionId],
							'system'										AS [CreatedBy],
							@Todaysdate										AS [CreatedDate]
			FROM [Admin].[view_InventoryInformation] Inv
			LEFT JOIN [Offchain].[Owner] O
			ON  Inv.InventoryProductId  = O.InventoryProductId
			INNER JOIN [Admin].[CategoryElement] Own -- Owner
			ON Own.ElementId = O.OwnerId
			AND Inv.SegmentId = @ElementId
			AND Inv.InventoryDate BETWEEN @StartDate AND @EndDate
			AND Inv.ProductId IS NOT NULL
			AND Inv.SegmentId IS NOT NULL--Segment Should not be NULL
			AND Inv.SourceSystem != 'TRUE' -- Excluding the inventories where source system is "TRUE'
			AND (Inv.NodeId = @NodeId OR @NodeId IS NULL)
			INNER JOIN [Admin].[OfficialNodeTagCalculationDate] NT
			ON NT.NodeId = Inv.NodeId
			AND NT.ExecutionId		= @ExecutionId
			Where Inv.ScenarioId	= 2
			)Subq

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to feed the OfficialInventoryInformation table',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveMonthlyOfficialInventoryDetailsWithoutCutOff',
    @level2type = NULL,
    @level2name = NULL