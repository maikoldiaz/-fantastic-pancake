@sharedsteps=25190 @Owner=jagudelos @ui @testplan=24148 @testsuite=24160
Feature: OperationalCutoffModifications
As a Segment Balance Professional, I need to modify the operational cut-off
to invoke the operational load of data for the analytical model.

Background: Login
Given I am logged in as "admin"

@testcase=25191 @bvt @manual
Scenario: Verify invoke of operational load of data when operational cut-off was succesfull
Given I want to calculate the "OperationalCutoff" in the system
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then I should see the Process Id for file tracking
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
Then I should see the Node Interfaces/Balance Tolerance/Unidentified losses are calculated for each day
And I should see operational loading of analytical model
And I should see the execution log with segment and ticket

@testcase=25192 @manual
Scenario: Verify invoke of operational load of data when operational cut-off was Unsuccesfull
Given I want to calculate the "OperationalCutoff" in the system
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then I should see the Process Id for file tracking
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
And I remove the Application Configuration
And I click on "NewCut" "button"
Then I should not see operational loading of analytical model

@testcase=25193 @manual
Scenario: Verify invoke of operational load of data when operational cut-off was succesfull but loading failed
Given I want to calculate the "OperationalCutoff" in the system
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then I should see the Process Id for file tracking
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
And I create error for loading to fail
And I click on "NewCut" "button"
Then I should not error log with Ticket and Segment value





