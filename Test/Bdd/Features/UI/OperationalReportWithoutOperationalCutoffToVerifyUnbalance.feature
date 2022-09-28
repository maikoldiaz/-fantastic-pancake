@owner=jagudelos @ui @testsuite=26834 @testplan=26817
Feature: OperationalReportWithoutOperationalCutoffToVerifyUnbalance
In order to verify the unbalance
As a query user
I need to view the operational report by node without operational cutoff

@testcase=28228 @ui
Scenario Outline: Validate balance summary of the segment and system where node has movements and inventories in the selected period
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select the StartDate lessthan "2" days from CurrentDate on "Report" DatePicker
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
Then Balance Operativo report should be displayed
And validate the title "Balance Operativo" is displayed
And validate the title "<Category> <Element> - NodeName" is displayed
And validate the title "Del dd-mmm-YY al dd-mmm-YY" is displayed
And validate  "InitialInventory", "Inputs", "Outputs", "IdentifiedLosses", "FinalInventory" and "Control" are calculated correctly
And validate balance summary values shown with two decimals for "InitialInventory", "Inputs", "Outputs", "IdentifiedLosses", "FinalInventory" and "Control"
Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=28229 @ui
Scenario Outline: Validate balance summary of the segment and system where node has no movements and inventories in the selected period
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select the StartDate lessthan "6" days from CurrentDate on "Report" DatePicker
And I select the FinalDate lessthan "6" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
Then Balance Operativo report should be displayed
And validate the title "Balance Operativo" is displayed
And validate the title "<Category> <Element> - NodeName" is displayed
And validate the title "Del dd-mmm-YY al dd-mmm-YY" is displayed
And validate balance summary table only displaying tiles
Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |
