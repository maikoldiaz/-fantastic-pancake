CREATE TABLE [Admin].[MovementDetailsWithOwnerOtherSegment]
(
[MovementId]						VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
[MovementTransactionId]			    INT					    NULL, 
[OwnershipTicketId]                 INT                     NULL,
[Operacion]						    NVARCHAR (300)			NULL,
[SourceNode]						NVARCHAR (300)			NULL,
[DestinationNode]					NVARCHAR (300)			NULL,
[SourceProduct]					    NVARCHAR (300)			NULL,
[DestinationProduct]				NVARCHAR (300)			NULL,
[NetStandardVolume]				    DECIMAL  (18,2)			NULL,
[MeasurementUnit]					NVARCHAR (300)			NULL,
[OwnershipPercentage]				DECIMAL  (5,2)          NULL,
[OwnerName]						    NVARCHAR (150)          NULL,
[OwnershipVolume]					DECIMAL  (18,2)         NULL,
[ScenarioName]						NVARCHAR (150)          NULL,
[Category]							NVARCHAR(150)			NULL,
[Element]							NVARCHAR(150)			NULL,
[NodeName]							NVARCHAR(150)			NULL,
[CalculationDate]                   DATE					NULL,
[OtherSegment]						NVARCHAR(150)			NULL,
[IsReconciled]                      INT                     NULL,
--Internal Common Columns

[CreatedBy]					        NVARCHAR (260)			NOT NULL,
[CreatedDate]					    DATETIME				NOT NULL,
LastModifiedBy					    NVARCHAR (260)			 NULL,
LastModifiedDate                    DATETIME                 NULL
)
GO
CREATE NONCLUSTERED INDEX [NIX_MovementDetailsWithOwnerOtherSegment_OwnershipTicketId] 
ON [Admin].[MovementDetailsWithOwnerOtherSegment] (OwnershipTicketId)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'MovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the MovementTransactionId',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ownership ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipTicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the operacion',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'Operacion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the SourceNode',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'SourceNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the DestinationNode ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the SourceProduct',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'SourceProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the DestinationProduct',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the first owner (category element of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of MeasurementUnit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the Owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'OwnerName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipPercentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipVolume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the Scenario',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'ScenarioName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of Category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the Element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'Element'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the Node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date when the record is Calculated',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name other Segment of the Element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = N'OtherSegment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag for identifying if conciliation of other segment is pending or not.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwnerOtherSegment',
    @level2type = N'COLUMN',
    @level2name = 'IsReconciled'