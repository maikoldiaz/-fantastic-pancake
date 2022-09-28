/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jun-10-2020
--<Description>: This table holds the data node tag Related to Report Of Segment.</Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[NodeTagCalculationDateForSegmentReport] 
(
    NodeTagCalculationDateForSegmentReportId    INT                                 NOT NULL IDENTITY(1,1),
	NodeId										INT									NULL,
	ElementId									INT									NULL,
	ElementName									NVARCHAR(150)						NULL,
	CalculationDate								DATE								NULL,
	ExecutionId									INT									NULL,
	
	 --Internal Common Columns													
	CreatedBy									NVARCHAR (260)						NULL,
	CreatedDate									DATETIME							NULL,

	--Constraints
    CONSTRAINT [PK_NodeTagCalculationDateForSegmentReportId]	            PRIMARY KEY CLUSTERED ([NodeTagCalculationDateForSegmentReportId] ASC),
    CONSTRAINT [FK_NodeTagCalculationDateForSegmentReport_ReportExecution]	FOREIGN KEY ([ExecutionId])			REFERENCES [Admin].[ReportExecution] ([ExecutionId])
)
GO

ALTER TABLE [Admin].[NodeTagCalculationDateForSegmentReport]  SET (LOCK_ESCALATION = DISABLE)
--CREATE NONCLUSTERED INDEX [NCI_NodeTagCalculationDateForSegmentReport]
--ON Admin.NodeTagCalculationDateForSegmentReport([NodeId],[CalculationDate],[ElementId])
--INCLUDE([ExecutionId],[InputCategory],[InputElementName],[InputStartDate],[InputEndDate],[InputNodeName])

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data node tag Related to Report Of Segment.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSegmentReport',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'NodeTagCalculationDateForSegmentReportId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'ElementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'ElementName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The calculation date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The execution id unique to session received from UI',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTagCalculationDateForSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'