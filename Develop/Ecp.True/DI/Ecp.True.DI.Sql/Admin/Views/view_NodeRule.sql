/*-- ===========================================================================================================================
-- Author:		Microsoft
-- Created date: March- 10 -2020
-- Updated date: March - 18 - 2020 : removing duplicate rows
-- <Description>:	This View is to Fetch Data for NodeRule from NodeOwnershipRule, Node, CategoryElement, product, Nodetag </Description>
-- ============================================================================================================================= */

CREATE VIEW [Admin].[view_NodeRule]
AS 
SELECT
        NodeId
       ,[Name]
       ,Segment
       ,Operator
       ,NodeType
	   ,RuleId
       ,RuleName
       ,[RowVersion]
FROM (
Select   N.NodeId                   as NodeId
        ,N.Name                     as [Name]
        ,CE1.Name					as Segment
        ,CE2.Name					as Operator
        ,CE3.Name					as NodeType
		,N.NodeOwnershipRuleId		as RuleId
        ,NOR.RuleName				as RuleName
        ,ROW_NUMBER() over( partition by N.NodeId, Operator.nodeid,Operator.startdate,Operator.enddate order by Operator.CreatedDate desc) rn_Operator
	    ,ROW_NUMBER() over( partition by N.NodeId, NodeType.nodeid,NodeType.startdate,NodeType.enddate order by NodeType.CreatedDate desc) rn_NodeType
        ,ROW_NUMBER() over( partition by N.NodeId, Segment.nodeid,Segment.startdate,Segment.enddate order by Segment.CreatedDate desc) rn_Segment
        ,n.RowVersion as [RowVersion]
From    admin.Node N
Inner Join Admin.NodeTag Segment
On N.NodeId = Segment.NodeId
Inner Join Admin.NodeTag Operator
On N.NodeId = Operator.NodeId
Inner Join Admin.NodeTag NodeType
On N.NodeId = NodeType.NodeId
Inner Join Admin.CategoryElement CE1
on CE1.ElementId  = Segment.ElementId
Inner Join Admin.CategoryElement CE2
on CE2.ElementId  = Operator.ElementId
Inner Join Admin.CategoryElement CE3
on CE3.ElementId  = NodeType.ElementId
Left Join Admin.NodeOwnershipRule NOR
On N.NodeOwnershipRuleId = NOR.RuleId
Where       CE1.CategoryId = 2
And         CE2.CategoryId = 3
And         CE3.CategoryId = 1
And         Operator.StartDate <= Admin.udf_GetTrueDate() and Operator.EndDate >= Admin.udf_GetTrueDate()
And         Segment.StartDate <= Admin.udf_GetTrueDate() and Segment.EndDate >= Admin.udf_GetTrueDate()
And         NodeType.StartDate <= Admin.udf_GetTrueDate() and NodeType.EndDate >= Admin.udf_GetTrueDate()

) TEMP
WHERE TEMP.rn_NodeType=1 AND rn_Operator=1 AND rn_Segment=1

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data for NodeRule from NodeOwnershipRule, Node, CategoryElement, product, Nodetag',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_NodeRule',
    @level2type = NULL,
    @level2name = NULL

