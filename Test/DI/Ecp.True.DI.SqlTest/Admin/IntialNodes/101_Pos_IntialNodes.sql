-- 1. ******************************************************************************
-- Type = INSERT-Positive
-- Get the Initial Nodes if the data is in window of 45 days
------------------------------------------------------------------------------------

EXEC Admin.usp_GetInitialNodes @SEGMENTID=48613, @STARTDATE='2020-01-25', @ENDDATE='2020-01-29'
