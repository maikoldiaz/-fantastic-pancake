/*-- ================================================================================================
-- Author:		Microsoft
-- Create date: Oct-21-2019
-- Updated date: Mar-20-2020
-- <Description>:This View is to Fetch Data related to movementdestination along with node and product information From Tables([MovementDestination],Node,view_StorageLocationProductWithProductName)</Description>
-- ==================================================================================================*/
Create view [Admin].[view_MovementDestinationWithNodeAndProductName] AS
Select md.*, n.Name As DestinationNode, slp.ProductName As DestinationProduct
from [Offchain].[MovementDestination] md
LEFT JOIN [Admin].[Node] n ON md.DestinationNodeId = n.NodeId
LEFT JOIN [Admin].[view_StorageLocationProductWithProductName] slp ON md.DestinationProductId = slp.ProductId;

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data related to movementdestination along with node and product information From Tables([MovementDestination],Node,view_StorageLocationProductWithProductName)',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_MovementDestinationWithNodeAndProductName',
    @level2type = NULL,
    @level2name = NULL