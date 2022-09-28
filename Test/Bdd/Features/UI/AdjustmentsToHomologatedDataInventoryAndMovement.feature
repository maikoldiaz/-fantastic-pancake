@owner=jagudelos @testplan=11317 @testsuite=11327 @ui
Feature: AdjustmentsToHomologatedDataInventoryAndMovement
In order to register an Inventory and Movement in the system
As a TRUE system
I want to homologate the data

@testcase=12117 @version=1
Scenario: Register an Inventory with Inventory Date less than CurrentDate
	Given I want to register an "Inventory" in the system using Excel
	When I receive the data​ with "InventoryDate" less than Current date
	Then "Inventory" should be registered

@testcase=12118 @version=1
Scenario: Register an Inventory with Inventory Date greater than CurrentDate minus the number of valid days
	Given I want to register an "Inventory" in the system using Excel
	When I receive the data​ with "InventoryDate" greater than current date minus the number of valid days
	Then "Inventory" should be registered

@testcase=12119 @version=1
Scenario: Register an Inventory with Inventory Date is equal to CurrentDate minus the number of valid days
	Given I want to register an "Inventory" in the system using Excel
	When I receive the data​ with "InventoryDate" is equal to current date minus the number of valid days
	Then "Inventory" should be registered

@testcase=12120 @version=1
Scenario: Register an Inventory with Inventory Date greater than CurrentDate
	Given I want to register an "Inventory" in the system using Excel
	When I receive the data​ with "InventoryDate" greater than the CurrentDate
	Then "Inventory" must be stored in a Pendingtransactions repository with validation "La fecha del inventario debe estar entre {Current date - number of valid days} y {current date}"

@testcase=12121 @version=1
Scenario: Register an Inventory with Inventory Date less than the current date minus the number of valid days
	Given I want to register an "Inventory" in the system using Excel
	When I receive the data​ with "InventoryDate" less than the current date minus the number of valid days
	Then "Inventory" must be stored in a Pendingtransactions repository with validation "La fecha del inventario debe estar entre {Current date - number of valid days} y {current date}"

@testcase=12122 @version=1
Scenario: Register a Movement with Operational Date less than CurrentDate
	Given I want to register an "Movement" in the system using Excel
	When I receive the data​ with "OperationalDate" less than Current date
	Then "Movement" should be registered

@testcase=12123 @version=1
Scenario: Register a Movement with Operational Date greater than CurrentDate minus the number of valid days
	Given I want to register an "Movement" in the system using Excel
	When I receive the data​ with "OperationalDate" greater than current date minus the number of valid days
	Then "Movement" should be registered

@testcase=12124 @version=1
Scenario: Register a Movement with Operational Date is equal to CurrentDate minus the number of valid days
	Given I want to register an "Movement" in the system using Excel
	When I receive the data​ with "OperationalDate" is equal to current date minus the number of valid days
	Then "Movement" should be registered

@testcase=12125 @version=1
Scenario: Register a Movement with Operational Date greater than CurrentDate
	Given I want to register an "Movement" in the system using Excel
	When I receive the data​ with "OperationalDate" greater than the CurrentDate
	Then "Movement" must be stored in a Pendingtransactions repository with validation "La fecha operativa del movimiento debe estar entre {Current date - number of valid days} y {current date}"

@testcase=12126 @version=1
Scenario: Register a Movement with Operational Date less than the current date minus the number of valid days
	Given I want to register an "Movement" in the system using Excel
	When I receive the data​ with "OperationalDate" less than the current date minus the number of valid days
	Then "Movement" must be stored in a Pendingtransactions repository with validation "La fecha operativa del movimiento debe estar entre {Current date - number of valid days} y {current date}"