/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-09-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data for Elements and related categories. This is a master table and contains seeded data. </Description>
==================================================================================================================================*/
CREATE TABLE [Admin].[CategoryElement]
(
	--Columns
    [ElementId]						INT             IDENTITY (1001, 1)  NOT NULL,
    [Name]							NVARCHAR (150)						NOT NULL,
    [Description]					NVARCHAR (1000)						NULL,
    [CategoryId]					INT									NOT NULL,
    [IsActive]						BIT									NOT NULL	DEFAULT 1,
	[IconId]						INT									NULL,
	[Color]							NVARCHAR(20)						NULL,
    [IsOperationalSegment]          BIT                                 NULL,
    [DeviationPercentage]           DECIMAL(18, 2)                      NULL,
	[RowVersion]					ROWVERSION,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)						NOT NULL,
	[CreatedDate]					DATETIME							NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)						NULL,
	[LastModifiedDate]				DATETIME							NULL,

	--Constraints
    CONSTRAINT [PK_CategoryElements]				PRIMARY KEY CLUSTERED ([ElementId] ASC),
    CONSTRAINT [FK_CategoryElement_Category]		FOREIGN KEY ([CategoryId])			REFERENCES [Admin].[Category] ([CategoryId]),
	CONSTRAINT [UQ_CategoryElement_Name_CategoryId] UNIQUE NONCLUSTERED ([Name], [CategoryId]),
    CONSTRAINT [FK_CategoryElement_Icon]		FOREIGN KEY ([IconId])			REFERENCES [Admin].[Icon] ([IconId])
);
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for elements and related category. This is a master table and contains seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = N'COLUMN',
    @level2name = N'ElementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the category to which the element belongs (segment, system, type of node, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = N'COLUMN',
    @level2name = N'CategoryId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the element is active or not, 1 means active',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = N'COLUMN',
    @level2name = N'IsActive'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The icon representing the element, relates to the Icon table',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = N'COLUMN',
    @level2name = N'IconId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The color representing the element in hex',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = N'COLUMN',
    @level2name = N'Color'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version column used for consistency',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = N'COLUMN',
    @level2name = N'RowVersion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag for a segment to to run the cutoff, ownership calculation, and operational deltas processes',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = N'COLUMN',
    @level2name = N'IsOperationalSegment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The segment deviation percentage identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'CategoryElement',
    @level2type = N'COLUMN',
    @level2name = N'DeviationPercentage'