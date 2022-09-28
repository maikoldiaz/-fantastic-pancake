/*==============================================================================================================================
--Author:        InterGrupo
--Created Date : Mar-23-2021
--Updated Date : Mar-24-2021
--<Description>: This table holds the ticket relation with the nodes.  </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[TicketNode]
(
	--Columns
	[TicketNodeId]			                INT IDENTITY (1, 1)  NOT NULL,	
	[TicketId]				                INT				     NOT NULL,
	[NodeId]			                    INT				     NOT NULL,		

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_TicketNode]		                    PRIMARY KEY NONCLUSTERED ([TicketNodeId] ASC),
    CONSTRAINT [FK_TicketNode_Ticket_TicketId]	        FOREIGN KEY ([TicketId])                        REFERENCES [Admin].[ticket] ([TicketId]),
    CONSTRAINT [FK_TicketNode_Node_NodeId]	            FOREIGN KEY ([NodeId])                          REFERENCES [Admin].[Node] ([NodeId]),
    CONSTRAINT [UC_TicketNode]					        UNIQUE CLUSTERED ([TicketId],[NodeId] ASC)
)


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ticket relation with the nodes',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNode',
    @level2type = N'COLUMN',
    @level2name = 'TicketNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNode',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNode',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNode',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNode',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNode',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNode',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
