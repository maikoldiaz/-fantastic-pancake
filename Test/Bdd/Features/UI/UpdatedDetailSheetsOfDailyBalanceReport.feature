@owner=jagudelos @ui @testsuite=39229 @testplan=39221
Feature: UpdatedDetailSheetsOfDailyBalanceReport
In order to update daily balance report with and without ownership to complement the information
As a query user
I need new data to be included in the detail sheets
@testcase=41504
Scenario Outline: Validate Movement Details Origen Column
Given I have "ownershipnodes" created
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I enter "NodeName" into "Node" "Name" "textbox"
And I click on "<ReportType>" "RadioButton"
And I select the StartDate lessthan "2" days from CurrentDate on "Report" DatePicker
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
And I should see "OperationalReport" "Screen"
And I click on "MovementDetails" "Tab"
Then validate "Origen" column is displayed next to "Unidad" column
And validate data in "Origen" column corresponds to "SystemName" of each movement

Examples:
| ReportType                       |
| OperatinalReport                 |
| OperatinalReportWithoutOwnership |
| OperatinalReportWithOwnership    |
@testcase=41505
Scenario Outline: Validate Inventory Details IdBatch Column
Given I have "ownershipnodes" created
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I enter "NodeName" into "Node" "Name" "textbox"
And I click on "<ReportType>" "RadioButton"
And I select the StartDate lessthan "2" days from CurrentDate on "Report" DatePicker
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
And I should see "OperationalReport" "Screen"
And I click on "InventoryDetails" "Tab"
Then validate "IdBatch" column is displayed next to "Tanque" column
And validate data in "IdBatch" column corresponds to "BatchId" of each inventory

Examples:
| ReportType                       |
| OperatinalReport                 |
| OperatinalReportWithoutOwnership |
| OperatinalReportWithOwnership    |
@testcase=41506
Scenario Outline: Validate Inventory Details IdBatch Column as Blank
Given I have "ownershipnodes" created
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I enter "NodeName" into "Node" "Name" "textbox"
And I click on "<ReportType>" "RadioButton"
And I select the StartDate lessthan "2" days from CurrentDate on "Report" DatePicker
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
And I should see "OperationalReport" "Screen"
And I click on "InventoryDetails" "Tab"
Then validate "IdBatch" column is displayed next to "Tanque" column
And validate Blank in "IdBatch" column when no "BatchId" in inventory

Examples:
| ReportType                       |
| OperatinalReport                 |
| OperatinalReportWithoutOwnership |
| OperatinalReportWithOwnership    |
@testcase=41507
Scenario: Validate Movement Details Porcentaje propiedad Column
Given I have "ownershipnodes" created
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I enter "NodeName" into "Node" "Name" "textbox"
And I click on "OperatinalReportWithOwnership" "RadioButton"
And I select the StartDate lessthan "2" days from CurrentDate on "Report" DatePicker
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
And I should see "OperationalReport" "Screen"
And I click on "MovementDetails" "Tab"
Then validate "Porcentaje propiedad" column is displayed next to "Volumen propiedad" column
And validate "Unidad propiedad" column is not displayed
@testcase=41508
Scenario Outline: Validate Mov. Origen, Pedido, Posición columns in Movements Detail and Movements quality
Given I have "ownershipnodes" created
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I enter "NodeName" into "Node" "Name" "textbox"
And I click on "OperatinalReportWithOwnership" "RadioButton"
And I select the StartDate lessthan "2" days from CurrentDate on "Report" DatePicker
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
And I should see "OperationalReport" "Screen"
And I click on "<Screen>" "Tab"
Then validate "Mov. Origen", "Pedido", "Posición" columns are displayed next to "Origen" column
And validate data in "Mov. Origen", "Pedido", "Posición" columns are obtained from active movements
And validate blank in "Mov. Origen", "Pedido", "Posición" columns when movement does not have corresponding data

Examples:
| Screen          |
| MovementDetails |
| MovementQuality |
@testcase=41509
Scenario Outline: Validate Operational Report With Ownership – Data Validation 1
Given I have "ownershipnodes" created
And there are movements of type "<MovementType>"
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I enter "NodeName" into "Node" "Name" "textbox"
And I click on "OperatinalReportWithOwnership" "RadioButton"
And I select the StartDate lessthan "2" days from CurrentDate on "Report" DatePicker
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
And I should see "OperationalReport" "Screen"
And I click on "<Screen>" "Tab"
Then validate data in "Mov. Origen", "Pedido", "Posición" columns if value of "Origen" column is "FICO"
And validate data in "Pedido", "Posición" columns if value of "Origen" column is "TRUE"
And validate blank in "Mov. Origen" column if value of "Origen" column is "TRUE"

Examples:
| Screen          | MovementType |
| MovementDetails | Purchase     |
| MovementQuality | Sell         |
| MovementDetails | Sell         |
| MovementQuality | Purchase     |
@testcase=41510
Scenario Outline: Validate Operational Report With Ownership – Data Validation 2
Given I have "ownershipnodes" created
And there are movements of type "<MovementType>"
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I enter "NodeName" into "Node" "Name" "textbox"
And I click on "OperatinalReportWithOwnership" "RadioButton"
And I select the StartDate lessthan "2" days from CurrentDate on "Report" DatePicker
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
And I should see "OperationalReport" "Screen"
And I click on "<Screen>" "Tab"
Then validate data in "Mov. Origen" column
And validate blank in "Pedido", "Posición" columns

Examples:
| Screen          | MovementType |
| MovementDetails | ACE Entrada  |
| MovementQuality | ACE Salida   |
| MovementDetails | ACE Salida   |
| MovementQuality | ACE Entrada  |
@testcase=41511 
Scenario Outline: Validate Operational Report With Ownership – Data Validation 3
Given I have "ownershipnodes" created
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I enter "NodeName" into "Node" "Name" "textbox"
And I click on "OperatinalReportWithOwnership" "RadioButton"
And I select the StartDate lessthan "2" days from CurrentDate on "Report" DatePicker
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
And I should see "OperationalReport" "Screen"
And I click on "<Screen>" "Tab"
Then validate blank in "Mov. Origen", "Pedido", "Posición" columns

Examples:
| Screen          |
| MovementDetails |
| MovementQuality |
