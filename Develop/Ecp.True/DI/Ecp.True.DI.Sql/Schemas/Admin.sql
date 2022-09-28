CREATE SCHEMA [Admin]

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This schema contains objects used for App.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = NULL,
    @level1name = NULL,
    @level2type = NULL,
    @level2name = NULL