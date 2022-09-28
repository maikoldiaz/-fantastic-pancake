@sharedsteps=4013 @ui @owner=jagudelos @testplan=61542 @testsuite=61550
Feature: ChainUserSegmentUse
As a TRUE user I need several adjustments to improve the use of the system by all segments
@testcase=66812
Scenario: Verify a list of menus​ is loaded and the user is assigned the role of “USUARIO DE CADENA”
Given I need several adjusts to improve system usage across all segments.
When the list of menus​ is loaded and the user is assigned the role of “USUARIO DE CADENA”
When I am logged in with "USUARIO DE CADENA" role
Then login should be successful
And display the options available as below to the userrole "USUARIO DE CADENA"
| examples                                    |
| Cálculo de deltas por ajuste oficial        |
| Deltas oficiales por nodo                   |
| Reporte balance operativo                   |
| Balance oficial por nodo                    |
| Balance oficial inicial cargado             |
| Pendientes oficiales de períodos anteriores |
@testcase=66813
Scenario: Verify a list of menus​ is not shown to the user is assigned the role of “CONSULTA”
Given I need several adjusts to improve system usage across all segments.
When the list of menus​ is loaded and the user is assigned the role of “USUARIO DE CADENA”
When I am logged in as "CONSULTA" role
Then login should be successful
And verify the options as below should not displayed to the userrole "CONSULTA"
| examples                                    |
| Cálculo de deltas por ajuste oficial        |
| Deltas oficiales por nodo                   |
| Reporte balance operativo                   |
| Balance oficial por nodo                    |
| Balance oficial inicial cargado             |
| Pendientes oficiales de períodos anteriores |
@testcase=66814
Scenario: Verify a Copy of menu “Balance operativo con o sin propiedad” is loaded and the user is assigned the role of “USUARIO DE CADENA”
Given I need several adjusts to improve system usage across all segments.
When the list of menus​ is loaded and the user is assigned the role of “USUARIO DE CADENA”
Then I am logged in as "USUARIO DE CADENA"
Then login should be successful
And Show a menu in “Gestión cadena de suministro” to see the operational balance
Then verify a new option "Reporte balance operativo" page must be a copy of the existing one for "balance operativo con propiedad"
Then The "Balance ductos y estaciones” menu page should show active segments configured as SON.
And The "Gestión cadena de suministro" menu page should show all active segments.
@testcase=66815
Scenario: Verify a Improved search for target data in homologations
Given I need several adjusts to improve system usage across all segments.
When the source or destination data finder ​is used in the visual to editing homologations
When I am logged in as "admin"
And login should be successful
Then I navigate to "Homologation page"
And Search for the condition with a "Contains" instead of a "Start with".
Then Show maximum 20 records
Then Display scroll bar when the list contains more than 8 items
And Sort the list alphabetically ascending
@testcase=66816
Scenario: Verify the category field is disabled when editing category items
Given I need several adjusts to improve system usage across all segments.
When I am logged in as "admin"
And login should be successful
When I navigate to "CategoryElement" page
And  visual is used to "edit" a category element
Then Disable the field for "categoryselection"
@testcase=66817
Scenario: Verify inactive connections is shown in connection settings
Given I need several adjusts to improve system usage across all segments.
When I am logged in as "admin"
And login should be successful
When I navigate to "Configuración de atributos conexiones" page
And  visual is used to setting connections
Then Shows "active" and "inactive" connections in the list
@testcase=66818
Scenario: Verify the deleted connections in Ownership calculation validations should not be considered
Given I need several adjusts to improve system usage across all segments.
When I am logged in as "admin"
When shows the step of validations of the ownership calculation
Then Do not consider deleted connections for validations in ownership validations
@testcase=66819
Scenario: Verify the subtitles to owner editing listings for nodes and connections are added
Given I need several adjusts to improve system usage across all segments.
When shows UI to edit owners in nodes and connections configuration
Then Include subtitles in the listings
@testcase=66820
Scenario: Verify X button in the search control is added to edit owners listings for nodes and connections
Given I need several adjusts to improve system usage across all segments
When shows UI to edit owners in nodes and connections configuration
Then Include an X-shaped button at the top of the listing
And Displaying when type a text in the search control
And When clicking must clean the searcher text so that all the results are displayed in the list
@testcase=66821
Scenario: Verify any character in the name of a category element is allowed while edit or adding a new element
Given I need several adjusts to improve system usage across all segments
When visual is used to edit or add a category element
Then Allow input any character in the element name
@testcase=66822
Scenario: Verify the new name the "Enviado" status in the ownership calculation process
Given I need several adjusts to improve system usage across all segments
When shows the list of ownership calculation processing by segment or node
Then Verify the name “Enviado” / "Sent" status to “Procesando” / "Processing"
@testcase=66823
Scenario: Verify the Deletion of System name column
Given I need several adjusts to improve system usage across all segments
When upload an Excel file to register movements and inventories
Then Verify Store the value of the "SistemaOrigen" column in the canonical "SourceSystemId" property using homologations
@testcase=66824 
Scenario: Verify Deletion of System name column (Visualization)
Given I need several adjusts to improve system usage across all segments
When show exceptions on the exception screen or in the operational cut-off
Then Change the data binding on the exception management screen to use the category items instead of the old column
Then Change the step data binding in the Operational Break wizard to use the category items instead of the old column
