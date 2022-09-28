@sharedsteps=4013 @owner=jagudelos @testplan=4938 @testsuite=5396
Feature: OperationalBalanceCalculation
In order to handle the imbalance
As a TRUE system
I want to apply Operational Balance Calculation process

Background: Login
	Given I am logged in as "admin"

@testcase=5884 @backend @database
Scenario: Calculate the Operational Balance
	Given I want to calculate the "OperationalBalance" in the system
	When I receive the input data
	Then I should be able to calculate the Operational Balance

@testcase=5885 @backend @database
Scenario Outline: Calculate the Operational Balance without mandatory fields
	Given I want to calculate the "OperationalBalance" in the system
	When I don't receive "<Field>"
	Then the result should fail with message "<Message>"

	Examples:
		| Field     | Message                             |
		| StartDate | STARTDATE_REQUIREDVALIDATION        |
		| EndDate   | ENDDATE_REQUIREDVALIDATION          |
		| NodeId    | INVENTORY_NODEID_REQUIREDVALIDATION |

@testcase=5886 @backend @database
Scenario: Calculate the Operational Balance with Start Date greater than the End Date
	Given I want to calculate the "OperationalBalance" in the system
	When I receive "StartDate" greater than the "EndDate"
	Then the result should fail with message "DATES_INCONSISTENT"

@testcase=5887 @backend @database
Scenario: Calculate the Operational Balance with End Date less than the Start Date
	Given I want to calculate the "OperationalBalance" in the system
	When I receive "EndDate" less than the "StartDate"
	Then the result should fail with message "DATES_INCONSISTENT"

@testcase=5888 @backend @database
Scenario: Calculate the Operational Balance with End Date greater than or equal to the Current Date
	Given I want to calculate the "OperationalBalance" in the system
	When I receive "EndDate" greater than or equal to the "CurrentDate"
	Then the result should fail with message "ENDDATE_BEFORENOWVALIDATION"