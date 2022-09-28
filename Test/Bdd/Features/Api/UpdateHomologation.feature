@sharedsteps= @owner=jagudelos @api @testplan=19772 @testsuite=21338
Feature: UpdateHomologation
In order to handle the Data Mapping
As an application administrator
I want to Update Homologation for Data Mapping

Background: Login
Given I am authenticated as "admin"

@testcase=21348 @output=QueryAll(GetHomologations)
Scenario Outline: Update Homologation for Data Mapping without mandatory fields
Given I have "Homologations" in the system
When I update a homologation without any "<Field>"
Then the response should fail with message "<Message>"

Examples:
| Field                                       | Message                                                                        |
| HomologationGroup GroupTypeId               | El identificador de la fuente de información en el sistema TRUE es obligatorio |
| HomologationObject HomologationObjectTypeId | El nombre del objeto en el sistema origen es obligatorio                       |
| HomologationDataMapping SourceValue         | El valor en el sistema origen es obligatorio                                   |
| HomologationDataMapping DestinationValue    | El valor en el sistema destino es obligatorio                                  |

@testcase=21349 @output=QueryAll(GetHomologations)
Scenario Outline: Update Homologation for Data Mapping with fields that contains any other character except numbers
Given I have "Homologations" in the system
When I update a homologation with "<Field>" that contains any alphabet
Then the response should fail with "BadRequest"

Examples:
| Field                                       |
| SourceSystemId                              |
| DestinationSystemId                         |
| HomologationGroup GroupTypeId               |
| HomologationObject HomologationObjectTypeId |

@testcase=21350 @output=QueryAll(GetHomologations)
Scenario Outline: Update Homologation for Data Mapping with field that exceeds 100 characters
Given I have "Homologations" in the system
When I update a homologation with "<Field>" that exceeds 100 characters
Then the response should fail with message "<Message>"

Examples:
| Field                                    | Message                                                             |
| HomologationDataMapping SourceValue      | El valor en el sistema origen puede contener máximo 100 caracteres  |
| HomologationDataMapping DestinationValue | El valor en el sistema destino puede contener máximo 100 caracteres |

@testcase=21351 @output=QueryAll(GetHomologations)
Scenario Outline: Update Homologation for Data Mapping without mandatory objects
Given I have "Homologations" in the system
When I update a homologation without any "<Field>"
Then the response should fail with message "<Message>"

Examples:
| Field                                     | Message                                                                      |
| HomologationGroups                        | Una homologación debe tener por lo menos un grupo de homologación            |
| HomologationObjects                       | Un grupo de homologación debe tener por lo menos una homologación de objetos |
| HomologationGroup HomologationDataMapping | Un grupo de homologación debe tener por lo menos un mapeo de datos           |

@testcase=21352 @bvt @output=QueryAll(GetHomologations)
Scenario: Update an existing Homologation Group
Given I have "Homologations" in the system
When I update an existing "HomologationGroup"
Then the new Homologation should be registered with success message "Homologación  actualizada con éxito"


@testcase=21353 @output=QueryAll(GetHomologations)
Scenario Outline: Update an existing homologation group without mandotary fields assigned
Given I have "Homologations" in the system
When I update a homologation without any "<Field>"
Then the response should fail with message "<Message>"

Examples:
| Field                                     | Message                                                                      |
| HomologationGroups                        | Una homologación debe tener por lo menos un grupo de homologación            |
| HomologationObjects                       | Un grupo de homologación debe tener por lo menos una homologación de objetos |
| HomologationGroup HomologationDataMapping | Un grupo de homologación debe tener por lo menos un mapeo de datos           |

@testcase=21354 @bvt @output=QueryAll(GetHomologations)
Scenario: Update Homologation with new Homologation Group
Given I have "Homologations" in the system
When I update a homologation with new "HomologationGroup"
Then the new Homologation should be registered with success message "Homologación  actualizada con éxito"

@testcase=21355 @bvt @output=QueryAll(GetHomologations)
Scenario: Update Homologation with new Homologation Object
Given I have "Homologations" in the system
When I update a homologation with new "HomologationObject"
Then the new Homologation should be registered with success message "Homologación  actualizada con éxito"

@testcase=21356 @bvt @output=QueryAll(GetHomologations)
Scenario: Update Homologation with new Data Mapping
Given I have "Homologations" in the system
When I update a homologation with new "HomologationDataMapping"
Then the new Homologation should be registered with success message "Homologación  actualizada con éxito"

@testcase=21357 @bvt @output=QueryAll(GetHomologations)
Scenario: Update existing Homologation Object data
Given I have "Homologations" in the system
When I update an existing "HomologationObject"
Then the new Homologation should be registered with success message "Homologación  actualizada con éxito"

@testcase=21358 @bvt @output=QueryAll(GetHomologations)
Scenario: Update existing Homologation Data Mapping
Given I have "Homologations" in the system
When I update an existing "HomologationDataMapping"
Then the new Homologation should be registered with success message "Homologación  actualizada con éxito"

@testcase=21359 @bvt @output=QueryAll(GetHomologations)
Scenario Outline: Verify message recieved when TRUE user tries to update homologation group with missing fields
Given I have "Homologations" in the system
When I delete existing "<objects>" from content
Then the response should fail with message "<ErrorMessage>"
Examples:
| objects                                   | ErrorMessage                                                                 |
| HomologationGroup HomologationObjects     | Un grupo de homologación debe tener por lo menos una homologación de objetos |
| HomologationGroup HomologationDataMapping | Un grupo de homologación debe tener por lo menos un mapeo de datos           |

@testcase=21360 @bvt  @output=QueryAll(GetHomologations)
Scenario: Verify TRUE user is able to delete existing homologation object
Given I have "Homologations" in the system
When I delete existing "HomologationObject"
Then the new Homologation should be registered with success message "Homologación  actualizada con éxito"



@testcase=21361 @bvt @output=QueryAll(GetHomologations)
Scenario:  Verify TRUE user is able to delete existing homologation mapping
Given I have "Homologations" in the system
When I delete existing "HomologationDataMapping"
Then the new Homologation should be registered with success message "Homologación  actualizada con éxito"


