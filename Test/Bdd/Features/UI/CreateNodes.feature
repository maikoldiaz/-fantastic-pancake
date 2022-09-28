@sharedsteps= @owner=jagudelos @ui  @testsuite=11319 @testplan=11317 @ThursRun
Feature: CreateNodes
In order to handle the Transport Network
As an application administrator
I want to create Transport Nodes from UI

Background: Login
	Given I am logged in as "admin"
	And I have segment category in the system

@testcase=12034 @bvt @version=4 @prodready
Scenario: Verify that the save button is enabled after providing data into all the required fields
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "Decimal" "Order" "textbox"
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
	Then   I should see "submit" "button" as enabled
	When I click on "submit" "button"
	Then it should be registered in the system with entered data

@testcase=12035 @version=2
Scenario: Verify that the logistic centers selection is enabled when Send to SAP is active
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I click on "CreateNode" "SendToSAP" "toggle" on the UI
	Then validate that "CreateNode" "LogisticCenter" "dropdown" is "enabled"
	And I should see the code and name as list on "CreateNode" "LogisticCenter" "dropdown"

@testcase=12036 @version=3 @prodready
Scenario: Verify that the logistic centers selection is disabled when Send to SAP is Inactive
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	Then validate that "CreateNode" "LogisticCenter" "dropdown" is "disabled"

@testcase=12037 @version=3 @prodready
Scenario: Verify SAP storage location is enabled when Send to SAP is active
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	And I click on "CreateNode" "SendToSAP" "toggle" on the UI
	When I click on "StorageLocations" tab
	When I click on "NodeStorageLocationGrid" "create" "button"
	Then validate that "NodeStorageLocation" "StorageLocationType" "dropdown" is "enabled"

@testcase=12038 @version=3 @prodready
Scenario: Verify SAP storage location is empty when Send to SAP is Inactive
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I click on "StorageLocations" tab
	When I click on "NodeStorageLocationGrid" "create" "button"
	Then validate that "NodeStorageLocation" "StorageLocations" "dropdown" is "disabled"

@testcase=12039 @bvt @version=4 @prodready
Scenario: Verify that the Storage Locations belongs to the Node
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "Decimal" "Order" "textbox"
	And I click on "CreateNode" "SendToSAP" "toggle" on the UI
	And I select any "LogisticCenterValue" from "CreateNode" "LogisticCenter" "dropdown"
	And I provide the value for "CreateNode" "description" "textarea"
	When I click on "StorageLocations" tab
	When I click on "NodeStorageLocationGrid" "create" "button"
	Then I should see "Create" "NodeStorageLocation" "form"
	When I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I select any "StorageLocationSAPValue" from "NodeStorageLocation" "StorageLocations" "dropdown"
	And I click on "NodeStorageLocation" "submit" "button"
	Then I should be able to see the "StorageLocation" that belongs to the Node

@testcase=12040 @bvt @version=4
Scenario: Verify list of Products displayed as per the selected Storage Location
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "Decimal" "Order" "textbox"
	And I provide the value for "CreateNode" "description" "textarea"
	And I click on "CreateNode" "SendToSAP" "toggle" on the UI
	And I select any "LogisticCenterValue" from "CreateNode" "LogisticCenter" "dropdown"
	When I click on "StorageLocations" tab
	When I click on "NodeStorageLocationGrid" "create" "button"
	Then I should see "Create" "NodeStorageLocation" "form"
	When I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I select any "StorageLocationSAPValue" from "NodeStorageLocation" "StorageLocations" "dropdown"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	When I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	When I enter new "ProductName2" into NodeStorageLocation name textbox
	Then I should see the list of "ProductName2" belongs to selected Storage Location

@testcase=12041 @version=4 @prodready
Scenario: Verify list of Products displayed as empty when the Storage Locations are not selected
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "Decimal" "Order" "textbox"
	And I provide the value for "CreateNode" "description" "textarea"
	When I click on "StorageLocations" tab
	When I click on "NodeStorageLocationGrid" "create" "button"
	Then I should see "Create" "NodeStorageLocation" "form"
	When I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I provide the value for "NodeStorageLocation" "description" "textarea"
	And I click on "NodeStorageLocation" "submit" "button"
	When I click on "CreateNodeInterface" tab
	And I click on "CreateNode" "SendToSAP" "toggle" on the UI
	When I click on "StorageLocations" tab
	When I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	When I enter new "ProductName" into NodeStorageLocation name textbox
	Then I should see the list of Products as empty

