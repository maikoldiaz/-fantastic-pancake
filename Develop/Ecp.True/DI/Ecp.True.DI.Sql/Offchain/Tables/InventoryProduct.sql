/*-- ==============================================================================================================================
-- Author:      Microsoft  
-- Create date: Nov-11-2019
-- Update date: Mar-27-2020
 <Description>: This table holds the details for association between inventories, products and their blockchain registration.  </Description>
-- ================================================================================================================================*/


CREATE TABLE [Offchain].[InventoryProduct]
(
	--Columns
	[InventoryProductId]								INT IDENTITY (1, 1)		NOT NULL,
	[ProductId]											NVARCHAR (20)			NOT NULL,
	[ProductType]										INT         			NULL,
	[ProductVolume]								        DECIMAL(18, 2)			NULL,
    [GrossStandardQuantity]								DECIMAL(18, 2)			NULL,
	[MeasurementUnit]									INT         			NULL,
	[UncertaintyPercentage]								DECIMAL(5, 2)			NULL,
	[OwnershipTicketId]									INT						NULL,
	[ReasonId]											INT						NULL,
	[Comment]											NVARCHAR(200)			NULL,
	[BlockchainStatus]									INT						NOT NULL,
	[BlockchainInventoryProductTransactionId]			UniqueIdentifier		NULL,
	[PreviousBlockchainInventoryProductTransactionId]	UniqueIdentifier		NULL,
	[TransactionHash]									NVARCHAR(255)			NULL,
	[BlockNumber]										NVARCHAR(255)			NULL,
	[RetryCount]										INT						NOT NULL			DEFAULT 0,

	-- Inventory Columns
	[SystemTypeId]										INT						NOT NULL,
	[DestinationSystem]									NVARCHAR (25)			NOT NULL,
	[EventType]											NVARCHAR (10)			NOT NULL,
	[TankName]											NVARCHAR (20)			NULL,
	[InventoryId]										VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
	[TicketId]											INT						NULL,
	[InventoryDate]										DATE    				NOT NULL,
	[NodeId]											INT						NOT NULL,
	[SegmentId]											INT						NULL,
	[Observations]										NVARCHAR (200)			NULL,
	[ScenarioId]										INT			            NOT NULL,
	[IsDeleted]											BIT						NOT NULL	DEFAULT 0,		--> 1=Deleted
	[FileRegistrationTransactionId]						INT						NULL,
	[OperatorId]									    INT			            NULL,
	[BatchId]											NVARCHAR(150)			NULL,
	[InventoryProductUniqueId]							NVARCHAR(150)			NULL,
    [SystemId]                                          INT                     NULL,
    [Version]                                           NVARCHAR(50)            NULL,
    [SourceSystemId]                                    INT                     NULL,
    [DeltaTicketId]                                     INT                     NULL,
    [OfficialDeltaTicketId]                             INT                     NULL,

	--Internal Common Columns
	[CreatedBy]											NVARCHAR (260)			NOT NULL,
	[CreatedDate]										DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]									NVARCHAR (260)			NULL,
	[LastModifiedDate]									DATETIME				NULL,

	--Constraints
	CONSTRAINT [PK_InventoryProduct]							PRIMARY KEY CLUSTERED ([InventoryProductId] ASC),
	CONSTRAINT [FK_InventoryProduct_Product]					FOREIGN KEY	([ProductId])							REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [FK_InventoryProduct_Ticket]						FOREIGN KEY([OwnershipTicketId])					REFERENCES [Admin].[Ticket] ([TicketId]),
	CONSTRAINT [FK_InventoryProduct_CategoryElement_Reason]		FOREIGN KEY ([ReasonId])							REFERENCES [Admin].[CategoryElement]([ElementId]),
	CONSTRAINT [FK_InventoryProduct_Node]								FOREIGN KEY	([NodeId])				        REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_InventoryProduct_Ticket_TicketId]					FOREIGN KEY ([TicketId])			        REFERENCES [Admin].[Ticket]([TicketId]),
	CONSTRAINT [FK_InventoryProduct_SystemType]							FOREIGN KEY	([SystemTypeId])		        REFERENCES [Admin].[SystemType] ([SystemTypeId]),
	CONSTRAINT [FK_InventoryProduct_FileRegistrationTransaction]		FOREIGN KEY	([FileRegistrationTransactionId])		REFERENCES [Admin].[FileRegistrationTransaction] ([FileRegistrationTransactionId]),
	CONSTRAINT [FK_InventoryProduct_CategoryElement_SegmentId]		    FOREIGN KEY ([SegmentId])    		        REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_InventoryProduct_ScenarioType]					    FOREIGN KEY ([ScenarioId])    		        REFERENCES [Admin].[ScenarioType] ([ScenarioTypeId]),
    CONSTRAINT [FK_InventoryProduct_CategoryElement_SystemId]		    FOREIGN KEY ([SystemId])			        REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_InventoryProduct_CategoryElement_OperatorId]		    FOREIGN KEY ([OperatorId])    		        REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_InventoryProduct_CategoryElement_SourceSystemId]		FOREIGN KEY ([SourceSystemId])              REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_InventoryProduct_Ticket_Delta]		                FOREIGN KEY ([DeltaTicketId])		        REFERENCES [Admin].[Ticket] ([TicketId]),
    CONSTRAINT [FK_InventoryProduct_Ticket_OfficialDelta]		        FOREIGN KEY ([OfficialDeltaTicketId])       REFERENCES [Admin].[Ticket] ([TicketId]),
    CONSTRAINT [FK_InventoryProduct_CategoryElement_ProductType]		FOREIGN KEY ([ProductType])                 REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_InventoryProduct_CategoryElement_MeasurementUnit]	FOREIGN KEY ([MeasurementUnit])             REFERENCES [Admin].[CategoryElement] ([ElementId])
);
GO

