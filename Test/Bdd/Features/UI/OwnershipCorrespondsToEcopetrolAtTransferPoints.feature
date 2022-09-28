@sharedsteps=4013 @owner=jagudelos @ui @testplan=19772 @testsuite=19782
Feature: OwnershipCorrespondsToEcopetrolAtTransferPoints
In order to calculate Ownership Corresponds to the Ecopetrol at Transfer Points
As TRUE system, I need to consume the analytical models
service to get the ownership that corresponds to Ecopetrol
at the transfer points

Background: Login
Given I am logged in as "admin"

@testcase=21308
Scenario: Verify Excel file generation when segment has no connections marked as a transfer point
Given I have ownership calculation for segment
When I did not have Transfer points for that segment
Then generation of Excel should be processed without data from Analytical Model

@testcase=21309
Scenario: Verify Excel file generation when segment has connections marked as a transfer point but there are no operational movements
Given I have ownership calculation for segment
And I have Transfer points for that segment
When I did not have operational movements for segment which is having Transfer Points
Then generation of Excel should be processed without data from Analytical Model

@testcase=21310
Scenario: Verify Excel file generation when analytical model service does not return data
Given I have ownership calculation for segment
And I have Transfer points for that segment
When the analytical model service does not return data
And continue processing the next pending transfer movement
Then Excel should be processed without data from Analytical Model

@testcase=21311
Scenario: Verify Excel file generation process when segment has connections marked as a transfer point
Given I have ownership calculation for segment
And I have Transfer points for that segment
When The analytical model service return data
And I have temporarily register for each movement the ownership that corresponds to Ecopetrol and Others
And continue processing the next pending transfer movement
Then Excel file should be generated with ownership of the movements calculated with the analytical models service

@testcase=21312 @bvt
Scenario: Verify stored Ownership records for Successful ownership response
Given I have ownership calculation for segment
And I have Transfer points for that segment
When The analytical model service return data
And I have temporarily register for each movement the ownership that corresponds to Ecopetrol and Others
And continue processing the next pending transfer movement
Then ownership records calculated with the analytical models service should be stored in final ownership tables​
And ownership records corresponding to the ticket number from the temporary table is Removed

@testcase=21313
Scenario: Verify temporary Ownership records for Ticket when Ownership response not successful
Given I have Excel file with the errores about ownership calculation
When ticket has temporary ownership records calculated with the analytical models service​
Then ownership records corresponding to the ticket number from the temporary table is Removed

@testcase=21314 
Scenario: Verify that ownership calculation process to continue when the ownership of the inputs from another segment is not calculated
Given I have "ownershipnodes" created
When the ownership of the inputs from other segments is not calculated​
And I have Transfer points for that segment
And I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
Then I should see "Start" "link"
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
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculation" "Next" "button"
Then verify that "ownershipCalulationValidations" "Submit" "button" is "enabled"
