/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-09-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data for movement type relationships. </Description>
==================================================================================================================================*/
CREATE TABLE [Admin].[Annulation]
(
	--Columns
    [AnnulationId]	                INT          IDENTITY (1, 1)        NOT NULL,
    [SourceMovementTypeId]          INT                                 NOT NULL,
    [AnnulationMovementTypeId]      INT                                 NOT NULL,
    [SourceNodeId]                  INT                                 NOT NULL,
    [DestinationNodeId]             INT                                 NOT NULL,
    [SourceProductId]               INT                                 NOT NULL,    
    [DestinationProductId]          INT                                 NOT NULL,
    [SapTransactionCodeId]             INT                                 NULL,
    [IsActive]						BIT			                        NOT NULL    DEFAULT 1,
	[RowVersion]					ROWVERSION,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)						NOT NULL,
	[CreatedDate]					DATETIME							NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)						NULL,
	[LastModifiedDate]				DATETIME							NULL,

	--Constraints
    CONSTRAINT [PK_Annulation]				                                    PRIMARY KEY CLUSTERED ([AnnulationId] ASC),
    CONSTRAINT [FK_Annulation_CategoryElement_SourceMovementTypeId]		        FOREIGN KEY ([SourceMovementTypeId])			REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_Annulation_CategoryElement_AnnulationMovementTypeId]		    FOREIGN KEY ([AnnulationMovementTypeId])		REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_Annulation_Node_SourceNodeId]		                        FOREIGN KEY ([SourceNodeId])			        REFERENCES [Admin].[OriginType] ([OriginTypeId]),
	CONSTRAINT [FK_Annulation_Node_DestinationNodeId]		                    FOREIGN KEY ([DestinationNodeId])			    REFERENCES [Admin].[OriginType] ([OriginTypeId]),
    CONSTRAINT [FK_Annulation_Product_SourceProductId]		                    FOREIGN KEY ([SourceProductId])			        REFERENCES [Admin].[OriginType] ([OriginTypeId]),
	CONSTRAINT [FK_Annulation_Product_DestinationProductId]		                FOREIGN KEY ([DestinationProductId])			REFERENCES [Admin].[OriginType] ([OriginTypeId]),
    CONSTRAINT [UQ_Annulation_SourceMovementTypeId_ReversalMovementTypeId]      UNIQUE NONCLUSTERED ([SourceMovementTypeId], [AnnulationMovementTypeId]),
    CONSTRAINT [FK_Annulation_CategoryElement_TransactionCodeId]		        FOREIGN KEY ([SapTransactionCodeId])		REFERENCES [Admin].[CategoryElement] ([ElementId]),
);
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for movement type relationships.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the relationship',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = N'COLUMN',
    @level2name = N'AnnulationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = N'COLUMN',
    @level2name = N'SourceMovementTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the reversed movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = N'COLUMN',
    @level2name = N'AnnulationMovementTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version column used for consistency',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = N'COLUMN',
    @level2name = N'RowVersion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the element is active or not, 1 means active',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = N'COLUMN',
    @level2name = N'IsActive'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Transformation according to the type of Movement Sap',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Annulation',
    @level2type = N'COLUMN',
    @level2name = 'SapTransactionCodeId'