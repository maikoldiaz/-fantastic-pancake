/*-- =================================================================================================
-- Author:		Microsoft
-- Created date: Mar- 06 -2020
-- Updated date: Mar- 18 - 2020 : removing duplicate rows
-- <Description>:	This View is to Fetch Data for NodeConnectionProducts from NodeConnectionProduct,Node, categoryelement, product, Nodetag </Description>
-- =================================================================================================== */

CREATE VIEW [Admin].[view_NodeConnectionProductRule]
AS
SELECT 
 [NodeConnectionProductid]
,[SourceOperator]
,[DestinationOperator]
,[SourceNode]
,[DestinationNode]
,[Product]
,[RuleId]
,[RuleName]	
,[RowVersion]

FROM (
SELECT
	 NCP.NodeConnectionProductid		AS [NodeConnectionProductid]
	,SourceOwner.[Name]					AS [SourceOperator]
	,DestinationOwner.[Name]			AS [DestinationOperator]
	,SourceNode.[Name]					AS [SourceNode]
	,DestinationNode.[Name]				AS [DestinationNode]
	,PRD.[Name]							AS [Product]
	,NCP.NodeconnectionProductruleId	AS [RuleId]
	,NCPRule.RuleName					AS [RuleName]	
	,ROW_NUMBER() over( partition by ncp.nodeconnectionproductid, ntsource.nodeid,ntsource.startdate,ntsource.enddate order by NTSource.CreatedDate desc) rn_src
	,ROW_NUMBER() over( partition by ncp.nodeconnectionproductid, NTDest.nodeid,NTDest.startdate,NTDest.enddate order by NTDest.CreatedDate desc) rn_des
	,ncp.RowVersion as [RowVersion]
FROM Admin.NodeConnectionProduct NCP 
INNER JOIN Admin.NodeConnection NC 
	ON NCP.nodeconnectionid = NC.nodeconnectionid 
INNER JOIN Admin.Node SourceNode 
	ON NC.SourceNodeid= SourceNode.Nodeid  -- sourceNode
INNER JOIN Admin.NodeTag NTSource 
	ON NTSource.Nodeid= SourceNode.Nodeid
INNER JOIN Admin.Categoryelement SourceOwner 
	ON SourceOwner.elementid= NTSource.elementid -- Source Owner
INNER JOIN Admin.Node DestinationNode 
	ON NC.DestinationNodeid=DestinationNode.Nodeid
INNER JOIN Admin.NodeTag NTDest 
	ON NTDest.Nodeid= DestinationNode.Nodeid  -- Destination Node
INNER JOIN Admin.Categoryelement DestinationOwner 
	ON DestinationOwner.elementid= NTDest.elementid  -- Destination Owner
INNER JOIN Admin.Product PRD 
	ON NCP.ProductId=PRD.Productid -- Product
LEFT JOIN Admin.NodeConnectionProductRule NCPRule 
	ON NCP.NodeconnectionProductruleId= NCPRule.Ruleid -- Rule
WHERE SourceOwner.CategoryId = 3 
AND DestinationOwner.categoryid = 3 -- Condition for Proprieto
AND	Admin.udf_GetTrueDate() BETWEEN NTSource.StartDate AND NTSource.EndDate
AND Admin.udf_GetTrueDate() BETWEEN NTDest.StartDate AND NTDest.EndDate
) TEMP 
WHERE TEMP.rn_src=1 AND TEMP.rn_des=1

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data for NodeConnectionProducts from NodeConnectionProduct,Node, categoryelement, product, Nodetag',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_NodeConnectionProductRule',
    @level2type = NULL,
    @level2name = NULL