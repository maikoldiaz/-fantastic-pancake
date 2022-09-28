-- 1. ******************************************************************************
-- Type = INSERT-Positive
-- INSERT --> Data into all Dependent tables
------------------------------------------------------------------------------------

DECLARE @CategoryElementId						INT = 10,
		@StartDate								DATETIME = '2019-09-01',
		@EndDate								DATETIME = '2019-10-15',
		@UserId									NVARCHAR (260) = 'system',
		@PendingTransactionErrorMessagesType    Admin.PendingTransactionErrorMessagesType ,
		@UnbalanceType						    Admin.UnbalanceType 


Insert Into @PendingTransactionErrorMessagesType  
(
 [ErrorId],
 [Comment]
)
Values (6, 'test Comment : 1'),
	   (7, 'test Comment : 2')


INSERT INTO @UnbalanceType 
(
	--Columns	   
	[NodeId]					,
	[ProductId]					,
	[Unbalance]					,
	[Units]						,
	[UnbalancePercentage]		,
	[ControlLimit]				,
	[Comment]					
)
SELECT 	'323' AS [NodeId]
	    ,'10000002199' AS [ProductId]					
	    ,'1100' AS [Unbalance]					
	    ,'Barriles' AS [Units]					
	    ,'001' AS [UnbalancePercentage]		
	    ,'1' AS [ControlLimit]				
	    ,'test Comment : 1' AS [Comment]

DECLARE @Out_TicketID INT

EXECUTE [Admin].usp_SaveTicket
		 @CategoryElementId					
		,@StartDate							
		,@EndDate							
		,@UserId								
		,@PendingTransactionErrorMessagesType
		,@UnbalanceType	
		,@Out_TicketId = @Out_TicketId


SELECT * FROM Admin.Ticket							WHERE TicketId = 23724
Select * from [Admin].Inventory						WHERE TicketId = 23724
Select * from [Admin].Movement						WHERE TicketId = 23724
Select * from [Admin].PendingTransaction			WHERE TicketId = 23724
Select * from [Admin].PendingTransactionError		WHERE ErrorId IN (6,7)
Select * from [Admin].UnbalanceComment				WHERE TicketId = 23724






