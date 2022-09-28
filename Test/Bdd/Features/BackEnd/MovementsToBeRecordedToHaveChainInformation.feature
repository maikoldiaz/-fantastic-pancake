@sharedsteps=7539 @owner=jagudelos @api @testplan=49466 @testsuite=49474 @MVP2and3 @parallel=false
Feature: MovementsToBeRecordedToHaveChainInformation
	As a supply chain administrator, I require movements
	to be recorded to have chain information

Background: Login
Given I am authenticated as "admin"

@parallel=false @testcase=52191 @BVT2
Scenario: Verify Movement is registered when official information attribute is provided in the request
Given I have data to process "Movements" in system
When I have 1 movement with official information of all internal mandatory attributes
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

@parallel=false @testcase=52192 @BVT2
Scenario: Verify Movement is registered with all mandatory and optional attributes are homologated as per MVP2
Given I have data to process "Movements" in system
When I have 1 movement with all mandatory and optional attributes are homologated
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

@parallel=false @testcase=52193 @BVT2
Scenario: Verify Movement is registered when shouldHomologate parameter as enabled
Given I have data to process "Movements" in system
When I have 1 movement when shouldHomologate parameter is enabled
And I register "Movements" in system with all homologable attributes
Then response should be successful
And 1 movement should be registered in system

@testcase=52194 @manual
Scenario: Verify Movement is registered when shouldHomologate parameter as disabled
Given I have data to process "MovementsHomologated" in system
And I have "shouldHomologate" flag set as False
When I have 1 movement
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

@parallel=false @testcase=52195 @BVT2
Scenario: Verify Movement is registered when optional attributes are not provided
Given I have data to process "Movements" in system
When I have 1 movement when optional attributes are not provided
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

@parallel=false @testcase=52196 @BVT2
Scenario: Verify Movement is registered when request includes all required attributes
Given I have data to process "Movements" in system
When I have 1 movement when request is provide with all required attributes
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

@parallel=false @testcase=52197 @BVT2
Scenario: Verify Movement is registered when there is direct mapping between registry and canonical attributes
Given I have data to process "Movements" in system
When I have 1 movement when request is having direct mapping between registry and canonical attributes
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system

@parallel=false @testcase=52198
Scenario: Verify Movement is registered when DestinationProductId is null and MovementSource has value
Given I have data to process "Movements" in system
When I have 1 movement when DestinationProductId is null and MovementSource has value
And I register "Movements" in system
Then response should be successful
And 1 movement should be registered in system
And sourceProductId value should be used as destinationProductId value

@parallel=false @testcase=52199 @BVT2
Scenario: Verify error message when DestinationProductId is null and MovementSource has no value
Given I have data to process "Movements" in system
When I have 1 movement when DestinationProductId is null and MovementSource has no value
And I register "Movements" in system
Then it must be stored in a Pendingtransactions repository with validation "Id Producto Destino (DestinationProductId) es obligatorio"

@parallel=false @testcase=52200 @BVT2
Scenario: Verify ScenarioId and name details in the database
When I have scenario table in the database
Then scenarioId and name details should present in the database

@parallel=false @testcase=52201
Scenario: Verify ScenarioId for old movement records in the database
	Given I have records in movements table
	Then for all old records in movements scenarioId should be 1

@parallel=false @testcase=52202
Scenario: Verify seed data information Source System category
	Given I have source sytem category in the system
	Then "Name" and "Description" data should present
	| Name     | Description                                          |
	| ROMSSGRC | Sistema Operativo de la Refinería de Cartagena       |
	| ROMSSGRB | Sistema Operativo de la Refinería de Barrancabermeja |
	| SAPS4    | Sistema Operativo Comercial                          |
	| BDP      | Sistema Operativo de Producción                      |