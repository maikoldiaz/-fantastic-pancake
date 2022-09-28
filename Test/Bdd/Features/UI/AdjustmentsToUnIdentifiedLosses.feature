@owner=jagudelos @ui @testsuite=19788 @testplan=19772
Feature: AdjustmentsToUnIdentifiedLosses
In order to perform operation cutoff of volumetric information
As an application administrator
I want to perform unidentified losses for operational Cutoff summary

@testcase=21175 @bvt
Scenario: Validate Unidentified Losses calculation for movements
Given I want to calculate the "OperationalBalance" in the system
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Inicio" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
Then I should see the unidentified losses for each product
And I should see the movements for each unidentified losses

@testcase=21176 @manual
Scenario Outline: Verify the errors while registering the Unidentified Losses for movements
Given I have the data with errors to register the movements in the system
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Inicio" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
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
