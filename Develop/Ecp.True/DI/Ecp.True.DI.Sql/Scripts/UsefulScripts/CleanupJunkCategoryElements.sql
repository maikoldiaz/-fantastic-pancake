/* Note: The below script may require little changes in some parts based on the data seeded in DB at the time of cleanup.
	The script is intentionally commented out to avoid any execution by mistake duing dacpac deployment.
 */
--------------------------------------------------------------------------------------
/*

Delete from [Admin].[NodeConnectionProductOwner] WHERE (OWNERID > 1000 )

DELETE FROM [Admin].[StorageLocationProductOwner] WHERE StorageLocationProductId IN (Select StorageLocationProductId from Admin.StorageLocationProduct 
		where NodeStorageLocationId IN (Select NodeStorageLocationId From Admin.NodeStorageLocation where StorageLocationTypeId > 1000))

Delete from Admin.StorageLocationProduct where NodeStorageLocationId IN (Select NodeStorageLocationId From Admin.NodeStorageLocation where StorageLocationTypeId > 1000)


Delete from Admin.MovementSource where SourceStorageLocationId IN (
Select SourceStorageLocationId From Admin.NodeStorageLocation where StorageLocationTypeId > 1000
)

Delete from Admin.MovementDestination where DestinationStorageLocationId IN (
Select DestinationStorageLocationId From Admin.NodeStorageLocation where StorageLocationTypeId > 1000
)

Delete from Offchain.MovementSource where SourceStorageLocationId IN (
Select SourceStorageLocationId From Admin.NodeStorageLocation where StorageLocationTypeId > 1000
)

Delete from Offchain.MovementDestination where DestinationStorageLocationId IN (
Select DestinationStorageLocationId From Admin.NodeStorageLocation where StorageLocationTypeId > 1000
)


Delete From Admin.NodeStorageLocation where StorageLocationTypeId > 1000



DELETE FROM [Admin].[NodeTag] where ElementId > 1000

Delete from Admin.PendingTransactionError where TransactionId IN (Select TransactionId from Admin.PendingTransaction where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000))

Delete from Admin.PendingTransaction where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)

Delete From Offchain.Unbalance where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)

Delete From Admin.UnbalanceComment where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)

Delete from Admin.MovementDestination where MovementTransactionId IN (
Select MovementTransactionId From Admin.Movement where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
)

Delete from Admin.MovementSource where MovementTransactionId IN (
Select MovementTransactionId From Admin.Movement where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
)

Delete from Admin.MovementPeriod where MovementTransactionId IN (
Select MovementTransactionId From Admin.Movement where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
)

Delete from Offchain.MovementDestination where MovementTransactionId IN (
Select MovementTransactionId From Offchain.Movement where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
)

Delete from Offchain.MovementSource where MovementTransactionId IN (
Select MovementTransactionId From Offchain.Movement where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
)

Delete from Offchain.MovementPeriod where MovementTransactionId IN (
Select MovementTransactionId From Offchain.Movement where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
)

Delete from Admin.Owner where MovementTransactionId IN (
Select MovementTransactionId From Admin.Movement where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
)

Delete from Admin.Attribute where MovementTransactionId IN (
Select MovementTransactionId From Admin.Movement where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
)

Delete From Admin.Movement where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
Delete From Offchain.Movement where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
----------------------------------------------------------------

Delete from Admin.Owner where InventoryProductId IN (
Select InventoryProductId From Admin.InventoryProduct where InventoryProductId IN (
	Select InventoryTransactionId from Admin.Inventory where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
))

Delete from Admin.Attribute where InventoryProductId IN (
Select InventoryProductId From Admin.InventoryProduct where InventoryProductId IN (
	Select InventoryTransactionId from Admin.Inventory where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
))

Delete from Admin.InventoryProduct Where InventoryProductId IN (
	Select InventoryTransactionId from Admin.Inventory where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
)

Delete from  Admin.Inventory where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)




Delete from Offchain.Owner where InventoryProductId IN (
Select InventoryProductId From Offchain.InventoryProduct where InventoryProductId IN (
	Select InventoryTransactionId from Offchain.Inventory where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
))

Delete from Admin.Attribute where InventoryProductId IN (
Select InventoryProductId From Offchain.InventoryProduct where InventoryProductId IN (
	Select InventoryTransactionId from Offchain.Inventory where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
))

Delete from Offchain.InventoryProduct Where InventoryProductId IN (
	Select InventoryTransactionId from Offchain.Inventory where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
)

Delete from  Offchain.Inventory where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
------------------------------------------------------------------------------------------

Delete from Admin.OwnershipCalculation Where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)

Delete from Admin.OwnershipNodeError Where OwnershipNodeId IN (
	Select OwnershipNodeId from Admin.OwnershipNode where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
)

Delete from Admin.OwnershipNode where TicketId IN (Select TicketId from Admin.Ticket where CategoryElementId > 1000)
--------------------------------------------------------------
Delete from Admin.Ticket where CategoryElementId > 1000



Delete from Offchain.MovementDestination where MovementTransactionId IN (Select MovementTransactionId from Admin.FileRegistrationTransaction Where FileRegistrationId IN 
		(Select FileRegistrationId from Admin.FileRegistration Where SegmentId > 1000))

Delete from Offchain.MovementSource where MovementTransactionId IN (Select MovementTransactionId from Admin.FileRegistrationTransaction Where FileRegistrationId IN 
		(Select FileRegistrationId from Admin.FileRegistration Where SegmentId > 1000))

Delete from Offchain.MovementPeriod where MovementTransactionId IN (Select MovementTransactionId from Admin.FileRegistrationTransaction Where FileRegistrationId IN 
		(Select FileRegistrationId from Admin.FileRegistration Where SegmentId > 1000))


Delete from Admin.Attribute where MovementTransactionId IN (
Select MovementTransactionId From Offchain.Movement where SegmentId > 1000)

--Select * from Admin.Attribute
Delete from Admin.Attribute where InventoryProductId > 100


Delete from Offchain.Owner


Delete from Offchain.Movement where MovementTransactionId IN (Select MovementTransactionId from Admin.FileRegistrationTransaction Where FileRegistrationId IN 
		(Select FileRegistrationId from Admin.FileRegistration Where SegmentId > 1000))


Delete from Offchain.Owner where MovementTransactionId IN (Select MovementTransactionId from Admin.FileRegistrationTransaction Where FileRegistrationId IN 
		(Select FileRegistrationId from Admin.FileRegistration Where SegmentId > 1000)) 



Delete from Offchain.Movement where FileRegistrationTransactionId IN (Select FileRegistrationTransactionId from Admin.FileRegistrationTransaction Where FileRegistrationId IN 
		(Select FileRegistrationId from Admin.FileRegistration Where SegmentId > 1000))



Select * from Admin.FileRegistrationTransaction
	Delete from Admin.FileRegistrationTransaction where FileRegistrationId IN (Select FileRegistrationId from Admin.FileRegistration Where SegmentId > 1000)

	Delete from Admin.FileRegistration Where SegmentId > 1000

Delete from Offchain.InventoryProduct where InventoryProductId IN (
Select InventoryProductId from Offchain.Inventory where 
FileRegistrationTransactionId IN (Select FileRegistrationTransactionId from Admin.FileRegistrationTransaction where FileRegistrationId IN (Select FileRegistrationId from Admin.FileRegistration Where SegmentId > 1000))
)










Select * from Offchain.Inventory
Delete from Offchain.Inventory where FileRegistrationTransactionId IN (
	Select FileRegistrationTransactionId from Admin.FileRegistrationTransaction where FileRegistrationId 
	IN (Select FileRegistrationId from Admin.FileRegistration where SegmentId > 1000))



Select * from Admin.FileRegistration where SegmentId > 1000

Delete from Admin.FileRegistrationTransaction where FileRegistrationId IN (Select FileRegistrationId from Admin.FileRegistration where SegmentId > 1000)






Delete from Admin.FileRegistrationTransaction Where FileRegistrationId IN (Select FileRegistrationId from Admin.FileRegistration Where SegmentId > 1000)



Delete from Admin.FileRegistration Where SegmentId > 1000



Delete from Admin.StorageLocationProductOwner where StorageLocationProductId > 1000

Delete from Admin.StorageLocationProduct where OwnershipRuleId IS NOT NULL

Delete from Admin.CategoryElement where ElementId > 100


Delete from Admin.Category where Name like '%Automation%'

Select * from Admin.Category
Select * from Admin.CategoryElement

*/