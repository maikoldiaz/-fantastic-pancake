/*===========================================================================================================================================================
-- Author:          Microsoft
-- Created date:	Dec-3-2019
-- Updated date:	Mar-20-2020
--<Description>: This table holds the data for Version of Homologation and Transformation. This is a master table and has seeded data. </Description>
*/
CREATE TABLE [Admin].[Version]
(
	[VersionId]		INT IDENTITY (1, 1)	NOT NULL, 
	[Number]		INT					NOT NULL DEFAULT 0,
	[Type]			NVARCHAR (50)       NOT NULL,
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the version',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Version',
    @level2type = N'COLUMN',
    @level2name = N'VersionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version number',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Version',
    @level2type = N'COLUMN',
    @level2name = N'Number'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the version (Homologation, Transformation)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Version',
    @level2type = N'COLUMN',
    @level2name = N'Type'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for Version of Homologation and Transformation. This is a master table and has seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Version',
    @level2type = NULL,
    @level2name = NULL