@sharedsteps=16581 @owner=jagudelos @ui @testsuite=26824 @testplan=26817 @parallel=false
Feature: UIToCreateAMovementWithOwnershipInANode
As a Balance Segment Professional User, I need a UI
to create a movement with ownership in a node

Background: Login
Given I am logged in as "profesional"

@testcase=28280 @bvt @version=2 @bvt1.5
Scenario: Verify data populated in create movement interface
	Given I have ownershipcalculated segment
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
	When I click on "EditOwnershipNode" "NewMovement" "button"
	Then I should see "CreateMovement" "Create" "Interface"
	And date should be end date of ownership period
	And verify that "CreateMovement" "Date" "Date" is "disabled"
	Then "variable" should be loaded with their corresponding values
	And "unit" should be loaded with their corresponding values
	And "movement type" should be loaded with their corresponding values
	And "reason for change" should be loaded with their corresponding values


@testcase=28281 @bvt @version=2 @bvt1.5
Scenario: Verify source node and destination node based on input value selected on create movement interface
	Given I have ownershipcalculated segment
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
	When I click on "EditOwnershipNode" "NewMovement" "button"
	Then I should see "CreateMovement" "Create" "Interface"
	When I selected "Input" from "CreateMovement" "variable" "combobox"
	Then "source node" list must contain the nodes of the input connections to the current node
	And destination node should be selected as current node 
	And "destination node" combobox on "create movement" interface should be "disabled"
	When I selected "Output" from "CreateMovement" "variable" "combobox"
	Then source node should be selected as current node
	And "source node" combobox on "create movement" interface should be "disabled"
	And "destination node" list must contain the nodes of the output connections of the current node
	When I selected "IdentifiedLoss​" from "CreateMovement" "variable" "combobox"
	Then source node should be selected as current node
	And "source node" combobox on "create movement" interface should be "disabled"
	And destination node list should appear disabled without elements
	When I selected "Interface​" from "CreateMovement" "variable" "combobox"
	Then source node should be selected as current node
	And "source node" combobox on "create movement" interface should be "disabled"
	And destination node should be selected as current node
	And "destination node" combobox on "create movement" interface should be "disabled"
	When I selected "Tolerance​" from "CreateMovement" "variable" "combobox"
	Then "source node" list must contain the current node​
	And "destination node" list must contain the current node​
	When I selected "UnidentifiedLoss​" from "CreateMovement" "variable" "combobox"
	Then "source node" list must contain the current node​
	And "destination node" list must contain the current node​

@testcase=28282 @version=2
Scenario Outline: Verify list of source and destination products must contain the products that belong to the selected source and destination node
	Given I have ownershipcalculated segment
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
	When I click on "EditOwnershipNode" "NewMovement" "button"
	Then I should see "CreateMovement" "Create" "Interface"
	When I selected "<variable>" from "CreateMovement" "variable" "combobox"
	And I select node from "CreateMovement" "SourceNode" "combobox" on create movement interface
	And I selected node from "CreateMovement" "destinationNode" "combobox" on create movement interface
	Then "source products" list should contain the products that belong to the selected source node
	And "destination products" list should contain the products that belong to the selected destination node
	Examples:
	| variable         |
	| Input            |
	| Output           |
	| Interface        |
	| Tolerance        |
	| UnidentifiedLoss |

@testcase=28283 @version=2
Scenario: Verify list of source and destination products when identified loss selected as variable
	Given I have ownershipcalculated segment
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
	When I click on "EditOwnershipNode" "NewMovement" "button"
	Then I should see "CreateMovement" "Create" "Interface"
	When I selected "IdentifiedLoss" from "CreateMovement" "variable" "combobox"
	Then source node should be selected as current node
	And "source node" combobox on "create movement" interface should be "disabled"
	And destination node list should appear disabled without elements
	And "source products" list should contain the products that belong to the selected source node
	And destination products list should be disabled without elements

