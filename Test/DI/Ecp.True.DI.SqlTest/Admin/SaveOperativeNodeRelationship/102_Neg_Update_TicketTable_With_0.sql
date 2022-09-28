--******************************************************************************
-- Type = UPDATE-On Failure execution
-- UPDATE --> Update Analytics Status with "0" aganest the ticket Id in [Admin].[Ticket] table.
--Note: Here SP won't fail because the tables which we are using in stored procedure are having unique data. 
--If we have duplicate data then it will fail and log the error status and error message in respective columns in the table.
-- Hence in this case will see the log with success, coz we don't have duplicate data.
------------------------------------------------------------------------------------
-- TicketId as Input Parameter
 DECLARE @TicketId	INT = (SELECT TOP 1 TicketId FROM [Admin].[Ticket] WHERE AnalyticsStatus IS NULL) 
   
-- Execute Stored Procedure
EXEC [Admin].usp_SaveOperativeNodeRelationship @TicketId

-- Check Whether AnalyticsStatus column is updated with correct status (0) and AnalyticsErrorMessage with proper error message
SELECT * FROM [Admin].[Ticket] WHERE TicketId = @TicketId

-- Change the status value back to NULL, so that it won't impact the actual execution
UPDATE [Admin].[Ticket] SET AnalyticsStatus = NULL, AnalyticsErrorMessage = NULL WHERE TicketId = @TicketId