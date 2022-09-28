@sharedsteps=7539 @owner=jagudelos @testplan=4938 @testsuite=5398
Feature: ManageConnectionBetweenNodes
In order to handle the connections
As an application administrator
I want to manage the connections between nodes

Background: Login
	Given I am authenticated as "admin"

@testcase=5777 @api @audit @output=QueryAll(GetNodeConnection) @bvt @prodready
Scenario: Create a connection between two nodes with valid data
	Given I want to create a "node-connection" in the system
	When I provide the required fields
	Then the response should succeed with message "Conexión creada con éxito"
	And "Create" should be registered in Audit-log

@testcase=5778 @api @bvt @output=QueryAll(GetNodeConnection) @prodready
Scenario Outline: Create a connection between two nodes when identifiers do not exist
	Given I want to create a "node-connection" in the system
	When I provide a "<Field>" do not exist
	Then the response should fail with message "<Message>"

	Examples:
		| Field             | Message                                      |
		| SourceNodeId      | Identificador del nodo origen no encontrado  |
		| DestinationNodeId | Identificador del nodo destino no encontrado |

@testcase=5779 @api @bvt @output=QueryAll(GetNodeConnection) @prodready
Scenario: Create a connection between two nodes when origin and destination node identifiers already exist
	Given I want to create a "node-connection" in the system
	When I provide "SourceNodeId" and "DestinationNodeId" already exist
	Then the response should fail with message "Ya existe una conexión entre los nodos"

@testcase=5780 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario Outline: Create a connection between two nodes without mandatory fields
	Given I want to create a "node-connection" in the system
	When I don't provide "<Field>"
	Then the response should fail with message "<Message>"

	Examples:
		| Field             | Message                                          |
		| SourceNodeId      | El identificador del nodo origen es obligatorio  |
		| DestinationNodeId | El identificador del nodo destino es obligatorio |

@testcase=5781 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario: Create a connection between two nodes without description
	Given I want to create a "node-connection" in the system
	When I don't provide "description"
	Then the response should succeed with message "Conexión creada con éxito"

@testcase=5782 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario: Create a connection between two nodes with Description that exceeds 300 characters
	Given I want to create a "node-connection" in the system
	When I provide "description" that exceeds 300 characters
	Then the response should fail with message "La descripción de la conexión del nodo puede tener máximo 300 caracteres"

@testcase=5783 @api @prodready
Scenario Outline: Create a connection between two nodes when source node identifier or destination node identifier contains characters other than numbers
	Given I want to create a "node-connection" in the system
	When I provide "<Field>" contains characters other than expected
	Then the response should fail with "BadRequest"

	Examples:
		| Field             |
		| SourceNodeId      |
		| DestinationNodeId |

@testcase=5784 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario: Create a connection between two nodes without any values
	Given I want to create a "node-connection" in the system
	When I don't provide any values
	Then the response should fail with messages
		| Message                                          |
		| NODECONNECTION_STATUS_REQUIREDVALIDATION         |
		| El identificador del nodo origen es obligatorio  |
		| El identificador del nodo destino es obligatorio |

@testcase=5785 @api @bvt @output=QueryAll(GetNodeConnection) @prodready
Scenario: Create a connection between two nodes when origin and destination node identifiers are same
	Given I want to create a "node-connection" in the system
	When I provide same "SourceNodeId" and "DestinationNodeId"
	Then the response should succeed with message "Conexión creada con éxito"

@testcase=5786 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario: Create a connection between two nodes that is inactive
	Given I want to create a "node-connection" in the system
	When I provide inactive connection details
	Then the response should succeed with message "Conexión creada con éxito"

@testcase=5787 @api @odata @bvt @output=QueryAll(GetActiveNodeConnection) @prodready
Scenario: Get all connections
	Given I have "node-connection" in the system
	When I Get all records
	Then the response should return all valid records

@testcase=5788 @api @bvt @output=QueryAll(GetNodeConnection) @prodready
Scenario: Get connection between two nodes by valid data
	Given I have "node-connection" in the system
	When I Get record with "SourceNodeId" and "DestinationNodeId"
	Then the response should return requested record details

@testcase=5789 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario Outline: Get connection between two nodes by invalid ids
	Given I have "node-connection" in the system
	When I Get record with invalid "<Field>"
	Then the response should fail with message "<Message>"

	Examples:
		| Field             | Message                                      |
		| SourceNodeId      | Identificador del nodo origen no encontrado  |
		| DestinationNodeId | Identificador del nodo destino no encontrado |

@testcase=5790 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario: Get connection between two nodes do not exist
	Given I have "node-connection" in the system
	When I Get record with "SourceNodeId" and "DestinationNodeId" do not exist
	Then the response should fail with message "No existe una conexión para los nodos origen y destino recibidos"

