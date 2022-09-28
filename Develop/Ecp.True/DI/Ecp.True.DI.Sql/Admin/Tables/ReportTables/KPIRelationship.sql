/*==============================================================================================================================
--Author:        Microsoft
--Created date : Sep-21-2019
--Updated date : Mar-20-2020
--<Description>: This table contains file action types in Spanish (like Insertar, Eliminar, etc). This is a master table and contains seeded data.</Description>
===============================================================================================================================*/
CREATE TABLE Admin.KPIRelationship
(
    --Columns
	[OrderToDisplay]      INT               NOT NULL,
    [Indicator]           NVARCHAR (50)     NOT NULL,

    --Internal Common Columns
	[CreatedBy]			  NVARCHAR (260)	NOT NULL,
	[CreatedDate]		  DATETIME			NOT NULL,
	
    --Constraints
    CONSTRAINT [PK_KPIRelationship]	PRIMARY KEY CLUSTERED (OrderToDisplay ASC)

);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table contains file action types in Spanish (like Insertar, Eliminar, etc). This is a master table and contains seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIRelationship',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the action type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIRelationship',
    @level2type = N'COLUMN',
    @level2name = N'OrderToDisplay'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The action name (Insertar, Eliminar, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIRelationship',
    @level2type = N'COLUMN',
    @level2name = N'Indicator'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIRelationship',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'KPIRelationship',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
