@sharedsteps=41450 @ui @owner=jagudelos @testsuite=39233 @testplan=39221
Feature: ReportToNodeConfigurationVisualizeOwnershipCalculation
As a query user,
I need a report with the node configuration to visualize
in a consolidated way the data required for the ownership calculation

Background: Login
Given I am logged in as "admin"

@testcase=41451 @bvt @manual
Scenario: Verify as a TRUE Query User, the initial loading of the filters page
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
Then I validate the Category list should show the items "Segment" and "System"
And I validate the The Element list should show the "Seleccionar" value by default

@testcase=41452 @bvt @manual
Scenario Outline: Verfiy as a TRUE Query User,required message must display for Mandatory fields in generation page of the node configuration report
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And  I click on "View" "Report" "button" without selecting a segment or an element
Then I verify system must display the "Requerido" message below each field

Examples:
| Selection       |
| Category        |
| CategoryElement |

@testcase=41453 @bvt @manual
Scenario Outline:Verify as a TRUE Query User,report contains two sheets with names as Configuraciones Nodos y Conexiones and Propiedad Conexiones
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And I select "<Selection>" from "Category"
And I select "<Element>" from "CategoryElement"
And I click on "View" "Report" "button"
Then I validate report as two sheets "Configuraciones Nodos y Conexiones" and "Propiedad Conexiones"

Examples:
| Selection | Element        |
| Sistema   | SystemElement  |
| Segmento  | SegmentElement |

@testcase=41454 @bvt @manual
Scenario Outline:Verify as a TRUE Query User, view report in detial
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And I select "<Selection>" from "Category"
And I select "<Element>" from "CategoryElement"
And I click on "View" "Report" "button"
Then I verify report display with the active nodes that belong to the selected segment or system on the current date​..
And I verify report title should display as Configuración Detallada por Nodo Segmento Name of selected segment or Sistema Name of selected system​
And I verify by default, the "Configuraciones Nodos y Conexiones" sheet should be displayed.
And I verify the "NODO" list, the first node in the list should be shown by default.
And I verify the "PRODUCTO" list, the option all should be shown by default.
And I verify the "TIPO DE CONEXIÓN" list, the "Entrada" option should be displayed by default
And I verify the "CONFIGURACIÓN GLOBAL DEL NODO" section, the general information of the node should be displayed and If there are fields without values, they should appear blank
And I verify the "PRODUCTOS VINCULADOS AL NODO" all the products of the node should be display  alphabetical order
And I verify the "PRODUCTOS VINCULADOS AL NODO" If there are fields without values ​​for the products, they should appear blank.​
And I verify the section "CONEXIONES VINCULADAS AL NODO" the configurations of all the products that belong to the input connections of the selected node should be shown
And I verify the  information in the table must be displayed alphabetically by node and product and If there are fields without values, they should appear blank
Examples:
| Selection | Element        |
| Sistema   | SystemElement  |
| Segmento  | SegmentElement |

@testcase=41455 @bvt @manual
Scenario Outline:Verify as a TRUE Query User, node configuration report with system current date for active nodes
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And I select "<Selection>" from "Category"
And I select "<Element>" from "CategoryElement"
And I click on "View" "Report" "button"
Then I validate the report with the active nodes that belong to the selected segment or system on the current date​.

Examples:
| Selection | Element        |
| Sistema   | SystemElement  |
| Segmento  | SegmentElement |

@testcase=41456 @bvt @manual
Scenario:Verify as a TRUE Query User,report on Configurations Nodes and Connections by select a node from list of nodes
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And  I click on "View" "Report" "button"
And  I able to view sheet "Configuraciones Nodos y Conexiones" in report
And I verify the nodes are displayed in alphabetical order and by default the first node is selected
And  I select new node from node list
And I verify node information in the "CONFIGURACIÓN GLOBAL DEL NODO" section, based on node selection
And I verify the information in the section "PRODUCTOS VINCULADOS AL NODO",based on node selection
Then I verify data of the products that belong to the selected node are displayed
When I verify the information in the section "CONEXIONES VINCULADAS AL NODO"
Then I verify data of the connections of the selected node are displayed

