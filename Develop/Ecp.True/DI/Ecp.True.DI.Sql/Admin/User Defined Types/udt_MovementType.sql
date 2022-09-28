
/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Oct-29-2020
 <Description>:		This is used as a parameter for Save Delta bulk Movements SP.  </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[MovementType] AS TABLE
(
	--Columns
	[Id]							INT NULL,
	[TempId]						INT NOT NULL,
	[MovementTypeId]                INT NOT NULL,
	[MessageTypeId]					INT NOT NULL,
	[SystemTypeId]					INT NOT NULL,
	[SourceSystemId]				INT NULL,
	[EventType]						NVARCHAR (25) NOT NULL,
	[MovementId]					VARCHAR (50) NOT NULL,
	[IsSystemGenerated]				BIT NULL,
	[OfficialDeltaTicketId]			INT NULL,
	[ScenarioId]					INT NOT NULL,
	[Observations]					NVARCHAR (150) NULL,
	[Classification]				NVARCHAR (30) NOT NULL,
	[PendingApproval]				BIT NULL,
	[NetStandardVolume]				DECIMAL (18, 2) NOT NULL,
	[SourceMovementTransactionId]	INT NULL,
	[MeasurementUnit]				INT NULL,
	[SegmentId]						INT NULL,
	[OperationalDate]				DATE NOT NULL,
	[OfficialDeltaMessageTypeId]	INT NULL,
	[PeriodStartTime]				DATETIME NOT NULL,
	[PeriodEndTime]					DATETIME NOT NULL,
	[SourceNodeId]					INT NULL,
	[SourceProductId]				NVARCHAR (20) NULL,
	[SourceProductTypeId]			INT NULL,
	[DestinationNodeId]				INT NULL,
	[DestinationProductId]			NVARCHAR (20) NULL,
	[DestinationProductTypeId]		INT NULL,
	[SourceInventoryProductId]      INT NULL,
	[ConsolidatedMovementTransactionId]  INT NULL,
    [ConsolidatedInventoryProductId] INT NULL,
	[OriginalMovementTransactionId]  INT NULL,
	[BlockchainStatus]               INT NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)	NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is used as a parameter for Save Delta bulk Movements SP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'MovementType',
    @level2type = NULL,
    @level2name = NULL
