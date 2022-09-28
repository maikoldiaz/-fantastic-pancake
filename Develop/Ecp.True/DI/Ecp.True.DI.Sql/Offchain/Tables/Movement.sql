/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Nov-11-2019
-- Updated Date:	Mar-31-2020
-- Updated Date:	Sep-30-2020  -- Created indexes to improve the performance
 <Description>:		This table holds the details related to Movements.  </Description>
-- ================================================================================================================================*/
CREATE TABLE [Offchain].[Movement]
(
	--Columns
	[MovementTransactionId]						INT IDENTITY (1, 1)			NOT NULL,
	[MessageTypeId]								INT							NOT NULL,
	[SystemTypeId]								INT							NOT NULL,
	[EventType]									NVARCHAR (25)				NOT NULL,
	[MovementId]								VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
	[MovementTypeId]							INT         				NOT NULL,
	[TicketId]									INT							NULL,
	[SegmentId]									INT							NULL,
	[OperationalDate]							DATE    					NOT NULL,
	[GrossStandardVolume]						DECIMAL(18, 2)				NULL,
	[NetStandardVolume]							DECIMAL(18, 2)				NOT NULL,
	[UncertaintyPercentage]						DECIMAL(5, 2)				NULL,
	[MeasurementUnit]							INT         				NULL,
	[ScenarioId]								INT			                NOT NULL,           
	[Observations]								NVARCHAR (200)				NULL,
	[Classification]							NVARCHAR (30)				NOT NULL,
	[IsDeleted]									BIT							NOT NULL			DEFAULT 0,		--> 1=Deleted
	[IsSystemGenerated]							BIT							NULL,
	[VariableTypeId]							INT							NULL,
	[FileRegistrationTransactionId]				INT							NULL,
	[OwnershipTicketId]							INT							NULL,
	[ReasonId]									INT							NULL,
	[Comment]									NVARCHAR(200)				NULL,
	[MovementContractId]						INT							NULL,
	[BlockchainStatus]							INT							NULL,
	[BlockchainMovementTransactionId]			UniqueIdentifier			NULL,
	[PreviousBlockchainMovementTransactionId]	UniqueIdentifier			NULL,
	[Tolerance]									DECIMAL(18,2)				NULL,
	[OperatorId]							    INT				            NULL,
	[BackupMovementId]							VARCHAR(50)	COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
	[GlobalMovementId]							NVARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
	[BalanceStatus]								NVARCHAR(50)				NULL,
	[SAPProcessStatus]							NVARCHAR(50)				NULL,
	[MovementEventId]							INT							NULL,
	[SourceMovementId]							INT							NULL,
	[TransactionHash]							NVARCHAR(255)				NULL,
	[BlockNumber]								NVARCHAR(255)				NULL,
	[RetryCount]								INT							NOT NULL			DEFAULT 0,
    [IsOfficial]								BIT							NOT NULL			DEFAULT 0, --0-Not an Official Transfer Point
    [Version]                                   NVARCHAR(50)                NULL,
    [BatchId]								    NVARCHAR(25)			    NULL,    
    [SystemId]                                  INT                         NULL,
    [SourceSystemId]                            INT                         NULL,
    [IsTransferPoint]                           BIT                         NOT NULL DEFAULT 0,
    [SourceMovementTransactionId]               INT                         NULL,
    [SourceInventoryProductId]                  INT                         NULL,
    [DeltaTicketId]                             INT                         NULL,
    [OfficialDeltaTicketId]                     INT                         NULL,
    [PendingApproval]                           BIT                         NULL,
    [OfficialDeltaMessageTypeId]                INT                         NULL,
    [ConsolidatedMovementTransactionId]         INT                         NULL,
    [ConsolidatedInventoryProductId]            INT                         NULL,
    [OriginalMovementTransactionId]             INT                         NULL,
    [OwnershipTicketConciliationId]             INT                         NULL,
    [IsReconciled]                              BIT                         NULL DEFAULT NULL,
	--Internal Common Columns
	[CreatedBy]									NVARCHAR (260)				NOT NULL,
	[CreatedDate]								DATETIME					NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]							NVARCHAR (260)				NULL,
	[LastModifiedDate]							DATETIME					NULL,

	--Constraints
	CONSTRAINT [PK_Movement]						PRIMARY KEY CLUSTERED ([MovementTransactionId] ASC),
	CONSTRAINT [FK_Movement_MessageType]			FOREIGN KEY	([MessageTypeId])		REFERENCES [Admin].[MessageType] ([MessageTypeId]),
	CONSTRAINT [FK_Movement_Ticket_TicketId]		FOREIGN KEY ([TicketId])			REFERENCES [Admin].[Ticket]([TicketId]),
	CONSTRAINT [FK_Movement_SystemType]				FOREIGN KEY	([SystemTypeId])		REFERENCES [Admin].[SystemType] ([SystemTypeId]),
	CONSTRAINT [FK_Movement_VariableType]			FOREIGN KEY	([VariableTypeId])		REFERENCES [Admin].[VariableType] ([VariableTypeId]),
	CONSTRAINT [FK_Movement_FileRegistrationTransaction]			FOREIGN KEY	([FileRegistrationTransactionId])		REFERENCES [Admin].[FileRegistrationTransaction] ([FileRegistrationTransactionId]),
	CONSTRAINT [FK_Movement_CategoryElement]		FOREIGN KEY([SegmentId])    		REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_Movement_Ticket_Ownership]		FOREIGN KEY([OwnershipTicketId])	REFERENCES [Admin].[Ticket] ([TicketId]),
	CONSTRAINT [FK_Movement_CategoryElement_Reason]	FOREIGN KEY ([ReasonId])			REFERENCES [Admin].[CategoryElement]([ElementId]),
	CONSTRAINT [FK_Movement_MovemnetContract]		FOREIGN KEY ([MovementContractId])	REFERENCES [Admin].[MovementContract]([MovementContractId]),
	CONSTRAINT [FK_Movement_MovementEvent]			FOREIGN KEY ([MovementEventId])		REFERENCES [Admin].[MovementEvent]([MovementEventId]),
    CONSTRAINT [FK_Movement_ScenarioType]			FOREIGN KEY ([ScenarioId])          REFERENCES [Admin].[ScenarioType] ([ScenarioTypeId]),
    CONSTRAINT [FK_Movement_CategoryElement_OperatorId]		        FOREIGN KEY ([OperatorId])    		                REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_Movement_CategoryElement_SystemId]		        FOREIGN KEY ([SystemId])			                REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_Movement_CategoryElement_SourceSystemId]		    FOREIGN KEY ([SourceSystemId])			            REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_Movement_Movement_MovementTransactionId]		    FOREIGN KEY ([SourceMovementTransactionId])			REFERENCES [Offchain].[Movement] ([MovementTransactionId]),
    CONSTRAINT [FK_Movement_InventoryProduct]		                FOREIGN KEY ([SourceInventoryProductId])			REFERENCES [Offchain].[InventoryProduct] ([InventoryProductId]),
    CONSTRAINT [FK_Movement_Ticket_Delta]		                    FOREIGN KEY ([DeltaTicketId])			            REFERENCES [Admin].[Ticket] ([TicketId]),
    CONSTRAINT [FK_Movement_Ticket_OfficialDelta]		            FOREIGN KEY ([OfficialDeltaTicketId])			    REFERENCES [Admin].[Ticket] ([TicketId]),    
    CONSTRAINT [FK_Movement_OfficialDeltaMessageType]		        FOREIGN KEY ([OfficialDeltaMessageTypeId])			REFERENCES [Admin].[OfficialDeltaMessageType] ([OfficialDeltaMessageTypeId]),
	CONSTRAINT [FK_Movement_ConsolidatedMovement]			        FOREIGN KEY	([ConsolidatedMovementTransactionId])	REFERENCES [Admin].[ConsolidatedMovement] ([ConsolidatedMovementId]),
	CONSTRAINT [FK_Movement_ConsolidatedInventoryProduct]		    FOREIGN KEY	([ConsolidatedInventoryProductId])      REFERENCES [Admin].[ConsolidatedInventoryProduct] ([ConsolidatedInventoryProductId]),
    CONSTRAINT [FK_Movement_Movement_OriginalMovementTransactionId]	FOREIGN KEY ([OriginalMovementTransactionId])	    REFERENCES [Offchain].[Movement] ([MovementTransactionId]),
    CONSTRAINT [FK_Movement_CategoryElement_MovementTypeId]	        FOREIGN KEY ([MovementTypeId])                      REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_Movement_CategoryElement_MeasurementUnit]		FOREIGN KEY ([MeasurementUnit])                     REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_Movement_Ticket_OwnershipConciliation]		    FOREIGN KEY ([OwnershipTicketConciliationId])		REFERENCES [Admin].[Ticket] ([TicketId]),
    
);
GO
CREATE NONCLUSTERED INDEX NCI_Movement_SegmentId_ScenarioId_OperationalDate
ON [Offchain].[Movement] ([SegmentId],[ScenarioId],[OperationalDate],[OwnershipTicketId])
INCLUDE ([MessageTypeId],[EventType],[MovementId],[MovementTypeId],
         [GrossStandardVolume],[NetStandardVolume],[UncertaintyPercentage],
         [MeasurementUnit],[Classification],[SystemId],[BatchId],[SourceSystemId]
         );
