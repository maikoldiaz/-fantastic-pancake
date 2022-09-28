@sharedsteps=4013 @bvt @owner=jagudelos @ui @testplan=39221 @testsuite=39223 @parallel=false
Feature: EditANodeGraphically
As an Administrator user, I need to edit a node from the graphical network configuration
to update the data of existing nodes

Background: Login
Given I am logged in as "admin"

@testcase=41392 @version=1 @bvt1.5
Scenario Outline: Validate that Order Label is displayed on the right side of a node's graphical representation
And I have data configured for "<Category>" network
When I navigate to "Graphic Network Configuration" page
And I select "<Category>" from "Category"
And I select ElementValue from "NodeFilter" "Element" "dropdown"
And I enter "Todos" into Node Texbox
And I click on "NodeFilter" "ViewReport" "button"
Then I should see the Order Label on the right side of a node's graphical representation

Examples:
| Category |
| Segmento |
| Sistema  |

@testcase=41393 @version=1
Scenario Outline: Validate that value for the Order field for a node's within graphical representation is appearing correctly
And I have data configured for "<Category>" network
When I navigate to "Graphic Network Configuration" page
And I select "<Category>" from "Category"
And I select ElementValue from "NodeFilter" "Element" "dropdown"
And I enter "Todos" into Node Texbox
And I click on "NodeFilter" "ViewReport" "button"
Then  Verify that value for the Order field for a node is appearing correctly

Examples:
| Category |
| Segmento |
| Sistema  |

@testcase=41394 @version=1
Scenario Outline: Validate the Create a Node Button should be disabled when user clicked on the Edit Node Button in Graphical Configuration
And I have data configured for "<Category>" network
When I navigate to "Graphic Network Configuration" page
And I select "<Category>" from "Category"
And I select ElementValue from "NodeFilter" "Element" "dropdown"
And I enter "Todos" into Node Texbox
And I click on "NodeFilter" "ViewReport" "button"
And I click on "edit" Node Button
Then verify that "new node" "button" is "disabled"

Examples:
| Category |
| Segmento |
| Sistema  |

@testcase=41395 @version=1
Scenario: Verify that title of edit node form in Graphical Configuration
And I have data configured for "Segmento" network
When I navigate to "Graphic Network Configuration" page
And I select "Segmento" from "Category"
And I select ElementValue from "NodeFilter" "Element" "dropdown"
And I enter "Todos" into Node Texbox
And I click on "NodeFilter" "ViewReport" "button"
And I click on "edit" Node Button
Then the title of the grid should be "Edit Node"

@testcase=41396 @version=1 @bvt1.5
Scenario Outline: Verify that User is able to inactive the node through edit node form in Graphical Configuration
And I have data configured for "<Category>" network
When I navigate to "Graphic Network Configuration" page
And I select "<Category>" from "Category"
And I select ElementValue from "NodeFilter" "Element" "dropdown"
And I enter "Todos" into Node Texbox
And I click on "NodeFilter" "ViewReport" "button"
And I click on "edit" Node Button
And I click on "create" "Active" "toggle" on the UI
And I click on "submit" "button"
Then I should see node with graphical state "InActive Node" on the network

Examples:
| Category |
| Segmento |
| Sistema  |

@testcase=41397 @version=1
Scenario: Verify that User is not able to perform any actions on Graphical Configuration until the editing of the selected node is finished.
And I have data configured for "Segmento" network
When I navigate to "Graphic Network Configuration" page
And I select "Segmento" from "Category"
And I select ElementValue from "NodeFilter" "Element" "dropdown"
And I enter "Todos" into Node Texbox
And I click on "NodeFilter" "ViewReport" "button"
And I click on "edit" Node Button
Then I "shouldn't" be able to perform any actions on graphics network
And Following message "Para continuar, ingrese la información necesaria en los campos que se encuentran más abajo." should be appear

@testcase=41398 @version=1
Scenario: Verify User must return to the network graphic screen on clicking on cancel button on Edit Node Screen
And I have data configured for "Segmento" network
When I navigate to "Graphic Network Configuration" page
And I select "Segmento" from "Category"
And I select ElementValue from "NodeFilter" "Element" "dropdown"
And I enter "Todos" into Node Texbox
And I click on "NodeFilter" "ViewReport" "button"
And I click on "edit" Node Button
And I click on "cancel" "button" without making any changes to the node
Then Edit Node tab should be closed
And I should see "newNode" "button" as enabled
And I "should" be able to perform any actions on graphics network

@testcase=41399 @version=1
Scenario Outline: Verify that in case of failure while editing the nodes through Graphical Configuration it should display the message
And I have data configured for "<Category>" network
When I navigate to "Graphic Network Configuration" page
And I select "<Category>" from "Category"
And I select ElementValue from "NodeFilter" "Element" "dropdown"
And I enter "Todos" into Node Texbox
And I click on "NodeFilter" "ViewReport" "button"
And I click on "edit" Node Button
And I enter "2147483648" into "Decimal" "Order" "textbox"
And I should see "Reordering the node" interface
And I click on "NodeAttributes" "Functions" "submit" "button"
Then I should see "Error" interface
And I see this message "Se presentó un error inesperado y no fue posible guardar el nodo. Por favor, intente de nuevo más tarde" on "confirm" "message" "container"
And I click on "confirm" "accept" "button"

Examples:
| Category |
| Segmento |
| Sistema  |

@testcase=41400 @version=1 @bvt1.5
Scenario Outline: Verify that User is able to edit the node through edit node form in Graphical Configuration
And I have data configured for "<Category>" network
When I navigate to "Graphic Network Configuration" page
And I select "<Category>" from "Category"
And I select ElementValue from "NodeFilter" "Element" "dropdown"
And I enter "Todos" into Node Texbox
And I click on "NodeFilter" "ViewReport" "button"
And I click on "edit" Node Button
And I clear value into "CreateNode" "Name" "textbox"
And I provide the value for "CreateNode" "name" "textbox"
And I enter "100" into "Decimal" "Order" "textbox"
And I provide the value for "CreateNode" "description" "textarea"
And I click on "StorageLocations" tab
And I click on "NodeStorageLocationGrid" "create" "button"
And I should see "Create" "NodeStorageLocation" "form"
And I provide the value for "NodeStorageLocation" "name" "textbox"
And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
And I provide the value for "NodeStorageLocation" "description" "textarea"
And I click on "NodeStorageLocation" "submit" "button"
And I click on "nodeStorageLocation" "AddProducts" "button" for 2 Product
And I enter new "ProductName" into NodeStorageLocation name textbox
And I click on "AddProduct" "submit" "button"
And I should see "submit" "button" as enabled
And I click on "submit" "button"
Then Edited Values for the Node should be updated into the Database
And I should see "newNode" "button" as enabled
And I "should" be able to perform any actions on graphics network
And the information updated should be displayed on the graphical network

Examples:
| Category |
| Segmento |
| Sistema  |
