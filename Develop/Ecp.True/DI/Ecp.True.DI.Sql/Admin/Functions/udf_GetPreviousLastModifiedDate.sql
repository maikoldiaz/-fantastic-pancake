/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	Jul-08-2020
-- Updated Date:	Mar-20-2020
-- <Description>:   This is a function to Return DateValue Based on Parameters from Ticket Table </Description>
-- ==============================================================================================================================*/
CREATE FUNCTION [Admin].[udf_GetPreviousLastModifiedDate] 
(
    @TicketId               INT,
    @TicketSegmentId        INT,
    @TicketStartDate        DATE,
    @TicketEndDate          DATE
)
RETURNS DATETIME
AS
BEGIN
    DECLARE @DateValue DATETIME
    SELECT TOP 1 @DateValue = CreatedDate
	FROM Admin.Ticket
	WHERE StartDate = @TicketStartDate
	AND EndDate  = @TicketEndDate
	AND CategoryElementId = @TicketSegmentId
	AND [Status] = 4 --Deltas Success
	AND TicketId < @TicketId
	ORDER BY TicketId DESC
    RETURN @DateValue
END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
                            @value = N'This is a function to Return DateValue Based on Parameters from Ticket Table',
                            @level0type = N'SCHEMA',
                            @level0name = N'Admin',
                            @level1type = N'FUNCTION',
                            @level1name = N'udf_GetPreviousLastModifiedDate',
                            @level2type = NULL,
                            @level2name = NULL