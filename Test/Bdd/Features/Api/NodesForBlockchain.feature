Feature: NodesForBlockchain
	As an Admin for True System
	Creating Nodes for BlockChain

Background: Login
	Given I am authenticated as "admin"

@api @output=QueryAll(GetNodes)
Scenario: Update Node with valid data
	Given I have "Node" in the system
	When I update a record with required fields
	Then the response should succeed with message "Nodo actualizado con éxito"

@api @audit @output=QueryAll(GetNodeConnection)
Scenario: Create a connection between two nodes with valid data
	Given I want to create a "node-connection" in the system
	When I provide the required fields
	Then the response should succeed with message "Conexión creada con éxito"

@api @bvt @output=QueryAll(GetNodeConnection)
Scenario: Update a connection between two nodes with valid data
	Given I have "node-connection" in the system
	When I update a record with required fields
	Then the response should succeed with message "Conexión actualizada con éxito"