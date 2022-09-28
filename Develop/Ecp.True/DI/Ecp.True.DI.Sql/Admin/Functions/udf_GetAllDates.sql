/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	Dec-18-2019
-- Updated Date:	Mar-20-2020
-- <Description>:   This function takes two dates and generates all the dates between them. </Description>
-- ==============================================================================================================================*/

CREATE FUNCTION Admin.udf_GetAllDates(@startDate date, @endDate date, @nodeId Int, @elementId NVARCHAR(250))
RETURNS @dates table (nodeId Int, elementId NVARCHAR(250), dates Date)
AS
BEGIN
IF (@startDate = @endDate)
	BEGIN
		Insert Into @dates
		select @nodeId, @elementId, @startDate;		
	END
ELSE
	BEGIN
		with dateRange as
		(
		  select dt = @startDate
		  where dateadd(dd, 1, @startDate) <= @endDate
		  union all
		  select dateadd(dd, 1, dt)
		  from dateRange
		  where dateadd(dd, 1, dt) <= @endDate
		)
		Insert Into @dates
		select @nodeId, @elementId, * from dateRange;		
	END
	RETURN;
END;

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This function takes two dates and generates all the dates between them.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'FUNCTION',
    @level1name = N'udf_GetAllDates',
    @level2type = NULL,
    @level2name = NULL