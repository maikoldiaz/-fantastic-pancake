--Scenario To Test Rules(nodeOwnershipCouldBeFound,nodesWithOwnershipRules) Which return Errors
DECLARE	
	   @SegmentId		INT			=	10,
       @StartDate		DATETIME	=	'2019-10-21',
       @EndDate			DATETIME    = 	'2019-10-23'

EXEC [Admin].[usp_ValidateOwnershipInputs] @SegmentId
										  ,@StartDate
										  ,@EndDate
GO