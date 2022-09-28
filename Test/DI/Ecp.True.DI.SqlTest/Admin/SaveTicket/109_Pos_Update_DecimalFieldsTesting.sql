-- . ******************************************************************************
--9 Passing UnBalance,[UnbalancePercentage],ControlLimit with More than 2 Decimals to Check if can save 2 digists decimal
--And PassingNodeId not in Database
------------------------------------------------------------------------------------

DECLARE @CategoryElementId						INT = 2,
		@StartDate								DATETIME = '2019-10-01',
		@EndDate								DATETIME = '9999-12-31',
		@UserId									NVARCHAR (260) = 'system',
		@PendingTransactionErrorMessagesType    Admin.PendingTransactionErrorMessagesType ,
		@UnbalanceType						    Admin.UnbalanceType 


Insert Into @PendingTransactionErrorMessagesType  
(
 [ErrorId],
 [Comment]
)
Values (3, 'test Comment : 3'),
	   (4, 'test Comment : 4')


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
SELECT 	'323232' AS [NodeId]
	    ,'10000002199' AS [ProductId]					
	    ,'1100.352' AS [Unbalance]					
	    ,'Units12' AS [Units]					
	    ,'18.356' AS [UnbalancePercentage]		
	    ,'91.585' AS [ControlLimit]				
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

SELECT * FROM [Admin].Ticket						WHERE TicketId = '23700'
Select * from [Admin].Inventory						WHERE TicketId = '23700'
Select * from [Admin].Movement						WHERE TicketId = '23700'
Select * from [Admin].PendingTransaction			WHERE TicketId = '23700'
Select * from [Admin].PendingTransactionError		WHERE ErrorId IN (3,4)
Select * from [Admin].UnbalanceComment				WHERE TicketId = '23700'






