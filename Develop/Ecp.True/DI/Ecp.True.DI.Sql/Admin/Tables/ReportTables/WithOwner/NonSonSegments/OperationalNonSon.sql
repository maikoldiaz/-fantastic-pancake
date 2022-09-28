/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-06-2020
--<Description>: This table holds the data for summary before cutoff for Non Son segments. This table is being used in before cutoff report.</Description>
================================================================================================================================*/
CREATE TABLE [Admin].[OperationalNonSon]
(
	--Columns
     [OperationalNonSonId]             INT                                 NOT NULL IDENTITY(1,1),
	 [ProductID]						NVARCHAR (20)						NOT NULL,	
	 [ProductName]						NVARCHAR (150)						NOT NULL,
	 [OwnerName]						NVARCHAR (150)						NULL,  
	 [MeasurementUnit]					VARCHAR (300)						NULL,  
	 [SegmentID]						INT									NULL,	
	 [SegmentName]						NVARCHAR (150)						NULL,
	 [NodeId]							INT									NULL,
	 [CalculationDate]					DATE								NOT NULL,
	 [Inputs]							DECIMAL	 (18,2)						NULL,
	 [Outputs]							DECIMAL	 (18,2)						NULL,
	 [IdentifiedLosses]					DECIMAL	 (18,2)						NULL,
	 [IntialInventory]					DECIMAL	 (18,2)						NULL,
	 [FinalInventory]					DECIMAL	 (18,2)						NULL,
	 [Interface]						DECIMAL	 (18,2)						NULL, 
	 [Tolerance]						DECIMAL	 (18,2)						NULL, 
	 [Control]							DECIMAL	 (18,2)						NULL, 
	 [ExecutionId]						INT						            NOT NULL,
	
	 --Internal Common Columns													
	 [CreatedBy]						NVARCHAR (260)						NOT NULL,
	 [CreatedDate]						DATETIME							NOT NULL 

     	--Constraints
    CONSTRAINT [PK_OperationalNonSonId]	                PRIMARY KEY CLUSTERED (OperationalNonSonId ASC),
    CONSTRAINT [FK_OperationalNonSon_ReportExecution]	FOREIGN KEY ([ExecutionId])			REFERENCES [Admin].[ReportExecution] ([ExecutionId])
)
GO

ALTER TABLE [Admin].[OperationalNonSon]	SET (LOCK_ESCALATION = DISABLE)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The unique identifier of the OperationalNonSon table',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'OperationalNonSonId'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'ProductID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'ProductName'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the Owner Name',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'OwnerName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the Measurement Unit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the segment ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'SegmentID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the segment',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'SegmentName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is calculated',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the inputs',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Inputs'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the outputs',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Outputs'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the identified losses',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'IdentifiedLosses'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the initial inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'IntialInventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the final inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'FinalInventory'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the Interface',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Interface'
	
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the Tolerance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Tolerance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the unbalance',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'Control'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for summary before cutoff. This table is being used in before cutoff report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OperationalNonSon',
    @level2type = NULL,
    @level2name = NULL