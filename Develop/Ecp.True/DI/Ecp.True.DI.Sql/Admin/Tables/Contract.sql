/*==============================================================================================================================
--Author:        Microsoft
--Created Date : Jan-06-2020	
--Updated Date : Mar-20-2020
--<Description>: This table holds the data for purchases and sales contracts registered in the system. </Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[Contract]
(
	--Columns
    [ContractId]			INT	 IDENTITY(1,1)	NOT NULL,
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
    [SourceSystem]          NVARCHAR(10) NULL, 
    [DateOrder]             DATETIME NULL, 
    [DateReceivedPo] datetime NULL,
	[EventType] nvarchar(20) NULL,
	[MessageId] nvarchar(100) NULL,
	[PurchaseOrderType] int NULL,
	[ExpeditionClass] int NULL,
	[Status] nvarchar(20) NULL,
	[StatusCredit] nvarchar(20) NULL,
	[DescriptionStatus] nvarchar(20) NULL,
	[PositionStatus] nvarchar(10) NULL,
	[Frequency] nvarchar(20) NULL,
	[EstimatedVolume] decimal(18, 2) NULL,
	[Tolerance] decimal(18, 2) NULL,
	[Value] decimal(18, 2) NULL,
    [Property] NVARCHAR(20) NULL, 
	[Uom] nvarchar(10) NULL,
	[DestinationStorageLocationId] nvarchar(10) NULL,
	[Batch] nvarchar(10) NULL,
    [FileRegistrationTransactionId] INT NULL,
	[IsDeleted]				BIT					NULL,
    [OriginMessageId]               NVARCHAR(50)    NULL, 
	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,


   
     
    --Constraints
    CONSTRAINT [PK_Contract]									PRIMARY KEY CLUSTERED ([ContractId] ASC),
	CONSTRAINT [FK_Contract_Node_SourceNodeId]					FOREIGN KEY	([SourceNodeId])		REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_Contract_Node_DestinationNodeId]				FOREIGN KEY	([DestinationNodeId])	REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_Contract_Product]							FOREIGN KEY	([ProductId])			REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [FK_Contract_CategoryElement_Owner1Id]			FOREIGN KEY	([Owner1Id])			REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_Contract_CategoryElement_Owner2Id]			FOREIGN KEY	([Owner2Id])			REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_Contract_CategoryElement_MovementTypeId]		FOREIGN KEY ([MovementTypeId])		REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_Contract_CategoryElement_MeasurementUnit]	FOREIGN KEY ([MeasurementUnit])		REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_Contract_FileRegistrationTransaction]		FOREIGN KEY	([FileRegistrationTransactionId])		REFERENCES [Admin].[FileRegistrationTransaction] ([FileRegistrationTransactionId])
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the contract',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'ContractId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The document number',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'DocumentNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The position',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'Position'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'MovementTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The start date of the contract',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The end date of the contract',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'EndDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the first owner (category element of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'Owner1Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the second owner (category element of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'Owner2Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'Volume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the measurement unit (category element of unit category, like Bbl)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the contract is deleted or not, 1 means delete',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'IsDeleted'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for purchases and sales contracts registered in the system.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Source system data sent by SAP',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystem'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Date the Order was created',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'DateOrder'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Date and time assigned by PO upon receiving the order',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'DateReceivedPo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Order action (Create / Modify)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Identifier created by SAP',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'MessageId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Order Document Number',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'PurchaseOrderType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Corresponds to the type of node in TRUE (Line, Tank truck or station)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'ExpeditionClass'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Order status (Valores permitidos: "Activa" o "Desautorizada".)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Position Status (valores "Vacio" , "L" Borrado,  o  "S" Bloqueado)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'PositionStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Order frequency (valores: "Diario", "Mensual", "Quincenal")',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'Frequency'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Order tolerance percentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'Tolerance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Volume budget of the order',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'EstimatedVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'If the property value is equal to "Purchase-Percentage" validate that the data is a number between 0 and 100 ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'Value'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The ownership of the order (Valores permitidos:  "Compra-Porcentaje" o "Compra-Volumen")',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'Uom'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination warehouse, SAP PO will send the identifier of the StorageLocation',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'DestinationStorageLocationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The sales Product Lot Identifier, generated by SAP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'Batch'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Order Property (Valores permitidos:  "Compra-Porcentaje" o "Compra-Volumen")',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Contract',
    @level2type = N'COLUMN',
    @level2name = N'Property'