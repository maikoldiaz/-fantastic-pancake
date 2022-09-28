/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jul-07-2020
--<Description>: This table holds the data for monthly movement details with owner.
================================================================================================================================*/
CREATE TABLE [Admin].[MovementDetailsWithoutOwner]
(
-- Columns
     [MovementId]                           VARCHAR   (50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL
    ,[MovementTransactionId]                INT				         NULL
    ,[OperationalDate]                      DATE			         NULL
    ,[Operacion]                            NVARCHAR  (300)	         NULL
    ,[SourceNode]                           NVARCHAR  (300)	         NULL
    ,[DestinationNode]                      NVARCHAR  (300)	         NULL
    ,[SourceProduct]                        NVARCHAR  (300)	         NULL
    ,[DestinationProduct]                   NVARCHAR  (300)	         NULL
    ,[NetStandardVolume]                    DECIMAL   (18,2)         NULL
    ,[GrossStandardVolume]                  DECIMAL   (18,2)         NULL
    ,[MeasurementUnit]                      NVARCHAR  (300)	         NULL
    ,[EventType]                            NVARCHAR  (50)	         NULL
    ,[BatchId]                              NVARCHAR  (50)	         NULL
    ,[SystemName]                           NVARCHAR  (300)	         NULL
    ,[Movement]                             VARCHAR   (25)	         NULL
    ,[PercentStandardUnCertainty]           DECIMAL   (18,2)         NULL
    ,[Uncertainty]                          DECIMAL   (18,2)         NULL
    ,[BackupMovementId]                     VARCHAR   (50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL
    ,[GlobalMovementId]                     NVARCHAR  (300) COLLATE SQL_Latin1_General_CP1_CS_AS NULL
    ,[SourceProductId]                      NVARCHAR  (40)	         NULL
    ,[Category]                             NVARCHAR  (300)	         NULL
    ,[Element]                              NVARCHAR  (300)	         NULL
    ,[NodeName]                             NVARCHAR  (300)	         NULL
    ,[CalculationDate]                      DATE 			         NULL
    ,[TicketId]                             INT 			         NULL
    
--Internal Common Columns
	,[CreatedBy]					        NVARCHAR (260)			 NULL
	,[CreatedDate]					        DATETIME				 NULL    DEFAULT Admin.udf_GetTrueDate()
	,LastModifiedBy					        NVARCHAR (260)			 NULL
	,LastModifiedDate                       DATETIME                 NULL
)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds MovementId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'MovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds MovementTransactionId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds OperationalDATE of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'OperationalDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds Operacion of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Operacion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds SourceNode of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'SourceNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds DestinationNode of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds SourceProduct of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'SourceProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds DestinationProduct of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds NetStandardVolume of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds GrossStandardVolume of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds MeasurementUnit of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds EventType of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds BatchId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds SystemName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds Movement of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Movement'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds PercentStandardUnCertainty of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'PercentStandardUnCertainty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds Uncertainty of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Uncertainty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds BackupMovementId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'BackupMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds GlobalMovementId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'GlobalMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds SourceProductId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds Category of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds Element of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Element'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds NodeName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds CalculationDATE of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds CreatedBy of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds CreatedDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds LastModifiedBy of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds LastModifiedDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data movement details with owner.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The ticket identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'