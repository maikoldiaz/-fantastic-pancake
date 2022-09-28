@sharedsteps=4013 @owner=jagudelos @ui @testplan=19722 @testsuite=19787
Feature: ExcelFileGenerationForEvents
In order to perform the Ownership calculation
As TRUE system, I need to send to the ownership
calculation process, the planning, programming
and collaboration agreements events to complement the ownership rules

Background: Login
Given I am logged in as "admin"

@testcase=21200 @version=2
Scenario: Verify Events sheet information when start and end date are within the Range of OwnershipCalculation date
	Given I have ownershipcalculation for segment and events information for same segment within the Range of OwnershipCalculation date
	Then excel file should be generated with the information required to calculate the ownership
	And excel file should contains Events information

@testcase=21201 @version=2
Scenario: Verify columns in the Excel when there are no Events information
	Given I have ownershipcalculation for segment and did not have events information for same segment within the Range of OwnershipCalculation date
	Then excel file should be generated with the information required to calculate the ownership
	And excel file should contains titles without information

@testcase=21202 @version=2
Scenario: Verify Events sheet information when start and end date are not within the Range of OwnershipCalculation date
	Given I have events information for segment but not within the range of OwnershipCalculation date
	Then excel file should be generated with the information required to calculate the ownership
	And excel file should contains titles without information