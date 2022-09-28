@sharedsteps=7539 @owner=jagudelos @testplan=8481 @testsuite=8491 @backend @souryanewbackend
Feature: HomologatedDataInventoryWithTank
	In order to register an Inventory in the system
	As a TRUE system
	I want to homologate the data

Background: Login
	Given I am authenticated as "admin"

@testcase=9228 @backend @prodready @version=2
Scenario: Register a Inventory with valid data
	Given I want to register an "Inventory" in the system
	When it meets "all" input validations
	And the "EventType" field is equal to "Insert"
	Then it should be registered

@testcase=9229 @backend @bvt @prodready
Scenario: Register a Inventory with LocationType as Tanque
	Given I want to register an "Inventory" in the system
	When I provide "LocationType" as "Tanque"
	And the "EventType" field is equal to "Insert"
	Then it should be registered

@testcase=9230 @backend
Scenario: Register more than one Inventory with same data but different Tank name
	Given I want to register an "Inventory" in the system
	When I provide "LocationType" as "Tanque"
	And the "EventType" field is equal to "Insert"
	Then it should be registered
	When I provide different "Tanque Name" with same product
	And the "EventType" field is equal to "Insert"
	Then it should be registered

@testcase=9231 @backend
Scenario: Register more than one Inventory with different data but same Tank name
	Given I want to register an "Inventory" in the system
	When I provide "LocationType" as "Tanque"
	And the "EventType" field is equal to "Insert"
	Then it should be registered
	When I provide different "Product" with same "Tanque Name"
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario ya existe en el sistema​"

@testcase=9232 @backend
Scenario: Register more than one Inventory with same data and Tank name
	Given I want to register an "Inventory" in the system
	When I provide "LocationType" as "Tanque"
	And the "EventType" field is equal to "Insert"
	Then it should be registered
	When I register with same data
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario ya existe en el sistema​"

@testcase=9233 @backend
Scenario: Register an Inventory with Tank name that exceeds 20 characters
	Given I want to register an "Inventory" in the system
	When I provide "Tank Name" that exceeds 20 characters for inventory
	Then it must be stored in a Pendingtransactions repository with validation "El Tanque puede contener máximo 20 caracteres"

@testcase=9234 @backend
Scenario: Update an existing Inventory with valid data
	Given I want to adjust an "Inventory"
	And I provide "EventType" field is equal to "Update"
	Then record a new "Inventory" with negative values for the "ProductVolume"
	And it should be registered

@testcase=9235 @backend @bvt @prodready
Scenario: Update an existing Inventory with LocationType as Tanque
	Given I want to adjust an 'Inventory' with "LocationType" as "Tanque"
	And I provide "EventType" field is equal to "Update"
	Then record a new "Inventory" with negative values for the "ProductVolume"
	And it should be registered

@testcase=9236 @backend
Scenario: Update an existing Inventory and add new product
	Given I want to adjust an 'Inventory' with "LocationType" as "Tanque"
	And I provide "EventType" field is equal to 'Update' and add new "Product"
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a ajustar no existe"

@testcase=9237 @backend
Scenario: Update an existing Inventory when the Inventory Identifier does not exist
	Given I want to adjust an "Inventory"
	When the 'InventoryIdentifier' does not exist and "EventType" field is equal to "Update"
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a ajustar no existe"

@testcase=9238 @backend
Scenario: Delete an existing Inventory with valid data
	Given I want to cancel an "Inventory"
	And I provide "EventType" field is equal to "Delete"
	Then record a new "Inventory" with negative values for the "ProductVolume"
	And it should be registered

@testcase=9239 @backend @bvt @prodready
Scenario: Delete an existing Inventory with LocationType as Tanque
	Given I want to cancel an 'Inventory' with "LocationType" as "Tanque"
	And I provide "EventType" field is equal to "Delete"
	Then record a new "Inventory" with negative values for the "ProductVolume"
	And it should be registered

@testcase=9240 @backend
Scenario: Delete an existing Inventory when the Inventory Identifier does not exist
	Given I want to cancel an "Inventory"
	When the 'InventoryIdentifier' does not exist and "EventType" field is equal to "Delete"
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a ajustar no existe"