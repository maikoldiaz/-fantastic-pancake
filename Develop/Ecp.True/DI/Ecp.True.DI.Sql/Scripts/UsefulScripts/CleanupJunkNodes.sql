/* Note: The below script may require little changes in some parts based on the data seeded in DB at the time of cleanup.
	The script is intentionally commented out to avoid any execution by mistake duing dacpac deployment.
 */
-- Date: 06-March-2020
-- DELETE script to remove unwanted Nodes
-- Environment: DEV
--------------------------------------------------------------------------------------
/*
SELECT COUNT(1) FROM Admin.Node

DELETE FROM Offchain.Owner WHERE MovementTransactionId IN (SELECT MovementTransactionId FROM Offchain.Movement WHERE ContractId IN (SELECT ContractId FROM Admin.Contract WHERE SourceNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)))

DELETE FROM Offchain.MovementDestination WHERE MovementTransactionId IN (SELECT MovementTransactionId FROM Offchain.Movement WHERE ContractId IN (SELECT ContractId FROM Admin.Contract WHERE SourceNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)))

DELETE FROM Offchain.MovementPeriod WHERE MovementTransactionId IN (SELECT MovementTransactionId FROM Offchain.Movement WHERE ContractId IN (SELECT ContractId FROM Admin.Contract WHERE SourceNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)))

DELETE FROM Offchain.MovementSource WHERE MovementTransactionId IN (SELECT MovementTransactionId FROM Offchain.Movement WHERE ContractId IN (SELECT ContractId FROM Admin.Contract WHERE SourceNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)))

DELETE FROM Offchain.Movement WHERE ContractId IN (SELECT ContractId FROM Admin.Contract WHERE SourceNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918))

DELETE FROM Admin.Contract WHERE SourceNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Admin.Event WHERE DestinationNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Admin.NodeConnectionProduct WHERE NodeConnectionId IN (SELECT NodeConnectionId FROM Admin.NodeConnection WHERE DestinationNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918))

DELETE FROM Admin.NodeConnection WHERE DestinationNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Admin.StorageLocationProductOwner WHERE StorageLocationProductId IN (SELECT StorageLocationProductId FROM Admin.StorageLocationProduct WHERE NodeStorageLocationId IN (SELECT NodeStorageLocationId FROM Admin.NodeStorageLocation WHERE NodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)))

DELETE FROM Admin.StorageLocationProduct WHERE NodeStorageLocationId IN (SELECT NodeStorageLocationId FROM Admin.NodeStorageLocation WHERE NodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918))

DELETE FROM Admin.NodeStorageLocation WHERE NodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Admin.NodeTag WHERE NodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Admin.OwnershipNodeError WHERE OwnershipNodeId IN (SELECT OwnershipNodeId FROM Admin.OwnershipNode WHERE NodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918))

DELETE FROM Admin.OwnershipNode WHERE NodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Offchain.MovementSource WHERE SourceNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Offchain.MovementDestination WHERE DestinationNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Offchain.Unbalance WHERE NodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Admin.Ticket WHERE NodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Admin.UnbalanceComment WHERE NodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Admin.Attribute WHERE InventoryProductId IN (SELECT InventoryProductId FROM Offchain.InventoryProduct WHERE InventoryProductId IN (SELECT InventoryProductId FROM Offchain.Inventory WHERE NodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)))

DELETE FROM Offchain.Owner WHERE InventoryProductId IN (SELECT InventoryProductId FROM Offchain.InventoryProduct WHERE InventoryProductId IN (SELECT InventoryProductId FROM Offchain.Inventory WHERE NodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)))

DELETE FROM Offchain.Ownership WHERE InventoryProductId IN (SELECT InventoryProductId FROM Offchain.InventoryProduct WHERE InventoryProductId IN (SELECT InventoryProductId FROM Offchain.Inventory WHERE NodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)))

DELETE FROM Offchain.InventoryProduct WHERE InventoryProductId IN (SELECT InventoryProductId FROM Offchain.Inventory WHERE NodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918))

DELETE FROM Offchain.Inventory WHERE NodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Admin.Transformation WHERE OriginDestinationNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Admin.Transformation WHERE OriginSourceNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Admin.NodeConnectionProduct WHERE NodeConnectionId IN (SELECT NodeConnectionId FROM Admin.NodeConnection WHERE SourceNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918))

DELETE FROM Admin.NodeConnection WHERE SourceNodeId IN (SELECT NodeId FROM Admin.Node WHERE NodeId < 5918)

DELETE FROM Admin.Node WHERE NodeId < 5918

SELECT COUNT(1) FROM Admin.Node

SELECT * FROM Admin.Node 
ORDER BY CreatedDate DESC

*/