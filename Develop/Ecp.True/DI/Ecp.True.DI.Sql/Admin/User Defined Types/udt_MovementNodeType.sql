/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Jul-13-2020
 <Description>:		This is used as a parameter for GetMovementsForConsolidation SP.  </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[MovementNodeType] AS TABLE
(
    [SourceNodeId] INT NULL,
    [DestinationNodeId] INT NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is used as a parameter for GetMovementsForConsolidation SP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'MovementNodeType',
    @level2type = NULL,
    @level2name = NULL