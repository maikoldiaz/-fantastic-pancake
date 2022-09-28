@sharedsteps=7539 @owner=jagudelos @api @testplan=49466 @testsuite=49470 @MVP2and3 @parallel=false
Feature: ServiceToSendTheDailyInventoriesToTrue
As SAP PO, I need to call a service to send the
daily inventories to TRUE

Background: Login
	Given I am authenticated as "admin"

@parallel=false @testcase=52141 @BVT2
Scenario: Verify Inventory is registered when inventoryId attribute is less than 50
	Given I have data to process "Inventories" in system
	When I have 1 inventory with inventoryId attribute is 49
	And I register "Inventories" in system
	Then response should be successful
	And 1 inventory should be registered in system

@parallel=false @testcase=52142 @BVT2
Scenario: Verify Inventory is registered when inventoryId attribute is equal to  50
	Given I have data to process "Inventories" in system
	When I have 1 inventory with inventoryId attribute is 50
	And I register "Inventories" in system
	Then response should be successful
	And 1 inventory should be registered in system

@parallel=false @testcase=52143 @version=2 @BVT2
Scenario: Verify error message is displayed when inventoryId attribute is greater than 50
	Given I have data to register "Inventories" in system
	When I have 1 inventory with inventoryId attribute is 51
	And I register "Inventories" in system
	Then the response should fail with message "Identificador del inventario (InventoryId) admite hasta 50 caracteres"

@parallel=false @testcase=52144 @BVT2
Scenario: Verify Inventory is registered when optional attributes like BatchId, Version, grossStandardQuantity, ProductType are not provided in the request
	Given I have data to process "Inventories" in system
	When I have 1 inventory without BatchId, Version, grossStandardQuantity, ProductType attributes
	And I register "Inventories" in system
	Then response should be successful
	And 1 inventory should be registered in system

@parallel=false @testcase=52145 @BVT2
Scenario: Verify Inventory is registered when scenarioId is provided in the request
	Given I have data to process "Inventories" in system
	When I have 1 inventory with scenarioId attribute
	And I register "Inventories" in system
	Then response should be successful
	And 1 inventory should be registered in system

@parallel=false @testcase=52146 @BVT2
Scenario: Verify Inventory is not registered when scenarioId is not provided in the request
	Given I have data to register "Inventories" in system
	When I have 1 inventory without scenarioId attribute
	And I register "Inventories" in system
	Then the response should fail with message "El escenario (ScenarioId) es obligatorio"

@parallel=false @testcase=52147 @BVT2
Scenario: Verify Inventory is registered when valid format for BatchId, Version, grossStandardQuantity, system, scenarioId are provided in the request
	Given I have data to process "Inventories" in system
	When I have 1 inventory with valid format for BatchId, Version, grossStandardQuantity, system, scenarioId attributes
	And I register "Inventories" in system
	Then response should be successful
	And 1 inventory should be registered in system

@parallel=false @testcase=52148 @version=2 @BVT2
Scenario Outline: Verify error message when inventory is tried to register without mandatory fields
	Given I have data to register "Inventories" in system
	When I have not provided "<Field>" attribute in the request
	And I register "Inventories" in system
	Then the response should fail with message "<ErrorMessage>"

	Examples:
		| Field               | ErrorMessage                                                            |
		| nodeId              | El identificador del nodo (NodeId) es obligatorio                       |
		| eventType           | El tipo de evento (EventType) es obligatorio                            |
		| segmentId           | El segmento (SegmentId) es obligatorio                                  |
		| inventoryId         | El identificador del inventario (InventoryId) es obligatorio            |
		| scenarioId          | El escenario (ScenarioId) es obligatorio                                |
		| sourceSystem        | El nombre del sistema origen (SourceSystem) es obligatorio              |
		| inventoryDate       | La fecha del inventario (InventoryDate) es obligatoria                  |
		| destinationSystem   | DestinationSystem es obligatorio                                        |
		| productId           | El identificador del producto (ProductId) es obligatorio                |
		| netStandardQuantity | El volumen (NetStandardQuantity) es obligatorio                         |
		| measurementUnit     | La unidad de medida del atributo (MeasurementUnit) es obligatoria       |
		| ownerId             | El identificador del propietario (OwnerId) es obligatorio               |
		| ownershipValue      | El valor de la propiedad (OwnershipValue) es obligatorio                |
		| ownerShipValueUnit  | La unidad del valor de la propiedad (OwnershipValueUnit) es obligatoria |
		| attributeId         | El identificador del atributo (AttributeId) es obligatorio              |
		| attributeValue      | El valor del atributo (AttributeValue) es obligatorio                   |
		| valueAttributeUnit  | La unidad de medida del atributo (ValueAttributeUnit) es obligatoria    |

