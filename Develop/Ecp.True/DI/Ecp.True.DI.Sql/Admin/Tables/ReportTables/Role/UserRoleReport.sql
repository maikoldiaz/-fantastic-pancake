/*==============================================================================================================================
--Author:        InterGrupo
--Created date : Jun-17-2021
--<Description>: This table contains the relationship of users, roles with menus </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[UserRoleReport]
(
	
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Name] NVARCHAR(250) NULL,
    [UserType] NVARCHAR(50) NOT NULL,
    [Email] NVARCHAR(250) NULL,
    [Role] NVARCHAR(50) NOT NULL,
	[FeatureNameId]                 NVARCHAR(250) NOT NULL, 
    [FeatureNameDescripcion]        NVARCHAR(250) NULL, 
    [ExecutionId]                   INT NOT NULL, 
	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	
)

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier user active directory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UserRoleReport',
    @level2type = N'COLUMN',
    @level2name = 'UserId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of user active directory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UserRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The user type in active directory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UserRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'UserType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The email of user in active directory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UserRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'Email'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The role of user in active directory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UserRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'Role'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the feature(sub menu)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UserRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'FeatureNameId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name traslate of the feature(sub menu)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UserRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'FeatureNameDescripcion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UserRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UserRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UserRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UserRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UserRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'