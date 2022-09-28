--Scenario which Fails Scenario(inputOwnershipOfInitialNodes) and returns Error Message with information(MovementId,NodeName i.e DestinationNodeName)
DECLARE	
	   @SegmentId		INT			=	7230,
       @StartDate		DATETIME	=	'2019-12-01',
       @EndDate			DATETIME    = 	'2019-12-03'

EXEC [Admin].[usp_ValidateOwnershipInputs] @SegmentId
										  ,@StartDate
										  ,@EndDate
GO