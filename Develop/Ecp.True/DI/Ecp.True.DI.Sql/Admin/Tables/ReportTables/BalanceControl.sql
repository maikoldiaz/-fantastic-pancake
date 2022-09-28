/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-05-2020
--<Description>:  This table is to Fetch Data [Admin].[BalanceControl] For PowerBi Report From Tables (Unbalance, Product, Node, NodeTag,  CategoryElement,Category)</Description>
DROP VIEW ADMIN.BALANCECONTROL
SELECT * from Admin.BalanceControl
===============================================================================================================================*/
CREATE TABLE [Admin].[BalanceControl](
	[Category]							NVARCHAR(150)				NOT NULL,
	[Element]							NVARCHAR(150)				NOT NULL,
	[NodeName]							NVARCHAR(150)				NOT NULL,
	[CalculationDate]					DATE						NULL,
	[Product]							NVARCHAR(150)				NOT NULL,
	[ProductId]							NVARCHAR(20)				NOT NULL,
	[TicketId]							INT							NOT NULL,
	[NodeId]							INT							NOT NULL,
	[Input]								DECIMAL(18, 2)				NULL,
	[Unbalance]							DECIMAL(18, 2)				NULL,
	[StandardUncertainty]				DECIMAL(18, 2)				NULL,
	[AverageUncertainty]				DECIMAL(18, 2)				NULL,
	[AverageUncertaintyUnbalance]		DECIMAL(18, 2)				NULL,
	[Warning]							DECIMAL(18, 2)				NULL,
	[Action]							DECIMAL(18, 2)				NULL,
	[ControlTolerance]					DECIMAL(18, 2)				NULL,
	[Warning(-)]						DECIMAL(20, 2)				NULL,
	[Action(-)]							DECIMAL(20, 2)				NULL,
	[ControlTolerance(-)]				DECIMAL(20, 2)				NULL,

	--Common Columns
	[CreatedBy]					        NVARCHAR (260)				NOT NULL,
	[CreatedDate]					    DATETIME					NOT NULL,
	[LastModifiedBy]					NVARCHAR (260)				NULL,
	[LastModifiedDate]                  DATETIME					NULL
); 
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table contains BalanceControl data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the action type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The action name (Insertar, Eliminar, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'Element'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'Product'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'Input'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'Unbalance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'StandardUncertainty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'AverageUncertainty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'AverageUncertaintyUnbalance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'Warning'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'Action'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'ControlTolerance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'Warning(-)'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'Action(-)'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'ControlTolerance(-)'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds CreatedBy of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds CreatedDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds LastModifiedBy of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds LastModifiedDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BalanceControl',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
