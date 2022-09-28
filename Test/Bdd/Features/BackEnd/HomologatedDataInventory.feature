@sharedsteps=7539 @owner=jagudelos @testplan=4938 @testsuite=5401 @backend @version=2
Feature: HomologatedDataInventory
In order to register an Inventory in the system
As a TRUE system
I want to homologate the data

Background: Login
	Given I am authenticated as "admin"

@testcase=5810 @backend @version=2
Scenario Outline: Register a Inventory without Mandatory Fields
	Given I want to register an "Inventory" in the system
	When I don't receive "<Field>" in XML
	Then it must be stored in a Pendingtransactions repository with validation "<Message>"

	Examples:
		| Field              | Message                                              |
		| SourceSystem       | El identificador del sistema origen es obligatorio   |
		| EventType          | El tipo de evento es obligatorio                     |
		| InventoryId        | El identificador del inventario es obligatorio       |
		| InventoryDate      | La fecha del inventario es obligatoria               |
		| ProductId          | El identificador del producto es obligatorio         |
		| ProductType        | El identificador del tipo de producto es obligatorio |
		| ProductVolume      | El volumen es obligatorio                            |
		| NodeId             | El identificador del nodo es obligatorio             |
		| Products           | Los productos del inventario son  obligatorios       |
		| AttributeId        | El identificador del atributo es obligatorio         |
		| AttributeValue     | El valor del atributo es obligatorio                 |
		| ValueAttributeUnit | La unidad de medida del atributo es obligatoria      |
		| OwnerId            | El identificador del propietario  es obligatorio     |
		| OwnershipValue     | El valor de la propiedad es obligatorio              |
		| OwnershipValueUnit | La unidad del valor de la propiedad es obligatoria   |

@testcase=5811 @backend @version=2
Scenario Outline: Register a Inventory without Optional Fields
	Given I want to register an "Inventory" in the system
	When I don't receive "<Field>" in XML
	Then it should be registered

	Examples:
		| Field                |
		| MeasurementUnit      |
		| Attributes           |
		| Owners               |
		| Observations         |
		| AttributeDescription |

@testcase=5812 @backend @version=2
Scenario: Register an Inventory with Event Type that exceeds 10 characters
	Given I want to register an "Inventory" in the system
	When I receive the data​ with "EventType" that exceeds 10 characters
	Then it must be stored in a Pendingtransactions repository with validation "El tipo de evento puede contener máximo 10 caracteres"

@testcase=5813 @backend @version=2
Scenario: Register an Inventory with Event Type containing spaces
	Given I want to register an "Inventory" in the system
	When I receive the data​ with "EventType" containing spaces
	Then it must be stored in a Pendingtransactions repository with validation "El tipo de evento solo admite letras"

@testcase=5814 @backend @version=2
Scenario: Register an Inventory with Event Type containing other than letters
	Given I want to register an "Inventory" in the system
	When I receive the data​ with "EventType" containing other than letters
	Then it must be stored in a Pendingtransactions repository with validation "El tipo de evento solo admite letras"

@testcase=5815 @backend @version=2
Scenario: Register an Inventory with Inventory Date greater than or equal to current date
	Given I want to register an "Inventory" in the system
	When I receive the data​ with "InventoryDate" greater than or equal to current date
	Then it must be stored in a Pendingtransactions repository with validation "La fecha del inventario debe ser menor a la fecha actual"

@testcase=5816 @backend @version=2
Scenario: Register an Inventory with Inventory Date Month different from the current date month
	Given I want to register an "Inventory" in the system
	When I receive the data​ with "InventoryDateMonth" different from the current date month
	Then it must be stored in a Pendingtransactions repository with validation "No es posible registrar un inventario de meses anteriores"

@testcase=5817 @backend @version=2
Scenario: Register an Inventory with Scenario that exceeds 50 characters
	Given I want to register an "Inventory" in the system
	When I receive the data​ with "Scenario" that exceeds 50 characters
	Then it must be stored in a Pendingtransactions repository with validation "El escenario puede contener máximo 50 caracteres"

@testcase=5819 @backend @version=2
Scenario: Register an Inventory with Observations that exceeds 150 characters
	Given I want to register an "Inventory" in the system
	When I receive the data​ with "Observations" that exceeds 150 characters
	Then it must be stored in a Pendingtransactions repository with validation "Las observaciones pueden contener máximo 150 caracteres"

