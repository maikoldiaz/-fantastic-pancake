@owner=jagudelos @ui @testplan=11317 @testsuite=11328
Feature: TrackExecutedOperationalCutoffs
In order to perform the volumetric cutoff information
As a Balance Segment Professional User
I need UI to track Executed Operational Cutoffs

@testcase=12088 @bvt @version=4
Scenario: Verify executed operational cutoffs information when there are records in the last 40 days
	Given I am having pending records in Operational Cutoff page
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	Then I should see "Start" "link"
	When I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on DatePicker
	When I click on "InitTicket" "submit" "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	Then I should see "Add Note Functions" interface
	When I provide value for "AddComment" "comment" "textbox"
	And I click on "AddComment" "submit" "button"
	Then I should see the message "No existen movimientos e inventarios pendientes" when there are no pending records
	When I click on "ErrorsGrid" "Submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	Then I should see "Note for imbalances" interface
	When I click on "AddComment" "submit" "button"
	And I provide value for "AddComment" "comment" "textbox"
	And I click on "unbalancesGrid" "submit" "button"
	When I click on "ConfirmCutoff" "Submit" "button"
	When I navigate to "Operational Cutoff" page
	Then I should see the information of executed "Operational Cutoffs" in the grid

@testcase=12089 @ignore @version=2
Scenario: Verify executed operational cutoffs information when there are no records in the last 40 days
	Given I have no information from the last 40 days
	When I navigate to "Operational Cutoff" page
	Then I should see error message "Sin registros"

@testcase=12090 @version=4
Scenario: Verify the functionality when the record is in InProgress state
	Given I am having pending records in Operational Cutoff page
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	Then I should see "Start" "link"
	When I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on DatePicker
	When I click on "InitTicket" "submit" "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	Then I should see "Add Note Functions" interface
	When I provide value for "AddComment" "comment" "textbox"
	And I click on "AddComment" "submit" "button"
	Then I should see the message "No existen movimientos e inventarios pendientes" when there are no pending records
	When I click on "ErrorsGrid" "Submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	Then I should see "Note for imbalances" interface
	When I click on "AddComment" "submit" "button"
	And I provide value for "AddComment" "comment" "textbox"
	And I click on "unbalancesGrid" "submit" "button"
	When I click on "ConfirmCutoff" "Submit" "button"
	When I navigate to "Operational Cutoff" page
	And I have a record with "InProgress" state and I searched in "OperationalCutoffTickets" Grid
	Then verify that "tickets" "download" "link" is "disabled"
	And verify that "tickets" "viewSummary" "link" is "disabled"
	Then I should see the tooltip "see summary" text for "tickets" "viewSummary" "link"
	And I should see the tooltip "to download" text for "tickets" "download" "link"

@testcase=12091 @version=4
Scenario: Verify the functionality when the record is in Completed with error state
	Given I am having pending records in Operational Cutoff page
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	Then I should see "Start" "link"
	When I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on DatePicker
	When I click on "InitTicket" "submit" "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	Then I should see "Add Note Functions" interface
	When I provide value for "AddComment" "comment" "textbox"
	And I click on "AddComment" "submit" "button"
	Then I should see the message "No existen movimientos e inventarios pendientes" when there are no pending records
	When I click on "ErrorsGrid" "Submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	Then I should see "Note for imbalances" interface
	When I click on "AddComment" "submit" "button"
	And I provide value for "AddComment" "comment" "textbox"
	And I click on "unbalancesGrid" "submit" "button"
	When I click on "ConfirmCutoff" "Submit" "button"
	When I navigate to "Operational Cutoff" page
	And I have a record with "Error" state and I searched in "OperationalCutoffTickets" Grid
	Then verify that "tickets" "download" "link" is "disabled"
	And verify that "tickets" "viewError" "link" is "enabled"
	Then I should see the tooltip "see error" text for "tickets" "viewError" "link"
	And I should see the tooltip "to download" text for "tickets" "download" "link"
	When I click on "tickets" "viewError" "link"
	Then I should see a modal window with the error information in "OperationalCutoffs" page

