﻿--Scenario with End Date('2020-05-31')
DECLARE	@SegmentId		INT			=	53363,
        @StartDate		DATETIME	=	'2020-05-17',
        @EndDate		DATETIME    = 	'2020-05-31'

EXEC [Admin].[usp_ValidateInitialInventoriesForOwnership] @SegmentId
													     ,@StartDate
													     ,@EndDate
GO


/*OutPut*/
--NodeName			InventoryDate	Type
--Automation_6ayqz	2020-05-20		2