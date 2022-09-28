
--Scenario StartDate is Greater than EndDate
DECLARE	
	   @SegmentId		INT			=	10,
       @StartDate		DATETIME	=	'2020-05-31',
       @EndDate			DATETIME    = 	'2020-05-01'

EXEC [Admin].[usp_ValidateInitialInventoriesForOwnership] @SegmentId
													     ,@StartDate
													     ,@EndDate
GO

--Expectation No Output

/*OutPut*/
--NodeName			InventoryDate	Type