@testcase=41457 @bvt @manual
Scenario:Verify as a TRUE Query User,report on Configurations Nodes and Connections by select a product from product list
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And  I click on "View" "Report" "button"
And  I able to view sheet "Configuraciones Nodos y Conexiones" in report
And I verify the when the report is loaded the first time, the default value of this filter should be the option all
And  I select one or more products from product list
And  I verify the information in the section "PRODUCTOS VINCULADOS AL NODO"
Then I verify data of the selected products are displayed.
When I verify the information in the section "CONEXIONES VINCULADAS AL NODO"
Then I verify data of the selected products are displayed

@testcase=41458 @bvt @manual
Scenario:Verify as a TRUE Query User,the input connections on Configurations Nodes and Connections where the destination node is equal to the selected node
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And  I click on "View" "Report" "button"
And  I able to view sheet "Configuraciones Nodos y Conexiones" in report
And  I select the "Entrada" option from the TIPO DE CONEXIÓN list
And  I verify the information in the section "CONEXIONES VINCULADAS AL NODO"
Then I veriy the connections where the destination node is equal to the selected node are displayed

@testcase=41459 @bvt @manual
Scenario:Verify as a TRUE Query User,the output connections on Configurations Nodes and Connections where the source node is equal to the selected node
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And  I click on "View" "Report" "button"
And  I able to view sheet "Configuraciones Nodos y Conexiones" in report
And  I select the "Salida" option from the TIPO DE CONEXIÓN list
And  I verify the information in the section "CONEXIONES VINCULADAS AL NODO"
Then I verify the connections where the source node is equal to the selected node are displayed

@testcase=41460 @bvt @manual
Scenario:Verify as a TRUE Query User,the ownership of the connections products
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And  I click on "View" "Report" "button"
And  I able to view sheet "Configuraciones Nodos y Conexiones" in report
And  I clicks on the "Propiedad Conexiones" sheet or on the link (->) found in the upper right part of the "CONFIGURACIÓN GLOBAL DEL NODO" section
Then I verify the information in the "Propiedad Conexiones"
And  I verify The node list should display the same node selected in the sheet "Configuraciones Nodos y Conexiones"
And  I verify Connections in the TIPO DE CONEXIÓN list, the same option selected in the "Configuraciones Nodos y Conexiones"
And  I verify the PRODUCTO list, show the products that belong to the selected node, sorted alphabetically and the first product must be selected by default
And  I verify "CONFIGURACIÓN GLOBAL DEL NODO" section, the general information of the node should be displayed.
And  I verify "CONFIGURACIÓN GLOBAL DEL NODO" section If there are fields without values, they should appear blank
And I verify the "PORCENTAJE PROPIEDAD POR PRODUCTO" ,a stacked column chart should be displayed, with the ownership distribution of the selected product in the connections of the selected connection type ("Input / Output ").
And I verify the  "Data Table" section ,the connection information for the selected connection type and for the selected product should be displayed
And I verify the "Data Table", section If there are fields without values, they should be blank.

@testcase=41461 @bvt @manual
Scenario:Verify as a TRUE Query User,report from Property Connections sheet by select a node from list of nodes
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And  I click on "View" "Report" "button"
And  I able to view sheet "Propiedad Conexiones" in report
And I verify the nodes are displayed in alphabetical order and by default the first node is selected
And  I select new node from node list on Property Connections
And  I verify node information in the "CONFIGURACIÓN GLOBAL DEL NODO" section
And  I verify the information in the "PORCENTAJE DE PROPIEDAD POR PRODUCTO" section
Then I verify that the property of the selected product is shown, in the connections of the selected node
When I verify the information in the data table
Then I verify  the connection data of the selected node is displayed.

@testcase=41462 @bvt @manual
Scenario:Verify as TRUE Query User,report from Property Connections sheet by a product from product list
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And  I click on "View" "Report" "button"
And  I able to view sheet "Propiedad Conexiones" in report
And  I verify the when the report is loaded the first time, the default value of this filter should be the option all
And  I select product from product list on Property Connections
And  I verify the information in the section "PORCENTAJE DE PROPIEDAD POR PRODUCTO"
Then I verify that only the ownership of the selected product is displayed on connections.
When I verify the information in the data table
Then I verify that only connections that have the selected product are displayed

