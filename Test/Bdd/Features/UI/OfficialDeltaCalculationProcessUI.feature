@owner=jagudelos @testplan=61542 @testsuite=61553 @ui @S15 @MVP2and3
Feature: OfficialDeltaCalculationProcessUI

@testcase=66857 @version=2
Scenario Outline:  Verify that admin and USUARIO DE CADENA
	Given I am logged in as "<user>"
	Then I navigate to "TransactionsAudit" tab
	Then I should see "Calculation of deltas by official adjustment" tab

	Examples:
		| user     |
		| admin    |
		| chain    |
		| consulta |

@testcase=66858 @version=2
Scenario Outline: Verify Official Delta Calculation page should not be present for user other than administrator and USUARIO DE CADENA role under Gestión cadena de suministro menu
	Given I am logged in as "<User>"
	Then I should not see "Calculation of deltas by official adjustment" tab

	Examples:
		| User        |
		| aprobador   |
		| programador |

@testcase=66859 @version=2
Scenario: Verify that results should be sorted based on the execution date descendant by default in Calculation of Delta Page
	Given I am logged in as "admin"
	And I navigates to "Calculation of deltas by official adjustment" link under "TransactionsAudit" tab
	Then I verify all columns "Tiquete","Segmento","Fecha Inicial","Fecha Final","Fecha Ejecución","Usuario","Estado" are present in Grid
	Then the results should be sorted based on ExecutionDate be descending in the Official Delta Grid
	Then the records count in "Calculation of deltas by official adjust" Grid shown per page should also be 50

@testcase=66860 @manual @version=2
Scenario Outline: Verify the filtering functionality on Calculation of Delta page
	Given I am logged in as "admin"
	And I configure the N days from the Configuration in storage explorer
	And I navigates to "Calculation of deltas by official adjustment" link under "TransactionsAudit" tab
	Then the records from deltas executed in the last N days should be displayed in the Grid
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

@testcase=66861 @version=2 @BVT2
Scenario: Validate the technical error message when an exception arrives during the operational consolidation
	Given I am logged in as "admin"
	And I set the prerequisite to have a "consolidation" error generated
	And I set the Segment status to "Inctive"
	And I navigate to "Calculation of deltas by official adjustment" page
	When the user clicks on the error icon of the respective segment or node
	Then a popup must appear with title "Error durante el cálculo de los deltas"
	And I validate the error message on popup "Se presentó un error técnico inesperado en la consolidación del escenario operativo. Por favor ejecute nuevamente el proceso."
	And I validate the value "period start date" on the popup
	And I validate the value "period end date" on the popup
	And I validate the value "segment" on the popup

@testcase=66862 @parallel=false @version=2 @BVT2
Scenario: Verify that active segments should be displayed in the segment field when clicked on new delta calculation button
	Given I am logged in as "chain"
	When I choose active segment for Official Delta calculation
	And I set the Segment status to "Inctive"
	And I navigate to "Calculation of deltas by official adjustment" page
	And I click on "newDeltasCalculation" "button"
	Then Verify if Active segments are showing in the segment drop down
	And I select Todos segment from "initOfficialDeltaTicket" "segment" "dropdown"
	And I set the Segment status to "Active"

@testcase=66864 @parallel=false @version=2
Scenario: Verify year dropdown should have last 5 years as values and defaulted to current year
	Given I am logged in as "admin"
	And I choose active segment for Official Delta calculation
	And I navigates to "Calculation of deltas by official adjustment" link under "TransactionsAudit" tab
	Then I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	Then I verify year drop down should have last 5 years as values and defaulted to current year
	Then I verify default value of Año drop down is current year

@testcase=66864 @version=2
Scenario: Verify start date and end date of the movement should be with in the selected year.
	Given I am logged in as "chain"
	And I choose active segment with movement start date and end date for Official Delta calculation
	Then I check the start date and end date of movements should be between start date and end date of the selected year

@testcase=66865 @parallel=false @version=2 @BVT2
Scenario: Verify first step of the wizard or confirmation dialog should be displayed if segment without official information is chosen
	Given I am logged in as "admin"
	And I choose active segment for which official information is not present
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	Then Verify that wizard has "No se encontró información oficial para el (los) segmento(s) elegido(s). Por favor, primero cargue la información oficial." error message
	And validate that "initOfficialDeltaTicket" "submit" "button" as disabled

@testcase=66866 @parallel=false @version=2
Scenario: Verify the error message should be displayed when deltas calculation is running for a segment and started the delta calculation process again for todos/All
	Given I am logged in as "chain"
	And I choose segment which has delta calculation process running in the background
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select Todos segment from "initOfficialDeltaTicket" "segment" "dropdown"
	Then I select "Jun" period from drop down
	And I click on "initOfficialDeltaTicket" "submit" "button"
	Then Verify that wizard has "Se encuentra en procesamiento un cálculo de deltas oficiales para el segmento o la cadena." error message
	And validate that "initOfficialDeltaTicket" "submit" "button" as disabled

@testcase=66867 @parallel=false @version=2
Scenario: Verify the error message should be displayed when deltas calculation is running for segment and started the delta calculation process again
	Given I am logged in as "admin"
	And I choose active segment for Official Delta calculation
	When I set the delta calculation process "running" in the background for a segment
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	Then I select "Jun" period from drop down
	And I click on "initOfficialDeltaTicket" "submit" "button"
	Then Verify that wizard has "Se encuentra en procesamiento un cálculo de deltas oficiales para el segmento o la cadena." error message
	And validate that "initOfficialDeltaTicket" "submit" "button" as disabled
	When I set the delta calculation process "stopped" in the background for a segment

