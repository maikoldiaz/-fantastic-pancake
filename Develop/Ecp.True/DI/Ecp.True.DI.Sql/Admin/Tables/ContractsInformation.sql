/*==============================================================================================================================
--Author:        Microsoft
--Created date : Apr-30-2020
--<Description>: This table holds the data for ContractsInformation, used in EventContract report. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[ContractsInformation] 
(
	--Columns
    [RNo]					INT				 NOT NULL,
	[DocumentNumber]		INT				 NOT NULL,
	[Position]			    INT				 NOT NULL,
    [TypeOfMovement]	    NVARCHAR (150)	 NOT NULL,
	[SourceNode]			NVARCHAR (150)	 NULL,
	[DestinationNode]		NVARCHAR (150)	 NULL,
	[Product]				NVARCHAR (150)	 NULL,
	[StartDate]             DATETIME         NOT NULL,		 
	[EndDate]               DATETIME         NOT NULL,		 
	[Owner1Name]			NVARCHAR (150)	 NOT NULL,
	[Owner2Name]			NVARCHAR (150)	 NOT NULL,
	[Volume]				DECIMAL(18, 2)	 NOT NULL,
	[MeasurementUnit]		NVARCHAR (150)	 NOT NULL,
	[InputElement]		    NVARCHAR (150)	 NOT NULL,
	[InputNodeName]         NVARCHAR (150)   NOT NULL,
	[ExecutionId] 		    NVARCHAR (250)	 NOT NULL,
	[SourceSystem]          nvarchar(20) NULL,
	[PurchaseOrderType]     NVARCHAR(150) NULL,
	[Status]                nvarchar(20) NULL,
	[PositionStatus]        nvarchar(20) NULL,
	[Frequency]             nvarchar(20) NULL,
	[Tolerance]             decimal(18, 2) NULL,
	[ExpeditionClass]       NVARCHAR(20) NULL,
	[EstimatedVolume]       decimal(18, 2) NULL,

    --Internal Common Columns															
    [CreatedBy]			    NVARCHAR (260)	 NOT NULL,
    [CreatedDate]			DATETIME		 NOT NULL    DEFAULT Admin.udf_GetTrueDate()
)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for ContractsInformation, used in EventContract report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The movement type name (category element of movement type category)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'TypeOfMovement'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'SourceNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'Product'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The start date of the contract',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The end data of the contract',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'EndDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The first owner name (category element of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'Owner1Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The second owner name (category element of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'Owner2Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The contract volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'Volume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the measurement unit (category element of unit category)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'InputElement'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node name',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'InputNodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The execution id unique to session received from UI',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'A rank generated over source node and start date and inserted in this column',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'RNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The document number',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'DocumentNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The position',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'Position'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Source system data sent by SAP',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystem'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Order Document Number',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'PurchaseOrderType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Order status (Valores permitidos: "Activa" o "Desautorizada".)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Position Status (valores "Vacio" , "L" Borrado,  o  "S" Bloqueado)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'PositionStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Order frequency (valores: "Diario", "Mensual", "Quincenal")',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'Frequency'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Order tolerance percentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'Tolerance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Corresponds to the type of node in TRUE (Line, Tank truck or station)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'ExpeditionClass'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Volume budget of the order',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ContractsInformation',
    @level2type = N'COLUMN',
    @level2name = N'EstimatedVolume'