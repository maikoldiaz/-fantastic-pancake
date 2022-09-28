@sharedsteps=16581 @owner=jagudelos @ui @testplan=14709 @testsuite=14717
Feature: TrackOwnershipCalculationForSegments
In order to perform the Ownership Calculation
As a Balance Segment Professional User
I want a UI to track the ownership calculation for segments

Background: Login
	Given I am logged in as "profesional"

@testcase=16582 @bvt @version=2
Scenario: Verify executed Ownership calculation for the segments information when there are records in the last 40 days
	Given I have Ownership Calculation for Segments information from the last 40 days
	When I navigate to "Volumetric Balance with ownership" page
	Then I should see the information of executed "Ownership Calculation for the Segments" in the grid

@testcase=16583 @manual
Scenario: Verify executed Ownership calculation for the segments information when there are no records in the last 40 days
	Given I have no information from the last 40 days
	When I navigate to "Volumetric Balance with ownership" page
	Then I should see error message "Sin registros"

@testcase=16584 @version=2
Scenario: Verify the functionality when the record is in Submitted state
	When I navigate to "Volumetric Balance with ownership" page
	And I have a record with "Submitted" state and I searched in "Ownership Calculation for the Segments" Grid
	Then verify that "tickets" "download" "link" is "disabled"
	And verify that "tickets" "viewSummary" "link" is "disabled"
	Then I should see the tooltip "see summary" text for "tickets" "viewSummary" "link"
	And I should see the tooltip "to download" text for "tickets" "download" "link"

@testcase=16585 @version=2
Scenario: Verify the functionality when the record is in Completed with error state
	When I navigate to "Volumetric Balance with ownership" page
	And I have a record with "Error" state and I searched in "Ownership Calculation for the Segments" Grid
	Then verify that "tickets" "download" "link" is "disabled"
	And verify that "tickets" "viewError" "link" is "enabled"
	And I should see the tooltip "see error" text for "tickets" "viewError" "link"
	And I should see the tooltip "to download" text for "tickets" "download" "link"
	When I click on "tickets" "viewError" "link"
	Then I should see a modal window with the error information in "Ownership Calculation for the Segments" page

@testcase=16586 @bvt @version=2
Scenario: Verify the functionality when the record is in Completed state
	When I navigate to "Volumetric Balance with ownership" page
	And I have a record with "Completed" state and I searched in "Ownership Calculation for the Segments" Grid
	Then verify that "tickets" "download" "link" is "enabled"
	And verify that "tickets" "viewSummary" "link" is "enabled"
	And I should see the tooltip "see summary" text for "tickets" "viewSummary" "link"
	And I should see the tooltip "to download" text for "tickets" "download" "link"
	When I click on "tickets" "viewSummary" "link"
	Then I see "Balance per Node" header

@testcase=16587 @version=2
Scenario: Verify the headers of Volumetric Balance with ownership page
	When I navigate to "Volumetric Balance with ownership" page
	Then I see "Volumetric Balance with ownership" header

@testcase=16588 @version=2
Scenario Outline: Verify Filters functionality
	Given I have Ownership Calculation for Segments information from the last 40 days
	When I navigate to "Volumetric Balance with ownership" page
	And I provide the value for "tickets" "<Field>" "<ControlType>" filter in "Ownership Calculation for the Segments" Grid
	Then I should see the information that matches the data entered for the "<Field>" in "Ownership Calculation for the Segments" Grid

	Examples:
		| Field                | ControlType |
		| TicketId             | textbox     |
		| StartDate            | date        |
		| EndDate              | date        |
		| CreatedDate          | date        |
		| CreatedBy            | textbox     |
		| categoryElement_name | textbox     |
		| Status               | combobox    |

@testcase=16589 @version=2
Scenario Outline: Verify Sorting functionality
	Given I have Ownership Calculation for Segments information from the last 40 days
	When I navigate to "Volumetric Balance with ownership" page
	And I click on the "<ColumnName>"
	Then the results should be sorted based on "<ColumnName>" in "Ownership Calculation for the Segments" Grid

	Examples:
		| ColumnName    |
		| Ticket        |
		| StartDate     |
		| EndDate       |
		| ExecutionDate |
		| Username      |
		| Segment       |
		| Status        |

@testcase=16590 @version=2
Scenario: Verify Pagination functionality
	Given I have Ownership Calculation for Segments information from the last 40 days
	When I navigate to "Volumetric Balance with ownership" page
	And I navigate to second page in "Ownership Calculation for the Segments" Grid
	Then the records should be displayed accordingly in "Ownership Calculation for the Segments" Grid
	When I change the elements count per page to 50
	Then the records count in "Ownership Calculation for the Segments" Grid shown per page should also be 50