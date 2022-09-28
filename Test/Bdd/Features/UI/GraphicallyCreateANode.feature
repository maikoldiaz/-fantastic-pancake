@sharedsteps=4013 @owner=jagudelos @ui @testplan=35673 @testsuite=35677 @parallel=false
Feature: CreateANodeGraphically
As an Administrator user, I require graphically
creating a node to add it to the current  network configuration

Background: Login
Given I am logged in as "admin"

@testcase=37326 @version=2
Scenario Outline: Verify New Node button on network
	And I have data configured for "<Category>"
	When I navigate to "Graphic Network Configuration" page
	And I enter "<Category>" in the Category dropdown
	And I select category element value from dropdown
	And I enter "Todos" into Node Texbox
	And I click on "nodeFilter" "viewReport" "button"
	And I see graphic network configuration
	Then I should see that "New Node" button as "Nuevo Nodo"
	And verify that "New Node" "button" is "enabled"

	Examples:
		| Category |
		| Segment  |
		| System   |

@testcase=37327 @version=2
Scenario Outline: Verify graphical state of Unsaved Node
	And I have data configured for "<Category>"
	When I navigate to "Graphic Network Configuration" page
	And I enter "<Category>" in the Category dropdown
	And I select category element value from dropdown
	And I enter "Todos" into Node Texbox
	And I click on "nodeFilter" "viewReport" "button"
	And I see graphic network configuration
	And I click on "new node" "button"
	Then I should see node with graphical state "Unsaved Node" on the network

	Examples:
		| Category |
		| Segment  |
		| System   |

@testcase=37328 @version=2
Scenario Outline: Verify node button is disabled while creation of a node
	And I have data configured for "<Category>"
	When I navigate to "Graphic Network Configuration" page
	And I enter "<Category>" in the Category dropdown
	And I select category element value from dropdown
	And I enter "Todos" into Node Texbox
	And I click on "nodeFilter" "viewReport" "button"
	And I see graphic network configuration
	And I click on "new node" "button"
	Then I should see node with graphical state "Unsaved Node" on the network
	And verify that "new node" "button" is "disabled"

	Examples:
		| Category |
		| Segment  |
		| System   |

@testcase=37329 @version=2
Scenario Outline: Verify data is cleared on node creation form when user clicks on cancel button
	And I have data configured for "<Category>"
	When I navigate to "Graphic Network Configuration" page
	And I enter "<Category>" in the Category dropdown
	And I select category element value from dropdown
	And I enter "Todos" into Node Texbox
	And I click on "nodeFilter" "viewReport" "button"
	And I see graphic network configuration
	And I click on "new node" "button"
	And I provide the value for "CreateNode" "name" "textbox"
	And I click on "cancel" "button"
	And create node tab should be closed
	And I click on "new node" "button"
	Then data should be cleared for "CreateNode" "name" "textbox"

	Examples:
		| Category |
		| Segment  |
		| System   |

@testcase=37330 @bvt @version=2 @bvt1.5
Scenario Outline: Verify Node creation on network and graphical state of Active node
	And I have data configured for "<Category>"
	When I navigate to "Graphic Network Configuration" page
	And I enter "<Category>" in the Category dropdown
	And I select category element value from dropdown
	And I enter "Todos" into Node Texbox
	And I click on "nodeFilter" "viewReport" "button"
	And I see graphic network configuration
	And I click on "new node" "button"
	And I see node with graphical state "Unsaved Node" on the network
	And I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "Decimal" "Order" "textbox"
	And I provide the value for "CreateNode" "description" "textarea"
	And I click on "StorageLocations" tab
	And I click on "NodeStorageLocationGrid" "create" "button"
	And I should see "Create" "NodeStorageLocation" "form"
	And I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	And I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	And I enter new "ProductName" into NodeStorageLocation name textbox
	And I click on "AddProduct" "submit" "button"
	And I should see "submit" "button" as enabled
	And I click on "submit" "button"
	Then it should be registered in the system with entered data
	And created node should be shown on the graphical network
	And I should see node with graphical state "Active Node" on the network

	Examples:
		| Category |
		| Segment  |
		| System   |

