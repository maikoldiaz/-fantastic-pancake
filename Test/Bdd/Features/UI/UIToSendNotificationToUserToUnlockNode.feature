@sharedsteps=16581 @owner=jagudelos @ui @testsuite=26831 @testplan=26817
Feature: UIToSendNotificationToUserToUnlockNode
	As a Balance Segment Professional User, 
	I need to send a notification to a user 
	to unlock a node.

	Background: Login
	Given I am logged in as "profesional"

@testcase= @version=1 @ui
Scenario: Validate user is able to view blocked node message when editing ownership
	Given I navigate to "Volumetric Balance with ownership for node" page
	When I click on "OwnershipNodes" "EditOwnership" "link"
	And I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
	And I click on "OwnershipNodeData" "Edit" "link"
	And I open Ownership node in other tab
	Then I should see "El balance está siendo modificado por" message
	And I should see "Request Unlocking button"

@testcase= @version=1 @ui
Scenario: Validate notification to request unlocking of node
	Given I navigate to "Volumetric Balance with ownership for node" page
	And I should see "balance is being modified by other user" message 
	When I click on "request unlocking" button
	Then I should see notification to unlock node
	And I should see username and nodename in notification

@testcase= @version=1 @ui
Scenario: Validate accept of unlocking a node
	Given I navigate to modal window with unlock request of node
	When I click on "accept node unlock" button
	Then I navigate back to the user requested node unlock
	And I should see "El nodo esta desbloqueado, la edición de la propiedad ya está permitida" message 

@testcase= @version=1 @ui
Scenario: Validate reject the unlocking a node
	Given I navigate to modal window with unlock request of node
	When I click on "reject node unlock" button
	Then I close the modal window
