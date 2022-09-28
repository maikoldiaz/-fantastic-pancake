@sharedsteps=4013 @Owner=jagudelos @ui @testplan=14709 @testsuite=14726
Feature: NodeExecutionOrder
In order to maintain the order of the transport nodes
As a TRUE Administrator
I want to User Interface to enter order

Background: Login
	Given I am logged in as "admin"
	And I have segment category in the system

@testcase=16550 @bvt @version=3 @prodready
Scenario: Create Node information with execution order number
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "combobox"
	And I select any "Operatorvalue" from "CreateNode" "operator" "combobox"
	And I select SegmentValue from "CreateNode" "segment" "combobox"
	And I provide the "value" for "CreateNode" "order" "dropdown"
	And I provide the value for "CreateNode" "description" "textarea"
	When I click on "StorageLocations" tab
	When I click on "NodeStorageLocationGrid" "create" "button"
	Then I should see "Create" "NodeStorageLocation" "form"
	When I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	When I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	When I enter new "ProductName" into NodeStorageLocation name textbox
	And I click on "AddProduct" "submit" "button"
	When I click on "submit" "button"
	Then it should be registered in the system with with execution order number

@testcase=16551 @version=2 @prodready
Scenario: Save Node information without entering the execution order number
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I click on "CreateNode" "order" "dropdown" without order number
	Then I should see error message "Requerido"

@testcase=16552 @version=2 @prodready
Scenario: Verify the submit button without entering the execution order number
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "combobox"
	And I select any "Operatorvalue" from "CreateNode" "operator" "combobox"
	And I select any "SegmentValue" from "CreateNode" "segment" "combobox"
	And I provide the value for "CreateNode" "description" "textarea"
	When I click on "StorageLocations" tab
	When I click on "NodeStorageLocationGrid" "create" "button"
	Then I should see "Create" "NodeStorageLocation" "form"
	When I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "combobox"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	When I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	When I enter new "ProductName" into NodeStorageLocation name textbox
	And I click on "AddProduct" "submit" "button"
	Then I should see "submit" "button" as disabled

@testcase=16553 @bvt @output=QueryAll(GetNodes) @version=2 @prodready
Scenario: Update the existing node with execution order number
	Given I have "Node" in the system
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I provide the "value" for "CreateNode" "order" "dropdown"
	When I click on "submit" "button"
	Then it should be updated in the system with with execution order number

@testcase=16554 @bvt @version=2 @prodready
Scenario: Enter duplicate order number for the segment and choose reorder option
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "combobox"
	And I select any "Operatorvalue" from "CreateNode" "operator" "combobox"
	And I select any "SegmentValue" from "CreateNode" "segment" "combobox"
	And I provide the "value" for "CreateNode" "order" "dropdown"
	And I provide the value for "CreateNode" "description" "textarea"
	When I click on "StorageLocations" tab
	When I click on "NodeStorageLocationGrid" "create" "button"
	Then I should see "Create" "NodeStorageLocation" "form"
	When I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "combobox"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	When I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	When I enter new "ProductName" into NodeStorageLocation name textbox
	And I click on "AddProduct" "submit" "button"
	When I click on "submit" "button"
	Then I should see "Reordering the node" interface
	When I click on "NodeAttributes" "Functions" "submit" "button"
	Then it should be registered in the system with reordered data

@testcase=16555 @version=2 @prodready
Scenario: Enter duplicate order number for the segment and don not choose reorder option
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "combobox"
	And I select any "Operatorvalue" from "CreateNode" "operator" "combobox"
	And I select any "SegmentValue" from "CreateNode" "segment" "combobox"
	And I provide the "value" for "CreateNode" "order" "dropdown"
	And I provide the value for "CreateNode" "description" "textarea"
	When I click on "StorageLocations" tab
	When I click on "NodeStorageLocationGrid" "create" "button"
	Then I should see "Create" "NodeStorageLocation" "form"
	When I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "combobox"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	When I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	When I enter new "ProductName" into NodeStorageLocation name textbox
	And I click on "AddProduct" "submit" "button"
	When I click on "submit" "button"
	Then I should see "Reordering the node" interface
	When I click on "NodeAttributes" "Functions" "cancel" "button"
	Then I should return to edit mode without save information

@testcase=16556
Scenario: Verify the messages on Reodering the node modal window
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "combobox"
	And I select any "Operatorvalue" from "CreateNode" "operator" "combobox"
	And I select any "SegmentValue" from "CreateNode" "segment" "combobox"
	And I provide existing value for "CreateNode" "ExecutionOrder"
	And I provide the value for "CreateNode" "description" "textarea"
	When I click on "StorageLocations" tab
	When I click on "NodeStorageLocationGrid" "create" "button"
	Then I should see "Create" "NodeStorageLocation" "form"
	When I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "combobox"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	When I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	When I enter new "ProductName" into NodeStorageLocation name textbox
	And I click on "AddProduct" "submit" "button"
	When I click on "submit" "button"
	Then I should see "Reordering the node" interface
	And I should see the message "El nodo {NodeName} tiene el mismo orden y segmento."
	And I should see the message "¿Desea reordenar automáticamente los siguientes nodos del segmento?"