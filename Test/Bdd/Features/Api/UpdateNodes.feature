@sharedsteps=7539 @owner=jagudelos @api @testplan=3993 @testsuite=4026
Feature: UpdateNodes
In order to handle the Transport Network
As an application administrator
I want to update Transport Nodes

Background: Login
	Given I am authenticated as "admin"

@testcase=4563 @api @output=QueryAll(GetNodes) @bvt @version=5 @prodready
Scenario: Update Node with valid data
	Given I have "Node" in the system
	When I update a record with required fields
	Then the response should succeed with message "Nodo actualizado con éxito"

@testcase=4564 @api @output=QueryAll(GetNodes) @version=6 @prodready
Scenario: Update Node without Node Name
	Given I have "Node" in the system
	When I update a record without "Name"
	Then the response should fail with message "El Nombre es obligatorio"

@testcase=4565 @api  @output=QueryAll(GetNodes) @version=5 @prodready
Scenario: Update Node with Node Name that exceeds 150 characters
	Given I have "Node" in the system
	When I update a record with "Name" that exceeds 150 characters
	Then the response should fail with message "El nombre puede contener máximo 150 caracteres"

@testcase=4566 @api @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Update Node with existing Node Name
	Given I have "Nodes" in the system
	When I update a record with existing "Name"
	Then the response should fail with message "El nombre del nodo ya existe"

@testcase=4567 @api @output=QueryAll(GetNodes) @version=7 @prodready
Scenario: Update Node with Node Name that contains special characters other than ":", "_", "-"
	Given I have "Node" in the system
	When I update a record with "Name" that contains special characters other than expected
	Then the response should fail with message "El nombre del nodo solo admite números, letras, espacios y los siguientes caracteres especiales “:”, “-“, “_”.”"

@testcase=4568 @api  @output=QueryAll(GetNodes) @version=5 @prodready
Scenario: Update Node with Node Description that exceeds 1000 characters
	Given I have "Node" in the system
	When I update a record with "Description" that exceeds 1000 characters
	Then the response should fail with message "La descripción del nodo puede contener máximo 1000 caracteres"

@testcase=4569 @api @output=QueryAll(GetNodes) @version=5 @ignore
Scenario: Update Node without Node Type Id
	Given I have "Node" in the system
	When I update a record without "NodeTypeId"
	Then the response should fail with message "El Tipo de Nodo es obligatorio"

@testcase=4570 @api @output=QueryAll(GetNodes) @version=6 @ignore
Scenario: Update Node without Segment Id
	Given I have "Node" in the system
	When I update a record without "SegmentId"
	Then the response should fail with message "El Segmento es obligatorio"

@testcase=4571 @api @output=QueryAll(GetNodes) @version=7 @ignore
Scenario: Update Node without Operator Id
	Given I have "Node" in the system
	When I update a record without "OperatorId"
	Then the response should fail with message "El Operador es obligatorio"

@testcase=4572 @api @output=QueryAll(GetNodes) @version=4 @prodready
Scenario: Update Node with sendToSap as true and without Logistic Center Id
	Given I have "Node" in the system
	When I update "SendToSap" as "true" and without "LogisticCenterId"
	Then the response should fail with message "El código SAP es obligatorio"

@testcase=4573 @api @output=QueryAll(GetNodes) @version=5 @prodready
Scenario: Update Node without Storage Location Name
	Given I have "Node" in the system
	When I update a record without "StorageLocation Name"
	Then the response should fail with message "El Nombre es obligatorio"

@testcase=4574 @api @output=QueryAll(GetNodes) @version=6 @prodready
Scenario: Update Node with Storage Location Name that exceeds 150 characters
	Given I have "Node" in the system
	When I update a record with "StorageLocation Name" that exceeds 150 characters
	Then the response should fail with message "El Nombre del almacén puede contener máximo 150 caracteres"

@testcase=4575 @api @output=QueryAll(GetNodes) @version=7 @prodready
Scenario: Update Node with Storage Location Name that contains special characters other than ":", "_", "-"
	Given I have "Nodes" in the system
	When I update a record with "StorageLocation Name" that contains special characters other than expected
	Then the response should fail with message "El Nombre del almacén solo admite números, letras, espacios y los siguientes caracteres especiales: “:”, “_”, “-”"

@testcase=4576 @api @output=QueryAll(GetNodes) @version=5 @prodready
Scenario: Update Node with Storage Location Description that exceeds 1000 characters
	Given I have "Node" in the system
	When I update a record with "StorageLocation Description" that exceeds 1000 characters
	Then the response should fail with message "La Descripción del almacén puede contener máximo 1000 caracteres"

@testcase=4577 @api @output=QueryAll(GetNodes) @version=5 @prodready
Scenario: Update Node without Storage Location Type id
	Given I have "Node" in the system
	When I update a record without "StorageLocation StorageLocationTypeId"
	Then the response should fail with message "El Tipo es obligatorio"

@testcase=4578 @api @output=QueryAll(GetNodes) @version=6 @prodready
Scenario: Update Node with Storage Location sendToSap as true and without Storage Location Id
	Given I have "Node" in the system
	When I update a record with "StorageLocation SendToSap" as "true" and without "StorageLocation StorageLocationId"
	Then the response should fail with message "El código SAP es obligatorio"

@testcase=4579 @api @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Update Node without any Storage Location assigned
	Given I have "Node" in the system
	When I update a record without "NodeStorageLocations"
	Then the response should fail with message "Un nodo debe tener por lo menos un almacén"

@testcase=4580 @api @output=QueryAll(GetNodes) @version=5 @prodready
Scenario: Update Node without any Product assigned to Storage Location
	Given I have "Node" in the system
	When I update a record without "Products"
	Then the response should fail with message "Un almacén debe tener por lo menos un producto asignado"

@testcase=4581 @api @output=QueryAll(GetNodes) @audit @version=5 @prodready
Scenario: Update Storage Location data
	Given I have "Node" in the system
	When I update "StorageLocation Name" from the existing "Storage Locations"
	Then the field of "StorageLocation Name" must be updated for respective "Storage Locations"
	And "Update" to the field "Name" should be registered in audit log

@testcase=4582 @api @output=QueryAll(GetNodes) @audit @version=5 @prodready
Scenario: Register new Storage Location
	Given I have "Node" in the system
	When I update a record with new "StorageLocation Name"
	Then the field of "NewStorageLocation Name" must be updated for respective "Storage Locations"
	And "Insert" to the field "Name" should be registered in audit log

@testcase=4583 @api @output=QueryAll(GetNodes) @audit @version=9
Scenario: Update Active field of Storage Location if it is deleted
	Given I have "Node" in the system
	When I update "StorageLocation IsActive" as "false" from the list of existing "Storage Locations"
	Then the active field of the "StorageLocationStatus" must be updated to "false" for respective "Storage Locations"
	And "Update" to the field "IsActive" as "false" should be registered in audit log for respective "Storage Locations"

@testcase=4584 @api @output=QueryAll(GetNodes) @audit @version=7 @prodready
Scenario: Update Active field of Product if it is deleted
	Given I have "Node" in the system
	When I update "ProductLocation IsActive" as "false" from the list of existing "Product Locations"
	Then the active field of the "ProductLocationStatus" must be updated to "false" for respective "Product Locations"
	And "Update" to the field "IsActive" as "false" should be registered in audit log for respective "Product Locations"

@testcase=4585 @api @output=QueryAll(GetNodes) @audit @version=4 @prodready
Scenario: Node data update
	Given I have "Node" in the system
	When the Node data has been modified
	Then Node data should be updated
	And "Update" to the "Node" should be registered in audit log