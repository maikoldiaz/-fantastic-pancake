-- ===========================================================================================================================================================
-- Author:          Microsoft
-- Create date:		Jan-2-2020
-- Modified date:	Jan-3-2020 ( Added 2 columns OwnerId and OwnershipTicketId )
--					Mar-20-2020
-- <Description>:   This table holds the data for segment ownership calculation. </Description>													  
-- ============================================================================================================================================================
CREATE TABLE [Admin].[SegmentOwnershipCalculation] 
(
	--Columns
    [SegmentOwnershipCalculationId]			INT IDENTITY(1,1)	NOT NULL,
    [SegmentId]								INT					NOT NULL,
    [ProductId]								NVARCHAR (20)       NOT NULL,
    [MeasurementUnit] 						INT          		NULL,
    [OwnerId] 								INT 				NULL,
    [OwnershipTicketId] 					INT 				NULL,	
	[InitialInventoryVolume] 				DECIMAL(18, 2) 		NULL,
	[InitialInventoryPercentage] 			DECIMAL(5, 2) 		NULL,
	[FinalInventoryVolume] 					DECIMAL(18, 2) 		NULL,
	[FinalInventoryPercentage] 				DECIMAL(5, 2) 		NULL,
	[InputVolume] 							DECIMAL(18, 2) 		NULL,
	[InputPercentage] 						DECIMAL(5, 2) 		NULL,
	[OutputVolume] 							DECIMAL(18, 2) 		NULL,
	[OutputPercentage] 						DECIMAL(5, 2) 		NULL,
	[IdentifiedLossesVolume] 				DECIMAL(18, 2) 		NULL,
	[IdentifiedLossesPercentage] 			DECIMAL(5, 2) 		NULL,
	[UnbalanceVolume] 						DECIMAL(18, 2) 		NULL,
	[UnbalancePercentage] 					DECIMAL(5, 2) 		NULL,
	[InterfaceVolume] 						DECIMAL(18, 2) 		NULL,
	[InterfacePercentage] 					DECIMAL(5, 2) 		NULL,
	[ToleranceVolume] 						DECIMAL(18, 2) 		NULL,
	[TolerancePercentage] 					DECIMAL(5, 2) 		NULL,
	[UnidentifiedLossesVolume] 				DECIMAL(18, 2) 		NULL,
	[UnidentifiedLossesPercentage] 			DECIMAL(5, 2) 		NULL,
	[Date]									DATETIME			NOT NULL,

	--Internal Common Columns
	[CreatedBy]								NVARCHAR (260)		NOT NULL,
	[CreatedDate]							DATETIME			NOT NULL	DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]						NVARCHAR (260)		NULL,
	[LastModifiedDate]						DATETIME			NULL,

	--Constraints
    CONSTRAINT [PK_SegmentOwnershipCalculation]								PRIMARY KEY CLUSTERED ([SegmentOwnershipCalculationId] ASC),
	CONSTRAINT [FK_SegmentOwnershipCalculation_CategoryElement_SegmentId]	FOREIGN KEY ([SegmentId])									REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_SegmentOwnershipCalculation_Product]						FOREIGN KEY ([ProductId])									REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [FK_SegmentOwnershipCalculation_CategoryElement_OwnerId]		FOREIGN KEY (OwnerId)										REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_SegmentOwnershipCalculation_Ticket]						FOREIGN KEY (OwnershipTicketId)								REFERENCES [Admin].[Ticket]([TicketId])	
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the segment ownership calculation',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'SegmentOwnershipCalculationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the segment',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'OwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ownership ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipTicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the initial inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'InitialInventoryVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the initial inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'InitialInventoryPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the final inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'FinalInventoryVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the final inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'FinalInventoryPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the input',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'InputVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the input',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'InputPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the output',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'OutputVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the output',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'OutputPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the identified losses',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'IdentifiedLossesVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the identified losses',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'IdentifiedLossesPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'UnbalanceVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'UnbalancePercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the interface',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'InterfaceVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the interface',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'InterfacePercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of  the tolerance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'ToleranceVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the tolerance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'TolerancePercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the unidentified losses',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'UnidentifiedLossesVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the unidentified losses ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'UnidentifiedLossesPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the segment ownership calculation was done',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'Date'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for segment ownership calculation.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SegmentOwnershipCalculation',
    @level2type = NULL,
    @level2name = NULL