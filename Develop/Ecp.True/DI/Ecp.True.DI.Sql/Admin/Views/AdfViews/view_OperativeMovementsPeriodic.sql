/*-- =================================================================================================
-- Author:		Microsoft
-- Created date: Dec-13-2019
-- Updated date: Mar-20-2020
-- <Description>:	This View is to Fetch Data of Movements for ADF to Load the data into OperativeMovements table.</Description>
-- ===================================================================================================*/
CREATE VIEW [Admin].[view_OperativeMovementsPeriodic] 
AS
SELECT	DISTINCT
		Mov.OperationalDate					AS	OperationalDate
		,Mov.TicketId                       AS  TicketId
		,Mov.DestinationNodeName			AS	DestinationNode
		,CeDestNodeType.Name				AS	DestinationNodeType
		,Mov.MovementTypeName				AS	MovementType
		,Mov.SourceNodeName					AS	SourceNode
		,CeSourceNodeType.Name				AS	SourceNodeType
		,Mov.SourceProductName				AS	SourceProduct
		,Ce.Name							AS	SourceProductType
		,Mov.NetStandardVolume				AS	NetStandardVolume
FROM Admin.view_MovementInformation Mov
INNER JOIN [Admin].[CategoryElement] Ce
ON Mov.[SourceProductTypeId] = Ce.ElementId
INNER JOIN [Admin].[NodeTag] NTSource
ON NTSource.NodeId = Mov.SourceNodeId
AND Mov.OperationalDate BETWEEN CAST(NTSource.StartDate AS DATE) and CAST(NTSource.EndDate AS DATE)
INNER JOIN [Admin].[NodeTag] NTDest
ON NTDest.NodeId = Mov.DestinationNodeId
AND Mov.OperationalDate BETWEEN CAST(NTDest.StartDate AS DATE) and CAST(NTDest.EndDate AS DATE)
INNER JOIN [Admin].[CategoryElement] CeSourceNodeType
ON CeSourceNodeType.ElementId = NTSource.ElementId
INNER JOIN [Admin].[CategoryElement] CeDestNodeType
ON CeDestNodeType.ElementId = NTDest.ElementId
WHERE Ce.CategoryId = 11--Product Type
AND CeSourceNodeType.CategoryId = 1--Node Type
AND CeDestNodeType.CategoryId = 1--Node Type
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data of Movements for ADF to Load the data into OperativeMovements table.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_OperativeMovementsPeriodic',
    @level2type = NULL,
    @level2name = NULL