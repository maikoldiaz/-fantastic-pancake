-- ===========================================================================================================================================================
-- Author:          Microsoft
-- Create date:		Nov-11-2019
-- ModifiedDate:    Jan-03-2020 ( Added FK on OwnerID )
-- <Description>:   This table holds the data for collect and calculate FICO ownership calculation response, used in Balance operativo con propiedad por nodo page. </Description>													  
-- ============================================================================================================================================================
CREATE TABLE [Admin].[OwnershipCalculation] 
(
	--Columns
	[OwnershipCalculationId]				INT IDENTITY(1,1)	NOT NULL,
	[NodeId] 								INT 				NOT NULL,
	[ProductId] 							NVARCHAR(20) 		NOT NULL,
    [MeasurementUnit] 						INT          		NULL,
	[OwnershipTicketId] 					INT 				NULL,
	[OwnerId] 								INT 				NULL,
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
	[InterfaceUnbalanceVolume] 				DECIMAL(18, 2) 		NULL,
	[InterfaceUnbalancePercentage] 			DECIMAL(5, 2) 		NULL,
	[ToleranceUnbalanceVolume] 				DECIMAL(18, 2) 		NULL,
	[ToleranceUnbalancePercentage] 			DECIMAL(5, 2) 		NULL,
	[IdentifiedLossesUnbalanceVolume] 		DECIMAL(18, 2) 		NULL,
	[IdentifiedLossesUnbalancePercentage]	DECIMAL(5, 2) 		NULL,
	[Date]									DATETIME			NULL,

	--Internal Common Columns
	[CreatedBy]								NVARCHAR (260)		NOT NULL,
	[CreatedDate]							DATETIME			NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]						NVARCHAR (260)		NULL,
	[LastModifiedDate]						DATETIME			NULL,

	--Constraints
    CONSTRAINT [PK_OwnershipCalculation]					PRIMARY KEY CLUSTERED ([OwnershipCalculationId] ASC),
	CONSTRAINT [FK_OwnershipCalculation_Node]				FOREIGN KEY (NodeId)									REFERENCES [Admin].[Node]([NodeId]),
	CONSTRAINT [FK_OwnershipCalculation_Product]			FOREIGN KEY (ProductId)									REFERENCES [Admin].[Product]([ProductId]),
	CONSTRAINT [FK_OwnershipCalculation_Ticket]				FOREIGN KEY (OwnershipTicketId)							REFERENCES [Admin].[Ticket]([TicketId]),
	CONSTRAINT [FK_OwnershipCalculation_CategoryElement]	FOREIGN KEY (OwnerId)									REFERENCES [Admin].[CategoryElement] ([ElementId])
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ownership calculation',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipCalculationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ownership ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipTicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'OwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volumen of the initial inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'InitialInventoryVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the initial inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'InitialInventoryPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volumen of the final inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'FinalInventoryVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the final inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'FinalInventoryPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of  the volume of the input',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'InputVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the input',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'InputPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the output',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'OutputVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the output',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'OutputPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the identified losses',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'IdentifiedLossesVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the identified losses',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'IdentifiedLossesPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'UnbalanceVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'UnbalancePercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the interface',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'InterfaceVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the interface',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'InterfacePercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the tolerance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'ToleranceVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the tolerance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'TolerancePercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the unidentified losses',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'UnidentifiedLossesVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the unidentified losses',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'UnidentifiedLossesPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the interface unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'InterfaceUnbalanceVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the interface unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'InterfaceUnbalancePercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the tolerance unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'ToleranceUnbalanceVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the tolerance unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'ToleranceUnbalancePercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the identified losses',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'IdentifiedLossesUnbalanceVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the identified losses',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'IdentifiedLossesUnbalancePercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is calculated',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'Date'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for collect and calculate FICO ownership calculation response, used in Balance operativo con propiedad por nodo page.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculation',
    @level2type = NULL,
    @level2name = NULL