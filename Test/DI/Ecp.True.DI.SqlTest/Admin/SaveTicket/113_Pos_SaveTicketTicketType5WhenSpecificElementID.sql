
DECLARE
	   @CategoryElementId                                    INT	= 10,
       @StartDate                                            DATETIME= GETDATE()-2,
       @EndDate                                              DATETIME= GETDATE(),
       @UserId                                               NVARCHAR(260) = 'System',
	   @TicketTypeId										 INT = 5,
       @UncertaintyPercentage								 DECIMAL(29,16)  = 0,
	   @OwnerId												 INT = NULL,
	   @NodeId												 INT = 0,
	   @TicketGroupId										 NVARCHAR(255) ,
	   @SessionId											 NVARCHAR(50) 

EXEC [Admin].[usp_SaveTicket]	 @CategoryElementId               
								,@StartDate                       
								,@EndDate                         
								,@UserId                          
								,@TicketTypeId					
								,@UncertaintyPercentage			
								,@OwnerId							
								,@NodeId							
								,@TicketGroupId					
								,@SessionId


--When Specific ElementId is Passed								
--OutPutTicketId
--26757