/*==============================================================================================================================
--Author:        Microsoft
--Created Date : Sep-20-2019 
--Updated Date : Mar-20-2020
                 May-15-2020 Removed RecordsProcessed, IsActive columns
--<Description>: This table is to capture file registration details for uploaded, updated, removed, etc files. </Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[FileRegistration] 
(
    --Columns
     [FileRegistrationId]				INT IDENTITY(1, 1) NOT NULL, 
     [UploadId]							NVARCHAR(50) NOT NULL, 
     [UploadDate]						DATETIME NOT NULL DEFAULT Admin.udf_GetTrueDate(), 
     [Name]								NVARCHAR(500) NOT NULL, 
     [Action]							INT NOT NULL, 
     [Status]							INT NOT NULL, 
     [SystemTypeId]						INT NOT NULL, 
	 [SegmentId]						INT NULL,
     [BlobPath]							NVARCHAR(MAX) NOT NULL, 
     [HomologationInventoryBlobPath]	NVARCHAR(MAX) NULL,
	 [HomologationMovementBlobPath]		NVARCHAR(MAX) NULL,
     [PreviousUploadId]					UNIQUEIDENTIFIER NULL, 
	 [IsParsed]							BIT NULL,		--> Processed = 0; Processing = 1 
     [SourceSystem]						VARCHAR(50) NULL, 
     [SourceTypeId]                     INT NULL,
     [IntegrationType]                  INT NULL,
    --Internal Common Columns
    [CreatedBy]							NVARCHAR (260)   NOT NULL,
    [CreatedDate]						DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
    [LastModifiedBy]					NVARCHAR (260)   NULL,
    [LastModifiedDate]					DATETIME        NULL,
    
    
    --Constraints
    CONSTRAINT [PK_FileRegistration]                                	PRIMARY KEY CLUSTERED ([FileRegistrationId] ASC),
    CONSTRAINT [UC_FileRegistration]                                	UNIQUE NONCLUSTERED ([UploadId] ASC),
    CONSTRAINT [FK_FileRegistrationError_FileUploadState]            	FOREIGN KEY ([Status])        						REFERENCES [Admin].[FileUploadState] ([FileUploadStateId]),
    CONSTRAINT [FK_FileRegistrationError_RegisterFileActionType]    	FOREIGN KEY([Action])        						REFERENCES [Admin].[RegisterFileActionType] ([ActionTypeId]),
    CONSTRAINT [FK_FileRegistrationError_SystemType]                	FOREIGN KEY([SystemTypeId])    						REFERENCES [Admin].[SystemType] ([SystemTypeId]),
	CONSTRAINT [FK_FileRegistrationError_CategoryElement]               FOREIGN KEY([SegmentId])    						REFERENCES [Admin].[CategoryElement] ([ElementId])
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the fileregistration',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'FileRegistrationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The GUID of upload',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'UploadId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date of the upload of file',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'UploadDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the file uploaded',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for the file action type (Insert, Update, Remove, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'Action'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for the file upload state (Processing, failed, etc.)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the system (excel, etc.) for the file',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'SystemTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the category element of segment category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The blob path for this file',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'BlobPath'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table is to capture file registration details for uploaded, updated, removed, etc files.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The blob path if the file registration is of homologated inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'HomologationInventoryBlobPath'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The blob path if the file registration is of homologated movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'HomologationMovementBlobPath'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Flag to signify whether canonical transformation is successful or not',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'IsParsed'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The previous upload id of the same file registration',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'PreviousUploadId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name or identifier of source system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystem'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Identifier of the source type (Compras,Ventas,movimientos,Inventarios)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistration',
    @level2type = N'COLUMN',
    @level2name = N'SourceTypeId'