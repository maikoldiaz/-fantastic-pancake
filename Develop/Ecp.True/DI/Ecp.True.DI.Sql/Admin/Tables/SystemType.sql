/* ======================================================================================================
--Author:          Microsoft
-- Created date:	Sep-05-2019
-- Updated date:	Mar-20-2020
--<Description>: This table holds the data for SystemType. This is a master table and has seeded data. </Description>
===========================================================================================================*/
CREATE TABLE [Admin].[SystemType]
(
    --Columns
	[SystemTypeId]				INT IDENTITY (101, 1) NOT NULL,
    [Name]						NVARCHAR (50)		NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
    CONSTRAINT [PK_SystemType] PRIMARY KEY CLUSTERED ([SystemTypeId] ASC)
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the system type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SystemType',
    @level2type = N'COLUMN',
    @level2name = N'SystemTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the type (like Sinoper, Excel, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SystemType',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SystemType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SystemType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SystemType',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SystemType',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for SystemType. This is a master table and has seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SystemType',
    @level2type = NULL,
    @level2name = NULL