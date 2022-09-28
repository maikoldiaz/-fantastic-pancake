@sharedsteps=7539 @owner=jagudelos @testplan=4938 @testsuite=5397
Feature: CreateHomologation
In order to handle the Data Mapping
As an application administrator
I want to Create Homologation for Data Mapping

Background: Login
	Given I am authenticated as "admin"

@testcase=5768 @api @bvt @audit @output=QueryAll(GetHomologations) @version=3 @prodready
Scenario Outline: Create Homologation for Data Mapping with valid data
	Given I want to create a "Homologation" in the system
	When I provide the valid data for "<GroupType>" of "<TRUELocation>"
	Then the response should succeed with message "Homologación creada con éxito"

	Examples:
		| GroupType        | TRUELocation |
		| Nodes            | Source       |
		| StorageLocations | Source       |
		| Products         | Source       |
		| Owners           | Source       |
		| Nodes            | Destination  |
		| StorageLocations | Destination  |
		| Products         | Destination  |
		| Owners           | Destination  |

@testcase=5769 @api @output=QueryAll(GetHomologations) @prodready
Scenario Outline: Create Homologation for Data Mapping with invalid data
	Given I want to create a "Homologation" in the system
	When I provide the invalid data for "<GroupType>" of "<TRUELocation>"
	Then the response should fail with message "La entidad relacionada no existe"

	Examples:
		| GroupType        | TRUELocation |
		| Nodes            | Source       |
		| StorageLocations | Source       |
		| Products         | Source       |
		| Owners           | Source       |
		| Nodes            | Destination  |
		| StorageLocations | Destination  |
		| Products         | Destination  |
		| Owners           | Destination  |

@testcase=5770 @api @output=QueryAll(GetHomologations) @prodready
Scenario: Create Homologation for Data Mapping with the existing data
	Given I want to create a "Homologation" in the system
	When I provide the existing data
	Then the response should fail with message "La homologación entre estos sistemas ya existe"

@testcase=5771 @api @output=QueryAll(GetHomologations) @prodready
Scenario Outline: Create Homologation for Data Mapping without mandatory fields
	Given I want to create a "Homologation" in the system
	When I don't provide "<Field>"
	Then the response should fail with message "<Message>"

	Examples:
		| Field                                       | Message                                                                        |
		| SourceSystemId                              | El identificador del sistema origen es obligatorio                             |
		| DestinationSystemId                         | El identificador del sistema destino es obligatorio                            |
		| HomologationGroup GroupTypeId               | El identificador de la fuente de información en el sistema TRUE es obligatorio |
		| HomologationObject HomologationObjectTypeId | El nombre del objeto en el sistema origen es obligatorio                       |
		| HomologationDataMapping SourceValue         | El valor en el sistema origen es obligatorio                                   |
		| HomologationDataMapping DestinationValue    | El valor en el sistema destino es obligatorio                                  |

@testcase=5772 @api @output=QueryAll(GetHomologations) @prodready
Scenario Outline: Create Homologation for Data Mapping with fields that contains any other character except numbers
	Given I want to create a "Homologation" in the system
	When I provide "<Field>" that contains any alphabet
	Then the response should fail with "BadRequest"

	Examples:
		| Field                                       |
		| SourceSystemId                              |
		| DestinationSystemId                         |
		| HomologationGroup GroupTypeId               |
		| HomologationObject HomologationObjectTypeId |

@testcase=5773 @api @output=QueryAll(GetHomologations) @prodready
Scenario: Create Homologation for Data Mapping without including the TRUE system as source or destination
	Given I want to create a "Homologation" in the system
	When I don't provide at least one of the source or destination system corresponds to the TRUE system
	Then the response should fail with message "Por lo menos uno de los sistemas de entrada o salida debe corresponder al sistema TRUE"

@testcase=5774 @api @output=QueryAll(GetHomologations) @prodready
Scenario Outline: Create Homologation for Data Mapping with field that exceeds 100 characters
	Given I want to create a "Homologation" in the system
	When I provide "<Field>" that exceeds 100 characters
	Then the response should fail with message "<Message>"

	Examples:
		| Field                                    | Message                                                             |
		| HomologationDataMapping SourceValue      | El valor en el sistema origen puede contener máximo 100 caracteres  |
		| HomologationDataMapping DestinationValue | El valor en el sistema destino puede contener máximo 100 caracteres |

@testcase=5775 @api @output=QueryAll(GetHomologations) @prodready
Scenario Outline: Create Homologation for Data Mapping without mandatory fields assigned
	Given I want to create a "Homologation" in the system
	When I don't provide any "<Field>"
	Then the response should fail with message "<Message>"

	Examples:
		| Field                                     | Message                                                                      |
		| HomologationGroups                        | Una homologación debe tener por lo menos un grupo de homologación            |
		| HomologationObjects                       | Un grupo de homologación debe tener por lo menos una homologación de objetos |
		| HomologationGroup HomologationDataMapping | Un grupo de homologación debe tener por lo menos un mapeo de datos           |