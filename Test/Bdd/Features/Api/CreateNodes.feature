@sharedsteps=7539 @owner=jagudelos @api @testplan=3993 @testsuite=4024
Feature: CreateNodes
In order to handle the Transport Network
As an application administrator
I want to create Transport Nodes

Background: Login
	Given I am authenticated as "admin"

@testcase=4494 @api @output=QueryAll(GetNodes) @bvt @version=3 @prodready
Scenario: Create Node with valid data
	Given I want to create a "Node" in the system
	When I provide the required fields
	Then the response should succeed with message "Nodo creado con éxito"

@testcase=4495 @api @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Create Node without Node Name
	Given I want to create a "Node" in the system
	When I don't provide "Name"
	Then the response should fail with message "El Nombre es obligatorio"

@testcase=4496 @api @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Create Node with Node Name that exceeds 150 characters
	Given I want to create a "Node" in the system
	When I provide "Name" that exceeds 150 characters
	Then the response should fail with message "El nombre puede contener máximo 150 caracteres"

@testcase=4497 @api @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Create Node with existing Node Name
	Given I want to create a "Node" in the system
	When I provide an existing "Name"
	Then the response should fail with message "El nombre del nodo ya existe"

@testcase=4498 @api @output=QueryAll(GetNodes) @version=5 @prodready
Scenario: Create Node with Node Name that contains special characters other than ":", "_", "-"
	Given I want to create a "Node" in the system
	When I provide "Name" that contains special characters other than expected
	Then the response should fail with message "El nombre del nodo solo admite números, letras, espacios y los siguientes caracteres especiales “:”, “-“, “_”.”"

@testcase=4499 @api @output=QueryAll(GetNodes) @version=6 @prodready
Scenario: Create Node with Node Description that exceeds 1000 characters
	Given I want to create a "Node" in the system
	When I provide "Description" that exceeds 1000 characters
	Then the response should fail with message "La descripción del nodo puede contener máximo 1000 caracteres"

@testcase=4500 @api @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Create Node without Node Type Id
	Given I want to create a "Node" in the system
	When I don't provide "NodeTypeId"
	Then the response should fail with message "El Tipo de Nodo es obligatorio"

@testcase=4501 @api @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Create Node without Segment Id
	Given I want to create a "Node" in the system
	When I don't provide "SegmentId"
	Then the response should fail with message "El Segmento es obligatorio"

@testcase=4502 @api @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Create Node without Operator Id
	Given I want to create a "Node" in the system
	When I don't provide "OperatorId"
	Then the response should fail with message "El Operador es obligatorio"

@testcase=4503 @api @output=QueryAll(GetNodes) @version=4 @prodready
Scenario: Create Node with sendToSap as true and without Logistic Center Id
	Given I want to create a "Node" in the system
	When I provide "SendToSap" value as "true" and without "LogisticCenterId"
	Then the response should fail with message "El código SAP es obligatorio"

@testcase=4504 @api @output=QueryAll(GetNodes) @version=5 @prodready
Scenario: Create Node without Storage Location Name
	Given I want to create a "Node" in the system
	When I don't provide "StorageLocation Name"
	Then the response should fail with message "El Nombre es obligatorio"

@testcase=4505 @api @output=QueryAll(GetNodes) @version=4 @prodready
Scenario: Create Node with Storage Location Name that exceeds 150 characters
	Given I want to create a "Node" in the system
	When I provide "StorageLocation Name" that exceeds 150 characters
	Then the response should fail with message "El Nombre del almacén puede contener máximo 150 caracteres"

@testcase=4506 @api @output=QueryAll(GetNodes) @version=6 @prodready
Scenario: Create Node with Storage Location Name that contains special characters other than ":", "_", "-"
	Given I want to create a "Node" in the system
	When I provide "StorageLocation Name" that contains special characters other than expected
	Then the response should fail with message "El Nombre del almacén solo admite números, letras, espacios y los siguientes caracteres especiales: “:”, “_”, “-”"

@testcase=4507 @api @output=QueryAll(GetNodes) @version=4 @prodready
Scenario: Create Node with Storage Location Description that exceeds 1000 characters
	Given I want to create a "Node" in the system
	When I provide "StorageLocation Description" that exceeds 1000 characters
	Then the response should fail with message "La Descripción del almacén puede contener máximo 1000 caracteres"

@testcase=4508 @api @output=QueryAll(GetNodes) @version=6 @prodready
Scenario: Create Node without Storage Location Type id
	Given I want to create a "Node" in the system
	When I don't provide "StorageLocation StorageLocationTypeId"
	Then the response should fail with message "El Tipo es obligatorio"

@testcase=4509 @api @output=QueryAll(GetNodes) @version=7 @ignore
Scenario: Create Node with Storage Location sendToSap as true and without Storage Location Id
	Given I want to create a "Node" in the system
	When I provide "StorageLocation SendToSap" value as "true" and without "StorageLocation StorageLocationId"
	Then the response should fail with message "El código SAP es obligatorio"

@testcase=4510 @api @output=QueryAll(GetNodes) @version=6 @prodready
Scenario: Create Node without any Storage Location assigned
	Given I want to create a "Node" in the system
	When I don't provide any "NodeStorageLocations"
	Then the response should fail with message "Un nodo debe tener por lo menos un almacén"

@testcase=4511 @api @output=QueryAll(GetNodes) @version=6 @ignore
Scenario: Create Node without any Product assigned to Storage Location
	Given I want to create a "Node" in the system
	When I don't provide any "Products"
	Then the response should fail with message "Un almacén debe tener por lo menos un producto asignado"

@testcase=4512 @api @output=QueryAll(GetLogisticCenters) @bvt @version=3
Scenario: Get All Logistic Centers
	Given I have "Logistic Centers" in the system
	When I Get all records
	Then the response should return all valid records

@testcase=4513 @api @output=QueryAll(GetStorageLocations) @bvt @version=3 @prodready
Scenario: Get All Storage Locations
	Given I have "Storage Locations" in the system
	When I Get all records
	Then the response should return all valid records

@testcase=4514 @api @output=QueryAll(GetNodes) @version=4 @prodready
Scenario: Verify the existence of Storage Location Name
	Given I have "Nodes" in the system
	When I verify the existence of "StorageLocation Name"
	Then the response should be successful