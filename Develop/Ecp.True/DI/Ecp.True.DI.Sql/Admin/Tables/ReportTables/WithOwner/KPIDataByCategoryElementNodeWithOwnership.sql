/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jul-31-2020
--Updated date : Aug-06-2020  Removed Default Constraint
--<Description>: This table holds the KPI data by category element node with owner values. 
================================================================================================================================*/
CREATE TABLE [Admin].[KPIDataByCategoryElementNodeWithOwnership] 
  ( 
    FilterType                   VARCHAR (50),
    OrderToDisplay				 INT,
    Category					 NVARCHAR (150),
    Element						 NVARCHAR (150),
    NodeName					 NVARCHAR (150),
    NodeId						 INT,
    CalculationDate				 DATE,
    Product						 NVARCHAR (150),
    Indicator					 NVARCHAR (50),
    CurrentValue				 DECIMAL (18,2),
    [OwnershipTicketId]					 INT,

	--Internal Common Columns
	[CreatedBy]					 NVARCHAR (260)			NOT NULL,
	[CreatedDate]				 DATETIME				NOT NULL,
	[LastModifiedBy]			 NVARCHAR (260)			NULL,
	[LastModifiedDate]			 DATETIME				NULL,
   )
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Filter type to find a perticular category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'FilterType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Order of the KPI Indicator',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'OrderToDisplay'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Name of a Category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Name of a Element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'Element'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Name of a Node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Id of a specific Node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Date when the value is calculated',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Name of Product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'Product'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Represents type of KPI',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'Indicator'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Current Value of the selected period',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'CurrentValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Ownershipticketid of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = 'OwnershipTicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the KPI data by category element node with owner values.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIDataByCategoryElementNodeWithOwnership',
    @level2type = NULL,
    @level2name = NULL