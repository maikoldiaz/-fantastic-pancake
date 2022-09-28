CREATE SCHEMA [Analytics]

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This schema contains objects related to ADF.',
    @level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = NULL,
    @level1name = NULL,
    @level2type = NULL,
    @level2name = NULL