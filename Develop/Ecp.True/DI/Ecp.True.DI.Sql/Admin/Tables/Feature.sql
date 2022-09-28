/*==============================================================================================================================
--Author:        Microsoft
--Created date : Oct-16-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data for Feature (sub menu in the nav bar under scenario/main menu). This is a master table and contains seeded data. </Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[Feature]
(
	--Columns
	[FeatureId]			INT IDENTITY (101, 1)		NOT NULL,
	[ScenarioId]		INT						NOT NULL,
	[Name]				NVARCHAR (50)			NOT NULL,
    [description]       NVARCHAR (250)			NULL,
	[Sequence]			INT						NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),

	--Constraints
	CONSTRAINT [PK_Feature]									PRIMARY KEY CLUSTERED ([FeatureId] ASC),
	CONSTRAINT [FK_Feature_Scenario_ScenarioId]            	FOREIGN KEY ([ScenarioId])        						REFERENCES [Admin].[Scenario] ([ScenarioId])
)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the feature (sub menu in the nav bar under scenario/main menu)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Feature',
    @level2type = N'COLUMN',
    @level2name = N'FeatureId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the scenario (main menu like for administration(Administración))',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Feature',
    @level2type = N'COLUMN',
    @level2name = N'ScenarioId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the feature(sub menu)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Feature',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The number signifying the sequence of the submenu under it''s parent menu',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Feature',
    @level2type = N'COLUMN',
    @level2name = N'Sequence'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Feature',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Feature',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for Feature (sub menu in the nav bar under scenario/main menu). This is a master table and contains seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Feature',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name traslate of the feature(sub menu)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Feature',
    @level2type = N'COLUMN',
    @level2name = N'description'
