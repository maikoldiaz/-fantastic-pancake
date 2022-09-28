@sharedsteps=7539 @owner=jagudelos @testplan=4938 @testsuite=5400 @backend @version=2
Feature: HomologatedDataMovement
In order to register a Movement in the system
As a TRUE system
I want to homologate the data

Background: Login
	Given I am authenticated as "admin"

@testcase=5833 @backend @version=2
Scenario Outline: Register a Movement without Mandatory Fields
	Given I want to register an "Movements" in the system
	When I don't receive "<Field>" in XML
	Then it must be stored in a Pendingtransactions repository with validation "<Message>"

	Examples:
		| Field              | Message                                                           |
		| EventType          | El tipo de evento es obligatorio                                  |
		| MovementId         | El identificador del movimiento es obligatorio                    |
		| MovementTypeId     | El identificador del tipo de movimiento es obligatorio            |
		| OperationalDate    | Retrieving data. Wait a few seconds and try to cut or copy again. |
		| Period             | El periodo es obligatorio                                         |
		| NetStandardVolume  | El volumen neto es obligatorio                                    |
		| Classification     | La clasificación del movimiento es obligatoria                    |
		| StartTime          | La hora de inicio del movimiento es obligatoria                   |
		| EndTime            | La hora final del movimiento es obligatoria                       |
		| AttributeId        | El identificador del atributo es obligatorio                      |
		| AttributeValue     | El valor del atributo es obligatorio                              |
		| ValueAttributeUnit | La unidad de medida del atributo es obligatoria                   |
		| OwnerId            | El identificador del propietario  es obligatorio                  |
		| OwnershipValue     | El valor de la propiedad es obligatorio                           |
		| OwnershipValueUnit | La unidad del valor de la propiedad es obligatoria                |

@testcase=5834 @backend @version=2
Scenario Outline: Register a Movement without Optional Fields
	Given I want to register an "Movements" in the system
	When I don't receive "<Field>" in XML
	Then it should be registered

	Examples:
		| Field                        |
		| GrossStandardVolume          |
		| MeasurementUnit              |
		| Attributes                   |
		| Owners                       |
		| Observations                 |
		| SourceStorageLocationId      |
		| DestinationStorageLocationId |
		| DestinationProductId         |
		| DestinationProductTypeId     |
		| AttributeDescription         |

@testcase=5835 @backend @version=2
Scenario: Register a Movement with Event Type that exceeds 10 characters
	Given I want to register an "Movements" in the system
	When I receive the data​ with "EventType" that exceeds 10 characters
	Then it must be stored in a Pendingtransactions repository with validation "El tipo de evento puede contener máximo 10 caracteres"

@testcase=5836 @backend @version=2
Scenario: Register a Movement with Event Type containing spaces
	Given I want to register an "Movements" in the system
	When I receive the data​ with "EventType" containing spaces
	Then it must be stored in a Pendingtransactions repository with validation "El tipo de evento solo admite letras"

@testcase=5837 @backend @version=2
Scenario: Register a Movement with Event Type containing other than letters
	Given I want to register an "Movements" in the system
	When I receive the data​ with "EventType" containing other than letters
	Then it must be stored in a Pendingtransactions repository with validation "El tipo de evento solo admite letras"

@testcase=5838 @backend @version=2
Scenario: Register a Movement with Operational Date greater than or equal to current date
	Given I want to register an "Movements" in the system
	When I receive the data​ with "OperationalDate" greater than or equal to current date
	Then it must be stored in a Pendingtransactions repository with validation "La fecha operacional debe ser menor a la fecha actual"

@testcase=5839 @backend @version=2
Scenario: Register a Movement with Operational Date month different from the current date month
	Given I want to register an "Movements" in the system
	When I receive the data​ with "OperationalDateMonth" different from the current date month
	Then it must be stored in a Pendingtransactions repository with validation "No es posible registrar un movimiento de meses anteriores"

@testcase=5840 @backend @version=2
Scenario: Register a Movement without Movement Source and Classification of Movement equal to Movimiento or Operacion Trazable
	Given I want to register an "Movements" in the system
	When I dont receive "MovementSource" and 'Classification' is equal to 'Movimiento' or 'OperacionTrazable'
	Then it must be stored in a Pendingtransactions repository with validation "El origen del movimiento es obligatorio"

@testcase=5841 @backend @version=2
Scenario: Register a Movement without Movement Source and Classification of Movement equal to PerdidaIdentificada
	Given I want to register an "Movements" in the system
	When I dont receive 'MovementSource' and 'Classification' is equal to 'PerdidaIdentificada'
	Then it should be registered

@testcase=5842 @backend @version=2
Scenario: Register a Movement without Movement Destination and Classification of Movement equal to Movimiento or Operacion Trazable
	Given I want to register an "Movements" in the system
	When I dont receive "MovementDestination" and 'Classification' is equal to 'Movimiento' or 'OperacionTrazable'
	Then it must be stored in a Pendingtransactions repository with validation "El destino del movimiento es obligatorio"

