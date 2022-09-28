@sharedsteps=4013 @owner=jagudelos @ui @testsuite=8489 @testplan=8481
Feature: OperationalCutoffSummary
In order to perform operation cutoff of volumetric information
As an application administrator
I want to perform the operation cutoff summary

Background: Login
	Given I am logged in as "admin"

@testcase=9899 @ui
Scenario: Perform successful operational cutoff of volumetric information
	Given I have verified pending "Movements" in the system
	When I navigate to "Ticket" page
	Then enter "StartDate" into "Tickets" "StartDate" "date"
	And enter "EndDate" into "Tickets" "EndDate" "date"
	When I click on "Tickets" "Edit" "link"
	Then I should see "TicketDetail" "TicketId" "label" and capture it
	And I should see "TicketDetail" "Records" "Total" "container"
	And I should see "TicketDetail" "Records" "Processed" "container"
	And I should see "TicketDetail" "Records" "New" "container"
	Then validate "TotalProcessed" and "TotalCreated" records displayed as expected in "TicketDetail" "Records" "Total" "container" chart
	And validate "TotalMovements" and "TotalInventories" records displayed as expected in "TicketDetail" "Records" "Processed" "container" graph
	And validate "TotalInterface", "TotalTolerance" and "TotalUnidentifiedLoss" processed by TRUE are displayed as expected in "TicketDetail" "Records" "New" "container"

@testcase=9900 @ui
Scenario Outline: Verify Filters functionality for StartDate and EndDate
	Given I have verified pending "Movements" in the system
	When I navigate to "Ticket" page
	When I provide "<FieldValue>" for "Tickets" "<Field>" "date" filter
	Then I should see a "<FieldValue>" belongs to "Tickets" in the grid

	Examples:
		| Field     | FieldValue |
		| startDate | StartDate  |
		| endDate   | EndDate    |

@testcase=9901 @ui
Scenario: Verify Filters functionality for CategoryElement
	Given I have verified pending "Movements" in the system
	When I navigate to "Ticket" page
	When I provide "SegmentName" for "Tickets" "CategoryElement" "Name" "textbox" filter
	Then I should see a "SegmentName" belongs to "Tickets" in the grid

@testcase=9902 @ui
Scenario: Verify Filters functionality for status
	Given I have verified pending "Movements" in the system
	When I navigate to "Ticket" page
	When I provide "Finalizado" for "Tickets" "status" "dropdown" filter
	Then I should see a "Finalizado" belongs to "Tickets" in the grid