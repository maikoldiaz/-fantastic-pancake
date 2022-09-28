/******************************************************************************
-- Type = After this stores procedure execute data should e loaded into 
          [Analytics].[OperativeMovementsWithOwnership and [Analytics].[OwnershipPercentageValues]
********************************************************************************/

-- Execute Stored Procedure

EXEC [Admin].[usp_SaveOperativeMovementsWithOwnershipPercentage] 24077


-- EXPECTED OUTPUT
/*
SINCE MOVEMENT TYPE IS NULL THEN THE EXPECTED OUTPUT IS
   1. Stored Proedure will execute succesfully
   2. Log Execution status (1) and error message (as "NULL") in [Admin].[Ticket] ([OwnershipAnalyticsStatus],[OwnershipAnalyticsErrorMessage])
*/

-- ACTUAL OUTPUT
SELECT * FROM [Admin].[Ticket] WHERE TicketId = 24077

/*
TicketId	CategoryElementId	StartDate	                    EndDate	         Status	TicketTypeId	TicketGroupId	                     ErrorMessage	OwnerId	    NodeId	BlobPath	AnalyticsStatus	AnalyticsErrorMessage	CreatedBy	CreatedDate	            LastModifiedBy	      LastModifiedDate	        
24077	       29250	      2020-05-04 00:00:00.000	2020-05-04 00:00:00.000	   2	      2	        df30c49c-6067-40bf-ab4e-e77fc539e2ba	NULL	       NULL	    NULL	NULL	     NULL	              NULL	             trueadmin	2020-05-05 09:17:16.507	   System	        2020-05-05 09:17:46.450  	

OwnershipAnalyticsStatus	OwnershipAnalyticsErrorMessage
1	                              NULL

*/