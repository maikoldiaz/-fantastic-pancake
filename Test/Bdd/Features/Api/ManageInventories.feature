@sharedsteps=7539 @owner=jagudelos @api  @testplan=31102 @testsuite=31113 @parallel=false
Feature: ManageInventories
In order to register inventories
As an SAP PO
I need a service to send inventories to TRUE

Background: Login
Given I am authenticated as "admin"

@testcase=33754 @version=3
Scenario Outline: Register an inventory without mandatory fields
Given I have data to process "Inventories" in system
When I don't provide "<Field>" in json 
Then the response should fail with message "<ErrorMessage>"

Examples:
| Field             | ErrorMessage                       |
| SourceSystem      | sourceSystem es obligatorio        |
| DestinationSystem | destinationSystem es obligatorio   |
| EventType         | eventType es obligatorio           |
| InventroyId       | movementId es obligatorio          |
| InventoryDate     | inventoryDate es obligatorio       |
| NodeId            | nodeId es obligatorio              |
| Products          | products es obligatorio            |
| Segment           | El segmento es obligatorio         |
| ProductId         | productId es obligatorio           |
| ProductType       | productType es obligatorio         |
| ProductVolume     | Producto Volumen es obligatorio    |
| MeasurementUnit   | La unidad de medida es obligatoria |

@testcase=33755 @version=3
Scenario Outline: Register an inventory without optional fields
Given I have data to process "Inventories" in system
When I don't provide "<Field>" in json
And I register "Inventories" in system
Then response should be successful

Examples:
| Field        |
| Attribute    |
| Owners       |
| Tank         |
| Observations |
| Scenario     |

@testcase=33756 @version=3 @bvt1.5
Scenario: Register an inventory with valid data
Given I have data to process "Inventories" in system
When I have 1 inventory
And I register "Inventories" in system
Then response should be successful

@testcase=33757 @version=3 @ignore
Scenario: Update an inventory with valid data
Given I have "Inventory" in the system
When I have 1 inventory
And I register "Inventories" in system
Then response should be successful
 
@testcase=33758 @ignore @version=2
Scenario: Delete an inventory
Given I have "Inventory" in the system
When I delete a record
Then response should be successful

@testcase=33759 @version=3
Scenario: Register 2000 inventories per call
Given I have data to process "Inventories" in system
When I have 2000 inventories
Then response should be successful

@testcase=33760 @version=3
Scenario: Register more than 2000 inventories per call
Given I have data to process "Inventories" in system
When I have 2001 inventories
Then the response should fail with message "Solo se admiten hasta 2000 registros por llamada"

@testcase=34084 @version=3 @manual
Scenario: Verify a Inventory with Homologated Json values is registered in TRUE System
Given I have "Inventory" in the system
When I have 1 inventory
And The fields for the inventories json are already homologated
And I register "Inventories" in system
Then response should be successful