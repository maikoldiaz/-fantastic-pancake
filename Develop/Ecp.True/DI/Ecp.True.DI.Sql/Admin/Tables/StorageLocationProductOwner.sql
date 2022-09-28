/*==============================================================================================================================
--Author:        Microsoft
--Created date : Sep-19-2019
--Updated date : Mar-20-2020
--<Description>: This table is to store storage location product and owner mapping.</Description>
================================================================================================================================*/
CREATE TABLE [Admin].[StorageLocationProductOwner]
(
	[StorageLocationProductOwnerId]		INT IDENTITY (1, 1) NOT NULL,
	[OwnerId]							INT NOT NULL,
	[OwnershipPercentage]				DECIMAL(5, 2) NOT NULL,
	[StorageLocationProductId]			INT NOT NULL,
	[IsDeleted]							BIT					NOT NULL	DEFAULT 0,		--> 1=Deleted

	--Internal Common Columns
	[CreatedBy]							NVARCHAR (260)   NOT NULL,
	[CreatedDate]						DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]					NVARCHAR (260)   NULL,
	[LastModifiedDate]					DATETIME        NULL,

	--Constraints
	CONSTRAINT [PK_StorageLocationProductOwner]							PRIMARY KEY CLUSTERED	([StorageLocationProductOwnerId] ASC),
	CONSTRAINT [FK_StorageLocationProductOwner_CategoryElement]			FOREIGN KEY		([OwnerId])										REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_StorageLocationProductOwner_StorageLocationProduct]	FOREIGN KEY		([StorageLocationProductId])					REFERENCES [Admin].[StorageLocationProduct] ([StorageLocationProductId])
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table is to store storage location product and owner mapping.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductOwner',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the relationship between a storage location product and a owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'StorageLocationProductOwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of a owner (category element of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The ownership percentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of a storage location product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'StorageLocationProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the storagelocationproduct-owner mapping is active or not, 1 means active',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'IsDeleted'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'