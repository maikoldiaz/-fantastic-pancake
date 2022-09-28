@sharedsteps=4013 @owner=jagudelos @ui @testsuite=26836 @testplan=26817
Feature: OperationalReportWithOwnershipDayByDay
In order to calculate the operational report with ownership
As a query user
I need that the calculations of the operational report with ownership by segment or system be executed day by day to the data to be consistent

Background: Login
Given I am logged in as "admin"

@testcase=28231 @ui
Scenario Outline: Validate movements and inventories for the segment and system with ownership in the selected period
Given I have "<Category>" ownership calculation data generated in the system
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select the StartDate lessthan "2" days from CurrentDate on "Report" DatePicker
And I select the FinalDate lessthan "2" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
Then Balance Operativo report should be displayed
And validate Balance Summary of "<Category>"
When I Right click on Product and Select "MovementDetails"
Then validate "operational date" of "movement" should be within date range selected
And I click on "Balance" "tab"
When I Right click on Product and Select "InventoryDetails"
Then validate "operational date" of "inventory" should be within date range selected

Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=28232 @ui
Scenario Outline: Validate no movements and inventories for the segment and system with ownership in the selected period
Given I have "<Category>" ownership calculation data generated in the system
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select the StartDate lessthan "2" days from CurrentDate on "Report" DatePicker
And I select the FinalDate lessthan "2" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
Then Balance Operativo report should be displayed
And validate balance summary table only displaying tiles

Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |
