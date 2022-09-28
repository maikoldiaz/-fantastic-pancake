/*==============================================================================================================================
--Author:        Microsoft
--Created date : Nov-06-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data types of State used in ownership nodes, tickets, fileregistrations. This is a master table and has seeded data. </Description>
===============================================================================================================================*/
CREATE TABLE [Offchain].[NodeStateType]
(
    --Columns
    [NodeStateTypeId] INT IDENTITY (10, 1) NOT NULL,
    [Name] VARCHAR(50) NOT NULL,

    --Internal Common Columns
    [CreatedBy] NVARCHAR (260) NOT NULL,
    [CreatedDate] DATETIME NOT NULL DEFAULT (Admin.udf_GetTrueDate())

        --Constraints
    CONSTRAINT [PK_NodeStateType]	PRIMARY KEY CLUSTERED ([NodeStateTypeId] ASC),
    CONSTRAINT [UC_NodeStateType] UNIQUE NONCLUSTERED ([Name] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node State type',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'NodeStateType',
    @level2type = N'COLUMN',
    @level2name = N'NodeStateTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the State (CreatedNode, UpdatedNode etc.)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'NodeStateType',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'NodeStateType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'NodeStateType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' This table holds the data types of State used in offchain node. This is a master table and has seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'NodeStateType',
    @level2type = NULL,
    @level2name = NULL