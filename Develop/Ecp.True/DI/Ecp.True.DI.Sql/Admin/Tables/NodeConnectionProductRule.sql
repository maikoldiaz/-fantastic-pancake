/*==============================================================================================================================
--Author:        Microsoft
--Created date : Mar-05-2020
--Updated date : Mar-20-2020
--<Description>: This table contains ownership rules for node connections. </Description>
===============================================================================================================================*/
CREATE TABLE [Admin].[NodeConnectionProductRule] 
(
	--Columns
    [RuleId]						INT						NOT NULL,
    [RuleName]						NVARCHAR(100)			NOT NULL,
	[IsActive]						BIT						NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)			NOT NULL,
	[CreatedDate]					DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)			NULL,
	[LastModifiedDate]				DATETIME				NULL,

	--Constraints
    CONSTRAINT [PK_NodeConnectionProductRule] PRIMARY KEY CLUSTERED ([RuleId] ASC)
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the rule',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductRule',
    @level2type = N'COLUMN',
    @level2name = N'RuleId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the rule',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductRule',
    @level2type = N'COLUMN',
    @level2name = N'RuleName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the rule is active or not, 1 means active',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductRule',
    @level2type = N'COLUMN',
    @level2name = N'IsActive'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductRule',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductRule',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductRule',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductRule',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table contains ownership rules for node connections.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductRule',
    @level2type = NULL,
    @level2name = NULL