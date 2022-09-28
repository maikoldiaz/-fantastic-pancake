@sharedsteps=7539 @owner=jagudelos @api @testplan=14709 @testsuite=14716
Feature: MapProductsOfSourceNodeToANewConnection
As an application administrator
I want products on the source node to be added when creating a new connection

Background: Login
	Given I am authenticated as "admin"

@testcase=16472 @bvt @output=QueryAll(GetNodeConnection)
Scenario: Creating connections for source node with a single store location
	Given I have a source node which has "single storage location" set up
	And I want to create a "node-connection" in the system
	When I provide the required fields
	Then the response should succeed with message "Conexión creada con éxito"
	And the products associated with the "storage location" on the source node should be added to the connection

@testcase=16473 @bvt @output=QueryAll(GetNodeConnection)
Scenario: Creating connections for source node with multiple store locations
	Given I have a source node which has "multiple storage locations" set up
	And I want to create a "node-connection" in the system
	When I provide the required fields
	Then the response should succeed with message "Conexión creada con éxito"
	When I update a node with new storage location
	Then the products associated with the "multiple storage locations" on the source node should be added to the connection