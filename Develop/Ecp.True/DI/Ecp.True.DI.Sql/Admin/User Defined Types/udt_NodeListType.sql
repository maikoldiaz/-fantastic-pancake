/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Dec-17-2019
-- Updated Date:	Mar-20-2020
 <Description>:		This is used as a parameter for GetInventories and GetMovements SP.  </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[NodeListType] AS TABLE
(
	--Columns
    [NodeId]	INT NOT NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is used as a parameter for GetInventories and GetMovements SP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'NodeListType',
    @level2type = NULL,
    @level2name = NULL