/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Sep-29-2019
-- Updated Date:	Mar-20-2020
 <Description>:		This is used as a parameter for NodeTag SP.  </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[NodeTagType] AS TABLE
(
	--Columns
    [NodeTagId]    				INT             NOT NULL,
    [NodeId]         			INT             NOT NULL,

	--Internal Common Columns
	[CreatedBy]					NVARCHAR (260)  NULL,
	[CreatedDate]				DATETIME        NULL,
	[LastModifiedBy]			NVARCHAR (260)  NULL,
	[LastModifiedDate]			DATETIME        NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is used as a parameter for NodeTag SP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'NodeTagType',
    @level2type = NULL,
    @level2name = NULL
