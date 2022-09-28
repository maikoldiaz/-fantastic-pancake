/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Aug-10-2020
-- Update date: 	Aug-11-2020 -- Changed condition to identify the executionId before the input date
-- Update date: 	Oct-01-2020 -- Deleting the data for a particular report based on executionId
-- Description:     This Procedure is used to delete the data from Before Cut Off related tables.
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_Cleanup_OperationalDataWithoutCutOff] 
(
 @Hour INT
)
AS
BEGIN
    DECLARE  @TodayDate			DATETIME = [Admin].[udf_GetTrueDate] ()
			,@MaxExecutionId	INT
	
    SELECT ExecutionId 
	INTO #Temp 
	FROM  [Admin].[ReportExecution] 
	WHERE [Name] = 'WithoutCutoff'
	  AND LastModifiedDate <= DATEADD(HOUR,-@Hour,@TodayDate)
	

	DELETE FROM [Admin].[MovementInformationMovforSegmentReport] 
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[MovementInformationMovforSystemReport] 
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[NodeTagCalculationDateForSegmentReport] 
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[NodeTagCalculationDateForSystemReport] 
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[OperationalInventory] 
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[OperationalInventoryOwner] 
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[OperationalInventoryQuality] 
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[OperationalMovement] 
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[OperationalMovementOwner] 
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[OperationalMovementQuality] 
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[Operational] 
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[ReportExecution] 
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	SELECT @MaxExecutionId = MAX(ExecutionId)
	FROM [Admin].[ReportExecution]
	
	IF @MaxExecutionId = 9999999
	BEGIN
		DBCC CHECKIDENT ('Admin.ReportExecution', RESEED, 1000000)
	END	
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to delete the data from Before Cut Off related tables.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_Cleanup_OperationalDataWithoutCutOff',
    @level2type = NULL,
    @level2name = NULL