@testcase=41463 @bvt @manual
Scenario:Verify as a TRUE Query User, the input connections on Property Connections
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And  I click on "View" "Report" "button"
And  I able to view sheet "Propiedad Conexiones" in report
And  I select the  "Entrada" option from the TIPO DE CONEXIÓN list
And  I verify the information in the data table
Then I verify that only the connections where the destination node is equal to the selected node are displayed
When I verify the information in the section "PORCENTAJE DE PROPIEDAD POR PRODUCTO"
Then I verify that only the ownership of the selected product is displayed on the input connections

@testcase=41464 @bvt @manual
Scenario:Verify as a TRUE Query User,the output connections on Property Connections
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And  I click on "View" "Report" "button"
And  I able to view sheet "Propiedad Conexiones" in report
And  I select the "Salida" option from the TIPO DE CONEXIÓN list
And  I verify the information in the data table
Then I verify that only the connections where the source node is equal to the selected node are displayed
When I verify the information in the section "PORCENTAJE DE PROPIEDAD POR PRODUCTO"
Then I verify that only the ownership of the selected product is displayed on the output connections

@testcase=41465 @bvt @manual
Scenario:Verify as a TRUE Query User, nodes and connections information when return to the Configurations Nodes and Connections sheet
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And  I click on "View" "Report" "button"
And  I able to view sheet "Propiedad Conexiones" in report
And  I click on the "Configuraciones Nodos y Conexiones" sheet or on the link (<-) which is located at the top right of section "GLOBAL NODE CONFIGURATION
Then I verify the information in the "Configuraciones Nodos y Conexiones"
And  I verify The node list should display the same node selected in the sheet "Connections Property"
And  I verify TIPO DE CONEXIÓN list, the same option selected in the "Connections Property" sheet should appear
And  I verify the "CONFIGURACIÓN GLOBAL DEL NODO" section, the general information of the node should be displayed
And  I verify "CONFIGURACIÓN GLOBAL DEL NODO" section If there are fields without values, they should appear blank
And I verify the "PRODUCTOS VINCULADOS AL NODO" section ,all the products of the node should be shown, ordered alphabetically
And I verify the "PRODUCTOS VINCULADOS AL NODO" section ,If there are fields without values ​​for the products, they should appear blank
And I verify the "CONEXIONES VINCULADAS AL NODO" section the configurations of all the products that belong to the connections of the selected type ("Input" / "Output")
And I verify the "CONEXIONES VINCULADAS AL NODO" section The information in the table must be displayed alphabetically by node and product
And I verify the "CONEXIONES VINCULADAS AL NODO" section  If there are fields without values, they should appear blank.

@testcase=41466 @bvt @manual
Scenario:Verify as a TRUE Query User,search with nodes in sheet Configurations Nodes and Connections will show matched nodes
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And  I click on "View" "Report" "button"
And  I able to view sheet  "Configuraciones Nodos y Conexiones" in report
And  I types part of a node name into the CONEXIÓN control and press the Enter key or clicks on the search icon
Then I verify result as "CONEXIONES VINCULADAS AL NODO" only the nodes that match the entered data

@testcase=41467 @bvt @manual
Scenario:Verify as a TRUE Query User,search with node in Property Connections sheet will display matched values
Given I have "ownershipnodes" created
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And  I click on "View" "Report" "button"
And  I able to view sheet "Propiedad Conexiones" in report
And  I types part of a node name into the CONEXIÓN control and press the Enter key or clicks on the search icon
Then I verify the "PORCENTAJE PROPIEDAD POR PRODUCTO" section, the ownership distribution of the selected product must be shown, in the connections that match the value entered.
Then I verify the information of the connections match the value entered

@testcase=41468 @bvt @manual
Scenario:Verify as a TRUE Query User,only titles for shown when no data is present in the report
Given I have generated report without template data configured
When I navigate to "Configuración detallada por nodo", from "Reportes de ductos y estaciones" menu
And  I click on "View" "Report" "button"
And  I able to view  node configuration report
Then I verify blank report cells are dispalyed.
