CREATE SCHEMA [Report]

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This schema contains objects related to powerbi reports.',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = NULL,
    @level1name = NULL,
    @level2type = NULL,
    @level2name = NULL
