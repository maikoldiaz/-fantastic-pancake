@owner = jagudelos  @testsuite=26103 @testplan=19772
Feature: ReportToVisualizeOperatingBalanceWithOwnershipAndTraceablitiyRulesOperative
As a query user, I need a report  to
view the operational balance with ownership
and the traceability of the rules applied (Operative)


@testcase=26163 @ui
Scenario Outline: Validate blank is displayed in all indicators if no data for the last value
Given I have "Operative Balance" in the system
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
And And validate "<columns>" are blank when no data is present
Examples:
| columns                   |
| Inventario Inicial        |
| Entradas                  |
| Salidas                   |
| Pérdidas Identificadas    |
| Interfases                |
| Tolerancia                |
| Pérdidas No Identificadas |
| Inventario Final          |
| Control                   |
@testcase=26164
Scenario Outline: Validate values for owenership report in operational section are calculated and displayed
Given I have "Operative Balance" in the system
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then validate "BalanceOperativeReport" is displayed for each product and its respective owner
And And validate "<columns>" calculated values are displayed
Examples:
| columns                   |
| Inventario Inicial        |
| Entradas                  |
| Salidas                   |
| Pérdidas Identificadas    |
| Interfases                |
| Tolerancia                |
| Pérdidas No Identificadas |
| Inventario Final          |
| Control                   |

@testcase=26165
Scenario Outline: Validate when any unbalance record is right clicked TRUE user is able to view movements associated
Given I have "Operative Balance" in the system
When I navigate to "OwnershipNodes" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then validate "BalanceOperativeReport" is displayed
And I right click on a cell and click View Movements
And validate "<Columns>" are displayed
Examples:
| Columns                 |
| Movement Identification |
| Date (operational)      |
| Operation               |
| Source Node             |
| Destination Node        |
| Source Product          |
| Destination Product     |
| Net Volume              |
| Gross Volume            |
| Unit                    |
| Owner                   |
| Ownership Volume        |
| Ownership Unit          |
| Ownership process date  |
| Rule                    |
| Movement                |
| Uncertainity            |

@testcase=26166
Scenario Outline: Verify quality details of movements associated with product and owner are displayed
Given I have "Operative Balance" in the system
When I navigate to "OwnershipNodes" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then validate "BalanceOperativeReport" is displayed
And I right click on a cell and click View Movements
And validate "<Columns>" are displayed
Examples:
| Columns             |
| Movement Id         |
| Date                |
| Operation           |
| Source Node         |
| Destination Node    |
| Source Product      |
| Destination Product |
| Net Volume          |
| Gross Volume        |
| Unit                |
| Attribute Value     |
| Attribute Unit      |
| Description         |

@testcase=26167
Scenario: Verify the control value displayed for each owner in each product
Given I have "Operative Balance" in the system
When I navigate to "OwnershipNodes" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
And The control value is to be displayed
Then I verify the control value is the same as Initial Inventory +  Inputs - Outputs - Identified Losses +/- Interfases +/- Tolerance+/- Unidentified Losses -Final Inventory in the other columns

@testcase=26168
Scenario: Verify I'm able to see aggregated variable only for the node selected
Given I have "Operative Balance" in the system
When I navigate to "OwnershipNodes" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I verify if the data displayed is for the node I selected
@testcase=26169
Scenario: Verify I'm able to see aggregated variable for date range between start date and end date
Given I have "Operative Balance" in the system
When I navigate to "OwnershipNodes" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I verify the aggregated variables displayed is for the date range selected
@testcase=26170
Scenario: Verify the Initial Inventories are displayed from start date minus 1
Given I have "Operative Balance" in the system
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I verify all intial inventories are displayed from the selected Start Date -1
@testcase=26171
Scenario: Verify the Final inventories are displayed until the End Date
Given I have "Operative Balance" in the system
When I navigate to "OwnershipNodes" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I verify all the final inventories are displayed until the end date and no others
@testcase=26172 
Scenario: Verify I'm able to export the report to an Excel
Given I have "Operative Balance" in the system
When I navigate to "OwnershipNodes" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I click on export to excel
And I verify the excel report is generated





