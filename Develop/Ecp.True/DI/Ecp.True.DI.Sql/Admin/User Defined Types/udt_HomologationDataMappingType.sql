/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Sep-08-2019
-- Updated Date:	Mar-20-2020
 <Description>:		This is used as a parameter for SaveHomologation SP. </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[HomologationDataMappingType] AS TABLE
(
    --Columns
	[TempId]						INT			        NOT NULL, 
	[HomologationDataMappingId]		INT                 NOT NULL,
    [SourceValue]					NVARCHAR (100)      NOT NULL,
    [DestinationValue]				NVARCHAR (100)      NOT NULL,
	[HomologationGroupId]			INT                 NOT NULL,

    --Internal Common Columns
    [CreatedBy]						NVARCHAR (260)      NOT NULL,
    [CreatedDate]					DATETIME            NOT NULL,
    [LastModifiedBy]				NVARCHAR (260)      NULL,
    [LastModifiedDate]				DATETIME            NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is used as a parameter for SaveHomologation SP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'HomologationDataMappingType',
    @level2type = NULL,
    @level2name = NULL