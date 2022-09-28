/*-- =================================================================================================
-- Author:		Microsoft
-- Create date: March-31-2020
--<Description>:	This Table is to store StorageLocationProduct and Variable mapping.  </Description>
-- ===================================================================================================*/


CREATE TABLE [Admin].[StorageLocationProductVariable]
(
    --Columns
	[StorageLocationProductVariableId]			INT IDENTITY (201, 1)			NOT NULL,
	[StorageLocationProductId]					INT								NOT NULL,
	[VariableTypeId]							INT								NOT NULL,
	[RowVersion]								ROWVERSION,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)			NOT NULL,
	[CreatedDate]					DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)			NULL,
	[LastModifiedDate]				DATETIME				NULL,

	--Constraints
    CONSTRAINT [PK_StorageLocationProductVariable]								PRIMARY KEY CLUSTERED ([StorageLocationProductVariableId] ASC),
	CONSTRAINT [FK_StorageLocationProductVariable_StorageLocationProduct]		FOREIGN KEY ([StorageLocationProductId])			REFERENCES [Admin].[StorageLocationProduct] ([StorageLocationProductId]),
	CONSTRAINT [FK_StorageLocationProductVariable_VariableType]					FOREIGN KEY ([VariableTypeId])						REFERENCES [Admin].[VariableType] ([VariableTypeId]),
	CONSTRAINT [UQ_StorageLocationProductVariable_StorageLocationProduct]		UNIQUE NONCLUSTERED ([StorageLocationProductId], [VariableTypeId])
)


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the storage location product variable',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductVariable',
    @level2type = N'COLUMN',
    @level2name = N'StorageLocationProductVariableId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the storage location product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductVariable',
    @level2type = N'COLUMN',
    @level2name = N'StorageLocationProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the variable type (like for Interfase, Tolerancia, Entrada, etc.)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductVariable',
    @level2type = N'COLUMN',
    @level2name = N'VariableTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version column used for consistency',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductVariable',
    @level2type = N'COLUMN',
    @level2name = N'RowVersion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductVariable',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductVariable',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductVariable',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductVariable',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Table is to store StorageLocationProduct and Variable mapping.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductVariable',
    @level2type = NULL,
    @level2name = NULL