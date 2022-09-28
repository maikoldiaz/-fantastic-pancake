/*==============================================================================================================================
--Author:        Microsoft
--Created date : Dec-10-2019
--Updated date : Jun-06-2021
--<Description>: This table is to store product and storage location mapping.</Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[StorageLocationProductMapping] 
(
	--Columns
	[StorageLocationProductMappingId]		INT IDENTITY (1, 1)		NOT NULL,
    [StorageLocationId]						NVARCHAR (20)			NOT NULL,
	[ProductId]								NVARCHAR (20)			NOT NULL,
    [RowVersion]		                    ROWVERSION,
	
	--Internal Common Columns
	[CreatedBy]								NVARCHAR (260)			NOT NULL,
	[CreatedDate]							DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_Id]						PRIMARY KEY CLUSTERED ([StorageLocationProductMappingId] ASC),
	CONSTRAINT [FK_StorageLocationProductMapping_StorageLocation]		FOREIGN KEY ([StorageLocationId])	REFERENCES [Admin].[StorageLocation] ([StorageLocationId]),
	CONSTRAINT [FK_StorageLocationProductMapping_Product]				FOREIGN KEY ([ProductId])			REFERENCES [Admin].[Product] ([ProductId])
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table is to store product and storage location mapping.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductMapping',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the relationship between a product and a storage location',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductMapping',
    @level2type = N'COLUMN',
    @level2name = N'StorageLocationProductMappingId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the storage location',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductMapping',
    @level2type = N'COLUMN',
    @level2name = N'StorageLocationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductMapping',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductMapping',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductMapping',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'