--Scenario which Fails and return message chainWithAnaliticalOperationalInfo
DECLARE	
	   @SegmentId		INT			=	11064,
       @StartDate		DATETIME	=	'2020-01-01',
       @EndDate			DATETIME    = 	'2020-01-11'

EXEC [Admin].[usp_ValidateOwnershipInputstest] @SegmentId
										      ,@StartDate
										      ,@EndDate


GO
