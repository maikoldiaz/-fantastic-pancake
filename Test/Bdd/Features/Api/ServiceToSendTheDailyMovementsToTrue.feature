@sharedsteps=7539 @owner=jagudelos @api @testplan=49466 @testsuite=49469 @MVP2and3 @parallel=false
Feature: ServiceToSendTheDailyMovementsToTrue
As SAP PO, I need to call a service to send
the daily movements to TRUE

Background: Login
Given I am authenticated as "admin"

@parallel=false @testcase=52156 @BVT2
Scenario Outline: Verify Movement is registered even though sourceSystem, grossStandardVolume, netStandardVolume, measurementUnit attributes are removed of backupMovement
Given I have data to process "Movements" in system
When I have 1 movement without "<Field>" attributes of backupMovement
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

Examples:
| Field               |
| sourceSystem        |
| grossStandardVolume |
| netStandardVolume   |
| measurementUnit     |

@parallel=false @testcase=52157 @BVT2
Scenario: Verify Movement is registered when batchId, version, isOfficial, system attributes are processed in the request
Given I have data to process "Movements" in system
When I have 1 movement
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

@parallel=false @testcase=52158 @BVT2
Scenario Outline: Verify Movement is registered when value is not provided for batchId, version, system attributes in the request
Given I have data to process "Movements" in system
When I have 1 movement without value for "<Field>" attributes
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

Examples:
| Field   |
| batchId |
| version |
| system  |

@parallel=false @testcase=52159 @BVT2
Scenario: Verify scenarioId attribute is mandatory and data type is int while processing a movement
Given I have data to process "Movements" in system
When I have 1 movement with scenarioId attribute
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

@parallel=false @testcase=52160 @version=2 @BVT2
Scenario Outline: Verify SourceProductTypeId, DestinationProductId, DestinationProductTypeId attributes are not mandatory while processing a movement
Given I have data to process "Movements" in system
When I have 1 movement without value for "<Field>" attributes
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

Examples:
| Field                    |
| sourceProductTypeId      |
| destinationProductId     |
| destinationProductTypeid |
@parallel=false @testcase=52161 @BVT2
Scenario Outline: Verify Movement is registered when grossStandardQuantity, netStandardQuantity, uncertainty, segmentId, operatorId attributes are processed in the request
Given I have data to process "Movements" in system
When I have 1 movement with value for "<Field>"
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

Examples:
| Field                 |
| grossStandardQuantity |
| netStandardQuantity   |
| uncertainty           |
| segmentId             |
| operatorId            |

@parallel=false @testcase=52162 @manual
Scenario: Verify updated sap movement request details in the swagger
Given I have movement json
When I login to swagger
Then same request details should be updated in the swagger as per V2 canonical structure

@parallel=false @testcase=52163 @BVT2
Scenario: Verify error message when neither movement source nor movement destination attributes are provided in the request
Given I have data to process "Movements" in system
When I have 1 movement without movement source and movement destination attributes in the request
And I register "Movements" in system
Then the response should fail with message "Es obligatorio reportar información del origen o del destino. (Ambas no pueden estar vacías)."

@parallel=false @testcase=52164 @BVT2
Scenario: Verify movement is registered when only movement source attribute is provided in the request
Given I have data to process "Movements" in system
When I have 1 movement with movement source attribute
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

@parallel=false @testcase=52165 @BVT2
Scenario: Verify movement is registered when only movement destination attribute is provided in the request
Given I have data to process "Movements" in system
When I have 1 movement with movement destination attribute
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

@parallel=false @testcase=52166
Scenario: Verify movement is registered when both movement source and destination attributes are provided in the request
Given I have data to process "Movements" in system
When I have 1 movement with both movement source and destination attributes
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

@parallel=false @testcase=52167 @BVT2
Scenario: Verify movement is registered with columbian hours
Given I have data to process "Movements" in system
When I have 1 movement
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system with columbian hours

@parallel=false @testcase=52168 @BVT2
Scenario: Verify isOfficial and globalMovementId and backupMovementId should belongs to OfficialInformation structure in movement request
Given I have data to process "Movements" in system
When I have 1 movement with isOfficial and globalMovementId and backupMovementId under OfficialInformation
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

@parallel=false @testcase=52169
Scenario: Verify movement is registered when official information is not provided in the request
Given I have data to process "Movements" in system
When I have 1 movement without official information attribute
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

@parallel=false @testcase=52170 @BVT2
Scenario: Verify movement is registered when all mandatory attributes are provided in the request
Given I have data to process "Movements" in system
When I have 1 movement with all mandatory attributes
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

@parallel=false @testcase=52171 @BVT2
Scenario: Verify multiple movement are registered when all mandatory attributes are provided in the request
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
Then response should be successful
And 2 movements should be registered in system

@parallel=false @testcase=52172 @version=3 @BVT2
Scenario Outline: Verify error message when movement is tried to register without mandatory fields
Given I have data to register "Movements" in system
When I have not provided "<Field>" attribute in the movement request
And I register "Movements" in system
Then the response should fail with message "<ErrorMessage>"

