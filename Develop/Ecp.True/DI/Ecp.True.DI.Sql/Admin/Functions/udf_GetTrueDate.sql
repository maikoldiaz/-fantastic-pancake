/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	Nov-15-2019
-- Updated Date:	Mar-20-2020
-- <Description>:   This function is to fetch current time in Colombian time zone. </Description>
-- ==============================================================================================================================*/

CREATE FUNCTION Admin.udf_GetTrueDate()
RETURNS DATETIME
AS

BEGIN
	-- Declare the return variable here
	DECLARE @CurrentTime DateTime
	
	SET @CurrentTime = (SELECT GETUTCDATE() AT TIME ZONE 'UTC' AT TIME ZONE 'SA Pacific Standard Time')

	-- Return the result of the function
	RETURN @CurrentTime	
END;

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This function is to fetch current time in Colombian time zone.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'FUNCTION',
    @level1name = N'udf_GetTrueDate',
    @level2type = NULL,
    @level2name = NULL