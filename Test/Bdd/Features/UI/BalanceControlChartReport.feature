@ui @owner=jagudelos @testplan=31102 @testsuite=31115 @sharedsteps=16534
Feature: BalanceControlChartReport
As a query user
I need a report with the balance control chart
to visualize the behavior of the balance

Background: Login
Given I am logged in as "consulta"

@testcase=32725 @bvt
Scenario: Verify the presence of balance control chart menu under reports
When I click on conveyor balance with property menu
Then I should see "BalanceControlChart" tab

@testcase=32726 @bvt
Scenario: Verify initial page load of balance control chart report
When I perform the operational cutoff process
And I navigate to "BalanceControlChart" page
And I click on "segment" "dropdown"
Then the segment list should show the active elements belongs to the segment category
And node list should not contain any nodes
@testcase=32727
Scenario: Verify node search functionality in balance control chart report
When I perform the operational cutoff process
And I navigate to "BalanceControlChart" page
And I select "CategorySegment" from "CategoryElement"
And I type part of the node name
Then the system must show the node list that match with the value typed and that belong to the selected segment on the current date
@testcase=32728
Scenario: Verify that end date is enabled until the date of the last operational cutoff executed for the selected node
When I perform the operational cutoff process
And I navigate to "BalanceControlChart" page
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
Then I validate the end date should only be enabled until the date of the last operational cutoff executed for the selected node
@testcase=32729
Scenario: Verify mandatory fields
When I perform the operational cutoff process
And I navigate to "BalanceControlChart" page
And I click on "View" "Report" "button"
Then I should see error message "Requerido" message below each field
@testcase=32730
Scenario: Verify that the start-date is greater than the end-date validation
When I perform the operational cutoff process
And I navigate to "BalanceControlChart" page
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" as "Greater than Current Date"
And enter "EndDate" as "Current Date"
And I click on "View" "Report" "button"
Then I validate the error message "La fecha inicial debe ser menor o igual a la fecha final"

@testcase=32731 @bvt
Scenario: View report by node when average uncertainity is not null
When I perform the operational cutoff process
And I navigate to "BalanceControlChart" page
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" as "Greater than Current Date"
And enter "EndDate" as "Current Date"
And I click on "View" "Report" "button"
Then the report title must be showed with the specified standard
And I should see the control chart for the selected node with the totals of the variables unbalance warning action and control tolerance on the dates of the selected period
And I should see the table with the data used to generate the control chart​
And I should see node product list
@testcase=32732
Scenario: View report by node when average uncertainity is null
When I perform the operational cutoff process
And I navigate to "BalanceControlChart" page
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" as "Greater than Current Date"
And enter "EndDate" as "Current Date"
And I click on "View" "Report" "button"
Then the report title must be showed with the specified standard
And I should not see the control chart plotted for the selected node
And I should see the table with the data used to generate the control chart​
And I should see node product list

@testcase=32733 @bvt
Scenario: View report by product
When I perform the operational cutoff process
And I navigate to "BalanceControlChart" page
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" as "Greater than Current Date"
And enter "EndDate" as "Current Date"
And I click on "View" "Report" "button"
And I see node product list
And I click on a product from the list of node products
Then I should see the control chart for the selected product with the values of the variables unbalance warning action and control tolerance on the dates of the selected period
@testcase=32734
Scenario: Verify the report of the balance control chart has the template data configured
When I open the report file and it has the template data settings
Then I should see the template pages with the data configured for the report
@testcase=32735
Scenario: Verify report of the balance control chart does not have the template data configured
When I open the report file and it dont have the template data settings
Then I should see the template pages only with titles
@testcase=32736
Scenario: Verify the columns added in the unbalance table
When I perform the operational cutoff process
Then I should see "StandardUncertainity" "AverageUncertainity" "AverageUncertainityUnbalance" "Warning" "Action" "ControlTolerance" columns in the unbalance table
@testcase=32737
Scenario: Verify the calculations when the inputs total is not equal to zero
When I perform the operational cutoff process
And inputs total is not equal to zero
Then verify the values calculated for all the variables in unbalance table
@testcase=32738
Scenario: Verify the calculations when the inputs total is equal to zero
When I perform the operational cutoff process
And inputs total is equal to zero
Then verify the values calculated for all the variables in unbalance table
@testcase=32739
Scenario: Verify the total calculations of the node
When I perform the operational cutoff process
Then verify the totals calculated for all the variables in unbalance table
@testcase=32740 
Scenario: Verify template information
When I perform the operational cutoff process
And I navigate to "BalanceControlChart" page
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" as "Greater than Current Date"
And enter "EndDate" as "Current Date"
And I click on "View" "Report" "button"
Then I should see ecopetrol template sheets included
And template information should be obtained from the template configuration table
