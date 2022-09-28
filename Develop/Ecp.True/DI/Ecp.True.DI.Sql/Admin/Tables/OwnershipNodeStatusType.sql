/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jan-05-2020
--Updated date : Mar-20-2020
--<Description>: This table holds statuses for ownership nodes. This is a master table and contains seeded data.</Description>
*/
CREATE TABLE [Admin].[OwnershipNodeStatusType]
(
	--Columns
	[OwnershipNodeStatusTypeId]			INT IDENTITY (101, 1)	NOT NULL,
	[Name]								VARCHAR(50)				NOT NULL,

	 --Internal Common Columns
	[CreatedBy]				NVARCHAR (260)			NOT NULL,
	[CreatedDate]			DATETIME				NOT NULL DEFAULT (Admin.udf_GetTrueDate())

	--Constraints
    CONSTRAINT [PK_OwnershipNodeStatusType]	PRIMARY KEY CLUSTERED ([OwnershipNodeStatusTypeId] ASC),
	CONSTRAINT [UC_OwnershipNodeStatusType] UNIQUE NONCLUSTERED ([Name] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for OwnershipNodeStatusType. This is a master table and contains seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeStatusType',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for the ownership node status ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeStatusType',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipNodeStatusTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The spanish name of the status (like Enviado, Propiedad)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeStatusType',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeStatusType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OwnershipNodeStatusType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'