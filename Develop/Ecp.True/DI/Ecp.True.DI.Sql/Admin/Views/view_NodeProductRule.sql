/*-- ===========================================================================================================================
-- Author:		Microsoft
-- Created date: March- 09 -2020
-- Updated date: March - 18 - 2020 : removing duplicate rows
-- <Description>:	This View is to Fetch Data for NodeProductRule from NodeProductRule,NodeStorageLocation,StorageLocationProduct, 
				Node, CategoryElement, product, Nodetag </Description>
-- ============================================================================================================================= */

CREATE VIEW [Admin].[view_NodeProductRule]
AS
SELECT  Segment
	   ,Operator
	   ,NodeType
	   ,[NodeName]
	   ,[StorageLocation]
	   ,[Product]
	   ,[RuleId]
	   ,[RuleName]
	   ,[StorageLocationProductID]
	   ,[RowVersion]
FROM (
Select   
         CE1.Name						as Segment
        ,CE2.Name						as Operator
        ,CE3.Name						as NodeType
		,ND.Name						AS [NodeName]
		,NSL.Name						AS [StorageLocation]
		,Prod.Name						AS [Product]
		,SLP.NodeProductRuleId			AS [RuleId]
		,NPR.RuleName					AS [RuleName]
		,SLP.StorageLocationProductID   AS [StorageLocationProductID]
		,ROW_NUMBER() over( partition by SLP.StorageLocationProductID, Operator.nodeid,Operator.startdate,Operator.enddate order by Operator.CreatedDate desc) rn_Operator
	    ,ROW_NUMBER() over( partition by SLP.StorageLocationProductID, NodeType.nodeid,NodeType.startdate,NodeType.enddate order by NodeType.CreatedDate desc) rn_NodeType
        ,ROW_NUMBER() over( partition by SLP.StorageLocationProductID, Segment.nodeid,Segment.startdate,Segment.enddate order by Segment.CreatedDate desc) rn_Segment
		,slp.RowVersion as [RowVersion]
From    admin.Node ND
Inner Join Admin.NodeTag Segment
On ND.NodeId = Segment.NodeId
Inner Join Admin.NodeTag Operator
On ND.NodeId = Operator.NodeId
Inner Join Admin.NodeTag NodeType
On ND.NodeId = NodeType.NodeId
Inner Join Admin.CategoryElement CE1
on CE1.ElementId  = Segment.ElementId
Inner Join Admin.CategoryElement CE2
on CE2.ElementId  = Operator.ElementId
Inner Join Admin.CategoryElement CE3
on CE3.ElementId  = NodeType.ElementId

INNER JOIN Admin.NodeStorageLocation NSL 
ON NSL.NodeID = ND.NodeID
INNER JOIN Admin.StorageLocationProduct SLP 
ON SLP.NodeStorageLocationID = NSL.NodeStorageLocationID
INNER JOIN Admin.Product Prod 
ON Prod.ProductId = SLP.ProductID
LEFT JOIN Admin.NodeProductRule NPR
ON NPR.RuleId = SLP.NodeProductRuleId

Where		CE1.CategoryId = 2	-- Segment
And			CE2.CategoryId = 3	-- Operador
And			CE3.CategoryId = 1	-- Tipo de Nodo
And			Operator.StartDate <= Admin.udf_GetTrueDate() and Operator.EndDate >= Admin.udf_GetTrueDate()
And			Segment.StartDate <= Admin.udf_GetTrueDate() and Segment.EndDate >= Admin.udf_GetTrueDate()
And			NodeType.StartDate <= Admin.udf_GetTrueDate() and NodeType.EndDate >= Admin.udf_GetTrueDate()

)TMP 
WHERE TMP.rn_NodeType=1 AND rn_Operator=1 AND rn_Segment=1

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data for NodeProductRule from NodeProductRule,NodeStorageLocation,StorageLocationProduct, Node, CategoryElement, product, Nodetag',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_NodeProductRule',
    @level2type = NULL,
    @level2name = NULL