@testcase=28284 @version=3
Scenario Outline: Verify ownership information on create movement interface based on selected source product
	Given I have ownershipcalculated segment
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
	When I click on "EditOwnershipNode" "NewMovement" "button"
	Then I should see "CreateMovement" "Create" "Interface"
	When I enter volume into "decimal" "netVolume" "textbox"
	And I selected "<variable>" from "CreateMovement" "variable" "combobox"
	And I selected node from either source node or destination node based on input variable
	And I selected "product" from "CreateMovement" "sourceProduct" "combobox"
	Then owners configured at the Connection-Product level should be displayed
	When I have modified owners volume on create movement interface
	Then ownership percentage is auto updated as per modifed volume on create movement interface
	And total volume and percentage should be auto updated on create movement interface
	When I have modified owners volume percentage on create movement interface
	Then owner's volume is auto updated
	Examples:
	| variable |
	| Input    |
	| Output   |

@testcase=28285 @version=3
Scenario Outline: Verify ownership information on create movement interface based on selected variable
	Given I have ownershipcalculated segment
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
	When I click on "EditOwnershipNode" "NewMovement" "button"
	Then I should see "CreateMovement" "Create" "Interface"
	When I enter volume into "decimal" "netVolume" "textbox"
	And I selected "<variable>" from "CreateMovement" "variable" "combobox"
	And I selected node from either source node or destination node based on input variable
	And I selected product from either source product or destination product
	Then owners configured at the Node-Product level should be displayed
	When I have modified owners volume on create movement interface
	Then ownership percentage is auto updated as per modifed volume on node product level
	And total volume and percentage should be auto updated on create movement interface
	When I have modified owners volume percentage on create movement interface
	Then owner's volume is auto updated on node product level
	Examples:
	| variable          |
	| Tolerance         |
	| UnidentifiedLoss​  |
	| IdentifiedLoss    |
	| Interface         |

@testcase=28286 @bvt @version=2
Scenario: Verify save button functionality on create movement interface when all input validations are met
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
When I click on "EditOwnershipNode" "NewMovement" "button"
Then I should see "CreateMovement" "Create" "Interface"
When all input validations met on create movement interface
And I click on "createMovement" "submit" "button"
Then create movement interface is closed
And balance summary with ownership is updated with newly created movement

@testcase=28287 @version=2
Scenario: Verify temporarily stored movement ownership information on edit Movement model window
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
When I click on "EditOwnershipNode" "NewMovement" "button"
Then I should see "CreateMovement" "Create" "Interface"
When all input validations met on create movement interface
And I click on "createMovement" "submit" "button"
Then create movement interface is closed
And I should see temporarily stored information for that Movement

@testcase=28288 @version=2
Scenario: Verify save button functionality on create movement interface when all input validations are not met
	Given I have ownershipcalculated segment
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
	When I click on "EditOwnershipNode" "NewMovement" "button"
	Then I should see "CreateMovement" "Create" "Interface"
	When I click on "createMovement" "submit" "button"
	Then I should see the message on interface "Requerido"

@testcase=28289 @version=2
Scenario: Verify comments textbox when user enters more than 150 characters on create movement interface
	Given I have ownershipcalculated segment
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
	When I click on "EditOwnershipNode" "NewMovement" "button"
	Then I should see "CreateMovement" "Create" "Interface"
	When I enter morethan 150 characters into "createMovement" "comments" "textbox"
	Then I should see error message "Máximo 150 caracteres"

@testcase=28290 @version=2
Scenario: Verify save button functionality on create movement interface when total ownership percentage is not met 100
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
When I click on "EditOwnershipNode" "NewMovement" "button"
Then I should see "CreateMovement" "Create" "Interface"
When input validations met on create movement interface except total ownership percentage
And I click on "createMovement" "submit" "button"
Then I should see error message "La sumatoria del volumen de propiedad debe ser igual al volumen del movimiento y la sumatoria de los porcentajes debe ser igual a 100"

@testcase=28291 @version=2
Scenario: Verify cancel button functionality on create movement interface
	Given I have ownershipcalculated segment
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
	When I click on "EditOwnershipNode" "NewMovement" "button"
	Then I should see "CreateMovement" "Create" "Interface"
	When I click on "CreateLogistics" "Cancel" "button"
	Then create movement interface is closed

