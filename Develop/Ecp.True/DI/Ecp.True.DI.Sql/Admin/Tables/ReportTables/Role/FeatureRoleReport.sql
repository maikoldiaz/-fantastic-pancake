/*==============================================================================================================================
--Author:        InterGrupo
--Created date : Jun-15-2021
--<Description>: This table contains the relationship of feature with menus </Description>
================================================================================================================================*/
CREATE TABLE [admin].[FeatureRoleReport]
(
	[FeatureRoleId]                 INT NOT NULL , 
    [RoleName]                      NVARCHAR(250) NOT NULL, 
    [FeatureNameId]                 NVARCHAR(250) NOT NULL, 
    [FeatureNameDescripcion]        NVARCHAR(250) NULL, 
    [ExecutionId]                   INT NOT NULL, 
    [CreatedBy]                     NVARCHAR(260) NOT NULL, 
    [CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL
)


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the feature (sub menu in the nav bar under scenario/main menu)',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'FeatureRoleId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the role (Administrador, Consulta, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'RoleName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the feature(sub menu)',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'FeatureNameId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name traslate of the feature(sub menu)',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'FeatureNameDescripcion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution ',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRoleReport',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'