@sharedsteps=4013 @owner=jagudelos @ui @testplan=26817 @testsuite=26835
Feature: ReportToVisualizeOperationalBalanceWithoutOwnership
As a query user,
I need that the calculations of the operational report without ownership by segment or system be executed day by day
so that the data to be consistent

Background: Login
Given I am logged in as "admin"
@testcase=28251 version = 2
Scenario:Validate the operational report when there are no movements and inventories for the system in the selected period
When I navigate to "OperativeBalanceReport" page
And I select "Sistema" from "Category"
And I select "CategorySystem" from "CategoryElement"
And I select "Todos" from "Node"
And I select the radio button "Con corte operativo"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I should see the balance summary only with the titles

@testcase=28252 @bvt version = 2
Scenario:Validate the operational report when there are movements and inventories for the system in the selected period
Given that I have operating balance without ownership calculated for system
When I navigate to "OperativeBalanceReport" page
And I select "Sistema" from "Category"
And I select "CategorySystem" from "CategoryElement"
And I select "Todos" from "Node"
And I select the radio button "Con corte operativo"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I should see the balance summary for the system
And I validate the sum of the values of the below variables for each product
| Key                 |
| Inputs              |
| Outputs             |
| Identified Losses   |
| Interfaces          |
| Tolerance           |
| Unidentified Losses |
And I validate for the initial inventories the values calculated on first day of the period should be used
And I validate for the final inventories the values calculated on the last day of the period should be used
And I should see all values with two decimal places
@testcase=28253 version = 2
Scenario:Validate the operational report when there are no movements and inventories for the segment in the selected period
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Todos" from "Node"
And I select the radio button "Con corte operativo"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I should see the balance summary only with the titles
@testcase=28254 version = 2
Scenario:Validate the operational report when there are movements and inventories for the segment in the selected period
Given that I have operating balance without ownership calculated for segment
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Todos" from "Node"
And I select the radio button "Con corte operativo"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I should see the balance summary for the segment
And I validate the sum of the values of the below variables for each product
| Key                 |
| Inputs              |
| Outputs             |
| Identified Losses   |
| Interfaces          |
| Tolerance           |
| Unidentified Losses |
And I validate for the initial inventories the values calculated on first day of the period should be used
And I validate for the final inventories the values calculated on the last day of the period should be used
And I should see all values with two decimal places
