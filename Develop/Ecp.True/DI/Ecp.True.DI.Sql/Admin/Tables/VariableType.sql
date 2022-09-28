/*===========================================================================================================================================================
-- Author:          Microsoft
-- Created date:	Oct-11-2019
-- Updated date:	Mar-31-2020
--<Description>: This table holds the data for types of variables (Interfase, Tolerancia, etc). This is a master table and has seeded data. </Description>
============================================================================================================================================================*/
CREATE TABLE [Admin].[VariableType]
(
	    --Columns
	[VariableTypeId]				INT IDENTITY (101, 1)	NOT NULL,
    [Name]							NVARCHAR (50)			NOT NULL,
	[ShortName]						NVARCHAR (10)			NOT NULL,
	[FicoName]						NVARCHAR (50)			NULL,
	[IsConfigurable]				BIT						NOT NULL DEFAULT 0,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)		NOT NULL,
	[CreatedDate]					DATETIME			NOT NULL    DEFAULT Admin.udf_GetTrueDate(),

	--Constraints
    CONSTRAINT [PK_VariableType] PRIMARY KEY CLUSTERED ([VariableTypeId] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the variable type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'VariableType',
    @level2type = N'COLUMN',
    @level2name = N'VariableTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The spanish name of the variable type (like Tolerancia, Entrada, Salida, etc.)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'VariableType',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The short name of the variable type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'VariableType',
    @level2type = N'COLUMN',
    @level2name = N'ShortName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The FICO alternative name for the variable type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'VariableType',
    @level2type = N'COLUMN',
    @level2name = N'FicoName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the variable type is configurable or not, 1 means yes',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'VariableType',
    @level2type = N'COLUMN',
    @level2name = N'IsConfigurable'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'VariableType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'VariableType',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for types of variables (Interfase, Tolerancia, etc). This is a master table and has seeded data. ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'VariableType',
    @level2type = NULL,
    @level2name = NULL