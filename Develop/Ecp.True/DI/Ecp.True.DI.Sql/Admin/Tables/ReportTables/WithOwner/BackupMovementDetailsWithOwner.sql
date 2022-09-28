/*==============================================================================================================================
--Author:        Microsoft
--Created date : July-30-2020
--Updated date : Sep-7-2020    Changed data type from INT to VARCHAR
--<Description>: This table holds the data for backup movements </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[BackupMovementDetailsWithOwner] 
(

 [MovementId]					VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS                         
,[BatchId] 						NVARCHAR(25)                   
,[MovementTransactionId]		NVARCHAR (150)	             
,[OperationalDate]				DATETIME	
,[Date]                         DATETIME
,[Operacion]					NVARCHAR (150)	             
,[SourceNode]					NVARCHAR (150)	             
,[DestinationNode]				NVARCHAR (150)	             
,[SourceProduct]				NVARCHAR (150)	             
,[DestinationProduct]			NVARCHAR (150)	             
,[NetStandardVolume]			DECIMAL(18, 2)	             
,[GrossStandardVolume]			DECIMAL(18, 2)	             
,[MeasurementUnit]				NVARCHAR (150)	             
,[EventType]					NVARCHAR (150)	             
,[SystemName]					NVARCHAR (150)               
,[Category]						NVARCHAR (150)	             
,[Element]						NVARCHAR (150)	             
,[NodeName]						NVARCHAR (150)	             
,[CalculationDate]				DATETIME				
,[BackupmovementId]				VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS               
,[GlobalmovementId]				NVARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS               
,[ProductId]					NVARCHAR (20)    
,[OwnershipTicketId]            INT                     NULL

	--Internal Common Columns
,[CreatedBy]					NVARCHAR (260)			NOT NULL
,[CreatedDate]					DATETIME				NOT NULL    
,[LastModifiedBy]				NVARCHAR (260)			NULL
,[LastModifiedDate]				DATETIME				NULL

);
GO

CREATE NONCLUSTERED INDEX [NIX_BackupMovementDetailsWithOwner_OwnershipTicketId] 
ON [Admin].[BackupMovementDetailsWithOwner] (OwnershipTicketId)
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Identifier MovementId',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'MovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Identifier BatchId',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Identifier MovementTransactionId',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Identifier OperationalDate',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'OperationalDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Identifier Date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Date'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Identifier Operacion',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Operacion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SourceNode',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'SourceNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'DestinationNode',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SourceProduct',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'SourceProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'DestinationProduct',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'NetStandardVolume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'GrossStandardVolume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'MeasurementUnit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'EventType',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SystemName',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'Element'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'NodeName',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'CalculationDate',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'BackupmovementId',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'BackupmovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'GlobalmovementId',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'GlobalmovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ProductId',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'CreatedBy',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'CreatedDate',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'LastModifiedBy',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'LastModifiedDate',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds Backup Movement Details With Owner Data For PowerBi Report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The ownership ticket identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipTicketId'