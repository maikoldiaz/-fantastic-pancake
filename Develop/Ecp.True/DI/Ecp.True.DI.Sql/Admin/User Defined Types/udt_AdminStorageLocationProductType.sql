/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Aug-20-2019
-- Updated Date:	Mar-20-2020
 <Description>:		This is used as a parameter for UnbalanceCalculation and NodeTag SP.  </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[StorageLocationProductType] AS TABLE
(
	--Columns
	[TempId]						INT				NOT NULL, 
	[StorageLocationProductId]		INT				NOT NULL,
	[IsActive]						BIT				NOT NULL	DEFAULT 1, 
	[ProductId]						NVARCHAR(20)	NOT NULL, 
	[NodeStorageLocationId]			INT				NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)	NULL,
	[CreatedDate]					DATETIME		NULL, 
	[LastModifiedBy]				NVARCHAR (260)	NULL,
	[LastModifiedDate]				DATETIME		NULL
);

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is used as a parameter for UnbalanceCalculation and NodeTag SP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'StorageLocationProductType',
    @level2type = NULL,
    @level2name = NULL