--Scenario which Fails(nodesWithStrategy, connectionProductWithStrategy, connectionProductWithPriority)
--TablesUsed for Rules
--SELECT * FROM [Admin].[NodeOwnershipRule] NdRule

--SELECT * FROM ADmin.Node
--WHERE NodeOwnershipRuleId = 1

--SELECT * FROM [Admin].[NodeProductRule] NpRule

--SELECT * FROM Admin.[NodeConnectionProductRule] NCPrdRule

			--SELECT TOP 1 
			--		NC.SourceNodeId,
			--	   '' AS SourceNodeName,
			--	   NC.DestinationNodeId,
			--	   '' AS DestinationNodeName,
			--	   NCP.ProductId,
			--	   NCP.[NodeConnectionProductRuleId],
			--	   NCP.Priority,
			--	   NCPrdRule.RuleName AS NodeConnectionProductRuleName,
			--	   Ndseg.StartDate,
			--	   NdSeg.EndDate,
			--	   NdSeg.ElementId
			----INTO #TempconnectionProductWithStrategy
			--FROM Admin.NodeTag NdSeg
			--INNER JOIN Admin.NodeConnection NC
			--ON (NdSeg.NodeId = NC.SourceNodeId
			--	OR NdSeg.NodeId = NC.DestinationNodeId)
			--INNER JOIN Admin.NodeConnectionProduct NCP
			--ON NC.NodeConnectionId = NCP.NodeConnectionId
			--LEFT JOIN Admin.[NodeConnectionProductRule] NCPrdRule
			--ON NCP.[NodeConnectionProductRuleId] = NCPrdRule.RuleId
			--WHERE NdSeg.ElementId = 19753

DECLARE	
	    @SegmentId							INT						= 19914
       ,@StartDate							DATETIME				= '2020-03-10'
       ,@EndDate							DATETIME				= '2020-03-12'
	   ,@DtNodeInActiveRules				Admin.[KeyValueType]
	   ,@DtNodeProductInActiveRules			Admin.[KeyValueType]
	   ,@DtConnectionProductInActiveRules	Admin.[KeyValueType]

SELECT TOP 1 
		 @SegmentId = NdSeg.ElementId
		,@StartDate = CAST(NdSeg.StartDate AS DATE)
		,@EndDate   = CAST(NdSeg.StartDate+2 AS DATE)
FROM Admin.NodeTag NdSeg
INNER JOIN Admin.NodeConnection NC
ON (NdSeg.NodeId = NC.SourceNodeId
	OR NdSeg.NodeId = NC.DestinationNodeId)
INNER JOIN Admin.NodeConnectionProduct NCP
ON NC.NodeConnectionId = NCP.NodeConnectionId
LEFT JOIN Admin.[NodeConnectionProductRule] NCPrdRule
ON NCP.[NodeConnectionProductRuleId] = NCPrdRule.RuleId
WHERE NdSeg.ElementId = 19753

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

EXEC [Admin].[usp_ValidateOwnershipInputs_Test]      @SegmentId
													,@StartDate
													,@EndDate
													,@DtNodeInActiveRules
													,@DtNodeProductInActiveRules
													,@DtConnectionProductInActiveRules

--Failed (nodesWithStrategy, connectionProductWithStrategy, connectionProductWithPriority)(3,5,6)
/*
Name									Result	ErrorMessage																		DisplayOrder
chainWithInitialNodes					1		NULL																				1
chainWithFinalNodes						0		NULL																				2
nodesWithStrategy						0		{"Name":"Automation_a9lgc"},{"Name":"Automation_a9lgc"},{"Name":"Automation_a9lgc"}	3
nodesWithOwnershipRules					0		{"Name":"Automation_a9lgc"}															4
connectionProductWithStrategy			0		{"Name":"Automation_zdq29 - Automation_a9lgc"}										5
connectionProductWithPriority			0		{"Name":"Automation_zdq29 - Automation_a9lgc"}										6
activeOwnershipStrategy					1		NULL																				7
nodeOwnershipCouldBeFound				0		{"Name":"Automation_a9lgc"}															8
chainWithAnaliticalOperationalInfo		0		NULL																				9
*/


