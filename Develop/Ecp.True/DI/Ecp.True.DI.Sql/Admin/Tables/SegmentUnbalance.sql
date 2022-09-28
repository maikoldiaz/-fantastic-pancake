-- ===========================================================================================================================================================
-- Author:          Microsoft
-- Created date:	Jan-4-2019
-- Updated date:	Mar-20-2020
-- <Description>:   This table holds the data for segment unbalance.</Description>												  
-- ============================================================================================================================================================
CREATE TABLE [Admin].[SegmentUnbalance] 
(
	--Columns
    [SegmentUnbalanceId]					INT IDENTITY(1,1)	NOT NULL,
    [SegmentId]								INT					NOT NULL,
    [ProductId]								NVARCHAR (20)       NOT NULL,
    [TicketId] 								INT 				NULL,	
	[InitialInventoryVolume] 				DECIMAL(18, 2) 		NULL,
	[FinalInventoryVolume] 					DECIMAL(18, 2) 		NULL,
	[InputVolume] 							DECIMAL(18, 2) 		NULL,
	[OutputVolume] 							DECIMAL(18, 2) 		NULL,
	[IdentifiedLossesVolume] 				DECIMAL(18, 2) 		NULL,
	[UnbalanceVolume] 						DECIMAL(18, 2) 		NULL,
	[InterfaceVolume] 						DECIMAL(18, 2) 		NULL,
	[ToleranceVolume] 						DECIMAL(18, 2) 		NULL,
	[UnidentifiedLossesVolume] 				DECIMAL(18, 2) 		NULL,
	[Date]									DATETIME			NOT NULL,

	--Internal Common Columns
	[CreatedBy]								NVARCHAR (260)		NOT NULL,
	[CreatedDate]							DATETIME			NOT NULL	DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]						NVARCHAR (260)		NULL,
	[LastModifiedDate]						DATETIME			NULL,

	--Constraints
    CONSTRAINT [PK_SegmentUnbalance]								PRIMARY KEY CLUSTERED ([SegmentUnbalanceId] ASC),
	CONSTRAINT [FK_SegmentUnbalance_CategoryElement_SegmentId]		FOREIGN KEY ([SegmentId])									REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_SegmentUnbalance_Product]						FOREIGN KEY ([ProductId])									REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [FK_SegmentUnbalance_Ticket]							FOREIGN KEY (TicketId)								REFERENCES [Admin].[Ticket]([TicketId])	
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the segment unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'SegmentUnbalanceId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the segment',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the initial inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'InitialInventoryVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the final inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'FinalInventoryVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the input volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'InputVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the output volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'OutputVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the identified losses volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'IdentifiedLossesVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the unbalance volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'UnbalanceVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the interface volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'InterfaceVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the tolerance volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'ToleranceVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the unidentified losses volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'UnidentifiedLossesVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date when the segment unbalance was generated',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = N'COLUMN',
    @level2name = N'Date'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for segment unbalance.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentUnbalance',
    @level2type = NULL,
    @level2name = NULL