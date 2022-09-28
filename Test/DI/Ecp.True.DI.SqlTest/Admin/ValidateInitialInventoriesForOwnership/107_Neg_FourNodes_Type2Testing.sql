--Scenario4:
--Cutoff already executed for 24 to 25 for Node1 and Node2
--I am executing cutoff 26 to 27
--I have Node 3 & Node 4--> Inventory with owners on 26th
--And we have created a movement  on 27th for these nodes(Node3 , Node4)

--Inputs:
DECLARE
            @SegmentId          INT				= 55673,
			@StartDate          DATETIME		= '2020-05-26',
			@EndDate	        DATETIME	    = '2020-05-27'

EXEC [Admin].[usp_ValidateInitialInventoriesForOwnership] @SegmentId
													     ,@StartDate
													     ,@EndDate
GO

--Output
--Node Name			Inventory Date		Type
--Automation_iakwt	2020-05-26			2
--Automation_j049e	2020-05-26			2
