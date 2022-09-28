/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Nov-21-2019
-- Updated Date:	Mar-20-2020
-- Updated Date:	Oct-05-2020  Adding indexes to improve the query performance
 <Description>:		This table holds the data for the Ownership with owner, movement, inventory, blockchain information and volume, percentage.  </Description>

-- ================================================================================================================================*/
CREATE TABLE [Offchain].[Ownership]
(	
	--Columns
	[OwnershipId]								INT	IDENTITY (1, 1)	NOT NULL,
	[MessageTypeId]								INT					NOT NULL,
	[TicketId]									INT					NOT NULL,
	[MovementTransactionId] 					INT					NULL,
	[InventoryProductId]						INT					NULL,
	[OwnerId]									INT					NOT NULL,
	[OwnershipPercentage]						DECIMAL(5, 2)		NOT NULL,
	[OwnershipVolume]							DECIMAL(18, 2)		NOT NULL,
	[AppliedRule]								NVARCHAR(50)		NOT NULL,
	[RuleVersion]								NVARCHAR(20)		NOT NULL,
	[ExecutionDate]								DATETIME			NOT NULL,
	[BlockchainStatus]							INT					NOT NULL,
	[BlockchainMovementTransactionId]			UniqueIdentifier	NULL,
	[BlockchainInventoryProductTransactionId]	UniqueIdentifier	NULL,
	[IsDeleted]									BIT					NOT NULL	DEFAULT(0),--0= not deleted; 1 = Deleted
	[EventType]									NVARCHAR (25)		NULL,
	[BlockchainOwnershipId]						UniqueIdentifier	NULL,
	[PreviousBlockchainOwnershipId]             UniqueIdentifier    NULL,
	[TransactionHash]							NVARCHAR(255)		NULL,
	[BlockNumber]								NVARCHAR(255)		NULL,
	[RetryCount]								INT					NOT NULL	DEFAULT 0,
    [DeltaTicketId]                             INT                 NULL,

	--Internal Common Columns
	[CreatedBy]									NVARCHAR (260)		NOT NULL,
	[CreatedDate]								DATETIME			NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]							NVARCHAR (260)		NULL,
	[LastModifiedDate]							DATETIME			NULL,

	--Constraints
	CONSTRAINT [PK_Ownership]					PRIMARY KEY CLUSTERED ([OwnershipId] ASC),
	CONSTRAINT [PK_Ownership_MessageType]		FOREIGN KEY (MessageTypeId)				REFERENCES [Admin].[MessageType]([MessageTypeId]),
	CONSTRAINT [FK_Ownership_InventoryProduct]	FOREIGN KEY (InventoryProductId)		REFERENCES [Offchain].[InventoryProduct]([InventoryProductId]),
	CONSTRAINT [FK_Ownership_Movement]			FOREIGN KEY (MovementTransactionId)		REFERENCES [Offchain].[Movement]([MovementTransactionId]),
	CONSTRAINT [FK_Ownership_Ticket]			FOREIGN KEY (TicketId)					REFERENCES [Admin].[Ticket]([TicketId]),
	CONSTRAINT [FK_Ownership_CategoryElement]	FOREIGN KEY ([OwnerId])					REFERENCES [Admin].[CategoryElement]([ElementId]),
    CONSTRAINT [FK_Ownership_Ticket_Delta]		FOREIGN KEY ([DeltaTicketId])			REFERENCES [Admin].[Ticket] ([TicketId])
)


GO
CREATE NONCLUSTERED INDEX NCIX_Ownership_MovTranId 
ON [Offchain].[Ownership] (MovementTransactionId)
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ownershipid',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the message type (like Movement, Inventory, Event, etc.)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'MessageTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the inventory product',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the owner (category element of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'OwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the rule applied (comes from algorithm analytics api)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'AppliedRule'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version of the rule (comes from algorithm analytics api)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'RuleVersion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is executed',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if present in blockchain register',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'BlockchainStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the blockchain movement transaction ',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'BlockchainMovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the blockchain inventory product transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'BlockchainInventoryProductTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the record is delete or not, 1 means delete',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'IsDeleted'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the event (like Insert, Update)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the blockchain ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'BlockchainOwnershipId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the previous blockchain ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'PreviousBlockchainOwnershipId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The transaction hash',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'TransactionHash'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The block number',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'BlockNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The number of retries',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'RetryCount'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for the Ownership with owner, movement, inventory, blockchain information and volume, percentage.',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The delta ticket identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Ownership',
    @level2type = N'COLUMN',
    @level2name = N'DeltaTicketId'