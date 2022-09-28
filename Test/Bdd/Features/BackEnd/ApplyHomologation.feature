@owner=jagudelos @backend  @testplan=4938 @testsuite=5395
Feature: ApplyHomologation
	As a Client Application
	I need an homologation service
	to apply data mapping to an input object

@testcase=5761 @backend
Scenario: Apply Homologation process for source not configured for Homologation
	Given I have source and destination not configured for Homologation
	When I pass valid Xml with required fields
	Then the response message should be equal to input message

@testcase=5927 @backend
Scenario: Apply Homologation process with invalid Xml
	Given I have source and destination configured for Homologation
	When I pass invalid Xml to Homologation function
	Then the response should fail with message "HOMOLOGATION_INPUTDATA_FORMATVALIDATION"

@testcase=5765 @backend @bvt @input=Movement/movement.xml
Scenario: Apply Homologation process for source and destination configured for Movement Homologation
	Given I have source and destination configured for Homologation
	When I pass valid Movement Xml to Homologation function
	Then the response message should be Homologated for "Movement"

@backend @bvt @testcase=5762 @input=Inventory/Inventory.xml
Scenario: Apply Homologation process for source and destination configured for Inventory Homologation
	Given I have source and destination configured for Homologation
	When I pass valid Inventory Xml to Homologation function
	Then the response message should be Homologated for "Inventory"

@testcase=5766 @backend
Scenario: Apply Homologation process for source and destination configured for Homologation without data mapping
	Given I have source and destination configured for Homologation without data mapping
	When I pass valid Movement Xml to Homologation function
	Then the response should fail with message "HOMOLOGATION_PROCESSRESULT_ERROR"