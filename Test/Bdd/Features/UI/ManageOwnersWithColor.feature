@sharedsteps=4013 @owner=jagudelos @ui @testsuite=39225 @testplan=39221 @parallel=false
Feature: ManageOwnersWithColor
As a user of the TRUE application,
I require that each owner has assigned a color
to identify it within the application

Background: Login
Given I am logged in as "admin"

@testcase=41434 @bvt @bvt1.5
Scenario: Verify when the owner has a color assigned
	And the owner has a color assigned
	And I have ownershipcalculated segment
	When I verify it in "Creation of movements with ownership​" page
	And I see that the owner must appear with its assigned color
	And I click on "CreateLogistics" "Cancel" "button"
	And I verify it in "Edit ownership of a movement" page
	And I see that the owner must appear with its assigned color
	And I click on "MovementOwnership" "Cancel" "button"
	And I verify it in "Elimination of ownership of a movement" page
	And I see that the owner must appear with its assigned color
	And I click on "MovementOwnership" "Cancel" "button"
	And I verify it in "Ownership details of a movement" page
	And I see that the owner must appear with its assigned color
	And I click on "MovementOwnership" "Cancel" "button"
	And I verify it in "Edit ownership of a final inventory" page
	And I see that the owner must appear with its assigned color
	And I click on "InventoryOwnership" "Cancel" "button"
	And I verify it in "Ownership details of an inventory" page
	And I see that the owner must appear with its assigned color
	And I click on "InventoryOwnership" "Cancel" "button"
	And I verify it in "Configure attributes nodes" page
	And I see that owner must appear with its assigned color
	And I click on "NodeProducts" "ownership" "edit" "link" of a combination having or not having value
	And I see that owner must appear with its assigned color in pie chart
	And I see that owner must appear with its assigned color individually
	And I click on "NodeProducts" "Ownership" "pie" "Cancel" "button"
	And I verify it in "Configure connections attributes" page
	And I see that owner must appear with its assigned color
	And I click on "connectionProducts" "ownership" "edit" "link" of a combination having or not having value
	And I see that owner must appear with its assigned color in pie chart
	Then I see that owner must appear with its assigned color individually

@testcase=41435 @bvt @bvt1.5
Scenario: Verify when the owner does not have a color assigned
	And the owner does not have a color assigned
	And I have ownershipcalculated segment
	When I verify it in "Creation of movements with ownership​" page
	And I see that the owner must appear with the default color
	And I click on "CreateLogistics" "Cancel" "button"
	And I verify it in "Edit ownership of a movement" page
	And I see that the owner must appear with the default color
	And I click on "MovementOwnership" "Cancel" "button"
	And I verify it in "Elimination of ownership of a movement" page
	And I see that the owner must appear with the default color
	And I click on "MovementOwnership" "Cancel" "button"
	And I verify it in "Ownership details of a movement" page
	And I see that the owner must appear with the default color
	And I click on "MovementOwnership" "Cancel" "button"
	And I verify it in "Edit ownership of a final inventory" page
	And I see that the owner must appear with the default color
	And I click on "InventoryOwnership" "Cancel" "button"
	And I verify it in "Ownership details of an inventory" page
	And I see that the owner must appear with the default color
	And I click on "InventoryOwnership" "Cancel" "button"
	And I verify it in "Configure attributes nodes" page
	And I see that owner must appear with the default color
	And I click on "NodeProducts" "ownership" "edit" "link" of a combination having or not having value
	And I see that owner must appear with the default color in pie chart
	And I see that owner must appear with the default color individually
	And I click on "NodeProducts" "Ownership" "pie" "Cancel" "button"
	And I verify it in "Configure connections attributes" page
	And I see that owner must appear with the default color
	And I click on "connectionProducts" "ownership" "edit" "link" of a combination having or not having value
	And I see that owner must appear with the default color in pie chart
	Then I see that owner must appear with the default color individually