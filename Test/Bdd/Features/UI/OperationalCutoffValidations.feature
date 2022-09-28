@sharedsteps=57721 @owner=jagudelos @ui @MVP2and3 @S14 @testsuite=55122 @testplan=55104
Feature: OperationalCutoffValidations
As a Balance Segment Professional User,
I need some adjustments to be made to the
operational cutoff to include new validations

Background: Login
	Given I am logged in as "admin"

@parallel=false @testcase=57722
Scenario: Verify Names of the operational cutoff wizard steps
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	Then I should see "{Inicio}" "{Verificar inventarios}" "{Verificar mensajería}" "{Puntos de transferencia}" "{Verificar consistencia}" on the wizard

@parallel=false @testcase=57723 @version=2 @BVT2
Scenario: Validate message when there are only movements or inventories of the official scenario
	Given I have valid official Movements with same identifier in the system
	And I update the segment to SON
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I refresh the page
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	Then I should see the message "No existen registros para el periodo seleccionado" in the Page

@parallel=false @testcase=57724 @version=3 @BVT2
Scenario: Verify next step of wizard should display when there are movements or inventories of the operative scenario
	Given I have "ownershipnodes" created
	When I navigate to "Operational Cutoff" page
	And I refresh the current page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
	Then I should see "InitTicket" "submit" "button" as enabled
	And I click on "InitTicket" "submit" "button"
	And I should see the title "Verificar inventarios para el corte del"

@parallel=false @testcase=57725 @version=2
Scenario: Verify the error message should be displayed when cut-off is running for segment during the initiation of operational cutoff
	Given I have nodes of the segment for the selected period already have an operational cutoff executed
	And I had again run the cutoff for the same segment in the another screen/tab
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	Then I should see the message "Ya existe un corte operativo en ejecución para el segmento seleccionado." in the Page

@parallel=false @testcase=57726 @version=3
Scenario: Verify the error message should be displayed when delta calculation process for same segment during the initiation of operational cutoff
	Given I have deltas calculation process is running for the segment
	And I had again run the cutoff for the same segment in the another screen/tab
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	Then I should see the message "Se encuentra en ejecución un cálculo de deltas para el segmento seleccionado. Por favor espere a que este proceso finalice para iniciar el corte operativo." in the Page

@parallel=false @testcase=57727 @version=3
Scenario: Verify details on messaging step of operational cutoff wizard
	Given I have "ownershipnodes" created
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	Then I should see the title "Mensajes con excepción para el corte del"
	And I should see the label contains word segmento at the top left in lowercase
	And I should see the "errorsGrid_addNote" button name as "Agregar nota" with the first capital letter
	And I should see the below "Columns" on the page
		| Columns          |
		| Excepción        |
		| Fuente           |
		| Clasificación    |
		| Acción           |
		| Nodo origen      |
		| Nodo destino     |
		| Producto origen  |
		| Producto destino |
		| Cantidad neta    |
		| Unidad           |
		| Fecha            |

@parallel=false @testcase=57728 @version=2
Scenario: Verify messages with exception to manage on messaging step of the operational cutoff wizard
	Given I have "ownershipnodes" created
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	Then I should see the title "Mensajes con excepción para el corte del"
	And I should see the message "Debe gestionar los mensajes" on the Page

@parallel=false @testcase=57729 @version=2
Scenario: Verify tool tip to see the full exception message on messaging step of the operational cutoff wizard
	Given I have "ownershipnodes" created
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	Then I should see the title "Mensajes con excepción para el corte del"
	And I should see the tooltip when hovers over an any exception message

@parallel=false @testcase=57730 @manual @version=2
Scenario: Verify Exceptions should not appear on the grid when the technical exceptions stored during the selected date period have already been discarded
	Given I have movement with technical exception
	And I update the segment to SON 
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	And I should the see the messages with exceptions in messaging grid
	And I navigate to "Exception management" page
	And I filtered message from Exception management
	And I click on "pendingTransactionErrors" "discardException" "link"
	And I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	Then I should not see the discarded exceptions on the grid

@parallel=false @testcase=57731 @manual @version=2
Scenario: Verify Exceptions should not appear on the grid when the technical exceptions stored during the selected date period have already been retried
	Given I have movement with technical exception
	And I update the segment to SON 
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	And I should the see the messages with exceptions in messaging grid
	And I navigate to "Exception management" page
	And I filtered message from Exception management
	And I click on "pendingTransactionErrors" "retryRecord" "link"
	And I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	Then I should not see the retried exceptions on the grid

@parallel=false @testcase=57732 @bvt @version=2 @manual
Scenario: Verify Exceptions should appear on the grid when the technical exceptions stored during the selected date period and not been discarded or retried
	Given I have movement with technical exception
	And I update the segment to SON 
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	And I should see exceptions on the grid

@parallel=false @testcase=57733 @manual
Scenario: Verify Exceptions should not appear on the grid when the business exceptions stored during the selected date period have already been discarded
	Given I have movement with business exception
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	And I should the see the messages with exceptions in messaging grid
	And I navigate to "Exception management" page
	And I filtered message from Exception management
	And I click on "pendingTransactionErrors" "discardException" "link"
	And I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	Then I should not see the discarded exceptions on the grid

@parallel=false @testcase=57734 @manual
Scenario: Verify Exceptions should not appear on the grid when the business exceptions stored during the selected date period have already been retried
	Given I have movement with business exception
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	And I should the see the messages with exceptions in messaging grid
	And I navigate to "Exception management" page
	And I filtered message from Exception management
	And I click on "pendingTransactionErrors" "retryRecord" "link"
	And I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	Then I should not see the retried exceptions on the grid

