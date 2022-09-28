/*-- =================================================================================================
-- Author:		Microsoft
-- Created date: Jul-29-2020
-- <Description>:	This table is to Fetch InventoryDetailsWithOwner Data For PowerBi Report From
				Tables(Movement, Product, Ticket, Node, NodeTag, CategoryElement, Category, VariableType)</Description>
-- ===================================================================================================*/

CREATE TABLE [Admin].[InventoryDetailsWithOwner](
	[Category]					[nvarchar](150)  NULL,
	[Element]					[nvarchar](150)  NULL,
	[NodeName]					[nvarchar](150)  NULL,
	[CalculationDate]			[date]           NULL,
	[Product]					[nvarchar](150)  NULL,
	[ProductId]					[nvarchar](20)   NULL,
	[OwnerName]					[nvarchar](150)  NULL,
	[InventoryId]				[varchar](50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
	[InventoryProductId]		[int]            NULL,
	[InventoryDate]				[date]           NULL,
	[TankName]					[nvarchar](20)   NULL,
	[BatchId]					[nvarchar](150)  NULL,
	[Net Volume]				[decimal](29, 2) NULL,
	[Volume Unit]				[nvarchar](150)  NULL,
	[EventType]					[nvarchar](10)   NULL,
	[SystemName]				[nvarchar](150)  NULL,
	[UncertainityPercentage]	[decimal](29, 2) NULL,
	[Incertidumbre]				[decimal](29, 2) NULL,
	[OwnershipVolume]			[decimal](29, 2) NULL,
	[OwnershipPercentage]	    [decimal](29, 2) NULL,
	[AppliedRule]				[nvarchar](50)   NULL,
	[GrossStandardQuantity]		[decimal](18, 2) NULL,
    [OwnershipTicketId]         [int]            NULL,

		--Internal Common Columns

	[CreatedBy]					        NVARCHAR (260)			NOT NULL,
	[CreatedDate]					    DATETIME				NOT NULL,
	LastModifiedBy					    NVARCHAR (260)			 NULL,
	LastModifiedDate                    DATETIME                 NULL

) 

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the Category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the Element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Element'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the Node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date of the Calculation',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the Product ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Product'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the Product ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the Owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnerName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the Inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'InventoryId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the InventoryProduct',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date of Inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'InventoryDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of TanK',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'TankName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier  of the Batch',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the Net Volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Net Volume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the Volume Unit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Volume Unit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the Event',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the System)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of UncertainityPercentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'UncertainityPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Incertidumbre',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Incertidumbre'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipVolume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipPercentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of AppliedRule',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'AppliedRule'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of GrossStandardQuantity',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for purchases and sales InventoryDetailsWithOwners registered in the system.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = NULL,
    @level2name = NULL

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The ownership ticket identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipTicketId'