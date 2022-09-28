@owner=jagudelos @ui @testplan=61542 @testsuite=61544 @manual @S15 @MVP2and3
Feature: ReportToViewInitialOfficialBalance
In order to view the initial official balance loaded
As a chain user
I want to view initial official balance report

@testcase=66935
Scenario Outline: Validate balance official report is displayed for user administrator and consultant
Given I am logged in as "<User>"
When I navigate to "Supply chain management" menu
Then validate "Initial official balance loaded" option is displayed

Examples:
| User     |
| admin    |
| consulta |

@testcase=66936 @verson=2
Scenario Outline: Validate balance official report is not displayed for other user
Given I am logged in as "<User>"
When I navigate to "Supply chain management" menu
Then validate "Initial official balance loaded" option is not displayed

Examples:
| User        |
| aprobador   |
| profesional |
| programador |

@testcase=66937
Scenario: Validate the template of initial official balance report
Given I am logged in as "admin"
When I have official movements and inventories in the system
And I navigate to "Initial official balance loaded" page
And I enter the required fields for Initial official balance report
And I click on "ReportFilter" "ViewReport" "button"
Then validate the report is loaded with default page "Initial official balance loaded"
And Validate the title "Balance Oficial Inicial Cargado" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Detalle de movimientos" tab
And validate the report page "Detalle de movimientos" is displayed
And I click on "Atributos movimientos" tab
And validate the report page "Atributos movimientos" is displayed
And I click on "Detalle de inventarios" tab
And validate the report page "Detalle de inventarios" is displayed
And I click on "Atributos inventarios" tab
And validate the report page "Atributos inventarioss" is displayed

@testcase=66938
Scenario: Validate initial official balance report where selected node does not have official movements or inventories in the selected period
Given I am logged in as "admin"
When I navigate to "Initial official balance loaded" page
And I do not have official "movements" and "inventories" in the system
And I enter the required fields for Initial official balance report
And I click on "ReportFilter" "ViewReport" "button"
Then validate the report is loaded with default page "Initial official balance"
And Validate the title "Balance Oficial Inicial Cargado" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And validate the page is displayed as blank

@testcase=66939
Scenario: Validate filters in the initial official balance loaded
Given I am logged in as "admin"
When I navigate to "Initial official balance loaded" page
And I have official movements and inventories in the system
And I enter the required fields for Initial official balance report
And I click on "ReportFilter" "ViewReport" "button"
Then validate the report is loaded with default page "Initial official balance loaded"
And Validate the title "Balance Oficial Inicial Cargado" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And validate 5 filters are displayed in the left pane
And validate the filter in "Initial Official Balance Loaded"
| filters   |
| SISTEMA   |
| VERSIÓN   |
| UNIDAD    |
| PRODUCTO  |
| PROPIEDAD |
And validate "Version" filter is configured with "All" option by default
And validate "Sistema" filter is configured with "All" option by default
And validate "Unidad" filter is configured with "All" option by default
And validate "Producto" filter is configured with "All" option by default
And validate "Propiedad" filter is configured with "All" option by default

@testcase=66940
Scenario: Validate initial official balance loaded page
Given I am logged in as "admin"
When I navigate to "Initial official balance loaded" page
And I have official movements and inventories in the system
And I enter the required fields for Initial official balance report
And I click on "ReportFilter" "ViewReport" "button"
Then validate the report is loaded with default page "Initial official balance loaded"
And Validate the title "Balance Oficial Inicial Cargado" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And validate the columns in "Initial Official Balance Loaded"
| columns      |
| Unidad       |
| Inv. initial |
| Entradas     |
| Salidas      |
| Inv. final   |
| Control      |
And validate records in table are grouped by unit, product and owner
And validate calculated values in the rows corresponding to the columns are grouped as per the input records
And validate column is empty if the value is Null

