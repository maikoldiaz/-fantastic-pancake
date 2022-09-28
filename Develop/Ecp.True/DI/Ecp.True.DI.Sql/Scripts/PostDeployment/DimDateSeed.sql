/*
 Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF	OBJECT_ID('Admin.DimDate') IS NOT NULL
BEGIN
    DECLARE  @StartDate DATE ='2019-01-01',
             @EndDate DATE = '2050-12-31'

        
    TRUNCATE TABLE [Admin].[DimDate]

 

    WHILE @StartDate <= @EndDate
        BEGIN
            INSERT INTO [Admin].[DimDate] (
                                            [DateKey],
                                            [Date],
                                            [Day],
                                            [DaySuffix],
                                            [Weekday],
                                            [WeekDayName],
                                            [WeekDayName_Short],
                                            [WeekDayName_FirstLetter],
                                            [DOWInMonth],
                                            [DayOfYear],
                                            [WeekOfMonth],
                                            [WeekOfYear],
                                            [Month],
                                            [MonthName],
                                            [MonthName_Short],
                                            [MonthName_FirstLetter],
                                            [Quarter],
                                            [QuarterName],
                                            [Year],
                                            [MMYYYY],
                                            [MonthYear],
                                            [IsWeekend],
                                            [IsHoliday]
                                            )
            SELECT DateKey = YEAR(@StartDate) * 10000 + MONTH(@StartDate) * 100 + DAY(@StartDate),
            DATE = @StartDate,
            Day = DAY(@StartDate),
            [DaySuffix] = CASE 
                            WHEN DAY(@StartDate) = 1
                            OR DAY(@StartDate) = 21
                            OR DAY(@StartDate) = 31
                            THEN 'st'
                            WHEN DAY(@StartDate) = 2
                            OR DAY(@StartDate) = 22
                            THEN 'nd'
                            WHEN DAY(@StartDate) = 3
                            OR DAY(@StartDate) = 23
                            THEN 'rd'
                            ELSE 'th'
                            END,
            WEEKDAY = DATEPART(dw, @StartDate),
            WeekDayName = DATENAME(dw, @StartDate),
            WeekDayName_Short = UPPER(LEFT(DATENAME(dw, @StartDate), 3)),
            WeekDayName_FirstLetter = LEFT(DATENAME(dw, @StartDate), 1),
            [DOWInMonth] = DAY(@StartDate),
            [DayOfYear] = DATENAME(dy, @StartDate),
            [WeekOfMonth] = DATEPART(WEEK, @StartDate) - DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM, 0, @StartDate), 0)) + 1,
            [WeekOfYear] = DATEPART(wk, @StartDate),
            [Month] = MONTH(@StartDate),
            [MonthName] = DATENAME(mm, @StartDate),
            [MonthName_Short] = UPPER(LEFT(DATENAME(mm, @StartDate), 3)),
            [MonthName_FirstLetter] = LEFT(DATENAME(mm, @StartDate), 1),
            [Quarter] = DATEPART(q, @StartDate),
            [QuarterName] = CASE 
                            WHEN DATENAME(qq, @StartDate) = 1
                                THEN 'First'
                            WHEN DATENAME(qq, @StartDate) = 2
                                THEN 'second'
                            WHEN DATENAME(qq, @StartDate) = 3
                                THEN 'third'
                            WHEN DATENAME(qq, @StartDate) = 4
                                THEN 'fourth'
                            END,
            [Year] = YEAR(@StartDate),
            [MMYYYY] = RIGHT('0' + CAST(MONTH(@StartDate) AS VARCHAR(2)), 2) + CAST(YEAR(@StartDate) AS VARCHAR(4)),
            [MonthYear] = CAST(YEAR(@StartDate) AS VARCHAR(4)) + UPPER(LEFT(DATENAME(mm, @StartDate), 3)),
            [IsWeekend] = CASE 
                            WHEN DATENAME(dw, @StartDate) = 'Sunday'
                                OR DATENAME(dw, @StartDate) = 'Saturday'
                                THEN 1
                            ELSE 0
                            END,
            [IsHoliday] = 0

 

        SET @StartDate = DATEADD(DD, 1, @StartDate)
    END
END