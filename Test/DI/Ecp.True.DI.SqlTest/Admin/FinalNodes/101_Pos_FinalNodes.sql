-- 1. ******************************************************************************
-- Type = INSERT-Positive
-- Get the Final Nodes if the data is in window of 45 days
------------------------------------------------------------------------------------



EXEC Admin.usp_GetFinalNodes @SEGMENTID=48613, @STARTDATE='2020-01-25', @ENDDATE='2020-01-29'