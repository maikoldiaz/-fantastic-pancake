/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-30-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data for Node Connections between source and destination Nodes.</Description>
================================================================================================================================*/
CREATE TABLE [Admin].[NodeConnection]
(
	--Columns
	[NodeConnectionId]				INT IDENTITY (1, 1) NOT NULL,
	[SourceNodeId]					INT					NOT NULL,
	[DestinationNodeId]				INT					NOT NULL,
	[Description]					NVARCHAR (300)		NULL,
	[ControlLimit]					DECIMAL(18, 2)		NULL,
	[AlgorithmId]					INT					NULL,
	[IsActive]						BIT					NOT NULL	DEFAULT 1,
	[IsDeleted]						BIT					NOT NULL	DEFAULT 0,		--> 1=Deleted
	[IsTransfer]					BIT					NOT NULL	DEFAULT 0,		--> 1=Transfer
	[RowVersion]					ROWVERSION,
	
	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
	CONSTRAINT [PK_NodeConnection]			PRIMARY KEY CLUSTERED ([NodeConnectionId] ASC),
	CONSTRAINT [FK_NodeConnection_Node_SourceNodeId]	FOREIGN KEY ([SourceNodeId])											REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_NodeConnection_Node_DestinationNodeId]	FOREIGN KEY ([DestinationNodeId])										REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_NodeConnection_Algorithm]	FOREIGN KEY ([AlgorithmId])				REFERENCES [Admin].[Algorithm] ([AlgorithmId])
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for connection between source and destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = N'COLUMN',
    @level2name = N'NodeConnectionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description of the connection',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The control limit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = N'COLUMN',
    @level2name = N'ControlLimit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of algorithm for analytical model',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = N'COLUMN',
    @level2name = N'AlgorithmId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the connection is active or not, 1 means active',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = N'COLUMN',
    @level2name = N'IsActive'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the connection is deleted, 1 means deleted',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = N'COLUMN',
    @level2name = N'IsDeleted'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version column used for consistency',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = N'COLUMN',
    @level2name = N'RowVersion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for Node Connections between source and destination Nodes.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the connection is a transfer point, 1 means yes. Transfer point ownership calculated through analyticsAPI',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnection',
    @level2type = N'COLUMN',
    @level2name = N'IsTransfer'