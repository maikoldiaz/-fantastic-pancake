/******************************************************************************
-- Type = After this stores procedure execute data should e loaded into 
          [Analytics].[OperativeMovementsWithOwnership and [Analytics].[OwnershipPercentageValues]
********************************************************************************/

-- Execute Stored Procedure

EXEC [Admin].[usp_SaveOperativeMovementsWithOwnershipPercentage] 24045

-- Process while calculating the movement types
/*
MovementID	OperationalDate	MovementType	SourceProductName	      SourceStorageLocationName	                        DestinationProductName	DestinationStorageLocationName	                       OwnershipVolume	NetStandardVolume	OwnershipPercentage	TransferPoint
4500016482	2020-05-08	        NULL	    CRUDO CAMPO CUSUCO	PR TISQUIRAMA ASO:PR TISQUIRAMA ASO : MATERIA PRIMA	     CRUDO CAMPO CUSUCO	     PR TISQUIRAMA ASO:PR TISQUIRAMA ASO : MATERIA PRIMA	116871.82	      116871.82	          100.00	         RELACION_SS
4500016548	2020-05-06	        NULL	    CRUDO CAMPO CUSUCO	PR TISQUIRAMA ASO:PR TISQUIRAMA ASO : MATERIA PRIMA	     CRUDO CAMPO CUSUCO	     PR TISQUIRAMA ASO:PR TISQUIRAMA ASO : MATERIA PRIMA	401475.74	      401475.74	          100.00	         RELACION_SS
*/

-- EXPECTED OUTPUT
/*
SINCE MOVEMENT TYPE IS NULL THEN THE EXPECTED OUTPUT IS
   1. Stored Proedure has to faile
   2. Log Execution status (0) and error message in [Admin].[Ticket] ([OwnershipAnalyticsStatus],[OwnershipAnalyticsErrorMessage])
*/

-- ACTUAL OUTPUT
SELECT * FROM [Admin].[Ticket] WHERE TicketId = 24045
/*
TicketId	CategoryElementId	StartDate	                EndDate	             Status	TicketTypeId	TicketGroupId	ErrorMessage	OwnerId	   NodeId	  BlobPath	AnalyticsStatus	AnalyticsErrorMessage	CreatedBy	CreatedDate	               LastModifiedBy	LastModifiedDate	     
24045	         26895	      2020-04-26 00:00:00.000	2020-04-29 00:00:00.000	   0	    1	              NULL	         NULL	       NULL	    NULL	   NULL	           1	         NULL	            trueadmin	2020-04-30 05:18:20.877	      System	    2020-04-30 05:18:26.717	 

OwnershipAnalyticsStatus	OwnershipAnalyticsErrorMessage
    0	                    Cannot insert the value NULL into column 'MovementType', table 
                            'UAT_May11.Analytics.OperativeMovementsWithOwnership'; 
                            column does not allow nulls. UPDATE fails.
*/