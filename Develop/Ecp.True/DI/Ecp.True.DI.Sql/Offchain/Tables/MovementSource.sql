/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Nov-11-2019
-- Updated Date:	Mar-20-2020
 <Description>:		This table holds the details for the Movement Source.  </Description>

-- ================================================================================================================================*/
CREATE TABLE [Offchain].[MovementSource]
(
	--Columns
	[MovementSourceId]				INT IDENTITY (1, 1)		NOT NULL,
	[MovementTransactionId]			INT						NOT NULL,
	[SourceNodeId]					INT						NULL,
	[SourceStorageLocationId]		INT						NULL,
	[SourceProductId]				NVARCHAR (20)			NULL,
	[SourceProductTypeId]			INT         			NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
	CONSTRAINT [PK_MovementSource]										PRIMARY KEY CLUSTERED ([MovementSourceId] ASC),
	CONSTRAINT [FK_MovementSource_Movement]								FOREIGN KEY ([MovementTransactionId])				REFERENCES [Offchain].[Movement] ([MovementTransactionId]),
	CONSTRAINT [FK_MovementSource_Node]									FOREIGN KEY ([SourceNodeId])						REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_MovementSource_StorageLocation]						FOREIGN KEY ([SourceStorageLocationId])				REFERENCES [Admin].[NodeStorageLocation] ([NodeStorageLocationId]),
	CONSTRAINT [FK_MovementSource_Product]								FOREIGN KEY ([SourceProductId])						REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [UC_MovementSource_Movement]								UNIQUE ([MovementTransactionId]),
    CONSTRAINT [FK_MovementSource_SourceProdTypeId]						FOREIGN KEY ([SourceProductTypeId])					REFERENCES [Admin].[CategoryElement] ([ElementId])
);
GO
CREATE NONCLUSTERED INDEX NCI_MovementSource_SourceNodeId_MovementTransactionId_SourceProductId_SourceProductTypeId
ON [Offchain].[MovementSource] ([SourceNodeId])
INCLUDE ([MovementTransactionId],[SourceProductId],[SourceProductTypeId])
GO

CREATE NONCLUSTERED INDEX NCI_MovementSrc_SrcNodeId_SrcStorageLocId_MovementTranId_SrcProductId_SrcPrdTypeId
ON [Offchain].[MovementSource] ([MovementTransactionId])
INCLUDE ([SourceNodeId],[SourceProductId],[SourceProductTypeId],[SourceStorageLocationId])
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement source',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementSource',
    @level2type = N'COLUMN',
    @level2name = N'MovementSourceId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement transaction ',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementSource',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementSource',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source storage location',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementSource',
    @level2type = N'COLUMN',
    @level2name = N'SourceStorageLocationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementSource',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source product type',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementSource',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementSource',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementSource',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementSource',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementSource',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the details for the Movement Source.',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementSource',
    @level2type = NULL,
    @level2name = NULL