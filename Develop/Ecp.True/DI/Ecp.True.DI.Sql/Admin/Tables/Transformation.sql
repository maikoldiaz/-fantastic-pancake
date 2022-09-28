/*===========================================================================================================================================================
-- Author:          Microsoft
-- Created date:	Nov-21-2019
-- Updated date:	Mar-20-2020
--<Description>: This table holds the details for Transformation from source to destination over a message type. </Description>
=============================================================================================================================================================*/
CREATE TABLE [Admin].[Transformation]
(
	-- Columns
	[TransformationId]							INT IDENTITY (1, 1) NOT NULL,
	[MessageTypeId]								INT NOT NULL,
	[OriginSourceNodeId]						INT NOT NULL,
	[OriginDestinationNodeId]					INT NULL,
	[OriginSourceProductId]						NVARCHAR (20) NOT NULL,
	[OriginDestinationProductId]				NVARCHAR (20) NULL,
	[OriginMeasurementId]						INT NOT NULL,

	[DestinationSourceNodeId]					INT NOT NULL,
	[DestinationDestinationNodeId]				INT NULL,
	[DestinationSourceProductId]				NVARCHAR (20) NOT NULL,
	[DestinationDestinationProductId]			NVARCHAR (20) NULL,
	[DestinationMeasurementId]					INT NOT NULL,
	[IsDeleted]									BIT	NULL		DEFAULT 0, -- 1 - Deleted
	[RowVersion]					ROWVERSION,
	
		--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_Transformation]								PRIMARY KEY CLUSTERED ([TransformationId] ASC),
	CONSTRAINT [PK_Transformation_MessageType]					FOREIGN KEY (MessageTypeId) REFERENCES [Admin].[MessageType]([MessageTypeId]),
	CONSTRAINT [FK_Transformation_Node1]						FOREIGN KEY ([OriginSourceNodeId])					REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_Transformation_Node2]						FOREIGN KEY ([OriginDestinationNodeId])				REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_Transformation_StorageLocationProduct1]		FOREIGN KEY ([OriginSourceProductId])				REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [FK_Transformation_StorageLocationProduct2]		FOREIGN KEY ([OriginDestinationProductId])			REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [FK_Transformation_CategoryElement1]				FOREIGN KEY ([OriginMeasurementId])					REFERENCES [Admin].[CategoryElement] ([ElementId]),

    CONSTRAINT [FK_Transformation_Node3]						FOREIGN KEY ([DestinationSourceNodeId])				REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_Transformation_Node4]						FOREIGN KEY ([DestinationDestinationNodeId])		REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_Transformation_StorageLocationProduct3]		FOREIGN KEY ([DestinationSourceProductId])			REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [FK_Transformation_StorageLocationProduct4]		FOREIGN KEY ([DestinationDestinationProductId])		REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [FK_Transformation_CategoryElement2]				FOREIGN KEY ([DestinationMeasurementId])			REFERENCES [Admin].[CategoryElement] ([ElementId]),
)


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the transformation',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'TransformationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the message type (like Movement, Inventory, Event, etc.)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'MessageTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the original source node ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'OriginSourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the original destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'OriginDestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the original source product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'OriginSourceProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the original destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'OriginDestinationProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the original measurement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'OriginMeasurementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationSourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationDestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination source product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationSourceProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationDestinationProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination measurement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationMeasurementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the record is delete or not, 1 means delete',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'IsDeleted'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'RowVersion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the details for Transformation from source to destination over a message type.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Transformation',
    @level2type = NULL,
    @level2name = NULL