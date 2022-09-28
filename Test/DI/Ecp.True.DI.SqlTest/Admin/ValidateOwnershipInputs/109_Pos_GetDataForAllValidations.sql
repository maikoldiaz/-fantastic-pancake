--Scenario which Passes Rules(nodesWithStrategy,nodesWithOwnershipRules,connectionProductWithStrategy,connectionProductWithPriority, activeOwnershipStrategy, nodeOwnershipCouldBeFound)
--TablesUsed for Rules
--SELECT * FROM [Admin].[NodeOwnershipRule] NdRule

--SELECT * FROM ADmin.Node
--WHERE NodeOwnershipRuleId = 1

--SELECT * FROM [Admin].[NodeProductRule] NpRule

--SELECT * FROM Admin.[NodeConnectionProductRule] NCPrdRule

DECLARE	
	    @SegmentId							INT						= 7217
       ,@StartDate							DATETIME				= '2020-01-07'
       ,@EndDate							DATETIME				= '2020-01-11'
	   ,@DtNodeInActiveRules				Admin.[KeyValueType]
	   ,@DtNodeProductInActiveRules			Admin.[KeyValueType]
	   ,@DtConnectionProductInActiveRules	Admin.[KeyValueType]

INSERT INTO @DtNodeInActiveRules 
(
	 [Key]	
	,[Value]
)
VALUES(1,'S-Disponsible')

INSERT INTO @DtNodeProductInActiveRules 
(
	 [Key]	
	,[Value]
)
VALUES(1,'Salidas')

INSERT INTO @DtConnectionProductInActiveRules 
(
	 [Key]	
	,[Value]
)
VALUES(1,'Entradas')

EXEC [Admin].[usp_ValidateOwnershipInputs]     @SegmentId
										      ,@StartDate
										      ,@EndDate
											  ,@DtNodeInActiveRules
											  ,@DtNodeProductInActiveRules
											  ,@DtConnectionProductInActiveRules