@testcase=12042 @version=4 @prodready
Scenario: Verify that the Product list on Add Product Interface
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "Decimal" "Order" "textbox"
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
	Then I should be able to see the "ProductName" in the list

@testcase=12043 @version=4
Scenario: Verify that the Product list on Add Product Interfaces for Storage Location
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "Decimal" "Order" "textbox"
	And I click on "CreateNode" "SendToSAP" "toggle" on the UI
	And I select any "LogisticCenterValue" from "CreateNode" "LogisticCenter" "dropdown"
	And I provide the value for "CreateNode" "description" "textarea"
	When I click on "StorageLocations" tab
	When I click on "NodeStorageLocationGrid" "create" "button"
	Then I should see "Create" "NodeStorageLocation" "form"
	When I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I select any "StorageLocationSAPValue" from "NodeStorageLocation" "StorageLocations" "dropdown"
	And I click on "NodeStorageLocation" "submit" "button"
	When I click on "nodeStorageLocation" "AddProducts" "button" for 1 Product
	When I enter new "ProductName2" into NodeStorageLocation name textbox
	Then I should be able to search all the "ProductName2" belongs to the Storage Location

@testcase=12044 @version=4 @prodready
Scenario: Verify the Edit Storage Location popup should display the information of selected Storage Location
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "Decimal" "Order" "textbox"
	And I provide the value for "CreateNode" "description" "textarea"
	When I click on "StorageLocations" tab
	When I click on "NodeStorageLocationGrid" "create" "button"
	Then I should see "Create" "NodeStorageLocation" "form"
	When I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I click on "NodeStorageLocation" "submit" "button"
	And I click on "NodeStorageLocation" "Edit" "link"
	Then I should see the Information of selected storgae Location

@testcase=12045 @version=4 @prodready
Scenario: Verify that Products are displayed as per the search text
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "Decimal" "Order" "textbox"
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
	Then I should see the "ProductName" that matches the search criteria

@testcase=12046  @version=4 @prodready
Scenario: Verify that the Products added are displayed on the grid of Add Products Interface
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "Decimal" "Order" "textbox"
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
	Then I should see 1 "ProductName" in the Product list grid

@testcase=12047 @version=4 @prodready
Scenario: Verify that the Product is deleted from the grid when delete button is clicked on Add Products Interface
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide the value for "CreateNode" "name" "textbox"
	And I select any "NodeTypeValue" from "CreateNode" "type" "dropdown"
	And I select any "Operatorvalue" from "CreateNode" "operator" "dropdown"
	And I select SegmentValue from "CreateNode" "segment" "dropdown"
	And I enter "1" into "Decimal" "Order" "textbox"
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
	When I click on "Products" "delete" "link"
	Then I should see the Product deleted form the grid

@testcase=12048 @version=4 @prodready
Scenario Outline: Create Storage Location with values exceeding maximum limit
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I click on "StorageLocations" tab
	And I click on "NodeStorageLocationGrid" "create" "button"
	Then I should see "Create" "NodeStorageLocation" "form"
	When I provide value for "NodeStorageLocation" "<Field>" "<Control>" that exceeds "<Limit>" characters
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	Then I should see error message "<Message>"

	Examples:
		| Field       | Limit | Control  | Message                |
		| name        | 150   | textbox  | Máximo 150 caracteres  |
		| description | 1000  | textarea | Máximo 1000 caracteres |

@testcase=12049 @version=4 @prodready
Scenario: Create Storage Location with Storage Location Name that contains special characters other than ":", "_", "-"
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I click on "StorageLocations" tab
	And I click on "NodeStorageLocationGrid" "create" "button"
	Then I should see "Create" "NodeStorageLocation" "form"
	When I provide value for "NodeStorageLocation" "name" "textbox" that contains special characters other than expected
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	Then I should see error message "Permite números, letras, espacios y los caracteres - _ :"

