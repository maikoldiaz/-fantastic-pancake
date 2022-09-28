/*===========================================================================================================================================================
-- Author:          Microsoft
-- Created date:	Oct-11-2019
-- Updated date:	Mar-20-2020
--<Description>: This table holds the details for Unbalance. </Description>
===============================================================================================================================================================*/
CREATE TABLE [Offchain].[Unbalance]
(
	--Columns
	[UnbalanceId]					        INT IDENTITY (1, 1)		   NOT NULL,	
    [TicketId]						        INT						   NOT NULL,
	[NodeId]						        INT						   NOT NULL,
	[ProductId]								NVARCHAR (20)			   NOT NULL,
	[InitialInventory]						DECIMAL(18, 2)			   NOT NULL,
	[Inputs]								DECIMAL(18, 2)			   NOT NULL,
	[Outputs]								DECIMAL(18, 2)			   NOT NULL,
	[FinalInvnetory]						DECIMAL(18, 2)			   NOT NULL,
	[IdentifiedLosses]						DECIMAL(18, 2)			   NOT NULL,
	[Unbalance]								DECIMAL(18, 2)			   NULL,
	[Interface]								DECIMAL(18, 2)			   NOT NULL,
	[Tolerance]								DECIMAL(18, 2)			   NULL,
	[UnidentifiedLosses]					DECIMAL(18, 2)			   NULL,
	[CalculationDate]						DATETIME				   NOT NULL	DEFAULT Admin.udf_GetTrueDate(),
	[InterfaceUnbalance]					DECIMAL(18, 2)			   NULL,
	[ToleranceUnbalance]					DECIMAL(18, 2)			   NULL,
	[UnidentifiedLossesUnbalance]			DECIMAL(18, 2)			   NULL,
	[StandardUncertainty]					DECIMAL(18, 2)			   NULL,
	[AverageUncertainty]					DECIMAL(18, 2)			   NULL,
	[AverageUncertaintyUnbalancePercentage]	DECIMAL(18, 2)			   NULL,
	[Warning]								DECIMAL(18, 2)			   NULL,
	[Action]								DECIMAL(18, 2)			   NULL,
	[ControlTolerance]						DECIMAL(18, 2)			   NULL,
	[ToleranceIdentifiedLosses]				DECIMAL(18, 2)			   NULL,
	[ToleranceInputs]						DECIMAL(18, 2)			   NULL,
	[ToleranceOutputs]						DECIMAL(18, 2)			   NULL,
	[ToleranceInitialInventory]				DECIMAL(18, 2)			   NULL,
	[ToleranceFinalInventory]				DECIMAL(18, 2)			   NULL,
	[TransactionHash]	                    NVARCHAR(255)	           NULL,
	[BlockNumber]		                    NVARCHAR(255)	           NULL,
    [BlockchainStatus]	                    INT                        NULL,
    [RetryCount]		                    INT                        NOT NULL DEFAULT 0,

	--Internal Common Columns
	[CreatedBy]						        NVARCHAR (260)			   NOT NULL,
	[CreatedDate]					        DATETIME				   NOT NULL DEFAULT Admin.udf_GetTrueDate(),
    [LastModifiedBy]	                    NVARCHAR(260)              NULL,
	[LastModifiedDate]	                    DATETIME	               NULL,

	--Constraints
    CONSTRAINT [PK_Unbalance]	PRIMARY KEY CLUSTERED ([UnbalanceId] ASC),
    CONSTRAINT [FK_Unbalance_Ticket_TicketId]		FOREIGN KEY ([TicketId])			REFERENCES [Admin].[Ticket]([TicketId]),
	CONSTRAINT [FK_Unbalance_Node_NodeId]			FOREIGN KEY ([NodeId])				REFERENCES [Admin].[Node]([NodeId]),
	CONSTRAINT [FK_Unbalance_Product_ProductId]		FOREIGN KEY ([ProductId])			REFERENCES [Admin].[Product]([ProductId])
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'UnbalanceId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the initial inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'InitialInventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the inputs',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'Inputs'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the outputs',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'Outputs'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the final inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'FinalInvnetory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the identified losses',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'IdentifiedLosses'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'Unbalance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the interface',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'Interface'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the tolerance',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'Tolerance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the unidentified losses',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'UnidentifiedLosses'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is calculated',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the interface unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'InterfaceUnbalance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the tolerance unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'ToleranceUnbalance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the unidentified losses unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'UnidentifiedLossesUnbalance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the standard uncertainty',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'StandardUncertainty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the average of the uncertainty ',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'AverageUncertainty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the average of uncertainty unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'AverageUncertaintyUnbalancePercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the warning',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'Warning'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the action',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'Action'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the control tolerance',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'ControlTolerance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the tolerance of identified losses',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'ToleranceIdentifiedLosses'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the tolerance of inputs',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'ToleranceInputs'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the tolerance of outputs',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'ToleranceOutputs'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the tolerance of initial inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'ToleranceInitialInventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the tolerance of final inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'ToleranceFinalInventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The last modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record was modified (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the details for Unbalance.',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The blockchain transaction hash',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'TransactionHash'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The blockchain block number',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'BlockNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The blockchain status',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'BlockchainStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The blockchain retry count',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Unbalance',
    @level2type = N'COLUMN',
    @level2name = N'RetryCount'
GO