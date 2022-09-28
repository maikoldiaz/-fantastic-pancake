/*-- =================================================================================================
-- Author:		Microsoft
-- Create date: March-27-2020
--<Description>:	This Table is to store contracts specific to movements.  </Description>
-- ===================================================================================================*/


CREATE TABLE [Admin].[MovementContract]
(
	--Columns
    [MovementContractId]	INT	 IDENTITY(1,1)	NOT NULL,
	[DocumentNumber]		INT					NOT NULL,
	[Position]				INT					NOT NULL,
	[MovementTypeId]		INT					NOT NULL,
	[SourceNodeId]			INT					NULL,
	[DestinationNodeId]		INT					NULL,
	[ProductId]				NVARCHAR(20)		NOT NULL,
	[StartDate]				DATETIME			NOT NULL,
	[EndDate]				DATETIME			NOT NULL,
	[Owner1Id]				INT					NULL	DEFAULT		124, -- Owner OTROS
	[Owner2Id]				INT					NULL	DEFAULT		124, -- Owner OTROS
	[Volume]				DECIMAL(18, 2)		NOT NULL,
	[MeasurementUnit]		INT					NOT NULL,
	[IsDeleted]				BIT					NULL,
	[ContractId]			INT					NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_MovementContract]									PRIMARY KEY CLUSTERED ([MovementContractId] ASC),
	CONSTRAINT [FK_MovementContract_Node_SourceNodeId]					FOREIGN KEY	([SourceNodeId])		REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_MovementContract_Node_DestinationNodeId]				FOREIGN KEY	([DestinationNodeId])	REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_MovementContract_Product]							FOREIGN KEY	([ProductId])			REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [FK_MovementContract_CategoryElement_Owner1Id]			FOREIGN KEY	([Owner1Id])			REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_MovementContract_CategoryElement_Owner2Id]			FOREIGN KEY	([Owner2Id])			REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_MovementContract_CategoryElement_MovementTypeId]		FOREIGN KEY ([MovementTypeId])		REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_MovementContract_CategoryElement_MeasurementUnit]	FOREIGN KEY ([MeasurementUnit])		REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_MovementContract_Contract_ContractId]				FOREIGN KEY ([ContractId])			REFERENCES [Admin].[Contract] ([ContractId])
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement contract',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'MovementContractId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The number of the document ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'DocumentNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the position',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'Position'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'MovementTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the movement contract is started',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the movement contract is ended',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'EndDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the first owner (category element of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'Owner1Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the second owner (category element of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'Owner2Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The volumen',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'Volume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name or identifier of the measurement unit ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the movement contract is deleted or not, 1 means delete',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'IsDeleted'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the contract',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'ContractId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Table is to store contracts specific to movements',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementContract',
    @level2type = NULL,
    @level2name = NULL