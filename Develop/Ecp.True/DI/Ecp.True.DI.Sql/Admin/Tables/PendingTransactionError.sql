/*==============================================================================================================================
--Author:        Microsoft
--Created date : Sep-05-2020
--Updated date : Mar-20-2020
--<Description>: This table holds the error details for pending transaction.</Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[PendingTransactionError]
(
	--Columns
	[ErrorId]								INT IDENTITY (1, 1)			NOT NULL,	
    [TransactionId]							INT							NULL,
	[RecordID]								NVARCHAR(250)				NULL,
	[ErrorMessage]							NVARCHAR(500)				NOT NULL,
	[Comment]								NVARCHAR(1000)				NULL,
	[IsRetrying]							BIT							NOT NULL	DEFAULT 0, -- 1 Retrying
    [SessionId]                             NVARCHAR(50)                NULL,

	--Internal Common Columns
	[CreatedBy]								NVARCHAR (260)				NOT NULL,
	[CreatedDate]							DATETIME					NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]						NVARCHAR (260)				NULL,
	[LastModifiedDate]						DATETIME					NULL,

	--Constraints
    CONSTRAINT [PK_PendingTransactionError ]	PRIMARY KEY CLUSTERED ([ErrorId] ASC),
    CONSTRAINT [FK_PendingTransactionError_PendingTransaction_TransactionId]		FOREIGN KEY ([TransactionId])			REFERENCES [Admin].[PendingTransaction]([TransactionId])
);
GO

CREATE NONCLUSTERED INDEX NCI_PendingTransactionError_TransactionId_ErrorMessage
ON [Admin].[PendingTransactionError] ([TransactionId])
INCLUDE ([ErrorMessage]);
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the pending transaction error ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransactionError',
    @level2type = N'COLUMN',
    @level2name = N'ErrorId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the transaction ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransactionError',
    @level2type = N'COLUMN',
    @level2name = N'TransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The message of the error ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransactionError',
    @level2type = N'COLUMN',
    @level2name = N'ErrorMessage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The comment for the error provided by the user',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransactionError',
    @level2type = N'COLUMN',
    @level2name = N'Comment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the transaction is currently being retried, 1 means yes',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransactionError',
    @level2type = N'COLUMN',
    @level2name = N'IsRetrying'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransactionError',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransactionError',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransactionError',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransactionError',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The unique identifier used to map to file regisration transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransactionError',
    @level2type = N'COLUMN',
    @level2name = N'RecordID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the error details for pending transaction.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransactionError',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The session identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransactionError',
    @level2type = N'COLUMN',
    @level2name = N'SessionId'