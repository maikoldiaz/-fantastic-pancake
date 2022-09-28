/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Mar-24-2021
 <Description>:		This is used as a parameter for get Movements SP..  </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[MovementListType] AS TABLE
(
	--Columns
    [MovementTransactionId]	INT NOT NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is used as a parameter for get Movements SP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'MovementListType',
    @level2type = NULL,
    @level2name = NULL