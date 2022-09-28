--Scenario which passes all the scenarios
DECLARE	
	   @SegmentId		INT			=	13,
       @StartDate		DATETIME	=	'2019-09-02',
       @EndDate			DATETIME    = 	'2019-09-03'

EXEC [Admin].[usp_ValidateOwnershipInputstest] @SegmentId
										      ,@StartDate
										      ,@EndDate


GO
