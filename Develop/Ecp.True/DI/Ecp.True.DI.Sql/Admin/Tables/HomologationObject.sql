/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-30-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data about Homologation Object, its type, its group and if it requires mapping.</Description>
================================================================================================================================*/
CREATE TABLE [Admin].[HomologationObject]
(
	--Columns
    [HomologationObjectId]			INT IDENTITY (1, 1) NOT NULL,
    [HomologationObjectTypeId]		INT		NOT NULL,
    [IsRequiredMapping]				BIT					NOT NULL    DEFAULT 1,
	[HomologationGroupId]			INT					NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_HomologationObject]												PRIMARY KEY CLUSTERED ([HomologationObjectId] ASC),
	CONSTRAINT [UC_HomologationObjectTypeId_HomologationGroupId]					UNIQUE NONCLUSTERED ([HomologationObjectTypeId], [HomologationGroupId]),
    CONSTRAINT [FK_HomologationObject_HomologationGroup]							FOREIGN KEY	([HomologationGroupId])											REFERENCES [Admin].[HomologationGroup] ([HomologationGroupId]),
	CONSTRAINT [FK_HomologationObject_HomologationObjectType]						FOREIGN KEY	([HomologationObjectTypeId])									REFERENCES [Admin].[HomologationObjectType] ([HomologationObjectTypeId])
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of homologation object',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObject',
    @level2type = N'COLUMN',
    @level2name = N'HomologationObjectId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of homologation object type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObject',
    @level2type = N'COLUMN',
    @level2name = N'HomologationObjectTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the mapping is required or not, 1 means require',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObject',
    @level2type = N'COLUMN',
    @level2name = N'IsRequiredMapping'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the homologation group',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObject',
    @level2type = N'COLUMN',
    @level2name = N'HomologationGroupId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObject',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObject',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObject',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObject',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data about Homologation Object, its type, its group and if it requires mapping.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObject',
    @level2type = NULL,
    @level2name = NULL