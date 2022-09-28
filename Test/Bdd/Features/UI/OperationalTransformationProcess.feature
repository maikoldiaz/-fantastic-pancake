@sharedsteps=4013 @owner=jagudelos @ui @testplan=14709 @testsuite=14715 @old
Feature: OperationalTransformationProcess
In order to consolidate the ownership
As a TRUE system
I need to process the Operational Transformation of movements and inventories

Background: Login
	Given I am logged in as "admin"

@testcase=16491
Scenario: Process an inventory message without transformation configuration
	Given I want to process the operational transformation of movements and inventories
	When I have an "Inventory" record which has no equivalent transformation has been setup
	Then the system must save the record without changing its contents

@testcase=16492
Scenario: Process a movement message without transformation configuration
	Given I want to process the operational transformation of movements and inventories
	When I have an "Movement" record which has no equivalent transformation has been setup
	Then the system must save the record without changing its contents

@testcase=16493 @bvt
Scenario: Process an inventory message with transformation configuration
	Given I want to process the operational transformation of movements and inventories
	When I have an "Inventory" record which has equivalent transformation has been setup
	Then the system must do the transformation
	And the system should perform the following subprocesses
	And it should save the record with changed values

@testcase=16494 @bvt
Scenario: Process a movement message with transformation configuration
	Given I want to process the operational transformation of movements and inventories
	When I have an "Movement" record which has equivalent transformation has been setup
	Then the system must do the transformation
	And the system should perform the following subprocesses
	And it should save the record with changed values