GO
CREATE NONCLUSTERED INDEX NCI_Movement_MovementId
ON [Offchain].[Movement] ([MovementId]);
GO

CREATE NONCLUSTERED INDEX NCI_Movement_SaveTicket
ON [Offchain].[Movement] ([OperationalDate])
INCLUDE ([UncertaintyPercentage],[ScenarioId],[VariableTypeId],[IsTransferPoint])
GO

CREATE NONCLUSTERED INDEX NCI_Movement_MovementId_FileRegistrationTransactionId
ON [Offchain].[Movement] ([FileRegistrationTransactionId])
INCLUDE ([MovementId]);
GO

CREATE NONCLUSTERED INDEX NCI_Movement_SegmentId_IsDeleted_OperationalDate
ON [Offchain].[Movement] ([SegmentId],[IsDeleted],[OperationalDate]);
GO

CREATE NONCLUSTERED INDEX NCI_Movement_SegmentId_TicketId_Include
ON [Offchain].[Movement] ([SegmentId],[TicketId])
INCLUDE ([SystemTypeId],[MovementTypeId],[OperationalDate],[SourceSystemId])
GO

CREATE NONCLUSTERED INDEX [NCI_Movement_TicketId_SegmentId_ScenarioId_IsTransferPoint_OperationalDate_Include]
ON [Offchain].[Movement] ([TicketId],[SegmentId],[ScenarioId],[IsTransferPoint],[OperationalDate])
INCLUDE ([MovementId],[MovementTypeId],[NetStandardVolume],[MeasurementUnit],[GlobalMovementId])
GO

