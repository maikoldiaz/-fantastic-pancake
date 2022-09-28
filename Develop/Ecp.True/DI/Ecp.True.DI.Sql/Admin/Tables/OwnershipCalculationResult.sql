/*==============================================================================================================================
--Author:        Microsoft
--Created date : Feb-03-2020
--Updated date : Mar-20-2020
--<Description>: This table holds the data for result of the ownership calculation. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[OwnershipCalculationResult]
(
	--Columns
	[OwnershipCalculationResultId]	INT IDENTITY (1, 1)	        NOT NULL,
	[OwnershipCalculationId]	    INT						    NOT NULL,
	[ControlTypeId]				    INT						    NULL,
	[OwnerId]					    INT						    NULL,
	[OwnershipPercentage]		    DECIMAL(5, 2)		    	NULL,
	[OwnershipVolume]			    DECIMAL(18, 2)			    NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)				NOT NULL,
	[CreatedDate]					DATETIME					NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)				NULL,
	[LastModifiedDate]				DATETIME					NULL,

	--Constraints
	CONSTRAINT [PK_OwnershipCalculationResult]					PRIMARY KEY CLUSTERED ([OwnershipCalculationResultId] ASC),
	CONSTRAINT [FK_OwnershipCalculationResult_OwnershipCalculation]		FOREIGN KEY (OwnershipCalculationId)	REFERENCES [Admin].[OwnershipCalculation]([OwnershipCalculationId]),
	CONSTRAINT [FK_OwnershipCalculationResult_CategoryElement]	FOREIGN KEY (OwnerId)					REFERENCES [Admin].[CategoryElement]([ElementId]),
	CONSTRAINT [FK_OwnershipCalculationResult_Type]				FOREIGN KEY (ControlTypeId)				REFERENCES [Admin].[ControlType]([ControlTypeId])
)



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ownership calculation result',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculationResult',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipCalculationResultId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ownership calculation',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculationResult',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipCalculationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the control type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculationResult',
    @level2type = N'COLUMN',
    @level2name = N'ControlTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the owner ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculationResult',
    @level2type = N'COLUMN',
    @level2name = N'OwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculationResult',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the volume of the ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculationResult',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculationResult',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculationResult',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculationResult',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculationResult',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for result of the ownership calculation.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipCalculationResult',
    @level2type = NULL,
    @level2name = NULL