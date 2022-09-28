/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Mar-09-2020
-- Updated Date:	Mar-20-2020
 <Description>:		This is used as a parameter for BulkUpdateRules SP.  </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[KeyType] AS TABLE
(
    [Key] INT
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is used as a parameter for BulkUpdateRules SP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'KeyType',
    @level2type = NULL,
    @level2name = NULL