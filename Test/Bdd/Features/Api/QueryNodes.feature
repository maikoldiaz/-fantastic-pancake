@sharedsteps=7539 @owner=jagudelos @testplan=3993 @testsuite=4025
Feature: QueryNodes
In order to handle the Transport Network
As an application administrator
I want to query Transport Nodes

Background: Login
	Given I am authenticated as "admin"

@testcase=4558 @api @output=QueryAll(GetNodes) @bvt @version=3 @prodready
Scenario: Get all Transport Nodes
	Given I have "Transport Nodes" in the system
	When I Get all records
	Then the response should return all valid records

@testcase=4559 @api @output=QueryAll(GetNodes) @version=2 @ignore
Scenario: Get all Transport Nodes when node list is empty
	Given I don't have any "Transport Node" in the system
	When I Get all records
	Then the response should fail with message "No existen Nodos registrados"

@testcase=4560 @api @output=QueryAll(GetNodes) @bvt @version=2 @prodready
Scenario: Get Node by Id with valid Id
	Given I have "Transport Nodes" in the system
	When I Get record with valid Id
	Then the response should return requested record details

@testcase=4561 @api @output=QueryAll(GetNodes) @prodready
Scenario: Get Node by Id with invalid Id
	Given I have "Transport Nodes" in the system
	When I Get record with invalid Id
	Then the response should fail with message "No existe el Nodo"