CREATE NONCLUSTERED INDEX NCI_InventoryProduct_InventoryDetailsWithoutCutOffForSegment
ON [Offchain].[InventoryProduct] ([SegmentId],[ScenarioId],[IsDeleted],[InventoryDate])
INCLUDE ([ProductId],[ProductVolume],[GrossStandardQuantity],[MeasurementUnit],[UncertaintyPercentage],
[EventType],[TankName],[InventoryId],[NodeId],[BatchId],[SourceSystemId]);
GO

CREATE NONCLUSTERED INDEX NCI_InventoryProduct_InventoryProductUniqueId
ON [Offchain].[InventoryProduct] ([InventoryProductUniqueId]);
GO

CREATE NONCLUSTERED INDEX NCI_InventoryProduct_FileRegistrationTransactionId_InventoryId
ON [Offchain].[InventoryProduct] ([FileRegistrationTransactionId])
INCLUDE ([InventoryId]);
GO

CREATE NONCLUSTERED INDEX NCI_InventoryProduct_SegmentId_IsDeleted_OperationalDate
ON [Offchain].[InventoryProduct] ([SegmentId],[IsDeleted],[InventoryDate])
INCLUDE ([NodeId]);
GO

CREATE NONCLUSTERED INDEX NCI_InventoryProduct_SegmentId_TicketId_NodeId--
ON [Offchain].[InventoryProduct] ([SegmentId],[TicketId])
INCLUDE ([NodeId]);
GO

CREATE NONCLUSTERED INDEX NCI_InventoryProduct_ScenarioId_InventoryDate_NodeId_Include
ON [Offchain].[InventoryProduct] ([ScenarioId],[InventoryDate],[NodeId])
INCLUDE ([ProductId],[ProductVolume],[MeasurementUnit],[UncertaintyPercentage],[InventoryId])
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier ot the inventory product',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the type of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'ProductType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The volumen of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'ProductVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the measurement unit (category element of unit category, like Bbl)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the uncertainty percentage',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'UncertaintyPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ownership ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipTicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the reason (category element of reason category)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'ReasonId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if present in blockchain register',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'BlockchainStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The GUID of the blockchain inventory product transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'BlockchainInventoryProductTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The GUID of the previous blockchain inventory product transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'PreviousBlockchainInventoryProductTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the transaction hash',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'TransactionHash'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the block number',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'BlockNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the retry count',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'RetryCount'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the system type (Sinoper, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'SystemTypeId'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination system',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'DestinationSystem'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the event (like Insert, Update, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the tank ',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'TankName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of inventory (Inventory table is now deprecated)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'InventoryId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the inventory was taken',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'InventoryDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the segment (category element of segment category)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The observations of the movement (like Reporte Operativo Cusiana -Fecha)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'Observations'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the scenario (like Operativo)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'ScenarioId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the element is deleted or not, 1 means delete',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'IsDeleted'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the file registration transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'FileRegistrationTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the operator',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'OperatorId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the batch',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the inventory product',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductUniqueId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The comment of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'Comment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for system',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'SystemId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'Version'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for source system (category element of source system category)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystemId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the details for association between inventories, products and their blockchain registration.',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The delta ticket identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'DeltaTicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The official delta ticket identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'OfficialDeltaTicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The gross standard quantity',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'InventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardQuantity'