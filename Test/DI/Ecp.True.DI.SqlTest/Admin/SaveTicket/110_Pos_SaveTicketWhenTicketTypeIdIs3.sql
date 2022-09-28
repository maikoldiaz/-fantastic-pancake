-- 10. ******************************************************************************
-- Type = INSERT-Positive
-- INSERT --> Data into Ticket table whith OwnerId when TicketTypeId = 3.
------------------------------------------------------------------------------------
DECLARE @CategoryElementId						INT = 10,
		@StartDate								DATETIME = '2019-12-10',
		@EndDate								DATETIME = '2019-12-15',
		@UserId									NVARCHAR (260) = 'Bond',
		@TicketTypeId							INT = 3,
		@PendingTransactionErrorMessagesType    Admin.PendingTransactionErrorMessagesType,
		@UnbalanceType						    Admin.UnbalanceType,
		@UncertaintyPercentage					DECIMAL(29, 16),
		@OwnerId								INT = 27;


Insert Into @PendingTransactionErrorMessagesType  ( [ErrorId], [Comment] )
Values (60, 'test Comment : 1'),
	   (70, 'test Comment : 2')


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
SELECT 	'32' AS [NodeId]
	    ,'10000002199' AS [ProductId]					
	    ,'1100' AS [Unbalance]					
	    ,'Bls' AS [Units]					
	    ,'001' AS [UnbalancePercentage]		
	    ,'1' AS [ControlLimit]	
	    ,'test Comment : 1' AS [Comment]

DECLARE @Out_TicketID INT;

EXECUTE [Admin].usp_SaveTicket
		 @CategoryElementId					
		,@StartDate							
		,@EndDate							
		,@UserId
		,@TicketTypeId
		,@PendingTransactionErrorMessagesType
		,@UnbalanceType	
		,@UncertaintyPercentage
		,@OwnerId
		,@Out_TicketId = @Out_TicketId;
