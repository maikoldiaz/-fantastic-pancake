@sharedsteps=  @owner=jagudelos @ui  @testsuite=14725 @testplan=14709
Feature: CreateTransformationOfMovementsAndInventories
As a Professional Segment Balance User
In order to transform movements and operational inventories
I need UI to create Transformations

Background: Login
	Given I am logged in as "profesional"

@testcase=16513
Scenario: Verify the grid when there are previous transformation configurations
	When I navigate to "TransformSettings" page
	And I find previous records available for transformation configurations
	Then I should see the records displayed on the grid

@testcase=16514
Scenario: Verify the grid when there are no previous transformation configurations
	When I navigate to "TransformSettings" page
	And I do not find previous records available for transformation configurations
	Then I should see message "Sin registros"

@testcase=16515 @version=2 @prodready
Scenario: Verify the mandatory fields on Inventory Transformation Interface
	When I navigate to "TransformSettings" page
	And I click on "Inventories" tab
	When I click on "Transformation" "button"
	Then I should see "Transformation" "Inventories" "Create" "Form"
	When I do not enter values for Mandatory fields for "InventoryTransformation"
	And I click on "Element" "Submit" "button"
	Then I should see the message on interface "Requerido"

@testcase=16516 @version=2 @prodready
Scenario: Verify the mandatory fields on Movement Transformation Interface
	When I navigate to "TransformSettings" page
	When I click on "Transformation" "button"
	Then I should see "Transformation" "Movements" "Create" "Form"
	When I do not enter values for Mandatory fields for "MovementTransformation"
	And I click on "Element" "Submit" "button"
	Then I should see the message on interface "Requerido"

@testcase=16517 @version=2
Scenario: Enter duplicate Transformation for node name on Inventory Transformation Interface
    And I have "Inventory" Transformation in the system
	When I navigate to "TransformSettings" page
	When I click on "Transformation" "button"
	Then I should see "Transformation" "Inventories" "Create" "Form"
	When I provide existing value for "Origin" "Node" "Textbox" on "Inventory" Interface
	And I select any "SourceProduct" from "Origin" "SourceProduct" "dropdown" on "Inventory" Interface
	And I select any "Unit" from "Origin" "MeasurementUnit" "combobox" on "Inventory" Interface
	When I provide value for "Destination" "Node" "Textbox" on "Inventory" Interface
	And I select any "SourceProduct" from "Destination" "SourceProduct" "dropdown" on "Inventory" Interface
	And I select any "Unit" from "Destination" "MeasurementUnit" "combobox" on "Inventory" Interface
	And I click on "Element" "Submit" "button"
	Then it should be registered in the system with entered data

@testcase=16518 @version=2 @prodready
Scenario: Enter duplicate Transformation on Inventory Transformation Interface
    And I have "Inventory" Transformation in the system
	And I have "Inventory" Transformation data in the system
	When I navigate to "TransformSettings" page
	When I click on "Transformation" "button"
	Then I should see "Transformation" "Inventories" "Create" "Form"
	When I provide existing value for "Origin" "Node" "Textbox" on "Inventory" Interface
	And I select existing "SourceProduct" from "Origin" "SourceProduct" "dropdown" on "Inventory" Interface
	And I select any "Unit" from "Origin" "MeasurementUnit" "combobox" on "Inventory" Interface
	When I provide value for "Destination" "Node" "Textbox" on "Inventory" Interface
	And I select any "SourceProduct" from "Destination" "SourceProduct" "dropdown" on "Inventory" Interface
	And I select any "Unit" from "Destination" "MeasurementUnit" "combobox" on "Inventory" Interface
	And I click on "Element" "Submit" "button"
	Then I should see the duplicate message "Ya existe una Transformación origen para los valores ingresados."

@testcase=16519 @bvt @version=2 @prodready
Scenario: Save Transformation on Inventory Transformation Interface
	And I have "Inventory" Transformation data in the system
	When I navigate to "TransformSettings" page
	And I click on "Inventories" tab
	When I click on "Transformation" "button"
	Then I should see "Transformation" "Inventories" "Create" "Form"
	When I provide value for "Origin" "Node" "Textbox" on "Inventory" Interface
	And I select any "SourceProduct" from "Origin" "SourceProduct" "dropdown" on "Inventory" Interface
	When I select any "Unit" from "Origin" "MeasurementUnit" "dropdown" on "Inventory" Interface
	When I provide value for "Destination" "Node" "Textbox" on "Inventory" Interface
	And I select any "SourceProduct" from "Destination" "SourceProduct" "dropdown" on "Inventory" Interface
	And I select any "Unit" from "Destination" "MeasurementUnit" "combobox" on "Inventory" Interface
	And I click on "Element" "Submit" "button"
	Then it should "register" the "Inventory" transforamtion data in the system

