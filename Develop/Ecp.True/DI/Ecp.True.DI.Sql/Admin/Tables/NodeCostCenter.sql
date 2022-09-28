/*==============================================================================================================================
--Author:        InterGrupo
--Created Date : Mar-18-2021
--Updated Date : Mar-19-2021
--<Description>: This table holds the node relation, type of movement by cost center.  </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[NodeCostCenter]
(
	--Columns
	[NodeCostCenterId]			                INT IDENTITY (1, 1)  NOT NULL,	
	[SourceNodeId]				                INT				     NOT NULL,
	[DestinationNodeId]			                INT				     NULL,
	[MovementTypeId]			                INT	                 NOT NULL,
	[CostCenterId]				                INT				     NOT NULL,
	[IsActive]					                BIT	NULL		DEFAULT 1, -- 0 - Inactive
	[IsDeleted]					                BIT	NULL		DEFAULT 0, -- 1 - Deleted
    [RowVersion]					            ROWVERSION,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_NodeCostCenter]		                PRIMARY KEY CLUSTERED ([NodeCostCenterId] ASC),
    CONSTRAINT [FK_NodeCostCenter_Node1]	            FOREIGN KEY ([SourceNodeId])         REFERENCES [Admin].[Node] ([NodeId]),
    CONSTRAINT [FK_NodeCostCenter_Node2]	            FOREIGN KEY ([DestinationNodeId])    REFERENCES [Admin].[Node] ([NodeId]),
    CONSTRAINT [FK_NodeCostCenter_CategoryElement1]	    FOREIGN KEY ([MovementTypeId])       REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_NodeCenter_CategoryElement2]	        FOREIGN KEY ([CostCenterId])         REFERENCES [Admin].[CategoryElement] ([ElementId]),
)



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node relation, type of movement by cost center',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeCostCenter',
    @level2type = N'COLUMN',
    @level2name = 'NodeCostCenterId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeCostCenter',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeCostCenter',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the type of movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeCostCenter',
    @level2type = N'COLUMN',
    @level2name = N'MovementTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the cost center',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeCostCenter',
    @level2type = N'COLUMN',
    @level2name = N'CostCenterId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the element is active or not, 1 means active',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeCostCenter',
    @level2type = N'COLUMN',
    @level2name = N'IsActive'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the element is deleted or not, 1 means delete',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeCostCenter',
    @level2type = N'COLUMN',
    @level2name = N'IsDeleted'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeCostCenter',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeCostCenter',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeCostCenter',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeCostCenter',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version column used for consistency',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeCostCenter',
    @level2type = N'COLUMN',
    @level2name = N'RowVersion'