@testcase=28292 @version=2
Scenario: Verify close(X) button functionality on create movement interface
	Given I have ownershipcalculated segment
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
	When I click on "EditOwnershipNode" "NewMovement" "button"
	Then I should see "CreateMovement" "Create" "Interface"
	When I click on close button
	Then create movement interface is closed

@testcase=28293 @version=2 
Scenario: Verify save button functionality on create movement interface when node is not blocked by other users
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
When node is not blocked by other users
And I click on "EditOwnershipNode" "NewMovement" "button"
Then I should see "CreateMovement" "Create" "Interface"
When all input validations met on create movement interface
And I click on "createMovement" "submit" "button"
Then create movement interface is closed
And icon should be displayed to show the user
And time and date of the last update should be displayed
And status should be updated to "Locked"
And verify that "newMovement" "button" is "enabled"
When I selected "Tolerance​" from "editOwnershipNode" "variable" "combobox"
Then verify that "edit" "link" is "enabled" in "Ownership Node Data" grid
And verify that "delete" "link" is "enabled" in "Ownership Node Data" grid
And verify that "OwnershipNodeDetails" "ViewReport" "link" is "enabled"
And verify that "OwnershipNodeDetails" "Publish" "link" is "enabled"
And verify that "OwnershipNodeDetails" "Unlock" "link" is "enabled"
And verify that "OwnershipNodeDetails" "SubmitToApproval" "link" is "disabled"

@testcase=28294 @version=2 @manual
Scenario: Verify save button functionality on create movement interface when node is blocked by other user
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
And I click on "EditOwnershipNode" "NewMovement" "button"
Then I should see "CreateMovement" "Create" "Interface"
When all input validations met on create movement interface
When node is blocked by other user
And I click on "createMovement" "submit" "button"
Then create movement interface is closed
And I should see error label "No se pueden realizar cambios"
And I should see error message "El balance está siendo modificado por [Nombre del usuario]"
And icon should be displayed to show the user
And time and date of the last update should be displayed
And status should be updated to "Locked"
And verify that "newMovement" "button" is "disabled"
When I selected "Tolerance​" from "editOwnershipNode" "variable" "combobox"
Then verify that "edit" "link" is "disabled" in "Ownership Node Data" grid
And verify that "delete" "link" is "disabled" in "Ownership Node Data" grid
And verify that "OwnershipNodeDetails" "ViewReport" "link" is "enabled"
And verify that "OwnershipNodeDetails" "Publish" "link" is "disabled"
And verify that "OwnershipNodeDetails" "Unlock" "link" is "disabled"
And verify that "OwnershipNodeDetails" "SubmitToApproval" "link" is "disabled"

@testcase=28295 @version=2 @manual
Scenario: verify publish functionality when there are no other types of adjustments like edit or delete ownership
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
When I click on "EditOwnershipNode" "NewMovement" "button"
Then I should see "CreateMovement" "Create" "Interface"
When all input validations met on create movement interface
And I click on "createMovement" "submit" "button"
Then create movement interface is closed
And balance summary with ownership is updated with newly created movement
When I click on "Actions" "button"
And I click on "publish" "link"
Then operational information of the movement should be stored
And ownership data of the movement should be stored
And updated date and reason for the change and comment and application user information is stored
And node status should be updated to "publishing"
And I should see the "Volumetric Balance with ownership for node" page
When publication of movements is finished
Then node status should be updated to "published"

@testcase=28296 @version=2 @manual
Scenario: verify publish functionality when there are other types of adjustments like edit or delete ownership
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
When I click on "EditOwnershipNode" "NewMovement" "button"
Then I should see "CreateMovement" "Create" "Interface"
When all input validations met on create movement interface
And I click on "createMovement" "submit" "button"
Then create movement interface is closed
And balance summary with ownership is updated with newly created movement
When I have other type adjustments like edit or delete ownership
And I click on "Actions" "button"
And I click on "publish" "link"
Then those adjustments should be processed