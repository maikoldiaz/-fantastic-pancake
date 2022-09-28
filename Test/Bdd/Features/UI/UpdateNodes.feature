@sharedsteps=16612 @owner=jagudelos @ui @testsuite=11321 @testplan=11317
Feature: UpdateNodes
In order to handle the Transport Network
As an application administrator
I want to update Transport Nodes

Background: Login
	Given I am authenticated as "admin"
	And I have "Node" in the system
	And I am logged in as "admin"

@testcase=12097 @ui @output=QueryAll(GetNodes) @version=3 @prodready
Scenario Outline: Update valid node name and description
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	And I update valid "<FieldValue>" into "CreateNode" "<Field>" "<control>"
	And I click on "Submit" "button"

	Examples:
		| Field       | FieldValue  | control  |
		| Name        | Name        | textbox  |
		| Description | Description | textarea |

@testcase=12098 @ui @ignore
Scenario Outline: Update valid nodeType, operator and segment
	When I navigate to "Node" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segment" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "node" in the grid
	When I click on "Nodes" "Edit" "button"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	And I change value from "<Field>"
	And I click on "Submit" "button"

	Examples:
		| Field     | FieldValue |
		| Node Type | NodeType   |
		| Operator  | Operator   |
		| Segment   | Segment    |

@testcase=12099 @ui @ignore
Scenario: Validate Save button is disabled when there is no update
	When I navigate to "Node" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segment" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "node" in the grid
	When I click on "Nodes" "Edit" "button"
	And validate that "Submit" "button" is disabled

@testcase=12100 @ui @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Update existing node name
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	And I change existing "Name" into "CreateNode" "Name" "textbox"
	Then I should see error message "El nombre del nodo ya existe"

@testcase=12101 @ui @output=QueryAll(GetNodes) @version=3 @prodready
Scenario Outline: Update node name and description with excess characters
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	When I enter "<limit>" characters into "CreateNode" "<Field>" "<ControlType>"
	Then I should see error message "<ErrorMessage>"

	Examples:
		| Field       | limit | ErrorMessage           | ControlType |
		| Name        | 151   | Máximo 150 caracteres  | textbox     |
		| Description | 1001  | Máximo 1000 caracteres | textarea    |

@testcase=12102 @ui @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Validate SAP code dropdown is disabled when Send to SAP is Inactive
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	And I click on "createNode" "sendToSAP" "toggle" to 'Inactive'
	Then validate that "CreateNode" "LogisticCenter" "dropdown" is "disabled"

@testcase=12103 @ui @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Validate SAP code dropdown is mandatory when Send to SAP is active
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	And I click on "createNode" "sendToSAP" "toggle" to 'Active'
	Then validate that "CreateNode" "LogisticCenter" "dropdown" is "enabled"

@testcase=12104 @ui @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Validate node name is mandatory
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	When I clear value into "CreateNode" "Name" "textbox"
	Then I should see error message "Requerido"

@testcase=12105 @ui @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Validate new storage location added to the existing node
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	And I click on "StorageLocations" tab
	And I click on "nodeStorageLocationGrid" "Create" "button"
	Then I should see "Create" "NodeStorageLocation" "interface"
	When I enter new "StorageLocationName" into "NodeStorageLocation" "Name" "textbox" modal
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "combobox"
	And I enter new "StorageLocationDescription" into "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "Submit" "button"
	Then I should see a "StorageLocationName" belongs to "NodeStorageLocation" in the grid
	When I click on "nodeStorageLocation" "AddProducts" "button" for 2 Product
	Then I should see "Modal" "AddProducts" "container"
	When I enter new "ProductName" into NodeStorageLocation name textbox
	Then I should see a "ProductName" belongs to "ProductsGrid" in the grid
	When I click on "addProduct" "Submit" "button"
	And I should see "1 Productos" on "nodeStorageLocation" "AddProducts" "button" for 2 Product
	And I click on "Submit" "button"

@testcase=12107 @ui @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Validate storage location SAP is mandatory when Send to SAP is active in node
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	And I click on "createNode" "sendToSAP" "toggle" to 'Active'
	And I select "1000 - PR HCT" from "Logistic Center SAP"
	When I click on "StorageLocations" tab
	And I click on "nodeStorageLocation" "Edit" "link"
	Then I should see "Update" "NodeStorageLocation" "interface"
	When I click on "NodeStorageLocation" "Submit" "button"
	Then I should see error message "Requerido"

