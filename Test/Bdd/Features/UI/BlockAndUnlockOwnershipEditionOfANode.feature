@sharedsteps=16581 @owner=jagudelos @ui @testplan=24148 @testsuite=24157 @manual
Feature: BlockAndUnlockOwnershipEditionOfANode
In order to make only one user can work at the same time
As a Balance Segment Professional User
I need to block and unlock ownership edition of a node

Background: Login
Given I am logged in as "profesional"

@testcase=25129 @bvt
Scenario: Block a node when editing the movement ownership
Given I am having records in Ownership Calculation Per Node page
When I navigate to update ownership of a movement page
And I click on "Save" "button"
Then the "Edit Movement" modal window should be closed
And update the node status to "Locked"
And icon should be displayed to show the user
And time and date of the last update should be displayed
And I should see "NewMovement" "button" as enabled
And verify that "edit" "link" is "enabled"
And verify that "delete" "link" is "enabled"
When I click on "Actions" "button"
Then verify that "viewReport" "link" is "enabled"
And verify that "publish" "link" is "enabled"
And verify that "unlock" "link" is "enabled"
And verify that "submitForApproval" "link" is "disabled"
@testcase=25130
Scenario: Block a node when editing the inventory ownership
Given I am having records in Ownership Calculation Per Node page
When I navigate to update ownership of an inventory page
And I click on "Save" "button"
Then the "Edit Inventory" modal window should be closed
And update the node status to "Locked"
And icon should be displayed to show the user
And time and date of the last update should be displayed
And I should see "NewMovement" "button" as enabled
And verify that "edit" "link" is "enabled"
And verify that "delete" "link" is "enabled"
When I click on "Actions" "button"
Then verify that "viewReport" "link" is "enabled"
And verify that "publish" "link" is "enabled"
And verify that "unlock" "link" is "enabled"
And verify that "submitForApproval" "link" is "disabled"
@testcase=25131
Scenario: Block a node when creating a movement with ownership
Given I am having records in Ownership Calculation Per Node page
When I navigate to create a movement with ownership page
And I click on "Save" "button"
Then the "New Movement" modal window should be closed
And update the node status to "Locked"
And icon should be displayed to show the user
And time and date of the last update should be displayed
And I should see "NewMovement" "button" as enabled
And verify that "edit" "link" is "enabled"
And verify that "delete" "link" is "enabled"
When I click on "Actions" "button"
Then verify that "viewReport" "link" is "enabled"
And verify that "publish" "link" is "enabled"
And verify that "unlock" "link" is "enabled"
And verify that "submitForApproval" "link" is "disabled"
@testcase=25132
Scenario: Block a node when deleting the ownership of a movement
Given I am having records in Ownership Calculation Per Node page
When I navigate to delete ownership of a movement page
And I click on "delete" "button"
Then the "Delete Movement" modal window should be closed
And update the node status to "Locked"
And icon should be displayed to show the user
And time and date of the last update should be displayed
And I should see "NewMovement" "button" as enabled
And verify that "edit" "link" is "enabled"
And verify that "delete" "link" is "enabled"
When I click on "Actions" "button"
Then verify that "viewReport" "link" is "enabled"
And verify that "publish" "link" is "enabled"
And verify that "unlock" "link" is "enabled"
And verify that "submitForApproval" "link" is "disabled"
@testcase=25133
Scenario: View a node locked by another user when loading the page
Given I am having records in Ownership Calculation Per Node page
And the node is locked by another user
When I click on "ownership" "edit" "button"
Then show the node status as "Locked"
And icon should be displayed to show the user
And time and date of the last update should be displayed
And I should see "NewMovement" "button" as disabled
And verify that "edit" "link" is "disabled"
And verify that "delete" "link" is "disabled"
When I click on "Actions" "button"
Then verify that "viewReport" "link" is "enabled"
And verify that "publish" "link" is "disabled"
And verify that "unlock" "link" is "disabled"
And verify that "submitForApproval" "link" is "disabled"
@testcase=25134
Scenario: Verify when a user enters the ownership edition page of a node-locked by it
Given I am having records in Ownership Calculation Per Node page
And the node is locked by same user
When I click on "ownership" "edit" "button"
Then show the node status as "Locked"
And icon should be displayed to show the user
And time and date of the last update should be displayed
And I should see "NewMovement" "button" as enabled
And verify that "edit" "link" is "enabled"
And verify that "delete" "link" is "enabled"
When I click on "Actions" "button"
Then verify that "viewReport" "link" is "enabled"
And verify that "publish" "link" is "enabled"
And verify that "unlock" "link" is "enabled"
And verify that "submitForApproval" "link" is "disabled"