@testcase=16520
Scenario: Enter duplicate Transformation for origin node name on Movement Transformation Interface
	And I have "Movement" Transformation data in the system
	When I navigate to "TransformSettings" page
	When I click on "Transformation" "button"
	Then I should see "Transformation" "Movements" "Create" "Form"
	When I provide existing value for "Origin" "Node" "Textbox" on "Movement" Interface
	And I select any "NodeDestination" from "Origin" "DestinationNode" "dropdown" on "Movement" Interface
	And I select any "SourceProduct" from "Origin" "SourceProduct" "dropdown" on "Movement" Interface
	And I select existing "DestinationProduct" from "Origin" "DestinationProduct" "dropdown" on "Movement" Interface
	When I select any "Unit" from "Origin" "MeasurementUnit" "dropdown" on "Movement" Interface
	When I provide value for "Destination" "NodeOrigin" "Textbox" on "Movement" Interface
	And I select any "NodeDestination" from "Destination" "DestinationNode" "dropdown" on "Movement" Interface
	And I select any "SourceProduct" from "Destination" "SourceProduct" "dropdown" on "Movement" Interface
	And I select any "DestinationProduct" from "Destination" "DestinationProduct" "dropdown" on "Movement" Interface
	When I select any "Unit" from "Destination" "MeasurementUnit" "dropdown" on "Movement" Interface
	And I click on "Element" "Submit" "button"
	Then it should be registered in the system with entered data

@testcase=16521 @version=2 @prodready
Scenario: Enter duplicate Transformation on Movement Transformation Interface
    And I have "Movement" Transformation in the system
	And I have "Movement" Transformation data in the system
	When I navigate to "TransformSettings" page
	When I click on "Transformation" "button"
	Then I should see "Transformation" "Movements" "Create" "Form"
	When I provide existing value for "Origin" "Node" "Textbox" on "Movement" Interface
	And I select existing "NodeDestination" from "Origin" "DestinationNode" "dropdown" on "Movement" Interface
	And I select existing "SourceProduct" from "Origin" "SourceProduct" "dropdown" on "Movement" Interface
	And I select existing "DestinationProduct" from "Origin" "DestinationProduct" "dropdown" on "Movement" Interface
	When I select any "Unit" from "Origin" "MeasurementUnit" "dropdown" on "Movement" Interface
	When I provide value for "Destination" "NodeOrigin" "Textbox" on "Movement" Interface
	And I select any "NodeDestination" from "Destination" "DestinationNode" "dropdown" on "Movement" Interface
	And I select any "SourceProduct" from "Destination" "SourceProduct" "dropdown" on "Movement" Interface
	And I select any "DestinationProduct" from "Destination" "DestinationProduct" "dropdown" on "Movement" Interface
	When I select any "Unit" from "Destination" "MeasurementUnit" "dropdown" on "Movement" Interface
	And I click on "Element" "Submit" "button"
	Then I should see the duplicate message "Ya existe una Transformación origen para los valores ingresados."

@testcase=16522 @bvt @version=2 @prodready
Scenario: Save Transformation on Movement Transformation Interface
	And I have "Movement" Transformation data in the system
	When I navigate to "TransformSettings" page
	When I click on "Transformation" "button"
	Then I should see "Transformation" "Movements" "Create" "Form"
	When I provide value for "Origin" "Node" "Textbox" on "Movement" Interface
	And I select any "NodeDestination" from "Origin" "DestinationNode" "dropdown" on "Movement" Interface
	And I select any "SourceProduct" from "Origin" "SourceProduct" "dropdown" on "Movement" Interface
	And I select any "DestinationProduct" from "Origin" "DestinationProduct" "dropdown" on "Movement" Interface
	When I select any "Unit" from "Origin" "MeasurementUnit" "dropdown" on "Movement" Interface
	When I provide value for "Destination" "NodeOrigin" "Textbox" on "Movement" Interface
	And I select any "NodeDestination" from "Destination" "DestinationNode" "dropdown" on "Movement" Interface
	And I select any "SourceProduct" from "Destination" "SourceProduct" "dropdown" on "Movement" Interface
	And I select any "DestinationProduct" from "Destination" "DestinationProduct" "dropdown" on "Movement" Interface
	When I select any "Unit" from "Destination" "MeasurementUnit" "dropdown" on "Movement" Interface
	And I click on "Element" "Submit" "button"
	Then it should "register" the "Movement" transforamtion data in the system

@testcase=16523
Scenario: Verify Pagination functionality on Movement Transformation grid
	When I navigate to "TransformSettings" page
	And I click on "Movement" "button"
	When I change the elements count per page to 50
	Then the records count shown per page should also be 50

@testcase=16524
Scenario: Verify Pagination functionality on Inventory Transformation grid
	When I navigate to "TransformSettings" page
	And I click on "Inventory" "button"
	When I change the elements count per page to 50
	Then the records count shown per page should also be 50

