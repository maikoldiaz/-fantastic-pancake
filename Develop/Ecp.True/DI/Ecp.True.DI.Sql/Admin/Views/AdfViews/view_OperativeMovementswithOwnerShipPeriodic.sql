/*-- =================================================================================================
-- Author:		Microsoft
-- Create date: Dec-13-2019
-- Updated date: Mar-20-2020
-- <Description>:	This View is to Fetch Data of Movements for ADF to Load the data.</Description>
-- ===================================================================================================*/
CREATE VIEW [Admin].[ view_OperativeMovementswithOwnerShipPeriodic] 
AS
SELECT	DISTINCT
		Mov.OperationalDate				AS	OperationalDate
	   ,Mov.DestinationNodeName			AS	DestinationNode
	   ,''								AS	DestinationNodeType--May I know the Source Column of This
	   ,Mov.MovementTypeName			AS	MovementType
	   ,Mov.SourceNodeName				AS	SourceNode
	   ,''								AS	SourceNodeType--May I know the Source Column of This
	   ,Mov.SourceProductName			AS	SourceProduct
	   ,Mov.SourceProductTypeId			AS	SourceProductType
	   ,''								AS	TransferPoint--May I know the Source Column of This
	   ,''								AS	FIeldWaterProduction--May I know the Source Column of This
	   ,''								AS	SourceField--May I know the Source Column of This
	   ,''								AS	RelatedSourceField--May I know the Source Column of This
	   ,Mov.NetStandardVolume			AS	NetStandardVolume
	   ,'TRUE'							AS	SourceSystem
	   ,Admin.udf_GetTrueDate()			AS	LoadDate--Can we remove This from View, we have it in the Destination Table
	   ,''								AS	ExecutionID--Can we remove This From View, we have it in the Destination Table
FROM Admin.view_MovementInformation Mov

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data of Movements for ADF to Load the data.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N' view_OperativeMovementswithOwnerShipPeriodic',
    @level2type = NULL,
    @level2name = NULL