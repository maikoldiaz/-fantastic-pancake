/*==============================================================================================================================
--Author:        Microsoft
--Created date : Sep-19-2019
--Updated date : Mar-20-2020
--Updated date : Jul-8-2020 -- Set default value of priority from 10 to 1 for PBI 31873
--<Description>: This table holds the details for associations of products with the connections of nodes. </Description>
===============================================================================================================================*/
CREATE TABLE [Admin].[NodeConnectionProduct]
(
	--Columns
	[NodeConnectionProductId]		INT IDENTITY (1, 1) NOT NULL,
	[NodeConnectionId]				INT					NOT NULL,
	[ProductId]						NVARCHAR (20)		NOT NULL,
	[UncertaintyPercentage]			DECIMAL(5, 2)		NULL,
	[IsDeleted]						BIT					NOT NULL	DEFAULT(0),		--> 1=Deleted
	[Priority]						INT					NULL	    DEFAULT(1),
	[NodeConnectionProductRuleId]	INT					NULL,
	[RowVersion]					ROWVERSION,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)		NOT NULL,
	[CreatedDate]					DATETIME			NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)		NULL,
	[LastModifiedDate]				DATETIME			NULL,

	--Constraints
	CONSTRAINT [PK_NodeConnectionProduct]								PRIMARY KEY CLUSTERED		([NodeConnectionProductId] ASC),
	CONSTRAINT [FK_NodeConnectionProduct_NodeConnection]				FOREIGN KEY					([NodeConnectionId])				REFERENCES [Admin].[NodeConnection] ([NodeConnectionId]),
	CONSTRAINT [FK_NodeConnectionProduct_Product]						FOREIGN KEY					([ProductId])						REFERENCES [Admin].[Product] ([ProductId]),
	CONSTRAINT [UC_NodeConnectionId_ProductId]							UNIQUE NONCLUSTERED			([NodeConnectionId], [ProductId]),
	CONSTRAINT [FK_NodeConnectionProduct_NodeConnectionProductRule]		FOREIGN KEY					([NodeConnectionProductRuleId])		REFERENCES [Admin].[NodeConnectionProductRule] ([RuleId])
)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProduct',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProduct',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProduct',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProduct',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version column used for consistency',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProduct',
    @level2type = N'COLUMN',
    @level2name = N'RowVersion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for an association of product with a node connection',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProduct',
    @level2type = N'COLUMN',
    @level2name = N'NodeConnectionProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for a node connection',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProduct',
    @level2type = N'COLUMN',
    @level2name = N'NodeConnectionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for a product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProduct',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The uncertainty percentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProduct',
    @level2type = N'COLUMN',
    @level2name = N'UncertaintyPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the connection is deleted, 1 means deleted',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProduct',
    @level2type = N'COLUMN',
    @level2name = N'IsDeleted'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of Estrategia de propiedad for connection',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProduct',
    @level2type = N'COLUMN',
    @level2name = N'NodeConnectionProductRuleId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' This table holds the details for associations of products with the connections of nodes.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProduct',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The priority of connection or product between 1 and 10',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProduct',
    @level2type = N'COLUMN',
    @level2name = N'Priority'