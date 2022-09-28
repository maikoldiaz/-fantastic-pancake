@sharedsteps=7539 @owner=jagudelos @api @testplan=31102 @testsuite=31112 @parallel=false
Feature: ReceiveMovementsFromSinoperThroughSAPPO
	TRUE must publish an REST API 
	to receive the movements 
	from SINOPER through SAP PO

Background: Login
	Given I am authenticated as "admin"

@testcase=34088 @version=3 @bvt1.5
Scenario: Verify a Movement is Registered when a Valid JSON is sent
	Given I have data to process "Movements" in system
	When I have 1 movement
	And I register "Movements" in system
	Then response should be successful

@testcase=34086 @version=2
Scenario: Verify a Movement with Homologated Json values should get registered in the TRUE System
	Given I have data to process "Movements" in system
	When I have 1 movement
	And The fields for the movements json are already homologated
	And I register "Movements" in system
	Then response should be successful

@testcase=32713 @version=3
Scenario Outline: Validate mandatory fields and error messages
	Given I have data to process "Movements" in system
	When I don't provide "<field>" in json 
	And I register "Movements" in system
	Then the response should fail with message "<error>"

	Examples:
		| field                                 | error                                           |
		| sourceSystem                          | sourceSystem es obligatorio                     |
		| eventType                             | eventType es obligatorio                        |
		| movementId                            | movementId es obligatorio                       |
		| movementTypeId                        | movementTypeId es obligatorio                   |
		| operationalDate                       | operationalDate es obligatorio                  |
		| movementSource sourceNodeId           | sourceNodeId es obligatorio                     |
		| movementDestination destinationNodeId | destinationNodeId es obligatorio                |
		| netStandardVolume                     | netStandardVolume es obligatorio                |
		| Attributes attributeId                | attributeId es obligatorio                      |
		| Attributes valueAttributeUnit         | La unidad de medida del atributo es obligatoria |
		| MovementOwner ownerId                 | ownerId es obligatorio                          |
		| BackupMovement movementId             | movementId es obligatorio                       |
		| globalMovementId                      | globalMovementId es obligatorio                 |
		| segmentId                             | El segmento es obligatorio                      |

@testcase=32714 @version=2 @manual
Scenario: Validate transient failure
	Given I have data to process "Movements" in system
	When I have a transient failure while proceesing movement message
	And I register "Movements" in system
	Then the error should be recorded in pending transaction table

@testcase=32715 @version=3
Scenario: Validate Processing 2000 movements
	Given I have data to process "Movements" in system
	When I have 2000 movements
	And I register "Movements" in system
	Then response should be successful

@testcase=32716 @version=3
Scenario Outline: Validate error while Processing more than 2000 movements
	Given I have data to process "Movements" in system
	When I have 2001 movements
	And I register "Movements" in system
	Then the response should fail with message "<error>"
	Examples: 
	| Error                                            |
	| Solo se admiten hasta 2000 registros por llamada |

@testcase=32717 @version=3 @ignore
Scenario Outline: Validate data types and error messages
	Given I have data to process "Movements" in system
	When I provide "<field>" with different "<dataType>"
	And I register "Movements" in system
	Then the response should fail with message "<error>"

	Examples:
		| field             | dataType | error                              |
		| sourceSystem      | int      | sourceSystem_datatypeMismatch      |
		| eventType         | int      | eventType_datatypeMismatch         |
		| movementId        | int      | movementId_datatypeMismatch        |
		| netStandardVolume | string   | netStandardVolume_datatypeMismatch |