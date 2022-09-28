--Scenario5:
--I have Node 1 & Node 2--> Inventory with owners on 24th And Node3 & Node 4 having inventory on 26th
--And we have created a movement  on 27th for these nodes(Node1 , Node2, Node 3 & Node4)

--Inputs:
DECLARE
            @SegmentId          INT				= 55839,
			@StartDate          DATETIME		= '2020-05-25',
			@EndDate	        DATETIME	    = '2020-05-28'

EXEC [Admin].[usp_ValidateInitialInventoriesForOwnership] @SegmentId
													     ,@StartDate
													     ,@EndDate
GO

--Output
--Node Name			Inventory Date		Type
--Automation_awhwp	2020-05-26			2
--Automation_n3n15	2020-05-26			2

----------------------------------------------
---------OtherDateRanges

--Inputs:
DECLARE
            @SegmentId          INT				= 55839,
			@StartDate          DATETIME		= '2020-05-27',
			@EndDate	        DATETIME	    = '2020-05-28'

EXEC [Admin].[usp_ValidateInitialInventoriesForOwnership] @SegmentId
													     ,@StartDate
													     ,@EndDate
GO

--Output
--Node Name			Inventory Date		Type
--Automation_9yud0	2020-05-28			2
--Automation_wz68	2020-05-28			2
