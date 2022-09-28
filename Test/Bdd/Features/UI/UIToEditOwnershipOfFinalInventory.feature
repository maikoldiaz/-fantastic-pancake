@sharedsteps=16581 @owner=jagudelos @ui @testsuite=24152 @testplan=24148 @parallel=false
Feature: UIToEditOwnershipOfFinalInventory
As a Balance Segment Professional user,
I need a UI to edit the ownership of a final inventory

Background: Login
	Given I am logged in as "profesional"

@testcase=25220 @version=2
Scenario: Verify operational and ownership information of the inventory when edit inventory modal window is opened
	Given I have ownershipcalculated segment
	When I navigate to "Volumetric Balance with ownership for node" page
	When I click on "OwnershipNodes" "EditOwnership" "link"
	When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
	When I click on "OwnershipNodeData" "Edit" "link"
	Then I should see "Edit Inventory" interface
	Then I should see "Operational" information of the inventory
	And I should see "Ownership" information of the inventory

@testcase=25221 @version=2
Scenario: Verify change Reason list of the inventory when edit inventory modal window is opened
	When I navigate to "Volumetric Balance with ownership for node" page
	And I click on "OwnershipNodes" "EditOwnership" "link"
	When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
	And I click on "OwnershipNodeData" "Edit" "link"
	Then I should see "Edit Inventory" interface
	Then I should see no item is selected for InventoryOwnership ReasonForChange dropdown
	When I click on "InventoryOwnership" "ReasonForChange" "dropdown"
	Then I should see all change reasons for "InventoryOwnership" "ReasonForChange" "dropdown"

@testcase=25222 @version=2
Scenario: Verify ownership percentage corresponding to the modified volume is updated when volume is modified on edit inventory modal window and vice versa
	When I navigate to "Volumetric Balance with ownership for node" page
	And I click on "OwnershipNodes" "EditOwnership" "link"
	When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
	And I click on "OwnershipNodeData" "Edit" "link"
	Then I should see "Edit Inventory" interface
	When I click on "InventoryOwnership" "AddOwner" "button"
	When I have modified owners volume
	Then ownership percentage is auto updated as per modifed volume
	And total volume and percentage should be auto updated
	When I have modified owners volume percentage
	Then I should see the owners volume is auto updated
	And total volume and percentage should be auto updated

@testcase=25223 @version=2
Scenario: Verify Add Owners button functionality on edit inventory model window
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
	When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
	And I click on "OwnershipNodeData" "Edit" "link"
	Then I should see "Edit Inventory" interface
	When I click on "InventoryOwnership" "AddOwner" "button"
	Then default owners should be added to the grid
	And new owners must display default configured volume and percentage
	When I click on "InventoryOwnership" "AddOwner" "button"
	Then I should see error message "No existen más propietarios asociados"
	And I should see error message "Por favor configure los propietarios a nivel nodo-producto"

@testcase=25224 @bvt @version=2 @bvt1.5
Scenario: Verify save button functionality on edit inventory model window when all input validations are met
	When I navigate to "Volumetric Balance with ownership for node" page
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
	When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
	And I click on "OwnershipNodeData" "Edit" "link"
	Then I should see "Edit Inventory" interface
	When I click on "InventoryOwnership" "submit" "button"
	Then I should see the edit inventory model window is closed
	And I should see the balance summary with ownership is updated

@testcase=25225 @version=2
Scenario: Verify temporarily stored inventory ownership information on edit inventory model window
	When I navigate to "Volumetric Balance with ownership for node" page
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
	When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
	And I click on "OwnershipNodeData" "Edit" "link"
	Then I should see "Edit Inventory" interface
	When I click on "InventoryOwnership" "submit" "button"
	Then I should see the edit inventory model window is closed
	And I should see the balance summary with ownership is updated
	When I click on "OwnershipNodeData" "Edit" "link"
	Then I should see "Edit Inventory" "Interface"
	And I should see temporarily stored information for that inventory

@testcase=25226 @version=2
Scenario: Verify save button functionality on edit inventory model window when all input validations are not met
	When I navigate to "Volumetric Balance with ownership for node" page
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
	When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
	Then I should see "Edit Inventory" interface
	When I click on "OwnershipNodeData" "Edit" "link"
	And I click on "InventoryOwnership" "submit" "button"
	Then I should see the message on interface "Requerido"

@testcase=25227 @version=2
Scenario: Verify comments textbox when user enters more than 150 characters on edit inventory model window
	When I navigate to "Volumetric Balance with ownership for node" page
	And I click on "OwnershipNodes" "EditOwnership" "link"
	When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
	And I click on "OwnershipNodeData" "Edit" "link"
	And I enter morethan 150 characters into "comments" "textbox"
	When I click on "InventoryOwnership" "submit" "button"
	Then I should see error message "Máximo 150 caracteres"

@testcase=25228 @version=2
Scenario: Verify save button functionality on edit inventory model window when total ownership percentage is not met 100
	When I navigate to "Volumetric Balance with ownership for node" page
	And I click on "OwnershipNodes" "EditOwnership" "link"
	When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
	And I click on "OwnershipNodeData" "Edit" "link"
	Then I should see "Edit Inventory" interface
	And Total ownership percentage is not met 100
	When I click on "InventoryOwnership" "submit" "button"
	Then I should see the message on interface "La sumatoria del volumen de propiedad debe ser igual al volumen del inventario y la sumatoria de los porcentajes debe ser igual a 100"

@testcase=25229 @version=2
Scenario: Verify cancel button functionality on edit inventory model window
	When I navigate to "Volumetric Balance with ownership for node" page
	When I click on "OwnershipNodes" "EditOwnership" "link"
	When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
	And I click on "OwnershipNodeData" "Edit" "link"
	Then I should see "Edit Inventory" interface
	When I click on "InventoryOwnership" "Cancel" "button"
	Then I should see the edit inventory model window is closed

@testcase=25230 @version=2
Scenario: Verify close(X) button functionality on edit inventory model window
	When I navigate to "Volumetric Balance with ownership for node" page
	When I click on "OwnershipNodes" "EditOwnership" "link"
	When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
	And I click on "OwnershipNodeData" "Edit" "link"
	Then I should see "Edit Inventory" interface
	When I click on "modal" "close" "label"
	Then I should see the edit inventory model window is closed