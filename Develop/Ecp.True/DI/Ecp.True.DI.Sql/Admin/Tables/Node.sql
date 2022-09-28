/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-20-2019
--Updated date : Mar-20-2020
--<Description>: This table holds information about the Node created in the system. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[Node] 
(
	--Columns
    [NodeId]						INT IDENTITY (1, 1) NOT NULL,
    [Name]							NVARCHAR (150)		NOT NULL,
    [Description]					NVARCHAR (1000)		NULL,
    [LogisticCenterId]				NVARCHAR (20)		NULL,
    [IsActive]						BIT					NOT NULL	DEFAULT 1,
    [SendToSAP]						BIT					NOT NULL,
	[ControlLimit]					DECIMAL(18, 2)		NULL,
	[AcceptableBalancePercentage]	DECIMAL(5, 2)		NULL,
	[Order]							INT					NOT NULL,
	[UnitId]						INT					NULL,
	[Capacity]						DECIMAL(18,2)		NULL,
	[NodeOwnershipRuleId]			INT					NULL,
	[RowVersion]					ROWVERSION,
    [IsExportation]                 BIT                 NULL	DEFAULT 0, 
	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	
    --Constraints
    CONSTRAINT [PK_Node]					PRIMARY KEY CLUSTERED ([NodeId] ASC),
    CONSTRAINT [FK_Node_LogisticCenter]		FOREIGN KEY ([LogisticCenterId])		REFERENCES [Admin].[LogisticCenter] ([LogisticCenterId]),
    CONSTRAINT [UC_Node]					UNIQUE NONCLUSTERED ([Name] ASC),
    CONSTRAINT [FK_Node_CategoryElement]	FOREIGN KEY ([UnitId])					REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_Node_NodeOwnershipRule]	FOREIGN KEY	([NodeOwnershipRuleId])		REFERENCES [Admin].[NodeOwnershipRule] ([RuleId])
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds information about the Node created in the system.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the logistic center',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'LogisticCenterId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the node is active or not, 1 means active',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'IsActive'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the node should be sent to SAP, 1 means yes',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'SendToSAP'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The control limit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'ControlLimit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The acceptable balance percentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'AcceptableBalancePercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The order',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'Order'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the unit (category element of unit category, like Bbl)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'UnitId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The capacity',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'Capacity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of Estrategia de propiedad for node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'NodeOwnershipRuleId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version column used for consistency',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'RowVersion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The export node identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'IsExportation'