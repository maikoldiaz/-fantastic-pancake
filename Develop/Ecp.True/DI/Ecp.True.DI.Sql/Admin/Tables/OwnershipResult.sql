/*==============================================================================================================================
--Author:        Microsoft
--Created date : Nov-21-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data for OwnershipResult.</Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[OwnershipResult]
(
	--Columns
	[OwnershipResultId]			INT IDENTITY(1,1)	NOT NULL,
	[MessageTypeId]				INT					NOT NULL,
	[MovementTransactionId] 	INT					NULL,
	[InventoryProductId]		INT					NULL,
	[NodeId]					INT					NOT NULL,
	[ProductId]					NVARCHAR(20)		NOT NULL,
	[ExecutionDate]				DATETIME			NOT NULL,
	[InitialInventory]			DECIMAL(18, 2)		NOT NULL,
	[FinalInventory]			DECIMAL(18, 2)		NOT NULL,
	[Input]						DECIMAL(18, 2)		NOT NULL,
	[Output]					DECIMAL(18, 2)		NOT NULL,
	[OwnerId]					INT					NOT NULL,
	[OwnershipPercentage]		DECIMAL(5, 2)		NOT NULL,
	[OwnershipVolume]			DECIMAL(18, 2)		NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)				NOT NULL,
	[CreatedDate]					DATETIME					NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)				NULL,
	[LastModifiedDate]				DATETIME					NULL,
	
	--Constraints
	CONSTRAINT [PK_OwnershipResult]						PRIMARY KEY CLUSTERED ([OwnershipResultId] ASC),
	CONSTRAINT [PK_OwnershipResult_MessageType]			FOREIGN KEY (MessageTypeId)				REFERENCES [Admin].[MessageType]([MessageTypeId]),
	CONSTRAINT [FK_OwnershipResult_InventoryProduct]	FOREIGN KEY (InventoryProductId)		REFERENCES [Offchain].[InventoryProduct]([InventoryProductId]),
	CONSTRAINT [FK_OwnershipResult_Movement]			FOREIGN KEY (MovementTransactionId)		REFERENCES [Offchain].[Movement]([MovementTransactionId]),
	CONSTRAINT [FK_OwnershipResult_Node]				FOREIGN KEY (NodeId)					REFERENCES [Admin].[Node]([NodeId]),
	CONSTRAINT [FK_OwnershipResult_Product]				FOREIGN KEY (ProductId)					REFERENCES [Admin].[Product]([ProductId]),
);



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ownership result',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipResultId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the type of the message',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'MessageTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the inventory product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime of the execution ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the initial inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'InitialInventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the final inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'FinalInventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the input',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'Input'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the output',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'Output'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the owner (categoryelement of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'OwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipResult',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'