-- 8. ******************************************************************************
-- Type = INSERT-Positive---Lengthy and Special characters for Comment Field in UnBalance
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
Values (3, 'test Comment : 1'),
	   (4, 'test Comment : 2')


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
	    ,'UnitesTest' AS [Units]					
	    ,'001' AS [UnbalancePercentage]		
	    ,'1' AS [ControlLimit]				
	    ,'!@#$%^&*())_+jkhjkhkjhkjhkjhkjhkhkhkhkhkhkhkhk!@#$%^&*()))(%#@#$%^#$%^%#@#$%^&' AS [Comment]


DECLARE @Out_TicketID INT

EXECUTE [Admin].usp_SaveTicket
		 @CategoryElementId					
		,@StartDate							
		,@EndDate							
		,@UserId								
		,@PendingTransactionErrorMessagesType
		,@UnbalanceType	
		,@Out_TicketId = @Out_TicketId

SELECT * FROM Admin.Ticket
Select * from [Admin].Inventory
Select * from [Admin].Movement
Select * from [Admin].PendingTransaction
Select * from [Admin].PendingTransactionError
Select * from [Admin].UnbalanceComment 






