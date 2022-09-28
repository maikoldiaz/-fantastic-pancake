--Scenario1:
--Segment Id: 55637
--I am executing cutoff 24 to 25
--I have Node 1 & Node 2--> Inventory with owners on 24th
--And we have created a movement  on 25th for these nodes(Node1 , Node2)

--Inputs:
DECLARE
            @SegmentId          INT				= 55637,
			@StartDate          DATETIME		= '2020-05-24',
			@EndDate	        DATETIME	    = '2020-05-25'

EXEC [Admin].[usp_ValidateInitialInventoriesForOwnership] @SegmentId
													     ,@StartDate
													     ,@EndDate
GO

--Output
--Node Name			Inventory Date		Type
--Automation_7v9wr	2020-05-24			2
--Automation_v0low	2020-05-24			2
