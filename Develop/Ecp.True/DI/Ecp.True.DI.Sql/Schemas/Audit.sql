CREATE SCHEMA [Audit]

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This schema contains AuditLog.',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = NULL,
    @level1name = NULL,
    @level2type = NULL,
    @level2name = NULL
