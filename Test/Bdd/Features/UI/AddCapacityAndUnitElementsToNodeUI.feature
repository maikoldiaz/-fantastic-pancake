@sharedsteps=16659 @owner=jagudelos @ui @testsuite=35682 @testplan=35673 @parallel=false
Feature: AddCapacityAndUnitFieldsToNodeUI
As TRUE system administrator,
I need to assign a node the full line
to know the capacity of the node

Background: Login
Given I am logged in as "admin"
And I have segment category in the system

@testcase=37288 @version=2
Scenario: Verify required message in create node UI when capacity field is provided without units field
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	And I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "decimal" "order" "textbox"
	And I provide the value for "CreateNode" "description" "textarea"
	And I enter "100.12" into "decimal" "capacity" "textbox"
	And I click on "StorageLocations" tab
	And I click on "NodeStorageLocationGrid" "create" "button"
	And I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	And I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	And I enter new "ProductName" into NodeStorageLocation name textbox
	And I click on "AddProduct" "submit" "button"
	And I click on "CreateNodeInterface" tab
	And I click on "submit" "button"
	Then I should see the message on interface "Requerido"

@testcase=37289 @version=2
Scenario: Verify required message in create node UI when units field is provided without capacity field
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	And I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "decimal" "order" "textbox"
	And I provide the value for "CreateNode" "description" "textarea"
	And I select any "capacityUnit" from "createNode" "unit" "dropdown"
	And I click on "StorageLocations" tab
	And I click on "NodeStorageLocationGrid" "create" "button"
	And I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	And I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	And I enter new "ProductName" into NodeStorageLocation name textbox
	And I click on "AddProduct" "submit" "button"
	And I click on "CreateNodeInterface" tab
	And I click on "submit" "button"
	Then I should see error message "Requerido"

@testcase=37290 @bvt @version=2 @bvt1.5
Scenario: Verify node is successfully created when both capacity and unit fields are provided
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	And I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "OperatorValue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "decimal" "order" "textbox"
	And I provide the value for "CreateNode" "description" "textarea"
	And I enter "100.12" into "decimal" "capacity" "textbox"
	And I select "Bbl" from "unit"
	And I click on "StorageLocations" tab
	And I click on "NodeStorageLocationGrid" "create" "button"
	And I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	And I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	And I enter new "ProductName" into NodeStorageLocation name textbox
	And I click on "AddProduct" "submit" "button"
	And I click on "submit" "button"
	Then it should be registered in the system with entered data

@testcase=37291 @version=2
Scenario: Verify capacity field in create node UI accepts 2 decimal places
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	And I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "decimal" "order" "textbox"
	And I provide the value for "CreateNode" "description" "textarea"
	And I enter "100.1234" into "decimal" "capacity" "textbox"
	And I select any "unit" from "createNode" "unit" "dropdown"
	And I click on "StorageLocations" tab
	And I click on "NodeStorageLocationGrid" "create" "button"
	And I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	And I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	And I enter new "ProductName" into NodeStorageLocation name textbox
	And I click on "AddProduct" "submit" "button"
	And I click on "submit" "button"
	Then I should see error message "capacityValidValue"

@testcase=37292 @output=QueryAll(GetNodes) @version=2
Scenario: Verify required message in edit node UI when capacity field is provided without units field
	And I have "Node" in the system
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	And I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	And I click on "Nodes" "Edit" "link"
	And I enter "100.12" into "decimal" "capacity" "textbox"
	And I enter "1" into "decimal" "order" "textbox"
	And I click on "Submit" "button"
	Then I should see the message on interface "Requerido"

@testcase=37293 @output=QueryAll(GetNodes) @version=2
Scenario: Verify required message in edit node UI when units field is provided without capacity field
	And I have "Node" in the system
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	And I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	And I click on "Nodes" "Edit" "link"
	And I select any "unit" from "createNode" "unit" "dropdown"
	And I enter "1" into "decimal" "order" "textbox"
	And I click on "Submit" "button"
	Then I should see error message "Requerido"

@testcase=37294 @output=QueryAll(GetNodes) @version=2
Scenario Outline: Verify node is successfully edited when both capacity and unit fields are provided 
	And I have "Node" in the system
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	And I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	And I click on "Nodes" "Edit" "link"
	And I enter "<CapcityValue>" into "decimal" "capacity" "textbox"
	And I select any "unit" from "createNode" "unit" "dropdown"
	And I enter "1" into "decimal" "order" "textbox"
	And I click on "Submit" "button"
	Then it should be updated in the system with "<CapcityValue>"
	Examples: 
	| CapcityValue |
	| 100.12       |

@testcase=37295 @output=QueryAll(GetNodes) @version=2
Scenario: Verify capacity field in Edit Nodes UI accepts 2 decimal places
	And I have "Node" in the system
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	And I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	And I click on "Nodes" "Edit" "link"
	And I enter "100.1234" into "decimal" "capacity" "textbox"
	And I select any "unit" from "createNode" "unit" "dropdown"
	And I enter "1" into "decimal" "order" "textbox"
	Then I should see error message "capacityValidValue"