/*==============================================================================================================================
--Author:        Microsoft
--Created date : Sep-05-2020
--Updated date : Mar-20-2020
--<Description>: This table holds the details for PendingTransaction. </Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[PendingTransaction]
(
	--Columns
	[TransactionId]				INT IDENTITY (1, 1)			NOT NULL,
	[MessageTypeId]				INT							NULL,
	[BlobName]					NVARCHAR(500)				NOT NULL,
	[MessageId]					NVARCHAR(50)				NOT NULL,
	[ErrorJson]					NVARCHAR(MAX)				NOT NULL,
    [SourceNodeId]				NVARCHAR(100)				NULL,
	[DestinationNodeId]			NVARCHAR(100)				NULL,
	[SourceProductId]			NVARCHAR(100)				NULL,
	[DestinationProductId]		NVARCHAR(100)				NULL,
	[ActionTypeId]				INT							NULL,
	[Volume]					NVARCHAR(50)				NULL,
	[Units]						INT         				NULL,
	[StartDate]					DATETIME					NULL,
	[EndDate]					DATETIME					NULL,
	[TicketId]					INT							NULL,
	[SystemTypeId]				INT							NULL,
	[SystemName]			    INT					        NULL,
	[OwnerId]					INT							NULL,
	[SegmentId]					INT							NULL,
	[TypeId]					INT							NULL,
	[Identifier]				NVARCHAR(50)				NULL,
	[Type]						NVARCHAR(50)				NULL,
	[Messagetype]				VARCHAR(50)					NULL,
	[ActionType]				VARCHAR(50)					NULL,
    [ScenarioId]                INT                         NULL,                   
	
	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,
	[OriginMessageId]               NVARCHAR(50)    NULL, 
    --Constraints
    CONSTRAINT [PK_PendingTransaction]								PRIMARY KEY CLUSTERED ([TransactionId] ASC),
	CONSTRAINT [FK_PendingTransaction _CategoryElement_ActionType]	FOREIGN KEY ([ActionTypeId])		REFERENCES [Admin].[RegisterFileActionType]([ActionTypeId]),
	CONSTRAINT [FK_PendingTransaction _CategoryElement_TicketId]	FOREIGN KEY ([TicketId])			REFERENCES [Admin].[Ticket]([TicketId]),
	CONSTRAINT [FK_PendingTransaction_MessageType]					FOREIGN KEY	([MessageTypeId])		REFERENCES [Admin].[MessageType] ([MessageTypeId]),
	CONSTRAINT [FK_PendingTransaction_SystemType]					FOREIGN KEY	([SystemTypeId])		REFERENCES [Admin].[SystemType] ([SystemTypeId]),
	CONSTRAINT [FK_PendingTransaction_CategoryElement_OwnerId]		FOREIGN KEY	([OwnerId])				REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_PendingTransaction_CategoryElement_TypeId]		FOREIGN KEY	([TypeId])				REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_PendingTransaction_ScenarioType]	                FOREIGN KEY	([ScenarioId])          REFERENCES [Admin].[ScenarioType] ([ScenarioTypeId]),
    CONSTRAINT [FK_PendingTransaction_CategoryElement_Units]		FOREIGN KEY	([Units])				REFERENCES [Admin].[CategoryElement] ([ElementId])
);
GO

CREATE NONCLUSTERED INDEX NCI_PendingTransaction_MessageId_Identifier
ON [Admin].[PendingTransaction] ([MessageId])
INCLUDE ([Identifier]);
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'TransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the message type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'MessageTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the blob ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'BlobName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the message',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'MessageId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The error json message ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'ErrorJson'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the action type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'ActionTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'Volume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the units',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'Units'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date when the pending transaction is started',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date when the pending transaction is ended',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'EndDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the system type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'SystemTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'OwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the segment',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'TypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'Identifier'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'Type'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the message type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'Messagetype'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the action type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'ActionType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the details for PendingTransaction.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the scenario type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'COLUMN',
    @level2name = N'ScenarioId'