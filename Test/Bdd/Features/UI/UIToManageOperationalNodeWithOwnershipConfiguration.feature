@owner=jagudelos @ui @sharedsteps=4013 @testplan=35673 @testsuite=35679
Feature: UIToManageOperationalNodeWithOwnershipConfiguration
As an TRUE Administrator user, I require a user interface to manage the
operational node with ownership configuration of the analytical model

Background:  Login
Given I am logged in as "admin"
And I want to create data for Operative Nodes with Ownership

@testcase=37466 @bvt  @priority=3 @manual @version=3
Scenario: Verify after Data load from ADF Pipelines we are able to edit from UI
And I have trained the analytical model with the historical data
When I navigate to "Configuration of transfer points - logistic nodes" page
And I click on "createTransferPoint" "button"
Then I should see all the uploaded data available in the UI

@testcase=37467 @bvt  @version=3
Scenario: Verify that TRUE administrator require to manage elements in the category
And I navigate to "Configuration of transfer points - logistic nodes" page
When I click on "createTransferPoint" "button"
And I should see "CreateRelationship" interface
Then I select any value from "transferPointLogistics" "transferPoint" "dropdown"

@testcase=37468 @bvt @priority=3 @version=3
Scenario Outline: Verify as a TRUE administrator when i try to save without the mandatory fields
And I navigate to "Configuration of transfer points - logistic nodes" page
When I click on "createTransferPoint" "button"
And I should see "CreateRelationship" interface
And I dont provide value in "transferPointLogistics" "<field>" "dropdown" in ui
And I click on "transferPointLogistics" "submit" "button"
Then  I should see the message on interface "Requerido"

Examples:
| field                      |
| sourceNode                 |
| destinationNode            |
| sourceStorageLocation      |
| destinationStorageLocation |
| sourceProduct              |
| destinationProduct         |
| transferPoint              |

@testcase=37469 @version=3  @priority=2
Scenario: Verify that TRUE administrator require to configure Valid settings only for TRUE transfer points
And I navigate to "Configuration of transfer points - logistic nodes" page
When I click on "createTransferPoint" "button"
And I should see "CreateRelationship" interface
Then I select any value from "transferPointLogistics" "transferPoint" "dropdown"
And I select any value from "transferPointLogistics" "sourceNode" "dropdown"
And I select any value from "transferPointLogistics" "destinationNode" "dropdown"

@testcase=37470 @version=3  @bvt
Scenario: Verify that TRUE administrator able to create transfer relationship with valid details
And I navigate to "Configuration of transfer points - logistic nodes" page
When I click on "createTransferPoint" "button"
And I should see "CreateRelationship" interface
And I select any value from "transferPointLogistics" "transferPoint" "dropdown"
And I select any value from "transferPointLogistics" "sourceNode" "dropdown"
And I select any value from "transferPointLogistics" "destinationNode" "dropdown"
And I select any value from "transferPointLogistics" "sourceStorageLocation" "dropdown"
And I select any value from "transferPointLogistics" "sourceProduct" "dropdown"
And I select any value from "transferPointLogistics" "destinationStorageLocation" "dropdown"
And I select any value from "transferPointLogistics" "destinationProduct" "dropdown"
And I click on "transferPointLogistics" "submit" "button"
Then I should see created TransferRelation information in "transferPointsLogistics" "logisticSourceCenter" "text"

@testcase=37471 @version=3  @priority=3
Scenario: Verify that TRUE administrator able to view transfer relationships records in grid with required information
Given I navigate to "Configuration of transfer points - logistic nodes" page
When I click on "createTransferPoint" "button"
And I should see "CreateRelationship" interface
Then I should see all the fields
And validate that "transferPointsLogistics" "deleteTransferPoint" "link" is "enabled"

@testcase=37472 @version=3 @priority=3
Scenario: Verify that TRUE administrator able to create a transfer relationships with existing details
And I navigate to "Configuration of transfer points - logistic nodes" page
When I click on "createTransferPoint" "button"
And I should see "CreateRelationship" interface
And I select any value from "transferPointLogistics" "transferPoint" "dropdown"
And I select any value from "transferPointLogistics" "sourceNode" "dropdown"
And I select any value from "transferPointLogistics" "destinationNode" "dropdown"
And I select any value from "transferPointLogistics" "sourceStorageLocation" "dropdown"
And I select any value from "transferPointLogistics" "sourceProduct" "dropdown"
And I select any value from "transferPointLogistics" "destinationStorageLocation" "dropdown"
And I select any value from "transferPointLogistics" "destinationProduct" "dropdown"
And I click on "transferPointLogistics" "submit" "button"
And I select any value from "transferPointLogistics" "transferPoint" "dropdown"
And I select any value from "transferPointLogistics" "sourceNode" "dropdown"
And I select any value from "transferPointLogistics" "destinationNode" "dropdown"
And I select any value from "transferPointLogistics" "sourceStorageLocation" "dropdown"
And I select any value from "transferPointLogistics" "sourceProduct" "dropdown"
And I select any value from "transferPointLogistics" "destinationStorageLocation" "dropdown"
And I select any value from "transferPointLogistics" "destinationProduct" "dropdown"
And I click on "transferPointLogistics" "submit" "button"
Then I should see error message "Ya existe una relación de transferencia para los valores ingresados."

@testcase=37473 @version=3 @priority=3
Scenario: Verify that TRUE administrator grid information if There are No transfer relationship configuration rows
Given I navigate to "Configuration of transfer points - logistic nodes" page
Then I should see the error message "Sin registros"