@testcase=66941
Scenario: Validate movement detail sheet
Given I am logged in as "admin"
When I navigate to "Initial official balance loaded" page
And I have official movements and inventories in the system
And I enter the required fields for Initial official balance report
And I click on "ReportFilter" "ViewReport" "button"
Then validate the report is loaded with default page "Initial official balance loaded"
And Validate the title "Balance Oficial Inicial Cargado" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Detalle de movimientos" tab
And validate the report page "Detalle de movimientos" is displayed
And validate the columns in "Detalle de movimientos"
| columns              |
| Sistema              |
| Versión              |
| Id movimiento        |
| Tipo de movimiento   |
| Movimiento           |
| Nodo origen          |
| Nodo destino         |
| Producto origen      |
| Producto destino     |
| Cantidad neta        |
| Cantidad bruta       |
| Unidad               |
| Propietario          |
| Cantidad propiedad   |
| Porcentaje propiedad |
| Origen               |
| Fecha de registro    |
And validate the filter displayed in "Detalle de movimientos"
| filters            |
| SISTEMA            |
| VERSIÓN            |
| NODO ORIGEN        |
| NODO DESTINO       |
| MOVIMIENTO         |
| TIPO DE MOVIMIENTO |
| PROPIEDAD          |
| ORIGEN             |
| UNIDAD             |
And validate the filters "Sistema", "Version", "Propiedad", "Unidad" are selected as per selected in "Initial Official Balance Loaded"
And validate the filters "Nodo origen", "Nodo destino", "Movimiento", "Tipo de Movimiento", "Origen" are selected as "All" by default
And validate the movements are displayed as per the filter applied
And validate movements must be sorted by "Nodo destino", "Nodo origen" and "Tipo de Movimiento"
And validate values in "Cantidad neta", "Cantidad bruta", "Cantidad propiedad" are shown with a thousand separator
And validate values in "Cantidad neta", "Cantidad bruta", "Cantidad propiedad", "Porcentaje propiedad" are shown with two decimal places
And validate value in "Fecha de registro" is in format "dd-MMM-yy"
And validate column is empty if the value is Null

@testcase=66942
Scenario: Validate filters in movement detail sheet
Given I am logged in as "admin"
When I navigate to "Initial official balance loaded" page
And I have official movements and inventories in the system
And I enter the required fields for Initial official balance report
And I click on "ReportFilter" "ViewReport" "button"
Then validate the report is loaded with default page "Initial official balance loaded"
And Validate the title "Balance Oficial Inicial Cargado" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Detalle de movimientos" tab
And validate the report page "Detalle de movimientos" is displayed
And I select one or more elements in "Sistema" filter
And validate movements that matches selected filter are displayed
And I select one or more elements in "Version" filter
And validate movements that matches selected filter are displayed
And I select one or more elements in "Nodo origen" filter
And validate movements that matches selected filter are displayed
And I select one or more elements in "Nodo destino" filter
And validate movements that matches selected filter are displayed
And I select one or more elements in "Movimiento" filter
And validate movements that matches selected filter are displayed
And I select one or more elements in "Tipo de Movimiento" filter
And validate movements that matches selected filter are displayed
And I select one or more elements in "Propiedad" filter
And validate movements that matches selected filter are displayed
And I select one or more elements in "Origen" filter
And validate movements that matches selected filter are displayed
And I select one or more elements in "Unidad" filter
And validate movements that matches selected filter are displayed

@testcase=66943
Scenario: Validate inventory detail sheet
Given I am logged in as "admin"
When I navigate to "Initial official balance loaded" page
And I have official movements and inventories in the system
And I enter the required fields for Initial official balance report
And I click on "ReportFilter" "ViewReport" "button"
Then validate the report is loaded with default page "Initial official balance loaded"
And Validate the title "Balance Oficial Inicial Cargado" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Detalle de inventarios" tab
And validate the report page "Detalle de inventarios" is displayed
And validate the columns in "Detalle de inventarios"
| columns              |
| Sistema              |
| Versión              |
| Id inventario        |
| Nodo                 |
| Producto             |
| Cantidad neta        |
| Cantidad bruta       |
| Unidad               |
| Propietario          |
| Cantidad propiedad   |
| Porcentaje propiedad |
| Origen               |
| Fecha de registro    |
And validate the filter displayed in "Detalle de inventarios"
| filters   |
| SISTEMA   |
| VERSIÓN   |
| PRODUCTO  |
| PROPIEDAD |
| NODO      |
And validate the filters "Sistema", "Version", "Propiedad", "Producto" are selected as per selected in "Initial Official Balance Loaded"
And validate the filters "Nodo" is selected as "All" by default
And validate the inventories are displayed as per the filter applied
And validate inventories must be sorted by "Producto" and "Propietario"
And validate values in "Cantidad neta", "Cantidad bruta", "Cantidad propiedad" are shown with a thousand separator
And validate values in "Cantidad neta", "Cantidad bruta", "Cantidad propiedad", "Porcentaje propiedad" are shown with two decimal places
And validate value in "Fecha de registr" is in format "dd-MMM-yy"
And validate column is empty if the value is Null