@parallel=false @testcase=52149 @version=2 @BVT2
Scenario Outline: Verify error message when inventory is tried to register with more than length that attributes accepts
	Given I have data to register "Inventories" in system
	When I have provided more than <Length> of "<Field>" that accepts
	And I register "Inventories" in system
	Then the response should fail with message "<ErrorMessage>"

	Examples:
		| Field                | Length | ErrorMessage                                                                            |
		| nodeId               | 150    | El identificador del nodo (NodeId) admite hasta 150 caracteres                          |
		| version              | 50     | Versión (Version) admite hasta 50 caracteres                                            |
		| system               | 150    | Sistema (System) admite hasta 150 caracteres                                            |
		| eventType            | 10     | El tipo de evento (EventType) puede contener máximo 10 caracteres                       |
		| segmentId            | 150    | El identificador del segmento (SegmentId) admite hasta 150 caracteres                   |
		| operatorId           | 150    | El operador (OperatorId) admite hasta 150 caracteres                                    |
		| observations         | 150    | Las observaciones (Observations) pueden contener máximo 150 caracteres                  |
		| sourceSystem         | 25     | El nombre del sistema origen (SourceSystem) admite hasta 25 caracteres                  |
		| destinationSystem    | 25     | DestinationSystem admite hasta 25 caracteres                                            |
		| batchId              | 25     | El identificador del batch (BatchId) puede contener máximo 25 caracteres                |
		| productId            | 150    | El identificador del producto (ProductId) admite hasta 150 caracteres                   |
		| productType          | 150    | Tipo Producto (ProductTypeId) admite hasta 150 caracteres                               |
		| measurementUnit      | 50     | La unidad de medida (MeasurementUnit) admite hasta 50 caracteres                        |
		| ownerId              | 150    | El identificador del propietario (OwnerId) admite hasta 150 caracteres                  |
		| ownerShipValueUnit   | 50     | La unidad de la propiedad (OwnershipValueUnit) admite hasta 50 caracteres               |
		| attributeId          | 150    | El identificador del atributo (AttributeId) admite hasta 150 caracteres                 |
		| attributeType        | 150    | El tipo de atributo (AttributeType) admite hasta 150 caracteres                         |
		| attributeValue       | 150    | El valor del atributo (AttributeValue) admite hasta 150 caracteres                      |
		| valueAttributeUnit   | 50     | La unidad del valor del atributo (ValueAttributeUnit) admite hasta 50 caracteres        |
		| attributeDescription | 150    | La descripción del atributo (AttributeDescription) puede contener máximo 150 caracteres |

@parallel=false @testcase=52150 @version=2 @BVT2
Scenario Outline: Verify Inventory should be registered when Scenario, Tolerance, Segment, Operator, ProductVolume is provided in the request
	Given I have data to process "Inventories" in system
	When I have 1 inventory with "<Field>" is provided in the request
	And I register "Inventories" in system
	Then response should be successful
	And 1 inventory should be registered in system

	Examples:
		| Field         |
		| scenario      |
		| tolerance     |
		| segment       |
		| operator      |
		| productVolume |

@parallel=false @testcase=52151 @BVT2
Scenario: Verify Inventory is registered when scenarioId, uncertainty, segmentId, operatorId, netStandardQuantity is provided in the request
	Given I have data to process "Inventories" in system
	When I have 1 inventory with scenarioId, uncertainty, segmentId, operatorId, netStandardQuantity attributes
	And I register "Inventories" in system
	Then response should be successful
	And 1 inventory should be registered in system

@parallel=false @testcase=52152 @BVT2
Scenario: Verify inventory is registered with columbian hours
	Given I have data to process "Inventories" in system
	When I have 1 inventory with inventoryId attribute is 50
	And I register "Inventories" in system
	Then response should be successful
	And 1 inventory should be registered in system with columbian hours

@parallel=false @testcase=52153 @BVT2
Scenario: Verify multiple inventories are registered when all mandatory attributes are provided in the request
	Given I have data to process "Inventories" in system
	When I have 2 inventories with all mandatory attributes
	And I register "Inventories" in system
	Then response should be successful
	And 2 inventories should be registered in system

@parallel=false @testcase=52154 @manual
Scenario: Verify updated sap inventory request details in the swagger
	Given I have inventory json
	When I login to swagger
	Then same request details should be updated in the swagger as per V2 canonical structure