@testcase=37331 @version=2 @bvt1.5
Scenario Outline: Verify Node creation on network and graphical state of Hovered node
	And I have data configured for "<Category>"
	When I navigate to "Graphic Network Configuration" page
	And I enter "<Category>" in the Category dropdown
	And I select category element value from dropdown
	And I enter "Todos" into Node Texbox
	And I click on "nodeFilter" "viewReport" "button"
	And I see graphic network configuration
	And I click on "new node" "button"
	And I see node with graphical state "Unsaved Node" on the network
	And I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "Decimal" "Order" "textbox"
	And I provide the value for "CreateNode" "description" "textarea"
	And I click on "StorageLocations" tab
	And I click on "NodeStorageLocationGrid" "create" "button"
	And I should see "Create" "NodeStorageLocation" "form"
	And I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	And I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	And I enter new "ProductName" into NodeStorageLocation name textbox
	And I click on "AddProduct" "submit" "button"
	And I should see "submit" "button" as enabled
	And I click on "submit" "button"
	Then it should be registered in the system with entered data
	And created node should be shown on the graphical network
	When I "hover over" the newly created node
	Then I should see node with graphical state "Hovered Node" on the network

	Examples:
		| Category |
		| Segment  |
		| System   |

@testcase=37332 @version=2 @bvt1.5
Scenario Outline: Verify Node creation on network and graphical state of Selected node
	And I have data configured for "<Category>"
	When I navigate to "Graphic Network Configuration" page
	And I enter "<Category>" in the Category dropdown
	And I select category element value from dropdown
	And I enter "Todos" into Node Texbox
	And I click on "nodeFilter" "viewReport" "button"
	And I see graphic network configuration
	And I click on "new node" "button"
	And I see node with graphical state "Unsaved Node" on the network
	And I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "Decimal" "Order" "textbox"
	And I provide the value for "CreateNode" "description" "textarea"
	And I click on "StorageLocations" tab
	And I click on "NodeStorageLocationGrid" "create" "button"
	And I should see "Create" "NodeStorageLocation" "form"
	And I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	And I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	And I enter new "ProductName" into NodeStorageLocation name textbox
	And I click on "AddProduct" "submit" "button"
	And I should see "submit" "button" as enabled
	And I click on "submit" "button"
	Then it should be registered in the system with entered data
	And created node should be shown on the graphical network
	When I "selected" the newly created node
	Then I should see node with graphical state "Selected Node" on the network

	Examples:
		| Category |
		| Segment  |
		| System   |

@testcase=37333 @version=2 @bvt1.5
Scenario Outline: Verify when node creation is failed
	And I have data configured for "<Category>"
	When I navigate to "Graphic Network Configuration" page
	And I enter "<Category>" in the Category dropdown
	And I select category element value from dropdown
	And I enter "Todos" into Node Texbox
	And I click on "nodeFilter" "viewReport" "button"
	And I see graphic network configuration
	And I click on "new node" "button"
	And I see node with graphical state "Unsaved Node" on the network
	And I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "2147483648" into "Decimal" "Order" "textbox"
	And I provide the value for "CreateNode" "description" "textarea"
	And I click on "StorageLocations" tab
	And I click on "NodeStorageLocationGrid" "create" "button"
	And I should see "Create" "NodeStorageLocation" "form"
	And I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	And I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	And I enter new "ProductName" into NodeStorageLocation name textbox
	And I click on "AddProduct" "submit" "button"
	And I should see "submit" "button" as enabled
	And I click on "submit" "button"
	And I should see "Reordering the node" interface
	And I click on "NodeAttributes" "Functions" "submit" "button"
	And I should see "Error" interface
	And I see this message "Se presentó un error inesperado y no fue posible guardar el nodo. Por favor, intente de nuevo más tarde" on "confirm" "message" "container"
	And I click on "confirm" "accept" "button"
	Then node creation is failed
	And created node should not be shown on the network

	Examples:
		| Category |
		| Segment  |
		| System   |

@testcase=37334 @version=2
Scenario Outline: Verify user must return to the network graphic screen on clicking on cancel button
	And I have data configured for "<Category>"
	When I navigate to "Graphic Network Configuration" page
	And I enter "<Category>" in the Category dropdown
	And I select category element value from dropdown
	And I enter "Todos" into Node Texbox
	And I click on "nodeFilter" "viewReport" "button"
	And I see graphic network configuration
	And I click on "new node" "button"
	And I see node with graphical state "Unsaved Node" on the network
	And I click on "cancel" "button"
	Then create node tab should be closed

	Examples:
		| Category |
		| Segment  |
		| System   |