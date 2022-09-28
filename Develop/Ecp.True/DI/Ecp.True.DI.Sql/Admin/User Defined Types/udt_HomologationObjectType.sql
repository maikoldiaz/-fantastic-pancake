/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Sep-08-2019
-- Updated Date:	Mar-20-2020
 <Description>:		This is used as a parameter for SaveHomologation SP.   </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[HomologationObjectType] AS TABLE
(
	[TempId]					INT				NOT NULL, 
	[HomologationObjectId]		INT             NOT NULL,
    [HomologationObjectTypeId]	INT				NOT NULL,
    [IsRequiredMapping]			BIT             NOT NULL    DEFAULT 1,
	[HomologationGroupId]		INT				NOT NULL,
    [CreatedBy]					NVARCHAR (260)  NOT NULL,
    [CreatedDate]				DATETIME        NOT NULL,
    [LastModifiedBy]			NVARCHAR (260)  NULL,
    [LastModifiedDate]			DATETIME        NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is used as a parameter for SaveHomologation SP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'HomologationObjectType',
    @level2type = NULL,
    @level2name = NULL