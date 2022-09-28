﻿/*-- ================================================================================================
-- Author:		Microsoft
-- Create date: Oct-21-2019
-- Updated date: Mar-20-2020
-- <Description>:This View is to Fetch Data related to movement along with node and product information From Tables(MovementSource,Node,view_StorageLocationProductWithProductName)</Description>
-- ==================================================================================================*/
Create view [Admin].[view_MovementSourceWithNodeAndProductName] AS
Select ms.*, n.Name As SourceNode, slp.ProductName As SourceProduct
from [Offchain].[MovementSource] ms
LEFT JOIN [Admin].[Node] n ON ms.SourceNodeId = n.NodeId
LEFT JOIN [Admin].[view_StorageLocationProductWithProductName] slp ON ms.SourceProductId = slp.ProductId;

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data related to movement along with node and product information From Tables(MovementSource,Node,view_StorageLocationProductWithProductName)',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_MovementSourceWithNodeAndProductName',
    @level2type = NULL,
    @level2name = NULL