CREATE NONCLUSTERED INDEX [NIX_Movement_SourceMovTranId] 
ON Offchain.Movement (SourceMovementTransactionId)
GO

CREATE NONCLUSTERED INDEX [NIX_Movement_OriginalMovTranId] 
ON Offchain.Movement (OriginalMovementTransactionId)
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the message type',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'MessageTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the system type',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'SystemTypeId'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the event (like Insert, Update, etc.)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'MovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement type',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'MovementTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ticket ',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the segment',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The operational date of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'OperationalDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the gross standard volume',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the net standard volume',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the uncertainty percentage',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'UncertaintyPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the measurement unit ',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the scenario (like Operativo)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'ScenarioId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The observations of the movement (like Reporte Operativo Cusiana -Fecha)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'Observations'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The classification of the movement (cls)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'Classification'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the element is deleted or not, 1 means delete',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'IsDeleted'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the system is generated or not, 1 means generate',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'IsSystemGenerated'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of variable type (like Interface, Tolerance, Entrada, Salida, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'VariableTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of file registration transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'FileRegistrationTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ownership ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipTicketId'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the reason (category element of reason category)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'ReasonId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The comment of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'Comment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement contract',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'MovementContractId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if present in blockchain register',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'BlockchainStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the blockchain inventory product transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'BlockchainMovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the previous blockchain ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'PreviousBlockchainMovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the tolerance',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'Tolerance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the operator',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'OperatorId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the backup movement',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'BackupMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the global movement ',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'GlobalMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the balance status',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'BalanceStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the SAP Process Status',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'SAPProcessStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement event',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'MovementEventId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source movement',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'SourceMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the transaction hash',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'TransactionHash'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the block number',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'BlockNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the retry count',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'RetryCount'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of if it is official transfer point',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'IsOfficial'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the version',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'Version'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The batch identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for system',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'SystemId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for source system (category element of source system category)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystemId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the details related to Movements.',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag for identifying if transfer point or not',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'IsTransferPoint'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of source movement',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'SourceMovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of source inventory product',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'SourceInventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The delta ticket identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'DeltaTicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The official delta ticket identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'OfficialDeltaTicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag for identifying if approval is pending or not.',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'PendingApproval'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of OfficialDeltaMessageType',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'OfficialDeltaMessageTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ConsolidatedMovementTransaction',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'ConsolidatedMovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ConsolidatedInventoryProduct',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'ConsolidatedInventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of original movement',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'OriginalMovementTransactionId'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Identifier of original movement conciliation of other segment',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = 'OwnershipTicketConciliationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag for identifying if conciliation of other segment is pending or not.',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Movement',
    @level2type = N'COLUMN',
    @level2name = N'IsReconciled'