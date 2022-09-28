@sharedsteps=4013 @owner=jagudelos @ui @testsuite=55106 @testplan=55104 @MVP2and3 @S14
Feature: ProcessDeltaCalculation
As a Balance Segment Professional User,
I need an UI to start the delta calculation process
for a specified period

@testcase=56885 @BVT2
Scenario Outline: Verify that Delta Calculation page should present for administrator and profesional role under Balance de ductos y estaciones menu
Given I am logged in as "<User>"
Then I should see "Calculation of deltas by operational adjust" page based on user "<User>"

Examples:
| User        |
| admin       |
| profesional |

@testcase=56886
Scenario Outline: Verify Delta Calculation page should not be present for user other than administrator and profesional role under Balance de ductos y estaciones menu
Given I am logged in as "<User>"
Then I should not see "Calculation of deltas by operational adjust" page based on user "<User>"

Examples:
| User        |
| aprobador   |
| audit       |
| programador |
| consulta    |

@testcase=56887 @version=2
Scenario: Verify Calculation of deltas by operational adjust page should present under Balance de ductos y estaciones submenu
Given I am logged in as "admin"
When I click on "Menu" toggler
And I click on "Balance of pipelines and stations" link
Then I should see the "Calculation of deltas by operational adjust" option

@testcase=56888
Scenario: Verify breadcrumb for Cálculo de deltas por ajuste operativo page
Given I am logged in as "admin"
When I navigate to "Calculation of deltas by operational adjust" page
Then I should see breadcrumb "Cálculo de deltas por ajuste operativo"

@testcase=56889
Scenario Outline: Verify the filtering functionality on Calculation of Delta page
Given I am logged in as "admin"
When I navigate to "Calculation of deltas by operational adjust" page
And I provided value for "<FieldName>" in the grid
Then I should see the records filtered as per the searched criteria

Examples:
| FieldName     |
| Segment       |
| StartDate     |
| EndDate       |
| ExecutionDate |
| Status        |
| Ticket        |
| User          |

@testcase=56890 @version=2
Scenario: Verify that results should be sorted based on the execution date descendant by default in Calculation of Delta Page
Given I am logged in as "admin"
When I navigate to "Calculation of deltas by operational adjust" page
Then the results should be sorted based on ExecutionDate be descending in the Grid

@testcase=56891 @version=2
Scenario: Verify that GRID_NORECORDS message should be displayed when there is no records in Calculation of Delta Page
Given I am logged in as "admin"
When I navigate to "Calculation of deltas by operational adjust" page
And I don't have any Delta Calculated Tickets in the system
Then I should see message "Sin registros"

@testcase=56892 @version=2
Scenario: Verify Pagination functionality for Calculation of Delta page
Given I am logged in as "admin"
And I have Segments information for Delta Calculation from the last Fourty days
When I navigate to "Calculation of deltas by operational adjust" page
And I navigate to second page in "Calculation of deltas by operational adjust" Grid
And I change the elements count per page to 50
Then the records count in "Calculation of deltas by operational adjust" Grid shown per page should also be 50

@testcase=56893 @version=2
Scenario: Verify that records from deltas executed in the last N days should be displayed in the Delta Calculation Grid
Given I am logged in as "admin"
And I configure the N days from the Configuration in storage explorer
When I navigate to "Calculation of deltas by operational adjust" page
And I change the elements count per page to 100
Then the records from deltas executed in the last N days should be displayed in the Grid

@testcase=56894 @version=2 @BVT2
Scenario: Verify that wizard interface when clicked on new delta calculation button
Given I am logged in as "admin"
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
Then I should see "Seleccione la información para el procesamiento" subtitle
And I should see "initDeltaTicket" "segment" "dropdown"
And I should see "Período del procesamiento" on initial wizard
And I should see "initDeltaTicket" "statDate" "Date"
And I should see "initDeltaTicket" "endDate" "Date"
And I should see "initDeltaTicket" "Submit" "button"
And validate that "initDeltaTicket" "Submit" "button" is "disabled"

@testcase=56895 @version=2
Scenario: Verify that active segments configured as SON should be displayed in the segment field when clicked on new delta calculation button
Given I am logged in as "admin"
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I click on "initDeltaTicket" "segment" "dropdown"
Then Active Segments "configured" as SON should be displayed in the dropdown

@testcase=56896 @version=2
Scenario: Verify that active segments which is not configured as SON should not displayed in the segment field when clicked on new delta calculation button
Given I am logged in as "admin"
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I click on "initDeltaTicket" "segment" "dropdown"
Then Active Segments which is "not configured" configured as SON shouldn't be displayed in the dropdown

@testcase=56897 @version=2
Scenario: Verify the value in start date should be calculated based on value of parameter when current day is less than or equal to the value set in the parameter
Given I am logged in as "admin"
And If the current day is less than or equal to the value set in the parameter (ValidDaysCurrentMonth) in Configuration setting
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
Then Verify that value from "initDeltaTicket" "statDate" "date" should be the first day of the previous month

