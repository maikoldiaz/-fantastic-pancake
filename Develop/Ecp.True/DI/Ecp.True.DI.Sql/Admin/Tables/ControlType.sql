/*==============================================================================================================================
--Author:        Microsoft
--Created date : Dec-03-2020
--Updated date : Mar-20-2020
--<Description>: This table holds the data for ControlType (InitialInventory, Unbalance, Tolerance, etc). This is a master table and contains seeded data. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[ControlType]
(
	--Columns
	[ControlTypeId]					INT IDENTITY(1,1)			NOT NULL,
	[Name]							NVARCHAR (50)				NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)				NOT NULL,
	[CreatedDate]					DATETIME					NOT NULL    DEFAULT Admin.udf_GetTrueDate(),

	--Constraints
	CONSTRAINT [PK_ControlTypeId]					PRIMARY KEY CLUSTERED ([ControlTypeId] ASC)
);

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the control type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ControlType',
    @level2type = N'COLUMN',
    @level2name = N'ControlTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the control type (Unbalance, Interface, InitialInventory, etc.)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ControlType',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ControlType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ControlType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for ControlType (InitialInventory, Unbalance, Tolerance, etc). This is a master table and contains seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ControlType',
    @level2type = NULL,
    @level2name = NULL