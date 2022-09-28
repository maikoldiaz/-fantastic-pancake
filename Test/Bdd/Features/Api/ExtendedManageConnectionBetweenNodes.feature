@sharedsteps=7539 @owner=jagudelos @api @testplan=6671 @testsuite=6699
Feature: ExtendedManageConnectionBetweenNodes
In order to handle the connections
As an application administrator
I want to manage the connections between nodes

Background: Login
	Given I am authenticated as "admin"

@testcase=7580 @api @audit @output=QueryAll(GetNodeConnection) @prodready
Scenario: Create a connection between two nodes with valid data
	Given I want to create a "node-connection" in the system
	When I provide the required fields
	Then the response should succeed with message "Conexión creada con éxito"
	And "Create" should be registered in Audit-log

@testcase=7581 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario Outline: Create a connection between two nodes without mandatory fields
	Given I want to create a "node-connection" in the system
	When I don't provide "<Field>"
	Then the response should fail with message "<Message>"

	Examples:
		| Field               | Message                                          |
		| SourceNodeId        | El identificador del nodo origen es obligatorio  |
		| DestinationNodeId   | El identificador del nodo destino es obligatorio |
		| ProductId           | El identificador del producto es obligatorio     |
		| OwnerId             | El identificador del propietario  es obligatorio |
		| OwnershipPercentage | El porcentaje de propiedad es obligatorio        |

@testcase=7582 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario Outline: Create a connection between two nodes without optional fields
	Given I want to create a "node-connection" in the system
	When I don't provide "<Field>"
	Then the response should succeed with message "Conexión creada con éxito"

	Examples:
		| Field                 |
		| Description           |
		| Connection Products   |
		| UncertaintyPercentage |
		| ControlLimit          |
		| Owners                |

@testcase=7583 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario: Create a connection between two nodes with sum of the owners percentages list equal to 100%
	Given I want to create a "node-connection" in the system
	When I provide the required fields when sum of the "OwnershipPercentage" list equal to 100
	Then the response should succeed with message "Conexión creada con éxito"

@testcase=7584 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario: Create a connection between two nodes with sum of the owners percentages list not equal to 100%
	Given I want to create a "node-connection" in the system
	When I provide the required fields when sum of the "OwnershipPercentage" list equal to 200
	Then the response should fail with message "La sumatoria de los valores de propiedad debe ser 100%"

@testcase=7585 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario: Create a connection between two nodes without any values
	Given I want to create a "node-connection" in the system
	When I don't provide any values
	Then the response should fail with messages
		| Message                                          |
		| NODECONNECTION_STATUS_REQUIREDVALIDATION         |
		| El identificador del nodo origen es obligatorio  |
		| El identificador del nodo destino es obligatorio |

@testcase=7586 @api @bvt @output=QueryAll(GetActiveNodeConnection) @prodready
Scenario: Get all connections
	Given I have "node-connection" in the system
	When I Get all records
	Then the response should return all valid records

@testcase=7587 @api @bvt @output=QueryAll(GetNodeConnection) @prodready
Scenario: Get connection between two nodes by valid data
	Given I have "node-connection" in the system
	When I Get record with "SourceNodeId" and "DestinationNodeId"
	Then the response should return requested record details

@testcase=7588 @api @audit @bvt @output=QueryAll(GetNodeConnection) @prodready
Scenario: Update a connection between two nodes with valid data
	Given I have "node-connection" in the system
	When I update a record with required fields
	Then the response should succeed with message "Conexión actualizada con éxito"
	And "Update" should be registered in Audit-log

@testcase=7589 @api @output=QueryAll(GetNodeConnection) @version=2 @prodready
Scenario: Update a connection between two nodes without description
	Given I have "node-connection" in the system
	When I update a record without "Description"
	Then the response should succeed with message "Conexión actualizada con éxito"

@testcase=7590 @api @output=QueryAll(GetNodeConnection)
Scenario Outline: Update a connection between two nodes without mandatory fields
	Given I have "node-connection" in the system
	When I update a record without "<Field>"
	Then the response should fail with message "<Message>"

	Examples:
		| Field               | Message                                          |
		| SourceNodeId        | El identificador del nodo origen es obligatorio  |
		| DestinationNodeId   | El identificador del nodo destino es obligatorio |
		| ProductId           | El identificador del producto es obligatorio     |
		| OwnerId             | El identificador del propietario  es obligatorio |
		| OwnershipPercentage | El porcentaje de propiedad es obligatorio        |

@testcase=7591 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario: Update a connection between two nodes with sum of the owners percentages list equal to 100%
	Given I have "node-connection" in the system
	When I update a record when sum of the "OwnershipPercentage" list equal to 100
	Then the response should succeed with message "Conexión actualizada con éxito"

@testcase=7592 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario: Update a connection between two nodes with sum of the owners percentages list not equal to 100%
	Given I have "node-connection" in the system
	When I update a record when sum of the "OwnershipPercentage" list equal to 200
	Then the response should fail with message "La sumatoria de los valores de propiedad debe ser 100%"

@testcase=7593 @api @audit @output=QueryAll(GetNodeConnection) @ignore
Scenario: Update a products in the connection
	Given I have "node-connection" with 4 products in the system
	When I update a connection to add one product
	Then the response should succeed with message "Conexión actualizada con éxito"
	And Validate 5 "Products" are created
	When I update a connection to delete one product
	Then the response should succeed with message "Conexión actualizada con éxito"
	And Validate 4 "Products" are active