/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-30-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data for homologationGroupId and their source, destination values.</Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[HomologationDataMapping]
(
	--Columns
    [HomologationDataMappingId]			INT IDENTITY (1, 1) NOT NULL,
    [SourceValue]						NVARCHAR (100)		NOT NULL,
    [DestinationValue]					NVARCHAR (100)		NOT NULL,
	[HomologationGroupId]				INT					NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_HomologationDataMapping]						PRIMARY KEY CLUSTERED ([HomologationDataMappingId] ASC),
	CONSTRAINT [FK_HomologationDataMapping_HomologationGroup]	FOREIGN KEY	([HomologationGroupId])							REFERENCES [Admin].[HomologationGroup] ([HomologationGroupId])
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of homologation data mapping',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationDataMapping',
    @level2type = N'COLUMN',
    @level2name = N'HomologationDataMappingId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source  ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationDataMapping',
    @level2type = N'COLUMN',
    @level2name = N'SourceValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationDataMapping',
    @level2type = N'COLUMN',
    @level2name = N'DestinationValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the homologation group',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationDataMapping',
    @level2type = N'COLUMN',
    @level2name = N'HomologationGroupId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationDataMapping',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationDataMapping',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationDataMapping',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationDataMapping',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' This table holds the data for homologationGroupId and their source, destination values.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationDataMapping',
    @level2type = NULL,
    @level2name = NULL