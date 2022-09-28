/*==============================================================================================================================
--Author:        Microsoft
--Created Date : July-06-2020
--Updated Date :	Oct-05-2020  Adding indexes to improve the query performance 
--<Description>: This table holds the data for delta nodes errors.  </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[DeltaNodeError]
(
	[DeltaNodeErrorId]				    INT IDENTITY (1, 1)		   NOT NULL,
    [DeltaNodeId]					    INT                 	   NOT NULL,
    [InventoryProductId]                INT                        NULL,
    [MovementTransactionId]             INT                        NULL,
	[ConsolidatedMovementId]	        INT	                       NULL,
    [ConsolidatedInventoryProductId]	INT		                   NULL,
    [ErrorMessage]                      NVARCHAR(MAX)              NOT NULL,

	--Internal Common Columns
	[CreatedBy]											NVARCHAR (260)			NOT NULL,
	[CreatedDate]										DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),

	--Constraints
	CONSTRAINT [PK_DeltaNodeError]					PRIMARY KEY CLUSTERED ([DeltaNodeErrorId] ASC),
	CONSTRAINT [FK_DeltaNodeError_DeltaNode]			            FOREIGN KEY ([DeltaNodeId])		                    REFERENCES [Admin].[DeltaNode] ([DeltaNodeId]),
	CONSTRAINT [FK_DeltaNodeError_InventoryProduct]			        FOREIGN KEY ([InventoryProductId])		            REFERENCES [Offchain].[InventoryProduct] ([InventoryProductId]),
	CONSTRAINT [FK_DeltaNodeError_MovementTransaction]			    FOREIGN KEY ([MovementTransactionId])		        REFERENCES [Offchain].[Movement] ([MovementTransactionId]),
	CONSTRAINT [FK_DeltaNodeError_ConsolidatedMovement]			    FOREIGN KEY	([ConsolidatedMovementId])			    REFERENCES [Admin].[ConsolidatedMovement] ([ConsolidatedMovementId]),
	CONSTRAINT [FK_DeltaNodeError_ConsolidatedInventoryProduct]		FOREIGN KEY	([ConsolidatedInventoryProductId])      REFERENCES [Admin].[ConsolidatedInventoryProduct] ([ConsolidatedInventoryProductId]),
)

GO

CREATE NONCLUSTERED INDEX NCIX_DeltaNodeError_MovInTranId
ON [Admin].[DeltaNodeError] (MovementTransactionId)
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of delta node error',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeError',
    @level2type = N'COLUMN',
    @level2name = N'DeltaNodeErrorId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of delta node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeError',
    @level2type = N'COLUMN',
    @level2name = N'DeltaNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of inventory product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeError',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of movement transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeError',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The error message',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeError',
    @level2type = N'COLUMN',
    @level2name = N'ErrorMessage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ConsolidatedMovementTransaction',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeError',
    @level2type = N'COLUMN',
    @level2name = N'ConsolidatedMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ConsolidatedInventoryProduct',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeError',
    @level2type = N'COLUMN',
    @level2name = N'ConsolidatedInventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeError',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeError',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for delta nodes errors.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeError',
    @level2type = NULL,
    @level2name = NULL