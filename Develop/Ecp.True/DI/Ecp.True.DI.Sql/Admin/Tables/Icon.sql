/*==============================================================================================================================
--Author:        Microsoft
--Created date : Feb-14-2020
--Updated date : Mar-20-2020
--<Description>: This table stores svg data for Icons of an element. This is a master table and contains seeded data. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[Icon]
(
	--Columns
	[IconId]							INT IDENTITY (101, 1)		NOT NULL,
	[Name]								NVARCHAR(50)				NOT NULL,
	[Content]							NVARCHAR(MAX)				NOT NULL,

	 --Internal Common Columns
	[CreatedBy]				NVARCHAR (260)			NOT NULL,
	[CreatedDate]			DATETIME				NOT NULL DEFAULT (Admin.udf_GetTrueDate()),
	[LastModifiedBy]		NVARCHAR (260)			NULL,
	[LastModifiedDate]		DATETIME				NULL,

	--Constraints
    CONSTRAINT [PK_Icon]						PRIMARY KEY CLUSTERED ([IconId] ASC),
	CONSTRAINT [UC_Icon_Name]					UNIQUE NONCLUSTERED ([Name] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table stores svg data for Icons of an element. This is a master table and contains seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Icon',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the icon',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Icon',
    @level2type = N'COLUMN',
    @level2name = N'IconId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the icon (for accessing)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Icon',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The svg data of the icon',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Icon',
    @level2type = N'COLUMN',
    @level2name = N'Content'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Icon',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Icon',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Icon',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Icon',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'