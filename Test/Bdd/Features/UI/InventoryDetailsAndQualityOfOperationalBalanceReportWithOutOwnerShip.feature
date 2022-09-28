@sharedsteps=16534 @owner=jagudelos @ui @testplan=14709 @testsuite=14731
Feature:InventoryDetailsAndQualityOfOperationalBalanceReportWithOutOwnerShip
In order to visualize the operating balance without ownership and traceability of the rules applied(Inventory Details and Quality)
As a Query User
I want to visualize  the Inventory Details and Quality in operating balance Report

Background: Login
	Given I am logged in as "consulta"

@testcase=16535 @manual
Scenario: Verify the inventory details in the operational Balance Report
	Given that I am in the operational balance report
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	And I right click on any cell in the report summary
	When I select the option to see the inventory detail​
	Then I should see the inventory details in a table view

@testcase=16536 @manual
Scenario: Verify the quality details of the inventory in the Operational Balance Report
	Given that I am in the operational balance report
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	And I right click on any cell in the report summary
	When I select the option to see the quality detail​ of the inventory
	Then I should see the quality details of the inventory in table view

@testcase=16537 @manual
Scenario: Verify the inventory details Sheet in a Table
	Given that I am in the operational balance report
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	When I select the option to see the inventory detail​
	Then I should see the "<Columns>" in the report file
		| Columns                       |
		| Identificación del inventario |
		| Fecha                         |
		| Nodo                          |
		| Tanque                        |
		| Producto                      |
		| Volumen Neto                  |
		| Unidad                        |
		| % Incertidumbre Estándar      |
		| Incertidumbre                 |

@testcase=16538 @manual
Scenario: Verify the quality detail of the inventories Sheet in a Table
	Given that I am in the operational balance report
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	When I select the option to see the quality detail​ of the inventory
	Then I should see the "<Columns>" in the report file
		| Columns                       |
		| Identificación del inventario |
		| Fecha                         |
		| Nodo                          |
		| Tanque                        |
		| Producto                      |
		| Volumen Neto                  |
		| Unidad                        |
		| Valor Atributo                |
		| Unidad Atributo               |
		| Descripción Atributo          |