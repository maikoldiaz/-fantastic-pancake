﻿----------------------- TEST SCRIPTS FOR [Admin].[usp_GetConnectionConfigurationDetails] ------------------------------------------------------------------------

 EXEC [Admin].[usp_GetConnectionConfigurationDetails] 23866

  SELECT * FROM Admin.Ticket WHERE TicketId=23866

  SELECT * FROM Admin.NodeTag WHERE ElementId IN (SELECT CategoryElementId FROM Admin.Ticket WHERE TicketId=23866)

  SELECT * FROM Admin.NodeConnection WHERE SourceNodeId IN ( SELECT NodeId FROM Admin.NodeTag WHERE ElementId IN (SELECT CategoryElementId FROM Admin.Ticket WHERE TicketId=23866))
  OR DestinationNodeId IN (SELECT NodeId FROM Admin.NodeTag WHERE ElementId IN (SELECT CategoryElementId FROM Admin.Ticket WHERE TicketId=23866))

  SELECT * FROM [Admin].[NodeConnectionProduct] WHERE NodeConnectionId IN (SELECT NodeConnectionId FROM Admin.NodeConnection WHERE SourceNodeId IN ( SELECT NodeId FROM Admin.NodeTag WHERE ElementId IN (SELECT CategoryElementId FROM Admin.Ticket WHERE TicketId=23866))
  OR DestinationNodeId IN (SELECT NodeId FROM Admin.NodeTag WHERE ElementId IN (SELECT CategoryElementId FROM Admin.Ticket WHERE TicketId=23866)))

  SELECT * FROM [Admin].[NodeConnectionProductOwner] WHERE NodeConnectionProductId IN (SELECT NodeConnectionProductId FROM [Admin].[NodeConnectionProduct] WHERE NodeConnectionId IN (SELECT NodeConnectionId FROM Admin.NodeConnection WHERE SourceNodeId IN ( SELECT NodeId FROM Admin.NodeTag WHERE ElementId IN (SELECT CategoryElementId FROM Admin.Ticket WHERE TicketId=23866))
  OR DestinationNodeId IN (SELECT NodeId FROM Admin.NodeTag WHERE ElementId IN (SELECT CategoryElementId FROM Admin.Ticket WHERE TicketId=23866))))

  ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  -- Given two Ticket Id's, 23863 and 23864

SELECT TicketId, CategoryElementId, StartDate, EndDate FROM Admin.Ticket WHERE TicketId=23863
SELECT TicketId, CategoryElementId, StartDate, EndDate FROM Admin.Ticket WHERE TicketId=23864

-- Both have CategoryElementId 10 and similiar StartDate and EndDate

EXEC [Admin].[usp_GetConnectionConfigurationDetails] 23863

EXEC [Admin].[usp_GetConnectionConfigurationDetails] 23864

-- Hence both will give similar results on execution
----------------------------------------------------------------------------------------------


-- When there is a ticket not of Ownership Type

SELECT TicketId FROM Admin.Ticket WHERE TicketTypeId=1  

EXEC [Admin].[usp_GetConnectionConfigurationDetails] 23684
-------------------------------------------------------------------

-- When the Date range is not within Ticket StartDate and EndDate

SELECT * FROM Admin.Ticket
 WHERE EndDate < '2019-11-09 23:59:59.000'

 EXEC [Admin].[usp_GetConnectionConfigurationDetails] 23721
 ---------------------------------------------------------------------------