@testcase=66868 @parallel=false @version=2
Scenario: Verify the error message should be displayed when there are nodes with deltas for a previous period and the official deltas have not been approved for all nodes
	Given I am logged in as "chain"
	And I choose the active segment with nodes with deltas for a previous period and the official deltas have not been approved for all nodes
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	Then I select "Jun" period from drop down
	And I click on "initOfficialDeltaTicket" "submit" "button"
	Then Verify that wizard has "Existen balances oficiales de nodos sin aprobar para el período anterior." error message
	And validate that "initOfficialDeltaTicket" "submit" "button" as disabled

@testcase=66869 @parallel=false @version=2
Scenario: Verify failed validation when there are operationally unapproved nodes for the chosen segment
	Given I am logged in as "admin"
	And I choose a segment which has operationally unapproved nodes
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	Then I select "Jun" period from drop down
	Then I select "2020" year from drop down
	And I click on "initOfficialDeltaTicket" "submit" "button"
	Then I verify Chosen segment and period are displayed
	Then I verify total number of unapproved nodes in segment should be displayed
	Then Verify wizard has "Error" as "Existen algunos nodos que no han sido aprobados para el período operativo seleccionado, los cuales pertenecen a segmentos configurados en TRUE como SON."
	And Verify wizard has below columns
		| Columns         |
		| Segmento        |
		| Nombre del nodo |
		| Fecha           |
		| Estado          |
	Then I validate no paging is available for grid
	Then validate that "validateOfficialDeltaTicket" "submit" "button" as disabled

@testcase=66870 @parallel=false @version=2
Scenario: Verify successful validation if there are no operationally unapproved nodes for the chosen segment
	Given I am logged in as "chain"
	And I choose a segment which has no operationally unapproved nodes
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	Then I select "Jun" period from drop down
	Then I select "2020" year from drop down
	And I click on "initOfficialDeltaTicket" "submit" "button"
	Then I verify Chosen segment and period are displayed
	Then Verify wizard has "Validación" as "No hay nodos que requieran aprobación para el período operativo seleccionado."
	Then Verify wizard has "Total de registros:" as "0"
	And Verify wizard has below columns
		| Columns         |
		| Segmento        |
		| Nombre del nodo |
		| Fecha           |
		| Estado          |
	Then I should see message "Sin registros"
	Then validate that "validateOfficialDeltaTicket" "submit" "button" as enabled

@testcase=66871 @parallel=false @version=2 @BVT2
Scenario: Verify delta calculation process is stopped if official delta calculation for the segment is not running after confirmation
	Given I am logged in as "admin"
	And I choose active segment with movement start date and end date for Official Delta calculation
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	Then I select "Jun" period from drop down
	Then I select "2020" year from drop down
	And I click on "initOfficialDeltaTicket" "submit" "button"
	And I click on "validateOfficialDeltaTicket" "submit" "button"
	Then I verify "Confirmación" model pop up displayed with message "Confirmación de consolidación operativa y cálculo de deltas oficiales:"
	Then I verify Chosen segment and period are displayed
	Then validate that "officialDeltaTicket" "cancel" "button" as enabled
	And I click on "officialDeltaTicket" "cancel" "button"
	Then Verify wizard has "Validación" as "No hay nodos que requieran aprobación para el período operativo seleccionado"
	And I click on "validateOfficialDeltaTicket" "submit" "button"
	When I set the delta calculation process "running" in the background for a segment
	And I refresh the page
	Then validate that "confirmOfficialDeltaTicket" "submit" "button" as enabled
	Then I click on "confirmOfficialDeltaTicket" "submit" "button"
	Then Verify that "No se encontró información oficial para el (los) segmento(s) elegido(s). Por favor, primero cargue la información oficial" error message should be displayed.
	When I set the delta calculation process "stopped" in the background for a segment

@testcase=66872 @parallel=false @version=2
Scenario: Verify delta calculation process asynchronously
	Given that the TRUE system is processing the operative movements consolidation
	When I have SON segment that has operative movements with an operational date within the dates of a period
	And movements have an ownership ticket
	And the movements with a source movement identifier are of different types than the cancellation types configured in the relationships between movement types
	And the segment has neither consolidated movements for the date period nor consolidated inventories with a date equal to the end date of the period
	Then consolidate the net quantity and gross quantity of the movements grouping the information by source node, destination node, source product, destination product and movement type
	And consolidate the ownership quantity of the movements by source node, destination node, source product, destination product, movement type and owner
	And the ownership of the movements must be obtained from the ownership information returned by FICO
	And join the information from the previous two points using source node, destination node, source product, destination product and movement type
	And store the consolidated movements with "<field>"
		| field                                                          |
		| start date                                                     |
		| end date                                                       |
		| movement type identifier                                       |
		| source node identifier                                         |
		| destination node identifier                                    |
		| source product identifier                                      |
		| destination product identifier                                 |
		| net quantity                                                   |
		| gross quantity                                                 |
		| unit identifier                                                |
		| owner ID                                                       |
		| ownership quantity                                             |
		| ownership percentage with respect to consolidated net quantity |
		| scenario identifier                                            |
		| segment identifier                                             |
		| source                                                         |
		| execution date-time of the consolidation process               |