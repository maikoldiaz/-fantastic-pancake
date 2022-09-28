--Scenario which Passes Rules(nodesWithStrategy,nodesWithOwnershipRules,connectionProductWithStrategy,connectionProductWithPriority, activeOwnershipStrategy, nodeOwnershipCouldBeFound)
--TablesUsed for Rules
--SELECT * FROM [Admin].[NodeOwnershipRule] NdRule

--SELECT * FROM ADmin.Node
--WHERE NodeOwnershipRuleId = 1

--SELECT * FROM [Admin].[NodeProductRule] NpRule

--SELECT * FROM Admin.[NodeConnectionProductRule] NCPrdRule

--select * from admin.nodeconnection where IsDeleted=1

DECLARE	
	    @SegmentId							INT						= 139741
       ,@StartDate							DATETIME				= '2020-01-08'
       ,@EndDate							DATETIME				= '2020-01-08'
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




--Name									Result		ErrorMessage						DisplayOrder
--chainWithInitialNodes					0			NULL								1
--chainWithFinalNodes					0			NULL								2
--nodesWithStrategy						0			{"Name":"Automation_i6ilj"}			3
--nodesWithOwnershipRules				0			{"Name":"Automation_20n2b"}			4
--connectionProductWithStrategy			1			NULL								5
--connectionProductWithPriority			1			NULL								6
--activeOwnershipStrategy				1			NULL								7
--nodeOwnershipCouldBeFound				0			{"Name":"Automation_20n2b"}			8
--chainWithAnaliticalOperationalInfo	0			NULL								9