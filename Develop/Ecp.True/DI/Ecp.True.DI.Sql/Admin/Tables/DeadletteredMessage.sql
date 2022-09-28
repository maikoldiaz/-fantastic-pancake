/*-- =================================================================================================
-- Author:		Microsoft
-- Create date: March-27-2020
--<Description>: This Table is to store messages from the deadletter service bus queue.  </Description>
-- ===================================================================================================*/


CREATE TABLE [Admin].[DeadletteredMessage]
(
	--Columns
	[DeadletteredMessageId]			INT IDENTITY (1, 1)			NOT NULL,
	[BlobPath]						NVARCHAR(MAX)				NOT NULL,
	[ProcessName]					NVARCHAR(100)				NOT NULL,
	[QueueName]						NVARCHAR(100)				NOT NULL,
	[Status]						BIT							NOT NULL,
	[ErrorMessage]					NVARCHAR(MAX)				NULL,
    [IsSessionEnabled]              BIT							NOT NULL DEFAULT(0),
    [TicketId]                      INT							NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)			NOT NULL,
	[CreatedDate]					DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)			NULL,
	[LastModifiedDate]				DATETIME				NULL,

	--Constraints
    CONSTRAINT [PK_DeadletteredMessage]		PRIMARY KEY CLUSTERED ([DeadletteredMessageId] ASC)
)
;


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the dead letter queue message',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeadletteredMessage',
    @level2type = N'COLUMN',
    @level2name = N'DeadletteredMessageId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The path of the blob ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeadletteredMessage',
    @level2type = N'COLUMN',
    @level2name = N'BlobPath'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the process (like OwnershipProcessing)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeadletteredMessage',
    @level2type = N'COLUMN',
    @level2name = N'ProcessName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the service bus queue',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeadletteredMessage',
    @level2type = N'COLUMN',
    @level2name = N'QueueName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the message is reprocessed or not',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeadletteredMessage',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The error message',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeadletteredMessage',
    @level2type = N'COLUMN',
    @level2name = N'ErrorMessage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the session is enabled or not, 1 means enable',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeadletteredMessage',
    @level2type = N'COLUMN',
    @level2name = N'IsSessionEnabled'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeadletteredMessage',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeadletteredMessage',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeadletteredMessage',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeadletteredMessage',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeadletteredMessage',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Table is to store messages from the deadletter service bus queue.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeadletteredMessage',
    @level2type = NULL,
    @level2name = NULL