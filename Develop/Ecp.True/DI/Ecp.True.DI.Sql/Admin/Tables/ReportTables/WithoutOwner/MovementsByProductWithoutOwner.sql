/*==============================================================================================================================
--Author:        Microsoft
--Created date : July-30-2020
--<Description>: This table holds the data for backup movements </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[MovementsByProductWithoutOwner](
	[FilterType]		                VARCHAR(16)				NOT NULL,
	[InitialInventory]	                DECIMAL(29, 2)			NULL,
	[Input]				                DECIMAL(29, 2)			NULL,
	[Output]			                DECIMAL(29, 2)			NULL,
	[IdentifiedLosses]                  DECIMAL(29, 2)			NULL,
	[Interface]                         DECIMAL(29, 2)			NULL,
	[Tolerance]                         DECIMAL(29, 2)			NULL,
	[UnidentifiedLosses]                DECIMAL(29, 2)			NULL,
	[FinalInventory]		            DECIMAL(29, 2)			NULL,
	[Control]				            DECIMAL(29, 2)			NULL,
	[Category]				            NVARCHAR(150)			NULL,
	[Element]				            NVARCHAR(150)			NULL,
	[NodeName]				            NVARCHAR(150)			NULL,
	[CalculationDate]		            DATE					NULL,
	[Product]				            NVARCHAR(150)			NULL,
	[ProductId]                         NVARCHAR(20)			NULL,
	[TicketId]				            INT						NULL,
	[NodeId]				            INT						NULL,
	[SegmentId]				            INT						NULL,
	[SystemId]				            INT						NULL,
	[PercentageValue]		            INT						NULL,

		--Internal Common Columns

	[CreatedBy]					        NVARCHAR (260)			NOT NULL,
	[CreatedDate]					    DATETIME				NOT NULL,
	LastModifiedBy					    NVARCHAR (260)			 NULL,
	LastModifiedDate                    DATETIME                 NULL


) ;

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'type of filter',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'FilterType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'the value of InitialInventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'InitialInventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'the value of Input',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Input'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'the value of output',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Output'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'the value of IdentifiedLosses',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'IdentifiedLosses'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'the value of Interface',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Interface'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Tolerance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Tolerance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'UnidentifiedLosses',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'UnidentifiedLosses'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'FinalInventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'FinalInventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Control',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Control'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Element'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'NodeName',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'CalculationDate',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ProductName',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Product'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ProductId',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'TicketId',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'NodeId',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SegmentId',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SystemId',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'SystemId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PercentageValue',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'PercentageValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'CreatedBy',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'CreatedDate',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'LastModifiedBy',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'LastModifiedDate',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds Backup Movement Details With Owner Data For PowerBi Report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementsByProductWithoutOwner',
    @level2type = NULL,
    @level2name = NULL