/*==============================================================================================================================
--Author:        Microsoft
--Created date : Sep-19-2019
--Updated date : Mar-20-2020
--<Description>: This table holds details for associations of owners with a node connection products. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[NodeConnectionProductOwner]
(
	[NodeConnectionProductOwnerId]	INT IDENTITY (1, 1) NOT NULL,
	[OwnerId]						INT NOT NULL,
	[OwnershipPercentage]			DECIMAL(5, 2) NOT NULL,
	[NodeConnectionProductId]		INT NOT NULL,
	[IsDeleted]						BIT					NOT NULL	DEFAULT 0,		--> 1=Deleted

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
	CONSTRAINT [PK_NodeConnectionProductOwner]							PRIMARY KEY CLUSTERED	([NodeConnectionProductOwnerId] ASC),
	CONSTRAINT [FK_NodeConnectionProductOwner_NodeConnectionProduct]	FOREIGN KEY		([NodeConnectionProductId])					REFERENCES [Admin].[NodeConnectionProduct] ([NodeConnectionProductId]),
	CONSTRAINT [FK_NodeConnectionProductOwner_CategoryElement]			FOREIGN KEY		([OwnerId])									REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [UC_NodeConnectionProductId_OwnerId]						UNIQUE NONCLUSTERED ([NodeConnectionProductId], [OwnerId])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for an association of an owner with a node connection product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeConnectionProductOwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the owner (category element of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The percentage of ownership',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of node connection product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'NodeConnectionProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the connection is deleted, 1 means deleted',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'IsDeleted'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds details for associations of owners with a node connection products.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionProductOwner',
    @level2type = NULL,
    @level2name = NULL