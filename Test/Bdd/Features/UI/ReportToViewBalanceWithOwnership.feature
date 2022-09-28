@owner=jagudelos @ui @MVP2and3 S16 @testsuite=70802 @testplan=70526
Feature: ReportToViewBalanceWithOwnership
As a user of chain, I require a report to view
the balance with ownership by other segments
@testcase=71743
Scenario Outline: Verify report template information
Given I am logged in as "<User>"
And I have a report to view the balance with ownership by other segments
When I navigate to "Operational Balance With Ownership Report" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select the radio button "Con propiedad"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I should see ecopetrol template sheets included
And template information should be obtained from the template configuration table

Examples:
| User  |
| admin |
| chain |
@testcase=71744
Scenario Outline: Verify report Header
Given I am logged in as "<User>"
And I have a report to view the balance with ownership by other segments
When I navigate to "Operational Balance With Ownership Report" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select the radio button "Con propiedad"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I should see breadcrumb "Balance operativo con propiedad por nodo"

Examples:
| User  |
| admin |
| chain |
@testcase=71745
Scenario Outline: Verify report Balance sheet Tab
Given I am logged in as "<User>"
And I have a report to view the balance with ownership by other segments
When I navigate to "Operational Balance With Ownership Report" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select the radio button "Con propiedad"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I should see breadcrumb "Balance operativo con propiedad por nodo"
And I click on "Balance" "Tab"
And I verify Display information in tabular form
And I verify records grouped by product and owner
And I verify display voids instead of null values
And I verify all columns "Producto","Propietario","Unidad","Inv. inicial","Entradas","Salidas","Pérdidas identificadas","Interfases","Tolerancia","Inv. final","Control" are present in Grid
And I verify the control Value

Examples:
| User  |
| admin |
| chain |
@testcase=71746
Scenario Outline: Verify report Detalle Movimientos Tab
Given I am logged in as "<User>"
And I have a report to view the balance with ownership by other segments
When I navigate to "Operational Balance With Ownership Report" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select the radio button "Con propiedad"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I should see breadcrumb "Balance operativo con propiedad por nodo"
And I click on "Detalle Movimientos" "Tab"
And I verify Display information in tabular form
And I verify records are sorted by id movement and posting date ascending.
And I verify display voids instead of null values
And I verify all columns "Id movimiento","Id batch","Fecha","Tipo movimiento","Nodo origen","Nodo destino","Producto origen","Producto destino","Cantidad neta","Cantidad bruta","Unidad","Origen","Mov. origen","Pedido","Posición","Propietario","Volumen propiedad","Porcentaje propiedad","Movimiento","Incertidumbre","Id movimiento respaldo","Id movimiento global" are present in Grid

Examples:
| User  |
| admin |
| chain |
@testcase=71747
Scenario Outline: Verify report Calidad movimientos sheet Tab
Given I am logged in as "<User>"
And I have a report to view the balance with ownership by other segments
When I navigate to "Operational Balance With Ownership Report" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select the radio button "Con propiedad"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I should see breadcrumb "Balance operativo con propiedad por nodo"
And I click on "Calidad movimientos" "Tab"
And I verify Display information in tabular form
And I verify records are sorted by id movement and posting date ascending.
And I verify display voids instead of null values
And I verify all columns "Id movimiento","Id batch","Fecha","Tipo movimiento","Nodo origen","Nodo destino","Producto origen","Producto destino","Cantidad neta","Cantidad bruta","Unidad","Origen","Mov. origen","Pedido","Posición","Id atributo","Valor atributo","Unidad atributo","Descripción atributo" are present in Grid

Examples:
| User  |
| admin |
| chain |
@testcase=71748
Scenario Outline: Verify report Detalle inventarios sheet Tab
Given I am logged in as "<User>"
And I have a report to view the balance with ownership by other segments
When I navigate to "Operational Balance With Ownership Report" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select the radio button "Con propiedad"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I should see breadcrumb "Balance operativo con propiedad por nodo"
And I click on "Detalle Inventarios" "Tab"
And I verify Display information in tabular form
And I verify records are sorted by id date and posting date ascending
And I verify display voids instead of null values
And I verify all columns "Identificación del inventario","Fecha","Nodo","Tanque","Id batch","Producto","Cantidad neta","Cantidad bruta","Unidad","Origen","Incertidumbre","Propietario","Volumen propiedad","Porcentaje propiedad" are present in Grid

Examples:
| User  |
| admin |
| chain |
@testcase=71749
Scenario Outline: Verify report calidad inventarios sheet Tab
Given I am logged in as "<User>"
And I have a report to view the balance with ownership by other segments
When I navigate to "Operational Balance With Ownership Report" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select the radio button "Con propiedad"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I should see breadcrumb "Balance operativo con propiedad por nodo"
And I click on "Calidad Inventarios" "Tab"
And I verify Display information in tabular form
And I verify records are sorted by id date and posting date ascending
And I verify display voids instead of null values
And I verify all columns "Identificación del inventario","Fecha","Nodo","Tanque","Id batch","Producto","Cantidad neta","Cantidad bruta","Unidad","Origen","Id atributo","Valor atributo","Unidad atributo","Descripción atributo" are present in Grid

Examples:
| User  |
| admin |
| chain |
@testcase=71750
Scenario Outline: Verify report Modify criteria selection
Given I am logged in as "<User>"
And I have a report to view the balance with ownership by other segments
When I navigate to "Operational Balance With Ownership Report" page
Then verify current warning message not displayed the “operativo” word
And verify options for selecting one of the two reports
And I verify Option “Reporte operativo” selected by default
And I select the radio button "Reporte operativo"
And I verify “Todos” option displayed in the node selection
And I validate the end date should only be enabled until the current date minus one day
And I select the radio button "Con propiedad"
And I verify “Todos” option is removed in the node selection
And I validate the end date should only be enabled until the current date minus one day

Examples:
| User  |
| admin |
| chain |
@testcase=71751 
Scenario Outline: Verify export to excel functionality
Given I am logged in as "<User>"
And I have a report to view the balance with ownership by other segments
When I navigate to "Operational Balance With Ownership Report" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select the radio button "Con propiedad"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I should see breadcrumb "Balance operativo con propiedad por nodo"
And I click on "Export to Excel" "link"
Then report should be exported to excel with proper columns and data

Examples:
| User  |
| admin |
| chain |
