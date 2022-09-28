/*==============================================================================================================================
--Author:        Microsoft
--Created date : Nov-06-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data types of status used in ownership nodes, tickets, fileregistrations. This is a master table and has seeded data. </Description>
===============================================================================================================================*/
CREATE TABLE [Admin].[StatusType]
(
	    --Columns
	[StatusTypeId]			INT IDENTITY (10, 1)	NOT NULL,
	[StatusType]			VARCHAR(50)				NOT NULL,

	 --Internal Common Columns
	[CreatedBy]				NVARCHAR (260)			NOT NULL,
	[CreatedDate]			DATETIME				NOT NULL DEFAULT (Admin.udf_GetTrueDate())

	--Constraints
    CONSTRAINT [PK_StatusType]	PRIMARY KEY CLUSTERED ([StatusTypeId] ASC),
	CONSTRAINT [UC_StatusType] UNIQUE NONCLUSTERED ([StatusType] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the status type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StatusType',
    @level2type = N'COLUMN',
    @level2name = N'StatusTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the status (Finalizado, Fallido, Procesando, etc.)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StatusType',
    @level2type = N'COLUMN',
    @level2name = N'StatusType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StatusType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StatusType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' This table holds the data types of status used in ownership nodes, tickets, fileregistrations. This is a master table and has seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StatusType',
    @level2type = NULL,
    @level2name = NULL