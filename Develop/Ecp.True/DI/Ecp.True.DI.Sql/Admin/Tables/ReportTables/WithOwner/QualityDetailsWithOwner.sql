/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-05-2019	
--<Description>: This table holds the data for quality details. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[QualityDetailsWithOwner](
	[Category]			                NVARCHAR(150)           NULL,
	[Element]			                NVARCHAR(150)           NULL,
	[NodeName]                          NVARCHAR(150)           NULL,
	[CalculationDate]                   DATE                    NULL,
	[Product]			                NVARCHAR(150)           NULL,
	[ProductId]			                NVARCHAR(20)            NULL,
	[InventoryId]		                VARCHAR (50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
	[InventoryProductId]                INT                     NULL,
	[InventoryDate]                     DATE                    NULL,
	[TankName]                          NVARCHAR(20)            NULL,
	[BatchId]                           NVARCHAR(150)           NULL,
	[NodeId]                            INT                     NULL,
	[ProductVolume]                     DECIMAL (29, 2)         NULL,
	[MeasurmentUnit]                    NVARCHAR(150)           NULL,
	[EventType]                         NVARCHAR(10)            NULL,
	[SystemName]                        NVARCHAR(150)           NULL,
	[AttributeValue]                    NVARCHAR(150)           NULL,
	[ValueAttributeUnit]                NVARCHAR(150)           NULL,
	[AttributeDescription]              NVARCHAR(150)           NULL,
	[OwnershipTicketId]				            INT                     NULL,
	[GrossStandardQuantity]             DECIMAL (18, 2)         NULL,
	[AttributeId]			            NVARCHAR(150)           NULL,
	
	--Common Columns
	[CreatedBy]					        NVARCHAR (260)			NOT NULL,
	[CreatedDate]					    DATETIME				NOT NULL,
	[LastModifiedBy]					NVARCHAR (260)			NULL,
	[LastModifiedDate]                  DATETIME                NULL

) 
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds InventoryId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'InventoryId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds Ownership TicketId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = 'OwnershipTicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds InventoryProductId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds InventoryDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'InventoryDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds CalculationDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds NodeId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds NodeName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds TankName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'TankName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds BatchId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds ProductName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Product'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds ProductVolume of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'ProductVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds MeasurmentUnit of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'MeasurmentUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds EventType of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds SystemName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds AttributeValue of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'AttributeValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds ValueAttributeUnit of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'ValueAttributeUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds AttributeDescription of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'AttributeDescription'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds ProdutId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds Category of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds Element of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Element'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds GrossStandardQuantity of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds AttributeId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'AttributeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds CreatedBy of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds CreatedDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds LastModifiedBy of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds LastModifiedDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds LastModifiedDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'QualityDetailsWithOwner',
    @level2type = NULL,
    @level2name = NULL
GO