/******************************************************************************
-- Type = UPDATE-On Successfull execution
-- UPDATE --> Update Analytics Status with "1" aganest the ticket Id in [Admin].[Ticket] table.
********************************************************************************/
-- TicketId as Input Parameter
DECLARE @TicketId	INT = (SELECT TOP 1 TicketId FROM [Admin].[Ticket] WHERE AnalyticsStatus IS NULL) 
 
-- Execute Stored Procedure
EXEC [Admin].usp_SaveOperativeNodeRelationship @TicketId

-- Check Whether AnalyticsStatus column is updated with correct status (1)
SELECT * FROM [Admin].[Ticket] WHERE TicketId = @TicketId

-- Change the status value back to NULL, so that it won't impact the actual execution
UPDATE [Admin].[Ticket] SET AnalyticsStatus = NULL WHERE TicketId = @TicketId