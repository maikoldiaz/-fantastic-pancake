/*-- ==============================================================================================================================
-- Author:			Microsoft  
-- Created Date:	Jan-31-2020
-- Updated Date:	Mar-30-2020 -- Added two extra columns (SourceSystem and ExecutionId)
 <Description>:		
                    This table holds the data for the Ownership Percentage Values.  
 </Description>
-- ================================================================================================================================*/
CREATE TABLE [Analytics].[OwnershipPercentageValues]
( 
	--Columns 
      [OwnershipPercentageValuesId]			INT				IDENTITY(1,1) NOT NULL,
      [OperationalDate]						DATE						  NOT NULL,
      [TransferPoint]						NVARCHAR(200)			      NOT NULL,
      [OwnershipPercentage]					DECIMAL(5, 2)				  NOT NULL,
	  [SourceSystem]                        NVARCHAR (200)                NOT NULL   DEFAULT 'CSV',
      [LoadDate]							DATETIME					  NOT NULL   DEFAULT admin.udf_GetTrueDate(),
	  [ExecutionID]			                UNIQUEIDENTIFIER              NULL, 

	--Internal Common Columns
	  [CreatedBy]							NVARCHAR (260)				  NOT NULL	 DEFAULT ('Analytics'), 
	  [CreatedDate]							DATETIME					  NOT NULL	 DEFAULT admin.udf_GetTrueDate(), 
	  [LastModifiedBy]						NVARCHAR (260)				  NULL, 
	  [LastModifiedDate]					DATETIME					  NULL,

	--Constraints
	CONSTRAINT [PK_OwnershipPercentageValues]	PRIMARY KEY CLUSTERED (OwnershipPercentageValuesId ASC)


)


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source system which record come from',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystem'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date of loading the record',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'LoadDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution of pipeline',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ownership percentage value',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentageValuesId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The operational date of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'OperationalDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the transfer point ',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'TransferPoint'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the ownership ',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for the Ownership Percentage Values. ',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'OwnershipPercentageValues',
    @level2type = NULL,
    @level2name = NULL