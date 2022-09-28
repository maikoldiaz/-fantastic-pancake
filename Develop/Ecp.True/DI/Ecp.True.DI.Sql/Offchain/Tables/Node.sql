/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jun-16-2020
--Updated date : Jun-16-2020
--<Description>: This table holds blockchain related information about the Node created in the system. </Description>
================================================================================================================================*/
CREATE TABLE [Offchain].[Node] 
(
	--Columns
    [Id]				                INT IDENTITY (1, 1) NOT NULL,
	[NodeId]			                INT                 NOT NULL,
    [Name]                              VARCHAR(150)        NOT NULL,
    [NodeStateTypeId]                   INT                 NOT NULL,
    [LastUpdateDate]                    DATETIME            NOT NULL,
    [IsActive]                          BIT                 NOT NULL,

    [TransactionHash]	                NVARCHAR(255)	    NULL,
	[BlockNumber]		                NVARCHAR(255)	    NULL,
    [BlockchainStatus]	                INT                 NOT NULL,
    [RetryCount]		                INT                 NOT NULL DEFAULT 0,

    --Internal Common Columns
    [CreatedBy]                         NVARCHAR (260)  NOT NULL,
    [CreatedDate]                       DATETIME        NOT NULL DEFAULT Admin.udf_GetTrueDate(),
    [LastModifiedBy]	                NVARCHAR(260)   NULL,
	[LastModifiedDate]	                DATETIME	    NULL,

	--Constraints
    CONSTRAINT [PK_Node]		        PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Node_Node]	        FOREIGN KEY ([NodeId])	        REFERENCES [Admin].[Node] ([NodeId]),
    CONSTRAINT [FK_Node_NodeStateType]	FOREIGN KEY ([NodeStateTypeId])	REFERENCES [Offchain].[NodeStateType] ([NodeStateTypeId])
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds blockchain related information about the Node created in the system.',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The business identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The status of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'IsActive'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The last update date of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'LastUpdateDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The state of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'NodeStateTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The blockchain transaction hash for node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'TransactionHash'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The blockchain block number for node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'BlockNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The blockchain status for node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'BlockchainStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The blockchain retry count for node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'RetryCount'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'Node',
    @level2type = N'COLUMN',
    @level2name = N'Id'