@testcase=56898 @version=2
Scenario: Verify the value in start date should be calculated based on value of parameter when current day is greater than the value set in the parameter
Given I am logged in as "admin"
And If the current day is greater than the value set in the parameter (ValidDaysCurrentMonth) in Configuration setting
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
Then Verify that value from "initDeltaTicket" "statDate" "date" the start date must be the first day of the current month

@testcase=56899 @version=2
Scenario: Verify the value in end date field should be the last date of ownership calculation of the chosen segment.
Given I am logged in as "admin"
And I have valid segment for Delta Calculation Process
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select delta segment from "initDeltaTicket" "segment" "dropdown"
Then Verify that value from "initDeltaTicket" "endDate" "date" should be the last date of ownership calculation of the chosen segment

@testcase=56900 @version=2 @BVT2
Scenario: Verify that both the initial and final date fields should be disabled during start in the delta calculation process.
Given I am logged in as "admin"
And I have valid segment for Delta Calculation Process
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select delta segment from "initDeltaTicket" "segment" "dropdown"
Then validate that "initDeltaTicket" "statDate" "date" as disabled
And validate that "initDeltaTicket" "endDate" "date" as disabled

@testcase=56901 @version=2
Scenario: Verify the error message should be displayed at the first step when segment without ownership is chosen during delta calculation.
Given I am logged in as "admin"
And I have Segment for which ownership is not generated
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select delta segment from "initDeltaTicket" "segment" "dropdown"
Then Verify that "No se encontró cálculo de propiedad para el segmento. Por favor realice el cálculo de propiedad primero." error message should be displayed.

@testcase=56902 @version=2
Scenario: Verify the error message should be displayed when cut-off is running for segment during the initiation of delta calculation.
Given I am logged in as "admin"
And I have Segment for which Cutoff Process is running in system
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select delta segment from "initDeltaTicket" "segment" "dropdown"
And I click on "initDeltaTicket" "Submit" "button"
Then Verify that "Está ejecutándose un corte operativo para el segmento, por favor espere a que el proceso termine e intente nuevamente." error message should be displayed.
And validate that "initDeltaTicket" "process" "date" as disabled

@testcase=56903 @version=2
Scenario: Verify the error message should be displayed when deltas calculation is running for segment and started the delta calculation process again.
Given I am logged in as "admin"
And I have delta calculation process running for the segment
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select delta segment from "initDeltaTicket" "segment" "dropdown"
And I click on "initDeltaTicket" "Submit" "button"
Then Verify that "Se encuentra en procesamiento un cálculo de deltas operativos para ese mismo segmento." error message should be displayed.
And validate that "initDeltaTicket" "Submit" "button" as disabled

@testcase=56904
Scenario: Verify that system should display pending inventory records in wizard during delta calculation process.
Given I am logged in as "admin"
And I uploaded the movements and inventories for the same segment in system
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select delta segment from "initDeltaTicket" "segment" "dropdown"
And I click on "initDeltaTicket" "Submit" "button"
Then I should see the "Columns" on pending inventory wizard
| Columns          |
| Id. inventario   |
| Nodo             |
| Producto         |
| cantidad neta    |
| Unidad           |
| Fecha inventario |
| acción           |
And All the records should not have an ownership calculation ticket
And the nodes should belongs to the selected segment
And the operating date is between the selected period including the start date or the end date
And the last record of each inventory should be displayed
And I verify that list must be sorted by Inventory date, Inventory Id for the oldest to the recent

@testcase=56905
Scenario: Verify the filtering functionality for pending inventory screen on Calculation of Delta page
Given I am logged in as "admin"
And I uploaded the movements and inventories for the same segment in system
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select delta segment from "initDeltaTicket" "segment" "dropdown"
And I click on "initDeltaTicket" "Submit" "button"
Then I verify filter the content of the list based on "Field Name"
| Field Name     |
| Inventory Id   |
| Node           |
| Product        |
| Amount         |
| Unit           |
| Inventory Date |
| Action         |
And I click on "pendingInventoriesGrid" "cancel" "button"
And verify that user must return to the Calculation of deltas by operational adjust page without performing any action

