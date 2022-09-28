/*==============================================================================================================================
--Author:        Microsoft
--Created date : Nov-21-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data for error related to the ownership node and maps it to the relevant inventory product or movement and error message.</Description>
================================================================================================================================*/
CREATE TABLE [Admin].[OwnershipNodeError]
(
	--Columns
	[OwnershipNodeErrorId]	INT IDENTITY(1,1)	NOT NULL,
	[OwnershipNodeId]		INT					NOT NULL,
	[InventoryProductId]	INT					NULL,
	[MovementTransactionId]	INT					NULL,
	[ErrorMessage]			NVARCHAR(500)		NOT NULL,
	[ExecutionDate]			DATETIME			NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)				NOT NULL,
	[CreatedDate]					DATETIME					NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)				NULL,
	[LastModifiedDate]				DATETIME					NULL,

	--Constraints
	CONSTRAINT [PK_OwnershipNodeError]					PRIMARY KEY CLUSTERED ([OwnershipNodeErrorId] ASC),
	CONSTRAINT [FK_OwnershipNodeError_OwnershipNode]	FOREIGN KEY (OwnershipNodeId) REFERENCES [Admin].[OwnershipNode]([OwnershipNodeId]),
	CONSTRAINT [FK_OwnershipNodeError_InventoryProduct]	FOREIGN KEY (InventoryProductId) REFERENCES [Offchain].[InventoryProduct]([InventoryProductId]),
	CONSTRAINT [FK_OwnershipNodeError_Movement]			FOREIGN KEY (MovementTransactionId)	REFERENCES [Offchain].[Movement]([MovementTransactionId])

);



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ownership node error',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeError',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipNodeErrorId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ownership node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeError',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of inventory product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeError',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeError',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The error message related with ownership node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeError',
    @level2type = N'COLUMN',
    @level2name = N'ErrorMessage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is executed',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeError',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeError',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeError',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeError',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeError',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for error related to the ownership node and maps it to the relevant inventory product or movement and error message.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeError',
    @level2type = NULL,
    @level2name = NULL