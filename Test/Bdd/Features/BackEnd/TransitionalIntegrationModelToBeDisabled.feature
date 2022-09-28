@owner=jagudelos @testplan=31102 @testsuite=31106
Feature: TransitionalIntegrationModelToBeDisabled
In order to register messages into TRUE either directly from MQ or through integration APIs with SAP PO
As a product owner
I need the transitional integration model with SINOPER to be disabled

@testcase=33853 @backend @version=2
Scenario Outline: Disable logic app and register a movement and an inventory from SINOPER
	Given I am authenticated as "admin"
	When I have "logic app" disabled
	And I register "<Entity>" in the system through SINOPER
	And validate "<Entity>" not registered through SINOPER
	And I register "<Entity>" in the system through SAP PO
	Then validate "<Entity>" registered through SAP PO

	Examples:
		| Entity    |
		| Movements |
		| Inventory |

@testcase=33854 @backend @version=2
Scenario Outline: Disable homologation and register a movement and an inventory from SINOPER
	Given I am authenticated as "admin"
	When I have "homologation" disabled
	And I register "<Entity>" in the system through SINOPER
	And validate "<Entity>" not registered through SINOPER
	And I register "<EntityType>" in the system through SAP PO
	Then validate "<Entity>" registered through SAP PO

	Examples:
		| Entity    | EntityType             |
		| Movements | MovementsHomologated   |
		| Inventory | InventoriesHomologated |

@testcase=33855 @backend @version=2
Scenario Outline: Disable homologation and logic app then register a movement and an inventory from SINOPER
	Given I am authenticated as "admin"
	When I have 'logic app' and 'homologation' disabled
	And I register "<Entity>" in the system through SINOPER
	And validate "<Entity>" not registered through SINOPER
	And I register "<EntityType>" in the system through SAP PO
	Then validate "<Entity>" registered through SAP PO

	Examples:
		| Entity    | EntityType             |
		| Movements | MovementsHomologated   |
		| Inventory | InventoriesHomologated |

@testcase=33856 @backend @version=2
Scenario Outline: Enable logic app and homologation then register an inventory and a movement from SINOPER
	Given I am authenticated as "admin"
	When I have "logic app" and "homologation" enabled
	And I register "<Entity>" in the system through SINOPER
	And validate "<Entity>" registered through SINOPER
	And I register "<Entity>" in the system through SAP PO
	Then validate "<Entity>" registered through SAP PO

	Examples:
		| Entity    |
		| Movements |
		| Inventory |

@testcase=33857 @ui
Scenario Outline: Register an inventory and a movement from EXCEL
	Given I have 'Homologation' in the system to register '<Entity>'
	When I receive "<Field>" of the current month
	Then it should be registered in the system

	Examples:
		| Entity    | Field         |
		| Movements | OperationDate |
		| Inventory | InventoryDate |