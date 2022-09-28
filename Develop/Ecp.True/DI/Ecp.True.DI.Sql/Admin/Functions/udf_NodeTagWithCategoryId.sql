/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	Sep-29-2019
-- Updated Date:	Mar-20-2020
-- <Description>:  This is a helper function for usp_NodeTag for 3 operations - Insert, Update and Expiration of Node Tags.  </Description>
-- ==============================================================================================================================*/

CREATE FUNCTION [Admin].[udf_NodeTagWithCategoryId] (@OperationType INT, @ElementId INT, @InputDate DATETIME, @dataTable [Admin].[NodeTagType] READONLY)
RETURNS @NodeTag TABLE (
    [NodeTagId]    					INT,
    [NodeId]         				INT				NOT NULL,
    [ElementId]  					INT				NOT NULL,
	[CategoryId]					INT				NOT NULL,
	[StartDate]						DATETIME		NOT NULL,
    [EndDate]						DATETIME		NOT NULL,
	[CreatedBy]						NVARCHAR (260)  NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL,
	[LastModifiedBy]				NVARCHAR (260)  NULL,
	[LastModifiedDate]				DATETIME        NULL
) 
AS
BEGIN
	IF (@OperationType = 1)		--> Insert
	BEGIN
		INSERT INTO @NodeTag
		Select	x.[NodeTagId], 
				x.[NodeId], 
				@ElementId as [ElementId], 
				y.[CategoryId], 
				@InputDate as [StartDate], 
				cast('9999-12-31 00:00:00.000' as datetime) as [EndDate], 
				x.[CreatedBy], 
				x.[CreatedDate], 
				x.[LastModifiedBy], 
				x.[LastModifiedDate]
		FROM @dataTable x 
		LEFT JOIN [Admin].[CategoryElement] y ON y.ElementId = @ElementId;
    END

	ELSE IF (@OperationType = 2 OR @OperationType = 3)		--> Update Or Set Expire
	BEGIN
		INSERT INTO @NodeTag
		Select	x.[NodeTagId], 
				x.[NodeId], 
				x.[ElementId], 
				x.[CategoryId], 
				x.[StartDate], 
				x.[EndDate], 
				x.[CreatedBy], 
				x.[CreatedDate], 
				x.[LastModifiedBy], 
				x.[LastModifiedDate]
		FROM [Admin].[view_NodeTagWithCategoryId] x
		LEFT JOIN [Admin].[CategoryElement] y ON y.ElementId = x.[ElementId]
		WHERE x.NodeTagId IN (SELECT NodeTagId FROM @dataTable);
	END
   RETURN;
END;

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is a helper function for usp_NodeTag for 3 operations - Insert, Update and Expiration of Node Tags.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'FUNCTION',
    @level1name = N'udf_NodeTagWithCategoryId',
    @level2type = NULL,
    @level2name = NULL
