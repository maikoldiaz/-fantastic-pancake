/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Jun-18-2020
-- Updated Date:	Jun-19-2020
 <Description>:		This is used as a parameter for UpdateCutOffComment SP.  </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[SapTrackingType] AS TABLE
(
	--Columns
	[SapTrackingId]					INT						    NOT NULL,
    [MovementTransactionId]			INT						    NOT NULL,
	[Comment]						NVARCHAR(1000)				NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is used as a parameter for SaveTicket SP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'SapTrackingType',
    @level2type = NULL,
    @level2name = NULL