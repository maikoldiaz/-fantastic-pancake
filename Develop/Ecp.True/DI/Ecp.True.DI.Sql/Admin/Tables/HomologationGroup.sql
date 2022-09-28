/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-30-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data for Homologation and categories associated with it.</Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[HomologationGroup]
(
	--Columns
	[HomologationGroupId]			INT IDENTITY (1, 1)		NOT NULL,
	[GroupTypeId]					INT						NOT NULL,
	[HomologationId]				INT						NOT NULL,
	[RowVersion]					ROWVERSION,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
	CONSTRAINT [PK_HomologationGroup]							PRIMARY KEY CLUSTERED ([HomologationGroupId] ASC),
	CONSTRAINT [FK_HomologationGroup_Category]					FOREIGN KEY ([GroupTypeId])							REFERENCES [Admin].[Category] ([CategoryId]),
	CONSTRAINT [FK_HomologationGroup_Homologation]				FOREIGN KEY ([HomologationId])						REFERENCES [Admin].[Homologation] ([HomologationId])
); 

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the homologation group',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationGroup',
    @level2type = N'COLUMN',
    @level2name = N'HomologationGroupId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the group type (CategoryId)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationGroup',
    @level2type = N'COLUMN',
    @level2name = N'GroupTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the homologation ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationGroup',
    @level2type = N'COLUMN',
    @level2name = N'HomologationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationGroup',
    @level2type = N'COLUMN',
    @level2name = N'RowVersion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationGroup',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationGroup',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationGroup',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationGroup',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for Homologation and categories associated with it.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationGroup',
    @level2type = NULL,
    @level2name = NULL