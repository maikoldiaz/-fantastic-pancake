/*==============================================================================================================================
--Author:        Microsoft
--Created Date : Jan-09-2020
--Updated Date : Mar-20-2020
--<Description>: This table holds the dates (all consecutive dates of a specific hardcoded range) to show on report header in Power Bi reports. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].DimDate (
   DateKey                   INT NOT NULL PRIMARY KEY,
   [Date]                    DATE NOT NULL,
   [Day]                     TINYINT NOT NULL,
   [DaySuffix]               CHAR(2) NOT NULL,
   [Weekday]                 TINYINT NOT NULL,
   [WeekDayName]             VARCHAR(10) NOT NULL,
   [WeekDayName_Short]       CHAR(3) NOT NULL,
   [WeekDayName_FirstLetter] CHAR(1) NOT NULL,
   [DOWInMonth]              TINYINT NOT NULL,
   [DayOfYear]               SMALLINT NOT NULL,
   [WeekOfMonth]             TINYINT NOT NULL,
   [WeekOfYear]              TINYINT NOT NULL,
   [Month]                   TINYINT NOT NULL,
   [MonthName]               VARCHAR(10) NOT NULL,
   [MonthName_Short]         CHAR(3) NOT NULL,
   [MonthName_FirstLetter]   CHAR(1) NOT NULL,
   [Quarter]                 TINYINT NOT NULL,
   [QuarterName]             VARCHAR(6) NOT NULL,
   [Year]                    INT NOT NULL,
   [MMYYYY]                  CHAR(6) NOT NULL,
   [MonthYear]               CHAR(7) NOT NULL,
   IsWeekend                 BIT NOT NULL,
   [IsHoliday]               BIT NOT NULL
   )

 

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the dates (all consecutive dates of a specific hardcoded range) to show on report header in Power Bi reports.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date (eg: 01/31/2019 12:00:00 AM)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'Date'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The day position in the monthly calendar (31 for 31st december)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'Day'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The day position in the weekly calendar (if Sunday, then 1)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'Weekday'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the weekday (Sunday)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'WeekDayName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The short of the weekday name (Sun for Sunday)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'WeekDayName_Short'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The firstletter of the weekday name (S for Sunday)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'WeekDayName_FirstLetter'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The day position in the yearly calendar (31st december 2019 will be 365)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'DayOfYear'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The week position in the monthly calendar (1 for 1st week of the month)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'WeekOfMonth'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The week position in the yearly calendar (12 for 12th week of the year)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'WeekOfYear'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The month number in the year (12 for december)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'Month'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The month name',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'MonthName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The short of the month name (Jan for January)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'MonthName_Short'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The firstletter of the month name (J for January)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'MonthName_FirstLetter'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The quarter number in the year (1 for 1st quarter)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'Quarter'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The quarter by name (First for 1st quarter)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'QuarterName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The year (2019 for the year of 2019)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'Year'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date in the format of 012019 for january 2019',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'MMYYYY'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if it is a weekend, 1 for yes',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'IsWeekend'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if it is a holiday, 1 for yes',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'IsHoliday'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date in form of YYYYDD (20190131)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'DateKey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The suffix for "Day" column (st for 1st, nd for 2nd, etc.)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'DaySuffix'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The day position in the monthly calendar (31 for 31st day in month of december)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'DOWInMonth'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date in the format of 2019JAN ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DimDate',
    @level2type = N'COLUMN',
    @level2name = N'MonthYear'