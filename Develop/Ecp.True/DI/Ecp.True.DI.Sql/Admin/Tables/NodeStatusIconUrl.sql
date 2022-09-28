/*=========================================================================================================================================================================================
--Author:        Microsoft
--Created date : Apr-02-2020
--Updated date : 
--<Description>: This table is created to store urls for icons, representing node statuses by different colors for a ticket, to display on Power Bi report
--.This is a seeded table, different for every environment</Description>
=========================================================================================================================================================================================*/
CREATE TABLE [admin].[NodeStatusIconUrl]
(
	[NodeStatusIconUrlId]		INT IDENTITY(1,1)	NOT NULL,
	[NodeStatusIconUrl]			NVARCHAR(2038)		NOT NULL,
	
	--Internal Common Columns
	[CreatedBy]					NVARCHAR (260)		NOT NULL,
	[CreatedDate]				DATETIME			NOT NULL    DEFAULT Admin.udf_GetTrueDate(),

	--Constraints
    CONSTRAINT [PK_NodeStatusIconUrl]	PRIMARY KEY CLUSTERED ([NodeStatusIconUrlId] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for status icons used in the report',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStatusIconUrl',
    @level2type = N'COLUMN',
    @level2name = N'NodeStatusIconUrlId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The icon url (deployed in UI) ',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStatusIconUrl',
    @level2type = N'COLUMN',
    @level2name = N'NodeStatusIconUrl'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStatusIconUrl',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStatusIconUrl',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table is created to store urls for icons, representing node statuses by different colors for a ticket, to display on Power Bi report. This is a seeded table, different for every environment.',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStatusIconUrl',
    @level2type = NULL,
    @level2name = NULL