@testcase=56906 @version=2
Scenario: Verify that GRID_NORECORDS message should be displayed when there is no pending inventories present in process of Calculation of Delta
Given I am logged in as "admin"
And I have Segment for which there is no pending inventories present in process of Calculation of Delta
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select delta segment from "initDeltaTicket" "segment" "dropdown"
And I click on "initDeltaTicket" "Submit" "button"
Then I should see message "Sin registros"
And validate that "pendingInventoriesGrid" "Submit" "button" as enabled
@testcase=56907 
Scenario: Verify that system should display pending movement records in wizard during delta calculation process.
Given I am logged in as "admin"
And I uploaded the movements and inventories for the same segment in system
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select delta segment from "initDeltaTicket" "segment" "dropdown"
And I click on "initDeltaTicket" "Submit" "button"
And I click on "pendingInventoriesGrid" "Submit" "button"
Then I should see the "Columns" on pending movement wizard
| Columns         |
| Id. movimiento  |
| Tipo movimiento |
| Nodo origen     |
| Nodo destino    |
| Prod. origen    |
| Prod. destino   |
| Cantidad neta   |
| Unidad          |
| Fecha operativa |
| acción          |
And All the movements records should not have an ownership calculation ticket
And the nodes should belongs to the selected segment
And All the records should not have an globalID
And the Movement operating date is between the selected period including the start date or the end date
And the last record of each movement should be displayed
And I verify that list must be sorted by Operational date, Movement Id for the oldest to the recent

@testcase=56908
Scenario: Verify the filtering functionality for pending movements screen on Calculation of Delta page
Given I am logged in as "admin"
And I uploaded the movements and inventories for the same segment in system
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select delta segment from "initDeltaTicket" "segment" "dropdown"
And I click on "initDeltaTicket" "Submit" "button"
And I click on "pendingInventoriesGrid" "Submit" "button"
Then I verify filter the content of the Pending Movements list based on "Field Name"
| Field Name          |
| Movement Id         |
| Movement type       |
| Source Node         |
| Destination Node    |
| Source Product      |
| Destination Product |
| Amount              |
| Unit                |
| Operational Date    |
| Action              |
And I click on "pendingMovementsGrid" "cancel" "button"
And verify that user must return to the Calculation of deltas by operational adjust page without performing any action

@testcase=56909  @version=2
Scenario: Verify that GRID_NORECORDS message should be displayed when there is no pending movements present in process of Calculation of Delta
Given I am logged in as "admin"
And I have Segment for which there is no pending movements present in process of Calculation of Delta
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select delta segment from "initDeltaTicket" "segment" "dropdown"
And I click on "initDeltaTicket" "Submit" "button"
And I click on "pendingInventoriesGrid" "Submit" "button"
Then I should see message "Sin registros"
And validate that "pendingMovementsGrid" "Submit" "button" as disabled

@testcase=56910 @version=2
Scenario: Verify that error message should be displayed when there is neither pending movements or pending inventories present in process of Calculation of Delta
Given I am logged in as "admin"
And I have Segment for which there is neither pending movements or pending inventories present in process of Calculation of Delta
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select delta segment from "initDeltaTicket" "segment" "dropdown"
And I click on "initDeltaTicket" "Submit" "button"
And I click on "pendingInventoriesGrid" "Submit" "button"
Then Verify that "No existen registros para procesar el cálculo de deltas operativos" error message should be displayed.

@testcase=56911 @version=3 @BVT2 @parallel=false
Scenario: Verify that Delta Calculation process should be successful for a specified period
Given I am logged in as "admin"
And I have ownership calculation data generated in the system
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Update" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestDataCutOff_Daywise" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
And I create the Annulation Movement for these updated movements
And I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select segment from "initDeltaTicket" "segment" "dropdown"
And I click on "initDeltaTicket" "Submit" "button"
And I click on "pendingInventoriesGrid" "Submit" "button"
And I click on "pendingMovementsGrid" "Submit" "button"
Then I should see "Modal" "confirmDeltaCalculation" "container"
And I click on "confirmDeltaCalculation" "submit" "Button"
And Verify that Delta Calculation should be successful

@testcase=56912
Scenario: Verify the functionality of cancel button in the Confirmation Interface during the Delta Calculation process
Given I am logged in as "admin"
And I uploaded the movements and inventories for the same segment in system
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select delta segment from "initDeltaTicket" "segment" "dropdown"
And I click on "initDeltaTicket" "Submit" "button"
And I click on "pendingInventoriesGrid" "Submit" "button"
And I click on "pendingMovementsGrid" "Submit" "button"
Then I should see "Modal" "confirmDeltaCalculation" "container"
And I click on "confirmDeltaCalculation" "cancel" "button"
And verify that user must return to the calculation of deltas by operational adjust page without performing any action

@testcase=56913 @manual
Scenario: Verify that system should not display records in pending movement for which movement  already has a reconciliation identifier (GlobalId) previously assigned
Given I am logged in as "admin"
And I have generated ownership for the segment
And I uploaded the movements and inventories for the same segment in system
And I set the movement for the chosen segment for which there is already has a reconciliation identifier (GlobalId) previously assigned
When I navigate to "Calculation of deltas by operational adjust" page
And I click on "newDeltasCalculation" "button"
And I select value from "Segment" "dropdown"
And I click on "Procesar deltas" "button"
And I click on "pendinginventories" "submit" "button"
Then I should not see the record in pending movements for which there is already has a reconciliation identifier (GlobalId) previously assigned
