@sharedsteps=16581 @owner=jagudelos @ui @testplan=24148 @testsuite=24161
Feature: ValidationOfAnalyticalModelDataInOwnershipCalculation
As a Professional Segment Balance User,
I need to modify the ownership calculation to include a validation
of data loaded to the analytical model

Background: Login
Given  I am logged in as "profesional"
@testcase=25283
Scenario: Inclusion of data loaded to the analytical model validation
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then I should see "Operational information loaded in the analytical model" validation group

@testcase=25284 @bvt
Scenario: Successful validation of data loaded to the analytical model
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
And the valid data is being loaded into analytical model
Then I should see message "Información operativa para el segmento cargada en el sistema analítico."
@testcase=25285 
Scenario: Failed validation of data loaded to the analytical model
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
And the valid data is not being loaded into analytical model
Then I should see message "La información operativa para el segmento no ha terminado de cargarse en el sistema analítico."