@testcase=12050 @version=3 @prodready
Scenario: Create Storage Location with existing Stroage Location Name
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I click on "StorageLocations" tab
	And I click on "NodeStorageLocationGrid" "Create" "button"
	When I provide the value for "NodeStorageLocation" "name" "textbox"
	And I select any "NodeTypeValue" from "NodeStorageLocation" "StorageLocationType" "dropdown"
	And I click on "NodeStorageLocation" "Submit" "button"
	And I click on "NodeStorageLocationGrid" "Create" "button"
	When I provide existing NodeStrogeLoaction value for "NodeStorageLocation" "name" "textbox"
	And I click on "NodeStorageLocation" "Submit" "button"
	Then I should see error message "El nombre de ubicación de almacenamiento de nodo ya existe"

@testcase=12051 @version=3 @prodready
Scenario: Create Storage Location without Mandatory fields
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I click on "CreateNode" "SendToSAP" "toggle" on the UI
	When I click on "StorageLocations" tab
	And I click on "NodeStorageLocationGrid" "Create" "button"
	And I click on "NodeStorageLocation" "Submit" "button"
	Then I should see the message on interface "Requerido"

@testcase=12052 @version=3 @prodready
Scenario Outline: Create Node with values exceeding maximum limit
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide value for "CreateNode" "<Field>" "<Control>" that exceeds "<Limit>" characters
	Then I should see error message "<Message>"

	Examples:
		| Field       | Limit | Control  | Message                |
		| name        | 150   | textbox  | Máximo 150 caracteres  |
		| description | 1000  | textarea | Máximo 1000 caracteres |

@testcase=12053 @version=2 @prodready
Scenario: Create Node with Node Name that contains special characters other than ":", "_", "-"
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide value for "CreateNode" "name" "textbox" that contains special characters other than expected
	Then I should see error message "Permite números, letras, espacios y los caracteres - _ :"

@testcase=12054 @version=4 @output=QueryAll(GetNodes)
Scenario: Create Node with existing Node Name
	Given I have "Transport Nodes" in the system
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I provide existing value for "CreateNode" "name" "textbox"
	And I click on "CreateNode" "type" "dropdown"
	Then I should see error message "El nombre del nodo ya existe"

@testcase=12055 @version=3 @prodready
Scenario: Create Node without Mandatory fields
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	When I click on "CreateNode" "SendToSAP" "toggle" on the UI
	When I click on "CreateNode" "name" "textbox"
	And I click on "CreateNode" "type" "dropdown"
	And I click on "CreateNode" "operator" "dropdown"
	And I click on "CreateNode" "LogisticCenter" "dropdown"
	And I click on "CreateNode" "segment" "dropdown"
	Then I should see the message on interface "Requerido"

@testcase=12056 @version=2 @manual
Scenario Outline: Verify the filtering functionality on StorageLocation grid
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	Then I should see "Node" "Information" "form"
	When I click on "StorageLocations" tab
	Then I should see "Node" "StorageLocation" "form"
	And I provide value for "<FieldName>" "name" "textbox"
	Then I should see the records filtered as per the search criteria

	Examples:
		| FieldName   |
		| Name        |
		| Type        |
		| Description |
		| Products    |

@testcase=12057 @version=2 @manual
Scenario Outline: Verify the filtering functionality for status column on StorageLocation grid
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	Then I should see "Node" "Information" "form"
	When I click on "StorageLocations" tab
	Then I should see "Node" "StorageLocation" "form"
	And I select "<Value>" from "status" "dropdwon"
	Then I should see the records filtered as per the search criteria

	Examples:
		| Value    |
		| Active   |
		| Inactive |

@testcase=12058 @version=2 @manual
Scenario Outline: Verify Sorting functionality on StorageLocation grid
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	Then I should see "Node" "Information" "form"
	When I click on "StorageLocations" tab
	Then I should see "Node" "StorageLocation" "form"
	And I click on the "<ColumnName>"
	Then the results should be sorted according to "<ColumnName>"

	Examples:
		| ColumnName  |
		| Name        |
		| Type        |
		| Description |
		| Status      |
		| Products    |

@testcase=12059 @version=2 @manual
Scenario: Verify Pagination functionality on StorageLocation grid
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	Then I should see "Node" "Information" "form"
	When I click on "StorageLocations" tab
	Then I should see "Node" "StorageLocation" "form"
	When I change the elements count per page to 50
	Then the records count shown per page should also be 50