@sharedsteps=7539 @owner=jagudelos @testplan=4938 @testsuite=5399
Feature: QueryDataMappingHomologationService
In order to handle Data Mapping Homologation Service
As an application administrator
I want to query Data Mapping Homologation Service

Background: Login
	Given I am authenticated as "admin"

@testcase=5806 @api @bvt @output=QueryAll(GetHomologations) @prodready
Scenario: Get all Homologations
	Given I have "Homologations" in the system
	When I Get all records
	Then the response should return all valid records

@testcase=5807 @api @bvt @output=QueryAll(GetHomologations) @prodready
Scenario: Get Homologation with valid Id
	Given I have "Homologations" in the system
	When I Get record with valid Id
	Then the response should return requested record details

@testcase=5808 @api @bvt @output=QueryAll(GetHomologations) @prodready
Scenario: Get Homologation by valid Homologation Id and Homologation group id
	Given I have "Homologations" in the system
	When I Get record by "HomologationId" and "GroupTypeId"
	Then the response should return requested record details

@testcase=5809  @api @bvt @output=QueryAll(GetHomologations) @ignore
Scenario: Get Homologation by valid Homologation Id and Homologation group name
	Given I have "Homologations" in the system
	When I Get record by "HomologationId" and "HomologationGroupName"
	Then the response should return requested record details