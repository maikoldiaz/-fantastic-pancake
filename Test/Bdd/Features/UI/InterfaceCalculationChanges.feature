@ui @owner=jagudelos @testplan=35673 @testsuite=35693 @sharedsteps=4013
Feature: InterfaceCalculationChanges
As a TRUE Admin I need to verify the changes made
to the calculation of Interfaces of a product on any
day

@testcase=37358 @ui @bvt @manual @version=2
Scenario: Validate the interface calculation when a node has multiple products with inputs, outputs
Given I want to calculate the "OperationalBalance" in the system
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I should see "Start" "link"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "InitTicket" "submit" "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
And I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
Then I navigate to "Reports" page
And I enter the Segmento, Initial Date and Final Date
And I enter the node for which Inputs and Ouptuts are present
And I click on generate report
And I verify that the interfaces have been calculated for the node

@testcase=37359 @ui @priority=2 @manual @version=2
Scenario: Validate the interface calculation when a node has single product with inputs, outputs
Given I want to calculate the "OperationalBalance" in the system
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I should see "Start" "link"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "InitTicket" "submit" "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
And I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
Then I navigate to "Reports" page
And I enter the Segmento, Initial Date and Final Date
And I enter the node for which Inputs and Ouptuts are present
And I click on generate report
And I Verify that the interface for the node has not been caluclated

@testcase=37360 @ui @priority=2 @manual @version=2
Scenario: Validate the interface calculation for multiple nodes with multiple products
Given I want to calculate the "OperationalBalance" in the system
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I should see "Start" "link"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "InitTicket" "submit" "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
And I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
Then I navigate to "Reports" page
And I enter the Segmento, Initial Date and Final Date
And I enter the todos
And I click on generate report
And I verify that the sum of all interfaces present for all days has been calculated