@testcase=5820 @backend @version=2
Scenario: Register an Inventory data​ with Attribute Description that exceeds 150 characters
	Given I want to register an "Inventory" in the system
	When I receive the data​ with "AttributeDescription" that exceeds 150 characters
	Then it must be stored in a Pendingtransactions repository with validation "La descripción del atributo puede contener máximo 150 caracteres"

@testcase=5821 @backend @version=2
Scenario: Validate when the Node does not exist
	Given I want to register an "Inventory" in the system
	When the "Node" does not exist
	Then it must be stored in a Pendingtransactions repository with validation "El nodo no existe"

@testcase=5822 @backend @version=2
Scenario: Validate when the product does not belong to the Node Storage Locations
	Given I want to register an "Inventory" in the system
	When "Product" does not belongs to one of the "NodeStorageLocations" of the "Node"
	Then it must be stored in a Pendingtransactions repository with validation "[ProductId] no pertenece al nodo"

@testcase=5823 @backend @version=2
Scenario: Validate the Ownership with different Ownership Value Unit
	Given I want to register an "Inventory" in the system
	When there are owner records with different "OwnershipValueUnit"
	Then it must be stored in a Pendingtransactions repository with validation "Los registros de propiedad no tienen la misma unidad"

@testcase=5824 @backend @version=2
Scenario: Validate the Ownership when Ownership Value Unit for all Owners as Percentage and the sum of OwnerShip Value is not 100%
	Given I want to register an "Inventory" in the system
	When there are owner records with "OwnershipValueUnit" for all Owners as "Percentage" and sum of "OwnershipValue" is not equal to "100%"
	Then it must be stored in a Pendingtransactions repository with validation "La sumatoria de los valores de propiedad debe ser 100%"

@testcase=5825 @backend @version=2
Scenario: Validate the Ownership with Ownership Value Unit for for all Owners as Volume and the sum of Ownership Value is not equal to the Product Volume
	Given I want to register an "Inventory" in the system
	When there are owner records with "OwnershipValueUnit" for all Owners as "Volume" and sum of "OwnershipValue" is not equal to "ProductVolume"
	Then it must be stored in a Pendingtransactions repository with validation "La sumatoria de los valores de propiedad debe ser igual al volumen del producto"

@testcase=5826 @backend @version=4 @bvt @prodready
Scenario: Register a new Inventory with valid data
	Given I want to register an "Inventory" in the system
	When it meets "all" input validations
	And the "EventType" field is equal to "Insert"
	Then it should be registered

@testcase=5827 @backend @version=2
Scenario: Cancel a new Inventory when the Inventory Identifier does not exist
	Given I want to cancel an "Inventory"
	When the 'InventoryIdentifier' does not exist and "EventType" field is equal to "Delete"
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a anular no existe"

@testcase=5828 @backend @version=2
Scenario: Cancel a new Inventory with Inventory Identifier
	Given I want to cancel an "Inventory"
	When the "EventType" field is equal to "Delete"
	Then record a new "Inventory" with negative values for the "ProductVolume"
	And it should be registered

@testcase=5829 @backend @version=2
Scenario: Adjust a new Inventory when the Inventory Identifier does not exist
	Given I want to adjust an "Inventory"
	When the 'InventoryIdentifier' does not exist and "EventType" field is equal to "Update"
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a ajustar no existe"

@testcase=5830 @backend @version=2
Scenario: Adjust a new Inventory with Inventory Identifier
	Given I want to adjust an "Inventory"
	When the "EventType" field is equal to "Update"
	Then record a new "Inventory" with negative values for the "ProductVolume"
	And it should be registered

@testcase=5831 @backend @version=2
Scenario: Register a new Inventory with existing Inventory Identifier
	Given I want to register an "Inventory" in the system
	When the "InventoryIdentifier" already exist
	And the "EventType" field is equal to "Insert"
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario ya existe en el sistema​"

@testcase=5832 @backend
Scenario:  Verify the data types of all the fields
	Given I want to register an "Inventory" in the system
	When I find mismatch between the datatypes
	Then a validation must be created for each field with message "[Field Name] Debe ser de tipo: [Data Type]"