/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-02-2020

--<Description>: This table holds the data for quality attributes of inventories before cutoff for Non sons segments. This table is being used in before cutoff report. </Description>
==================================================================================================================================*/
CREATE TABLE [Admin].[OperationalInventoryQualityNonSon]
(
	--Columns
    [OperationalInventoryQualityNonSonId]           INT                     NOT NULL IDENTITY(1,1),
	[RNo]							                INT						NOT NULL,	
	[InventoryId]					                VARCHAR  (50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
	[InventoryProductId]			                INT                     NULL,
	[CalculationDate]				                DATETIME				NOT NULL,
	[NodeName]                                      NVARCHAR (150)			NULL,
	[TankName]						                NVARCHAR (20)			NULL,
    [BatchId]						                NVARCHAR(150)           NULL,	
	[Product]		                                NVARCHAR (150)    		NULL,
    [ProductId]			                            NVARCHAR (20)           NULL,
	[NetStandardVolume]                             DECIMAL  (18,2)   		NULL,
	[GrossStandardQuantity]                         DECIMAL  (18,2)         NULL,
    [MeasurementUnit]			                    NVARCHAR (150)			NULL,
    [Order]                                         NVARCHAR (50)           NULL, 
	[SystemName]					                VARCHAR  (50)			NULL,     -- origen
    [AttributeId]                                   NVARCHAR(150)           NULL,
	[AttributeValue]                                NVARCHAR (150)          NULL,                                   
    [ValueAttributeUnit]                            NVARCHAR (50)           NULL,                                       
    [AttributeDescription]                          NVARCHAR (150)          NULL, 
    [ExecutionId]                                   INT                     NOT NULL,

    --Internal Common Columns                                                                   
    [CreatedBy]                    	                NVARCHAR (260)          NOT NULL,
    [CreatedDate]                  	                DATETIME              	NOT NULL

      --Constraints
  CONSTRAINT [PK_OperationalInventoryQualityNonSonId]	            PRIMARY KEY CLUSTERED (OperationalInventoryQualityNonSonId ASC),
  CONSTRAINT [FK_OperationalInventoryQualityNonSon_ReportExecution]	FOREIGN KEY ([ExecutionId])			REFERENCES [Admin].[ReportExecution] ([ExecutionId])
)
GO

ALTER TABLE [Admin].[OperationalInventoryQualityNonSon]		  SET (LOCK_ESCALATION = DISABLE)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The unique identifier of OperationalInventoryQualityNonSon',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'OperationalInventoryQualityNonSonId'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The correlative number of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'RNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'InventoryId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the calculation was done',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the tank',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'TankName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the batch',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Product'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the net standard volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the measurement unit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of order',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Order'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the attribute',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'AttributeValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the attribute unit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'ValueAttributeUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description of the attribute',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'AttributeDescription'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of source system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the inventory product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of an Attribute',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'AttributeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of GrossStandardQuantity',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for quality attributes of inventories before cutoff for Non sons segments. This table is being used in before cutoff report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQualityNonSon',
    @level2type = NULL,
    @level2name = NULL