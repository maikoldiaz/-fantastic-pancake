/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Nov-11-2019
-- Updated Date:	Mar-20-2020
-- Updated Date:	Oct-05-2020  Adding indexes to improve the query performance
 <Description>:		This table holds the details for the Attributes.  </Description>

-- ================================================================================================================================*/
CREATE TABLE [Admin].[Attribute]
(
	--Columns
	[Id]							INT IDENTITY (1, 1)		NOT NULL,
	[AttributeId]					INT         			NOT NULL,
	[AttributeValue]				NVARCHAR (150)			NOT NULL,
	[ValueAttributeUnit]			INT         			NOT NULL,
	[AttributeDescription]			NVARCHAR (150)			NULL,
	[InventoryProductId]			INT						NULL,
	[MovementTransactionId]			INT						NULL,
	[AttributeType]					NVARCHAR(150)			NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
	CONSTRAINT [PK_Attribute]					PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Attribute_InventoryProduct]	FOREIGN KEY ([InventoryProductId])				REFERENCES [Offchain].[InventoryProduct] ([InventoryProductId]),
	CONSTRAINT [FK_Attribute_Movement]			FOREIGN KEY ([MovementTransactionId])			REFERENCES [Offchain].[Movement] ([MovementTransactionId]),
    CONSTRAINT [FK_Attribute_AttributeId]		FOREIGN KEY ([AttributeId])			            REFERENCES [Admin].[CategoryElement] ([ElementId]),
    CONSTRAINT [FK_Attribute_ValueAttributeUnit] FOREIGN KEY ([ValueAttributeUnit])			    REFERENCES [Admin].[CategoryElement] ([ElementId])
);
GO

CREATE NONCLUSTERED INDEX [NIX_Attribute_InventoryProductId] 
ON [Admin].[Attribute] (InventoryProductId)
GO

CREATE NONCLUSTERED INDEX [NIX_Attribute_MovementTransactionId] 
ON [Admin].[Attribute] (MovementTransactionId)
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Attribute',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the attribute (category element of attribute category)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Attribute',
    @level2type = N'COLUMN',
    @level2name = N'AttributeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the attribute in a unit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Attribute',
    @level2type = N'COLUMN',
    @level2name = N'AttributeValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the attribute unit (category element of attributeunit category, like Bbl)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Attribute',
    @level2type = N'COLUMN',
    @level2name = N'ValueAttributeUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description of the attribute',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Attribute',
    @level2type = N'COLUMN',
    @level2name = N'AttributeDescription'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the inventory product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Attribute',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement transaction',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Attribute',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the attribute (like General)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Attribute',
    @level2type = N'COLUMN',
    @level2name = N'AttributeType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Attribute',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Attribute',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Attribute',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Attribute',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the details for the Attributes.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Attribute',
    @level2type = NULL,
    @level2name = NULL