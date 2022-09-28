/*==============================================================================================================================
--Author:        Microsoft
--Created date : Mar-05-2020
--Updated date : Mar-20-2020
--<Description>: This table holds the previous ownership rule refresh logs.</Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[OwnershipRuleRefreshHistory]
(
	--Columns
    [OwnershipRuleRefreshHistoryId]		INT IDENTITY (1, 1)		NOT NULL,
    [Status]							BIT					    NOT NULL,
	[RequestedBy]						NVARCHAR(60)		 	NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)	NOT NULL,
	[CreatedDate]					DATETIME		NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)  NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_OwnershipRuleRefreshHistory] PRIMARY KEY CLUSTERED ([OwnershipRuleRefreshHistoryId] ASC)
)



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ownership rule refresh history',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipRuleRefreshHistory',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipRuleRefreshHistoryId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the rule refresh was success, 1 means success',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipRuleRefreshHistory',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the user who requested it',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipRuleRefreshHistory',
    @level2type = N'COLUMN',
    @level2name = N'RequestedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipRuleRefreshHistory',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipRuleRefreshHistory',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipRuleRefreshHistory',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipRuleRefreshHistory',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the previous ownership rule refresh logs.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipRuleRefreshHistory',
    @level2type = NULL,
    @level2name = NULL