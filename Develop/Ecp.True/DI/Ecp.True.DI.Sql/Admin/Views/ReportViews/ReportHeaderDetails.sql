-- ======================================================================================================================
-- Author: Microsoft
-- Create date:  Jan-07-2019
-- Updated date: Mar-20-2020
--               May-07-2020 updated view only to Fetch System and Segment related Category data
-- <Description>: This View is to Fetch header details For PowerBi Report From Tables(NodeTag, Node,CategoryElement,Category)</Description>
-- ======================================================================================================================
CREATE VIEW [Admin].[ReportHeaderDetails]
AS
SELECT      [Admin].[udf_PascalCase](Cat.[Name])    AS PC_Category
           ,[Admin].[udf_PascalCase](CE.[Name])     AS PC_Element
           ,[Admin].[udf_PascalCase](N.[Name])      AS PC_NodeName
		  , Cat.[Name]                              AS Category
           ,CE.[Name]                               AS Element
           ,N.[Name]                                AS NodeName
FROM [Admin].[CategoryElement] CE
INNER JOIN [Admin].Category Cat
ON Cat.CategoryId = CE.CategoryId
AND Cat.CategoryId IN (2,8)
LEFT JOIN [Admin].[NodeTag] NT
ON CE.ElementId = NT.ElementId
LEFT JOIN [Admin].[Node] N
ON NT.NodeId = N.NodeId

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch header details For PowerBi Report From Tables(NodeTag, Node,CategoryElement,Category)',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'ReportHeaderDetails',
    @level2type = NULL,
    @level2name = NULL