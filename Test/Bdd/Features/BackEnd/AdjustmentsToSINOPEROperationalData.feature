@sharedsteps= @owner=jagudelos @backend @testplan=39221 @testsuite=39235
Feature: AdjustmentsToSinoperOperationalData
As a Balance Segment Professional User,
I need to make adjustments to the movement
and inventory load to allow the recording
of all SINOPER operational data

Background: Login
Given I am authenticated as "profesional"

@testcase=41349 @bvt
Scenario: Register an Inventory with valid product,node,tank,inventory date and batch identifier
And I want to register an "Inventory" in the system with unique batch identifier
When it meets "all" input validations
And the "EventType" field is equal to "Insert"
Then it should be registered

@testcase=41350 @bvt @version=2
Scenario: Verify that Inventory registration failed for second product when same batch identifier is sent to two commodities
	And I want to register an "Inventory" in the system with same batch identifier
	And it not met batch identifier validation
	When it meets "all" input validations
	And the "EventType" field is equal to "Insert"
	Then first inventory product should be registered in the system
	And second inventory product must be stored in a Pendingtransactions repository with validation "El identificador del inventario ya existe en el sistema"

@testcase=41351 @bvt @version=2
Scenario: Delete an existing Inventory with valid product,node,tank,inventory date and batch identifier
	And I want to cancel an "Inventory"
	And I provide "EventType" field is equal to "Delete" with unique batch identifier
	Then record a new "Inventory" with negative values for the "ProductVolume"
	And it should be registered

@testcase=41352 @bvt
Scenario: Delete an existing Inventory when the Inventory Identifier does not exist
And I want to cancel an "Inventory"
When the 'InventoryIdentifier' does not exist and "EventType" field is equal to "Delete"
Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a anular no existe"

@testcase=41353
Scenario: Verify that Inventory deletion failed for second product when same batch identifier is sent to two commodities
And I want to cancel an "Inventory" in the system with same batch identifier
And I provide "EventType" field is equal to "Delete"
Then record a new "Inventory" with negative values for the "ProductVolume"
And first inventory product should be registered in the system
Then second inventory product must be stored in a Pendingtransactions repository with validation "El identificador del inventario a anular no existe"

@testcase=41354
Scenario: Register an Inventory with batch identifier that exceeds 25 characters
And I provide batch identifier that exceeds 25 characters for inventory
And I want to register an "Inventory" in the system
When it meets "all" input validations
Then it must be stored in a Pendingtransactions repository with validation "El identificador del batch puede contener máximo 25 caracteres"

@testcase=41355 @bvt
Scenario: Update an existing Inventory with valid data
	And I want to adjust an "Inventory"
	And I provide "EventType" field is equal to "Update" with unique batch identifier
	Then record a new "Inventory" with negative values for the "ProductVolume"
	And record a new Inventory with positive product volume

@testcase=41356
Scenario: Update an existing Inventory with LocationType as Tanque
	And I want to adjust an 'Inventory' with "LocationType" as "Tanque"
	And I provide "EventType" field is equal to "Update" with unique batch identifier
	Then record a new "Inventory" with negative values for the "ProductVolume"
	And record a new Inventory with positive product volume

@testcase=41357 @version=2
Scenario: Update an existing Inventory and add new product
	And I want to adjust an 'Inventory' with "LocationType" as "Tanque"
	And I provide "EventType" field is equal to 'Update' and add new batch identifier
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a ajustar no existe"

@testcase=41358 @version=2
Scenario: Verify that Inventory update should be succeeded when same batch identifier is sent to two commodities
	And I want to adjust an "Inventory" with same batch identifier
	And I provide "EventType" field is equal to "Update"
	Then record a new "Inventory" with negative values for the "ProductVolume"
	And record a new Inventory with positive product volume
	And updated should be successful for second product

@testcase=41359
Scenario: Update an existing Inventory when the Inventory Identifier does not exist
And I want to adjust an "Inventory"
When the 'InventoryIdentifier' does not exist and "EventType" field is equal to "Update"
Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a ajustar no existe"

@testcase=41360 @ui @version=2
Scenario: Verify inventories with the batch identifier are included in the Final inventory calculation
	And I have an "Inventory" and "Movements" in the system
	When there are inventories with the batch identifier on the period day of the operational cutoff
	And the system is executing the operational cutoff
	Then created inventories should be included in final inventory calculation

@testcase=41361 @ui @version=2
Scenario: Verify inventories with the batch identifier are included in the Initial inventory calculation
	And I have an "Inventory" and "Movements" in the system
	When there are inventories with the batch identifier on the day before the period day of the operational cutoff
	And the system is executing the operational cutoff
	Then created inventories should be included in initial inventory calculation

@testcase=41362 @ui @manual @version=2
Scenario: Verify the inventories with the batch identifier are included in inventories collection that will be sent to FICO
	And I have an "Inventory" and "Movements" in the system
	When there are inventories with the batch identifier on the period day of the ownership calculation
	And the system is executing the ownership calculation
	Then created inventory should be included in inventories collection that will be sent to FICO

@testcase=41363 @ui @manual @version=2
Scenario: Verify the inventories with the batch identifier are included in Initial inventories collection that will be sent to FICO
	And I have an "Inventory" and "Movements" in the system
	When there are inventories with the batch identifier on the day before the period day of the ownership calculation
	And the system is executing the ownership calculation
	Then created inventory should be included in initial inventories collection that will be sent to FICO

@testcase=41364 @bvt
Scenario: Verify the SINOPER processed movement which has a destination product
And I want to register an "Movements" in the system
When it meets "all" input validations
And the "EventType" field is equal to "Insert"
Then it should be registered
And product should be stored in the destination product
And product type should be stored in the destination producttype

@testcase=41365
Scenario: Verify the SINOPER processed movement does not have destination product
And I want to register Movements in the system without destination product
When it meets "all" input validations
And the "EventType" field is equal to "Insert"
Then it should be registered
And source product should be stored in the destination product
And source product type should be stored in the destination producttype

@testcase=41366 @bvt
Scenario: Verify multiple inventories for the same product,on the same date and on the same node are processed with different batch identifier
And I want to register an "Inventory" in the system with unique batch identifier
When it meets "all" input validations
And the "EventType" field is equal to "Insert"
Then multiple inventories should be registered

@testcase=41367 @version=2
Scenario: Verify update functionality for multiple inventories for the same product,on the same date and on the same node are processed with different batch identifier
	And I want to adjust an "Inventory" in the system with unique batch identifier
	And I provide "EventType" field is equal to "Update"
	Then record a new "Inventory" with negative values for the "ProductVolume"
	And record multiple inventories with positive product volume

@testcase=41368 @version=2
Scenario: Verify delete functionality for multiple inventories for the same product,on the same date and on the same node are processed with different batch identifier
	And I want to cancel an "Inventory" in the system with unique batch identifier
	And I provide "EventType" field is equal to "Delete"
	Then record a new "Inventory" with negative values for the "ProductVolume"
	And multiple inventories should be registered