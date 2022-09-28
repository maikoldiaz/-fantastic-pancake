/*===========================================================================================================================================================
-- Author:          Microsoft
-- Created date:	Oct-11-2019
-- Updated date:	Mar-20-2020
-- Updated date:	Jun-03-2020  -- Removing OwnershipAnalyticsStatus/ OwnershipAnalyticsErrorMessage columns as these are not required in this table.
--<Description>: This table holds the data for Tickets generated in the system.</Description>

=============================================================================================================================================================*/
CREATE TABLE [Admin].[Ticket]
(
	--Columns
	[TicketId]						    INT		IDENTITY (23678, 1) NOT NULL,	
    [CategoryElementId]				    INT						    NOT NULL,
	[StartDate]						    DATETIME				    NOT NULL,	
	[EndDate]						    DATETIME					NOT NULL,
	[Status]						    INT							NOT NULL,
	[TicketTypeId]					    INT							NULL		DEFAULT 1,--1 - Cutoff, 2 - Ownership
	[TicketGroupId]					    NVARCHAR(255)				NULL,
	[ErrorMessage]					    NVARCHAR(MAX)				NULL,
	[OwnerId]						    INT							NULL,
	[NodeId]						    INT							NULL,
    [ScenarioTypeId]						INT							NULL,
	[BlobPath]						    NVARCHAR(MAX)				NULL, 
	[AnalyticsStatus]                   INT                         NULL, -- 0--Success , 1-- Failure
    [AnalyticsErrorMessage]             NVARCHAR(MAX)               NULL,
    
	--Internal Common Columns
	[CreatedBy]						NVARCHAR(260)				NOT NULL,
	[CreatedDate]					DATETIME					NOT NULL   DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR(260)				NULL,
	[LastModifiedDate]				DATETIME					NULL,

	--Constraints
    CONSTRAINT [PK_Ticket]												PRIMARY KEY CLUSTERED ([TicketId] ASC),
	CONSTRAINT [FK_Ticket_StatusType]									FOREIGN KEY ([Status])				REFERENCES [Admin].[StatusType]([StatusTypeId]),
	CONSTRAINT [FK_Ticket_TicketType]									FOREIGN KEY (TicketTypeId)			REFERENCES [Admin].[TicketType]([TicketTypeId]),
    CONSTRAINT [FK_Ticket_CategoryElement_CategoryElementId]			FOREIGN KEY (CategoryElementId)		REFERENCES [Admin].[CategoryElement]([ElementId]),
	CONSTRAINT [FK_Ticket_CategoryElement_OwnerId]						FOREIGN KEY ([OwnerId])				REFERENCES [Admin].[CategoryElement]([ElementId]),
	CONSTRAINT [FK_Ticket_Node_NodeId]									FOREIGN KEY ([NodeId])				REFERENCES [Admin].[Node]([NodeId]),
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the category element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'CategoryElementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the ticket is started',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the ticket is ended',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'EndDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the status of the ticket , 1 means Ok, 2 means failed',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ticket type (Logistics, Ownership, cutoff)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'TicketTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ticket group',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'TicketGroupId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The error message when the ticket is processed and failed',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'ErrorMessage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the owner (category element of owner category)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'OwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the status of the ticket , 0 means success, 1 means failed',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'AnalyticsStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The error message when the ticket is processed and failed',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'AnalyticsErrorMessage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The blobpath',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = N'BlobPath'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for Tickets generated in the system.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for the scenario type ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'COLUMN',
    @level2name = 'ScenarioTypeId'