@testcase=25135 @bvt
Scenario Outline: Update a node blocked by another user
Given I am having records in Ownership Calculation Per Node page
And the node is locked by another user
When I click on "<Field>" "<ControlType>"
Then I should see the message "No se pueden realizar cambios El balance está siendo modificado por [Nombre del usuario]"
And  show the node status as "Locked"
And icon should be displayed to show the user
And time and date of the last update should be displayed
And I should see "NewMovement" "button" as disabled
And verify that "edit" "link" is "disabled"
And verify that "delete" "link" is "disabled"
When I click on "Actions" "button"
Then verify that "viewReport" "link" is "enabled"
And verify that "publish" "link" is "disabled"
And verify that "unlock" "link" is "disabled"
And verify that "submitForApproval" "link" is "disabled"
Examples:
| Field       | ControlType |
| NewMovement | Button      |
| Edit        | Link        |
| Delete      | Link        |
@testcase=25136
Scenario: Update a node blocked by another user - update ownership of a movement​
Given I am having records in Ownership Calculation Per Node page
And the node is locked by another user
When I navigate to update ownership of a movement page
And I click on "Save" "button"
Then the "Edit Movement" modal window should be closed
Then I should see the message "No se pueden realizar cambios El balance está siendo modificado por [Nombre del usuario]"
And  show the node status as "Locked"
And icon should be displayed to show the user
And time and date of the last update should be displayed
And I should see "NewMovement" "button" as disabled
And verify that "edit" "link" is "disabled"
And verify that "delete" "link" is "disabled"
When I click on "Actions" "button"
Then verify that "viewReport" "link" is "enabled"
And verify that "publish" "link" is "disabled"
And verify that "unlock" "link" is "disabled"
And verify that "submitForApproval" "link" is "disabled"
@testcase=25137
Scenario: Update a node blocked by another user - update ownership of an inventory​
Given I am having records in Ownership Calculation Per Node page
And the node is locked by another user
When I navigate to update ownership of an inventory page
And I click on "Save" "button"
Then the "Edit Inventory" modal window should be closed
Then I should see the message "No se pueden realizar cambios El balance está siendo modificado por [Nombre del usuario]"
And  show the node status as "Locked"
And icon should be displayed to show the user
And time and date of the last update should be displayed
And I should see "NewMovement" "button" as disabled
And verify that "edit" "link" is "disabled"
And verify that "delete" "link" is "disabled"
When I click on "Actions" "button"
Then verify that "viewReport" "link" is "enabled"
And verify that "publish" "link" is "disabled"
And verify that "unlock" "link" is "disabled"
And verify that "submitForApproval" "link" is "disabled"
@testcase=25138
Scenario: Update a node blocked by another user - remove ownership of a movement
Given I am having records in Ownership Calculation Per Node page
And the node is locked by another user
When I navigate to remove ownership of a movement page
And I click on "Save" "button"
Then the "Delete Movement" modal window should be closed
Then I should see the message "No se pueden realizar cambios El balance está siendo modificado por [Nombre del usuario]"
And  show the node status as "Locked"
And icon should be displayed to show the user
And time and date of the last update should be displayed
And I should see "NewMovement" "button" as disabled
And verify that "edit" "link" is "disabled"
And verify that "delete" "link" is "disabled"
When I click on "Actions" "button"
Then verify that "viewReport" "link" is "enabled"
And verify that "publish" "link" is "disabled"
And verify that "unlock" "link" is "disabled"
And verify that "submitForApproval" "link" is "disabled"
@testcase=25139
Scenario: Update a node blocked by another user - create a movement with ownership​
Given I am having records in Ownership Calculation Per Node page
And the node is locked by another user
When I navigate to create a movement with ownership page
And I click on "Save" "button"
Then the "Create Movement" modal window should be closed
Then I should see the message "No se pueden realizar cambios El balance está siendo modificado por [Nombre del usuario]"
And  show the node status as "Locked"
And icon should be displayed to show the user
And time and date of the last update should be displayed
And I should see "NewMovement" "button" as disabled
And verify that "edit" "link" is "disabled"
And verify that "delete" "link" is "disabled"
When I click on "Actions" "button"
Then verify that "viewReport" "link" is "enabled"
And verify that "publish" "link" is "disabled"
And verify that "unlock" "link" is "disabled"
And verify that "submitForApproval" "link" is "disabled"

@testcase=25140 @bvt
Scenario: Unlock a node
Given I am having records in Ownership Calculation Per Node page
When I click on "ownership" "edit" "button"
When I click on "Actions" "button"
When I click on "Unlock" "link"
Then I should see the message "Al desbloquear se perderán los cambios no publicados"
When I click on "Cancel" "button"
Then confirmation message should be closed
When I click on "Actions" "button"
When I click on "Unlock" "link"
When I click on "Accept" "button"
Then node status should be updated to "Unlocked"
And temporarily stored changes should be deleted
And I should be navigated back to Ownership Calculation Per Node page
