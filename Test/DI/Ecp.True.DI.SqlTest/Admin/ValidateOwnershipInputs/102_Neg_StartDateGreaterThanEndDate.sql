--Scenario StartDate is Greater than EndDate
DECLARE	
	   @SegmentId		INT			=	10,
       @StartDate		DATETIME	=	'2019-10-30',
       @EndDate			DATETIME    = 	'2019-10-14'

EXEC [Admin].[usp_ValidateOwnershipInputs] @SegmentId
										  ,@StartDate
										  ,@EndDate
GO