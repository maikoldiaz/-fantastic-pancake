----------------------- TEST SCRIPTS FOR [Admin].[usp_GetInputMovementPropertyDetails] ------------------------------------------------------------------------

EXEC [Admin].[usp_GetInputMovementPropertyDetails] 23866

SELECT CategoryElementId, StartDate, EndDate FROM [Admin].[Ticket] WHERE TicketId=23866 

SELECT * FROM [Admin].[NodeTag] WHERE ElementId IN (SELECT CategoryElementId FROM [Admin].[Ticket] WHERE TicketId=23866)


SELECT * FROM Offchain.Movement m
LEFT JOIN [Offchain].[MovementSource] ms 
ON [m].[MovementTransactionId] = [ms].[MovementTransactionId] 
LEFT JOIN [Offchain].[MovementDestination] md 
ON [m].[MovementTransactionId] = [md].[MovementTransactionId]
WHERE ms.SourceNodeId IN (SELECT NodeId FROM [Admin].[NodeTag] WHERE ElementId IN (SELECT CategoryElementId FROM [Admin].[Ticket] WHERE TicketId=23866))
OR md.DestinationNodeId IN (SELECT NodeId FROM [Admin].[NodeTag] WHERE ElementId IN (SELECT CategoryElementId FROM [Admin].[Ticket] WHERE TicketId=23866))

SELECT * FROM Offchain.Ownership
WHERE TicketId IN (SELECT m.TicketId FROM Offchain.Movement m
LEFT JOIN [Offchain].[MovementSource] ms 
ON [m].[MovementTransactionId] = [ms].[MovementTransactionId] 
LEFT JOIN [Offchain].[MovementDestination] md 
ON [m].[MovementTransactionId] = [md].[MovementTransactionId]
WHERE ms.SourceNodeId IN (SELECT NodeId FROM [Admin].[NodeTag] WHERE ElementId IN (SELECT CategoryElementId FROM [Admin].[Ticket] WHERE TicketId=23866))
OR md.DestinationNodeId IN (SELECT NodeId FROM [Admin].[NodeTag] WHERE ElementId IN (SELECT CategoryElementId FROM [Admin].[Ticket] WHERE TicketId=23866)))


 -----------------------------------------------------------------------------------------------------------------------------------

 -- Given two Ticket Id's, 23678 and 23683

SELECT TicketId, CategoryElementId, StartDate, EndDate FROM Admin.Ticket WHERE TicketId=23678
SELECT TicketId, CategoryElementId, StartDate, EndDate FROM Admin.Ticket WHERE TicketId=23683

-- Both have CategoryElementId 10 and similiar StartDate and EndDate

EXEC [Admin].[usp_GetInputMovementPropertyDetails] 23678

EXEC [Admin].[usp_GetInputMovementPropertyDetails] 23683

-- Hence both will give similar results on execution
----------------------------------------------------------------------------------------------
-- When there is a Negative value present in Volume

SELECT TicketId FROM Offchain.Movement WHERE NetStandardVolume < 0

EXEC [Admin].[usp_GetInputMovementPropertyDetails] 23834
----------------------------------------------------------------------------------------
-- When the Date range is not within Ticket StartDate and EndDate

SELECT * FROM Admin.Ticket
 WHERE EndDate < '2019-11-09 23:59:59.000'

 EXEC [Admin].[usp_GetInputMovementPropertyDetails] 23721
  ---------------------------------------------------------------------------