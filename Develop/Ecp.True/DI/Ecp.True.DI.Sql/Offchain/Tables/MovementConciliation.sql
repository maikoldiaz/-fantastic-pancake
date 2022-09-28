CREATE TABLE [OffChain].[MovementConciliation]
(
	[MovementConciliationId] INT Identity(1,1) NOT NULL PRIMARY KEY, 
    [MovementTransactionId ] INT NULL, 
    [MovementTypeId] INT NULL, 
    [SourceNodeId] INT NULL, 
    [DestinationNodeId ] INT NULL, 
    [SourceProductId ] NVARCHAR(50) NULL, 
    [DestinationProductId ] NVARCHAR(50) NULL, 
    [OwnershipVolume ] DECIMAL(18, 2) NULL, 
    [OwnerId] INT NULL, 
    [MeasurementUnit] INT NULL, 
    [SegmentId] INT NULL, 
    [OwnershipPercentage] DECIMAL(18, 2) NULL, 
    [NetStandardVolume] DECIMAL(18, 2) NULL, 
    [Description] NVARCHAR(260) NULL, 
    [Sign] NVARCHAR(50) NULL, 
    [DeltaConciliated] DECIMAL(18, 2) NULL, 
    [OperationalDate] DATETIME NULL, 
    [UncertaintyPercentage ] DECIMAL(18, 2) NULL, 
    [CollectionType] INT NOT NULL, 
    [OwnershipTicketConciliationId] INT NOT NULL, 
    [CreatedBy] NVARCHAR(260) NOT NULL, 
    [CreatedDate] DATETIME NOT NULL DEFAULT Admin.udf_GetTrueDate(), 
    [LastModifiedBy] NVARCHAR(260) NULL, 
    [LastModifiedDate] DATETIME NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the conciliation movement',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'MovementConciliationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement transaction',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId '
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement type',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'MovementTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement source node',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement destination node',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeId '
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement source product',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductId '
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement destination product',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProductId '
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the ownership',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume '
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the owner',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'OwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the measurement unit ',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the segment',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the ownership',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the net standard volume',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The delta conciliated',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'DeltaConciliated'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The operational date of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'OperationalDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the uncertainty percentage',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'UncertaintyPercentage '
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Identifier of original movement conciliation of other segment',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipTicketConciliationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The conciliation movement collection type',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'CollectionType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The sign',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'Sign'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'OffChain',
    @level1type = N'TABLE',
    @level1name = N'MovementConciliation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'