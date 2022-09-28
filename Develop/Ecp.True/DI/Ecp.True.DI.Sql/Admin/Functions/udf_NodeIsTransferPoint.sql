/*-- ============================================================================================================================
-- Author:          Intergrupo
-- Created Date:	May-04-2021
-- <Description>:   This return if the node is a transfer point. </Description>
-- ==============================================================================================================================*/

CREATE FUNCTION [Admin].[udf_NodeIsTransferPoint](@NodeId Int)
RETURNS BIT
AS
BEGIN
	DECLARE @TransferConnectionCount INT;

	SET @TransferConnectionCount = (SELECT COUNT(*)
		FROM Admin.NodeConnection NC
		LEFT JOIN Admin.NodeTag NTS ON NC.SourceNodeId = NTS.NodeId
		LEFT JOIN Admin.CategoryElement CES ON CES.ElementId = NTS.ElementId
		LEFT JOIN Admin.NodeTag NTD ON NC.DestinationNodeId = NTD.NodeId
		LEFT JOIN Admin.CategoryElement CED ON CED.ElementId = NTD.ElementId
		WHERE (NC.SourceNodeId = @NodeId OR NC.DestinationNodeId = @NodeId) AND CES.CategoryId=2 AND CED.CategoryId=2 AND CES.ElementId <> CED.ElementId);

	RETURN CASE WHEN @TransferConnectionCount > 0 THEN 1 ELSE 0 END;
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This return if the node is a transfer point.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'FUNCTION',
    @level1name = N'udf_NodeIsTransferPoint',
    @level2type = NULL,
    @level2name = NULL
