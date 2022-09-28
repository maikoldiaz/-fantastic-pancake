@sharedsteps=16581 @owner=jagudelos @ui @testplan=49466 @testsuite=49476 @MVP2and3
Feature: GenerateTheInventoryAndMovementAuditReports
As an auditor or administrator user, I need a filter
page to generate the inventory and movement audit reports

Background: Login
	Given I am logged in as "admin"
	And I have segment category in the system
	And I have inactive segment "Random"

@testcase=52350
Scenario: Verify report page should present under Gestión cadena de suministro menu
	When I click on "Menu" toggler
	And I click on "Supply Chain Management" link
	Then I should see the "TransactionsAudit" option

@testcase=52351
Scenario: Verify breadcrumb for Inventory and movements audit page
	When I navigate to "TransactionsAudit" page
	Then I should see breadcrumb "Auditoría de movimientos e inventarios"

@testcase=52352 @version=2 @BVT2
Scenario: Verify inactive segments should not display in the segment dropdown on initial page load of Inventory and movements audit page
	When I navigate to "TransactionsAudit" page
	Then I should see the "Select" option
	When I should not see inactive segment "Random" from "NodeFilter" "element" "dropdown"

@testcase=52353 @BVT2
Scenario: Verify movements radio button should be selected by default on initial page load of Inventory and movements audit page
	When I navigate to "TransactionsAudit" page
	Then I should see movements radio button checked by default

@testcase=52354
Scenario: Verify start date and end date fields on initial page load of Inventory and movements audit page
	When I navigate to "TransactionsAudit" page
	Then I validate that the date in "nodeFilter" "initialDate" "date" should be enabled until one day before the current date
	And I validate that the date in "nodeFilter" "finalDate" "date" should be enabled until one day before the current date

@testcase=52355 @BVT2
Scenario: Verify mandatory fields on initial page load of Inventory and movements audit page
	When I navigate to "TransactionsAudit" page
	And I click on "nodeFilter" "Submit" "button"
	Then I should see error message "Requerido" below each field on "TransactionsAudit" page

@testcase=52356
Scenario: Verify error message when user selects initial date greater than final date in TransactionsAudit page
	When I navigate to "TransactionsAudit" page
	When I select any "SegmentValue" from "NodeFilter" "element" "dropdown"
	And I click on "Movimientos" RadioButton
	And I select the date in "nodeFilter" "initialDate" "date" based on below criteria
		| key           | value               |
		| dateSelection | initial             |
		| daysLessThen  | 2                   |
		| page          | Audit Configuration |
	And I select the date in "nodeFilter" "finalDate" "date" based on below criteria
		| key           | value               |
		| dateSelection | final               |
		| daysLessThen  | 4                   |
		| page          | Audit Configuration |
	And I click on "nodeFilter" "Submit" "button"
	Then I should see the error message "La fecha inicial debe ser menor o igual a la fecha final."

@testcase=52357 @BVT2
Scenario: Verify error message when user selects number of valid days between the initial and final date is greater than the value configured in the valid days parameter
	When I navigate to "TransactionsAudit" page
	When I select any "SegmentValue" from "NodeFilter" "element" "dropdown"
	And I click on "Inventarios" RadioButton
	And I select the date in "nodeFilter" "initialDate" "date" based on below criteria
		| key           | value               |
		| dateSelection | initial             |
		| daysLessThen  | 64                  |
		| page          | Audit Configuration |
	And I select the date in "nodeFilter" "finalDate" "date" based on below criteria
		| key           | value               |
		| dateSelection | final               |
		| daysLessThen  | 1                   |
		| page          | Audit Configuration |
	And I click on "nodeFilter" "Submit" "button"
	Then I should see the error message "El rango de días elegidos debe ser menor a 62 días"

@testcase=52358
Scenario: Verify inventory audit report
	When I navigate to "TransactionsAudit" page
	When I select any "SegmentValue" from "NodeFilter" "element" "dropdown"
	And I click on "Inventarios" RadioButton
	And I select the date in "nodeFilter" "initialDate" "date" based on below criteria
		| key           | value               |
		| dateSelection | initial             |
		| daysLessThen  | 4                   |
		| page          | Audit Configuration |
	And I select the date in "nodeFilter" "finalDate" "date" based on below criteria
		| key           | value               |
		| dateSelection | final               |
		| daysLessThen  | 3                   |
		| page          | Audit Configuration |
	And I click on "nodeFilter" "Submit" "button"
	Then I should see "backToTransactionsAudit" "button"

@testcase=52359 @BVT2
Scenario: Verify movement audit report
	When I navigate to "TransactionsAudit" page
	When I select any "SegmentValue" from "NodeFilter" "element" "dropdown"
	And I click on "Movimientos" RadioButton
	And I select the date in "nodeFilter" "initialDate" "date" based on below criteria
		| key           | value               |
		| dateSelection | initial             |
		| daysLessThen  | 10                  |
		| page          | Audit Configuration |
	And I select the date in "nodeFilter" "finalDate" "date" based on below criteria
		| key           | value               |
		| dateSelection | final               |
		| daysLessThen  | 3                   |
		| page          | Audit Configuration |
	And I click on "nodeFilter" "Submit" "button"
	Then I should see "backToTransactionsAudit" "button"

@testcase=52360 @BVT2
Scenario: Verify Change filters button functionaliy of movement audit report
	When I navigate to "TransactionsAudit" page
	And I select any "SegmentValue" from "NodeFilter" "element" "dropdown"
	And I click on "Movimientos" RadioButton
	And I select the date in "nodeFilter" "initialDate" "date" based on below criteria
		| key           | value               |
		| dateSelection | initial             |
		| daysLessThen  | 4                   |
		| page          | Audit Configuration |
	And I select the date in "nodeFilter" "finalDate" "date" based on below criteria
		| key           | value               |
		| dateSelection | final               |
		| daysLessThen  | 3                   |
		| page          | Audit Configuration |
	And I click on "nodeFilter" "Submit" "button"
	And I click on "backToTransactionsAudit" "button"
	Then validate previously selected "initial" date in "nodeFilter" "initialDate" "date" on the filters page
	And validate previously selected "final" date in "nodeFilter" "finalDate" "date" on the filters page

@testcase=52361 @BVT2
Scenario: Verify Change filters button functionaliy of inventory audit report
	When I navigate to "TransactionsAudit" page
	When I select any "SegmentValue" from "NodeFilter" "element" "dropdown"
	And I click on "Inventarios" RadioButton
	And I select the date in "nodeFilter" "initialDate" "date" based on below criteria
		| key           | value               |
		| dateSelection | initial             |
		| daysLessThen  | 10                  |
		| page          | Audit Configuration |
	And I select the date in "nodeFilter" "finalDate" "date" based on below criteria
		| key           | value               |
		| dateSelection | final               |
		| daysLessThen  | 10                  |
		| page          | Audit Configuration |
	And I click on "nodeFilter" "Submit" "button"
	And I click on "backToTransactionsAudit" "button"
	Then validate previously selected "initial" date in "nodeFilter" "initialDate" "date" on the filters page
	And validate previously selected "final" date in "nodeFilter" "finalDate" "date" on the filters page

@testcase=52362 @manual
Scenario Outline: Verify user is able to select only date period not more than configured parameter
	When I navigate to "TransactionsAudit" page
	And I selected segment in the "segment" "dropdown"
	And I click on "<entity>" "radio"
	Then I am able to select only date period not more than configured parameter

	Examples:
		| entity      |
		| movements   |
		| inventories |