@sharedsteps=4013 @owner=jagudelos @ui @testsuite=70800 @testplan=70526 @S16
Feature: ValidateOfficialManualInventoryFunctionality
In the TRUE system
As a Programador user
I should be able to generate report for official balance by node

Background: Login
Given I am logged in as "programador"
@testcase=72660
Scenario: Validate that the programador user has access to only the following pages in TRUE system
Then I validate the programador user has access to only following pages
| Module                          | SubModule                                      |
| Balance de ductos y estaciones  | Cargue de compras, ventas y eventos PPA        |
| Balance de ductos y estaciones  | Balance operativo con propiedad por nodo       |
| Reportes de ductos y estaciones | Configuración de compras, ventas y eventos PPA |
| Reportes de ductos y estaciones | Balance operativo con o sin propiedad          |
| Reportes de ductos y estaciones | Reportes generados                             |
| Gestión cadena de suministro    | Balance operativo con o sin propiedad          |
| Gestión cadena de suministro    | Reporte balance operativo                      |
| Gestión cadena de suministro    | Reportes generados                             |
@testcase=72661
Scenario: Validate that a new consolidated inventory is not created when a consolidated inventory already exists
Given the TRUE System is processing the operative inventories consolidation
And the consolidated inventory belongs to a NON-SON segment
And the inventory date is equal to the start date of the period minus one day
When there exists a consolidated record for the node, product and inventory date
Then I validate that another consolidated inventory is not created
@testcase=72662
Scenario: Validate that a new consolidated inventory is created when a consolidated inventory does not exists
Given the TRUE System is processing the operative inventories consolidation
And the consolidated inventory belongs to a NON-SON segment
And the inventory date is equal to the start date of the period minus one day
When there does not exists a consolidated record for the node, product and inventory date
Then I validate that another consolidated inventory is created
@testcase=72663
Scenario: Validate the Positive initial inventory Delta
Given the TRUE System is generating the report of the official balance by node
And a manual movement of the official scenario with 'ManualInvOficial' as source system exists in the system
And the manual movement has an associated official delta ticket
And the operational date is equal to the start date of the period minus one day
And the manual movement has only destination node
When the node is equal to the node selected to generate the report
Then I validate that the sum is performed on net quantity of the movement in the variable 'Delta inv. inicial'
And I validate that the movement is shown in the 'Detalle de movimientos' sheet
And I validate that 'Delta Inv. Inicial' value is shown under the 'Movement' column
@testcase=72664
Scenario: Validate the Negative initial inventory Delta
Given the TRUE System is generating the report of the official balance by node
And a manual movement of the official scenario with 'ManualInvOficial' as source system exists in the system
And the manual movement has an associated official delta ticket
And the operational date is equal to the start date of the period minus one day
And the manual movement has only source node
When the node is equal to the node selected to generate the report
Then I validate that the net quantity of the movement is multiplied by '-1', sum is performed on the result and assigned to variable 'Delta inv. inicial'
And I validate that the movement is shown in the 'Detalle de movimientos' sheet
And I validate that 'Delta Inv. Inicial' value is shown under the 'Movement' column
@testcase=72665
Scenario: Validate the Positive final inventory Delta
Given the TRUE System is generating the report of the official balance by node
And a manual movement of the official scenario with 'ManualInvOficial' as source system exists in the system
And the manual movement has an associated official delta ticket
And the operational date is equal to the end date of the period
And the manual movement has only destination node
When the node is equal to the node selected to generate the report
Then I validate that the sum is performed on net quantity of the movement in the variable 'Delta inv. final'
And I validate that the movement is shown in the 'Detalle de movimientos' sheet
And I validate that 'Delta Inv. Final' value is shown under the 'Movement' column
@testcase=72666
Scenario: Validate the Negative final inventory Delta
Given the TRUE System is generating the report of the official balance by node
And a manual movement of the official scenario with 'ManualInvOficial' as source system exists in the system
And the manual movement has an associated official delta ticket
And the operational date is equal to the end date of the period
And the manual movement has only source node
When the node is equal to the node selected to generate the report
Then I validate that the net quantity of the movement is multiplied by '-1', sum is performed on the result and assigned to variable 'Delta inv. final'
And I validate that the movement is shown in the 'Detalle de movimientos' sheet
And I validate that 'Delta Inv. Final' value is shown under the 'Movement' column
@testcase=72667
Scenario: Validate the Input delta
Given the TRUE System is generating the report of the official balance by node
And a manual movement of the official scenario with 'ManualMovOficial' as source system exists in the system
And the manual movement has an associated official delta ticket
And the start date is equal to the start date of the period
And the end date is equal to the end date of the period
When the destination node is equal to the node selected to generate the report
Then I validate that the sum is performed on net quantity of the movement in the variable 'Delta entradas'
And I validate that the movement is shown in the 'Detalle de movimientos' sheet
And I validate that 'Delta Entradas' value is shown under the 'Movement' column
@testcase=72668
Scenario: Validate the Output delta
Given the TRUE System is generating the report of the official balance by node
And a manual movement of the official scenario with 'ManualMovOficial' as source system exists in the system
And the manual movement has an associated official delta ticket
And the start date is equal to the start date of the period
And the end date is equal to the end date of the period
When the source node is equal to the node selected to generate the report
Then I validate that the sum is performed on net quantity of the movement in the variable 'Delta salidas'
And I validate that the movement is shown in the 'Detalle de movimientos' sheet
And I validate that 'Delta Salidas' value is shown under the 'Movement' column
@testcase=72669
Scenario: Validate the movements originated by manual deltas for ManualInvOficial as source system and where source node is equal to the node selected to generate the report
Given the TRUE System is generating the initial official balance report
And a movement of the official scenario with 'ManualInvOficial' as source system exists in the system
And the source node is equal to the node selected to generate the report
And the start date is equal to the start date of the period
And the end date is equal to the end date of the period
When the origin of the movements is a manual delta of an inventory
Then I validate that movements are excluded from input and output calculations of the balance
And I validate that movements are excluded from the movement detail sheet
And I validate that movements are excluded from the movement attributes sheet
@testcase=72670
Scenario: Validate the movements originated by manual deltas for ManualInvOficial as source system and where destination node is equal to the node selected to generate the report
Given the TRUE System is generating the initial official balance report
And a movement of the official scenario with 'ManualInvOficial' as source system exists in the system
And the destination node is equal to the node selected to generate the report
And the start date is equal to the start date of the period
And the end date is equal to the end date of the period
When the origin of the movements is a manual delta of an inventory
Then I validate that movements are excluded from input and output calculations of the balance
And I validate that movements are excluded from the movement detail sheet
And I validate that movements are excluded from the movement attributes sheet
@testcase=72671
Scenario: Validate the movements originated by manual deltas for ManualMovOficial as source system and where source node is equal to the node selected to generate the report
Given the TRUE System is generating the initial official balance report
And a movement of the official scenario with 'ManualMovOficial' as source system exists in the system
And the source node is equal to the node selected to generate the report
And the start date is equal to the start date of the period
And the end date is equal to the end date of the period
When the origin of the movements is a manual delta of a movement
Then I validate that movements are excluded from input and output calculations of the balance
And I validate that movements are excluded from the movement detail sheet
And I validate that movements are excluded from the movement attributes sheet
@testcase=72672 
Scenario: Validate the movements originated by manual deltas for ManualMovOficial as source system and where destination node is equal to the node selected to generate the report
Given the TRUE System is generating the initial official balance report
And a movement of the official scenario with 'ManualMovOficial' as source system exists in the system
And the destination node is equal to the node selected to generate the report
And the start date is equal to the start date of the period
And the end date is equal to the end date of the period
When the origin of the movements is a manual delta of a movement
Then I validate that movements are excluded from input and output calculations of the balance
And I validate that movements are excluded from the movement detail sheet
And I validate that movements are excluded from the movement attributes sheet
