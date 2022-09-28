/*==============================================================================================================================
--Author:        InterGrupo
--Created date : May-17-2021
--Updated date : May-17-2021
--Updated date : Ago-02-2021 Change name NetStandartVolume to Ownershipvolume
--Updated date : Ago-04-2021 Change length HomologatedMovementType to 150
--Updated date : Ago-30-2022 Add column ConcatMovementId
--<Description>: This table holds logistics movements related information that will be sent to sap. </Description>
================================================================================================================================*/
CREATE TABLE [Offchain].[LogisticMovement] 
(
	--Columns    
    [LogisticMovementId]			    INT Identity(1,1)   NOT NULL,
	[MovementTransactionId]			    INT                 NOT NULL,
    [TicketId]                  INT                 NULL, 

    -- campos homologables -- 
    [EventType]                         NVARCHAR(20)       NOT NULL,
    [DestinationSystem]                 NVARCHAR(20)       NULL,    
    [MovementOrder]                     INT                NULL,
    [NumReg]                            INT                NULL,
    [NodeOrder]                         INT                NULL,    
    [StartTime]                         DATETIME           NULL,
    [SourceLogisticCenterId]            NVARCHAR(4)        NULL, --SourcePlant  
    [DestinationLogisticCenterId]       NVARCHAR(4)        NULL, --DestinationPlant
    [OwnershipVolume]                 DECIMAL(18, 2)             NULL,
    [MeasurementUnit]                   BIGINT             NULL,
    [LogisticMovementTypeId]            NVARCHAR(30)       NULL,
    [HomologatedMovementType]           NVARCHAR(150)       NULL, 
    [DocumentNumber]			    NVARCHAR(10)       NULL, --SalesOrd
--PoNumber
    [Position]                 INT                NULL, --PoItem
--PositionOrd
    [CostCenterId]			        INT       NULL, -- Centro de Costo
    [SapTransactionCode]                NVARCHAR(2)        NULL, -- GM_CODE
    [StatusProcessId]                     INT       NULL, -- Estado de Respuesta de SAP or True   
    [MessageProcess]                    NVARCHAR(MAX)       NULL, 
    [SapTransactionId]                  NVARCHAR(10)       NULL,
    [IsCheck]			                INT                NOT NULL DEFAULT 0,  --IsCheck 
    [SapSentDate]                       DATETIME            NULL, 
    --Internal Common Columns
    [CreatedBy]                         NVARCHAR (260)  NOT NULL,
    [CreatedDate]                       DATETIME        NOT NULL DEFAULT Admin.udf_GetTrueDate(),
    [LastModifiedBy]	                NVARCHAR(260)   NULL,
	[LastModifiedDate]	                DATETIME	    NULL,
    [SourceProductId]                   NVARCHAR(20)    NULL, 
    [DestinationProductId]              NVARCHAR(20)    NULL, 
    [SourceLogisticNodeId]              INT             NULL, 
    [DestinationLogisticNodeId]         INT             NULL, 
    [ConcatMovementId]                  VARCHAR(100)    COLLATE SQL_Latin1_General_CP1_CS_AS,
    --Constraints
    CONSTRAINT [PK_LogisticMovement]		            PRIMARY KEY NONCLUSTERED ([LogisticMovementId] ASC),
    CONSTRAINT [FK_LogisticMovement_Movement]		    FOREIGN KEY ([MovementTransactionId])				REFERENCES [Offchain].[Movement] ([MovementTransactionId]),
    CONSTRAINT [FK_LogisticMovement_CategoryElement]	FOREIGN KEY ([CostCenterId])                REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_LogisticMovement_Ticket]		        FOREIGN KEY ([TicketId])			        REFERENCES [Admin].[Ticket] ([TicketId]),
    CONSTRAINT [UC_LogisticMovement]		            UNIQUE CLUSTERED ([MovementTransactionId],[TicketId]),
    
   
);
GO

--CREATE NONCLUSTERED INDEX NCI_MovementLogistic_MovementTransactionId
--ON [Offchain].[MovementLogistic] (MovementTransactionId)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the DestinationSystem',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'DestinationSystem'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The order number of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = 'MovementOrder'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The total number of register ',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'NumReg'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The order number of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = 'NodeOrder'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Startime of period',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'StartTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The homologation of the logistics center and source node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = 'SourceLogisticCenterId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The homologation of the logistics center and destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = 'DestinationLogisticCenterId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the net standard volume',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = 'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The unit of measure',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Sales Orden',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = 'DocumentNumber'
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The item number of the purchasing document',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = 'Position'
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of cost center',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = 'CostCenterId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Transformation according to the type of Movement Sap',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = 'SapTransactionCode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Status Process Sap or True',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = 'StatusProcessId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The movement check',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'IsCheck'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the logistic movement type',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'LogisticMovementTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of registers',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = 'LogisticMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the event (Create)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The message returned by sap or True',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = 'MessageProcess'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier returned by sap from a successful process',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'SapTransactionId'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The sending logistics movements to sap ticket identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = 'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the logistic movement type Homologated',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'HomologatedMovementType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when sent movement to sap ',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'LogisticMovement',
    @level2type = N'COLUMN',
    @level2name = N'SapSentDate'