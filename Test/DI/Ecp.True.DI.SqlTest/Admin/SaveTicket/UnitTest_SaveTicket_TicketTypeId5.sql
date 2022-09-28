DECLARE
  @CategoryElementId                                    INT= 3,
       @StartDate                                            DATETIME = '01-07-2020',
       @EndDate                                              DATETIME= '10-07-2020',
       @UserId                                               NVARCHAR(260)= 'trueadmin',
	   @TicketTypeId										 INT = 5,
       @UncertaintyPercentage								 DECIMAL(29,16) ,
	   @OwnerId												 INT=27 ,
	   @NodeId												 INT ,
	   @TicketGroupId										 NVARCHAR(255) ,
	   @SessionId											 NVARCHAR(50)


EXEC Admin.usp_SaveTicket @CategoryElementId       
						  ,@StartDate               
						  ,@EndDate                 
						  ,@UserId                  
						  ,@TicketTypeId			
						  ,@UncertaintyPercentage	
						  ,@OwnerId					
						  ,@NodeId					
						  ,@TicketGroupId			
						  ,@SessionId	
						  
--26893

Select * from Admin.Ticket
where TicketId='26893'

--TicketId	CategoryElementId	StartDate				EndDate	Status				TicketTypeId	TicketGroupId	ErrorMessage	OwnerId	NodeId	BlobPath	AnalyticsStatus	AnalyticsErrorMessage	CreatedBy	CreatedDate				LastModifiedBy	LastModifiedDate
--26893			3				2020-01-07 00:00:00.000	2020-10-07 00:00:00.000	1	5				NULL			NULL			NULL	NULL	NULL			NULL			NULL				trueadmin	2020-07-10 05:40:51.460		NULL		NULL