/*-- ==============================================================================================================================
-- Author:			Microsoft  
-- Created Date:	Jul-16-2019
 <Description>:		This table holds the data for the OperativeNode Relationship With Ownership.  </Description>

-- ================================================================================================================================*/
CREATE TABLE [Analytics].[Stage_OperativeNodeRelationshipWithOwnership] 
( 
     --Columns 

	[SourceProduct]											NVARCHAR(200)				NOT NULL, 
	[LogisticSourceCenter]									NVARCHAR(200)				NOT NULL,
	[DestinationProduct]									NVARCHAR(200)				NOT NULL, 
	[LogisticDestinationCenter]								NVARCHAR(200)				NOT NULL,
	[TransferPoint]											NVARCHAR(200)				NOT NULL,
	[IsDeleted]												BIT							NOT NULL		DEFAULT 0,
	[Notes]													NVARCHAR(1000)				NULL, 
	[SourceSystem]											NVARCHAR(200)				NOT NULL		DEFAULT 'CSV', 
	[LoadDate]												DATETIME					NOT NULL		DEFAULT Admin.udf_GetTrueDate(), 
	
	--Internal Common Columns 

	[CreatedBy]												NVARCHAR (260)				NOT NULL		DEFAULT 'ADF',  
	[CreatedDate]											DATETIME					NOT NULL		DEFAULT Admin.udf_GetTrueDate(), 
	[LastModifiedBy]										NVARCHAR (260)				NULL, 
	[LastModifiedDate]										DATETIME					NULL,

)




GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationshipWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'SourceProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the logistic source center ',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationshipWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'LogisticSourceCenter'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationshipWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the logistic destination center',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationshipWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'LogisticDestinationCenter'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the transfer point',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationshipWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'TransferPoint'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the record is deleted or not, 1 means delete',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationshipWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'IsDeleted'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Additional record information',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationshipWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'Notes'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source system which record come from',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationshipWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystem'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date of loading the record',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationshipWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'LoadDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationshipWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationshipWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationshipWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationshipWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for the OperativeNode Relationship With Ownership.',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeNodeRelationshipWithOwnership',
    @level2type = NULL,
    @level2name = NULL