@testcase=5843 @backend @version=2
Scenario: Register a Movement without Movement Destination and Classification of Movement is equal to PerdidaIdentificada
	Given I want to register an "Movements" in the system
	When I dont receive 'MovementDestination' and 'Classification' is equal to 'PerdidaIdentificada'
	Then it should be registered

@testcase=5844 @backend @version=2
Scenario: Register a Movement with Scenario that exceeds 50 characters
	Given I want to register an "Movements" in the system
	When I receive the data​ with "Scenario" that exceeds 50 characters
	Then it must be stored in a Pendingtransactions repository with validation "El escenario puede contener máximo 50 caracteres"

@testcase=5846 @backend @version=2
Scenario: Register a Movement with Observations that exceeds 150 characters
	Given I want to register an "Movements" in the system
	When I receive the data​ with "Observations" that exceeds 150 characters
	Then it must be stored in a Pendingtransactions repository with validation "Las observaciones pueden contener máximo 150 caracteres"

@testcase=5847 @backend @version=2
Scenario: Register a Movement with Classification that exceeds 30 characters
	Given I want to register an "Movements" in the system
	When I receive the data​ with "Classification" that exceeds 30 characters
	Then it must be stored in a Pendingtransactions repository with validation "La clasificación del movimiento puede contener máximo 30 caracteres"

@testcase=5848 @backend @version=2
Scenario: Register a Movement with Classification containing spaces
	Given I want to register an "Movements" in the system
	When I receive the data​ with "Classification" containing spaces
	Then it must be stored in a Pendingtransactions repository with validation "La clasificación del movimiento solo admite letras"

@testcase=5849 @backend @version=2
Scenario: Register a Movement with Classification containing other than letters
	Given I want to register an "Movements" in the system
	When I receive the data​ with "Classification" containing other than letters
	Then it must be stored in a Pendingtransactions repository with validation "La clasificación del movimiento solo admite letras"

@testcase=5850 @backend @version=2
Scenario: Register a Movement data​ without Source Node Id and Classification of Movement equal to Movimiento or Operacion Trazable
	Given I want to register an "Movements" in the system
	When I dont receive "SourceNodeId" and 'Classification' is equal to 'Movimiento' or 'OperacionTrazable'
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del nodo origen es obligatorio"

@testcase=5851 @backend @version=2
Scenario: Register a Movement data​ without Source Node Id and Classification of Movement equal to PerdidaIdentificada
	Given I want to register an "Movements" in the system
	When I dont receive 'SourceNodeId' and 'Classification' is equal to 'PerdidaIdentificada'
	Then it should be registered

@testcase=5852 @backend @version=2
Scenario: Register a Movement data​ without Source Product Id and Classification of Movement equal to Movimiento or Operacion Trazable
	Given I want to register an "Movements" in the system
	When I dont receive "SourceProductId" and 'Classification' is equal to 'Movimiento' or 'OperacionTrazable'
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del producto origen es obligatorio"

@testcase=5853 @backend @version=2
Scenario: Register a Movement data​ without Source Product Id and Classification of Movement equal to PerdidaIdentificada
	Given I want to register an "Movements" in the system
	When I dont receive 'SourceProductId' and 'Classification' is equal to 'PerdidaIdentificada'
	Then it should be registered

@testcase=5854 @backend @version=2
Scenario: Register a Movement data​ without Source Product Type Id and Classification of Movement equal to Movimiento or Operacion Trazable
	Given I want to register an "Movements" in the system
	When I dont receive "SourceProductTypeId" and 'Classification' is equal to 'Movimiento' or 'OperacionTrazable'
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del tipo de producto origen es obligatorio"

@testcase=5855 @backend @version=2
Scenario: Register a Movement data​ without Source Product Type Id and Classification of Movement equal to PerdidaIdentificada
	Given I want to register an "Movements" in the system
	When I dont receive 'SourceProductTypeId' and 'Classification' is equal to 'PerdidaIdentificada'
	Then it should be registered

@testcase=5856 @backend @version=2
Scenario: Register a Movement data​ without Destination Node Id and Classification of Movement equal to Movimiento or Operacion Trazable
	Given I want to register an "Movements" in the system
	When I dont receive "DestinationNodeId" and 'Classification' is equal to 'Movimiento' or 'OperacionTrazable'
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del nodo destino es obligatorio"

@testcase=5857 @backend @version=2
Scenario: Register a Movement data​ without Destination Node Id and Classification of Movement equal to PerdidaIdentificada
	Given I want to register an "Movements" in the system
	When I dont receive 'DestinationNodeId' and 'Classification' is equal to 'PerdidaIdentificada'
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del nodo destino es obligatorio"

