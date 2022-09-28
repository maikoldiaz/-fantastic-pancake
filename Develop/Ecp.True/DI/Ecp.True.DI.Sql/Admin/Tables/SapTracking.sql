CREATE TABLE [Admin].[SapTracking]
(
	[SapTrackingId]							INT IDENTITY (1, 1)		   NOT NULL,
	[MovementTransactionId]					INT						   NULL,
    [FileRegistrationId]                    INT                        NULL,
	[StatusTypeId]							INT						   NOT NULL,
	[OperationalDate]						DATETIME				   NULL,
	[ErrorMessage]							NVARCHAR(MAX)			   NULL,		
	[SessionId]                             NVARCHAR(50)               NULL,
    [Comment]                               NVARCHAR(1000)             NULL,
    [BlobPath]                              NVARCHAR(256)              NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)			   NOT NULL,
	[CreatedDate]					DATETIME				   NOT NULL     DEFAULT Admin.udf_GetTrueDate(),
    [LastModifiedBy]				NVARCHAR (260)				NULL,
	[LastModifiedDate]				DATETIME					NULL,

    --Constraints
    CONSTRAINT [PK_SapTracking]				                PRIMARY KEY CLUSTERED ([SapTrackingId] ASC),
	CONSTRAINT [FK_SapTracking_StatusType]	                FOREIGN KEY([StatusTypeId])			            REFERENCES [Admin].[StatusType] ([StatusTypeId]),
	CONSTRAINT [FK_SapTracking_Movement]	                FOREIGN KEY([MovementTransactionId])			REFERENCES [Offchain].[Movement] ([MovementTransactionId]),
	CONSTRAINT [FK_SapTracking_FileRegistration]	        FOREIGN KEY([FileRegistrationId])	            REFERENCES [Admin].[FileRegistration] ([FileRegistrationId])
)
GO

CREATE NONCLUSTERED INDEX NCI_MovementTransactionId_ErrorMessage
ON [Admin].[SapTracking] ([MovementTransactionId],[SessionId])
INCLUDE ([ErrorMessage])
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of sap tracking record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTracking',
    @level2type = N'COLUMN',
    @level2name = N'SapTrackingId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The transaction identifier of movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTracking',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the status of the transaction (Processing, failed, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTracking',
    @level2type = N'COLUMN',
    @level2name = N'StatusTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The send to SAP date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTracking',
    @level2type = N'COLUMN',
    @level2name = N'OperationalDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The error message',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTracking',
    @level2type = N'COLUMN',
    @level2name = N'ErrorMessage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the session',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTracking',
    @level2type = N'COLUMN',
    @level2name = N'SessionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The note',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTracking',
    @level2type = N'COLUMN',
    @level2name = N'Comment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTracking',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTracking',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of file registration',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTracking',
    @level2type = N'COLUMN',
    @level2name = N'FileRegistrationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTracking',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTracking',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for SAP PO Process status.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTracking',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The blob path',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTracking',
    @level2type = N'COLUMN',
    @level2name = N'BlobPath'