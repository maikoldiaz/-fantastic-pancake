/*==============================================================================================================================
--Author:        Microsoft
--Created Date : July-06-2020
--Updated Date : 
--<Description>: This table holds the data for consolidated movements.  </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[ConsolidatedMovement]
(
	--Columns
	[ConsolidatedMovementId]			INT IDENTITY (1, 1)		NOT NULL,
	[SourceNodeId]					    INT						NULL,
	[SourceProductId]				    NVARCHAR (20)			NULL,
	[DestinationNodeId]					INT						NULL,
	[DestinationProductId]				NVARCHAR (20)			NULL,
	[MovementTypeId]				    NVARCHAR (150)			NOT NULL,
	[StartDate]					        DATETIME		        NOT NULL,
	[EndDate]					        DATETIME		        NOT NULL,
	[NetStandardVolume]				    DECIMAL(18, 2)			NOT NULL,
	[GrossStandardVolume]			    DECIMAL(18, 2)		    NULL,
	[MeasurementUnit]				    NVARCHAR (50)			NOT NULL,
	[TicketId]						    INT				        NOT NULL,
	[SegmentId]							INT				        NOT NULL,
    [SourceSystemId]                    INT                     NOT NULL,
	[ExecutionDate]						DATETIME			    NOT NULL,
    [IsActive]						    BIT			            NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)  NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)  NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_ConsolidatedMovement]						        PRIMARY KEY CLUSTERED ([ConsolidatedMovementId] ASC),
    CONSTRAINT [FK_ConsolidatedMovement_SourceNode]					    FOREIGN KEY ([SourceNodeId])						REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_ConsolidatedMovement_SourceProduct]				    FOREIGN KEY ([SourceProductId])						REFERENCES [Admin].[Product] ([ProductId]),
    CONSTRAINT [FK_ConsolidatedMovement_DestinationNode]		        FOREIGN KEY ([DestinationNodeId])					REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_ConsolidatedMovement_DestinationProduct]				FOREIGN KEY ([DestinationProductId])				REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [FK_ConsolidatedMovement_Ticket]		                    FOREIGN KEY ([TicketId])			                REFERENCES [Admin].[Ticket]([TicketId]),
	CONSTRAINT [FK_ConsolidatedMovement_Segment]		                FOREIGN KEY ([SegmentId])    		                REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_ConsolidatedMovement_SourceSystem]		            FOREIGN KEY ([SourceSystemId])			            REFERENCES [Admin].[CategoryElement] ([ElementId]),
)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Consolidated Movement Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'ConsolidatedMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Source Node Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Source Product Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Destination Node Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Destination Product Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Movement Type Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'MovementTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Start Date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The End Date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'EndDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Net Standard Volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Gross Standard Volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Measurement Unit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Ticket Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Segment Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Source System Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystemId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Execution Date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The column to determine active records.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'IsActive'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for consolidated movements.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedMovement',
    @level2type = NULL,
    @level2name = NULL