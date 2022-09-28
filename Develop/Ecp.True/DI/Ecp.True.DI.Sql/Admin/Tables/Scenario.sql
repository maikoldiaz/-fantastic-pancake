/*==============================================================================================================================
--Author:        Microsoft
--Created date : Oct-16-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data for different scenarios like balanceIntegration,reports (main menu). This is a master table and has seeded data. </Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[Scenario]
(
	--Columns
	[ScenarioId]			INT IDENTITY (101, 1)		NOT NULL,
	[Name]					NVARCHAR (50)			NOT NULL,
	[Sequence]				INT						NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),

	--Constraints
	CONSTRAINT [PK_Scenario]			PRIMARY KEY CLUSTERED ([ScenarioId] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the scenario (main menu in nav bar)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Scenario',
    @level2type = N'COLUMN',
    @level2name = N'ScenarioId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for different scenarios like balanceIntegration,reports (main menu). This is a master table and has seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Scenario',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the scenario (balanceTransportes, etc.)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Scenario',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The number signifying sequence in which scenarios (main menu) must be ordered in navbar',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Scenario',
    @level2type = N'COLUMN',
    @level2name = N'Sequence'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Scenario',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Scenario',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'