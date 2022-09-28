/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-20-2020
--Updated date : Mar-20-2020
--<Description>: This table holds the data for LogisticCenter.</Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[LogisticCenter]
(
	--Columns
    [LogisticCenterId]		NVARCHAR (20)		NOT NULL,
    [Name]					NVARCHAR (150)		NOT NULL,
    [IsActive]				BIT					NOT NULL    DEFAULT 1,
	
	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_LogisticsCenter] PRIMARY KEY CLUSTERED ([LogisticCenterId] ASC),
    CONSTRAINT [UC_LogisticsCenter] UNIQUE NONCLUSTERED ([Name] ASC)
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for LogisticCenter.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'LogisticCenter',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'LogisticCenter',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'LogisticCenter',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'LogisticCenter',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'LogisticCenter',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the logistic center',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'LogisticCenter',
    @level2type = N'COLUMN',
    @level2name = N'LogisticCenterId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the logistic center',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'LogisticCenter',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the logistic center is active or not, 1 means active',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'LogisticCenter',
    @level2type = N'COLUMN',
    @level2name = N'IsActive'