/*==============================================================================================================================
--Author:        Microsoft
--Created date : Oct-03-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data for different Roles in the system like ADMINISTRADOR,APROBADOR. This is a master table and has seeded data. </Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[Role]
(
	[RoleId]		INT IDENTITY (101, 1)		NOT NULL,
	[RoleName]		NVARCHAR (260)			NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),

	--Constraints
	CONSTRAINT [PK_Role]			PRIMARY KEY CLUSTERED ([RoleId] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the role',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Role',
    @level2type = N'COLUMN',
    @level2name = N'RoleId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the role (Administrador, Consulta, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Role',
    @level2type = N'COLUMN',
    @level2name = N'RoleName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Role',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Role',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for different Roles in the system like ADMINISTRADOR,APROBADOR. This is a master table and has seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Role',
    @level2type = NULL,
    @level2name = NULL