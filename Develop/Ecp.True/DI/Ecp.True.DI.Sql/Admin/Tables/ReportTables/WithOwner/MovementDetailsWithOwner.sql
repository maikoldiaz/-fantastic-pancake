/*-- =================================================================================================
-- Author:		Microsoft
-- Created date: Jul-29-2020
-- <Description>:	This table is to Fetch MovementDetailsWithOwner Data For PowerBi Report From
				Tables(Movement, Product, Ticket, Node, NodeTag, CategoryElement, Category, VariableType)</Description>
-- ===================================================================================================*/

CREATE Table [Admin].[MovementDetailsWithOwner] (
[MovementId]						VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
[MovementTransactionId]			    INT					    NULL,
[OperationalDate]					DATE				    NULL,
[Operacion]						    NVARCHAR (300)			NULL,
[SourceNode]						NVARCHAR (300)			NULL,
[DestinationNode]					NVARCHAR (300)			NULL,
[SourceProduct]					    NVARCHAR (300)			NULL,
[DestinationProduct]				NVARCHAR (300)			NULL,
[NetStandardVolume]				    DECIMAL  (18,2)			NULL,
[GrossStandardVolume]				DECIMAL	 (18,2)			NULL,
[MeasurementUnit]					NVARCHAR (300)			NULL,
[EventType]						    NVARCHAR (50)           NULL,
[SystemName]						NVARCHAR  (300)	        NULL,
[SourceMovementId]				    VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
[Order]							    INT						NULL,
[Position]							INT						NULL,
[OwnerName]						    NVARCHAR (150)          NULL,
[OwnershipVolume]					DECIMAL  (18,2)          NULL,
[OwnershipProcessDate]				  DATE					NULL,
[Rule]								NVARCHAR( 50)			NULL,
[Movement]							NVARCHAR (150)			NULL,
[% Standard Uncertainty]			DECIMAL	 (5, 2)			NULL,
[Uncertainty]						DECIMAL	 (18,2)			NULL,
[BackupMovementId]					varchar	 (50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
[GlobalMovementId]					nvarchar (300) COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
[SourceProductId]					NVARCHAR (40)           NULL,
[OwnershipPercentage]				DECIMAL  (5,2)          NULL,
[Category]							NVARCHAR(150)			NULL,
[Element]							NVARCHAR(150)			NULL,
[NodeName]							 NVARCHAR(150)			NULL,
[BatchId]							NVARCHAR (150)          NULL,
[CalculationDate]                   DATE					NULL,
[OwnershipTicketId]                 INT                     NULL,
[ScenarioName]							NVARCHAR (150)          NULL,
--Internal Common Columns

[CreatedBy]					        NVARCHAR (260)			NOT NULL,
[CreatedDate]					    DATETIME				NOT NULL,
LastModifiedBy					    NVARCHAR (260)			 NULL,
LastModifiedDate                    DATETIME                 NULL
)
GO
CREATE NONCLUSTERED INDEX [NIX_MovementDetailsWithOwner_OwnershipTicketId] 
ON [Admin].[MovementDetailsWithOwner] (OwnershipTicketId)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'MovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the MovementTransactionId',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date of the operation',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'OperationalDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the operacion',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Operacion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the SourceNode ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'SourceNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the DestinationNode ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the SourceProduct',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'SourceProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the DestinationProduct',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the first owner (category element of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of GrossStandardVolume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of MeasurementUnit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type  of the event(like insert, update)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'SourceMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the order',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Order'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Position',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Position'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the Owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnerName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipVolume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Date of OwnershipProcess',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipProcessDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the Rule',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Rule'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of Movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Movement'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of percentage uncertanity',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'% Standard Uncertainty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Uncertanity',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Uncertainty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of BackupMovement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'BackupMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of GlobalMovement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'GlobalMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of SourceProduct',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipPercentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of Category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the Element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Element'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the Node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of Batch',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date when the record is Calculated',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for purchases and sales MovementDetailsWithOwners registered in the system.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = NULL,
    @level2name = NULL

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The ownership ticket identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipTicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the Scenario',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = 'ScenarioName'