@testcase=5791 @api @bvt @output=QueryAll(GetNodeConnection) @prodready
Scenario: Get connection between two nodes when origin and destination node identifiers are same
	Given I have "node-connection" in the system
	When I Get record with same "SourceNodeId" and "DestinationNodeId"
	Then the response should return requested record details

@testcase=5792 @api @audit @bvt @output=QueryAll(GetNodeConnection) @prodready
Scenario: Update a connection between two nodes with valid data
	Given I have "node-connection" in the system
	When I update a record with required fields
	Then the response should succeed with message "Conexión actualizada con éxito"
	And "Update" should be registered in Audit-log

@testcase=5793 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario: Update a connection between two nodes without description
	Given I have "node-connection" in the system
	When I update a record without "description"
	Then the response should succeed with message "Conexión actualizada con éxito"

@testcase=5794 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario Outline: Update a connection between two nodes without mandatory fields
	Given I have "node-connection" in the system
	When I update a record without "<Field>"
	Then the response should fail with message "<Message>"

	Examples:
		| Field             | Message                                          |
		| SourceNodeId      | El identificador del nodo origen es obligatorio  |
		| DestinationNodeId | El identificador del nodo destino es obligatorio |

@testcase=5795 @api @output=QueryAll(GetNodeConnection) @prodready @prodready
Scenario: Update a connection between two nodes with Description exceeds 300 characters
	Given I have "node-connection" in the system
	When I update a record with "description" that exceeds 300 characters
	Then the response should fail with message "La descripción de la conexión del nodo puede tener máximo 300 caracteres"

@testcase=5796 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario Outline: Update a connection between two nodes when source node identifier or destination node identifier contains characters other than numbers
	Given I have "node-connection" in the system
	When I update a record with "<Field>" contains characters other than expected
	Then the response should fail with "BadRequest"

	Examples:
		| Field             |
		| SourceNodeId      |
		| DestinationNodeId |

@testcase=5797 @api @bvt @output=QueryAll(GetNodeConnection) @prodready
Scenario Outline: Update a connection between two nodes when identifiers do not exist
	Given I have "node-connection" in the system
	When I update a record with a "<Field>" do not exist
	Then the response should fail with message "No existe una conexión para los nodos origen y destino recibidos"

	Examples:
		| Field             |
		| SourceNodeId      |
		| DestinationNodeId |

@testcase=5798 @api @bvt @output=QueryAll(GetNodeConnection) @prodready
Scenario: Update a connection between two nodes when origin and destination node identifiers are same
	Given I have "node-connection" in the system
	When I update a record where "SourceNodeId" and "DestinationNodeId" are same
	Then the response should succeed with message "Conexión actualizada con éxito"

@testcase=5799 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario: Update a connection between two nodes to active
	Given I have inactive "node-connection" in the system
	When I update a connection with "isActive" as "True"
	Then the response should succeed with message "Conexión actualizada con éxito"

@testcase=5800 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario: Update a connection between two nodes to inactive
	Given I have "node-connection" in the system
	When I update a connection with "isActive" as "False"
	Then the response should succeed with message "Conexión actualizada con éxito"

@testcase=5801 @api @audit @bvt @output=QueryAll(GetNodeConnection) @prodready
Scenario: Delete a connection between two nodes without movements
	Given I have "node-connection" in the system
	When I delete a record with "no movements"
	Then the response should succeed with message "Conexión eliminada con éxito"
	And "Delete" should be registered in Audit-log

@testcase=5802 @api @bvt @output=QueryAll(GetNodeConnection)
Scenario: Delete a connection between two nodes with movements
	Given I have "node-connection" in the system
	When I delete a record with "movements"
	Then the response should fail with message "No es posible eliminar la conexión debido a que tiene movimientos asociados"

@testcase=5803 @api @bvt @output=QueryAll(GetNodeConnection) @prodready
Scenario: Delete a connection between two nodes do not exist
	Given I have "node-connection" in the system
	When I delete a record do not exist
	Then the response should fail with message "No existe una conexión para los nodos origen y destino recibidos"

@testcase=5804 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario: Delete a connection between two nodes when origin and destination node identifiers are same
	Given I have "node-connection" in the system
	When I delete a record where "SourceNodeId" and "DestinationNodeId" are same
	Then the response should succeed with message "Conexión eliminada con éxito"

@testcase=5805 @api @output=QueryAll(GetNodeConnection) @prodready
Scenario Outline: Delete a connection between two nodes by invalid ids
	Given I have "node-connection" in the system
	When I delete a record with invalid "<Field>"
	Then the response should fail with message "<Message>"

	Examples:
		| Field             | Message                                      |
		| SourceNodeId      | Identificador del nodo origen no encontrado  |
		| DestinationNodeId | Identificador del nodo destino no encontrado |