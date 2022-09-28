/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Sep-24-2019
-- Updated Date:	Mar-20-2020
 <Description>:		UDT to act as a parameter of Type StorageLocationProductOwnerType.  </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[StorageLocationProductOwnerType] AS TABLE
(
	--Columns
	[TempId]						INT				NOT NULL, 
	[StorageLocationProductOwnerId]	INT				NOT NULL,
	[OwnerId]						INT				NOT NULL,
	[OwnershipPercentage]			DECIMAL(29, 16) NOT NULL,
	[StorageLocationProductId]		INT				NOT NULL,
	[IsDeleted]						BIT				NOT NULL	DEFAULT 0,		--> 1=Deleted

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)  NOT NULL,
	[CreatedDate]					DATETIME		NOT NULL,
	[LastModifiedBy]				NVARCHAR (260)  NULL,
	[LastModifiedDate]				DATETIME        NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'UDT to act as a parameter of Type StorageLocationProductOwnerType.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'StorageLocationProductOwnerType',
    @level2type = NULL,
    @level2name = NULL