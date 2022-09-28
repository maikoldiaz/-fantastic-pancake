/*==============================================================================================================================
--Author:        Intergrupo
--Created date : Jun-16-2021
--<Description>: This table holds the data for Node states with official delta. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[NodeOficialStatus]
(
	SegmentName nvarchar(150),
	SystemName nvarchar(150),
	NodeName  nvarchar(150),
	statusNode nvarchar(150),
	StatusDateChange datetime,
	Approver nvarchar(150),
	Comment nvarchar(256),
	ExecutionId nvarchar(150) NOT NULL,
	CreatedBy nvarchar(20) NOT NULL,
	CreatedDate datetime NOT NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the segment',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeOficialStatus',
    @level2type = N'COLUMN',
    @level2name = N'SegmentName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeOficialStatus',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeOficialStatus',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the status node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeOficialStatus',
    @level2type = N'COLUMN',
    @level2name = N'statusNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the status changed',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeOficialStatus',
    @level2type = N'COLUMN',
    @level2name = N'StatusDateChange'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the approver',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeOficialStatus',
    @level2type = N'COLUMN',
    @level2name = N'Approver'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The comment provided by the user',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeOficialStatus',
    @level2type = N'COLUMN',
    @level2name = N'Comment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The execution id unique to session received from UI',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeOficialStatus',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeOficialStatus',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeOficialStatus',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'