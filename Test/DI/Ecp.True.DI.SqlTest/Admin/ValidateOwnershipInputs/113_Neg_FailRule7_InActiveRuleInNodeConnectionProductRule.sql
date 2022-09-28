--Rule 7 Failed because of [NodeConnectionProductRule] table Rule ID--> is InActive
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
	    @SegmentId							INT						= 15
       ,@StartDate							DATETIME				= '2020-03-10'
       ,@EndDate							DATETIME				= '2020-03-12'
	   ,@DtNodeInActiveRules				Admin.[KeyValueType]
	   ,@DtNodeProductInActiveRules			Admin.[KeyValueType]
	   ,@DtConnectionProductInActiveRules	Admin.[KeyValueType]


SELECT	TOP 1 
				 @SegmentId = NdSeg.ElementId
				,@StartDate = CAST(NdSeg.StartDate AS DATE)
				,@EndDate   = CAST(NdSeg.StartDate+2 AS DATE)
FROM Admin.Nodetag NdSeg
INNER JOIN Admin.NodeConnection NC
ON (NdSeg.NodeId = NC.SourceNodeId
	OR NdSeg.NodeId = NC.DestinationNodeId)
INNER JOIN Admin.NodeConnectionProduct NCP
ON NC.NodeConnectionId = NCP.NodeConnectionId
LEFT JOIN Admin.[NodeConnectionProductRule] NCPrdRule
ON NCP.[NodeConnectionProductRuleId] = NCPrdRule.RuleId
WHERE NCPrdRule.RuleName IS NOT NULL
AND NdSeg.ElementId = 15

UPDATE [Admin].[NodeConnectionProductRule]
SET IsActive = 0
WHERE RuleId = 1


INSERT INTO @DtNodeInActiveRules 
(
	 [Key]	
	,[Value]
)
VALUES(1,'S-Disponsible')

INSERT INTO @DtConnectionProductInActiveRules 
(
	 [Key]	
	,[Value]
)
VALUES(1,'Entradas')

INSERT INTO @DtNodeProductInActiveRules 
(
	 [Key]	
	,[Value]
)
VALUES(1,'Salidas')



EXEC [Admin].[usp_ValidateOwnershipInputs]			@SegmentId
													,@StartDate
													,@EndDate
													,@DtNodeInActiveRules
													,@DtNodeProductInActiveRules
													,@DtConnectionProductInActiveRules

UPDATE [Admin].[NodeConnectionProductRule]
SET IsActive = 1
WHERE RuleId = 1

--SELECT * FROM  [Admin].[NodeConnectionProductRule]

/*
--Rule 7 Failed because of [NodeConnectionProductRule] table Rule ID--> is InActive

Name								Result				ErrorMessage																		DisplayOrder
chainWithInitialNodes				1					NULL																				1
chainWithFinalNodes					1					NULL																				2
nodesWithStrategy					0					{"Name":"zxxsd"},{"Name":"zxxsd"},{"Name":"zxxsd"},{"Name":"Automation_p4eyz"},{"Name":"Automation_p4eyz"},{"Name":"Automation_p4eyz"}	3
nodesWithOwnershipRules				0					{"Name":"zxxsd"}																	4
connectionProductWithStrategy		0					{"Name":"zxxsd - Automation_p4eyz"}													5
connectionProductWithPriority		0					{"Name":"zxxsd - Automation_p4eyz"}													6
activeOwnershipStrategy				0					[{"Name":"Entradas","Type":"ConnectionProduct"}]									7
nodeOwnershipCouldBeFound			0					{"Name":"zxxsd"}																	8
chainWithAnaliticalOperationalInfo	0					NULL																				9

*/