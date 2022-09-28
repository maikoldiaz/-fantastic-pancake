/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jul-30-2020
--Updated date : 
--<Description>: This table holds the types of reports. This is a master table and has seeded data. </Description>
===============================================================================================================================*/
CREATE TABLE [Admin].[ReportType]
(
	--Columns
	[ReportTypeId]			INT IDENTITY (101, 1)	NOT NULL,
	[Name]			        VARCHAR(50)				NOT NULL,

	 --Internal Common Columns
	[CreatedBy]				NVARCHAR (260)			NOT NULL,
	[CreatedDate]			DATETIME				NOT NULL DEFAULT (Admin.udf_GetTrueDate()),

	--Constraints
    CONSTRAINT [PK_ReportType]	PRIMARY KEY CLUSTERED ([ReportTypeId] ASC),
	CONSTRAINT [UC_ReportType] UNIQUE NONCLUSTERED    ([Name] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the report type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportType',
    @level2type = N'COLUMN',
    @level2name = N'ReportTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the report',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportType',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the types of reports. This is a master table and has seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportType',
    @level2type = NULL,
    @level2name = NULL