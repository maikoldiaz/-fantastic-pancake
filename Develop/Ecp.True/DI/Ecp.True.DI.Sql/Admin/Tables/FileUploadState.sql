/*==============================================================================================================================
--Author:        Microsoft
--Created Date : Sep-21-2019
--Updated Date : Mar-20-2020
--<Description>: This table holds the data for File upload state like Processing, Finalized and Failed. This is a master table and contains seeded data. </Description>
================================================================================================================================*/
CREATE TABLE Admin.FileUploadState
(
    --Columns
	FileUploadStateId		INT					NOT NULL,
	FileUploadState			VARCHAR(50)			NOT NULL,

	 --Internal Common Columns
	[CreatedBy]				NVARCHAR (260)		NOT NULL,
	[CreatedDate]			DATETIME			NOT NULL DEFAULT (Admin.udf_GetTrueDate()),
	[LastModifiedBy]		NVARCHAR (260)		NULL,
	[LastModifiedDate]		DATETIME			NULL,

	--Constraints
    CONSTRAINT [PK_FileUploadState]	PRIMARY KEY CLUSTERED (FileUploadStateId ASC)
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for the file upload state',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileUploadState',
    @level2type = N'COLUMN',
    @level2name = N'FileUploadStateId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name for the state (FINALIZED, PROCESSING, FAILED)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileUploadState',
    @level2type = N'COLUMN',
    @level2name = N'FileUploadState'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileUploadState',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileUploadState',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileUploadState',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileUploadState',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for File upload state like Processing, Finalized and Failed. This is a master table and contains seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileUploadState',
    @level2type = NULL,
    @level2name = NULL