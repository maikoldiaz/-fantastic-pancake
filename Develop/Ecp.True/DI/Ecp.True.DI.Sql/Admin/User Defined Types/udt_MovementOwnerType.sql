/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Oct-29-2020
 <Description>:		This is used as a parameter for Save Delta bulk Movements SP.  </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[MovementOwnerType] AS TABLE
(
	--Columns
	[Id]							INT NULL,
	[TempId]						INT NOT NULL,
	[OwnerId]						INT NOT NULL,
	[OwnershipValue]				DECIMAL (18, 2) NOT NULL,
	[OwnershipValueUnit]			NVARCHAR (50) NOT NULL,
	[BlockchainStatus]              INT NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)	NULL
);

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is used as a parameter for Save Delta bulk Movements SP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'MovementOwnerType',
    @level2type = NULL,
    @level2name = NULL
