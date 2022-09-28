/*===========================================================================================================================================================
-- Author:          Microsoft
-- Created date:	Nov-21-2019
-- Updated date:	Mar-20-2020
--<Description>: This table holds the data for TicketType values like Cutoff,Ownership,Logistics. This is a master table and has seeded data. </Description>
=============================================================================================================================================================*/
CREATE TABLE [Admin].[TicketType]
(
	--Columns
	[TicketTypeId]	INT	IDENTITY(101,1)	NOT NULL,
	[Name]			NVARCHAR(50)		NOT NULL,
	
	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),

	--Constraints
	CONSTRAINT [PK_TicketType]					PRIMARY KEY CLUSTERED ([TicketTypeId] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for TicketType values like Cutoff,Ownership,Logistics. This is a master table and has seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketType',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for the ticket type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketType',
    @level2type = N'COLUMN',
    @level2name = N'TicketTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the ticket type (Logistics, Cutoff, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketType',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'