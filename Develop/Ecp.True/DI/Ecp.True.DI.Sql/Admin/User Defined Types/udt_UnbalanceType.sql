/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Oct-15-2019
-- Updated Date:	Mar-20-2020
 <Description>:		This is used as a parameter for SaveTicket SP.  </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[UnbalanceType] AS TABLE
(
	--Columns	   
	[NodeId]						INT						   NOT NULL,
	[ProductId]						NVARCHAR (20)			   NOT NULL,
	[Unbalance]						DECIMAL(29, 16)			   NOT NULL,
	[Units]							NVARCHAR(50)			   NOT NULL,
	[UnbalancePercentage]			DECIMAL(29, 16)			   NOT NULL,
	[ControlLimit]					DECIMAL(29, 16)			   NOT NULL,
	[Comment]						NVARCHAR(1000)			   NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is used as a parameter for SaveTicket SP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'UnbalanceType',
    @level2type = NULL,
    @level2name = NULL
