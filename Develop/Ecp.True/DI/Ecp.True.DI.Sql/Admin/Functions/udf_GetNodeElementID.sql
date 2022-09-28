/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	Jul-08-2020
-- Updated Date:	Mar-20-2020
-- <Description>:   This is a function to Return Element Based on Parameters from Nodetag Table </Description>
-- ==============================================================================================================================*/
CREATE FUNCTION [Admin].[udf_GetNodeElementID] 
(
    @NodeId               INT,
    @CategoryId           INT
)
RETURNS INT
AS
BEGIN
    DECLARE  @ElementID     INT-->Can be Segment Or System
            ,@TodaysDate    DATE = Admin.udf_GetTrueDate()

    SELECT TOP 1 @ElementID = SrcNT.ElementId
	FROM Admin.NodeTag SrcNT	
	INNER JOIN Admin.CategoryElement SrcCE
	ON  SrcCE.ElementId = SrcNT.ElementId
    WHERE @TodaysDate BETWEEN SrcNT.StartDate AND SrcNT.EndDate
	AND SrcCE.CategoryId = @CategoryId--2--Segment,8--System
    AND SrcNT.NodeId = @NodeId

    RETURN @ElementID
END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
                            @value = N'This is a function to Return Element Based on Parameters from Nodetag Table',
                            @level0type = N'SCHEMA',
                            @level0name = N'Admin',
                            @level1type = N'FUNCTION',
                            @level1name = N'udf_GetNodeElementID',
                            @level2type = NULL,
                            @level2name = NULL