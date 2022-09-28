/* ======================================================================================================
--Author:          Microsoft
-- Created date:	Jul-08-2020
-- Updated date:	
--<Description>: This table holds the data for OfficialDeltaMessageType. This is a master table and has seeded data. </Description>
===========================================================================================================*/
CREATE TABLE [Admin].[OfficialDeltaMessageType]
(
    --Columns
	[OfficialDeltaMessageTypeId]		INT IDENTITY (101, 1)   NOT NULL,
    [Name]						        NVARCHAR (50)		    NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME         NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME         NULL,

	--Constraints
    CONSTRAINT [PK_OfficialDeltaMessageType] PRIMARY KEY CLUSTERED ([OfficialDeltaMessageTypeId] ASC),
	CONSTRAINT [UC_OfficialDeltaMessageType] UNIQUE NONCLUSTERED ([Name] ASC)
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the official delta message type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMessageType',
    @level2type = N'COLUMN',
    @level2name = N'OfficialDeltaMessageTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the type.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMessageType',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMessageType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMessageType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMessageType',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMessageType',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for OfficialDeltaMessageType. This is a master table and has seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMessageType',
    @level2type = NULL,
    @level2name = NULL