/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jun-10-2020
--<Description>: This table holds the data node tag Related to Report Of System.</Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[NodeTagCalculationDateForSystemReport]
(
    NodeTagCalculationDateForSystemReportId     INT                                 NOT NULL IDENTITY(1,1),
	NodeId										INT									NULL,
	ElementId									INT									NULL,
	ElementName									NVARCHAR(150)						NULL,
	CalculationDate								DATE								NULL,
	ExecutionId									INT									NULL,
	
	 --Internal Common Columns													
	CreatedBy									NVARCHAR (260)						NULL,
	CreatedDate									DATETIME							NULL,

	--Constraints
    CONSTRAINT [PK_NodeTagCalculationDateForSystemReportId]	                PRIMARY KEY CLUSTERED (NodeTagCalculationDateForSystemReportId ASC),
    CONSTRAINT [FK_NodeTagCalculationDateForSystemReport_ReportExecution]	FOREIGN KEY ([ExecutionId])			REFERENCES [Admin].[ReportExecution] ([ExecutionId])
)
GO

ALTER TABLE [Admin].[NodeTagCalculationDateForSystemReport]   SET (LOCK_ESCALATION = DISABLE)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data node tag Related to Report Of System.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSystemReport',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSystemReport',
    @level2type = N'COLUMN',
    @level2name = N'NodeTagCalculationDateForSystemReportId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSystemReport',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the category element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSystemReport',
    @level2type = N'COLUMN',
    @level2name = N'ElementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the category element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSystemReport',
    @level2type = N'COLUMN',
    @level2name = N'ElementName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The calculation date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSystemReport',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The execution id unique to session received from UI',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSystemReport',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSystemReport',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSystemReport',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'