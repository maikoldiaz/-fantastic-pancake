CREATE TABLE [Admin].[MovementSendSapInformation]
(
	[SapStatus]				NVARCHAR (150)	 NOT NULL,
	[TypeOfMovement]	    NVARCHAR (150)	 NOT NULL,
	[SourceNode]			NVARCHAR (150)	 NULL,
	[DestinationNode]		NVARCHAR (150)	 NULL,
	[SourceProduct]			NVARCHAR (150)	 NULL,
	[DestinationProduct]	NVARCHAR (150)	 NULL,
    [Scenario]	NVARCHAR (150)	 NULL,
	[StartDate]             DATETIME         NOT NULL,		 
	[EndDate]               DATETIME         NOT NULL,		 
	[OwnerName]				NVARCHAR (150)	 NOT NULL,
	[OwnershipVolume]		DECIMAL(18,2)	 NOT NULL,
	[MovementId]			VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
	[MovementTransactionId]	INT				 NOT NULL,
	[InputElement]		    NVARCHAR (150)	 NOT NULL,
	[ExecutionId] 		    INT	 NOT NULL,

    --Internal Common Columns															
    [CreatedBy]			    NVARCHAR (260)	 NOT NULL,
    [CreatedDate]			DATETIME		 NOT NULL    DEFAULT Admin.udf_GetTrueDate()
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Status Process Sap',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'SapStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description of the message type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'TypeOfMovement'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'SourceNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'SourceProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The start date of the movement sent to sap',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The end date of the movement sent to sap',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'EndDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the owner (category element of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'OwnerName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'MovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'InputElement'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The execution id unique to session received from UI',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description of the Scenario.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementSendSapInformation',
    @level2type = N'COLUMN',
    @level2name = N'Scenario'