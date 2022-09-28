/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Aug-10-2020
-- Update date: 	Aug-11-2020 -- Changed condition to identify the executionId before the input date
-- Update date: 	Oct-01-2020 -- Deleting the data for a particular report based on executionId
-- Description:     This Procedure is used to delete the data from Balance official per node related tables.
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_Cleanup_MonthlyOfficialDataWithoutCutOff] 
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
	WHERE [Name] = 'OfficialInitialBalanceReport'
	  AND LastModifiedDate <= DATEADD(HOUR,-@Hour,@TodayDate)

	DELETE FROM [Admin].[OfficialMovementInformation]
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[OfficialNodeTagCalculationDate]
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[OfficialMonthlyMovementDetails]
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[OfficialMonthlyMovementQualityDetails]
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[OfficialMonthlyInventoryDetails]
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[OfficialMonthlyInventoryQualityDetails]
	WHERE ExecutionId IN (SELECT ExecutionId FROM #Temp)

	DELETE FROM [Admin].[OfficialMonthlyBalance]
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
    @value = N'This Procedure is used to delete the data from Balance official per node related tables.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_Cleanup_MonthlyOfficialDataWithoutCutOff',
    @level2type = NULL,
    @level2name = NULL