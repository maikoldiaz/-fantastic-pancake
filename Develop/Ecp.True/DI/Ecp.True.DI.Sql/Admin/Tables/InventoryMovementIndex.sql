/*==============================================================================================================================
--Author:        Microsoft
--Created date : Sep-08-2020
--<Description>: This table holds the hash data for inventory and movement registration. </Description>
==================================================================================================================================*/
CREATE TABLE [Admin].[InventoryMovementIndex]
(
	--Columns
    [InventoryMovementIndexId]		INT             IDENTITY (1, 1)  NOT NULL,
    [RecordHashColumn]			    NVARCHAR (150)						NOT NULL,
    

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)						NOT NULL,
	[CreatedDate]					DATETIME							NOT NULL    DEFAULT Admin.udf_GetTrueDate(),

	--Constraints
    CONSTRAINT [PK_InventoryMovementIndex]				    PRIMARY KEY CLUSTERED ([InventoryMovementIndexId] ASC),    
	CONSTRAINT [UQ_InventoryMovementIndex_RecordHashColumn] UNIQUE NONCLUSTERED ([RecordHashColumn])
);
GO


EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of inventory movement records.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryMovementIndex',
    @level2type = N'COLUMN',
    @level2name = N'InventoryMovementIndexId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'It contains the unique hash for the inventory and movement.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryMovementIndex',
    @level2type = N'COLUMN',
    @level2name = N'RecordHashColumn'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column).',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryMovementIndex',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column).',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryMovementIndex',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the hash data for inventory and movement registration.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'InventoryMovementIndex',
    @level2type = NULL,
    @level2name = NULL