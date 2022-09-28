@sharedsteps=16581 @owner=jagudelos @ui @testplan=31102 @testsuite=31109 @parallel=false
Feature: AdjustmentsToLogisticReportGenerationProcess
As a Professional Segment Balance User, I need to update
the logistic report UI to generate it per node and validate approvals

Background: Login
Given I am logged in as "profesional"

@testcase=33769 @bvt1.5
Scenario: Verify the displayed columns on Logistic Report Generation page
When I navigate to "Logistic Report Generation" page
Then I should see the "Columns" on the page
| Columns         |
| Segmento        |
| Nodo            |
| Propietario     |
| Fecha Inicial   |
| Fecha Final     |
| Fecha Ejecución |
| Usuario         |
| Estado          |

@testcase=33770 @version=2 @bvt1.5
Scenario: Verify Logistic report generation UI wizard
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "Logistics Criteria" "Interface"
	And I should see "Criterios" "Período" "Validación" on the wizard

@testcase=33771 @version=2 @bvt1.5
Scenario: Verify criteria step on Logistic report generation UI wizard
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	And I see "Criterios" step on the wizard
	Then I should see "Seleccione los criterios" label on the wizard
	And startdate and enddate fields should not be displayed
	And ecopetrol should be shown first in the owners
	And "Nodo" field should be displayed

@testcase=33772 @version=2
Scenario: Verify field on criteria step of Logistic report generation UI wizard
	And I have a segment where ownership is already calculated
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	And I see "Criterios" step on the wizard
	And I select ownership calculated segment from "Logistics Criteria" "Segment" "combobox"
	And I click on Node textbox on criteria step
	Then it should display "Todos" value for selection
	And I should be able to select nodes belong to that segment 
	And sent to SAP property is enabled for those nodes

@testcase=33773 @version=2
Scenario: Verify period step on Logistic report generation UI wizard
	And I have a segment where ownership is already calculated
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	And I have provided all required validations on criteria step
	And I click on "Logistics Criteria" "next" "button"
	Then I should see "Seleccione los criterios" label on the wizard
	And startdate and enddate fields should be displayed

@testcase=33774 @version=2
Scenario: Verify validation step on Logistic report generation UI wizard when all nodes in selected segment and period does not have status Approved
	And I have a segment where ownership is already calculated
	And I have nodes in the segment with status "is not Approved"
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	And I have provided all required validations on criteria step
	And I click on "Logistics Criteria" "next" "button"
	And I have selected start date and end date on period step
	And I click on "Logistics Period" "next" "button"
	Then I should see selected segment, node, owner and period
	And I should see error message on confirmation popup as "Existen algunos nodos que no están aprobados para el periodo seleccionado."
	And I should see total number of failed records
	And one row for each day that is not approved, sorted by node and date descendent
	And I should see name of button as "Aceptar"

@testcase=33775 @version=2
Scenario: Verify validation step on Logistic report generation UI wizard when all nodes in selected segment and period have status Approved
	And I have a segment where ownership is already calculated
	And I have nodes in the segment with status "is Approved"
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	And I have provided all required validations on criteria step
	And I click on "Logistics Criteria" "next" "button"
	And I have selected start date and end date on period step
	And I click on "Logistics Period" "next" "button"
	Then I should see "La validación de nodos aprobados se completó correctamente" label on the wizard
	And I should see selected segment, node, owner and period
	And I should see name of button as "Crear reporte logístico"

@testcase=33776
Scenario: Verify accept button functionality on validation step of Logistic report generation UI wizard
And I have a segment where ownership is already calculated
And I have nodes in the segment with status "is not Approved"
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I have provided all required validations on criteria step
And I click on "Logistics Criteria" "next" "button"
And I have selected start date and end date on period step
And I click on "Logistics Period" "next" "button"
And I click on "Logistics Validation" "Accept" "button"
Then the popup should be closed

@testcase=33777 @version=2 @bvt1.5
Scenario: Verify Logistic Report generated in Logistic Report Generation page
	And I have a segment where ownership is already calculated
	And I have nodes in the segment with status "is Approved"
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	And I have provided all required validations on criteria step
	And I click on "Logistics Criteria" "next" "button"
	And I have selected start date and end date on period step
	And I click on "Logistics Period" "next" "button"
	And I see label "La validación de nodos aprobados se completó correctamente"
	And I see selected segment, node, owner and period
	And I click on "Logistics Validation" "Create Logistic Report" "button"
	Then I should see Logistic Report for selected segment, node and period in the Logistic Report Generation grid

