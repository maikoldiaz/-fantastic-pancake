/*==============================================================================================================================
--Author:        Microsoft
--Created Date : July-06-2020
--Updated Date : 
--<Description>: This table holds the data for consolidated owners.  </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[ConsolidatedOwner]
(
	--Columns
	[ConsolidatedOwnerId]			    INT IDENTITY (1, 1)		NOT NULL,
	[OwnerId]						    INT						NOT	NULL,
	[OwnershipVolume]			        DECIMAL(18, 2)			NOT NULL,
	[OwnershipPercentage]		        DECIMAL(5, 2)			NOT NULL,
	[ConsolidatedMovementId]	        INT	                    NULL,
    [ConsolidatedInventoryProductId]	INT		                NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)  NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)  NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_ConsolidatedOwner]						                        PRIMARY KEY CLUSTERED ([ConsolidatedOwnerId] ASC),
	CONSTRAINT [FK_ConsolidatedOwner_Owner]			                                FOREIGN KEY	([OwnerId])			                    REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_ConsolidatedOwner_ConsolidatedMovement]			                FOREIGN KEY	([ConsolidatedMovementId])			    REFERENCES [Admin].[ConsolidatedMovement] ([ConsolidatedMovementId]),
	CONSTRAINT [FK_ConsolidatedOwner_ConsolidatedInventoryProduct]			        FOREIGN KEY	([ConsolidatedInventoryProductId])      REFERENCES [Admin].[ConsolidatedInventoryProduct] ([ConsolidatedInventoryProductId]),
)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Consolidated Owner Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedOwner',
    @level2type = N'COLUMN',
    @level2name = N'ConsolidatedOwnerId'
GO
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Owner Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Ownership Volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Ownership Percentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedOwner',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Consolidated Movement Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedOwner',
    @level2type = N'COLUMN',
    @level2name = N'ConsolidatedMovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Consolidated Inventory Product Identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedOwner',
    @level2type = N'COLUMN',
    @level2name = N'ConsolidatedInventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedOwner',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedOwner',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for consolidated owners.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ConsolidatedOwner',
    @level2type = NULL,
    @level2name = NULL