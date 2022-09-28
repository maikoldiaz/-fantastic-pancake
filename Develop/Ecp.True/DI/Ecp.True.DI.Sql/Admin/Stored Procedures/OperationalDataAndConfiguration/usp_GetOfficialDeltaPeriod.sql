/*-- =============================================================================================================================================================================
-- Author:          Microsoft
-- Created Date:	July-08-2020
-- Updated Date:	July-09-2020 -- Added @NoOfYears parameter and removed comma delimited logic
-- Updated Date:	July-14-2020 -- Updated SP for showing 0 MonthInfo for non-existing YearInfo records
-- Updated Date:	July-15-2020 -- Renamed SP from usp_GetOfficialMovementPeriod name, Updated SP for getting delta ticket periods for node report and return periods of all segments if SegmentId=0
-- <Description>:	This Procedure is used to get official delta period (month, year) in the last N years. </Description>
-- ==============================================================================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_GetOfficialDeltaPeriod] 
(
		 @SegmentId			INT
		,@NoOfYears			INT
		,@IsPerNodeReport	BIT = 0
)
AS 
BEGIN

    IF OBJECT_ID('tempdb..#TempYearInfo')IS NOT NULL
	DROP TABLE #TempYearInfo

    DECLARE @FirstDayOfInput   DATETIME,
	        @LastDayOfInput	   DATETIME,
            @Today             DATETIME,
			@TempYearInfoVal   INT,
			@FirstYear         INT,
			@LastYear          INT,
			@CurrentYear	   INT

	
    SET @Today = Admin.udf_GetTrueDate()
	SELECT @CurrentYear = DATEDIFF(yy, 0, @Today)
    SELECT @FirstDayOfInput = DATEADD(yy, @CurrentYear - (@NoOfYears-1), 0)
    SELECT @LastDayOfInput = DATEADD (dd, -1, DATEADD(yy, @CurrentYear +1, 0))

	SET @FirstYear = YEAR(@FirstDayOfInput)
    SET @LastYear = YEAR(@LastDayOfInput)
	SET @TempYearInfoVal = @FirstYear

	CREATE TABLE #TempYearInfo
    (
        YearInfo     INT
    )
	
	WHILE @TempYearInfoVal <= @LastYear
    BEGIN
		INSERT INTO #TempYearInfo VALUES (@TempYearInfoVal)
		SET @TempYearInfoVal = @TempYearInfoVal + 1
    END

	IF OBJECT_ID('tempdb..#PeriodSegmentInfo') IS NOT NULL
		DROP TABLE #PeriodSegmentInfo

	CREATE TABLE #PeriodSegmentInfo ( SegmentId INT )

	IF @SegmentId =0
	BEGIN
		INSERT INTO #PeriodSegmentInfo ( SegmentId )
		SELECT ElementId
		FROM Admin.CategoryElement
		WHERE CategoryId = 2 AND IsActive = 1
	END
	ELSE
	BEGIN
		INSERT INTO #PeriodSegmentInfo ( SegmentId ) SELECT  @SegmentId
	END

	IF @IsPerNodeReport = 0
	BEGIN
		SELECT  Tyi.YearInfo AS [YearInfo]
			   ,CASE WHEN A.MonthInfo IS NULL THEN 0 ELSE A.MonthInfo END AS [MonthInfo]
		FROM (
			SELECT DISTINCT YEAR(MovPrd.StartTime) AS [YearInfo]
						   ,MONTH(MovPrd.StartTime) AS [MonthInfo]
			FROM Offchain.Movement Mov
			INNER JOIN Offchain.MovementPeriod MovPrd
			ON Mov.MovementTransactionId = MovPrd.MovementTransactionId
			WHERE Mov.SegmentId IN (SELECT SegmentId FROM #PeriodSegmentInfo)
			AND Mov.ScenarioId = 2
			AND MovPrd.StartTime >= @FirstDayOfInput
			AND MovPrd.EndTime <= @LastDayOfInput
		) A
		RIGHT JOIN #TempYearInfo Tyi
		ON Tyi.YearInfo = A.YearInfo
	END
	ELSE
	BEGIN
		SELECT  Tyi.YearInfo AS [YearInfo]
			   ,CASE WHEN A.MonthInfo IS NULL THEN 0 ELSE A.MonthInfo END AS [MonthInfo]
		FROM (
			SELECT DISTINCT YEAR(Tic.StartDate) AS [YearInfo]
						   ,MONTH(Tic.EndDate) AS [MonthInfo]
			FROM Admin.Ticket Tic
			WHERE Tic.CategoryElementId IN (SELECT SegmentId FROM #PeriodSegmentInfo)
			AND Tic.[Status] = 4
			AND Tic.TicketTypeId=5
			AND Tic.StartDate >= @FirstDayOfInput
			AND Tic.EndDate <= @LastDayOfInput
		) A
		RIGHT JOIN #TempYearInfo Tyi
		ON Tyi.YearInfo = A.YearInfo
	END
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get official delta period (month, year) in the last N years.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetOfficialDeltaPeriod',
    @level2type = NULL,
    @level2name = NULL