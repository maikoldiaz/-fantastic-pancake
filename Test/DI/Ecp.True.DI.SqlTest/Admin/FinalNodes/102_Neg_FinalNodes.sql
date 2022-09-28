 -- 1. ******************************************************************************
-- Type = INSERT-Negative
-- Get the Final Nodes if the data is in window of 45 days
------------------------------------------------------------------------------------



EXEC Admin.usp_GetFinalNodes@SEGMENTID=48613, @STARTDATE='2019-01-25', @ENDDATE='2019-02-25'