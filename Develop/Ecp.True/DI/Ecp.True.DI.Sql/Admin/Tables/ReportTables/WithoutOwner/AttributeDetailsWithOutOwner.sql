/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jul-07-2020
--<Description>: This table holds the data for attribute details with out owner.
================================================================================================================================*/
CREATE TABLE [Admin].[AttributeDetailsWithoutOwner](
	[MovementId]               VARCHAR (50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
    [MovementTransactionId]    INT                NOT NULL,
    [OperationalDate]          DATE               NULL,     
    [Operacion]                NVARCHAR(150)      NOT NULL,
    [SourceNodeName]           NVARCHAR(150)      NULL,
    [DestinationNodeName]      NVARCHAR(150)      NULL,
    [SourceProductName]        NVARCHAR(150)      NULL,
    [DestinationProductName]   NVARCHAR(150)      NULL,
    [NetStandardVolume]        DECIMAL (18, 2)    NOT NULL,
    [GrossStandardVolume]      DECIMAL(18, 2)     NULL,
    [MeasurementUnit]          NVARCHAR(150)      NOT NULL,
    [EventType]                NVARCHAR(25)       NOT NULL,
    [BatchId]                  NVARCHAR(25)       NULL,
    [SystemName]               NVARCHAR(150)      NOT NULL,
    [AttributeId]              NVARCHAR(150)      NOT NULL,
    [AttributeValue]           NVARCHAR(150)      NOT NULL,
    [ValueAttributeUnit]       NVARCHAR(150)      NOT NULL,
    [AttributeDescription]     NVARCHAR(150)      NULL,
    [SourceProductId]          NVARCHAR(20)       NOT NULL,
    [Category]                 NVARCHAR(150)      NOT NULL,
    [Element]                  NVARCHAR(150)      NOT NULL,
    [NodeName]                 NVARCHAR(309)      NOT NULL,
    [CalculationDate]          DATE               NULL,
    [TicketId]                 INT                NULL,
    
--Internal Common Columns
	[CreatedBy]			       NVARCHAR (260)	  NULL,
	[CreatedDate]			   DATETIME			  NULL    DEFAULT Admin.udf_GetTrueDate(),
	LastModifiedBy			   NVARCHAR (260)	  NULL,
	LastModifiedDate           DATETIME           NULL,
)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds MovementId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'MovementId'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds MovementTransactionId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds OperationalDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'OperationalDate'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds Operacion of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Operacion'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds SourceNodeName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeName'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds DestinationNodeName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeName'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds SourceProductName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductName'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds DestinationProductName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProductName'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds NetStandardVolume of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds GrossStandardVolume of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardVolume'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds MeasurementUnit of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds EventType of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds BatchId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds SystemName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds AttributeId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'AttributeId'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds AttributeValue of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'AttributeValue'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds ValueAttributeUnit of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'ValueAttributeUnit'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds AttributeDescription of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'AttributeDescription'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds SourceProductId of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductId'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds Category of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Category'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds Element of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Element'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds NodeName of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds CalculationDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds CreatedBy of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds CreatedDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds LastModifiedBy of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds LastModifiedDate of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table hold attribute details',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = NULL,
    @level2name = NULL
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The ticket identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'AttributeDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'