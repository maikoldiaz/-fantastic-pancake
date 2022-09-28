/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Nov-11-2019
-- Updated Date:	Mar-20-2020
 <Description>:		This table holds the details for the Movement Destination. </Description>

-- ================================================================================================================================*/
CREATE TABLE [Offchain].[MovementDestination]
(
	--Columns
	[MovementDestinationId]				INT IDENTITY (1, 1)		NOT NULL,
	[MovementTransactionId]				INT						NOT NULL,
	[DestinationNodeId]					INT						NULL,
	[DestinationStorageLocationId]		INT						NULL,
	[DestinationProductId]				NVARCHAR (20)			NULL,
	[DestinationProductTypeId]			INT         			NULL,
	
	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
	CONSTRAINT [PK_MovementDestination]						PRIMARY KEY CLUSTERED ([MovementDestinationId] ASC),
	CONSTRAINT [FK_MovementDestination_Movement]			FOREIGN KEY ([MovementTransactionId])					REFERENCES [Offchain].[Movement] ([MovementTransactionId]),
	CONSTRAINT [FK_MovementDestination_Node]				FOREIGN KEY ([DestinationNodeId])						REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_MovementDestination_StorageLocation]		FOREIGN KEY ([DestinationStorageLocationId])			REFERENCES [Admin].[NodeStorageLocation] ([NodeStorageLocationId]),
	CONSTRAINT [FK_MovementDestination_Product]				FOREIGN KEY ([DestinationProductId])					REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [UC_MovementDestination_Movement]			UNIQUE ([MovementTransactionId]),
    CONSTRAINT [FK_MovementDestination_DestinationProdTypeId]	FOREIGN KEY ([DestinationProductTypeId])			REFERENCES [Admin].[CategoryElement] ([ElementId])
);
GO

CREATE NONCLUSTERED INDEX NCI_MovementDestination_DestinationNodeId_MovementTransactionId_DestinationProductId_DestinationProductTypeId
ON [Offchain].[MovementDestination] ([DestinationNodeId])
INCLUDE ([MovementTransactionId],[DestinationProductId],[DestinationProductTypeId])
GO

CREATE NONCLUSTERED INDEX NCI_MovementDest_DestNode_DestStorageLocId_MovementTransId_DestProductId_DestPrdTypeId
ON [Offchain].[MovementDestination] ([MovementTransactionId])
INCLUDE ([DestinationNodeId],[DestinationProductId],[DestinationProductTypeId],[DestinationStorageLocationId])
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement destination',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementDestination',
    @level2type = N'COLUMN',
    @level2name = N'MovementDestinationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementDestination',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementDestination',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination storage location',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementDestination',
    @level2type = N'COLUMN',
    @level2name = N'DestinationStorageLocationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementDestination',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination product type',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementDestination',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProductTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementDestination',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementDestination',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementDestination',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementDestination',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the details for the Movement Destination.',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementDestination',
    @level2type = NULL,
    @level2name = NULL