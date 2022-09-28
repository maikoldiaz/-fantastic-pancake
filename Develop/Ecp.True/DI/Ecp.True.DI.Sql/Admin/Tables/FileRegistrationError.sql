/*==============================================================================================================================
--Author:        Microsoft
--Created Date : Sep-20-2019
--Updated Date : Mar-20-2020
--<Description>: This table is to capture errors encountered during file registration process. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[FileRegistrationError]
(
    --Columns
    [FileRegistrationErrorId]        INT IDENTITY (1, 1) NOT NULL,
    [FileRegistrationId]             INT NOT NULL,    
    [ErrorMessage]                   NVARCHAR (MAX) NOT NULL, 
	[MessageId]                      NVARCHAR (50) NULL, 
    
    --Internal Common Columns
    [CreatedBy]                     NVARCHAR (260)   NOT NULL,
    [CreatedDate]                   DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),

    --Constraints
    CONSTRAINT [PK_FileRegistrationError]							PRIMARY KEY CLUSTERED ([FileRegistrationErrorId] ASC),
    CONSTRAINT [FK_FileRegistrationError_FileRegistration]			FOREIGN KEY ([FileRegistrationId])						REFERENCES [Admin].[FileRegistration] ([FileRegistrationId])
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the file registration error',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationError',
    @level2type = N'COLUMN',
    @level2name = N'FileRegistrationErrorId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the file registration to which this error is for.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationError',
    @level2type = N'COLUMN',
    @level2name = N'FileRegistrationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The error message',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationError',
    @level2type = N'COLUMN',
    @level2name = N'ErrorMessage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationError',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationError',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The upload identifier (QU1RIEVBSTAyLkQuUU0gIF5Yh0wg93My for /true/sinoper/xml/inventory/QU1RIEVBSTAyLkQuUU0gIF5Yh0wg93My)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationError',
    @level2type = N'COLUMN',
    @level2name = N'MessageId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table is to capture errors encountered during file registration process.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FileRegistrationError',
    @level2type = NULL,
    @level2name = NULL