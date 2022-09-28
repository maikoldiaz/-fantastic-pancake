-- 1. ******************************************************************************
-- Type = INSERT-Negative
-- Get the Initial Nodes if the data is in window of 45 days
------------------------------------------------------------------------------------

EXEC Admin.usp_GetInitialNodes @SEGMENTID=48613, @STARTDATE='2019-01-25', @ENDDATE='2019-02-25'
