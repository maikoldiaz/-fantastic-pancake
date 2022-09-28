/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Dec-19-2019
-- Updated Date:	Mar-20-2020
 <Description>:		This table holds the data for the Pipeline Log (ADF).  </Description>

-- ================================================================================================================================*/
CREATE TABLE [Audit].[PipelineLog]
(
	[PipelineId]			INT IDENTITY(1,1)	NOT NULL,
	[PipelineRunId]			VARCHAR(100)		NOT NULL,
	[PipelineName]			VARCHAR(100)		NOT NULL,
	[PipelineStatusId]		INT					NOT NULL,
	[PipelineStartTime]		DATETIME			NOT NULL,
	[PipelineEndTime]		DATETIME			NULL,

	--Constraints
	CONSTRAINT [PK_PipelineId]						PRIMARY KEY CLUSTERED ([PipelineId] ASC),
	CONSTRAINT [FK_PipelineLog_AuditStatus]			FOREIGN KEY([PipelineStatusId]) REFERENCES [Audit].AuditStatus (StatusId)
)



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the pipeline',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'PipelineLog',
    @level2type = N'COLUMN',
    @level2name = N'PipelineId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution of pipeline',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'PipelineLog',
    @level2type = N'COLUMN',
    @level2name = N'PipelineRunId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the pipeline',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'PipelineLog',
    @level2type = N'COLUMN',
    @level2name = N'PipelineName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The id of the status of the pipeline',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'PipelineLog',
    @level2type = N'COLUMN',
    @level2name = N'PipelineStatusId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the pipeline is started',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'PipelineLog',
    @level2type = N'COLUMN',
    @level2name = N'PipelineStartTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the pipeline is ended',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'PipelineLog',
    @level2type = N'COLUMN',
    @level2name = N'PipelineEndTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for the Pipeline Log (ADF).',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'PipelineLog',
    @level2type = NULL,
    @level2name = NULL