@testcase=12092 @bvt @version=4
Scenario: Verify the functionality when the record is in Completed state
	Given I am having pending records in Operational Cutoff page
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	Then I should see "Start" "link"
	When I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on DatePicker
	When I click on "InitTicket" "submit" "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	Then I should see "Add Note Functions" interface
	When I provide value for "AddComment" "comment" "textbox"
	And I click on "AddComment" "submit" "button"
	Then I should see the message "No existen movimientos e inventarios pendientes" when there are no pending records
	When I click on "ErrorsGrid" "Submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	Then I should see "Note for imbalances" interface
	When I click on "AddComment" "submit" "button"
	And I provide value for "AddComment" "comment" "textbox"
	And I click on "unbalancesGrid" "submit" "button"
	When I click on "ConfirmCutoff" "Submit" "button"
	When I navigate to "Operational Cutoff" page
	And I have a record with "Completed" state and I searched in "OperationalCutoffTickets" Grid
	Then verify that "tickets" "download" "link" is "enabled"
	And verify that "tickets" "viewSummary" "link" is "enabled"
	Then I should see the tooltip "see summary" text for "tickets" "viewSummary" "link"
	And I should see the tooltip "to download" text for "tickets" "download" "link"
	When I click on "tickets" "download" "link"
	Then I should see a page with the operational report
	When I navigate to "Operational Cutoff" page
	And I have a record with "Completed" state and I searched in "OperationalCutoffTickets" Grid
	And I click on "tickets" "viewSummary" "link"
	Then I should see a page with the Operational cutoff summary

@testcase=12093 @version=2
Scenario: Verify the headers of Operational Cutoff page
	Given I am logged in as "admin"
	When I navigate to "Operational Cutoff" page
	Then I see "Operational Cutoffs" header
	When I click on "NewCut" "button"
	Then I see "Execution Operational Cutoff" header

@testcase=12094 @version=4 @manual
Scenario Outline: Verify Filters functionality
	Given I am having pending records in Operational Cutoff page
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	Then I should see "Start" "link"
	When I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on DatePicker
	When I click on "InitTicket" "submit" "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	Then I should see "Add Note Functions" interface
	When I provide value for "AddComment" "comment" "textbox"
	And I click on "AddComment" "submit" "button"
	Then I should see the message "No existen movimientos e inventarios pendientes" when there are no pending records
	When I click on "ErrorsGrid" "Submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	Then I should see "Note for imbalances" interface
	When I click on "AddComment" "submit" "button"
	And I provide value for "AddComment" "comment" "textbox"
	And I click on "unbalancesGrid" "submit" "button"
	When I click on "ConfirmCutoff" "Submit" "button"
	When I navigate to "Operational Cutoff" page
	And I provide the value for "tickets" "<Field>" "<ControlType>" filter in "OperationalCutoffTickets" Grid
	Then I should see the information that matches the data entered for the "<Field>" in "OperationalCutoffTickets" Grid

	Examples:
		| Field                | ControlType |
		| TicketId             | textbox     |
		| categoryElement_name | textbox     |
		| StartDate            | date        |
		| EndDate              | date        |
		| CreatedDate          | date        |
		| CreatedBy            | textbox     |
		| Status               | combobox    |

@testcase=12095 @version=4 @manual
Scenario Outline: Verify Sorting functionality
	Given I am having pending records in Operational Cutoff page
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	Then I should see "Start" "link"
	When I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on DatePicker
	When I click on "InitTicket" "submit" "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	Then I should see "Add Note Functions" interface
	When I provide value for "AddComment" "comment" "textbox"
	And I click on "AddComment" "submit" "button"
	Then I should see the message "No existen movimientos e inventarios pendientes" when there are no pending records
	When I click on "ErrorsGrid" "Submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	Then I should see "Note for imbalances" interface
	When I click on "AddComment" "submit" "button"
	And I provide value for "AddComment" "comment" "textbox"
	And I click on "unbalancesGrid" "submit" "button"
	When I click on "ConfirmCutoff" "Submit" "button"
	When I navigate to "Operational Cutoff" page
	And I click on the "<ColumnName>"
	Then the results should be sorted according to the "<ColumnName>"

	Examples:
		| ColumnName    |
		| Ticket        |
		| Segment       |
		| StartDate     |
		| EndDate       |
		| ExecutionDate |
		| Username      |
		| State         |

@testcase=12096 @version=4 @manual
Scenario: Verify Pagination functionality
	Given I am logged in as "admin"
	When I navigate to "Operational Cutoff" page
	And I navigate to second page in "OperationalCutoffTickets" Grid
	Then the records should be displayed accordingly in "OperationalCutoffTickets" Grid
	When I change the elements count per page to 50
	Then the records count in "OperationalCutoffTickets" Grid shown per page should also be 50