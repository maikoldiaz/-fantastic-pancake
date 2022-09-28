@owner = jagudelos @ui @testplan=19772 @testsuite=19778
Feature: ReportToVisualizeInventoryDetailsOfOperationalBalanceWithOwnership
As a query user, I need a report  to
view the operational balance with ownership
and the traceability of the rules applied (InventoryDetails)



@testcase=21404 @ui
Scenario Outline: Validate blank is displayed in all indicators if no data for the last value
Given I have "Operative Balance" in the system
When I navigate to "OwnershipNodeReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
And And validate "<columns>" are blank when no data is present
Examples:
| Columns              |
| Identificación del inventario       |
| Fecha                |
| Nodo                 |
| Tanque               |
| Producto             |
| Volumen Neto         |
| Unidad               |
| Valor Atributo       |
| Unidad Atributo      |
| Descripción Atributo |

@testcase=21405
Scenario Outline: Verify as a Query User I am able to see the inventory details of a particular segment
Given I have "Operative Balance" in the system
When I navigate to "OwnershipNodeReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then validate when i click on any cell in the report summary and select the option to see the inventory detail​
And And validate "<columns>" calculated values are displayed
Examples:
| Columns              |
|Identificación del inventario        |
| Fecha                |
| Nodo                 |
| Tanque               |
| Producto             |
| Volumen Neto         |
| Unidad               |
| Valor Atributo       |
| Unidad Atributo      |
| Descripción Atributo |

@testcase=21406 
Scenario:  Verify as a Query user I am able to see the quality details of inventory
Given I have "Operative Balance" in the system
When I navigate to "OwnershipNodeReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
And I right click on any cell in the report summary and select the option to see the quality detail​ of the inventory
Then I verify I am able to see the quality details of inventories