@testcase=12108 @ui @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Validate storage location SAP is not mandatory when Send to SAP is Inactive in node
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	And I click on "createNode" "sendToSAP" "toggle" to 'Inactive'
	Then validate that "CreateNode" "LogisticCenter" "dropdown" is "disabled"
	When I click on "StorageLocations" tab
	And I click on "nodeStorageLocationGrid" "Create" "button"
	Then I should see "Create" "NodeStorageLocation" "interface"
	When I enter new "StorageLocationName" into "NodeStorageLocation" "Name" "textbox" modal
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "combobox"
	And I enter new "StorageLocationDescription" into "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "Submit" "button"
	Then I should see a "StorageLocationName" belongs to "NodeStorageLocation" in the grid

@testcase=12109 @ui @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Validate create storage location mandatory fields
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	And I click on "createNode" "sendToSAP" "toggle" to 'Inactive'
	Then validate that "CreateNode" "LogisticCenter" "dropdown" is "disabled"
	When I click on "StorageLocations" tab
	And I click on "nodeStorageLocationGrid" "Create" "button"
	Then I should see "Create" "NodeStorageLocation" "interface"
	When I enter new "StorageLocationName" into "NodeStorageLocation" "Name" "textbox" modal
	And I click on "NodeStorageLocation" "Submit" "button"
	Then I should see error message "Requerido"
	When I clear value into "NodeStorageLocation" "Name" "textbox" modal
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "combobox"
	And I click on "NodeStorageLocation" "Submit" "button"
	Then I should see error message "Requerido"

@testcase=12110 @ui @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Validate storage location name and storage location description with excess characters
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	And I click on "StorageLocations" tab
	And I click on "nodeStorageLocation" "Edit" "link"
	Then I should see "Update" "NodeStorageLocation" "interface"
	When I enter "151" characters into "NodeStorageLocation" "Name" "textbox" modal
	Then I should see error message "Máximo 150 caracteres"
	When I clear value into "NodeStorageLocation" "Name" "textbox" modal
	And I enter new "StorageLocationName" into "NodeStorageLocation" "Name" "textbox" modal
	When I enter "1001" characters into "NodeStorageLocation" "description" "textarea"
	Then I should see error message "Máximo 1000 caracteres"

@testcase=12111 @ui @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Validate products displayed as per the storage location selected
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	And I click on "createNode" "sendToSAP" "toggle" to 'Active'
	And I select "1000 - PR HCT" from "Logistic Center SAP"
	When I click on "StorageLocations" tab
	And I click on "nodeStorageLocation" "Edit" "link"
	Then I should see "Update" "NodeStorageLocation" "interface"
	When I select "1000:M001 - PR HCT : MATERIA PRIMA" from "Storage Location SAP"
	And I click on "NodeStorageLocation" "Submit" "button"
	And I should see "Adicionar Productos" on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	When I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	Then I should see "Modal" "AddProducts" "container"
	When I enter new "StorageLocationProductName" into NodeStorageLocation name textbox
	Then I should see message "Sin registros"

@testcase=12112 @ui @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Validate given products is not displayed when its not belong to selected storage location
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	And I click on "createNode" "sendToSAP" "toggle" to 'Active'
	And I select "1000 - PR HCT" from "Logistic Center SAP"
	When I click on "StorageLocations" tab
	And I click on "nodeStorageLocation" "Edit" "link"
	Then I should see "Update" "NodeStorageLocation" "interface"
	When I select "1000:M001 - PR HCT : MATERIA PRIMA" from "Storage Location SAP"
	And I click on "NodeStorageLocation" "Submit" "button"
	And I should see "Adicionar Productos" on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	When I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	Then I should see "Modal" "AddProducts" "container"
	When I enter new "InValidProductName" into NodeStorageLocation name textbox
	Then I should see message "Sin registros"

@testcase=12113 @ui @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Validate product count displayed in the storage location grid
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see a "SegmentName" belongs to "NodeGrid" in the grid
	When I click on "Nodes" "Edit" "link"
	And I enter "1" into "CreateNode" "Order" "dropdown"
	And I click on "StorageLocations" tab
	And I should see "1 Productos" on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	When I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	Then I should see "Modal" "AddProducts" "container"
	When I enter new "ProductName2" into NodeStorageLocation name textbox
	Then I should see a "ProductName2" belongs to "ProductsGrid" in the grid
	When I click on "addProduct" "Submit" "button"
	And I should see "2 Productos" on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	And I click on "Submit" "button"