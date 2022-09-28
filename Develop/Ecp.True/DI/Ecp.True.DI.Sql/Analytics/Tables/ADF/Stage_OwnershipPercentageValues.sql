/*-- ==============================================================================================================================
-- Author:			Microsoft  
-- Created Date:	Mar-30-2020
<Description>:		
                    This stage table holds the CSV data for the Ownership Percentage Values.  
</Description>
-- ================================================================================================================================*/
CREATE TABLE [Analytics].[Stage_OwnershipPercentageValues]
( 
	--Columns 
      [OperationalDate]						DATE						  NOT NULL,
      [TransferPoint]						NVARCHAR(200)			      NOT NULL,
      [OwnershipPercentage]					DECIMAL(5, 2)				  NOT NULL,
	  [SourceSystem]                        NVARCHAR (200)                NOT NULL   DEFAULT 'CSV',
      [LoadDate]							DATETIME					  NOT NULL   DEFAULT admin.udf_GetTrueDate(),
	  [ExecutionID]			                UNIQUEIDENTIFIER              NULL, 
)


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution of pipeline',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date of loading the record',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'LoadDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source system which record come from',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystem'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the percentage of the ownership ',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the transfer point ',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'TransferPoint'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The operational date of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OwnershipPercentageValues',
    @level2type = N'COLUMN',
    @level2name = N'OperationalDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This stage table holds the CSV data for the Ownership Percentage Values. ',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OwnershipPercentageValues',
    @level2type = NULL,
    @level2name = NULL