/*-- ================================================================================================
-- Author:		Microsoft
-- Created date: Oct-21-2019
-- Updated date: Mar-20-2020
-- <Description>:	This View is to Fetch Data related to node tags and related elements From Tables(NodeTag,CategoryElement)</Description>
-- ==================================================================================================*/
Create View [Admin].[view_NodeTagWithCategoryId] AS
	Select x.[NodeTagId], x.[NodeId], x.[ElementId], y.[CategoryId], x.[StartDate], x.[EndDate], x.[CreatedBy], x.[CreatedDate], x.[LastModifiedBy], x.[LastModifiedDate]
	FROM [Admin].[NodeTag] x 
	LEFT JOIN [Admin].[CategoryElement] y ON x.ElementId = y.ElementId;

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data related to node tags and related elements From Tables(NodeTag,CategoryElement)',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_NodeTagWithCategoryId',
    @level2type = NULL,
    @level2name = NULL