/*==============================================================================================================================
--Author:        Microsoft
--Created date : Dec-11-2019	
--Updated date : Mar-20-2020
--<Description>: This table holds the data for Algorithms based on Analytical Model. This is a master table and contains seeded data. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[Algorithm] 
(
	--Columns
    [AlgorithmId]					INT IDENTITY (1, 1)		NOT NULL,
    [ModelName]						NVARCHAR (150)		    NOT NULL,
    [PeriodsToForecast]				INT						NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)			NOT NULL,
	[CreatedDate]					DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)			NULL,
	[LastModifiedDate]				DATETIME				NULL,

	--Constraints
    CONSTRAINT [PK_Algorithm] PRIMARY KEY CLUSTERED ([AlgorithmId] ASC)
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the Algorithm',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Algorithm',
    @level2type = N'COLUMN',
    @level2name = N'AlgorithmId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the analytical model',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Algorithm',
    @level2type = N'COLUMN',
    @level2name = N'ModelName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the periods to forecast',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Algorithm',
    @level2type = N'COLUMN',
    @level2name = N'PeriodsToForecast'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Algorithm',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Algorithm',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Algorithm',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Algorithm',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for Algorithms based on Analytical Model. This is a master table and contains seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Algorithm',
    @level2type = NULL,
    @level2name = NULL