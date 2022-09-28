/*==============================================================================================================================
--Author:        Microsoft
--Created Date : Dec-12-2019
--Updated Date : Mar-20-2020
--<Description>: This table holds the data for events of planning, programming and collaboration agreements.  </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[Event]
(
	--Columns
	[EventId]					INT             IDENTITY (101, 1)  NOT NULL,
	[EventTypeId]				INT				NOT NULL,
	[SourceNodeId]				INT				NOT NULL,
	[DestinationNodeId]			INT				NOT NULL,
	[SourceProductId]			NVARCHAR(20)	NOT NULL,
	[DestinationProductId]		NVARCHAR(20)	NOT NULL,
	[StartDate]					DATETIME		NOT NULL,
	[EndDate]					DATETIME		NOT NULL,
	[Owner1Id]					INT				NOT NULL	DEFAULT		124,  -- Owner OTROS
	[Owner2Id]					INT				NOT NULL	DEFAULT		124,  -- Owner OTROS
	[Volume]					DECIMAL(18, 2)	NOT NULL,
	[MeasurementUnit]			NVARCHAR(50)	NOT NULL,
	[IsDeleted]					BIT	NULL		DEFAULT 0, -- 1 - Deleted

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_Event]								PRIMARY KEY CLUSTERED ([EventId] ASC),
	CONSTRAINT [FK_Event_EventType]						FOREIGN KEY ([EventTypeId])						REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_Event_Product1]						FOREIGN KEY ([SourceProductId])					REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [FK_Event_Product2]						FOREIGN KEY ([DestinationProductId])			REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [FK_Event_Node1]							FOREIGN KEY ([SourceNodeId])					REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_Event_Node2]							FOREIGN KEY ([DestinationNodeId])				REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_Event_CategoryElement_Owner1Id]		FOREIGN KEY ([Owner1Id])						REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_Event_CategoryElement_Owner2Id]		FOREIGN KEY ([Owner2Id])						REFERENCES [Admin].[CategoryElement] ([ElementId])
)



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the event',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'EventId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the category element of event category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'EventTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The start date of the event',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The end date of the event ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'EndDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the first owner (category element of owner category like for Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'Owner1Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the second owner (category element of owner category like for Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'Owner2Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'Volume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier or name of the measurement unit (category element of unit category) like for Bbl(Barriles)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the element is deleted or not, 1 means delete',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'IsDeleted'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for events of planning, programming and collaboration agreements.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Event',
    @level2type = NULL,
    @level2name = NULL