/* Note: The below script may require little changes in some parts based on the data seeded in DB at the time of cleanup.
  The script is intentionally commented out to avoid any execution by mistake duing dacpac deployment.
*/ 
-- Date: 05-OCT-2020   
-- DELETE script to remove nodes where NodeId < 87691 AND NODEID IN (SELECT NodeId FROM [Admin].Node WHERE Name not like 'perf%')
-- Environment: DEV   
--------------------------------------------------------------------------------------   
--SELECT Count(1) AS NodeCount   
--FROM   [Admin].Node   
  
--SELECT Count(1) AS ElementCount   
--FROM   [Admin].CategoryElement  

/*
DELETE FROM offchain.owner 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  movementcontractid IN 
       (SELECT 
       movementcontractid 
                              FROM   [Admin].contract 
                              WHERE 
       sourcenodeid IN (SELECT nodeid 
                        FROM   [Admin].node 
                        WHERE  nodeid < 87691)) 
                                ) 

DELETE FROM offchain.owner 
WHERE  MovementTransactionId IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  movementcontractid IN 
       (SELECT 
       movementcontractid 
                              FROM   [Admin].contract 
                              WHERE 
       sourcenodeid IN (SELECT nodeid 
                        FROM   [Admin].node 
                        WHERE  nodeid < 87691)) 
                                ) 

DELETE FROM offchain.movementdestination 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  movementcontractid IN 
       (SELECT 
       movementcontractid 
                              FROM   [Admin].contract 
                              WHERE 
       sourcenodeid IN (SELECT nodeid 
                        FROM   [Admin].node 
                        WHERE  nodeid < 87691)) 
                                ) 

DELETE FROM offchain.movementperiod 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  movementcontractid IN 
       (SELECT 
       movementcontractid 
                              FROM   [Admin].contract 
                              WHERE 
       sourcenodeid IN (SELECT nodeid 
                        FROM   [Admin].node 
                        WHERE  nodeid < 87691)) 
                                ) 

DELETE FROM offchain.movementsource 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  movementcontractid IN 
       (SELECT 
       movementcontractid 
                              FROM   [Admin].contract 
                              WHERE 
       sourcenodeid IN (SELECT nodeid 
                        FROM   [Admin].node 
                        WHERE  nodeid < 87691)) 
                                ) 

DELETE FROM offchain.ownership 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  movementcontractid IN 
       (SELECT 
       movementcontractid 
                              FROM   [Admin].contract 
                              WHERE 
       sourcenodeid IN (SELECT nodeid 
                        FROM   [Admin].node 
                        WHERE  nodeid < 87691)) 
                                ) 

DELETE FROM Admin.attribute 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  movementcontractid IN 
       (SELECT 
       movementcontractid 
                              FROM   [Admin].contract 
                              WHERE 
       sourcenodeid IN (SELECT nodeid 
                        FROM   [Admin].node 
                        WHERE  nodeid < 87691)) 
                                ) 

DELETE FROM offchain.movement 
WHERE  movementcontractid IN (SELECT movementcontractid 
                              FROM   [Admin].contract 
                              WHERE  sourcenodeid IN (SELECT nodeid 
                                                      FROM   [Admin].node 
                                                      WHERE  nodeid < 87691)) 

DELETE FROM [Admin].movementcontract 
WHERE  movementcontractid IN (SELECT movementcontractid 
                              FROM   [Admin].contract 
                              WHERE  sourcenodeid IN (SELECT nodeid 
                                                      FROM   [Admin].node 
                                                      WHERE  nodeid < 87691)) 

DELETE FROM [Admin].contract 
WHERE  sourcenodeid IN (SELECT nodeid 
                        FROM   [Admin].node 
                        WHERE  nodeid < 87691) 

DELETE FROM [Admin].event 
WHERE  destinationnodeid IN (SELECT nodeid 
                             FROM   [Admin].node 
                             WHERE  nodeid < 87691) 

DELETE FROM [Admin].nodeconnectionproductowner 
WHERE  nodeconnectionproductid IN (SELECT nodeconnectionproductid 
                                   FROM   [Admin].nodeconnectionproduct 
                                   WHERE  nodeconnectionid IN 
                                          (SELECT nodeconnectionid 
                                           FROM   [Admin].nodeconnection 
                                           WHERE  destinationnodeid IN 
                                                  (SELECT nodeid 
                                                   FROM   [Admin].node 
                                                   WHERE  nodeid < 87691) 
                                          )) 

DELETE FROM [Admin].nodeconnectionproduct 
WHERE  nodeconnectionid IN (SELECT nodeconnectionid 
                            FROM   [Admin].nodeconnection 
                            WHERE  destinationnodeid IN (SELECT nodeid 
                                                         FROM   [Admin].node 
                                                         WHERE  nodeid < 87691)) 

DELETE FROM Offchain.Nodeconnection 
WHERE  NodeConnectionId IN (SELECT NodeConnectionId 
                             FROM [Admin].nodeconnection 
							 WHERE  destinationnodeid IN (SELECT nodeid 
                             FROM   [Admin].node 
                             WHERE  nodeid < 87691)) 

DELETE FROM [Admin].nodeconnection 
WHERE  destinationnodeid IN (SELECT nodeid 
                             FROM   [Admin].node 
                             WHERE  nodeid < 87691) 

DELETE FROM [Admin].storagelocationproductowner 
WHERE  storagelocationproductid IN (SELECT storagelocationproductid 
                                    FROM   [Admin].storagelocationproduct 
                                    WHERE  nodestoragelocationid IN 
       (SELECT 
       nodestoragelocationid 
                                 FROM   [Admin].nodestoragelocation 
                                 WHERE 
       nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691))) 

DELETE FROM [Admin].storagelocationproductvariable 
WHERE  storagelocationproductid IN (SELECT storagelocationproductid 
                                    FROM   [Admin].storagelocationproduct 
                                    WHERE  nodestoragelocationid IN 
       (SELECT 
       nodestoragelocationid 
                                 FROM   [Admin].nodestoragelocation 
                                 WHERE 
       nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691))) 

DELETE FROM [Admin].storagelocationproduct 
WHERE  nodestoragelocationid IN (SELECT nodestoragelocationid 
                                 FROM   [Admin].nodestoragelocation 
                                 WHERE  nodeid IN (SELECT nodeid 
                                                   FROM   [Admin].node 
                                                   WHERE  nodeid < 87691)) 

DELETE FROM Offchain.MovementDestination
WHERE  DestinationStorageLocationId IN (SELECT nodestoragelocationid 
                                 FROM   [Admin].nodestoragelocation 
                                 WHERE  nodeid IN (SELECT nodeid 
                                                   FROM   [Admin].node 
                                                   WHERE  nodeid < 87691)) 

DELETE FROM Offchain.MovementSource
WHERE  SourceStorageLocationId IN (SELECT nodestoragelocationid 
                                 FROM   [Admin].nodestoragelocation 
                                 WHERE  nodeid IN (SELECT nodeid 
                                                   FROM   [Admin].node 
                                                   WHERE  nodeid < 87691)) 

DELETE FROM [Admin].nodestoragelocation 
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691) 

DELETE FROM [Admin].nodetag 
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691) 

DELETE FROM [Admin].ownershipnodeerror 
WHERE  ownershipnodeid IN (SELECT ownershipnodeid 
                           FROM   [Admin].ownershipnode 
                           WHERE  nodeid IN (SELECT nodeid 
                                             FROM   [Admin].node 
                                             WHERE  nodeid < 87691)) 

DELETE FROM [Admin].ownershipnode 
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691) 

DELETE FROM offchain.movementsource 
WHERE  sourcenodeid IN (SELECT nodeid 
                        FROM   [Admin].node 
                        WHERE  nodeid < 87691) 

DELETE FROM offchain.movementdestination 
WHERE  destinationnodeid IN (SELECT nodeid 
                             FROM   [Admin].node 
                             WHERE  nodeid < 87691) 

DELETE FROM [Offchain].unbalance 
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691) 

DELETE FROM [Admin].ConsolidatedOwner 
WHERE ConsolidatedInventoryProductId IN
(SELECT ConsolidatedInventoryProductId FROM 
[Admin].ConsolidatedInventoryProduct WHERE NodeId < 87691)

DELETE FROM [Admin].ConsolidatedInventoryProduct 
WHERE TicketId In (SELECT TicketId FROM Admin.Ticket
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691)) 

DELETE FROM OFFCHAIN.MOVEMENT WHERE MovementTransactionId IN
(SELECT M.MovementTransactionId FROM Offchain.Movement M
JOIN Offchain.MovementSource MS ON M.MovementTransactionId = MS.MovementTransactionId
JOIN Offchain.MovementDestination MD ON M.MovementTransactionId = MD.MovementTransactionId
WHERE MS.SourceNodeId < 87691 OR MD.DestinationNodeId < 87691)

DELETE FROM Offchain.Owner
WHERE MovementTransactionId IN (SELECT MovementTransactionId FROM OFFCHAIN.MOVEMENT WHERE MovementTransactionId IN
(SELECT M.MovementTransactionId FROM Offchain.Movement M
JOIN Offchain.MovementSource MS ON M.MovementTransactionId = MS.MovementTransactionId
JOIN Offchain.MovementDestination MD ON M.MovementTransactionId = MD.MovementTransactionId
WHERE MS.SourceNodeId < 87691 OR MD.DestinationNodeId < 87691))

DELETE FROM Offchain.Movement
WHERE MovementTransactionId IN
(SELECT M.MovementTransactionId FROM Offchain.Movement M
JOIN Offchain.MovementSource MS ON M.MovementTransactionId = MS.MovementTransactionId
JOIN Offchain.MovementDestination MD ON M.MovementTransactionId = MD.MovementTransactionId
WHERE MS.SourceNodeId < 87691 OR MD.DestinationNodeId < 87691)

DELETE FROM Admin.PendingTransaction
WHERE TicketId IN (SELECT TicketId From Admin.Ticket
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691))

DELETE FROM Admin.DeltaNode
WHERE TicketId IN (SELECT TicketId From Admin.Ticket
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691))

DELETE FROM Offchain.Owner
WHERE MovementTransactionId IN
(SELECT MovementTransactionId FROM Offchain.Movement
WHERE TicketId IN (SELECT TicketId From Admin.Ticket
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691)))

DELETE FROM Offchain.Ownership
WHERE MovementTransactionId IN
(SELECT MovementTransactionId FROM Offchain.Movement
WHERE TicketId IN (SELECT TicketId From Admin.Ticket
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691)))

DELETE FROM Offchain.MovementPeriod
WHERE MovementTransactionId IN
(SELECT MovementTransactionId FROM Offchain.Movement
WHERE TicketId IN (SELECT TicketId From Admin.Ticket
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691)))

DELETE FROM Admin.OwnershipResult
WHERE MovementTransactionId IN
(SELECT MovementTransactionId FROM Offchain.Movement
WHERE TicketId IN (SELECT TicketId From Admin.Ticket
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691)))

DELETE FROM Offchain.Movement
WHERE TicketId IN (SELECT TicketId From Admin.Ticket
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691))

DELETE FROM Admin.OwnershipCalculationResult
WHERE OwnershipCalculationId In
(SELECT OwnershipCalculationId FROM Admin.OwnershipCalculation
WHERE OwnershipTicketId IN (SELECT TicketId From Admin.Ticket
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691)))

DELETE FROM Admin.OwnershipCalculation
WHERE OwnershipTicketId IN (SELECT TicketId From Admin.Ticket
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691))

DELETE FROM Admin.ConsolidatedOwner
WHERE ConsolidatedMovementId IN
(SELECT ConsolidatedMovementId FROM [Admin].ConsolidatedMovement 
WHERE TicketId IN (SELECT TicketId From Admin.Ticket
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691)))

DELETE FROM [Admin].ConsolidatedMovement 
WHERE TicketId IN (SELECT TicketId From Admin.Ticket
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691))

DELETE FROM Admin.ConsolidatedOwner
WHERE ConsolidatedInventoryProductId IN
(SELECT ConsolidatedInventoryProductId FROM [Admin].ConsolidatedInventoryProduct 
WHERE TicketId IN (SELECT TicketId From Admin.Ticket
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691)))

DELETE FROM [Admin].ConsolidatedInventoryProduct 
WHERE TicketId IN (SELECT TicketId From Admin.Ticket
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691))

DELETE FROM [Admin].Ticket 
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691) 

DELETE FROM [Admin].unbalancecomment 
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691) 

DELETE FROM Admin.attribute 
WHERE  inventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  inventoryproductid IN 
                                     (SELECT inventoryproductid 
                                      FROM   offchain.inventoryproduct 
                                      WHERE  nodeid IN (SELECT nodeid 
                                                        FROM   [Admin].node 
                                                        WHERE  nodeid < 87691))) 

DELETE FROM offchain.owner 
WHERE  inventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  inventoryproductid IN 
                                     (SELECT inventoryproductid 
                                      FROM   offchain.inventoryproduct 
                                      WHERE  nodeid IN (SELECT nodeid 
                                                        FROM   [Admin].node 
                                                        WHERE  nodeid < 87691))) 

DELETE FROM offchain.ownership 
WHERE  inventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  inventoryproductid IN 
                                     (SELECT inventoryproductid 
                                      FROM   offchain.inventoryproduct 
                                      WHERE  nodeid IN (SELECT nodeid 
                                                        FROM   [Admin].node 
                                                        WHERE  nodeid < 87691))) 

DELETE FROM [Admin].DeltaError 
WHERE  InventoryProductId IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  nodeid IN (SELECT nodeid 
                                                FROM   [Admin].node 
                                                WHERE  nodeid < 87691)) 

DELETE FROM Offchain.Owner where MovementTransactionId IN
(SELECT MovementTransactionId FROM offchain.Movement 
WHERE  Sourceinventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  nodeid IN (SELECT nodeid 
                                                FROM   [Admin].node 
                                                WHERE  nodeid < 87691))) 

DELETE FROM Offchain.MovementPeriod where MovementTransactionId IN
(SELECT movementtransactionid FROM offchain.Movement 
WHERE  Sourceinventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  nodeid IN (SELECT nodeid 
                                                FROM   [Admin].node 
                                                WHERE  nodeid < 87691))) 

DELETE FROM offchain.Ownership
WHERE MovementTransactionId In (SELECT MovementTransactionId 
FROM Offchain.Movement WHERE Sourceinventoryproductid 
IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  nodeid IN (SELECT nodeid 
                                                FROM   [Admin].node 
                                                WHERE  nodeid < 87691))) 

DELETE FROM Offchain.Owner
WHERE MovementTransactionId IN
(SELECT MovementTransactionId FROM Offchain.Movement
WHERE SourceMovementTransactionId IN
(SELECT MovementTransactionId FROM offchain.Movement 
WHERE  Sourceinventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  nodeid IN (SELECT nodeid 
                                                FROM   [Admin].node 
                                                WHERE  nodeid < 87691))))

DELETE FROM Offchain.MovementPeriod
WHERE MovementTransactionId IN
(SELECT MovementTransactionId FROM Offchain.Movement
WHERE SourceMovementTransactionId IN
(SELECT MovementTransactionId FROM offchain.Movement 
WHERE  Sourceinventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  nodeid IN (SELECT nodeid 
                                                FROM   [Admin].node 
                                                WHERE  nodeid < 87691))))

DELETE FROM Offchain.Movement
WHERE SourceMovementTransactionId IN
(SELECT MovementTransactionId FROM offchain.Movement 
WHERE  Sourceinventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  nodeid IN (SELECT nodeid 
                                                FROM   [Admin].node 
                                                WHERE  nodeid < 87691)))

DELETE FROM offchain.Movement 
WHERE  Sourceinventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  nodeid IN (SELECT nodeid 
                                                FROM   [Admin].node 
                                                WHERE  nodeid < 87691)) 

DELETE FROM Admin.DeltaNodeError WHERE InventoryProductId IN
(SELECT InventoryProductId FROM Offchain.InventoryProduct 
WHERE  inventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  nodeid IN (SELECT nodeid 
                                                FROM   [Admin].node 
                                                WHERE  nodeid < 87691))) 

DELETE FROM offchain.inventoryproduct 
WHERE  nodeid IN (SELECT nodeid 
                  FROM   [Admin].node 
                  WHERE  nodeid < 87691) 

DELETE FROM [Admin].transformation 
WHERE  origindestinationnodeid IN (SELECT nodeid 
                                   FROM   [Admin].node 
                                   WHERE  nodeid < 87691)
OR
originsourcenodeid IN (SELECT nodeid 
                              FROM   [Admin].node 
                              WHERE  nodeid < 87691) 

DELETE FROM [Admin].NodeConnectionProductOwner where NodeConnectionProductId In
(SELECT NodeConnectionProductId FROM [Admin].nodeconnectionproduct 
WHERE  nodeconnectionid IN (SELECT nodeconnectionid 
                            FROM   [Admin].nodeconnection 
                            WHERE  sourcenodeid IN (SELECT nodeid 
                                                    FROM   [Admin].node 
                                                    WHERE  nodeid < 87691))) 


DELETE FROM [Admin].nodeconnectionproduct 
WHERE  nodeconnectionid IN (SELECT nodeconnectionid 
                            FROM   [Admin].nodeconnection 
                            WHERE  sourcenodeid IN (SELECT nodeid 
                                                    FROM   [Admin].node 
                                                    WHERE  nodeid < 87691)) 

DELETE FROM [Offchain].NOdeconnection 
WHERE NodeConnectionId IN
(SELECT NodeConnectionId FROM Admin.NodeConnection
WHERE  sourcenodeid IN (SELECT nodeid 
                        FROM   [Admin].node 
                        WHERE  nodeid < 87691) 
	OR destinationnodeid IN (SELECT nodeid 
                        FROM   [Admin].node 
                        WHERE  nodeid < 87691)) 

DELETE FROM [Admin].nodeconnection 
WHERE  sourcenodeid IN (SELECT nodeid 
                        FROM   [Admin].node 
                        WHERE  nodeid < 87691) 

DELETE FROM [Admin].ownershipcalculation 
WHERE  nodeid < 87691 

DELETE FROM offchain.obsolete_inventory 
WHERE  nodeid < 87691 

DELETE FROM offchain.ownership 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE 
       movementeventid IN (SELECT movementeventid 
                           FROM   [Admin].movementevent 
                           WHERE  sourcenodeid < 87691 
                          )) 

DELETE FROM offchain.movement 
WHERE  movementeventid IN (SELECT movementeventid 
                           FROM   [Admin].movementevent 
                           WHERE  sourcenodeid < 87691) 

DELETE FROM [Admin].movementevent 
WHERE  sourcenodeid < 87691 

DELETE FROM Offchain.node 
WHERE  nodeid < 87691 

DELETE FROM [Admin].DeltaNodeError 
WHERE  DeltaNodeId < 87691 

DELETE FROM [Admin].DeltaNode 
WHERE  nodeid < 87691 

--DELETE FROM [Admin].OfficialBalanceExecutionStatus 
--WHERE NodeId < 87691

DELETE FROM Offchain.Owner
WHERE MovementTransactionId IN 
(SELECT movementtransactionid FROM offchain.MovementSource 
WHERE  SourceNodeId < 87691) 

DELETE FROM Offchain.Owner
WHERE MovementTransactionId IN 
(SELECT movementtransactionid FROM offchain.MovementDestination 
WHERE  DestinationNodeId < 87691) 

DELETE FROM [Admin].ConsolidatedOwner 
WHERE ConsolidatedMovementId IN
(SELECT ConsolidatedMovementId FROM [Admin].ConsolidatedMovement 
WHERE SourceNodeId < 87691 OR DestinationNodeId < 87691)

DELETE FROM OFFCHAIN.MovementDestination WHERE 
 MovementTransactionId In (
SELECT MovementTransactionId FROM Admin.ConsolidatedMovement
WHERE SourceNodeId < 87691 OR DestinationNodeId < 87691)

DELETE FROM OFFCHAIN.MovementSource WHERE 
 MovementTransactionId In (
SELECT MovementTransactionId FROM Admin.ConsolidatedMovement
WHERE SourceNodeId < 87691 OR DestinationNodeId < 87691)

DELETE FROM Offchain.MovementPeriod
WHERE MovementTransactionId IN
(SELECT M.MovementTransactionId FROM OFFCHAIN.MOVEMENT M 
JOIN Offchain.MovementSource MS ON M.MovementTransactionId = MS.MovementTransactionId
JOIN Offchain.MovementDestination MD ON M.MovementTransactionId = MD.MovementTransactionId
WHERE MS.SourceNodeId < 87691 OR MD.DestinationNodeId < 87691)

DELETE FROM Offchain.Owner
WHERE MovementTransactionId IN
(SELECT MovementTransactionId FROM OFFCHAIN.MOVEMENT WHERE 
ConsolidatedMovementTransactionId In (
SELECT ConsolidatedMovementTransactionId FROM [Admin].ConsolidatedMovement 
WHERE SourceNodeId < 87691 OR DestinationNodeId < 87691))

DELETE FROM Offchain.MovementPeriod
WHERE MovementTransactionId IN
(SELECT MovementTransactionId FROM OFFCHAIN.MOVEMENT WHERE 
ConsolidatedMovementTransactionId In (
SELECT ConsolidatedMovementTransactionId FROM [Admin].ConsolidatedMovement 
WHERE SourceNodeId < 87691 OR DestinationNodeId < 87691))

DELETE FROM Offchain.Owner
where MovementTransactionId IN
(SELECT MovementTransactionId FROM Offchain.Movement
WHERE SourceMovementTransactionId IN
(SELECT MovementTransactionId FROM OFFCHAIN.MOVEMENT WHERE 
ConsolidatedMovementTransactionId In (
SELECT ConsolidatedMovementTransactionId FROM [Admin].ConsolidatedMovement 
WHERE SourceNodeId < 87691 OR DestinationNodeId < 87691)))

DELETE FROM Offchain.MovementPeriod
where MovementTransactionId IN
(SELECT MovementTransactionId FROM Offchain.Movement
WHERE SourceMovementTransactionId IN
(SELECT MovementTransactionId FROM OFFCHAIN.MOVEMENT WHERE 
ConsolidatedMovementTransactionId In (
SELECT ConsolidatedMovementTransactionId FROM [Admin].ConsolidatedMovement 
WHERE SourceNodeId < 87691 OR DestinationNodeId < 87691)))

DELETE FROM Offchain.Movement
WHERE SourceMovementTransactionId IN
(SELECT MovementTransactionId FROM OFFCHAIN.MOVEMENT WHERE 
ConsolidatedMovementTransactionId In (
SELECT ConsolidatedMovementTransactionId FROM [Admin].ConsolidatedMovement 
WHERE SourceNodeId < 87691 OR DestinationNodeId < 87691))

DELETE FROM OFFCHAIN.MOVEMENT WHERE 
ConsolidatedMovementTransactionId In (
SELECT ConsolidatedMovementTransactionId FROM [Admin].ConsolidatedMovement 
WHERE SourceNodeId < 87691 OR DestinationNodeId < 87691)

DELETE from OFFCHAIN.MovementPeriod
WHERE MovementTransactionId In (
SELECT MovementTransactionId FROM OFFCHAIN.MOVEMENT WHERE 
ConsolidatedInventoryProductId In (
SELECT ConsolidatedInventoryProductId FROM [Admin].ConsolidatedInventoryProduct 
WHERE NodeId < 87691))

DELETE from OFFCHAIN.Owner
WHERE MovementTransactionId In (
SELECT MovementTransactionId FROM OFFCHAIN.MOVEMENT WHERE 
ConsolidatedInventoryProductId In (
SELECT ConsolidatedInventoryProductId FROM [Admin].ConsolidatedInventoryProduct 
WHERE NodeId < 87691))

DELETE FROM OFFCHAIN.MOVEMENT WHERE 
ConsolidatedInventoryProductId In (
SELECT ConsolidatedInventoryProductId FROM [Admin].ConsolidatedInventoryProduct 
WHERE NodeId < 87691)

DELETE FROM [Admin].ConsolidatedMovement 
WHERE SourceNodeId < 87691 OR DestinationNodeId < 87691

DELETE FROM [Admin].ConsolidatedOwner 
WHERE ConsolidatedInventoryProductId IN
(SELECT ConsolidatedInventoryProductId FROM 
[Admin].ConsolidatedInventoryProduct WHERE NodeId < 87691)

DELETE FROM [Admin].ConsolidatedInventoryProduct 
WHERE NodeId < 87691

DELETE FROM [Admin].ReportExecution 
WHERE  nodeid < 87691 

DELETE FROM [Admin].DeltaNodeApproval 
WHERE  nodeid < 87691 

DELETE FROM [Admin].Node 
WHERE  nodeid < 87691 

*/
--Select Top 50 * from [Admin].Node order by NodeId desc -- 87691
--Select Top 50 * from [Admin].CategoryElement order by ElementId desc -- 302760

--Select Count(*) as NodeCount from Admin.Node
--Select Count(*) as ElementCount from Admin.CategoryElement