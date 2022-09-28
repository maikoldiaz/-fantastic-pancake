/******************************************************************************
-- Type = input paramter is null or not passed along with SP
Output: It should throw an error saying input paramter @TicketId is required
********************************************************************************/
   
-- Execute Stored Procedure without an error
EXEC [Admin].usp_SaveOperativeNodeRelationship 

