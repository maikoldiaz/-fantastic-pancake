@sharedsteps=4013 @owner=jagudelos @ui @testsuite=70805 @testplan=70526 @S16
Feature: TriggerOfficialSivGenerationProcess
When Chain user is logged into the TRUE system
And the system has required segments, nodes approved
Then the user should be able to trigger the official SIV file generation process

Background: Login
Given I am logged in as "chain"
@testcase=71754
Scenario: Validate the navigation to the SIV file generation wizard
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
Then I validate that a popup is displayed containing the SIV file generation wizard
@testcase=71755
Scenario: Validate the contents and functionality of each element on Criteria step inside the wizard
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
Then I validate that a dropdwon is displayed with title Segmento
And I validate that all Active segments are listed inside the Segmento dropdown
And I validate that a dropdwon is displayed with title Nodo
And I validate that active nodes belonging to the chosen segment and nodes with "Sent to SAP" property enabled are listed inside the Nodo dropdown
And I validate that the Nodo dropdown displays Todos as a dropdown value
And I validate the results in the Nodo dropdown when the user enters some text
And I validate that owners Ecopetrol and Reficar are displayed under Propietario section
And I validate that Cancelar button is displayed and is in enabled state
And I validate that Siguiente button is displayed and is in enabled state
@testcase=71756
Scenario: Validate the functionality of Cancel button on Criteria step inside the wizard
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
When I select a segment from the Segmento dropdown
And I select a node from the Nodo dropdown
And I select Ecopetrol as owner
And I click on the Cancelar button
Then I validate that all the selected fields are cleared
@testcase=71757
Scenario: Validate the functionality of Accept button on Criteria step inside the wizard
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
And I click on the Siguiente button
Then I validate that the mandatory fields validation is done
And I validate that the control still resides in the Criteria step
@testcase=71758
Scenario: Validate the contents and state of each element on Period step inside the wizard
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
When I select a segment from the Segmento dropdown
And I select a node from the Nodo dropdown
And I select Ecopetrol as owner
And I click on the Siguiente button
Then I validate the subtitle 'Seleccione el período para el procesamiento:'
And I validate that a dropdwon is displayed with title 'Año'
And I validate that a dropdwon is displayed with title 'Período del procesamiento'
And I validate that Cancelar button is displayed and is in enabled state
And I validate that Siguiente button is displayed and is in enabled state
@testcase=71759
Scenario: Validate the functionality of Year dropdown on Period step inside the wizard
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
When I select a segment from the Segmento dropdown
And I select a node from the Nodo dropdown
And I select Ecopetrol as owner
And I click on the Siguiente button
Then I validate that the year dropdown displays last 5 years in descending order as options
And I validate that by default current year is selected inside the year dropdown
@testcase=71760
Scenario: Validate the functionality of Period dropdown on Period step inside the wizard
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
When I select a segment from the Segmento dropdown
And I select a node from the Nodo dropdown
And I select Ecopetrol as owner
And I click on the Siguiente button
Then I validate that Period dropdown displays all the official deltas in Deltas status for the chosen segment
And I validate that the start time is greater than or equal to the first day of year selected
And I validate that the end time is less than or equal to the last day of year selected
And I validate that the periods are shown in descending order inside the Period dropdown
And I validate the format of the periods displayed
And I validate the results in the Period dropdown when the user enters some text
@testcase=71761
Scenario: Validate the functionality of Cancel button on Period step inside the wizard
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
And I select a segment from the Segmento dropdown
And I select a node from the Nodo dropdown
And I select Ecopetrol as owner
And I click on the Siguiente button
And I select a year from year dropdown
And I select a period from the period dropdown
And I click on the Cancelar button
Then I validate that all the selected fields are cleared
And the control is navigated back to the Criteria step of the wizard
@testcase=71762
Scenario: Validate the functionality of Accept button on Period step inside the wizard
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
And I select a segment from the Segmento dropdown
And I select a node from the Nodo dropdown
And I select Ecopetrol as owner
And I click on the Siguiente button
And I click on the Siguiente button
Then I validate that the mandatory fields validation is done
And I validate that the control still resides in the Period step
@testcase=71763
Scenario: Validate the error message when no values exists in Period dropdown
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
And I select a segment from the Segmento dropdown
And I select a node from the Nodo dropdown
And I select Ecopetrol as owner
And I click on the Siguiente button
And I select a year to which there are no periods with official deltas from year dropdown
Then I validate that error message 'No se encontró información de deltas calculados para el segmento y año elegido. Por favor, primero ejecute el cálculo de deltas.' is diplayed
@testcase=71764
Scenario: Validate the functionality when there are unapproved official balances for the chosen segment or node when all nodes are selected
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
And I select a segment from the Segmento dropdown
And I select Todos from the Nodo dropdown
And I select Ecopetrol as owner
And I click on the Siguiente button
And I select a year from year dropdown
And I select a period from the period dropdown
And I click on the Siguiente button
Then I validate the value of Segmento
And I validate the value of Nodo as 'Todos'
And I validate the value of Propietario as the one selected during Criteria step
And I validate the value of Periodo as the one selected during Criteria step
And I validate that 'Error' label is displayed with message 'Existen algunos nodos que no han sido aprobados para el período oficial seleccionado.'
And I validate the value of Total de nodos
And I validate that the nodes that are not approved are displayed in a tabular format in ascending order along with their statuses
And I validate that Cancelar button is displayed and is in enabled state
And I validate the state of Create Logistic Report button as 'disabled'
@testcase=71765
Scenario: Validate the functionality when official balances of the chosen node is unapproved
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
And I select a segment from the Segmento dropdown
And I select a node from the Nodo dropdown
And I select Ecopetrol as owner
And I click on the Siguiente button
And I select a year from year dropdown
And I select a period from the period dropdown
And I click on the Siguiente button
Then I validate the value of Segmento
And I validate the value of Nodo as 'the one selected during Criteria step'
And I validate the value of Propietario as the one selected during Criteria step
And I validate the value of Periodo as the one selected during Criteria step
And I validate that 'Error' label is displayed with message 'El nodo no ha sido aprobado para el período oficial seleccionado.'
And I validate the value of Total de nodos as 1
And I validate that the table shows only the selected node along with its current status
And I validate that Cancelar button is displayed and is in enabled state
And I validate the state of Create Logistic Report button as 'disabled'
@testcase=71766
Scenario: Validate the functionality when there are NO unapproved official balances for the chosen segment or node
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
And I select a segment from the Segmento dropdown
And I select Todos from the Nodo dropdown
And I select Ecopetrol as owner
And I click on the Siguiente button
And I select a year from year dropdown
And I select a period from the period dropdown
And I click on the Siguiente button
Then I validate the value of Segmento
And I validate the value of Nodo as 'Todos'
And I validate the value of Propietario as the one selected during Criteria step
And I validate the value of Periodo as the one selected during Criteria step
And I validate that 'Validation' label is displayed with message 'Todos los nodos se encuentran aprobados para continuar el proceso.'
And I validate the value of Total de nodos as 0
And I validate that the table displays the value 'Sin registros'
And I validate that Cancelar button is displayed and is in enabled state
And I validate the state of Create Logistic Report button as 'enabled'
@testcase=71767
Scenario: Validate the functionality when official balances of the chosen node is approved
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
And I select a segment from the Segmento dropdown
And I select a node from the Nodo dropdown
And I select Ecopetrol as owner
And I click on the Siguiente button
And I select a year from year dropdown
And I select a period from the period dropdown
And I click on the Siguiente button
Then I validate the value of Segmento
And I validate the value of Nodo as 'the one selected during Criteria step'
And I validate the value of Propietario as the one selected during Criteria step
And I validate the value of Periodo as the one selected during Criteria step
And I validate that 'Validation' label is displayed with message 'El nodo seleccionado se encuentra aprobado para continuar el proceso.'
And I validate the value of Total de nodos as 0
And I validate that the table displays the value 'Sin registros'
And I validate that Cancelar button is displayed and is in enabled state
And I validate the state of Create Logistic Report button as 'enabled'
@testcase=71768
Scenario: Validate the functionality of Cancel button on Validation step inside the wizard
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
And I select a segment from the Segmento dropdown
And I select a node from the Nodo dropdown
And I select Ecopetrol as owner
And I click on the Siguiente button
And I select a year from year dropdown
And I select a period from the period dropdown
And I click on the Siguiente button
And I click on the Cancelar button
And the control is navigated back to the Criteria step of the wizard
@testcase=71769
Scenario: Validate the functionality and contents of the Confirmation popup when user clicks on the Create Logistic Report button on Validation step inside the wizard
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
And I select a segment from the Segmento dropdown
And I select a node from the Nodo dropdown
And I select Ecopetrol as owner
And I click on the Siguiente button
And I select a year from year dropdown
And I select a period from the period dropdown
And I click on the Siguiente button
And I click on the Create Logistic Report button
Then I validate that a popup is displayed with title 'Confirmación'
And I validate the tag inside the popup as 'Confirmación del procesamiento del archivo logístico oficial:'
And I validate the value of Segment on the popup
And I validate the value of Node on the popup
And I validate the value of Owner on the popup
And I validate the value of Period on the popup
And I validate that Cancelar button is visible on the popup
And I validate that Aceptar button is visible on the popup
@testcase=71770 
Scenario: Validate the functionality of Cancel and Accept buttons on the confirmation popup
When I navigate to "Official logistics movements and inventories" page
And I click on the Create file button
And I select a segment from the Segmento dropdown
And I select a node from the Nodo dropdown
And I select Ecopetrol as owner
And I click on the Siguiente button
And I select a year from year dropdown
And I select a period from the period dropdown
And I click on the Siguiente button
And I click on the Create Logistic Report button
And I click on the Cancelar button
Then I validate that the popup disappears and the control is still on Validation step
When I click on the Create Logistic Report button
And I click on the Aceptar button
Then I validate that the system initiated the official SIV file generation process
And I validate that the control is now on 'Official logistics movements and inventories' page
And I validate that a new record is displayed on the official SIV process table in 'Procesando' status
And I validate the values of the newly created record for each available column
