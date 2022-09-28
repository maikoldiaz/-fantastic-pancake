/*==============================================================================================================================
--Author:        Microsoft
--Created date : Nov-06-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data for transactions of file registration. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[FileRegistrationTransaction]
(
	--Columns
	[FileRegistrationTransactionId]			INT IDENTITY (1, 1)		NOT NULL, 
	[FileRegistrationId]					INT						NOT NULL,
	[BlobPath]								NVARCHAR(MAX)			NULL, 
	[SessionId]								NVARCHAR(MAX)			NULL, 
	[StatusTypeId]							INT						NOT NULL, 
	[RecordID]								NVARCHAR(250)			NULL,

	 --Internal Common Columns
	[CreatedBy]								NVARCHAR (260)			NOT NULL,
	[CreatedDate]							DATETIME				NOT NULL DEFAULT (Admin.udf_GetTrueDate()),
    [LastModifiedBy]						NVARCHAR (260)   NULL,
    [LastModifiedDate]						DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_FileRegistrationTransaction]						PRIMARY KEY CLUSTERED ([FileRegistrationTransactionId] ASC),
	CONSTRAINT [FK_FileRegistrationTransaction_FileRegistration]    FOREIGN KEY([FileRegistrationId])    			REFERENCES [Admin].[FileRegistration] ([FileRegistrationId]),
	CONSTRAINT [FK_FileRegistrationTransaction_StatusType]			FOREIGN KEY([StatusTypeId])    					REFERENCES [Admin].[StatusType] ([StatusTypeId])
)
GO
CREATE NONCLUSTERED INDEX NCI_FileRegistrationTransaction_FileRegistrationId
ON [Admin].[FileRegistrationTransaction] ([FileRegistrationId]);
GO

CREATE NONCLUSTERED INDEX NCI_FileRegistrationTransaction_StatusTypeId
ON [Admin].[FileRegistrationTransaction] ([StatusTypeId])
INCLUDE ([FileRegistrationId])
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of fileregistrations belonging to this transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationTransaction',
    @level2type = N'COLUMN',
    @level2name = N'FileRegistrationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the transaction of file registration',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationTransaction',
    @level2type = N'COLUMN',
    @level2name = N'FileRegistrationTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The path of the blob (like a json/xml for event for a system like sinoper)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationTransaction',
    @level2type = N'COLUMN',
    @level2name = N'BlobPath'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the status of the transaction (Processing, failed, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationTransaction',
    @level2type = N'COLUMN',
    @level2name = N'StatusTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationTransaction',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationTransaction',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationTransaction',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationTransaction',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for transactions of file registration.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationTransaction',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier used for pending transaction to link with this transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationTransaction',
    @level2type = N'COLUMN',
    @level2name = N'RecordID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'A movement id or a unique id generated from inventory product id',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationTransaction',
    @level2type = N'COLUMN',
    @level2name = N'SessionId'