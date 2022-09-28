/*==============================================================================================================================
--Author:        Microsoft
--Created date : Sep-29-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the information about Nodes and tagged elements with it.</Description>
================================================================================================================================*/
CREATE TABLE [Admin].[NodeTag]
(
	--Columns
    [NodeTagId]    	INT IDENTITY (1, 1) NOT NULL,
    [NodeId]         			INT NOT NULL,
    [ElementId]  				INT NOT NULL,
	[StartDate]					DATETIME NOT NULL,
    [EndDate]					DATETIME NOT NULL,
	--Internal Common Columns
	[CreatedBy]					NVARCHAR (260)   NOT NULL,
	[CreatedDate]				DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]			NVARCHAR (260)   NULL,
	[LastModifiedDate]			DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_NodeTag]					PRIMARY KEY CLUSTERED ([NodeTagId] ASC),
    CONSTRAINT [FK_NodeTag_Node]				FOREIGN KEY ([NodeId])			REFERENCES [Admin].[Node] ([NodeId]),
	CONSTRAINT [FK_NodeTag_CategoryElement]	FOREIGN KEY ([ElementId])		REFERENCES [Admin].[CategoryElement] ([ElementId])
);
GO

CREATE NONCLUSTERED INDEX NCI_ElementId_StartDate_EndDate
ON [Admin].[NodeTag] ([ElementId],[StartDate],[EndDate])
GO

CREATE NONCLUSTERED INDEX NCI_NodeTag_ElementId
ON [Admin].[NodeTag] ([ElementId])
GO

CREATE NONCLUSTERED INDEX NCI_NodeTag_NodeId_ElementId
ON [Admin].[NodeTag] ([NodeId])
INCLUDE ([ElementId])
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the association between element and node for a date range',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTag',
    @level2type = N'COLUMN',
    @level2name = N'NodeTagId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTag',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the category element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTag',
    @level2type = N'COLUMN',
    @level2name = N'ElementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The startdate of the node validity for the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTag',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The enddate of the node validity for the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTag',
    @level2type = N'COLUMN',
    @level2name = N'EndDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTag',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTag',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTag',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTag',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the information about Nodes and tagged elements with it.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTag',
    @level2type = NULL,
    @level2name = NULL