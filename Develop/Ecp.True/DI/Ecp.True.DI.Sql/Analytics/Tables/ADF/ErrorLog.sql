/*-- ==============================================================================================================================
-- Author:			Microsoft  
-- Created Date:	Feb-04-2020
-- Updated Date:	Mar-20-2020
 <Description>:		This table holds the details for the Error Log (ADF).  </Description>

-- ================================================================================================================================*/
CREATE TABLE [Analytics].[ErrorLog]
(
	--Columns
	[ErrorId]       INT IDENTITY (1,1)		NOT NULL,
	[PipelineId]    INT						NOT NULL,
	[ErrorMsg]      VARCHAR (MAX)			NOT NULL,
	[ErrorDate]     DATETIME				NOT NULL,

	--Constraints
	 CONSTRAINT [PK_Analytics_ErrorLog]							PRIMARY KEY CLUSTERED ([ErrorId] ASC),
	 CONSTRAINT [FK_Analytics_ErrorLog_Pipeline]					FOREIGN KEY([PipelineId]) REFERENCES [Analytics].PipelineLog (PipelineId)
)


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the error',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ErrorLog',
    @level2type = N'COLUMN',
    @level2name = N'ErrorId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the pipeline',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ErrorLog',
    @level2type = N'COLUMN',
    @level2name = N'PipelineId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The message of the error ',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ErrorLog',
    @level2type = N'COLUMN',
    @level2name = N'ErrorMsg'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date when the error occurs',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ErrorLog',
    @level2type = N'COLUMN',
    @level2name = N'ErrorDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the details for the Error Log (ADF).',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ErrorLog',
    @level2type = NULL,
    @level2name = NULL