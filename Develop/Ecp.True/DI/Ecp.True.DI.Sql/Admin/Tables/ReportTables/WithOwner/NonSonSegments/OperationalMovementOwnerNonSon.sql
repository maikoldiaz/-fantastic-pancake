-- DROP TABLE [Admin].[OperationalMovementOwnerNonSon];
/* ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Aug-03-2020
-- Update date: 	Oct-09-2020 Added new columns
-- Description:     This Table is to Get TimeOne movement Owner Details Data for Non Sons Segment , Element, Node, StartDate, EndDate.
-- ==============================================================================================================================*/
CREATE TABLE [Admin].[OperationalMovementOwnerNonSon]
(
  --Columns
  [OperationalMovementOwnerNonSonId]             INT                    NOT NULL IDENTITY(1,1),
  [RNo]                                          INT                    NOT NULL,
  [MovementId]                                   VARCHAR (50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
  [BatchId]                                      NVARCHAR (150)         NULL,
  [MovementTransactionId]                        INT                    NULL,
  [CalculationDate]                              DATE                   NOT NULL,
  [TypeMovement]                                 NVARCHAR (150)         NULL,
  [SourceNode]                                   NVARCHAR (150)         NULL,
  [DestinationNode]                              NVARCHAR (150)         NULL,
  [SourceProductId]			                     NVARCHAR (20)	        NULL,
  [SourceProduct]                                NVARCHAR (150)         NULL,
  [DestinationProductId]	                     NVARCHAR (20)	        NULL,
  [DestinationProduct]                           NVARCHAR (150)         NULL,
  [NetQuantity]                                  DECIMAL (18, 2)        NULL,
  [GrossQuantity]                                DECIMAL (18, 2)        NULL,
  [MeasurementUnit]                              VARCHAR (300)          NULL,
  [EventType]                                    NVARCHAR (20)          NULL,
  [Order]                                        NVARCHAR (150)         NULL,  
  [Position]                                     NVARCHAR (150)         NULL,  
  [SystemName]                                   NVARCHAR (150)         NULL,
  [SourceMovementId]                             VARCHAR (50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
  [OwnerName]                                    NVARCHAR (150)         NULL,
  [Ownershipvolume]                              DECIMAL (18, 2)        NULL,
  [ExecutionDate]                                DATETIME               NULL,
  [Rule]                                         NVARCHAR (50)          NULL,
  [Movement]                                     NVARCHAR (50)          NULL,
  [UncertaintyPercentage]                        DECIMAL (5, 2)         NULL,
  [Uncertainty]		                             DECIMAL (18,2)	        NULL,  
  [BackupMovementId]                             NVARCHAR (50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
  [GlobalMovementId]                             NVARCHAR (50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
  [ProductID]                                    NVARCHAR (20)          NOT NULL,
  [ProductName]                                  NVARCHAR (150)         NULL,
  [Ownershippercentage]                          DECIMAL (18, 2)        NULL,
  [ExecutionId]                                  INT                    NOT NULL,
 
  --Internal Common Columns
  [CreatedBy]                                   NVARCHAR (260)         NOT NULL,
  [CreatedDate]                                 DATETIME               NOT NULL
   
  --Constraints
  CONSTRAINT [PK_OperationalMovementOwnerNonSonId]	                PRIMARY KEY CLUSTERED (OperationalMovementOwnerNonSonId ASC),
  CONSTRAINT [FK_OperationalMovementOwnerNonSon_ReportExecution]	FOREIGN KEY ([ExecutionId])			REFERENCES [Admin].[ReportExecution] ([ExecutionId])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The unique identifier of OperationalMovementOwnerNonSon',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'OperationalMovementOwnerNonSonId'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The correlative number of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'RNo'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Type of movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'TypeMovement'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Holds information about applied rule fro ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Rule'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Contains movement information like entradas, salidas, tolerancia,PerdidaIdentificada,Interfases',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Movement'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the Movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'MovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the batch',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The transaction number of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the calculation was done',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'SourceNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source product Id',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'SourceProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination product Id',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Net Quantity',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'NetQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Gross Quantity',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'GrossQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the measurement units',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the event (Insert, Update, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'new column will be populated later',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Order'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'new column will be populated later',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Position'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the system ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Name of the owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'OwnerName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Ownership volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Ownershipvolume'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Execution Date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionDate'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Uncertainty Percentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'UncertaintyPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Product ID',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'ProductID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Product Name',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'ProductName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Ownership Percentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Ownershippercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the uncertainty',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Uncertainty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'For future column',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'BackupMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'For Future column',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'GlobalMovementId'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for movement with owners before cutoff for Non sons segments. This table is being used in before cutoff report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementOwnerNonSon',
    @level2type = NULL,
    @level2name = NULL