-- 1. ******************************************************************************
-- GetAllNodesSP - This SP generates the list of eligible nodes for each date in the given date range.
--	Input to SP -> CategoryElement, StartDate, EndDate, IsInterface (optional), IsDebug (Optional).
------------------------------------------------------------------------------------

-- Note: these parameters data is for dev.

Declare
@catElementId INT = 10,
@startDate date = '2020-02-28',
@endDate date = '2020-03-04'


-- With IsInterface in debug Mode
EXEC [Admin].[usp_GetAllNodesTest] @catElementId, @startDate, @endDate, @isInterface = 1, @isDebug = 1


-- Without IsInterface in debug Mode
EXEC [Admin].[usp_GetAllNodesTest] @catElementId, @startDate, @endDate, @isInterface = 0, @isDebug = 1

