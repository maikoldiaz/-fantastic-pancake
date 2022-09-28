@Owner=jagudelos @ui @testplan=11317 @testsuite=11329
Feature: AdjustmentsToVolumetricCutoffCheckMessaging
In order to perform the volumetric cutoff information
As a transport segement user
I want to check messaging

@testcase=12893 @version=3 @prodready
Scenario: Verify error message is displayed when there are no movements and inventories in the selected period
	Given I am logged in as "admin"
	When I navigate to "Operational Cutoff" page
	When I click on "NewCut" "button"
	Then I should see "Start" "link"
	When I choose CategoryElement from "InitTicket" "Segment" "combobox" which does not have movements and inventories in the selected period
	And I select the FinalDate lessthan "2" days from CurrentDate on DatePicker
	When I click on "InitTicket" "submit" "button"
	Then I should see the message "No existen registros para el periodo seleccionado" in the Page
	And I should see "InitTicket" "submit" "button" as disabled

@testcase=12894 @version=2
Scenario: Verify the text on the Check Consistency modal confirmation window
	Given I have pending transactions in the system
	When I navigate to "Operational Cutoff" page
	When I click on "NewCut" "button"
	Then I should see "Start" "link"
	When I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on DatePicker
	When I click on "InitTicket" "submit" "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	Then I should see "Add Note Functions" interface
	When I provide value for "AddComment" "comment" "textbox"
	And I click on "AddComment" "submit" "button"
	Then I should see the message "No existen movimientos e inventarios pendientes" when there are no pending records
	When I click on "ErrorsGrid" "Submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	Then I should see "Note for imbalances" interface
	When I provide value for "AddComment" "comment" "textbox"
	And I click on "AddComment" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	Then I should see "Confirmation" interface
	And I should see the label "Confirmación de Ejecución de Corte Operativo"
	And I should see selected values in the labels Category, CategoryElement and Period

@testcase=12895 @version=2
Scenario: Verify Operational Cutoff tracking page is displayed when Accept button is clicked on the Check Consistency confirmation window
	Given I am having pending records in Operational Cutoff page
	When I navigate to "Operational Cutoff" page
	When I click on "NewCut" "button"
	Then I should see "Start" "link"
	When I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on DatePicker
	When I click on "InitTicket" "submit" "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	Then I should see "Add Note Functions" interface
	When I provide value for "AddComment" "comment" "textbox"
	And I click on "AddComment" "submit" "button"
	Then I should see the message "No existen movimientos e inventarios pendientes" when there are no pending records
	When I click on "ErrorsGrid" "Submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	Then I should see "Note for imbalances" interface
	When I provide value for "AddComment" "comment" "textbox"
	And I click on "AddComment" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	Then I should see "Confirmation" interface
	And I should see the label "Confirmación de Ejecución de Corte Operativo"
	And I should see selected values in the labels Category, CategoryElement and Period
	When I click on "ConfirmCutoff" "Submit" "button"
	Then I should see the "Operational Cutoffs" page