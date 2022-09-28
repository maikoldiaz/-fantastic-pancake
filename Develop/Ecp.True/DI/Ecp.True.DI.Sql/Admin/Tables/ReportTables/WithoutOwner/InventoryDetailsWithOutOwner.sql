/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jul-07-2020
--<Description>: This table holds the data for inventory details with out owner.
================================================================================================================================*/
CREATE TABLE [Admin].[InventoryDetailsWithoutOwner](
	[InventoryId]              VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
	[InventoryProductId]       INT                NOT NULL,
	[CalculationDate]          DATE               NULL,
	[NodeId]                   INT                NOT NULL,
	[NodeName]                 NVARCHAR(150)      NOT NULL,
	[TankName]                 NVARCHAR(20)       NULL,
	[BatchId]                  NVARCHAR(150)      NULL,
	[ProductName]              NVARCHAR(150)      NOT NULL,
	[ProductVolume]            DECIMAL (29, 2)    NULL,
	[MeasurmentUnit]           NVARCHAR(150)      NOT NULL,
	[EventType]                NVARCHAR(10)       NOT NULL,
	[SystemName]               NVARCHAR(150)      NOT NULL,
	[UncertainityPercentage]   DECIMAL(29, 2)     NULL,
	[Incertidumbre]            DECIMAL(29, 2)     NULL,
	[TicketId]                 INT                NULL,
	[ProdutId]                 NVARCHAR(20)       NOT NULL,
	[Category]                 NVARCHAR(150)      NOT NULL,
	[Element]                  NVARCHAR(150)      NOT NULL,
	[GrossStandardQuantity]    DECIMAL (18, 2)    NULL,

	--Internal Common Columns
	[CreatedBy]			       NVARCHAR (260)	  NULL,
	[CreatedDate]			   DATETIME			  NULL    DEFAULT Admin.udf_GetTrueDate(),
	LastModifiedBy			   NVARCHAR (260)	  NULL,
	LastModifiedDate           DATETIME           NULL,
)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds InventoryId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'InventoryId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds InventoryProductId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds CalculationDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds NodeId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds NodeName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds TankName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'TankName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds BatchId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds ProductName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'ProductName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds ProductVolume of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'ProductVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds MeasurmentUnit of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'MeasurmentUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds EventType of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds SystemName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds UncertainityPercentage of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'UncertainityPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds Incertidumbre of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Incertidumbre'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds TicketId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds ProdutId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'ProdutId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds Category of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds Element of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Element'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds GrossStandardQuantity of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds CreatedBy of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds CreatedDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds LastModifiedBy of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds LastModifiedDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table hold attribute details',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryDetailsWithoutOwner',
    @level2type = NULL,
    @level2name = NULL
GO