@sharedsteps=16581 @owner=jagudelos @ui @testplan=24148 @testsuite=24158 @parallel=false
Feature: ReopenAnApprovedNode
In order to edit the ownership
As a Balance Segment Professional User
I want to reopen the ticket of an approved node

Background: Login
	Given I am logged in as "profesional"

@testcase=25206 @version=2
Scenario: 02 Initial page load of a node in approved state
	When I navigate to "Volumetric Balance with ownership for node" page
	When I search for a record with "Approved" status
	And I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should see reopen option next to node state
	And I should see the tooltip "Reopen" text for reopen button

@testcase=25207 @version=2
Scenario: 04 Initial page load of a node in a state different than approved
	When I navigate to "Volumetric Balance with ownership for node" page
	When I search for a record with "Ownership" status
	And I click on "OwnershipNodes" "EditOwnership" "link"
	Then I should not see reopen option next to node state

@testcase=25208 @bvt @version=2 @bvt1.5
Scenario: 03 Justification note validations to reopen a ticket
	When I navigate to "Volumetric Balance with ownership for node" page
	When I search for a record with "Approved" status
	And I click on "OwnershipNodes" "EditOwnership" "link"
	And I click on reopen button
	Then I should see "Note To Reopen" interface
	When I click on "AddComment" "submit" "button"
	Then I should see error message "La nota es requerida"
	When I provide value for "AddComment" "comment" "textbox" that exceeds "1000" characters
	Then I should see error message "La nota puede contener máximo 1000 caracteres"

@testcase=25209 @bvt @version=2 @bvt1.5
Scenario: 01 Reopen a ticket node
	And I have ownership calculation data generated in the system
	When I navigate to "Volumetric Balance with ownership for node" page
	When I search for a record with "Approved" status
	And I click on "OwnershipNodes" "EditOwnership" "link"
	And I click on reopen button
	Then I should see "Note To Reopen" interface
	When I provide valid value for "AddComment" "comment" "textbox"
	And I click on "AddComment" "submit" "button"
	Then update the ticket node status to "Reopened" at the top right of the page
	And I should not see reopen option next to node state
	And I should see "NewMovement" "EditOwnershipNode" "button" as enabled
	When I select the variable in the filter corresponds to "InitialInventory"
	Then verify that "Ownership" "edit" "link" is "disabled"
	And verify that "Ownership" "delete" "link" is "disabled"
	When I select the variable in the filter corresponds to "FinalInventory"
	Then verify that "Ownership" "edit" "link" is "enabled"
	And verify that "Ownership" "delete" "link" is "disabled"
	When I select the variable in the filter different from Initial Inventory and Final Inventory
	Then verify that "Ownership" "edit" "link" is "enabled"
	And verify that "Ownership" "delete" "link" is "enabled"