@testcase=66944
Scenario: Validate filters in inventory detail sheet
Given I am logged in as "admin"
When I navigate to "Initial official balance loaded" page
And I have official movements and inventories in the system
And I enter the required fields for Initial official balance report
And I click on "ReportFilter" "ViewReport" "button"
Then validate the report is loaded with default page "Initial official balance loaded"
And Validate the title "Balance Oficial Inicial Cargado" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Detalle de inventarios" tab
And validate the report page "Detalle de inventarios" is displayed
And I select one or more elements in "Sistema" filter
And validate inventories that matches selected filter are displayed
And I select one or more elements in "Version" filter
And validate inventories that matches selected filter are displayed
And I select one or more elements in "Producto" filter
And validate inventories that matches selected filter are displayed
And I select one or more elements in "Propiedad" filter
And validate inventories that matches selected filter are displayed
And I select one or more elements in "Nodo" filter
And validate inventories that matches selected filter are displayed

@testcase=66945
Scenario: Validate movement attributes sheet
Given I am logged in as "admin"
When I navigate to "Initial official balance loaded" page
And I have official movements and inventories in the system
And I enter the required fields for Initial official balance report
And I click on "ReportFilter" "ViewReport" "button"
Then validate the report is loaded with default page "Initial official balance loaded"
And Validate the title "Balance Oficial Inicial Cargado" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Atributos movimientos" tab
And validate the report page "Atributos movimientos" is displayed
And validate the columns in "Atributos movimientos"
| columns              |
| Sistema              |
| Versión              |
| Tipo de movimiento   |
| Nodo origen          |
| Nodo destino         |
| Producto origen      |
| Producto destino     |
| Cantidad neta        |
| Cantidad bruta       |
| Unidad               |
| Propietario          |
| Cantidad propiedad   |
| Porcentaje propiedad |
| Origen               |
| Fecha de registro    |
| Atributo             |
| Valor atributo       |
| Unidad atributo      |
| Descripción atributo |
And validate the filter displayed in "Atributos movimientos"
| filters      |
| SISTEMA      |
| VERSIÓN      |
| PROPIEDAD    |
| ORIGEN       |
| NODO ORIGEN  |
| NODO DESTINO |
And validate the filters "Sistema", "Version", "Propiedad" are selected as per selected in "Initial Official Balance Loaded"
And validate the filters "Origen", "Nodo origen", "Nodo destino" are selected as "All" by default
And validate the movement attributes are displayed as per the filter applied
And validate movement attributes must be sorted by "Nodo destino", "Nodo origen" and "Tipo de Movimiento"
And validate values in "Cantidad neta", "Cantidad bruta", "Cantidad propiedad" are shown with a thousand separator
And validate values in "Cantidad neta", "Cantidad bruta", "Cantidad propiedad", "Porcentaje propiedad" are shown with two decimal places
And validate value in "Fecha de registr" is in format "dd-MMM-yy"
And validate column is empty if the value is Null

@testcase=66946
Scenario: Validate filters in movement attributes sheet
Given I am logged in as "admin"
When I navigate to "Initial official balance loaded" page
And I have official movements and inventories in the system
And I enter the required fields for Initial official balance report
And I click on "ReportFilter" "ViewReport" "button"
Then validate the report is loaded with default page "Initial official balance loaded"
And Validate the title "Balance Oficial Inicial Cargado" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Atributos movimientos" tab
And validate the report page "Atributos movimientos" is displayed
And I select one or more elements in "Sistema" filter
And validate movement attributes that matches selected filter are displayed
And I select one or more elements in "Version" filter
And validate movement attributes that matches selected filter are displayed
And I select one or more elements in "Propiedad" filter
And validate movement attributes that matches selected filter are displayed
And I select one or more elements in "Origen" filter
And validate movement attributes that matches selected filter are displayed
And I select one or more elements in "Nodo origen" filter
And validate movement attributes that matches selected filter are displayed
And I select one or more elements in "Nodo destino" filter
And validate movement attributes that matches selected filter are displayed

