/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Nov-11-2019
-- Updated Date:	Mar-20-2020
 <Description>:		This table holds the data for the Owner with its associated movement, inventory and blockchain status.  </Description>

-- ================================================================================================================================*/
CREATE TABLE [Offchain].[Owner]
(
	--Columns
	[Id]										INT IDENTITY (1, 1)		NOT NULL,
	[OwnerId]									INT         			NOT NULL,
	[OwnershipValue]							DECIMAL(18, 2)			NOT NULL,
	[OwnershipValueUnit]						NVARCHAR (50)			NOT NULL,
	[InventoryProductId]						INT						NULL,
	[MovementTransactionId]						INT						NULL,
	[BlockchainStatus]							INT						NOT NULL,
	[BlockchainMovementTransactionId]			UniqueIdentifier		NULL,
	[BlockchainInventoryProductTransactionId]	UniqueIdentifier		NULL,
    [TransactionHash]							NVARCHAR(255)		    NULL,
	[BlockNumber]								NVARCHAR(255)		    NULL,
	[RetryCount]								INT					    NOT NULL	    DEFAULT 0,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
	CONSTRAINT [PK_Owner]								PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Owner_InventoryProduct]				FOREIGN KEY ([InventoryProductId])								REFERENCES [Offchain].[InventoryProduct] ([InventoryProductId]),
	CONSTRAINT [FK_Owner_Movement]						FOREIGN KEY ([MovementTransactionId])							REFERENCES [Offchain].[Movement] ([MovementTransactionId]),
    CONSTRAINT [FK_Owner_OwnerId]						FOREIGN KEY ([OwnerId])							                REFERENCES [Admin].[CategoryElement] ([ElementId])


);
GO

CREATE NONCLUSTERED INDEX NCI_OWNER_InventoryProductId
ON [Offchain].[Owner] (InventoryProductId,OwnerId)
INCLUDE(OwnershipValue,OwnershipValueUnit);
GO

CREATE NONCLUSTERED INDEX Owner_MovementTransactionid_IncludeClass
ON [Offchain].[Owner] ([MovementTransactionId],[InventoryProductId])
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the owner',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'OwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the ownership in the unit',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name or identifier of the unit (category element of unit category, like Bbl)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipValueUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the inventory product',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if present in blockchain register',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'BlockchainStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the blockchain movement transaction ',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'BlockchainMovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the blockchain inventory product transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'BlockchainInventoryProductTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for the Owner with its associated movement, inventory and blockchain status.',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The hash of the transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'TransactionHash'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The block number from blockchain',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'BlockNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The number of retries',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Owner',
    @level2type = N'COLUMN',
    @level2name = N'RetryCount'