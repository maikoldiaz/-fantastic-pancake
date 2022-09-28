/*==========================================================================================================
--Author:        Microsoft
--Created date : Jul-30-2020	
--Updated date : Aut-09-2020
--<Description>: This table holds the status for reports on the fly calculation. </Description>
============================================================================================================*/
CREATE TABLE [Admin].[ReportExecution]
(
    --Columns
	[ExecutionId]		            INT IDENTITY (1, 1)	        NOT NULL,
    [CategoryId]                    INT                             NULL,
	[ElementId]						INT				                NULL,
    [NodeId]						INT						        NULL,
    [StartDate]                     DATE                            NOT NULL,
    [EndDate]                       DATE                            NOT NULL,
	[StatusTypeId]			        INT					            NOT NULL,
    [ReportTypeId]                  INT                             NOT NULL,
    [ScenarioId]                    INT                             NOT NULL,
    [OwnerId]                       INT                             NULL,
    [Name]						    NVARCHAR(200)			        NULL,
    [Hash]                          NVARCHAR(256)                   NULL,

    --Internal Common Columns
	[CreatedBy]						NVARCHAR (260)			        NOT NULL,
	[CreatedDate]					DATETIME				        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)			        NULL,
	[LastModifiedDate]				DATETIME				        NULL,

	--Constraints
    CONSTRAINT [PK_ReportExecution]				                    PRIMARY KEY CLUSTERED ([ExecutionId] ASC),
	CONSTRAINT [FK_ReportExecution_Category]		                FOREIGN KEY ([CategoryId])    		        REFERENCES [Admin].[Category] ([CategoryId]),
	CONSTRAINT [FK_ReportExecution_Element]		                    FOREIGN KEY ([ElementId])    		        REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_ReportExecution_Node]		                    FOREIGN KEY ([NodeId])						REFERENCES [Admin].[Node] ([NodeId]),
)
GO
CREATE NONCLUSTERED INDEX NCI_ReportExecution_CategoryId
ON [Admin].[ReportExecution] ([CategoryId]);
GO
CREATE NONCLUSTERED INDEX NCI_ReportExecution_ElementId
ON [Admin].[ReportExecution] ([ElementId]);
GO
CREATE NONCLUSTERED INDEX NCI_ReportExecution_NodeId
ON [Admin].[ReportExecution] ([NodeId]);
GO
CREATE NONCLUSTERED INDEX NCI_ReportExecution_StatusTypeId
ON [Admin].[ReportExecution] ([StatusTypeId]);
GO
CREATE NONCLUSTERED INDEX NCI_ReportExecution_ReportTypeId
ON [Admin].[ReportExecution] ([ReportTypeId]);
GO
CREATE NONCLUSTERED INDEX NCI_ReportExecution_ScenarioId
ON [Admin].[ReportExecution] ([ScenarioId]);
GO
CREATE NONCLUSTERED INDEX NCI_ReportExecution_StartDate
ON [Admin].[ReportExecution] ([StartDate]);
GO
CREATE NONCLUSTERED INDEX NCI_ReportExecution_OwnerId
ON [Admin].[ReportExecution] ([OwnerId]);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The unique identifier for execution (sent from User Interface)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportExecution',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of status type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportExecution',
    @level2type = N'COLUMN',
    @level2name = N'StatusTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of report type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportExecution',
    @level2type = N'COLUMN',
    @level2name = N'ReportTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of scenario',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportExecution',
    @level2type = N'COLUMN',
    @level2name = N'ScenarioId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the status for official balance report on the fly calculation.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportExecution',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportExecution',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportExecution',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportExecution',
    @level2type = N'COLUMN',
    @level2name = N'CategoryId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of node.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportExecution',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of element.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportExecution',
    @level2type = N'COLUMN',
    @level2name = N'ElementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportExecution',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The start date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportExecution',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The end date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportExecution',
    @level2type = N'COLUMN',
    @level2name = N'EndDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportExecution',
    @level2type = N'COLUMN',
    @level2name = 'OwnerId'