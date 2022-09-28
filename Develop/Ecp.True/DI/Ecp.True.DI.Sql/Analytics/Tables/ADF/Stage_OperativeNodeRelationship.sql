/*-- ==============================================================================================================================
-- Author:			Microsoft  
-- Created Date:	Jul-16-2019
 <Description>:		This table holds the data for the OperativeNode Relationship.  </Description>

-- ================================================================================================================================*/
CREATE TABLE [Analytics].[Stage_OperativeNodeRelationship] 
( 
     --Columns 

	[TransferPoint]								NVARCHAR(200)					NOT NULL, 
	[SourceField]								NVARCHAR(200)					NOT NULL, 
	[FieldWaterProduction]						NVARCHAR(200)					NOT NULL, 
	[RelatedSourceField]						NVARCHAR(200)					NOT NULL, 
	[DestinationNode]							NVARCHAR(200)					NOT NULL, 
	[DestinationNodeType]						NVARCHAR(200)					NOT NULL, 
	[MovementType]								NVARCHAR(200)					NOT NULL, 
	[SourceNode]								NVARCHAR(200)					NOT NULL, 
	[SourceNodeType]							NVARCHAR(200)					NOT NULL, 
	[SourceProduct]								NVARCHAR(200)					NOT NULL, 
	[SourceProductType]							NVARCHAR(200)					NOT NULL, 
	[SourceSystem]								NVARCHAR(200)					NOT NULL		DEFAULT 'CSV', 
	[LoadDate]									DATETIME						NOT NULL		DEFAULT Admin.udf_GetTrueDate(), 
	[Notes]										NVARCHAR(200)					NULL,
	[IsDeleted]									BIT								NULL			DEFAULT 0,
	
	--Internal Common Columns 

	[CreatedBy]									NVARCHAR (260)					NOT NULL		DEFAULT 'ADF',  
	[CreatedDate]								DATETIME						NOT NULL		DEFAULT Admin.udf_GetTrueDate(), 
	[LastModifiedBy]							NVARCHAR (260)					NULL, 
	[LastModifiedDate]							DATETIME						NULL,
	
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the transfer point',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'TransferPoint'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source field',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'SourceField'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the field of water of production ',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'FieldWaterProduction'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source field related',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'RelatedSourceField'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the type of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the type of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'MovementType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'SourceNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the type of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'SourceProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the type of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source system which record come from',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystem'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date of loading the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'LoadDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the record is deleted or not, 1 means delete',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'IsDeleted'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Additional record information',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'Notes'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for the OperativeNode Relationship.',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationship',
    @level2type = NULL,
    @level2name = NULL