@testcase=66947
Scenario: Validate inventory attributes sheet
Given I am logged in as "admin"
When I navigate to "Initial official balance loaded" page
And I have official movements and inventories in the system
And I enter the required fields for Initial official balance report
And I click on "ReportFilter" "ViewReport" "button"
Then validate the report is loaded with default page "Initial official balance loaded"
And Validate the title "Balance Oficial Inicial Cargado" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Atributos inventarios" tab
And validate the report page "Atributos inventarios" is displayed
And validate the columns in "Atributos inventarios"
| columns              |
| Sistema              |
| Versión              |
| Id inventario        |
| Nodo                 |
| Producto             |
| Cantidad neta        |
| Cantidad bruta       |
| Unidad               |
| Propietario          |
| Cantidad propiedad   |
| Porcentaje propiedad |
| Origen               |
| Fecha de registro    |
| Atributo             |
| Valor atributo       |
| Unidad atributo      |
| Descripción atributo |
And validate the filter displayed in "Atributos inventarios"
| filters   |
| SISTEMA   |
| VERSIÓN   |
| PROPIEDAD |
| ORIGEN    |
| NODO      |
And validate the filters "Sistema", "Version", "Propiedad" are selected as per selected in "Initial Official Balance Loaded"
And validate the filters "Origen", "Nodo" is selected as "All" by default
And validate the inventory attributes are displayed as per the filter applied
And validate inventory attributes must be sorted by "Producto" and "Propietario"
And validate values in "Cantidad neta", "Cantidad bruta", "Cantidad propiedad" are shown with a thousand separator
And validate values in "Cantidad neta", "Cantidad bruta", "Cantidad propiedad", "Porcentaje propiedad" are shown with two decimal places
And validate value in "Fecha de registr" is in format "dd-MMM-yy"
And validate column is empty if the value is Null

@testcase=66948
Scenario: Validate filters in inventory attributes sheet
Given I am logged in as "admin"
When I navigate to "Initial official balance loaded" page
And I have official movements and inventories in the system
And I enter the required fields for Initial official balance report
And I click on "ReportFilter" "ViewReport" "button"
Then validate the report is loaded with default page "Initial official balance loaded"
And Validate the title "Balance Oficial Inicial Cargado" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Atributos inventarios" tab
And validate the report page "Atributos inventarios" is displayed
And I select one or more elements in "Sistema" filter
And validate inventory attributes that matches selected filter are displayed
And I select one or more elements in "Version" filter
And validate inventory attributes that matches selected filter are displayed
And I select one or more elements in "Propiedad" filter
And validate inventory attributes that matches selected filter are displayed
And I select one or more elements in "Origen" filter
And validate inventory attributes that matches selected filter are displayed
And I select one or more elements in "Nodo" filter
And validate inventory attributes that matches selected filter are displayed
@testcase=68032 
Scenario: Validate the hidden pages of initial official balance report
Given I am logged in as "admin"
When I navigate to "Initial official balance loaded"" page
And I have official movements and inventories in the system
And I enter the required fields for Initial official balance report
And I click on "ReportFilter" "ViewReport" "button"
And I open the report file in powerbi
Then validate "Cover" page hidden in UI
And validate "Confidentiality agreement" page hidden in UI
And validate "Log" page hidden in UI
And validate the report is loaded with default page "Initial official balance loaded"
And Validate the title "Balance Oficial Inicial Cargado" is displayed
And validate the title "Segmento <Nombre del Segmento> - <Nombre nodo>" is displayed
And validate the title "Período del dd-MMM-yy al dd-MMM-yy" is displayed
And I click on "Detalle de movimientos" tab
And validate the report page "Detalle de movimientos" is displayed
And I click on "Atributos movimientos" tab
And validate the report page "Atributos movimientos" is displayed
And I click on "Detalle de inventarios" tab
And validate the report page "Detalle de inventarios" is displayed
And I click on "Atributos inventarios" tab
And validate the report page "Atributos inventarioss" is displayed
