----------------------- TEST SCRIPTS FOR [Admin].[usp_GetInventoryDetails] ------------------------------------------------------------------------

EXEC [Admin].[usp_GetInventoryDetails] 23866

SELECT CategoryElementId, StartDate, EndDate FROM [Admin].[Ticket] WHERE TicketId=23866 

SELECT * FROM [Admin].[NodeTag] WHERE ElementId IN (SELECT CategoryElementId FROM [Admin].[Ticket] WHERE TicketId=23866)

SELECT * FROM [Offchain].[Inventory] WHERE NodeId IN (SELECT NodeId FROM [Admin].[NodeTag] WHERE ElementId IN (SELECT CategoryElementId FROM [Admin].[Ticket] WHERE TicketId=23866))

SELECT * FROM [Offchain].[InventoryProduct] WHERE InventoryProductId IN (SELECT InventoryTransactionId FROM [Offchain].[Inventory] WHERE NodeId IN (SELECT NodeId FROM [Admin].[NodeTag] WHERE ElementId IN (SELECT CategoryElementId FROM [Admin].[Ticket] WHERE TicketId=23866)))

SELECT * FROM [Offchain].[Owner] WHERE InventoryProductId IN (SELECT InventoryProductId FROM [Offchain].[InventoryProduct] WHERE InventoryProductId IN (SELECT InventoryTransactionId FROM [Offchain].[Inventory] WHERE NodeId IN (SELECT NodeId FROM [Admin].[NodeTag] WHERE ElementId IN (SELECT CategoryElementId FROM [Admin].[Ticket] WHERE TicketId=23866))))

SELECT * FROM Offchain.InventoryProduct ip
INNER JOIN Offchain.Owner o
  ON ip.InventoryProductId =o.InventoryProductId 
  WHERE ip.InventoryProductId IN (SELECT InventoryTransactionId FROM [Offchain].[Inventory] WHERE NodeId IN (SELECT NodeId FROM [Admin].[NodeTag] WHERE ElementId IN (SELECT CategoryElementId FROM [Admin].[Ticket] WHERE TicketId=23866)) AND InventoryDate BETWEEN '2019-10-19 00:00:00.000' AND '2019-11-27 00:00:00.000')
  AND ProductVolume > 0

  ------------------------------------------------------------------------------------------------
-- Given two Ticket Id's, 23678 and 23683

SELECT TicketId, CategoryElementId, StartDate, EndDate FROM Admin.Ticket WHERE TicketId=23678
SELECT TicketId, CategoryElementId, StartDate, EndDate FROM Admin.Ticket WHERE TicketId=23683

-- Both have CategoryElementId 10 and similiar StartDate and EndDate

EXEC [Admin].[usp_GetInventoryDetails] 23678

EXEC [Admin].[usp_GetInventoryDetails] 23683

-- Hence both will give similar results on execution
----------------------------------------------------------------------------------------------


-- When there is a ticket not of Ownership Type

SELECT TicketId FROM Admin.Ticket WHERE TicketTypeId=1  

EXEC [Admin].[usp_GetInventoryDetails] 23684
-----------------------------------------------------------------------------------------

-- When there is a Negative value present in Volume
SELECT TicketId FROM Offchain.Inventory i INNER JOIN Offchain.InventoryProduct ip
ON i.InventoryTransactionId=ip.InventoryProductId WHERE ProductVolume < 0

EXEC [Admin].[usp_GetInventoryDetails] 23855
----------------------------------------------------------------------------------------
-- When the Date range is not within Ticket StartDate and EndDate

SELECT * FROM Admin.Ticket
 WHERE EndDate < '2019-11-09 23:59:59.000'

 EXEC [Admin].[usp_GetInventoryDetails] 23721
  ---------------------------------------------------------------------------