Examples:
| Field               | ErrorMessage                                                            |
| sourceSystem        | El nombre del sistema origen (SourceSystem) es obligatorio              |
| eventType           | El tipo de evento (EventType) es obligatorio                            |
| movementId          | El identificador del movimiento (MovementId) es obligatorio             |
| movementTypeId      | El identificador del tipo de movimiento (MovementTypeId) es obligatorio |
| operationalDate     | La fecha operacional (OperationDate) es obligatoria                     |
| period              | El periodo (Period) es obligatorio                                      |
| netStandardQuantity | El volumen neto (NetStandardQuantity) es obligatorio                    |
| measurementUnit     | La unidad de medida del atributo (MeasurementUnit) es obligatoria       |
| segmentId           | El segmento (SegmentId) es obligatorio                                  |
| scenarioId          | El escenario (ScenarioId) es obligatorio                                |
| classification      | La clasificación del movimiento (Classification) es obligatoria         |
| startTime           | La hora de inicio del movimiento (StartTime) es obligatoria             |
| endTime             | La hora final del movimiento (EndTime) es obligatoria                   |
| sourceNodeId        | El nodo origen (SourceNodeId) es obligatorio                            |
| sourceProductId     | El identificador del producto origen (SourceProductId) es obligatorio   |
| destinationNodeId   | El nodo destino (DestinationNodeId) es obligatorio                      |
| attributeId         | El identificador del atributo (AttributeId) es obligatorio              |
| attributeValue      | El valor del atributo (AttributeValue) es obligatorio                   |
| valueAttributeUnit  | La unidad de medida del atributo (ValueAttributeUnit) es obligatoria    |
| ownerId             | El identificador del propietario (OwnerId) es obligatorio               |
| ownershipValue      | El valor de la propiedad (OwnershipValue) es obligatorio                |
| ownerShipValueUnit  | La unidad del valor de la propiedad (OwnershipValueUnit) es obligatoria |
| isOfficial          | Es Oficial (IsOfficial) es obligatorio                                  |
| globalMovementId    | Id Movimiento Global (GlobalMovementId) es obligatorio                  |

@parallel=false @testcase=52173 @version=2 @BVT2
Scenario Outline: Verify error message when movement is tried to register with more than length that attributes accepts
Given I have data to register "Movements" in system
When I have provided more than <Length> of "<Field>" that accepts in movement request
And I register "Movements" in system
Then the response should fail with message "<ErrorMessage>"

Examples:
| Field                        | Length | ErrorMessage                                                                                            |
| sourceSystem                 | 25     | El nombre del sistema origen (SourceSystem) admite hasta 25 caracteres                                  |
| eventType                    | 25     | El tipo de evento (EventType) puede contener máximo 25 caracteres                                       |
| movementId                   | 50     | El identificador del movimiento (MovementId) admite hasta 50 caracteres                                 |
| batchId                      | 25     | El identificador del batch (BatchId) puede contener máximo 25 caracteres                                |
| movementTypeId               | 150    | El identificador del tipo de movimiento (MovementTypeId) admite hasta 150 caracteres                    |
| system                       | 150    | Sistema (System) admite hasta 150 caracteres                                                            |
| measurementUnit              | 50     | La unidad de medida (MeasurementUnit) admite hasta 50 caracteres                                        |
| segmentId                    | 150    | El identificador del segmento (SegmentId) admite hasta 150 caracteres                                   |
| operatorId                   | 150    | El operador (OperatorId) admite hasta 150 caracteres                                                    |
| version                      | 50     | Versión (Version) admite hasta 50 caracteres                                                            |
| observations                 | 150    | Las observaciones (Observations) pueden contener máximo 150 caracteres                                  |
| classification               | 30     | La clasificación del movimiento (Classification) puede contener máximo 30 caracteres                    |
| sapProcessStatus             | 50     | El estado del proceso (SapProcessStatus) admite hasta 50 caracteres                                     |
| sourceNodeId                 | 150    | El identificador del nodo origen (SourceNodeId) admite hasta 150 caracteres                             |
| sourceStorageLocationId      | 150    | El identificador del almacén de origen (SourceStorageLocationId) admite hasta 150 caracteres            |
| sourceProductId              | 150    | El identificador del producto origen (SourceProductId) admite hasta 150 caracteres                      |
| sourceProductTypeId          | 150    | El identificador del tipo de producto origen (SourceProductTypeId) admite hasta 150 caracteres          |
| destinationNodeId            | 150    | El identificador del nodo de destino (DestinationNodeId) admite hasta 150 caracteres                    |
| destinationStorageLocationId | 150    | El identificador del almacén de destino (DestinationStorageLocationId) admite hasta 150 caracteres      |
| destinationProductId         | 150    | El identificador del producto de destino (DestinationProductId) admite hasta 150 caracteres             |
| destinationProductTypeid     | 150    | El identificador del tipo de producto de destino (DestinationProductTypeId) admite hasta 150 caracteres |
| attributeId                  | 150    | El identificador del atributo (AttributeId) admite hasta 150 caracteres                                 |
| attributeType                | 150    | El tipo de atributo (AttributeType) admite hasta 150 caracteres                                         |
| attributeValue               | 150    | El valor del atributo (AttributeValue) admite hasta 150 caracteres                                      |
| valueAttributeUnit           | 50     | La unidad del valor del atributo (ValueAttributeUnit) admite hasta 50 caracteres                        |
| attributeDescription         | 150    | La descripción del atributo (AttributeDescription) puede contener máximo 150 caracteres                 |
| ownerId                      | 150    | El identificador del propietario (OwnerId) admite hasta 150 caracteres                                  |
| ownerShipValueUnit           | 50     | La unidad de la propiedad (OwnershipValueUnit) admite hasta 50 caracteres                               |
| backupMovementId             | 50     | El identificador del movimiento de respaldo (BackupMovementId) admite hasta 50 caracteres               |
| globalMovementId             | 50     | El identificador del movimiento global (GlobalMovementId) admite hasta 50 caracteres                    |