/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Aug-07-2020
-- Update date: 	Aug-11-2020 -- Changed condition to identify the executionId before the input date
-- Description:     This Procedure is to delete data from NonSon Segment tables 
-- [Admin].[usp_Cleanup_NonSonSegmentDataWithoutCutOff] 1
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_Cleanup_NonSonSegmentDataWithoutCutOff]
(
@Hour INT
)
AS
BEGIN
    DECLARE  @TodayDate			DATETIME = [Admin].[udf_GetTrueDate] ()
			,@MaxExecutionId	INT
    
    
    IF OBJECT_ID('tempdb..#TempDeleted')IS NOT NULL
    DROP TABLE #TempDeleted
    
    SELECT ExecutionId 
    INTO #TempDeleted 
    FROM [Admin].[ReportExecution] 
    WHERE [Name] = 'NonSonWithOwnerReport'
      AND LastModifiedDate <= DATEADD(HOUR,-@Hour,@TodayDate)
   
    DELETE FROM [Admin].[OperationalNonSon]
    WHERE ExecutionId IN (SELECT ExecutionId FROM #TempDeleted)
    
    DELETE FROM [Admin].[OperationalMovementQualityNonSon]
    WHERE ExecutionId IN (SELECT ExecutionId FROM #TempDeleted)
    
    DELETE FROM [Admin].[OperationalMovementOwnerNonSon]
    WHERE ExecutionId IN (SELECT ExecutionId FROM #TempDeleted)
    
    DELETE FROM [Admin].[OperationalInventoryQualityNonSon]
    WHERE ExecutionId IN (SELECT ExecutionId FROM #TempDeleted)
    
    DELETE FROM [Admin].[OperationalInventoryOwnerNonSon]
    WHERE ExecutionId IN (SELECT ExecutionId FROM #TempDeleted)

	DELETE FROM [Admin].[ReportExecution] 
	WHERE ExecutionId IN (SELECT ExecutionId FROM #TempDeleted)

	SELECT @MaxExecutionId = MAX(ExecutionId)
	FROM [Admin].[ReportExecution]
	
	IF @MaxExecutionId = 9999999
	BEGIN
		DBCC CHECKIDENT ('Admin.ReportExecution', RESEED, 1000000)
	END	
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to delete data from tables for NON SONS Segment report .',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_Cleanup_NonSonSegmentDataWithoutCutOff',
    @level2type = NULL,
    @level2name = NULL