/*===========================================================================================================================================================
-- Author:          Microsoft
-- Created date:	Oct-16-2019
-- Updated date:	Mar-20-2020
--<Description>: This table holds the details for unbalance verification before generating cutoff ticket or before starting cutoff.  </Description>
=============================================================================================================================================================
*/
CREATE TABLE [Admin].[UnbalanceComment]
(
	--Columns
	[UnbalanceId]					INT IDENTITY (1, 1)		   NOT NULL,	
    [TicketId]						INT						   NULL,
	[NodeId]						INT						   NOT NULL,
	[ProductId]						NVARCHAR (20)			   NOT NULL,
	[Unbalance]						DECIMAL(18, 2)			   NOT NULL,
	[Units]							NVARCHAR(50)			   NOT NULL,
	[UnbalancePercentage]			DECIMAL(18, 2)			   NOT NULL,
	[ControlLimit]					DECIMAL(18, 2)			   NOT NULL,
	[Comment]						NVARCHAR(1000)			   NULL,
	[Status]						INT						   NOT NULL,	-- 1 = In progress; 0 = Processed
	[CalculationDate]				DATETIME				   NOT NULL		DEFAULT Admin.udf_GetTrueDate(),
    [SessionId]                     NVARCHAR(50)               NULL,
    [SegmentId]                     INT                        NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),

	--Constraints
    CONSTRAINT [PK_UnbalanceComment]	PRIMARY KEY CLUSTERED ([UnbalanceId] ASC),
    CONSTRAINT [FK_UnbalanceComment_Ticket_TicketId]		FOREIGN KEY ([TicketId])			REFERENCES [Admin].[Ticket]([TicketId]),
	CONSTRAINT [FK_UnbalanceComment_Node_NodeId]			FOREIGN KEY ([NodeId])				REFERENCES [Admin].[Node]([NodeId]),
	CONSTRAINT [FK_UnbalanceComment_Product_ProductId]		FOREIGN KEY ([ProductId])			REFERENCES [Admin].[Product]([ProductId]),
    CONSTRAINT [Fk_UnbalanceComment_Segment_SegmentId]      FOREIGN KEY ([SegmentId])           REFERENCES [Admin].[CategoryElement] ([ElementId])
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the unbalance comment',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'UnbalanceId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'Unbalance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the units',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'Units'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'UnbalancePercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the control limit ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'ControlLimit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The comment of the result of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'Comment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The status (1 means inprogress , 0 means processed)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date when the unbalance was calculated',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the details for unbalance verification before generating cutoff ticket or before starting cutoff. ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The session identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'SessionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The segment identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'