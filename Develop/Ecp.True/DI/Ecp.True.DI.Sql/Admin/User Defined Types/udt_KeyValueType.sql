/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Mar-06-2020
-- Updated Date:	Mar-20-2020
 <Description>:		This is used as a parameter for ValidateOwnershipInputs SP.  </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[KeyValueType]	AS TABLE
(
	[Key]					INT				  NOT NULL,
	[Value]					NVARCHAR(250)	  NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is used as a parameter for ValidateOwnershipInputs SP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'KeyValueType',
    @level2type = NULL,
    @level2name = NULL