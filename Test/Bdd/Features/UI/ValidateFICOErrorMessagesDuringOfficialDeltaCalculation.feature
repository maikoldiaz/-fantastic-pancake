@sharedsteps=4013 @owner=jagudelos @ui @testsuite=61546 @testplan=61542 @S15
Feature: ValidateFICOErrorMessagesDuringOfficialDeltaCalculation
As a True user
I need to be able to see the error messages sent by FICO
When an exception/error occurs during the process of official delta calculation

Background: Login
Given I am logged in as "admin"
@testcase=66956
Scenario: Verify breadcrumb for Calculation of deltas by official adjustment page
When I navigate to "Calculation of deltas by official adjustment" page
Then I should see breadcrumb "Cálculo de deltas por ajuste oficial"
@testcase=66957
Scenario: Validate the technical error message when an exception arrives during the operational consolidation
Given I set the prerequisite to have a "consolidation" error generated
Given I navigate to "Calculation of deltas by official adjustment" page
When the user clicks on the error icon of the respective segment or node
Then a popup must appear with title "Error durante el cálculo de los deltas"
And I validate the error message on popup "Se presentó un error técnico inesperado en la consolidación del escenario operativo. Por favor ejecute nuevamente el proceso."
And I validate the value "period start date" on the popup
And I validate the value "period end date" on the popup
And I validate the value "segment" on the popup
@testcase=66958
Scenario: Validate the technical error message when an exception arrives during the delta calculation
Given I set the prerequisite to have a "delta" error generated
And I navigate to "Calculation of deltas by official adjustment" page
When the user clicks on the error icon of the respective segment or node
Then a popup must appear with title "Error durante el cálculo de los deltas"
And I validate the error message on popup "Se presentó un error técnico inesperado al enviar la información al motor de reglas para el cálculo de deltas oficiales. Por favor ejecute nuevamente el proceso."
And I validate the value "period start date" on the popup
And I validate the value "period end date" on the popup
And I validate the value "segment" on the popup
@testcase=66959
Scenario: Validate the error message due to failed backend validations in FICO
Given I set the prerequisite to have a "backend validations" error generated
And I navigate to "Calculation of deltas by official adjustment" page
When the user clicks on the error icon of the respective segment or node
Then a popup must appear with title "Error durante el cálculo de los deltas"
And I validate the error message on popup "El segmento no tiene información oficial pendiente en el período de fechas."
And I validate the value "period start date" on the popup
And I validate the value "period end date" on the popup
And I validate the value "segment" on the popup
#@testcase=66960
#Scenario: Validate that the page is redirected and table with node level business errors are displayed when user clicks on segment level business error
#Given The Delta calculation process is in progress
#And the delta calculation process ends with a #And I navigate to "Calculation of deltas by official adjustment" pag
#When the user clicks on the error icon of the respective segment
##Then validate that control is navigated to 'official deltas per node' page
#Then I should see breadcrumb "Deltas oficiales por nodo"
#And I validate that the failed nodes are displayed with error view icons in a tabular format
@testcase=66961
Scenario: Validate the list and order of business errors per node during the delta calculation process
Given I have segments nodes movements and inventories created
Given I set the prerequisite to have a "business" error generated
And I navigate to "Calculation of deltas by official adjustment" page
When the user clicks on the error icon of the business error related segment
Then I should see breadcrumb "Deltas oficiales por nodo"
When the user clicks on the error icon of the respective segment or node
Then a popup must appear with title "Detalle del error al procesar el cálculo de deltas oficiales"
And I validate the value "Node" on the popup
And I validate the value "Execution Date" on the popup
And I validate that the table on the popup has following columns displayed with respective error messages
| ColumnNames      |
| Tipo             |
| Nodo Origen      |
| Nodo Destino     |
| Producto Origen  |
| Producto Destino |
| Cantidad neta    |
| Unidad           |
And I validate the business error messages on the popup
And I validate that the errors are sorted by type ascending
When the user clicks on the Aceptar button
Then the error popup must disappear
And I should see breadcrumb "Deltas oficiales por nodo"