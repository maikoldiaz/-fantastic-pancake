 /*==============================================================================================================================
--Author:        Microsoft
--Created date : Jul-07-2020
--Updated date : Sep-17-2020  -- Adding a new column "Date" to show in report
--<Description>: This table holds the data for backup movement details with out owner.
================================================================================================================================*/
CREATE TABLE [Admin].[BackupMovementDetailsWithoutOwner] 
             ( 
			    -- Columns
                [MovementId]            VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL, 
                [BatchId]               NVARCHAR  (25)        NULL, 
                [MovementTransactionId] INT                   NOT NULL, 
                [OperationalDate]       DATE                  NULL, 
				[Date]                  DATE                  NULL,
                [Operacion]             NVARCHAR  (150)       NOT NULL, 
                [SourceNode]            NVARCHAR  (150)       NULL, 
                [DestinationNode]       NVARCHAR  (150)       NULL, 
                [SourceProduct]         NVARCHAR  (150)       NULL, 
                [DestinationProduct]    NVARCHAR  (150)       NULL, 
                [NetStandardVolume]     DECIMAL   (18,2)      NOT NULL, 
                [GrossStandardVolume]   DECIMAL   (18,2)      NULL, 
                [MeasurementUnit]       NVARCHAR  (150)       NOT NULL, 
                [EventType]             NVARCHAR  (25)        NOT NULL, 
                [SystemName]            NVARCHAR  (150)       NOT NULL, 
                [BackupmovementId]      VARCHAR   (50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL, 
                [GlobalmovementId]      NVARCHAR  (50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL, 
                [ProductId]             NVARCHAR  (20)        NOT NULL, 
                [Category]              NVARCHAR  (150)       NOT NULL, 
                [Element]               NVARCHAR  (150)       NOT NULL, 
                [NodeName]              NVARCHAR  (156)       NOT NULL, 
                [CalculationDate]       DATE                  NULL, 
                [TicketId]              INT                   NULL,

                --Internal Common Columns 
                [CreatedBy]				NVARCHAR (260)		  NOT NULL,
                [CreatedDate]			DATETIME			  NOT NULL,  
                [LastModifiedBy]		NVARCHAR (260)		  NULL,
                [LastModifiedDate]		DATETIME			  NULL
              ) 
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds movementid  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'MovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds batchid  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds movementtransactionid  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds operationaldate  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'OperationalDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds date  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Date'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds operacion  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Operacion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds sourcenode  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'SourceNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds destinationnode  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds sourceproduct  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'SourceProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds destinationproduct  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds netstandardvolume  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds grossstandardvolume  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds measurementunit  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds eventtype  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds systemname  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds backupmovementid  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'BackupMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds globalmovementid  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'GlobalMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds productid  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds category  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds element  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'Element'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds nodename  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds calculationdate  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds createdby  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds createddate  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds lastmodifiedby  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds lastmodifieddate  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This holds lastmodifieddate  of row',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The ticket identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'BackupMovementDetailsWithoutOwner',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'