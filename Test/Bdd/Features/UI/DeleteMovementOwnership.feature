@sharedsteps=4013 @owner=jagudelos @ui @testsuite=26825 @testplan=26817 @parallel=false
Feature: DeleteMovementOwnership
As a Balance Segment Professional User,
I need UI to delete the ownership for the movements

Background: Login
	Given I am logged in as "admin"

@testcase=28199 @version=2
Scenario: Verify list of reasons displayed on Delete ownership dropdown
	When I navigate to "Volumetric Balance with ownership for node" page
	And I click on "OwnershipNodes" "EditOwnership" "link"
	When I select "Entrada" from "Variable" combo box in "Edit Ownership Node" grid
	And I click on "OwnershipNodeData" "Edit" "link"
	Then I should see no item is selected for InventoryOwnership ReasonForChange dropdown
	And "reason for change" should be loaded with their corresponding values

@testcase=28200 @bvt @version=2 @bvt1.5
Scenario: Delete the ownership movement
	When I navigate to "Volumetric Balance with ownership for node" page
	When I click on "OwnershipNodes" "EditOwnership" "link"
	When I select "Entrada" from "Variable" combo box in "Edit Ownership Node" grid
	Then verify that "delete" "link" is "enabled"
	When I click on "OwnershipNodeData" "Delete" "link"
	Then I should see the "Delete Movement" interface
	When I select any "Official Information Settings" from "DeleteOwnership" "ReasonForChange" "dropdown"
	And I enter valid value into "DeleteOwnership" "Comment" "textbox"
	And I click on "DeleteOwnership" "Submit" "button"
	Then it should be marked as deleted
	And I should see the "OwershipEdition" page
	And I should see the update summary of ownership to exclude removed movements

@testcase=28201 @version=2
Scenario: Publish the ownership movement
	Given I have ownershipcalculated segment
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see the "Result Volume Balance with Ownership by Node" interface
	When I select any "value" from "OwnershipNode" "Variable" "dropdown"
	Then verify that "delete" "link" is "enabled" in "Ownership Node Data" grid
	When I click on "delete" "Ownership" "link"
	Then I should see the "Delete Movement" interface
	When I click on "DeleteOwnership" "Reason" "dropdown"
	And I select "value" from "DeleteOwnership" "Reason" "dropdown"
	When I provide value for "AddComment" "comment" "textbox"
	And I click on "DeleteOwnership" "Save" "button"
	When I click on "Ownership" "Action" "dropdown"
	And I select "Publish" from "Ownership" "Action" "dropdown"
	Then it should be marked as deleted
	And it should store the "Reason for Change" "Comment" "DeletedDate" "User" in DB
	And I should see the "OwershipEdition" page
	Then I should see the node status updated to "Published"
	 
@testcase=28202 @version=2
Scenario: Delete the ownership movement when node is not locked
	When I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see the "Result Volume Balance with Ownership by Node" interface
	When I select any "value" from "OwnershipNode" "Variable" "dropdown"
	Then verify that "delete" "link" is "enabled" in "Ownership Node Data" grid
	When I click on "delete" "Ownership" "link"
	Then I should see the "Delete Movement" interface
	When I click on "DeleteOwnership" "Reason" "dropdown"
	And I select "value" from "DeleteOwnership" "Reason" "dropdown"
	When I provide value for "AddComment" "comment" "textbox"
	And I click on "DeleteOwnership" "Save" "button"
	When I click on "Ownership" "Action" "dropdown"
	Then it should be marked as deleted
	Then I should see the node status updated to "Locked"
	And icon should be displayed to show the user
	And time and date of the last update should be displayed
	And I should see "NewMovement" "button" as "enabled"
	And verify that "edit" "link" is "enabled"
	And verify that "delete" "link" is "enabled"
	When I click on "Actions" "button"
	Then verify that "viewReport" "link" is "enabled"
	And verify that "publish" "link" is "enabled"
	And verify that "unlock" "link" is "enabled"
	And verify that "submitForApproval" "link" is "enabled"

@testcase=28203 @version=2
Scenario: Delete the ownership movement when node is locked
	When I navigate to "Volumetric Balance with ownership for node" page
	And I log in as new user
	And I click on "OwnershipNodes" "EditOwnership" "link"
	When I select "Entrada" from "Variable" combo box in "Edit Ownership Node" grid
	And I click on "OwnershipNodeData" "Delete" "link"
	Then verify that "Delete" "link" is "enabled"
	When I click on "OwnershipNodeData" "Delete" "link"
	Then I should see the "Delete Movement" interface
	When I click on "DeleteOwnership" "Reason" "dropdown"
	And I select "value" from "DeleteOwnership" "Reason" "dropdown"
	When I provide value for "AddComment" "comment" "textbox"
	And I click on "DeleteOwnership" "Save" "button"
	Then I should see message "No se pueden realizar cambios"
	And I should see message "El balance está siendo modificado por [Nombre del usuario]"
	Then I should see the node status updated to "Locked"
	And icon should be displayed to show the user
	And time and date of the last update should be displayed
	And I should see "NewMovement" "button" as "enabled"
	And verify that "edit" "link" is "disabled"
	And verify that "delete" "link" is "disabled"
	When I click on "Actions" "button"
	Then verify that "viewReport" "link" is "enabled"
	And verify that "publish" "link" is "disabled"
	And verify that "unlock" "link" is "disabled"
	And verify that "submitForApproval" "link" is "disabled"

@testcase=28204 @version=2
Scenario: Verify the validation on Delete owmership movement modal window
	When I navigate to "Volumetric Balance with ownership for node" page
	And I click on "OwnershipNodes" "EditOwnership" "link"
	When I select "Entrada" from "Variable" combo box in "Edit Ownership Node" grid
	Then verify that "Delete" "link" is "enabled"
	When I click on "OwnershipNodeData" "Delete" "link"
	Then I should see the "Delete Movement"
	When I click on "DeleteOwnership" "Submit" "button"
	Then I should see error message "Requerido"
	When I select any "Official Information Settings" from "DeleteOwnership" "ReasonForChange" "dropdown"
	When I provide value for "AddComment" "comment" "textbox" that exceeds "1000" characters
	Then I should see error message "La nota puede contener máximo 1000 caracteres"