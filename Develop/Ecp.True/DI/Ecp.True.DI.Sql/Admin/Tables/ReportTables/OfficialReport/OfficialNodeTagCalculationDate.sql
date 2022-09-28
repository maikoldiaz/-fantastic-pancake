/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jul-08-2020
-- Updated date: Aug-10-2020 -- Added identity column
--<Description>: This table holds the Official Node tags Related to Official Report (Report 12 & 13).</Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[OfficialNodeTagCalculationDate] 
(
	NodeId				INT									NULL,
	ElementId			INT									NULL,
	ElementName			NVARCHAR(150)						NULL,
	CalculationDate		DATE								NULL,
	[ExecutionId]		INT         						NOT NULL,
	
	 --Internal Common Columns													
	CreatedBy			NVARCHAR (260)						NULL,
	CreatedDate			DATETIME							NULL, 
    
    --Constraints
    CONSTRAINT [FK_OfficialNodeTagCalculationDate_ReportExecution]	FOREIGN KEY ([ExecutionId])			REFERENCES [Admin].[ReportExecution] ([ExecutionId]),

	--Indexes
    [OfficialNodeTagCalculationDateId] INT IDENTITY (1,1) CONSTRAINT PK_Official_NodeTag_CalculationDate_Id PRIMARY KEY CLUSTERED
)
GO

ALTER TABLE [Admin].[OfficialNodeTagCalculationDate]		  SET (LOCK_ESCALATION = DISABLE)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the official Node tags data Related to Official Report (Report 12 & 13).',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialNodeTagCalculationDate',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialNodeTagCalculationDate',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialNodeTagCalculationDate',
    @level2type = N'COLUMN',
    @level2name = N'ElementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialNodeTagCalculationDate',
    @level2type = N'COLUMN',
    @level2name = N'ElementName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The calculation date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialNodeTagCalculationDate',
    @level2type = N'COLUMN',
    @level2name = N'CalculationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The execution id unique to session received from UI',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialNodeTagCalculationDate',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialNodeTagCalculationDate',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialNodeTagCalculationDate',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialNodeTagCalculationDate',
    @level2type = N'COLUMN',
    @level2name = N'OfficialNodeTagCalculationDateId'