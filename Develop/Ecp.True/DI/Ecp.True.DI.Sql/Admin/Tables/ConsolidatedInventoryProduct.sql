/*==============================================================================================================================
--Author:        Microsoft
--Created Date : July-06-2020
--Updated Date : 
--<Description>: This table holds the data for consolidated inventories.  </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[ConsolidatedInventoryProduct]
(
	--Columns
	[ConsolidatedInventoryProductId]	INT IDENTITY (1, 1)		NOT NULL,
	[NodeId]					        INT						NOT NULL,
	[ProductId]				            NVARCHAR (20)			NOT NULL,
	[InventoryDate]					    DATETIME		        NOT NULL,
	[ProductVolume]				        DECIMAL(18, 2)			NOT NULL,
	[GrossStandardQuantity]			    DECIMAL(18, 2)		    NULL,
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
    CONSTRAINT [PK_ConsolidatedInventoryProduct]						    PRIMARY KEY CLUSTERED ([ConsolidatedInventoryProductId] ASC),
    CONSTRAINT [FK_ConsolidatedInventoryProduct_Node]					    FOREIGN KEY ([NodeId])						REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_ConsolidatedInventoryProduct_Product]				    FOREIGN KEY ([ProductId])					REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [FK_ConsolidatedInventoryProduct_Ticket]		                FOREIGN KEY ([TicketId])			        REFERENCES [Admin].[Ticket]([TicketId]),
	CONSTRAINT [FK_ConsolidatedInventoryProduct_Segment]		            FOREIGN KEY ([SegmentId])    		        REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_ConsolidatedInventoryProduct_SourceSystem]		        FOREIGN KEY ([SourceSystemId])			    REFERENCES [Admin].[CategoryElement] ([ElementId]),
)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Consolidated Inventory Product Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'ConsolidatedInventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Node Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Product Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Inventory Date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'InventoryDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Product Volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'ProductVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Gross Standard Quantity',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Measurement Unit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Ticket Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Segment Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Source System Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystemId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Execution Date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The column to determine active records.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'IsActive'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for consolidated inventories.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedInventoryProduct',
    @level2type = NULL,
    @level2name = NULL