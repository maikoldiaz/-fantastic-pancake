--Scenario which Passes Rules(nodesWithStrategy,nodesWithOwnershipRules,connectionProductWithStrategy,connectionProductWithPriority, activeOwnershipStrategy, nodeOwnershipCouldBeFound)
--TablesUsed for Rules
--SELECT * FROM [Admin].[NodeOwnershipRule] NdRule

--SELECT * FROM ADmin.Node
--WHERE NodeOwnershipRuleId = 1

--SELECT * FROM [Admin].[NodeProductRule] NpRule

--SELECT * FROM Admin.[NodeConnectionProductRule] NCPrdRule

SELECT  DISTINCT
		 NT.ElementId
		,ND.Name as NodeName
		,ND.NodeOwnershipRuleId 
		,StartDate
		,EndDate
FROM Admin.NodeTag NT
INNER JOIN Admin.Node ND
ON ND.NodeId = NT.NodeId
WHERE Nd.NodeOwnershipRuleId IS NULL
AND NT.ElementId = 19914

DECLARE	
	    @SegmentId							INT						= 19914
       ,@StartDate							DATETIME				= '2020-03-10'
       ,@EndDate							DATETIME				= '2020-03-12'
	   ,@DtNodeInActiveRules				Admin.[KeyValueType]
	   ,@DtNodeProductInActiveRules			Admin.[KeyValueType]
	   ,@DtConnectionProductInActiveRules	Admin.[KeyValueType]

SELECT TOP 1 
		 @SegmentId = NT.ElementId
		,@StartDate = CAST(StartDate AS DATE)
		,@EndDate   = CAST(StartDate+2 AS DATE)
FROM Admin.NodeTag NT
INNER JOIN Admin.Node ND
ON ND.NodeId = NT.NodeId
WHERE Nd.NodeOwnershipRuleId IS NULL
AND NT.ElementId = 19914

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

EXEC [Admin].[usp_ValidateOwnershipInputs_Test]     @SegmentId
										      ,@StartDate
										      ,@EndDate
											  ,@DtNodeInActiveRules
											  ,@DtNodeProductInActiveRules
											  ,@DtConnectionProductInActiveRules

/*Result
Name								Result	ErrorMessage	DisplayOrder
chainWithInitialNodes				0		NULL			1
chainWithFinalNodes					0		NULL			2
nodesWithStrategy					0		{"Name":"Automation_3stdn"},{"Name":"Automation_3stdn"},{"Name":"Automation_3stdn"}	3
nodesWithOwnershipRules				0		{"Name":"Automation_3stdn"}	4
connectionProductWithStrategy		1		NULL			5
connectionProductWithPriority		1		NULL			6
activeOwnershipStrategy				1		NULL			7
nodeOwnershipCouldBeFound			0		{"Name":"Automation_3stdn"}	8
chainWithAnaliticalOperationalInfo	0		NULL			9
*/