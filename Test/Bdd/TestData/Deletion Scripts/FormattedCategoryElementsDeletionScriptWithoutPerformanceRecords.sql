/* Note: The below script may require little changes in some parts based on the data seeded in DB at the time of cleanup.
The script is intentionally commented out to avoid any execution by mistake duing dacpac deployment.
*/ 
-- Date: 05-OCT-2020  
-- DELETE script to remove CategoryElements where elementid > 1000 and elementid < 358437  
-- Environment: DEV  
--------------------------------------------------------------------------------------  

/*
DELETE FROM [Admin].[nodeconnectionproductowner] 
WHERE  ( ownerid > 1000 
         AND ownerid < 358437 ); 

DELETE FROM [Admin].[storagelocationproductowner] 
WHERE  storagelocationproductid IN (SELECT storagelocationproductid 
                                    FROM   [Admin].storagelocationproduct 
                                    WHERE  nodestoragelocationid IN 
       (SELECT 
       nodestoragelocationid 
                                 FROM   [Admin].nodestoragelocation 
                                 WHERE 
       storagelocationtypeid > 1000 
       AND storagelocationtypeid < 358437)); 

DELETE FROM [Admin].storagelocationproductvariable 
WHERE  storagelocationproductid IN (SELECT storagelocationproductid 
                                    FROM   [Admin].storagelocationproduct 
                                    WHERE  nodestoragelocationid IN 
       (SELECT 
       nodestoragelocationid 
                                 FROM   [Admin].nodestoragelocation 
                                 WHERE 
       storagelocationtypeid > 1000 
       AND storagelocationtypeid < 358437)); 

DELETE FROM [Admin].storagelocationproduct 
WHERE  nodestoragelocationid IN (SELECT nodestoragelocationid 
                                 FROM   [Admin].nodestoragelocation 
                                 WHERE  storagelocationtypeid > 1000 
                                        AND storagelocationtypeid < 358437); 

DELETE FROM offchain.movementsource 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  segmentid IN 
                                        (SELECT elementid 
                                         FROM   [Admin].[CategoryElement] 
                                         WHERE  elementid > 1000 
                                                AND elementid < 358437) 
                                         OR reasonid IN 
                                            (SELECT elementid 
                                             FROM   [Admin].[CategoryElement] 
                                             WHERE  elementid > 1000 
                                                    AND elementid < 358437)); 

--SELECT COUNT(1) FROM   Offchain.MovementDestination  
--WHERE  MovementTransactionId IN (SELECT MovementTransactionId  
--                                 FROM   Offchain.Movement  
--WHERE  SegmentId IN (SELECT ElementId  
--                     FROM   [Admin].[CategoryElement]  
--                     WHERE  NAME NOT LIKE 'perf%'  
--                            AND ElementId > 1000  
--                            AND ElementId < 358437) 
--OR ReasonId IN (SELECT ElementId  
--                     FROM   [Admin].[CategoryElement]  
--                     WHERE  NAME NOT LIKE 'perf%'  
--                            AND ElementId > 1000  
--                            AND ElementId < 358437)); -- 946 
-- 
--Select count(1) from Offchain.MovementDestination  -- 69947 ---69901 
DELETE FROM offchain.movementdestination 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  segmentid IN 
                                        (SELECT elementid 
                                         FROM   [Admin].[CategoryElement] 
                                         WHERE  elementid > 1000 
                                                AND elementid < 358437) 
                                         OR reasonid IN 
                                            (SELECT elementid 
                                             FROM   [Admin].[CategoryElement] 
                                             WHERE  elementid > 1000 
                                                    AND elementid < 358437)); 

DELETE FROM Admin.Attribute 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  segmentid IN 
                                        (SELECT elementid 
                                         FROM   [Admin].[CategoryElement] 
                                         WHERE  elementid > 1000 
                                                AND elementid < 358437) 
                                         OR reasonid IN 
                                            (SELECT elementid 
                                             FROM   [Admin].[CategoryElement] 
                                             WHERE  elementid > 1000 
                                                    AND elementid < 358437)); 

DELETE FROM offchain.movementperiod 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  segmentid IN 
                                        (SELECT elementid 
                                         FROM   [Admin].[CategoryElement] 
                                         WHERE  elementid > 1000 
                                                AND elementid < 358437) 
                                         OR reasonid IN 
                                            (SELECT elementid 
                                             FROM   [Admin].[CategoryElement] 
                                             WHERE  elementid > 1000 
                                                    AND elementid < 358437)); 

DELETE FROM offchain.owner 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  segmentid IN 
                                        (SELECT elementid 
                                         FROM   [Admin].[CategoryElement] 
                                         WHERE  elementid > 1000 
                                                AND elementid < 358437) 
                                         OR reasonid IN 
                                            (SELECT elementid 
                                             FROM   [Admin].[CategoryElement] 
                                             WHERE  elementid > 1000 
                                                    AND elementid < 358437)); 

DELETE FROM offchain.ownership 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  segmentid IN 
                                        (SELECT elementid 
                                         FROM   [Admin].[CategoryElement] 
                                         WHERE  elementid > 1000 
                                                AND elementid < 358437) 
                                         OR reasonid IN 
                                            (SELECT elementid 
                                             FROM   [Admin].[CategoryElement] 
                                             WHERE  elementid > 1000 
                                                    AND elementid < 358437)); 

DELETE FROM [Admin].ownershipnodeerror 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  segmentid IN 
                                        (SELECT elementid 
                                         FROM   [Admin].[CategoryElement] 
                                         WHERE  elementid > 1000 
                                                AND elementid < 358437) 
                                         OR reasonid IN 
                                            (SELECT elementid 
                                             FROM   [Admin].[CategoryElement] 
                                             WHERE  elementid > 1000 
                                                    AND elementid < 358437)); 

--Select count(*) from Offchain.Movement --70405 
-- 
--SELECT COUNT(*) FROM Offchain.Movement  
--WHERE  SegmentId IN (SELECT ElementId  
--                     FROM   [Admin].[CategoryElement]  
--                     WHERE  NAME NOT LIKE 'perf%'  
--                            AND ElementId > 1000  
--                            AND ElementId < 358437) 
--OR ReasonId IN (SELECT ElementId  
--                     FROM   [Admin].[CategoryElement]  
--                     WHERE  NAME NOT LIKE 'perf%'  
--                            AND ElementId > 1000  
--                            AND ElementId < 358437); --1324 

DELETE FROM Admin.SapTrackingError 
WHERE SapTrackingId IN
(SELECT SapTrackingId FROM [Admin].SapTracking 
WHERE MovementTransactionId IN (
SELECT MovementTransactionId FROM offchain.movement 
WHERE  segmentid IN (SELECT elementid 
                     FROM   [Admin].[CategoryElement] 
                     WHERE  elementid > 1000 
                            AND elementid < 358437) 
        OR reasonid IN (SELECT elementid 
                        FROM   [Admin].[CategoryElement] 
                        WHERE  elementid > 1000 
                               AND elementid < 358437)));

DELETE FROM [Admin].SapTracking 
WHERE MovementTransactionId IN (
SELECT MovementTransactionId FROM offchain.movement 
WHERE  segmentid IN (SELECT elementid 
                     FROM   [Admin].[CategoryElement] 
                     WHERE  elementid > 1000 
                            AND elementid < 358437) 
        OR reasonid IN (SELECT elementid 
                        FROM   [Admin].[CategoryElement] 
                        WHERE  elementid > 1000 
                               AND elementid < 358437));

DELETE FROM [Admin].SapTracking 
WHERE FileRegistrationId IN (SELECT 
       fileregistrationid 
                              FROM   [Admin].fileregistration 
                              WHERE  segmentid > 1000 
                                     AND 
       segmentid < 358437);

DELETE FROM [Admin].DeltaError
WHERE MovementTransactionId IN (
SELECT MovementTransactionId FROM offchain.movement 
WHERE  segmentid IN (SELECT elementid 
                     FROM   [Admin].[CategoryElement] 
                     WHERE  elementid > 1000 
                            AND elementid < 358437) 
        OR reasonid IN (SELECT elementid 
                        FROM   [Admin].[CategoryElement] 
                        WHERE  elementid > 1000 
                               AND elementid < 358437));

DELETE FROM offchain.movement 
WHERE  segmentid IN (SELECT elementid 
                     FROM   [Admin].[CategoryElement] 
                     WHERE  elementid > 1000 
                            AND elementid < 358437) 
        OR reasonid IN (SELECT elementid 
                        FROM   [Admin].[CategoryElement] 
                        WHERE  elementid > 1000 
                               AND elementid < 358437); 

DELETE FROM [Admin].nodestoragelocation 
WHERE  storagelocationtypeid > 1000 
       AND storagelocationtypeid < 358437; 

DELETE FROM [Admin].[nodetag] 
WHERE  elementid > 1000 
       AND elementid < 358437; 

DELETE FROM [Admin].pendingtransactionerror 
WHERE  transactionid IN (SELECT transactionid 
                         FROM   [Admin].pendingtransaction 
                         WHERE  ticketid IN (SELECT ticketid 
                                             FROM   [Admin].ticket 
                                             WHERE  CategoryElementid > 1000 
                                                    AND CategoryElementid < 
                                                        358437)) 

DELETE FROM [Admin].pendingtransaction 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437) 

DELETE FROM Offchain.unbalance 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437) 

DELETE FROM [Admin].unbalancecomment 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437) 

DELETE FROM offchain.owner 
WHERE  inventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  inventoryproductid IN 
                                     (SELECT inventoryproductid 
                                      FROM   offchain.inventoryproduct 
                                      WHERE  ticketid IN (SELECT ticketid 
                                                          FROM   [Admin].ticket 
                                                          WHERE 
                                             CategoryElementid > 
                                             1000 
                                             AND CategoryElementid 
                                                 < 358437) 
                                             )) 

DELETE FROM Admin.Attribute 
WHERE  inventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  inventoryproductid IN 
                                     (SELECT inventoryproductid 
                                      FROM   offchain.inventoryproduct 
                                      WHERE  ticketid IN (SELECT ticketid 
                                                          FROM   [Admin].ticket 
                                                          WHERE 
                                             CategoryElementid > 
                                             1000 
                                             AND CategoryElementid 
                                                 < 358437) 
                                             )) 

DELETE FROM offchain.ownership 
WHERE  inventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  ticketid IN (SELECT ticketid 
                                                  FROM   [Admin].ticket 
                                                  WHERE  CategoryElementid > 
                                                         1000 
                                                         AND CategoryElementid < 
                                                             358437) 
                                    ) 

DELETE FROM [Admin].ownershipnodeerror 
WHERE  inventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  ticketid IN (SELECT ticketid 
                                                  FROM   [Admin].ticket 
                                                  WHERE  CategoryElementid > 
                                                         1000 
                                                         AND CategoryElementid < 
                                                             358437) 
                                    ) 

DELETE FROM offchain.inventoryproduct 
WHERE  inventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  ticketid IN (SELECT ticketid 
                                                  FROM   [Admin].ticket 
                                                  WHERE  CategoryElementid > 
                                                         1000 
                                                         AND CategoryElementid < 
                                                             358437) 
                                    ) 

DELETE FROM offchain.inventoryproduct 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437) 

DELETE FROM [Admin].ownershipnodeerror 
WHERE  ownershipnodeid IN (SELECT ownershipnodeid 
                           FROM   [Admin].ownershipnode 
                           WHERE  ticketid IN (SELECT ticketid 
                                               FROM   [Admin].ticket 
                                               WHERE  CategoryElementid > 1000 
                                                      AND CategoryElementid < 
                                                          358437)) 

DELETE FROM [Admin].ownershipnode 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437) 

DELETE FROM Admin.Attribute 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  segmentid IN 
                                        (SELECT elementid 
                                         FROM   [Admin].[CategoryElement] 
                                         WHERE  elementid > 1000 
                                                AND elementid < 358437) 
                                         OR reasonid IN 
                                            (SELECT elementid 
                                             FROM   [Admin].[CategoryElement] 
                                             WHERE  elementid > 1000 
                                                    AND elementid < 358437)) 

DELETE FROM [Admin].segmentunbalance 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437) 

DELETE FROM [Admin].systemunbalance 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437) 

DELETE FROM [Admin].segmentownershipcalculation 
WHERE  ownershipticketid IN (SELECT ticketid 
                             FROM   [Admin].ticket 
                             WHERE  CategoryElementid > 1000 
                                    AND CategoryElementid < 358437) 

DELETE FROM [Admin].systemownershipcalculation 
WHERE  ownershipticketid IN (SELECT ticketid 
                             FROM   [Admin].ticket 
                             WHERE  CategoryElementid > 1000 
                                    AND CategoryElementid < 358437) 

DELETE FROM offchain.inventoryproduct 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437) 

DELETE FROM [Admin].ownershipcalculation 
WHERE  ownershipticketid IN (SELECT ticketid 
                             FROM   [Admin].ticket 
                             WHERE  CategoryElementid > 1000 
                                    AND CategoryElementid < 358437) 

DELETE FROM offchain.ownership 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437) 

DELETE FROM [Admin].DeltaError 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437) 

DELETE FROM [Admin].SapTrackingError 
WHERE SapTrackingId IN
(SELECT SapTrackingId FROM [Admin].SapTracking 
WHERE  MovementTransactionId IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  segmentid IN 
                                        (SELECT elementid 
                                         FROM   [Admin].[CategoryElement] 
                                         WHERE  elementid > 1000 
                                                AND elementid < 358437) 
                                         OR reasonid IN 
                                            (SELECT elementid 
                                             FROM   [Admin].[CategoryElement] 
                                             WHERE  elementid > 1000 
                                                    AND elementid < 358437)))

DELETE FROM [Admin].SapTracking 
WHERE  MovementTransactionId IN (SELECT movementtransactionid 
                                 FROM   offchain.movement 
                                 WHERE  segmentid IN 
                                        (SELECT elementid 
                                         FROM   [Admin].[CategoryElement] 
                                         WHERE  elementid > 1000 
                                                AND elementid < 358437) 
                                         OR reasonid IN 
                                            (SELECT elementid 
                                             FROM   [Admin].[CategoryElement] 
                                             WHERE  elementid > 1000 
                                                    AND elementid < 358437))

DELETE FROM Offchain.MovementPeriod
WHERE MovementTransactionId IN
(SELECT MovementTransactionId FROM Offchain.Movement 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437)) 

DELETE FROM Offchain.Ownership
WHERE MovementTransactionId IN
(SELECT MovementTransactionId FROM Offchain.Movement 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437)) 

DELETE FROM Offchain.Movement 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437) 

DELETE FROM [Admin].ConsolidatedOwner 
WHERE ConsolidatedMovementId IN (
SELECT ConsolidatedMovementId FROM Admin.ConsolidatedMovement
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437)) 

DELETE FROM [Admin].ConsolidatedMovement 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437) 

DELETE FROM [Admin].ConsolidatedOwner 
WHERE ConsolidatedInventoryProductId IN (
SELECT ConsolidatedInventoryProductId FROM Admin.ConsolidatedInventoryProduct
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437)) 

DELETE FROM [Admin].ConsolidatedInventoryProduct 
WHERE  ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437) 

DELETE FROM Admin.DeltaNode 
WHERE ticketid IN (SELECT ticketid 
                    FROM   [Admin].ticket 
                    WHERE  CategoryElementid > 1000 
                           AND CategoryElementid < 358437) 

DELETE FROM Offchain.InventoryProduct 
WHERE OfficialDeltaTicketId IN
(SELECT TicketId FROM [Admin].Ticket 
WHERE  CategoryElementid > 1000 
       AND CategoryElementid < 358437); 

DELETE FROM Admin.DeltaNodeApprovalHistory 
WHERE TicketId IN
(SELECT TicketId FROM [Admin].Ticket 
WHERE  CategoryElementid > 1000 
       AND CategoryElementid < 358437); 

DELETE FROM [Offchain].Movement
WHERE DeltaTicketId IN
(SELECT TicketId FROM [Admin].ticket 
WHERE  CategoryElementid > 1000 
       AND CategoryElementid < 358437); 

DELETE FROM [Offchain].Movement
WHERE OfficialDeltaTicketId IN
(SELECT TicketId FROM [Admin].ticket 
WHERE  CategoryElementid > 1000 
       AND CategoryElementid < 358437); 

DELETE FROM [Offchain].Owner
Where InventoryProductId IN
(SELECT InventoryProductId FROM Offchain.InventoryProduct
WHERE OfficialDeltaTicketId IN
(SELECT TicketId FROM [Admin].ticket 
WHERE  CategoryElementid > 1000 
       AND CategoryElementid < 358437)); 

DELETE FROM [Offchain].InventoryProduct
WHERE OfficialDeltaTicketId IN
(SELECT TicketId FROM [Admin].ticket 
WHERE  CategoryElementid > 1000 
       AND CategoryElementid < 358437); 

DELETE FROM [Admin].ticket 
WHERE  CategoryElementid > 1000 
       AND CategoryElementid < 358437; 

DELETE FROM Admin.Attribute 
WHERE  inventoryproductid > 100 

DELETE FROM offchain.owner 
WHERE  movementtransactionid IN (SELECT movementtransactionid 
                                 FROM   [Admin].fileregistrationtransaction 
                                 WHERE  fileregistrationid IN 
       (SELECT 
       fileregistrationid 
                              FROM   [Admin].fileregistration 
                              WHERE  segmentid > 1000 
                                     AND 
       segmentid < 358437)) 

DELETE FROM offchain.owner 
WHERE  inventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  fileregistrationtransactionid IN 
                                     (SELECT fileregistrationtransactionid 
                                      FROM   [Admin].fileregistrationtransaction 
                                      WHERE 
              fileregistrationid IN (SELECT fileregistrationid 
                                     FROM   [Admin].fileregistration 
                                     WHERE  segmentid > 1000 
                                            AND segmentid < 358437))) 

DELETE FROM Admin.Attribute 
WHERE  inventoryproductid IN (SELECT inventoryproductid 
                              FROM   offchain.inventoryproduct 
                              WHERE  fileregistrationtransactionid IN 
                                     (SELECT fileregistrationtransactionid 
                                      FROM   [Admin].fileregistrationtransaction 
                                      WHERE 
              fileregistrationid IN (SELECT fileregistrationid 
                                     FROM   [Admin].fileregistration 
                                     WHERE  segmentid > 1000 
                                            AND segmentid < 358437))) 

DELETE FROM offchain.inventoryproduct 
WHERE  fileregistrationtransactionid IN 
       (SELECT fileregistrationtransactionid 
        FROM   [Admin].fileregistrationtransaction 
        WHERE 
              fileregistrationid IN (SELECT fileregistrationid 
                                     FROM   [Admin].fileregistration 
                                     WHERE  segmentid > 1000 
                                            AND segmentid < 358437)) 

DELETE FROM Admin.Attribute 
WHERE  MovementTransactionId IN (
SELECT MovementTransactionId FROM offchain.Movement 
WHERE  fileregistrationtransactionid IN 
       (SELECT fileregistrationtransactionid 
        FROM   [Admin].fileregistrationtransaction 
        WHERE 
              fileregistrationid IN (SELECT fileregistrationid 
                                     FROM   [Admin].fileregistration 
                                     WHERE  segmentid > 1000 
                                            AND segmentid < 358437))) 


DELETE FROM offchain.Movement 
WHERE  fileregistrationtransactionid IN 
       (SELECT fileregistrationtransactionid 
        FROM   [Admin].fileregistrationtransaction 
        WHERE 
              fileregistrationid IN (SELECT fileregistrationid 
                                     FROM   [Admin].fileregistration 
                                     WHERE  segmentid > 1000 
                                            AND segmentid < 358437)) 

DELETE FROM [Admin].fileregistrationtransaction 
WHERE  fileregistrationid IN (SELECT fileregistrationid 
                              FROM   [Admin].fileregistration 
                              WHERE  segmentid > 1000 
                                     AND segmentid < 358437) 

DELETE FROM [Admin].fileregistration 
WHERE  segmentid > 1000 
       AND segmentid < 358437 

DELETE FROM [Admin].fileregistration 
WHERE  segmentid > 1000 
       AND segmentid < 358437 

DELETE FROM [Admin].storagelocationproductowner 
WHERE  storagelocationproductid > 1000 
       AND storagelocationproductid < 358437 

DELETE FROM [Admin].pendingtransactionerror 
WHERE  transactionid IN (SELECT transactionid 
                         FROM   [Admin].pendingtransaction 
                         WHERE  segmentid > 1000 
                                AND segmentid < 358437) 

DELETE FROM [Admin].pendingtransaction 
WHERE  segmentid > 1000 
       AND segmentid < 358437 

DELETE FROM [Admin].ownershipcalculation 
WHERE  ( ownerid > 1000 
         AND ownerid < 358437 ) 

DELETE FROM [Admin].storagelocationproductowner 
WHERE  ownerid IN (SELECT elementid 
                   FROM   [Admin].CategoryElement 
                   WHERE  elementid > 1000 
                          AND elementid < 358437) 

DELETE FROM [Admin].Annulation 
WHERE  sourceMovementTypeId > 1000 
       AND sourceMovementTypeId < 358437 

DELETE FROM [Admin].Annulation 
WHERE  AnnulationMovementTypeId > 1000 
       AND AnnulationMovementTypeId < 358437	

DELETE FROM Admin.Attribute 
WHERE MovementTransactionId IN
(SELECT MovementTransactionId FROM Offchain.Movement
WHERE SystemId > 1000 AND SystemId < 358437)

DELETE FROM Offchain.MovementPeriod 
WHERE MovementTransactionId IN
(SELECT MovementTransactionId FROM Offchain.Movement
WHERE SystemId > 1000 AND SystemId < 358437)

DELETE FROM Admin.SapTracking 
WHERE MovementTransactionId IN
(SELECT MovementTransactionId FROM Offchain.Movement
WHERE SystemId > 1000 AND SystemId < 358437)
	   
DELETE FROM Offchain.Movement
WHERE SystemId > 1000 AND SystemId < 358437

DELETE FROM Offchain.Movement
WHERE SegmentId > 1000 AND SegmentId < 358437

--DELETE FROM Admin.OfficialBalanceExecutionStatus
--WHERE SegmentId > 1000 AND SegmentId < 358437

DELETE FROM [Admin].ReportExecution 
WHERE  ElementId > 1000 
       AND ElementId < 358437 

DELETE FROM [Admin].CategoryElement 
WHERE  ElementId > 1000 
       AND ElementId < 358437 
*/