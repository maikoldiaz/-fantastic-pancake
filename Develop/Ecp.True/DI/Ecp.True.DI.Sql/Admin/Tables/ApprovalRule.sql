/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jan-04-2020
--Updated date : Apr-06-2020
--<Description>: This table holds the rules for ticket node approval. This is a master table and has seeded data. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[ApprovalRule]
(
	--Columns
	[ApprovalRuleId]					INT IDENTITY (101, 1)		NOT NULL,
	[Rule]								VARCHAR(100)				NOT NULL,

	 --Internal Common Columns
	[CreatedBy]							NVARCHAR (260)				NOT NULL,
	[CreatedDate]						DATETIME					NOT NULL DEFAULT (Admin.udf_GetTrueDate())

	--Constraints
    CONSTRAINT [PK_ApprovalRule]		PRIMARY KEY CLUSTERED ([ApprovalRuleId] ASC)
)



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the approval rule',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ApprovalRule',
    @level2type = N'COLUMN',
    @level2name = N'ApprovalRuleId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The expression of the rule (like PI/E < 0.2)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ApprovalRule',
    @level2type = N'COLUMN',
    @level2name = N'Rule'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ApprovalRule',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ApprovalRule',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the rules for ticket node approval. This is a master table and has seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ApprovalRule',
    @level2type = NULL,
    @level2name = NULL