/*==============================================================================================================================
--Author:        Microsoft
--Created Date : July-06-2020
--Updated Date : July-30-2020 -- add
--<Description>: This table holds the data for delta nodes.  </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[DeltaNode]
(
	[DeltaNodeId]					INT IDENTITY (1, 1)		   NOT NULL,
	[TicketId]						INT						   NOT NULL,
    [NodeId]						INT						   NOT NULL,
	[Status]						INT				           NOT NULL,
    [LastApprovedDate]              DATETIME                   NULL,
    [Comment]                       NVARCHAR (1000)            NULL,
	[Editor]						NVARCHAR (50)		       NULL,
	[Approvers]					    NVARCHAR (1000)		       NULL,

	--Internal Common Columns
	[CreatedBy]											NVARCHAR (260)			NOT NULL,
	[CreatedDate]										DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]									NVARCHAR (260)			NULL,
	[LastModifiedDate]									DATETIME				NULL,

	--Constraints
	CONSTRAINT [PK_DeltaNode]						PRIMARY KEY CLUSTERED ([DeltaNodeId] ASC),
    CONSTRAINT [FK_DeltaNode_Node]					FOREIGN KEY ([NodeId])						REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_DeltaNode_Ticket]				FOREIGN KEY ([TicketId])					REFERENCES [Admin].[Ticket] ([TicketId]),
	CONSTRAINT [FK_DeltaNode_StatusType]	        FOREIGN KEY ([Status])					    REFERENCES [Admin].[OwnershipNodeStatusType]([OwnershipNodeStatusTypeId]),
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of delta node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNode',
    @level2type = N'COLUMN',
    @level2name = N'DeltaNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNode',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNode',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The status',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNode',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The status',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNode',
    @level2type = N'COLUMN',
    @level2name = N'LastApprovedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The status',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNode',
    @level2type = N'COLUMN',
    @level2name = N'Comment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The status',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNode',
    @level2type = N'COLUMN',
    @level2name = N'Editor'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The status',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNode',
    @level2type = N'COLUMN',
    @level2name = N'Approvers'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNode',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNode',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNode',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNode',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for delta nodes.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNode',
    @level2type = NULL,
    @level2name = NULL