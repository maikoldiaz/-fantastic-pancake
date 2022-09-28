/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Nov-06-2020
-- Updated Date:	Oct-05-2020  Adding indexes to improve the query performance
 <Description>:		This table holds the details for the Delta errors.  </Description>

-- ================================================================================================================================*/

CREATE TABLE [Admin].[DeltaError]
(
	[DeltaErrorId]					INT IDENTITY (1, 1)		   NOT NULL,	
	[MovementTransactionId]			INT						   NULL,
	[InventoryProductId]			INT 					   NULL,
	[TicketId]						INT						   NOT NULL,
    [ErrorMessage]                  NVARCHAR(MAX)              NOT NULL,

	--Internal Common Columns
	[CreatedBy]											NVARCHAR (260)			NOT NULL,
	[CreatedDate]										DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]									NVARCHAR (260)			NULL,
	[LastModifiedDate]									DATETIME				NULL,

	--Constraints
	CONSTRAINT [PK_DeltaError]						PRIMARY KEY CLUSTERED ([DeltaErrorId] ASC),
	CONSTRAINT [FK_DeltaError_Movement]				FOREIGN KEY([MovementTransactionId])		REFERENCES [Offchain].[Movement] ([MovementTransactionId]),
	CONSTRAINT [FK_DeltaError_InventoryProduct]		FOREIGN KEY([InventoryProductId])			REFERENCES [Offchain].[InventoryProduct] ([InventoryProductId]),
	CONSTRAINT [FK_DeltaError_Ticket]				FOREIGN KEY([TicketId])						REFERENCES [Admin].[Ticket] ([TicketId])
)

GO

CREATE NONCLUSTERED INDEX NCIX_DeltaError_MovTranId 
ON [Admin].[DeltaError] (MovementTransactionId)
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of delta error',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaError',
    @level2type = N'COLUMN',
    @level2name = N'DeltaErrorId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaError',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of inventory product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaError',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaError',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaError',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaError',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaError',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaError',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The error message',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaError',
    @level2type = N'COLUMN',
    @level2name = N'ErrorMessage'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the errors faced during delta process.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaError',
    @level2type = NULL,
    @level2name = NULL