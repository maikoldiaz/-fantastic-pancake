/*==============================================================================================================================
--Author:        Microsoft
--Created date : Dec-09-2019	
--Updated date : Mar-20-2020
--<Description>: This table holds the data for ControlScript (deployment scripts) used for making one time or everytime changes before or after deployment </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[ControlScript]
(
	[Id]				UNIQUEIDENTIFIER	NOT NULL DEFAULT newid(),  -- Unique ID for Script
	[Status]			BIT					NOT NULL DEFAULT 0, -- Predeployment Script Ran Or Not. 0 = NotRan; 1 = Already Ran
	[DeploymentType]	NVARCHAR(10)		NULL,		-- Pre or Post
	[ExecutedDate]		DATETIME			NOT NULL    DEFAULT Admin.udf_GetTrueDate()
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The GUID of controlscript(deployment script)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ControlScript',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the controlscript ran successfully or not, 1 means success',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ControlScript',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the controlscript, post for postdeployment, null for predeployment',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ControlScript',
    @level2type = N'COLUMN',
    @level2name = N'DeploymentType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the controlscript was executed',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ControlScript',
    @level2type = N'COLUMN',
    @level2name = N'ExecutedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for ControlScript (deployment scripts) used for making one time or everytime changes before or after deployment',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ControlScript',
    @level2type = NULL,
    @level2name = NULL