/*==============================================================================================================================
--Author:        Microsoft
--Created date : Feb-03-2020
--Updated date : Mar-20-2020
--Updated date : Apr-06-2020 -- Added BatchId,SystemName as per PBI 28962
--Updated date : Apr-22-2020 -- Added EventType Column as per PBI 25056
--Updated date : May-05-2020 -- Added InventoryProductId Column
--Updated date : Jun-15-2020 -- Added AttributeId,GrossStandardQuality Column as per PBI 31874
--<Description>: This table holds the data for quality attributes of inventories before cutoff. This table is being used in before cutoff report. </Description>
==================================================================================================================================*/
CREATE TABLE [Admin].[OperationalInventoryQuality]
(
	--Columns
    [OperationalInventoryQualityId] INT                     NOT NULL IDENTITY(1,1),
	[RNo]							INT						NOT NULL,	
	[InventoryId]					VARCHAR  (50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
	[InventoryProductId]			INT                     NULL,
	[CalculationDate]				DATETIME				NOT NULL,
	[NodeName]                      NVARCHAR (150)			NULL,
	[TankName]						NVARCHAR (20)			NULL,
    [BatchId]						NVARCHAR(150)           NULL,	
	[Product]		                NVARCHAR (150)    		NULL,
    [ProductId]			            NVARCHAR (20)           NULL,
	[NetStandardVolume]             DECIMAL  (18,2)   		NULL,
    [MeasurementUnit]			    NVARCHAR (150)			NULL,
    [EventType]                     NVARCHAR (20)           NULL,
	[SystemName]					VARCHAR  (50)			NULL,
    [PercentStandardUnCertainty]    DECIMAL  (18,2)         NULL,	
    [AttributeValue]                NVARCHAR (150)          NULL,                                   
    [ValueAttributeUnit]            NVARCHAR (150)          NULL,                                       
    [AttributeDescription]          NVARCHAR (150)          NULL,
	[ExecutionId]					INT						NOT NULL,
    [AttributeId]                   NVARCHAR(150)           NULL,
    [GrossStandardQuantity]         DECIMAL(18,2)           NULL,

    --Internal Common Columns                                                   
    [CreatedBy]                    	NVARCHAR (260)          NOT NULL,
    [CreatedDate]                  	DATETIME              	NOT NULL,

	--Constraints
    CONSTRAINT [PK_OperationalInventoryQualityId]               PRIMARY KEY CLUSTERED ([OperationalInventoryQualityId] ASC),
    CONSTRAINT [FK_OperationalInventoryQuality_ReportExecution]	FOREIGN KEY ([ExecutionId])			REFERENCES [Admin].[ReportExecution] ([ExecutionId])
)
GO

ALTER TABLE [Admin].[OperationalInventoryQuality] SET (LOCK_ESCALATION = DISABLE)
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The correlative number of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'RNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'InventoryId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the calculation was done',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the tank',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'TankName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the batch',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'Product'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the net standard volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the measurement unit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage standard of the uncertainty',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'PercentStandardUnCertainty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the attribute',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'AttributeValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the attribute unit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'ValueAttributeUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description of the attribute',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'AttributeDescription'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the inventory product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the event (Insert, Update, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of an Attribute',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'AttributeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of GrossStandardQuantity',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for quality attributes of inventories before cutoff. This table is being used in before cutoff report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryQuality',
    @level2type = NULL,
    @level2name = NULL