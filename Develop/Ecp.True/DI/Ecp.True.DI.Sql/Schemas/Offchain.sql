CREATE SCHEMA [Offchain]

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This schema contains objects for data from blockchain.',
    @level0type = N'SCHEMA',
    @level0name = N'Offchain',
    @level1type = NULL,
    @level1name = NULL,
    @level2type = NULL,
    @level2name = NULL