@testcase=5858 @backend @version=2
Scenario: Register a Movement data​ with Attribute Description that exceeds 150 characters
	Given I want to register an "Movements" in the system
	When I receive the data​ with "AttributeDescription" that exceeds 150 characters
	Then it must be stored in a Pendingtransactions repository with validation "La descripción del atributo puede contener máximo 150 caracteres"

@testcase=5859 @backend @version=2
Scenario: Validate when the connection between Source Node and Destination Node does not exist
	Given I want to register an "Movements" in the system
	And connection between "SourceNode" and "DestinationNode" does not exist
	Then it must be stored in a Pendingtransactions repository with validation "No existe una conexión para los nodos origen y destino recibidos"

@testcase=5860 @backend @version=2
Scenario: Validate when the Source Product does not belong to the Storage Location of the Source Node
	Given I want to register an "Movements" in the system
	When "SourceProduct" does not belongs to one of the "NodeStorageLocations" of the "SourceNode"
	Then it must be stored in a Pendingtransactions repository with validation "El producto origen no pertenece al nodo origen"

@testcase=5861 @backend @version=2
Scenario: Validate when the Destination Product does not belong to the Storage Location of the Destination Node
	Given I want to register an "Movements" in the system
	When "DestinationProduct" does not belongs to one of the "NodeStorageLocations" of the "DestinationNode"
	Then it must be stored in a Pendingtransactions repository with validation "El producto destino no pertenece al nodo destino"

@testcase=5862 @backend @version=2
Scenario: Validate the Ownership with different Ownership Value Unit
	Given I want to register an "Movements" in the system
	When there are owner records with different "OwnershipValueUnit"
	Then it must be stored in a Pendingtransactions repository with validation "Los registros de propiedad no tienen la misma unidad"

@testcase=5863 @backend @version=2
Scenario: Validate the Ownership when Ownership Value Unit for all owners as Percentage and sum of OwnerShip Value is not 100%
	Given I want to register an "Movements" in the system
	When there are owner records with "OwnershipValueUnit" for all Owners as "Percentage" and sum of "OwnershipValue" is not equal to "100%"
	Then it must be stored in a Pendingtransactions repository with validation "La sumatoria de los valores de propiedad debe ser 100%"

@testcase=5864 @backend @version=2
Scenario: Validate the Ownership with Ownership Value Unit for all owners as Volume and the sum of Ownership Value is not equal to the Net Standard Volume
	Given I want to register an "Movements" in the system
	When there are owner records with "OwnershipValueUnit" for all Owners as "Volume" and sum of "OwnershipValue" is not equal to "NetStandardVolume"
	Then it must be stored in a Pendingtransactions repository with validation "La sumatoria de los valores de propiedad debe ser igual al volumen neto"

@testcase=5865 @backend @version=4 @bvt @prodready
Scenario: Register a new Movement with valid data
	Given I want to register an "Movements" in the system
	When it meets "all" input validations
	And the "EventType" field is equal to "Insert"
	Then it should be registered

@testcase=5866 @backend @version=2
Scenario: Cancel a new Movement when the Movement Identifier does not exist
	Given I want to cancel an "Movements"
	When the 'MovementIdentifier' does not exist and "EventType" field is equal to "Delete"
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del movimiento a anular no existe"

@testcase=5867 @backend @version=2
Scenario: Cancel a new Movement with Movement Identifier
	Given I want to cancel an "Movements"
	When the "EventType" field is equal to "Delete"
	Then record a new "MovementIdentifier" with negative values for the 'GrossStandardVolume' and 'NetStandardVolume'
	And it should be registered

@testcase=5868 @backend @version=2
Scenario: Adjust a new Movement when the Movement Identifier does not exist
	Given I want to adjust an "Movements"
	When the 'MovementIdentifier' does not exist and "EventType" field is equal to "Update"
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del movimiento a ajustar no existe"

@testcase=5869 @backend @version=2
Scenario: Adjust a new Movement with Movement Identifier
	Given I want to adjust an "Movements"
	When the "EventType" field is equal to "Update"
	Then record a new "MovementIdentifier" with negative values for the 'GrossStandardVolume' and 'NetStandardVolume'
	And it should be registered

@testcase=5870 @backend @version=2
Scenario: Register a new Movement with existing Movement Identifier
	Given I want to register an "Movements" in the system
	When the "MovementIdentifier" already exist
	And the "EventType" field is equal to "Insert"
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del movimiento ya existe en el sistema​"

@testcase=5871 @backend @version=2
Scenario: Register a Identified Lost Type Movement
	Given I want to register an "IdentifiedlostTypeMovement" in the system
	When the "IdentifiedlostTypeMovement" has a "SourceNode" or a "DestinationNode"
	Then it must be stored in a Pendingtransactions repository with validation "El movimiento de tipo perdida identificada debe tener un nodo origen o un nodo destino"

@testcase=5872 @backend @version=2
Scenario:  Verify the data types of all the fields
	Given I want to register an "Movements" in the system
	When I find mismatch between the datatypes
	Then a validation must be created for each field with message "DATATYPE_ERROR"