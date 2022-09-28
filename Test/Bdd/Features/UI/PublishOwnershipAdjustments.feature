@sharedsteps=25196 @owner=jagudelos @ui @testsuite=24159 @testplan=24148
Feature: PublishOwnershipAdjustments
As a Balance Segment Professional User,
I need to publish the ownership adjustments made to a node ticket
to definitively register the information in the system

Background: Login
	Given I am logged in as "admin"

@testcase=25197 @version=2 @manual
Scenario: Verify edited movements or inventories of ownership records are deleted when movements are edited
	Given I have onwership adjustments of movements in the system
	When I navigate to "Volumetric Balance with ownership for node" page
	And I click on "Acciones" "button"
	And I select "Publicar" "dropdown"
	Then Current ownership records of edited movements to be deleted

@testcase=25198 @version=2 @manual
Scenario: Verify new ownership distribution of movements/inventories are stored when movements are edited
	Given I have onwership adjustments of movements in the system
	When I navigate to "Volumetric Balance with ownership for node" page
	And I click on "Acciones" "button"
	And I select "Publicar" "dropdown"
	Then Verify new onwership distribution of movements or inventories are stored
	And Verify "Change Date" is stored
	And Verify "Reason for Change Date" is stored
	And Verify "Comment" is stored
	And Verify "Application User" is stored

@testcase=25199 @version=2 @manual
Scenario: Verify processing of adjustments when no other adjustments present for movements edited
	Given I have onwership adjustments of movements in the system
	When I navigate to "Volumetric Balance with ownership for node" page
	And I click on "Acciones" "button"
	And I select "Publicar" "dropdown"
	Then Verify the node state to "Desbloqueado"
	And Verify user returned to page to track ownership calculation

@testcase=25200 @version=2 @manual
Scenario: Verify state of ownership records when ownership of movements are removed
	Given I have movements with ownership removed in the system
	When I navigate to "Volumetric Balance with ownership for node" page
	And I click on "Acciones" "button"
	Then I verify ownership records are marked as deleted
	And Verify "Change Date" is stored
	And Verify "Reason for Change Date" is stored
	And Verify "Comment" is stored
	And Verify "Application User" is stored

@testcase=25201 @version=2 @manual
Scenario: Verify processing of adjustments when no other adjustments present for movements removed
	Given I have onwership adjustments of movements in the system
	When I navigate to "Volumetric Balance with ownership for node" page
	And I click on "Acciones" "button"
	And I select "Publicar" "dropdown"
	Then Verify the node state to "Desbloqueado"
	And Verify user returned to page to track ownership calculation

@testcase=25202 @version=2 @manual
Scenario: Verify when user Pulishes new movements with Ownership
	Given I have onwership adjustments of movements in the system
	When I navigate to "Volumetric Balance with ownership for node" page
	And I click on "Acciones" "button"
	And I select "Publicar" "dropdown"
	Then Verify ownership of movements data is stored

@testcase=25203 @version=2 @manual
Scenario: Verify state of ownership records when New ownership of movements are present
	Given I have movements with ownership removed in the system
	When I navigate to "Volumetric Balance with ownership for node" page
	And I click on "Acciones" "button"
	Then Verify new onwership distribution of movements or inventories are stored
	And Verify "Change Date" is stored
	And Verify "Reason for Change Date" is stored
	And Verify "Comment" is stored
	And Verify "Application User" is stored

@testcase=25204 @version=2 @manual
Scenario: Verify processing of adjustments when no other types of adjustments are present
	Given I have onwership adjustments of movements in the system
	When I navigate to "Volumetric Balance with ownership for node" page
	And I click on "Acciones" "button"
	And I select "Publicar" "dropdown"
	Then Verify the node state to "Desbloqueado"
	And Verify user returned to page to track ownership calculation