/*-- ==============================================================================================================================
-- Author:			Microsoft  
-- Created Date:	Mar-09-2020
-- Updated Date:	Mar-20-2020
 <Description>:		This table holds the data for the Operative Movements With Ownership from CSV.  </Description>

-- ================================================================================================================================*/
CREATE TABLE [Analytics].[Stage_OperativeMovementsWithOwnership] 
( 
	--Columns 

	[OperationalDate]								DATE						NOT NULL, 
	[MovementType]									NVARCHAR(200)				NOT NULL, 
	[SourceProduct]									NVARCHAR(200)				NOT NULL, 
	[SourceStorageLocation]							NVARCHAR(200)				NOT NULL, 
	[DestinationProduct]							NVARCHAR(200)				NOT NULL, 
	[DestinationStorageLocation]					NVARCHAR(200)				NOT NULL, 
	[OwnershipVolume]								DECIMAL(18, 2)				NOT NULL, 
	[TransferPoint]									NVARCHAR(200)				NOT NULL, 
	[Month]											INT							NOT NULL, 
	[Year]											INT							NOT NULL, 
	[DayOfMonth]									INT							NOT NULL, 
	[SourceSystem]									NVARCHAR(200)				NOT NULL		DEFAULT 'CSV', 
	[LoadDate]										DATETIME					NOT NULL		DEFAULT Admin.udf_GetTrueDate(), 
	[ExecutionID]									UNIQUEIDENTIFIER			NULL, 

	--Internal Common Columns 

	[CreatedBy]										NVARCHAR (260)				NOT NULL		DEFAULT 'ADF',  
	[CreatedDate]									DATETIME					NOT NULL		DEFAULT Admin.udf_GetTrueDate(), 
	[LastModifiedBy]								NVARCHAR (260)				NULL, 
	[LastModifiedDate]								DATETIME					NULL
) 


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source system which record come from',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystem'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date of loading the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'LoadDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution of pipeline',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The day of the month of the operational date',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'Month'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The year of the operational date',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'Year'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The day of the month of the operational date',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'DayOfMonth'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the transfer point',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'TransferPoint'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The volume of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the location of the destination storage',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'DestinationStorageLocation'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the location of the source storage',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'SourceStorageLocation'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'SourceProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the type of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'MovementType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The operational date of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = N'COLUMN',
    @level2name = N'OperationalDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for the Operative Movements With Ownership from CSV.',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'TABLE',
    @level1name = N'Stage_OperativeMovementsWithOwnership',
    @level2type = NULL,
    @level2name = NULL