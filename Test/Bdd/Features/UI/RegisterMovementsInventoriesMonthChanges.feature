@owner=jagudelos @testplan=26817 @testsuite=26830
Feature: RegisterMovementsInventoriesMonthChanges
In order to register inventory and movement
As a true system
I need to update the validation of dates to accept records from previous days when the month changes

@testcase=28234 @ui
Scenario: Register an inventory of the previous month in the first days of the current month
Given I have 'Homologation' in the system to register 'Inventory'
When I receive "InventoryDate" of the previous month in the first days of the current month
Then it should be registered in the system

@testcase=28235 @ui
Scenario: Register an inventory of the previous month not in the first days of the current month
Given I have 'Homologation' in the system to register 'Inventory'
When I receive "InventoryDate" of the previous month not in the first days of the current month
Then "Inventory" must be stored in a Pendingtransactions repository with error message "Solo se permite el registro de información operativa del mes anterior durante los 4 primeros días del mes"

@testcase=28236 @ui
Scenario: Register a movement of the previous month in the first days of the current month
Given I have 'Homologation' in the system to register 'Movement'
When I receive "OperationDate" of the previous month in the first days of the current month
Then it should be registered in the system

@testcase=28237 @ui
Scenario: Register a movement of the previous month not in the first days of the current month
Given I have 'Homologation' in the system to register 'Movement'
When I receive "OperationDate" of the previous month not in the first days of the current month
Then "Movement" must be stored in a Pendingtransactions repository with error message "Solo se permite el registro de información operativa del mes anterior durante los 4 primeros días del mes"

@testcase=28238 @ui
Scenario: Register an inventory of the current month
Given I have 'Homologation' in the system to register 'Inventory'
When I receive "InventoryDate" of the current month
Then it should be registered in the system

@testcase=28239 @ui
Scenario: Register a movement of the current month
Given I have 'Homologation' in the system to register 'Movement'
When I receive "OperationDate" of the current month
Then it should be registered in the system

@testcase=28240 @ui
Scenario: Register an inventory of the todays date
Given I have 'Homologation' in the system to register 'Inventory'
When I receive "InventoryDate" of the todays date
Then "Movement" must be stored in a Pendingtransactions repository with error message "FUTURE_DATE_ERRORMESSAGE"

@testcase=28241 @ui
Scenario: Register a movement of the todays date
Given I have 'Homologation' in the system to register 'Movement'
When I receive "OperationDate" of the todays date
Then "Movement" must be stored in a Pendingtransactions repository with error message "FUTURE_DATE_ERRORMESSAGE"

@testcase=29532 @ui
Scenario: Register an inventory of the invalid previous month in the first days of the current month
Given I have 'Homologation' in the system to register 'Inventory'
When I receive "InventoryDate" of the invalid previous month in the first days of the current month
Then "Inventory" must be stored in a Pendingtransactions repository with validation "La fecha del inventario debe estar entre {previous month 1st valid date} y {previous month last date}"

@testcase=29533 @ui
Scenario: Register a movement of the invalid previous month in the first days of the current month
Given I have 'Homologation' in the system to register 'Movement'
When I receive "OperationDate" of the invalid previous month in the first days of the current month
Then "Movement" must be stored in a Pendingtransactions repository with validation "La fecha del inventario debe estar entre {previous month 1st valid date} y {previous month last date}"
