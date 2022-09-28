@sharedsteps=16581 @owner=jagudelos @ui  @testsuite=14727 @testplan=14709
Feature: UpdateAndDeleteTheTransformation
As a Professional Segment Balance User
In order to transform movements and operational inventories
I need UI to Update/Delete Transformations

Background: Login
	Given I am logged in as "profesional"

@testcase=16602 @bvt @prodready
Scenario: Edit a Inventory Transformation
	Given I have "Inventory" Transformation in the system
	When I navigate to "TransformSettings" page
	And I click on "Inventories" tab
	When I click on "Inventories" "Edit" "link" for existing transformation
	Then I should see "Transformation" "Inventories" "Update" "Form"
	When I select any "SourceProduct" from "Destination" "SourceProduct" "dropdown" on "Inventory" Interface
	And I select any "Unit" from "Destination" "MeasurementUnit" "combobox" on "Inventory" Interface
	And I click on "Element" "Submit" "button"
	Then it should "update" the "Inventory" transforamtion data in the system

@testcase=16603 @bvt @prodready
Scenario: Edit a Movement Transformation
	Given I have "Movement" Transformation in the system
	When I navigate to "TransformSettings" page
	When I click on "Movements" "Edit" "link" for existing transformation
	Then I should see "Transformation" "Movements" "Update" "Form"
	##When I select any "NodeDestination" from "Destination" "DestinationNode" "dropdown" on "Movement" Interface
	##When I select any "SourceProduct" from "Destination" "SourceProduct" "dropdown" on "Movement" Interface
	##And I select any "DestinationProduct" from "Destination" "DestinationProduct" "dropdown" on "Movement" Interface
	When I select any "Unit" from "Destination" "MeasurementUnit" "dropdown" on "Movement" Interface
	And I click on "Element" "Submit" "button"
	Then it should "update" the "Movement" transforamtion data in the system

@testcase=16604 @bvt @prodready
Scenario: Delete a Inventory Transformation
	Given I have "Inventory" Transformation in the system
	When I navigate to "TransformSettings" page
	And I click on "Inventories" tab
	When I click on "Inventories" "Delete" "link" for existing transformation
	Then it should be deleted from the system
	And it should refresh the "Invetories" list on the grid

@testcase=16605 @bvt @prodready
Scenario: Delete a Movement Transformation
	Given I have "Movement" Transformation in the system
	When I navigate to "TransformSettings" page
	When I click on "Movements" "Delete" "link" for existing transformation
	Then it should be deleted from the system
	And it should refresh the "Movements" list on the grid

@testcase=16606
Scenario: Verify the source fields are disabled on Edit Inventory Transformation Interface
    Given I have "Inventory" Transformation in the system
	When I navigate to "TransformSettings" page
	And I click on "Inventories" tab
	When I click on "Inventories" "Edit" "link" for existing transformation
	Then I should see "Origin" "Node" "Textbox" as disabled
	And I should see "Origin" "ProductName" "combobox" as disabled
	And I should see "Origin" "Unit" "combobox" as disabled

@testcase=16607
Scenario: Verify the source fields are disabled on Edit Movement Transformation Interface
	When I navigate to "TransformSettings" page
	And I click on "Movement" "button"
	When I click on "EditTranformation" "link"
	Then I should see "Origin" "NodeOrigin" "Textbox" as disabled
	And I should see "Origin" "NodeDestination" "combobox" as disabled
	And I should see "Origin" "ProductOrigin" "combobox" as disabled
	And I should see "Origin" "ProductDestination" "combobox" as disabled
	And I should see "Origin"  "Unit" "combobox" as disabled

@testcase=16608
Scenario: Verify the mandatory fields on Edit Inventory Transformation Interface
	When I navigate to "TransformSettings" page
	And I click on "Inventory" "button"
	When I click on "EditTranformation" "link"
	Then I should see "Edit" "InventoryTransformation" "Interface"
	When I do not enter values for Mandatory fields for "InventoryTransformation"
	And I click on "Save" "button"
	Then I should see message "Requerido"

@testcase=16609
Scenario: Verify the mandatory fields on Edit Movement Transformation Interface
	When I navigate to "TransformSettings" page
	And I click on "Movement" "button"
	When I click on "EditTranformation" "link"
	Then I should see "Edit" "MovementTransformation" "Interface"
	When I do not enter values for Mandatory fields "MovementTransformation"
	And I click on "Save" "button"
	Then I should see message "Requerido"

@testcase=16610
Scenario: Verify the existing values of original Transformation on Inventory Transformation Interface
	When I navigate to "TransformSettings" page
	And I click on "Inventory" "button"
	When I click on "EditTranformation" "link"
	Then I should see the existing values of original "Inventory" Transformation

@testcase=16611
Scenario: Verify the existing values of original Transformation on Movement Transformation Interface
	When I navigate to "TransformSettings" page
	And I click on "Movement" "button"
	When I click on "EditTranformation" "link"
	Then I should see the existing values of original "Movement" Transformation