/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jan-05-2020
--Updated date : Mar-20-2020
--<Description>: This table holds types of scenarios. This is a master table and contains seeded data.</Description>
*/
CREATE TABLE [Admin].[ScenarioType]
(
	--Columns
	[ScenarioTypeId]			        INT IDENTITY (1, 1)	    NOT NULL,
	[Name]								VARCHAR(50)				NOT NULL,

	 --Internal Common Columns
	[CreatedBy]				            NVARCHAR (260)			NOT NULL,
	[CreatedDate]			            DATETIME				NOT NULL DEFAULT (Admin.udf_GetTrueDate())

	--Constraints
    CONSTRAINT [PK_ScenarioType]	PRIMARY KEY CLUSTERED ([ScenarioTypeId] ASC),
	CONSTRAINT [UC_ScenarioType]    UNIQUE NONCLUSTERED ([Name] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for OriginType. This is a master table and contains seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ScenarioType',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for the scenario type ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ScenarioType',
    @level2type = N'COLUMN',
    @level2name = N'ScenarioTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The spanish name of the type (like Operativo, Oficial)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ScenarioType',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ScenarioType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ScenarioType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'