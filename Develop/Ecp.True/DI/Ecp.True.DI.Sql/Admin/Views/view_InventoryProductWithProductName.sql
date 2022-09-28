-- ======================================================================================================================
-- Author: Microsoft
-- Create date: Jan-11-2019
-- Updated date: Mar-20-2020
--				 Apr-09-2020  -- Removed(BlockchainStatus = 1)   
-- <Description>: This View is to Fetch Data [Admin].[[view_InventoryProductWithProductName]] For PowerBi Report From Tables(InventoryProduct,[view_StorageLocationProductWithProductName])</Description>
-- =========================================================================================================================

CREATE VIEW [Admin].[view_InventoryProductWithProductName] 
AS
SELECT distinct ip.*, slp.ProductName 
FROM [Offchain].[InventoryProduct] ip
INNER JOIN [Admin].[view_StorageLocationProductWithProductName] slp 
ON slp.ProductId = ip.ProductId;

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data [Admin].[[view_InventoryProductWithProductName]] For PowerBi Report From Tables(InventoryProduct,[view_StorageLocationProductWithProductName])',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_InventoryProductWithProductName',
    @level2type = NULL,
    @level2name = NULL