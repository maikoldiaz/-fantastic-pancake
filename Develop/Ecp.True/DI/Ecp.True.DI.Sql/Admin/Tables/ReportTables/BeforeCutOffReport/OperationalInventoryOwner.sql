/* ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jul-07-2020
-- Description:     This Table is to Get TimeOne Inventory Owner Details Data for Segment Category, Element, Node, StartDate, EndDate.
-- ==============================================================================================================================*/
CREATE TABLE [Admin].[OperationalInventoryOwner]
(
	--Columns
    [OperationalInventoryOwnerId]   INT                     NOT NULL IDENTITY(1,1),
	[RNo]							INT						NOT NULL,
	[InventoryId]					VARCHAR  (50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
	[InventoryProductId]			INT                     NULL,
	[CalculationDate]				DATETIME				NOT NULL,
	[NodeName]                      NVARCHAR (150)			NULL,
	[TankName]						NVARCHAR (20)			NULL,
    [BatchId]						NVARCHAR (150)          NULL,	
	[Product]		                NVARCHAR (150)    		NULL,
	[ProductId]			            NVARCHAR (20)           NULL,       
	[NetStandardVolume]             DECIMAL  (18,2)   		NULL,
    [MeasurementUnit]			    NVARCHAR (150)			NULL,
    [EventType]                     NVARCHAR (20)           NULL,
	[SystemName]					VARCHAR  (50)			NULL,
 	[GrossStandardQuantity]         DECIMAL  (18,2)         NULL, 
	[Owner]                         NVARCHAR (150)          NOT NULL,
	[OwnershipVolume]               DECIMAL  (18,2)         NOT NULL,
	[OwnershipPercentage]           DECIMAL  (5,2)          NOT NULL,
	 ExecutionId					INT						NOT NULL,
          
     --Internal Common Columns                                                   
    [CreatedBy]                    	NVARCHAR (260)          NOT NULL,
    [CreatedDate]                  	DATETIME                NOT NULL,

	--Constraints
    CONSTRAINT [PK_OperationalInventoryOwnerId]                 PRIMARY KEY CLUSTERED ([OperationalInventoryOwnerId] ASC),
    CONSTRAINT [FK_OperationalInventoryOwner_ReportExecution]	FOREIGN KEY ([ExecutionId])			REFERENCES [Admin].[ReportExecution] ([ExecutionId]),

    --Indexes
    INDEX      [NCI_OperationalInventoryOwner_CreatedDate] NONCLUSTERED  ([CreatedDate])
)
GO

ALTER TABLE [Admin].[OperationalInventoryOwner]  SET (LOCK_ESCALATION = DISABLE)
Go
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The correlative number of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'RNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'InventoryId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the calculation was done',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the tank',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'TankName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the batch',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'Product'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Name of the owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'Owner'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipVolume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipPercentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the net standard volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the measurement units',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the system ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the inventory product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the event (Insert, Update, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of GrossStandardQuantity',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for inventories with owners before cutoff. This table is being used in before cutoff report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalInventoryOwner',
    @level2type = NULL,
    @level2name = NULL
