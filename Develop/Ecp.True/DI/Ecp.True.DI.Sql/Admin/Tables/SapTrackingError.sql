/*==============================================================================================================================
--Author:        Microsoft
--Created Date : Aug-17-2020
--Updated Date : 
--<Description>: This table holds the data for sap tracking errors.  </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[SapTrackingError]
(
	[SapTrackingErrorId]			INT IDENTITY (1, 1)		   NOT NULL,
    [SapTrackingId]					INT                 	   NOT NULL,
    [ErrorCode]					    NVARCHAR(5)                NOT NULL,
    [ErrorDescription]              NVARCHAR(MAX)              NOT NULL,

	--Internal Common Columns
	[CreatedBy]					    NVARCHAR (260)			NOT NULL,
	[CreatedDate]				    DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
    [LastModifiedBy]				NVARCHAR (260)				NULL,
	[LastModifiedDate]				DATETIME					NULL,

	--Constraints
	CONSTRAINT [PK_SapTrackingError]					PRIMARY KEY CLUSTERED ([SapTrackingErrorId] ASC),
	CONSTRAINT [FK_SapTrackingError_SapTracking]	    FOREIGN KEY ([SapTrackingId])		                    REFERENCES [Admin].[SapTracking] ([SapTrackingId]),
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of sap tracking error',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTrackingError',
    @level2type = N'COLUMN',
    @level2name = N'SapTrackingErrorId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of sap tracking',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTrackingError',
    @level2type = N'COLUMN',
    @level2name = N'SapTrackingId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The error message',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTrackingError',
    @level2type = N'COLUMN',
    @level2name = N'ErrorDescription'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The error code',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTrackingError',
    @level2type = N'COLUMN',
    @level2name = N'ErrorCode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTrackingError',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTrackingError',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTrackingError',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTrackingError',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for sap tracking errors.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapTrackingError',
    @level2type = NULL,
    @level2name = NULL