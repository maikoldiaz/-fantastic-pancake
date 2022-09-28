/*==============================================================================================================================
--Author:        Microsoft
--Created Date : July-28-2020
--Updated Date : 
--<Description>: This table holds the data for delta nodes approval history.  </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[DeltaNodeApprovalHistory]
(
	[DeltaNodeApprovalHistoryId]	INT IDENTITY (1, 1)		   NOT NULL,
	[TicketId]						INT						   NOT NULL,
    [NodeId]					    INT						   NOT NULL,
    [Date]						    DATETIME				   NOT NULL,
	[Status]						INT				           NOT NULL,

	--Internal Common Columns
	[CreatedBy]											NVARCHAR (260)			NOT NULL,
	[CreatedDate]										DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]									NVARCHAR (260)			NULL,
	[LastModifiedDate]									DATETIME				NULL,

	--Constraints
	CONSTRAINT [PK_DeltaNodeApprovalHistory]	                PRIMARY KEY CLUSTERED ([DeltaNodeApprovalHistoryId] ASC),
	CONSTRAINT [FK_DeltaNodeApprovalHistory_Ticket]				FOREIGN KEY ([TicketId])					REFERENCES [Admin].[Ticket] ([TicketId]),
	CONSTRAINT [FK_DeltaNodeApprovalHistory_StatusType]	        FOREIGN KEY ([Status])					    REFERENCES [Admin].[OwnershipNodeStatusType]([OwnershipNodeStatusTypeId]),
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of delta node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApprovalHistory',
    @level2type = N'COLUMN',
    @level2name = N'DeltaNodeApprovalHistoryId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The approval date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApprovalHistory',
    @level2type = N'COLUMN',
    @level2name = N'Date'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApprovalHistory',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The status',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApprovalHistory',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApprovalHistory',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApprovalHistory',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApprovalHistory',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApprovalHistory',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApprovalHistory',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for delta nodes approval history.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApprovalHistory',
    @level2type = NULL,
    @level2name = NULL