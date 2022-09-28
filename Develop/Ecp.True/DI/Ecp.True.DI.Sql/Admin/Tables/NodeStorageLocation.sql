/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-19-2020
--Updated date : Mar-30-2020
--Updated Date : Oct-05-2020  Adding indexes to improve the query performance
--<Description>: This table contains the association between nodes and storage locations. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[NodeStorageLocation] 
(
	--Columns
    [NodeStorageLocationId]             INT IDENTITY (1, 1)		NOT NULL,
    [Name]                              NVARCHAR (150)			NOT NULL,
    [Description]                       NVARCHAR (1000)			NULL,
    [StorageLocationTypeId]             INT						NOT NULL,
    [IsActive]                          BIT						NOT NULL		DEFAULT 1,
    [NodeId]                            INT						NOT NULL,
    [SendToSAP]                         BIT						NOT NULL,
    [StorageLocationId]                 NVARCHAR (20)			NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_StorageLocationsNode]					                PRIMARY KEY CLUSTERED ([NodeStorageLocationId] ASC),
    CONSTRAINT [FK_NodeStorageLocation_CategoryElement]		                FOREIGN KEY ([StorageLocationTypeId])					REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_NodeStorageLocation_Node]				                FOREIGN KEY ([NodeId])									REFERENCES [Admin].[Node] ([NodeId]),
    CONSTRAINT [FK_NodeStorageLocation_StorageLocation]		                FOREIGN KEY ([StorageLocationId])						REFERENCES [Admin].[StorageLocation] ([StorageLocationId])
);
GO

CREATE NONCLUSTERED INDEX [NIX_NodeStorageLocation_NodeId] 
ON [Admin].[NodeStorageLocation] (NodeId )
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for an association of node and storage location',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStorageLocation',
    @level2type = N'COLUMN',
    @level2name = N'NodeStorageLocationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name for the association between node and storage location',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStorageLocation',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStorageLocation',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the storage location (category element of storage location category)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStorageLocation',
    @level2type = N'COLUMN',
    @level2name = N'StorageLocationTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the association between node and storage location is active or not, 1 means active',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStorageLocation',
    @level2type = N'COLUMN',
    @level2name = N'IsActive'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStorageLocation',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if this should be sent to SAP or not , 1 means yes',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStorageLocation',
    @level2type = N'COLUMN',
    @level2name = N'SendToSAP'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of storage location',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStorageLocation',
    @level2type = N'COLUMN',
    @level2name = N'StorageLocationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStorageLocation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStorageLocation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStorageLocation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStorageLocation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table contains the association between nodes and storage locations.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeStorageLocation',
    @level2type = NULL,
    @level2name = NULL