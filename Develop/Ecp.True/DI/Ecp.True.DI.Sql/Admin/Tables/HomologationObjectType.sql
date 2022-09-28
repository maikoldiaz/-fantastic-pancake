/*==============================================================================================================================
--Author:        Microsoft
--Created date : Sep-08-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data for type of Homologation Objects (OwnerId, NodeId, ProductId, etc.). This is a master table and contains seeded data. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[HomologationObjectType]
(
	--Columns
    [HomologationObjectTypeId]		INT IDENTITY (1001, 1)		NOT NULL,
    [Name]							NVARCHAR (30)			NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_HomologationObjectType] PRIMARY KEY CLUSTERED ([HomologationObjectTypeId] ASC),
	CONSTRAINT [UC_HomologationObjectType] UNIQUE NONCLUSTERED ([Name] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the homologation object type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObjectType',
    @level2type = N'COLUMN',
    @level2name = N'HomologationObjectTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the homologation object type (NodeId, ProductId, MeasurementUnit, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObjectType',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObjectType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObjectType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObjectType',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObjectType',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for type of Homologation Objects (OwnerId, NodeId, ProductId, etc.). This is a master table and contains seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObjectType',
    @level2type = NULL,
    @level2name = NULL