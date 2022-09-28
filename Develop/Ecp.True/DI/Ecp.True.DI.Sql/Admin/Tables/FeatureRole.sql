/*==============================================================================================================================
--Author:        Microsoft
--Created date : Nov-01-2019
--Updated date : Mar-20-2020
--<Description>: This table holds the data for FeatureRole (submenu and the roles for which it is accessible). This is a master table and contains seeded data. </Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[FeatureRole]
(
	[FeatureRoleId] INT IDENTITY (101, 1)		NOT NULL,
	[RoleId]		INT						NOT NULL,
	[FeatureId]		INT						NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),

	--Constraints
	CONSTRAINT [PK_FeatureRole]							PRIMARY KEY CLUSTERED ([FeatureRoleId] ASC),
	CONSTRAINT [FK_FeatureRole_Role_RoleId]				FOREIGN KEY ([RoleId])        		REFERENCES [Admin].[Role] ([RoleId]),
	CONSTRAINT [FK_FeatureRole_Feature_FeatureId]		FOREIGN KEY ([FeatureId])        	REFERENCES [Admin].[Feature] ([FeatureId])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the featurerole',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRole',
    @level2type = N'COLUMN',
    @level2name = N'FeatureRoleId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the role to which the submenu is accessible',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRole',
    @level2type = N'COLUMN',
    @level2name = N'RoleId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the feature (submenu)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRole',
    @level2type = N'COLUMN',
    @level2name = N'FeatureId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRole',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRole',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for FeatureRole (submenu and the roles for which it is accessible). This is a master table and contains seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'FeatureRole',
    @level2type = NULL,
    @level2name = NULL