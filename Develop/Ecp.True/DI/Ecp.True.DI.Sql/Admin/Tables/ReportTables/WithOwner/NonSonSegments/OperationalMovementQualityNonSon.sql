/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-03-2020

--<Description>: This table holds the data for quality attributes of movements before cutoff for Non Sons Segments. This table is being used in before cutoff report.</Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[OperationalMovementQualityNonSon]
(
	--Columns
    [OperationalMovementQualityNonSonId]            INT                                 NOT NULL IDENTITY(1,1),
	[RNo]						                    INT							        NOT NULL,
	[BatchId]                        		        NVARCHAR (25)                       NULL,
	[MovementId]					                VARCHAR	 (50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
	[MovementTransactionId]				            INT                                 NULL,
	[CalculationDate]				                DATETIME						    NOT NULL,
	[MovementTypeName]				                NVARCHAR (150)						NULL,
	[SourceNode]					                NVARCHAR (150)						NULL,
	[DestinationNode]				                NVARCHAR (150)						NULL,
	[SourceProduct]					                NVARCHAR (150)						NULL,
	[DestinationProduct]				            NVARCHAR (150)						NULL,
	[NetStandardVolume]				                DECIMAL	 (18,2)						NULL,
	[GrossStandardVolume]				            DECIMAL	 (18,2)						NULL,
	[MeasurementUnit]				                NVARCHAR (150)						NULL,
	[EventType]                      		        NVARCHAR (20)                       NULL,
	[SystemName]					                VARCHAR  (50)						NULL,	 
	[Movement]					                    NVARCHAR (150)						NULL, 
	[SourceMovementId]				                VARCHAR  (50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
    [Order]                        			        NVARCHAR (50)						NULL, 
    [Position]                     			        NVARCHAR (50)						NULL, 
	[AttributeId]                    		        NVARCHAR (150)                      NULL,
	[AttributeValue]					            NVARCHAR (150)						NULL,									
    [ValueAttributeUnit]				            NVARCHAR (50)						NULL,										
    [AttributeDescription]				            NVARCHAR (150)						NULL,	
	[ProductID]					                    NVARCHAR (20)						NOT NULL,		 
    [ExecutionId]					                INT						            NOT NULL,
	
    --Internal Common Columns		                											
	[CreatedBy]					                    NVARCHAR (260)						NOT NULL,
	[CreatedDate]					                DATETIME						    NOT NULL

   --Constraints
    CONSTRAINT [PK_OperationalMovementQualityNonSonIdId]	            PRIMARY KEY CLUSTERED (OperationalMovementQualityNonSonId ASC),
    CONSTRAINT [FK_OperationalMovementQualityNonSon_ReportExecution]	FOREIGN KEY ([ExecutionId])			REFERENCES [Admin].[ReportExecution] ([ExecutionId])
)
GO

ALTER TABLE [Admin].[OperationalMovementQualityNonSon]	SET (LOCK_ESCALATION = DISABLE)
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The unique identifier of OperationalMovementQualityNonSon',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'OperationalMovementQualityNonSonId'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Source MovementId',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'SourceMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The correlative number of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'RNo'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The batch number of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'MovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the calculation was done',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the movement type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'MovementTypeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'SourceNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'SourceProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the net standard volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the gross standard volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the measurement unit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Movement'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'new column for now no value',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Order'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'new column for now no value',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Position'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The id of the attribute',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'AttributeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the attribute',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'AttributeValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the attribute unit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'ValueAttributeUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description of the attribute',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'AttributeDescription'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'ProductID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the event (Insert, Update, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for quality attributes of movements before cutoff for Non sons segments. This table is being used in before cutoff report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalMovementQualityNonSon',
    @level2type = NULL,
    @level2name = NULL