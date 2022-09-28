--Scenario2:
--I am executing cutoff 26 to 27
--I have Node 1 & Node 2--> Inventory without  owners on 26th
--And we have created a movement  on 27th for these nodes(Node1 , Node2)

--Inputs:
DECLARE
            @SegmentId          INT				= 55655,
			@StartDate          DATETIME		= '2020-05-26',
			@EndDate	        DATETIME	    = '2020-05-27'

EXEC [Admin].[usp_ValidateInitialInventoriesForOwnership] @SegmentId
													     ,@StartDate
													     ,@EndDate
GO

--Output
--Node Name			Inventory Date		Type
--Automation_ujsbj	2020-05-27			2
--Automation_zwki0	2020-05-27			2
