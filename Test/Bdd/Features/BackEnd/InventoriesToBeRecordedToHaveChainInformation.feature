@sharedsteps=7539 @owner=jagudelos @api @testplan=49466 @testsuite=49475 @MVP2and3 @parallel=false
Feature: InventoriesToBeRecordedToHaveChainInformation
As a supply chain administrator, I require inventories
to be recorded to have chain information

Background: Login
Given I am authenticated as "admin"

@parallel=false @testcase=52176 @BVT2
Scenario: Verify inventory is registered with all mandatory and optional attributes are homologated as per MVP2
Given I have data to process "Inventories" in system
When I have 1 inventory with all mandatory and optional attributes are homologated
And I register "Inventories" in system
Then response should be successful
And 1 inventory should be registered in system

@parallel=false @testcase=52177 @BVT2
Scenario: Verify inventory is registered when shouldHomologate parameter as enabled
Given I have data to process "Inventories" in system
When I have 1 inventory when shouldHomologate parameter is enabled
And I register "Inventories" in system with all homologable attributes
Then response should be successful
And 1 inventory should be registered in system

@testcase=52178 @manual
Scenario: Verify inventory is registered when shouldHomologate parameter as disabled
Given I have data to process "InventoriesHomologated" in system
And I have "shouldHomologate" flag set as False
When I have 1 inventory
And I register "Inventories" in system
Then response should be successful
And 1 inventory should be registered in system

@parallel=false @testcase=52179 @BVT2
Scenario: Verify inventory is registered when optional attributes are not provided
Given I have data to process "Inventories" in system
When I have 1 inventory when optional attributes are not provided
And I register "Inventories" in system
Then response should be successful
And 1 inventory should be registered in system

@parallel=false @testcase=52180 @BVT2
Scenario: Verify inventory is registered when request includes all required attributes
Given I have data to process "Inventories" in system
When I have 1 inventory when request is provide with all required attributes
And I register "Inventories" in system
Then response should be successful
And 1 inventory should be registered in system

@parallel=false @testcase=52181 @BVT2
Scenario: Verify inventory is registered when there is direct mapping between registry and canonical attributes
Given I have data to process "Inventories" in system
When I have 1 inventory when request is having direct mapping between registry and canonical attributes
And I register "Inventories" in system
Then response should be successful
And 1 inventory should be registered in system

@parallel=false @testcase=52182 @BVT2
Scenario: Verify ScenarioId and name details in the database
When I have scenario table in the database
Then scenarioId and name details should present in the database

@parallel=false @testcase=52183 @BVT2
Scenario: Verify ScenarioId for old inventory records in the database
Given I have records in inventories table
Then for all old records in Inventories scenarioId should be 1

@parallel=false @testcase=52184 @BVT2
Scenario: Verify inventory is registered when it meets all validations and EventType field is equal to Insert
Given I have "Inventories" in the application
When I have 1 inventory with event type is insert
And I register "Inventories" in system
Then response should be successful
And 1 inventory should be registered in system

@parallel=false @testcase=52185 @BVT2
Scenario: Verify error message when inventory already exists in the system
Given I have data to process "Inventories" in system
When I have 1 inventory with event type is "INSERT"
And I register "Inventories" in system
Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario ya existe en el sistema"

@parallel=false @testcase=52186 @BVT2
Scenario: Verify inventory is updated when it meets all validations and EventType field is equal to UPDATE
Given I have "Inventories" in the application
When I have 1 inventory with event type is "UPDATE"
And I register "Inventories" in system
Then response should be successful
And inventory should be updated in system
@parallel=false @testcase=52187 @BVT2
Scenario: Verify error message when inventory not exists in the system when request with event type is update
When I processed "Inventories" request with event type is "UPDATE"
And "inventory" does not exists in the system
Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a ajustar no existe"

@parallel=false @testcase=52188 @BVT2
Scenario: Verify inventory is deleted when it meets all validations and EventType field is equal to DELETE
Given I have "Inventories" in the application
When I have 1 inventory with event type is "DELETE"
And I register "Inventories" in system
Then response should be successful
And inventory should be deleted in system

@parallel=false @testcase=52189 @BVT2
Scenario: Verify error message when inventory not exists in the system when request with event type is delete
When I processed "Inventories" request with event type is "DELETE"
And "inventory" does not exists in the system
Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a anular no existe"