@owner=jagudelos @ui @testplan=61542 @testsuite=61551 @S15 @MVP2and3 @manual
Feature: ReportToViewOfficialBalanceByNode
In order to view the official balance by node
As a user of chain
I want to view official balance by node report
@testcase=68033
Scenario Outline: Validate official balance by node report is displayed for user administrator, consultant and chain
Given I am logged in as "<User>"
When I navigate to "Supply chain management" menu
Then validate "Official balance by node" option is displayed

Examples:
| User     |
| admin    |
| consulta |
| chain    |
@testcase=68034
Scenario Outline: Validate official balance by node report is not displayed for other user
Given I am logged in as "<User>"
When I navigate to "Supply chain management" menu
Then validate "Official balance by node" option is not displayed

Examples:
| User        |
| aprobador   |
| profesional |
| programador |
@testcase=68035 @version=2
Scenario: Validate the template of official balance by node report
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
Then validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Detalle de movimientos" tab
And validate the report page "Detalle de movimientos" is displayed
And I click on "Detalle de inventarios" tab
And validate the report page "Detalle de inventarios" is displayed
@testcase=68036 @version=2
Scenario: Validate the graph current unit
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
Then validate "BBL" is selected by default in "Current Unit"
And validate that "Unidad actual" "label" is displayed
@testcase=68037 @version=2
Scenario: Validate the graph delta initial inventory
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
Then validate that percentage value is calculated correctly in "Delta Initial Inventory"
And validate that "Delta inv. inicial" "label" is displayed
@testcase=68038 @version=2
Scenario: Validate the graph delta inputs
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
Then validate that percentage value is calculated correctly in "Delta Inputs"
And validate that "Delta entradas" "label" is displayed
@testcase=68039 @version=2
Scenario: Validate the graph delta outputs
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
Then validate that percentage value is calculated correctly in "Delta Outputs"
And validate that "Delta salidas" "label" is displayed
@testcase=68040 @version=2
Scenario: Validate the graph delta final inventory
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
Then validate that percentage value is calculated correctly in "Delta Final Inventory"
And validate that "Delta inv. final" "label" is displayed
@testcase=68041 @version=2
Scenario: Validate the graph control
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
Then validate that percentage value is calculated correctly in "Control"
And validate value in "Control" displayed as "Green" if value is 0
And validate value in "Control" displayed as "Red" if value is other than 0
And validate that "Control" "label" is displayed
@testcase=68042 @version=2
Scenario: Validate the graph dynamics of the official balance by product and owner
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
Then validate that graph title as "DINÁMICA DEL BALANCE OFICIAL POR PRODUCTO Y PROPIETARIO"
And validate that graph shows "Green" bar if value is positive
And validate that graph shows "Red" bar if value is negative
@testcase=68043 @version=2
Scenario: Validate filters in the official balance by node
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
Then validate 3 filters are displayed in the left pane
And validate the filter in "Official balance by node"
| filters   |
| UNIDAD    |
| PRODUCTO  |
| PROPIEDAD |
And validate "Unidad" filter is configured with "BBL" by default
And validate "Unidad" filter allow selection of one value at a time
And validate "Producto" filter is configured with "All" option by default
And validate "Propiedad" filter is configured with "All" option by default
And validate the graph "dynamics of the official balance by product and owner" is changed when filter values are changed
And validate tabular format is not updated with filter selection
@testcase=68044 @version=2
Scenario: Validate official balance by node page
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
Then validate the tabular columns in "Official balance by node"
| columns            |
| Unidad             |
| Inv. initial       |
| Delta inv. inicial |
| Entradas           |
| Delta entradas     |
| Salidas            |
| Delta salidas      |
| Inv. final         |
| Delta inv. final   |
| Control            |
And validate records in table are grouped by unit, product and owner
And validate column is empty if the value is Null
And validate calculated values in the rows corresponding to the columns are grouped as per the input records
@testcase=68045 @version=2
Scenario: Validate movement detail sheet in official balance by node report
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Detalle de movimientos" tab
Then validate the report page "Detalle de movimientos" is displayed
And validate the columns in "Detalle de movimientos"
| columns              |
| Fecha inicio         |
| Fecha final          |
| Movimiento           |
| Tipo de movimiento   |
| Nodo origen          |
| Nodo destino         |
| Producto origen      |
| Producto destino     |
| Cantidad neta        |
| Cantidad bruta       |
| Unidad               |
| Propiedad            |
| Cantidad propiedad   |
| Porcentaje propiedad |
| Escenario            |
| Versión              |
| Origen               |
| Fecha ejecución      |
And validate the filter displayed in "Detalle de movimientos"
| filters            |
| PROPIEDAD          |
| MOVIMIENTO         |
| TIPO DE MOVIMIENTO |
| ORIGEN             |
| UNIDAD             |
| VERSIÓN            |
And validate the filter "Propiedad" is selected as per selected in "Official balance by node"
And validate the filters "Movimiento", "Tipo de Movimiento", "Origen", "Unidad", "Versión" are selected as "All" by default
And validate the movements are displayed as per the filter applied
And validate movements must be sorted by "Nodo destino", "Nodo origen" and "Tipo de Movimiento"
And validate values in "Cantidad neta", "Cantidad bruta", "Cantidad propiedad" are shown with a thousand separator
And validate values in "Cantidad neta", "Cantidad bruta", "Cantidad propiedad", "Porcentaje propiedad" are shown with two decimal places
And validate value in "Fecha inicio", "Fecha final", "Fecha ejecución" are in format "dd-MMM-yy"
And validate column is empty if the value is Null
@testcase=68046 @version=2
Scenario: Validate filters in movement detail sheet of official balance by node report
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Detalle de movimientos" tab
Then validate the report page "Detalle de movimientos" is displayed
And I select one or more elements in "Propiedad" filter
And validate movements that matches selected filter are displayed
And I select one or more elements in "Movimiento" filter
And validate movements that matches selected filter are displayed
And I select one or more elements in "Tipo de Movimiento" filter
And validate movements that matches selected filter are displayed
And I select one or more elements in "Origen" filter
And validate movements that matches selected filter are displayed
And I select one or more elements in "Unidad" filter
And validate movements that matches selected filter are displayed
And I select one or more elements in "Versión" filter
And validate movements that matches selected filter are displayed
@testcase=68047 @version=2
Scenario: Validate inventory detail sheet in official balance by node report
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Detalle de inventarios" tab
Then validate the report page "Detalle de inventarios" is displayed
And validate the columns in "Detalle de inventarios"
| columns              |
| Fecha                |
| Nodo                 |
| Producto             |
| Cantidad neta        |
| Cantidad bruta       |
| Unidad               |
| Propiedad            |
| Cantidad propiedad   |
| Porcentaje propiedad |
| Escenario            |
| Origen               |
| Fecha ejecución      |
And validate the filter displayed in "Detalle de inventarios"
| filters   |
| NODO      |
| PROPIEDAD |
| ORIGEN    |
| UNIDAD    |
And validate the filters "Propiedad" is selected as per selected in "Official balance by node"
And validate the filters "Nodo", "Origen", "Unidad" are selected as "All" by default
And validate the inventories are displayed as per the filter applied
And validate inventories must be sorted by "Producto" and "Propietario"
And validate values in "Cantidad neta", "Cantidad bruta", "Cantidad propiedad" are shown with a thousand separator
And validate values in "Cantidad neta", "Cantidad bruta", "Cantidad propiedad", "Porcentaje propiedad" are shown with two decimal places
And validate value in "Fecha", "Fecha ejecución" are in format "dd-MMM-yy"
And validate column is empty if the value is Null
@testcase=68048 @version=2
Scenario: Validate filters in inventory detail sheet of official balance by node report
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Detalle de inventarios" tab
Then validate the report page "Detalle de inventarios" is displayed
And I select one or more elements in "Nodo" filter
And validate inventories that matches selected filter are displayed
And I select one or more elements in "Propiedad" filter
And validate inventories that matches selected filter are displayed
And I select one or more elements in "Origen" filter
And validate inventories that matches selected filter are displayed
And I select one or more elements in "Unidad" filter
And validate inventories that matches selected filter are displayed
@testcase=68049
Scenario: Validate official balance by node report where selected node does not have data in the selected period
Given I am logged in as "admin"
When I navigate to "Initial official balance loaded" page
And I have no official "movements", "delta movements", "inventories" and "delta inventories" in the system
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And validate the page is displayed as empty tiles
@testcase=68050 @version=2
Scenario: Validate the hidden pages of official balance by node report
Given I am logged in as "admin"
When I have official delta in the system
And I navigate to "Official balance by node" page
And I enter the required fields for official balance by node report
And I click on "ReportFilter" "ViewReport" "button"
And I open the report file in powerbi
Then validate "Cover" page hidden in UI
And validate "Confidentiality agreement" page hidden in UI
And validate "Log" page hidden in UI
And validate the report is loaded with default page "Official balance by node"
And Validate the title "Balance Oficial por Nodo" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Detalle de movimientos" tab
And validate the report page "Detalle de movimientos" is displayed
And I click on "Detalle de inventarios" tab
And validate the report page "Detalle de inventarios" is displayed
