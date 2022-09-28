@owner=jagudelos @ui @testsuite=70806 @testplan=70526 @S16
Feature: ValidateFunctionalitiesOfOfficialSivTable
In the TRUE stsytem
As a Chain user
I should be able to start process of official SIV generation process and validate the the related table
@testcase=71772
Scenario Outline: Verify the presence of Official logistics movements and inventories page
Given I am logged in as "<user>"
When I navigate to "Gestión cadena de suministro" menu item
Then I should see a submenu with title "Movimientos e inventarios logísticos oficiales"
Examples:
| user        |
| admin       |
| profesional |
| chain       |
@testcase=71773
Scenario Outline: Verify that the Official logistics movements and inventories page is not accessible to any user other than admin, profesional and chain
Given I am logged in as "<user>"
When I navigate to "Gestión cadena de suministro" menu item
Then I should not see a submenu with title "Movimientos e inventarios logísticos oficiales"
Examples:
| user        |
| aprobador   |
| audit       |
| programador |
| consulta    |
@testcase=71774
Scenario Outline: Verify breadcrumb for Official logistics movements and inventories page
Given I am logged in as "<user>"
When I navigate to "Official logistics movements and inventories" page
Then I should see breadcrumb "Movimientos e inventarios logísticos oficiales"
Examples:
| user        |
| admin       |
| profesional |
| chain       |
@testcase=71775
Scenario Outline: Validate the column names of official SIV table on Official logistics movements and inventories page
Given I am logged in as "<user>"
When I navigate to "Official logistics movements and inventories" page
Then I validate that the table has below mentioned columns in it
| ColumnNames     |
| Segmento        |
| Nodo            |
| Propietario     |
| Fecha inicial   |
| Fecha final     |
| Fecha ejecución |
| Usuario         |
| Estado          |
Examples:
| user        |
| admin       |
| profesional |
| chain       |
@testcase=71776
Scenario Outline: Validate the behaviour of official SIV table on Official logistics movements and inventories page
Given I am logged in as "<user>"
When I navigate to "Official logistics movements and inventories" page
Then I validate that the official SIV table is visible on the page
And I validate that the results in the table are sorted by execution date in descending order by default
And I validate that the table shows the results from last 365 days by default
And I validate that the table shows default page size as 50
When I select the value '10' from the pagination dropdown
Then I validate that the records shown in the grid are no more than '10'
When I filter the records with value 'Transporte' by typing the text inside the Segmento column searchbox
Then I validate that only records with Segmento value 'Transporte' are displayed in the grid
When the user clicks on the 'Fecha ejecución' column label
Then I validate that the results in the table are sorted by execution date in descending order
When the user clicks on the 'Fecha ejecución' column label
Then I validate that the results in the table are sorted by execution date in ascending order
Examples:
| user        |
| admin       |
| profesional |
| chain       |
@testcase=71777
Scenario Outline: Validate the possible statuses of the records inside the official SIV grid
Given I am logged in as "<user>"
When I navigate to "Official logistics movements and inventories" page
Then I validate that the statuses in the table grid are only from the following list
| possible statuses |
| Procesando        |
| Finalizado        |
| Fallido           |
Examples:
| user        |
| admin       |
| profesional |
| chain       |
@testcase=71778
Scenario Outline: Validate different states of action icons on the table grid on Official logistics movements and inventories page
Given I am logged in as "<user>"
When I navigate to "Official logistics movements and inventories" page
Then I validate that neither of the action icons are enabled when the status is 'Procesando'
And I validate that the View error action icon is 'enabled' for records with status 'Fallido'
And I validate that the View error action icon is 'disabled' for records with status 'Finalizado'
And I validate that the Download action icon is 'enabled' for records with status 'Finalizado'
And I validate that the Download action icon is 'disabled' for records with status 'Fallido'
Examples:
| user        |
| admin       |
| profesional |
| chain       |
@testcase=71779
Scenario Outline: Validate the error message when no records are available on the official SIV grid
Given I am logged in as "<user>"
When I navigate to "Official logistics movements and inventories" page
Given there are no records on the official table gird
Then I validate that the 'Sin registros.' error is displayed inside the table
Examples:
| user        |
| admin       |
| profesional |
| chain       |
@testcase=71780
Scenario Outline: Validate the functionality of Create file button on Official logistics movements and inventories page
Given I am logged in as "<user>"
When I navigate to "Official logistics movements and inventories" page
Then I vaidate that a button with name 'Crear archivo' is displayed
When the user clicks on the Crear archivo button
Then I validate that a popup is displayed containing the SIV file generation wizard
Examples:
| user        |
| admin       |
| profesional |
| chain       |
@testcase=71781
Scenario Outline: Validate that the Error dialog popup is displayed when user clicks on the View error action icon
Given I am logged in as "<user>"
When I navigate to "Official logistics movements and inventories" page
And I click on the error icon on any of the available records in Fallido state
Then I validate that the Error dialog is displayed as a popup
Examples:
| user        |
| admin       |
| profesional |
| chain       |
@testcase=71782
Scenario Outline: Validate the contents of Error dialog popup for a technical error
Given I am logged in as "<user>"
When I navigate to "Official logistics movements and inventories" page
And I click on the error icon on any of the available technical error records in Fallido state
Then I validate the popup is displayed with title 'Error al generar el archivo de reporte logístico'
And I validate the error tag as 'Error en generación de reporte logístico:'
And I validate the value of Segment on the popup
And I validate the value of Node on the popup
And I validate the value of Owner on the popup
And I validate the value of Period on the popup
And I validate that the format of the Period value is '[StartTime] al [EndTime]'
And I validate the error message as configured in the database
Examples:
| user        |
| admin       |
| profesional |
| chain       |
@testcase=71783 
Scenario Outline: Validate the contents of Error dialog popup for a business error
Given I am logged in as "<user>"
When I navigate to "Official logistics movements and inventories" page
And I click on the error icon on any of the available business error records in Fallido state
Then I validate the popup is displayed with title 'Error al generar el archivo de reporte logístico'
And I validate the error tag as 'Error en generación de reporte logístico:'
And I validate the value of Segment on the popup
And I validate the value of Node on the popup
And I validate the value of Owner on the popup
And I validate the value of Period on the popup
And I validate that the format of the Period value is '[StartTime] al [EndTime]'
And I validate the error message as configured in the database
Examples:
| user        |
| admin       |
| profesional |
| chain       |