@testcase=33778 @version=2
Scenario: Verify Ownership balance file can be generated more than once for same segment, node with same date range
	And I have a segment where ownership is already calculated
	And I have nodes in the segment with status "is Approved"
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	And I have provided all required validations on criteria step
	And I click on "Logistics Criteria" "next" "button"
	And I have selected start date and end date on period step
	And I click on "Logistics Period" "next" "button"
	And I see label "La validación de nodos aprobados se completó correctamente"
	And I see selected segment, node, owner and period
	And I click on "Logistics Validation" "Create Logistic Report" "button"
	Then I should see Logistic Report for selected segment, node and period in the Logistic Report Generation grid
	When I click on "CreateLogistics" "button"
	And I have test data for second logistic report generation
	And I have provided all required validations on criteria step
	And I click on "Logistics Criteria" "next" "button"
	And I have selected start date and end date on period step
	And I click on "Logistics Period" "next" "button"
	And I see label "La validación de nodos aprobados se completó correctamente"
	And I see selected segment, node, owner and period
	And I click on "Logistics Validation" "Create Logistic Report" "button"
	Then I should see total 2 Logistic Reports for selected segment, node and period in the Logistic Report Generation grid

@testcase=33779
Scenario: Verify required error message validation on criteria step of Logistic report generation UI wizard
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I see "Criterios" step on the wizard
And I have not selected segment, node and owner on the wizard
And I click on "Logistics Criteria" "next" "button"
Then I should see the message on interface "Requerido"

@testcase=33780
Scenario: Verify required error message validation on period step of Logistic report generation UI wizard
And I have a segment where ownership is already calculated
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I have provided all required validations on criteria step
And I click on "Logistics Criteria" "next" "button"
And I have not selected start date and end date on the wizard
And I click on "Logistics Period" "next" "button"
Then I should see the message on interface "Requerido"

@testcase=33781 @version=2
Scenario: Verify error message validation when user selected start date greater than final date on period step of Logistic report generation UI wizard
	And I have a segment where ownership is already calculated
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	And I have provided all required validations on criteria step
	And I click on "Logistics Criteria" "next" "button"
	And I have selected start date greater than end date on the wizard
	And I click on "Logistics Period" "next" "button"
	Then I should see error message "La fecha inicial debe ser menor o igual a la fecha final"

@testcase=33782 @version=2
Scenario: Verify error message validation when user selected range between start and end date greater than 60 days on period step of Logistic report generation UI wizard
	And I have a segment where ownership is already calculated
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	And I have provided all required validations on criteria step
	And I click on "Logistics Criteria" "next" "button"
	And I have selected range between start date and end date is more than 60 days
	And I click on "Logistics Period" "next" "button"
	Then I should see error message "El rango de días elegidos debe ser menor a 60 días."

@testcase=33783 @version=2
Scenario: Verify validations while generation of a file in Logistic Report Generation page where Ownership is not calculated for selected Segment
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	And I select Segment where Ownership is not calculated for it
	And I have provided other required validations on criteria step
	And I click on "Logistics Criteria" "next" "button"
	Then I should see error message "No se encontró cálculo de propiedad para el segmento. Por favor realice el cálculo de propiedad primero."

@testcase=33784
Scenario: Verify Cancel button functionality on criteria step of Logistic report generation UI wizard
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I see "Criterios" step on the wizard
And I click on "Logistics Criteria" "Cancel" "button"
Then the popup should be closed

@testcase=33785 @version=2
Scenario: Verify Cancel button functionality on period step of Logistic report generation UI wizard
	And I have a segment where ownership is already calculated
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	And I have provided all required validations on criteria step
	And I click on "Logistics Criteria" "next" "button"
	And I see "Period" step on the wizard
	And I click on "Logistics Period" "Cancel" "button"
	Then the popup should be closed

@testcase=33786 @version=2
Scenario: Verify Close button functionality on Logistic report generation UI wizard
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	And I see "Criterios" step on the wizard
	And I click on "modal" "close" "label"
	Then the popup should be closed