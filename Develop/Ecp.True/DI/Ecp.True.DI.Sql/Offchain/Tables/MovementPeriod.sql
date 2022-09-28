/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Nov-11-2019
-- Updated Date:	Mar-20-2020
-- Updated Date:	Oct-05-2020  Adding indexes to improve the query performance
 <Description>:		This table holds the details for the Movement Period.  </Description>

-- ================================================================================================================================*/
CREATE TABLE [Offchain].[MovementPeriod]
(
	--Columns
	[MovementPeriodId]				INT IDENTITY (1, 1)		NOT NULL,
	[MovementTransactionId]			INT						NOT NULL,
	[StartTime]						DATETIME				NOT NULL,
	[EndTime]						DATETIME				NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
	CONSTRAINT [PK_MovementPeriod]				PRIMARY KEY CLUSTERED ([MovementPeriodId] ASC),
	CONSTRAINT [FK_MovementPeriod_Movement]		FOREIGN KEY ([MovementTransactionId]) REFERENCES [Offchain].[Movement] ([MovementTransactionId]),
	CONSTRAINT [UC_MovementPeriod_Movement]		UNIQUE ([MovementTransactionId])
);


GO

CREATE NONCLUSTERED INDEX NCIX_MovPeriod_MovTranId 
ON [Offchain].[MovementPeriod] (MovementTransactionId)
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement period',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementPeriod',
    @level2type = N'COLUMN',
    @level2name = N'MovementPeriodId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement transaction ',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementPeriod',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the movement is started',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementPeriod',
    @level2type = N'COLUMN',
    @level2name = N'StartTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the movement is ended',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementPeriod',
    @level2type = N'COLUMN',
    @level2name = N'EndTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementPeriod',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementPeriod',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementPeriod',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementPeriod',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the details for the Movement Period.',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = N'TABLE',
    @level1name = N'MovementPeriod',
    @level2type = NULL,
    @level2name = NULL