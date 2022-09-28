/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-20-2020
--Updated date : Aug-26-2020  -- Added 2 new columns Start and End Date
--<Description>: This table holds Official consolidated delta inventories details. </Description>
=================================================================================================================================*/
CREATE TABLE [Report].[OfficialDeltaInventory]
(
  --Columns
  [StartDate]                    DATE                   NOT NULL,
  [EndDate]                      DATE                   NOT NULL,
  [SegmentId]                    INT                    NULL,
  [NodeId]                       INT                    NULL,
  [InventoryProductId]           INT                    NOT NULL,
  [Date]                         DATE                   NOT NULL,
  [NodeName]                     NVARCHAR (150)			NULL,
  [Product]		                 NVARCHAR (150)    		NULL,
  [NetQuantity]                  DECIMAL  (18, 2)   	NULL,
  [GrossQuantity]                DECIMAL  (18, 2)       NULL, 
  [MeasurementUnit]		         NVARCHAR (150)			NULL,
  [Owner]                        NVARCHAR (150)         NULL,
  [OwnershipVolume]              DECIMAL  (18,2)        NULL,
  [OwnershipPercentage]          DECIMAL  (5,2)         NULL,
  [Scenario]                     NVARCHAR (50)          NULL,
  [Origin]                       NVARCHAR (150)         NULL,
  [ExecutionDate]                DATETIME               NULL,
  --Internal Common Columns
  [CreatedBy]                    NVARCHAR (260)         NOT NULL,
  [CreatedDate]                  DATETIME               NOT NULL,
  [LastModifiedBy]               NVARCHAR (260)         NOT NULL,
  [LastModifiedDate]             DATETIME               NOT NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Start date of the Inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'End date of the Inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'EndDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The id of segment',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Iif of node',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The InventoryProduct number of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'Date'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'Product'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Net Quantity',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'NetQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Gross Quantity',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'GrossQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the measurement units',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Name of the owner',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'Owner'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipVolume',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipPercentage',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The scenario of movement',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'Scenario'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Origin of movement',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'Origin'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the Sp is executed',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'who updated the record for the last time (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'when the record updated for the last time (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds Official consolidated delta inventories details.',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaInventory',
    @level2type = NULL,
    @level2name = NULL