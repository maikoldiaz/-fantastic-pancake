/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	Jul-14-2020 
-- <Description>:   This is a function to Return NodeName Based On NodeId </Description>
-- ==============================================================================================================================*/
CREATE FUNCTION [Admin].[udf_GetNodeName] 
(
    @NodeId               INT
)
RETURNS NVARCHAR(300)
AS
BEGIN

    DECLARE @NodeName NVARCHAR(300)

    SELECT TOP 1 @NodeName = [Name]
	FROM [Admin].[Node] 
    WHERE [NodeId] = @NodeId
    RETURN @NodeName
END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
                            @value = N'This is a function to Return NodeName Based On NodeId',
                            @level0type = N'SCHEMA',
                            @level0name = N'Admin',
                            @level1type = N'FUNCTION',
                            @level1name = N'udf_GetNodeName',
                            @level2type = NULL,
                            @level2name = NULL