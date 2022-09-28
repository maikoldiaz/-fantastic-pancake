@sharedsteps=4013 @owner=jagudelos  @ui @testplan=39221 @testsuite=39231
Feature: AddOrderAndPositionColumnsInformationToLogisticReport
As a Balance Segment Professional User, I need the logistic report to be updated to add the order and position data

Background: Login
Given I am logged in as "profesional"
@testcase=41376
Scenario: Verify that the order and position columns are added to the logistic reports
Given I have ownership calculation data generated in the system
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I should see "CreateLogistics" "Create" "Interface"
And I selected Segment from "Segment" "CreateLogistics" "combobox"
And I selected TODOS from the list of options provided under the node dropdown
And I select Owner on the Create file interface
And I select Start date and End Date on Create file Interface
And I click on "CreateLogistics" "Submit" "button"
Then I should see Logistic Report for selected segment in the Logistic Report Generation page
And verify that the order and position data is updated in the movement sheet for the entries which have associated purchase or sales contracts
And verify that the order value is alphanumeric
@testcase=41377 
Scenario: Verify that the order and position columns are empty in logisitic reports for movements which do not have related purchase or sales contracts
Given I have ownership calculation data generated in the system
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I should see "CreateLogistics" "Create" "Interface"
And I selected Segment from "Segment" "CreateLogistics" "combobox"
And I selected TODOS from the list of options provided under the node dropdown
And I select Owner on the Create file interface
And I select Start date and End Date on Create file Interface
And I click on "CreateLogistics" "Submit" "button"
Then I should see Logistic Report for selected segment in the Logistic Report Generation page
And verify that the order and position data is empty in the movement sheet for the entries which do not have any associated purchase or sales contracts
