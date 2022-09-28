/*==============================================================================================================================
--Author:        Microsoft
--Created date : Nov-21-2019
--Updated date : Mar-20-2020
--Updated date : Jun-03-2020  -- Adding OwnershipAnalyticsStatus/ OwnershipAnalyticsErrorMessage columns as these are required capture the status
                                 when SP [Admin].[usp_SaveOperativeMovementsWithOwnershipPercentage] runs againest a ticket id.
--<Description>: This table contains current ownership status of that node. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[OwnershipNode]
(
	--Columns
	[OwnershipNodeId]				 INT IDENTITY(1,1)	         NOT NULL,
	[TicketId]						 INT				         NOT NULL,
	[NodeId]						 INT				         NOT NULL,
	[Status]						 INT				         NOT NULL,
	[OwnershipStatusId]				 INT				         NULL,
	[Editor]						 NVARCHAR(50)		         NULL,
	[ReasonId]						 INT				         NULL,
	[Comment]						 NVARCHAR(200)		         NULL,
	[EditorConnectionId]			 NVARCHAR(50)		         NULL,
	[ApproverAlias]					 NVARCHAR(50)		         NULL,
	[RegistrationDate]				 DATETIME			         NULL,
	[OwnershipAnalyticsStatus]       INT                         NULL, -- 1--Success , 0-- Failure
    [OwnershipAnalyticsErrorMessage] NVARCHAR(MAX)               NULL,

	--Internal Common Columns
	[CreatedBy]						 NVARCHAR (260)				NOT NULL,
	[CreatedDate]					 DATETIME					NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				 NVARCHAR (260)				NULL,
	[LastModifiedDate]				 DATETIME					NULL,

	--Constraints
	CONSTRAINT [PK_OwnershipNode]							PRIMARY KEY CLUSTERED ([OwnershipNodeId] ASC),
	CONSTRAINT [FK_OwnershipNode_Ticket]					FOREIGN KEY (TicketId)					REFERENCES [Admin].[Ticket]([TicketId]),
	CONSTRAINT [FK_OwnershipNode_Node]						FOREIGN KEY (NodeId)					REFERENCES [Admin].[Node]([NodeId]),
	CONSTRAINT [FK_OwnershipNode_StatusType]				FOREIGN KEY ([Status])					REFERENCES [Admin].[StatusType]([StatusTypeId]),
	CONSTRAINT [FK_OwnershipNode_OwnershipNodeStatusType]	FOREIGN KEY ([OwnershipStatusId])		REFERENCES [Admin].[OwnershipNodeStatusType]([OwnershipNodeStatusTypeId]),
	CONSTRAINT [FK_OwnershipNode_CategoryElement_Reason]	FOREIGN KEY ([ReasonId])				REFERENCES [Admin].[CategoryElement]([ElementId])
);



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ownership node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ticket ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The status of ownership node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ownership status',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipStatusId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the reason (category element of reason category)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'ReasonId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The comment provided by the user',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'Comment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the editor',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'Editor'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SignalR Hub ConnectionId (used in Balance operativo con propiedad por nodo for ConcurrentEdit)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'EditorConnectionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The approver alias',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'ApproverAlias'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is registered',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'RegistrationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The error message when the ticket is processed and failed for Ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipAnalyticsStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The error message when the ticket is processed and failed for Ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipAnalyticsErrorMessage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table contains current ownership status of that node.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNode',
    @level2type = NULL,
    @level2name = NULL