@testcase=16525
Scenario Outline: Verify the filtering functionality on Movement Transformation grid
	When I navigate to "TransformSettings" page
	And I click on "Movement" "button"
	And I provide value for "<FieldName>" "name" "textbox"
	Then I should see the records filtered as per the search criteria

	Examples:
		| FieldName                      |
		| Origin_NodeOrigin              |
		| Orgin_NodeDestination          |
		| Orgin_ProductOrigin            |
		| Orgin_ProductDestination       |
		| Destination_NodeOrigin         |
		| Destination_NodeDestination    |
		| Destination_ProductOrigin      |
		| Destination_ProductDestination |

@testcase=16526
Scenario Outline: Verify the filtering functionality on Inventory Transformation grid
	When I navigate to "TransformSettings" page
	And I click on "Inventory" "button"
	And I provide value for "<FieldName>" "name" "textbox"
	Then I should see the records filtered as per the search criteria

	Examples:
		| FieldName              |
		| Origin_NodeOrigin      |
		| Orgin_Product          |
		| Orgin_Unit             |
		| Destination_NodeOrigin |
		| Destination_Product    |
		| Destination_Unit       |

@testcase=16527
Scenario Outline: Verify the sorting functionality on Movement Transformation grid
	When I navigate to "TransformSettings" page
	And I click on "Movement" "button"
	And I click on the "<ColumnName>"
	Then the results should be sorted according to "<ColumnName>"

	Examples:
		| ColumnName                     |
		| Origin_NodeOrigin              |
		| Orgin_NodeDestination          |
		| Orgin_ProductOrigin            |
		| Orgin_ProductDestination       |
		| Destination_NodeOrigin         |
		| Destination_NodeDestination    |
		| Destination_ProductOrigin      |
		| Destination_ProductDestination |

@testcase=16528
Scenario Outline: Verify the sorting functionality on Inventory Transformation grid
	When I navigate to "TransformSettings" page
	And I click on "Inventory" "button"
	And I click on the "<ColumnName>"
	Then the results should be sorted according to "<ColumnName>"

	Examples:
		| ColumnName             |
		| Origin_NodeOrigin      |
		| Orgin_Product          |
		| Orgin_Unit             |
		| Destination_NodeOrigin |
		| Destination_Product    |
		| Destination_Unit       |

@testcase=16529 @version=2
Scenario: Verify that the Product list shows products associated with the selected node on Inventory Transformation Interface
	And I have "Movement" Transformation data in the system
	When I navigate to "TransformSettings" page
	When I click on "Transformation" "button"
	Then I should see "Transformation" "Movements" "Create" "Form"
	When I provide value for "Origin" "Node" "Textbox" on "Movement" Interface
	And I click on "NodeDestination" from "Origin" "DestinationNode" "dropdown" on "Movement" Interface
	Then I should see the products associated with the selected node
	When I provide value for "Destination" "NodeOrigin" "Textbox" on "Movement" Interface
	And I click on "NodeDestination" from "Destination" "DestinationNode" "dropdown" on "Movement" Interface
	Then I should see the products associated with the selected node

@testcase=16530
Scenario: Verify that the Product list shows products associated with the selected node on Movement Transformation Interface
	When I navigate to "TransformSettings" page
	When I click on "CreateTransformation" "button"
	Then I should see "Create" "MovementTransformation" "Interface"
	When I provide value for "Origin" "NodeOrigin" "Textbox"
	And I click on "ProductOrigin" from "Origin" "ProductOrigin" "combobox"
	Then I should see the products associated with the selected node
	When I select any "NodeDestination" from "Origin" "NodeDestination" "combobox"
	And I click on "ProductDestination" from "Origin" "ProductDestination" "combobox"
	Then I should see the products associated with the selected node
	When I provide value for "Destination" "NodeOrigin" "Textbox"
	And I click on "ProductOrigin" from "Destination" "ProductOrigin" "combobox"
	Then I should see the products associated with the selected node
	When I select any "NodeDestination" from "Destination" "NodeDestination" "combobox"
	And I click on "ProductDestination" from "Destination" "ProductDestination" "combobox"
	Then I should see the products associated with the selected node

@testcase=16531
Scenario: Verify that the list of destination nodes are displayed having connection with Source Node
	When I navigate to "TransformSettings" page
	When I click on "CreateTransformation" "button"
	Then I should see "Create" "MovementTransformation" "Interface"
	When I provide value for "Origin" "NodeOrigin" "Textbox"
	When I click on "NodeDestination" from "Origin" "NodeDestination" "combobox"
	Then I should see the destination nodes having connection with source node selected
	When I provide value for "Destination" "NodeOrigin" "Textbox"
	And I click on "NodeDestination" from "Destination" "NodeDestination" "combobox"
	Then I should see the destination nodes having connection with source node selected