/*-- ==============================================================================================================================
-- Author:			Microsoft  
-- Created Date:	Jan-22-2020
-- Updated Date:	Mar-20-2020
 <Description>:		This table holds the data for the Model Evaluation.  </Description>

-- ================================================================================================================================*/
CREATE TABLE [Analytics].[ModelEvaluation]
(
	--Columns
	[ModelEvaluationId]				INT IDENTITY (1, 1)			NOT NULL, 
	[OperationalDate]				DATE						NOT NULL,
	[TransferPoint]					NVARCHAR(200)				NOT NULL,
	[OwnershipPercentage]			DECIMAL(5, 2)				NOT NULL,
	[AlgorithmId]					INT							NOT NULL,
	[AlgorithmType]					NVARCHAR(200)				NOT NULL,
	[MeanAbsoluteError]				DECIMAL(5, 2)				NULL,
	[LoadDate]						DATETIME					NOT NULL,

	--Internal Common Columns 
	[CreatedBy]												NVARCHAR (260)						NOT NULL		DEFAULT 'Analytics',  
	[CreatedDate]											DATETIME							NOT NULL		DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]										NVARCHAR (260)						NULL, 
	[LastModifiedDate]										DATETIME							NULL,

	--Constraints
	CONSTRAINT [PK_ModelEvaluation]						PRIMARY KEY CLUSTERED ([ModelEvaluationId] ASC)
)



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date of loading the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ModelEvaluation',
    @level2type = N'COLUMN',
    @level2name = N'LoadDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ModelEvaluation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ModelEvaluation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ModelEvaluation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ModelEvaluation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the model evaluation',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ModelEvaluation',
    @level2type = N'COLUMN',
    @level2name = N'ModelEvaluationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The operational date of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ModelEvaluation',
    @level2type = N'COLUMN',
    @level2name = N'OperationalDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of transfer point',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ModelEvaluation',
    @level2type = N'COLUMN',
    @level2name = N'TransferPoint'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ModelEvaluation',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the algorithm',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ModelEvaluation',
    @level2type = N'COLUMN',
    @level2name = N'AlgorithmId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the type of the algorithm',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ModelEvaluation',
    @level2type = N'COLUMN',
    @level2name = N'AlgorithmType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the error absolute',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ModelEvaluation',
    @level2type = N'COLUMN',
    @level2name = N'MeanAbsoluteError'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for the Model Evaluation.',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'ModelEvaluation',
    @level2type = NULL,
    @level2name = NULL