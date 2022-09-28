/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-08-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data for different categories in the system. This is a master table and contains seeded data. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[Category] 
(
	--Columns
    [CategoryId]					INT IDENTITY (201, 1)	NOT NULL,
    [Name]							NVARCHAR (150)		    NOT NULL,
    [Description]					NVARCHAR (1000)		    NULL,
    [IsActive]						BIT					    NOT NULL    DEFAULT 1,
    [IsGrouper]						BIT					    NOT NULL    DEFAULT 0,
	[IsReadOnly]					BIT					    NOT NULL    DEFAULT 1,
	[IsHomologation]				BIT						NOT NULL	DEFAULT 0,
	[RowVersion]					ROWVERSION,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)			NOT NULL,
	[CreatedDate]					DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)			NULL,
	[LastModifiedDate]				DATETIME				NULL,

	--Constraints
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([CategoryId] ASC),
    CONSTRAINT [UC_Category] UNIQUE NONCLUSTERED ([Name] ASC)
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Category',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Category',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Category',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Category',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version column used for consistency',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Category',
    @level2type = N'COLUMN',
    @level2name = N'RowVersion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Category',
    @level2type = N'COLUMN',
    @level2name = N'CategoryId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Category',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description of the category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Category',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the category is active or not, 1 means active',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Category',
    @level2type = N'COLUMN',
    @level2name = N'IsActive'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the category is readonly or can be modified',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Category',
    @level2type = N'COLUMN',
    @level2name = N'IsReadOnly'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for different categories in the system. This is a master table and contains seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Category',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Defines a category as a grouper of nodes',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Category',
    @level2type = N'COLUMN',
    @level2name = N'IsGrouper'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the category is homologated, 1 for yes',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Category',
    @level2type = N'COLUMN',
    @level2name = N'IsHomologation'