@testcase=37474 @version=3 @priority=2
Scenario: Verify that TRUE administrator can Delete a transfer relationship
And I navigate to "Configuration of transfer points - logistic nodes" page
When I click on "createTransferPoint" "button"
And I should see "CreateRelationship" interface
And I select any value from "transferPointLogistics" "transferPoint" "dropdown"
And I select any value from "transferPointLogistics" "sourceNode" "dropdown"
And I select any value from "transferPointLogistics" "destinationNode" "dropdown"
And I select any value from "transferPointLogistics" "sourceStorageLocation" "dropdown"
And I select any value from "transferPointLogistics" "sourceProduct" "dropdown"
And I select any value from "transferPointLogistics" "destinationStorageLocation" "dropdown"
And I select any value from "transferPointLogistics" "destinationProduct" "dropdown"
And I click on "transferPointLogistics" "submit" "button"
Then I click on "transferPointsLogistics" "deleteTransferPoint" "link"
And I should see "DeleteTransferPoint" interface
And I provide the value for "CreateNode" "description" "textarea"
And I click on "transferPointLogistics" "submit" "button"

@testcase=37475 @version=3 @priority=3
Scenario: Verify that TRUE administrator can Delete a transfer relationship with values exceeding maximum limit
Given I navigate to "Configuration of transfer points - logistic nodes" page
When I click on "createTransferPoint" "button"
And I should see "CreateRelationship" interface
And I select any value from "transferPointLogistics" "transferPoint" "dropdown"
And I select any value from "transferPointLogistics" "sourceNode" "dropdown"
And I select any value from "transferPointLogistics" "destinationNode" "dropdown"
And I select any value from "transferPointLogistics" "sourceStorageLocation" "dropdown"
And I select any value from "transferPointLogistics" "sourceProduct" "dropdown"
And I select any value from "transferPointLogistics" "destinationStorageLocation" "dropdown"
And I select any value from "transferPointLogistics" "destinationProduct" "dropdown"
And I click on "transferPointLogistics" "submit" "button"
Then I click on "transferPointsLogistics" "deleteTransferPoint" "link"
And I should see "DeleteTransferPoint" interface
And I provide the value for "CreateNode" "description" "textarea"
And I provide the value for "CreateNode" "description" "textarea"
And I click on "transferPointLogistics" "submit" "button"
And I should see error message "El campo puede contener máximo 1000 caracteres"

@testcase=37476 @version=3 @priority=3
Scenario Outline: Verify that TRUE administrator can filter by various columns
And I navigate to "Configuration of transfer points - logistic nodes" page
When I click on "createTransferPoint" "button"
And I should see "CreateRelationship" interface
And I select any value from "transferPointLogistics" "transferPoint" "dropdown"
And I select any value from "transferPointLogistics" "sourceNode" "dropdown"
And I select any value from "transferPointLogistics" "destinationNode" "dropdown"
And I select any value from "transferPointLogistics" "sourceStorageLocation" "dropdown"
And I select any value from "transferPointLogistics" "sourceProduct" "dropdown"
And I select any value from "transferPointLogistics" "destinationStorageLocation" "dropdown"
And I select any value from "transferPointLogistics" "destinationProduct" "dropdown"
And I click on "transferPointLogistics" "submit" "button"
Then I enter data into the "<filter>"
And I should see the records filtered as per the search criteria

Examples:
| filter                   |
| Punto de Transferencia   |
| Centro - almacén Origen  |
| Producto Origen          |
| Centro - almacén Destino |
| Producto Destino         |

@priority=4 @version=3 @testcase=37477
Scenario Outline: Verify that TRUE administrator can sort by various colums
And I navigate to "Configuration of transfer points - logistic nodes" page
When I click on "createTransferPoint" "button"
And I should see "CreateRelationship" interface
And I select any value from "transferPointLogistics" "transferPoint" "dropdown"
And I select any value from "transferPointLogistics" "sourceNode" "dropdown"
And I select any value from "transferPointLogistics" "destinationNode" "dropdown"
And I select any value from "transferPointLogistics" "sourceStorageLocation" "dropdown"
And I select any value from "transferPointLogistics" "sourceProduct" "dropdown"
And I select any value from "transferPointLogistics" "destinationStorageLocation" "dropdown"
And I select any value from "transferPointLogistics" "destinationProduct" "dropdown"
And I click on "transferPointLogistics" "submit" "button"
And I click on "<column>" filter
Then I should see the records in grid sorted

Examples:
| column                   |
| Punto de Transferencia   |
| Centro - almacén Origen  |
| Producto Origen          |
| Centro - almacén Destino |
| Producto Destino         |

@testcase=41501 @priority=2
Scenario: Verify that TRUE Administrator can only create transferpoints for nodes with logistic center
And I navigate to "Configuration of transfer points - logistic nodes" page
When I click on "createTransferPoint" "button"
And I should see "CreateRelationship" interface
And I select any value from "transferPointLogistics" "transferPoint" "dropdown"
And I select any value from "transferPointLogistics" "sourceNode" "dropdown"
Then I should see the node in the dropdown

@testcase=41502 @priority=2
Scenario: Verify that TRUE Administrator cannot create transferpoints for nodes with no logistic center
And I navigate to "Configuration of transfer points - logistic nodes" page
When I click on "createTransferPoint" "button"
And I should see "CreateRelationship" interface
And I select any value from "transferPointLogistics" "transferPoint" "dropdown"
And I select any value from "transferPointLogistics" "sourceNode" "dropdown"
Then I should not see the node in the dropdown
