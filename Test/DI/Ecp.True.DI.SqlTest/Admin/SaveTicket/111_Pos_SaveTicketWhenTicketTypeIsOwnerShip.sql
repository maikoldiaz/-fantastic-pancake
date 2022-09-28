DECLARE @PendingTransactionErrorMessagesType       Admin.PendingTransactionErrorMessagesType ,
        @UnbalanceType                              Admin.UnbalanceType ,
        @Out_TicketId                               INT 

DECLARE @ElementID INT = (
							SELECT TOP 1 ElementId FROM ADmin.CategoryElement CE
							WHERE CategoryId = 2
							ORDER BY ElementId ASC
						  )
DECLARE @StartDate DATE									 = GETDATE()-3, 
		@EndDate DATE									 = GETDATE()-1,
		@ID NVARCHAR(255)								 = NEWID(),
		@UncertaintyPercentage		DECIMAL(29, 16)      = 10

EXEC  [Admin].[usp_SaveTicket]    @ElementID
                                 ,@StartDate 
                                 ,@EndDate
                                 ,'system'
                                 ,'2'--OwnershipTicketType
                                  ,@PendingTransactionErrorMessagesType
                                 ,@UnbalanceType
                                 ,@UncertaintyPercentage
                                 ,NULL
                                 ,NULL
                                 ,@ID
                                 ,@Out_TicketId