@parallel=false @testcase=57735 @bvt @manual
Scenario: Verify Exceptions should appear on the grid when the business exceptions stored during the selected date period and not been discarded or retried
	Given I have business exceptions in grid which not been discarded or retried
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	And I should see exceptions on the grid

@parallel=false @testcase=57736 @version=3
Scenario: Verify note field messages with exceptions on messaging step of the operational cutoff wizard
	Given I have "ownershipnodes" created
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	Then I should see the title "Mensajes con excepción para el corte del"
	And I select all pending records from grid
	And I click on "errorsGrid" "addNote" "button"
	And I should see the label "Escriba sus comentarios aquí." on Note popup
	And I click on "addComment" "submit" "button"
	And I should see the mandatory message "Requerido" in the Page

@parallel=false @testcase=57737 @version=3
Scenario: Verify details on Check consistency step of the operational cutoff wizard
	Given I have "ownershipnodes" created
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	Then I should see the title "Mensajes con excepción para el corte del"
	And I select all pending records from grid
	And I click on "errorsGrid" "addNote" "button"
	And I enter valid value into "addComment" "Comment" "textbox"
	And I click on "addComment" "submit" "button"
	And I click on "ErrorsGrid" "Submit" "button"
	And I should see the title "Movimientos de puntos de transferencia sin conciliación oficial"
	And I click on "officialPointsGrid" "submit" "button"
	And I should see the title "Nodos en desbalance para el corte del"
	And I should see the label contains word segmento at the top left in lowercase
	And I should see the "consistencyCheck_addNote" button name as "Agregar nota" with the first capital letter
	And I should see the message "Debe gestionar los desbalances" on the Page

@parallel=false @testcase=57738 @version=3
Scenario: Verify Add notes for unbalances field messages on the check consistency step of the operational cutoff wizard.
	Given I have "ownershipnodes" created
	When I navigate to "Operational Cutoff" page
	And I refresh the page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "14" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	Then I should see the title "Mensajes con excepción para el corte del"
	And I select all pending records from grid
	And I click on "errorsGrid" "addNote" "button"
	And I enter valid value into "addComment" "Comment" "textbox"
	And I click on "addComment" "submit" "button"
	And I click on "ErrorsGrid" "Submit" "button"
	And I should see the title "Movimientos de puntos de transferencia sin conciliación oficial"
	And I select all pending records from grid
	And I click on "officialPointsGrid" "addNote" "button"
	And I enter valid value into "addComment" "Comment" "textbox"
	And I click on "addComment" "submit" "button"
	And I click on "officialPointsGrid" "submit" "button"
	And I should see the title "Nodos en desbalance para el corte del"
	And I select all pending records from grid
	And I click on "consistencyCheck" "addNote" "button"
	And I should see the label "Escriba sus comentarios aquí" on Note popup
	And I click on "addComment" "submit" "button"
	And I should see the mandatory message "Requerido" in the Page

@parallel=false @testcase=57739 @version=3
Scenario: Verify the error message should be displayed when already a cut-off is running
	Given I have "ownershipnodes" created
	And I had again run the cutoff for the same segment in the another screen
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	Then I should see the title "Mensajes con excepción para el corte del"
	And I select all pending records from grid
	And I click on "errorsGrid" "addNote" "button"
	And I enter valid value into "addComment" "Comment" "textbox"
	And I click on "addComment" "submit" "button"
	And I click on "ErrorsGrid" "Submit" "button"
	And I should see the title "Movimientos de puntos de transferencia sin conciliación oficial"
	And I select all pending records from grid
	And I click on "officialPointsGrid" "addNote" "button"
	And I enter valid value into "addComment" "Comment" "textbox"
	And I click on "addComment" "submit" "button"
	And I click on "officialPointsGrid" "submit" "button"
	And I should see the title "Nodos en desbalance para el corte del"
	And I select all pending records from grid
	And I click on "consistencyCheck" "addNote" "button"
	And I enter valid value into "addComment" "Comment" "textbox"
	And I click on "addComment" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I should see message popup window
	And I click on "Accept" "button"
	Then I should see the message "Ya existe un corte operativo en ejecución para el segmento seleccionado." in the Page

@parallel=false @testcase=57740 @version=3
Scenario: Verify the error message should be displayed when A delta calculation process for the same segment is running
	Given I have deltas calculation process is running for the segment
	And I had again run the cutoff for the same segment in the another screen
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "5" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And I click on "validateInitialInventory" "submit "button"
	Then I should see the title "Mensajes con excepción para el corte del"
	And I select all pending records from grid
	And I click on "errorsGrid" "addNote" "button"
	And I enter valid value into "addComment" "Comment" "textbox"
	And I click on "addComment" "submit" "button"
	And I click on "ErrorsGrid" "Submit" "button"
	And I should see the title "Movimientos de puntos de transferencia sin conciliación oficial"
	And I click on "officialPointsGrid" "submit" "button"
	And I should see the title "Nodos en desbalance para el corte del"
	And I select all pending records from grid
	And I click on "consistencyCheck" "addNote" "button"
	And I enter valid value into "addComment" "Comment" "textbox"
	And I click on "addComment" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I should see message popup window
	And I click on "Accept" "button"
	Then I should see the message "Se encuentra en ejecución un cálculo de deltas para el segmento seleccionado. Por favor espere a que este proceso finalice para iniciar el corte operativo." in the Page