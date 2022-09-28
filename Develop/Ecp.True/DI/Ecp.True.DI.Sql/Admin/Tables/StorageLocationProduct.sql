/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-20-2019
--Updated date : Mar-20-2020
--Updated Date : Oct-05-2020  Adding indexes to improve the query performance
--<Description>: This table holds the details for association product and node-storage location. Also, Estrategias de propiedad of storage location-product.</Description>
===============================================================================================================================*/
CREATE TABLE [Admin].[StorageLocationProduct] 
(
	--Columns
    [StorageLocationProductId]		INT	IDENTITY (1, 1) NOT NULL,
    [IsActive]						BIT					NOT NULL	DEFAULT(1),
    [ProductId]						NVARCHAR (20)		NOT NULL,
    [NodeStorageLocationId]			INT					NOT NULL,
	[OwnershipRuleId]				INT					NULL,
	[UncertaintyPercentage]			DECIMAL(5, 2)		NULL,
	[NodeProductRuleId]				INT					NULL,
	[RowVersion]					ROWVERSION,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)		NOT NULL,
	[CreatedDate]					DATETIME			NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)		NULL,
	[LastModifiedDate]				DATETIME			NULL,

	--Constraints
    CONSTRAINT [PK_StorageLocationProducts]								PRIMARY KEY CLUSTERED ([StorageLocationProductId] ASC),
    CONSTRAINT [FK_StorageLocationProduct_NodeStorageLocation]			FOREIGN KEY ([NodeStorageLocationId])			REFERENCES [Admin].[NodeStorageLocation] ([NodeStorageLocationId]),
    CONSTRAINT [FK_StorageLocationProduct_Product]						FOREIGN KEY ([ProductId])						REFERENCES [Admin].[Product]  ([ProductId]),
	CONSTRAINT [FK_StorageLocationProduct_OwnershipRuleId]				FOREIGN KEY ([OwnershipRuleId])					REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_StorageLocationProduct_NodeProductRule]				FOREIGN KEY	([NodeProductRuleId])				REFERENCES [Admin].[NodeProductRule] ([RuleId])
);
GO

CREATE NONCLUSTERED INDEX [NIX_StorageLocationProduct_NodeStorageLocationId] 
ON Admin.StorageLocationProduct (NodeStorageLocationId)
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the association between product and node-storagelocation',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProduct',
    @level2type = N'COLUMN',
    @level2name = N'StorageLocationProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if this is active or not, 1 means active',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProduct',
    @level2type = N'COLUMN',
    @level2name = N'IsActive'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProduct',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the association between storage location and node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProduct',
    @level2type = N'COLUMN',
    @level2name = N'NodeStorageLocationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of rule (category element of rule category)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProduct',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipRuleId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The uncertaintity percentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProduct',
    @level2type = N'COLUMN',
    @level2name = N'UncertaintyPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of Estrategia de propiedad for association between product and node-storagelocation',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProduct',
    @level2type = N'COLUMN',
    @level2name = N'NodeProductRuleId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version column used for consistency',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProduct',
    @level2type = N'COLUMN',
    @level2name = N'RowVersion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProduct',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProduct',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProduct',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProduct',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the details for association product and node-storage location. Also, Estrategias de propiedad of storage location-product.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProduct',
    @level2type = NULL,
    @level2name = NULL