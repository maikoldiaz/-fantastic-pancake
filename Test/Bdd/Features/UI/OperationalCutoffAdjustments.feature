@sharedsteps=4013 @Owner=jagudelos @ui @testplan=11317 @testsuite=11326
Feature: OperationalCutoffAdjustments
In order to calculate Operational Balance Day by Day
As a transport segment user
I need to make some adjustments to Operational Cutoff Process

@testcase=12075 @bvt @version=5 @prodready
Scenario: Verify the Node Interfaces/Balance Tolerance/Unidentified losses calculated for each day
	Given I want to calculate the daywise "OperationalBalance" in the system
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
	When I click on "InitTicket" "submit" "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	Then validate that "ErrorsGrid" "Submit" "button" as enabled
	When I click on "ErrorsGrid" "Submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "submit" "button"
	When I click on "unbalancesGrid" "submit" "button"
	When I click on "ConfirmCutoff" "Submit" "button"
	Then I should see the Node Interfaces/Balance Tolerance/Unidentified losses are calculated for each day

@testcase=12076 @manual
Scenario Outline: Verify the errors registered during execution process of the Operational cutoff
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I select CategoryElement from "Segment" dropdown
	And I select the FinalDate greater than CurrentDate from DatePicker
	When I click on "initTicket" "apply" "button"
	And I select all pending repositories from grid
	And I click on "Notes" "button"
	Then I should see "Add Note Functions" interface
	When I enter valid value into "Add Notes" "textbox"
	And I click on "AddComment" "submit" "button"
	Then I should see "ErrorsGrid" "Submit" "button" as enabled
	When I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	Then I should see "Note for imbalances" interface
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "submit" "button"
	Then I should see the errors registered with below <Details>

	Examples:
		| Details        |
		| Ticket         |
		| Segment        |
		| Initial Date   |
		| End Date       |
		| Execution Date |
		| User           |
		| State          |
		| Error          |