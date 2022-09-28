--Scenario with End Date(9999-12-31')
DECLARE	
	   @SegmentId		INT			=	2,
       @StartDate		DATETIME	=	'2019-10-22',
       @EndDate			DATETIME    = 	'9999-12-31'

EXEC [Admin].[usp_ValidateOwnershipInputs] @SegmentId
										  ,@StartDate
										  ,@EndDate
GO