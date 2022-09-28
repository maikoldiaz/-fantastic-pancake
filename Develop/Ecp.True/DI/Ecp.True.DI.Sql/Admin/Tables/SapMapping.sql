CREATE TABLE [Admin].[SapMapping]
(
	[SapMappingId]							INT IDENTITY (1, 1)		   NOT NULL,
    [OfficialSystem]                        INT               		   NOT NULL,     
	[SourceSystemId]					    INT						   NOT NULL,
	[SourceMovementTypeId]				    INT						   NOT NULL,
	[SourceProductId]				        NVARCHAR(20)			   NOT NULL,
	[SourceSystemSourceNodeId]			    INT						   NOT NULL,
	[SourceSystemDestinationNodeId]		    INT						   NOT NULL,
	[DestinationSystemId]				    INT						   NOT NULL,
	[DestinationMovementTypeId]		    	INT						   NOT NULL,
	[DestinationProductId]				    NVARCHAR(20)			   NOT NULL,
	[DestinationSystemSourceNodeId]		    INT						   NOT NULL,
	[DestinationSystemDestinationNodeId]    INT				           NOT NULL,

    --Internal Common Columns
	[CreatedBy]						NVARCHAR (260)			   NOT NULL,
	[CreatedDate]					DATETIME				   NOT NULL     DEFAULT Admin.udf_GetTrueDate(),

	--Constraints
	CONSTRAINT [PK_SapMapping]				   PRIMARY KEY CLUSTERED ([SapMappingId] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of SAP Node record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = N'COLUMN',
    @level2name = N'SapMappingId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of source system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystemId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of source movement type (category element of movement type category)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = N'COLUMN',
    @level2name = N'SourceMovementTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of source product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of source node of source system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystemSourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of destination node of source system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystemDestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of destination system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = N'COLUMN',
    @level2name = N'DestinationSystemId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of destination movement type (category element of movement type category)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = N'COLUMN',
    @level2name = N'DestinationMovementTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of source node of destination system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = N'COLUMN',
    @level2name = N'DestinationSystemSourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of destination node of destination system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = N'COLUMN',
    @level2name = N'DestinationSystemDestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The official system name',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = N'COLUMN',
    @level2name = N'OfficialSystem'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for Sap Mapping information.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMapping',
    @level2type = NULL,
    @level2name = NULL