@ui @sharedsteps=4013 @owner=jagudelos @ui @testplan=24148 @testsuite=24153 @parallel=false
Feature: UIToViewOwnershipDetailsForMovementOrInventories
As a Balance Segment Professional User,
I need a UI to see the ownership detail of a
movement or inventory

Background: Login
Given I am logged in as "admin"

@testcase=25231 @bvt @ui @version=2 @bvt1.5
Scenario: Verify TRUE User is able to view movement information for a unedited movement
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When I select "Equion" from "Propietario" combo box in "Edit Ownership Node" grid
When I select "Salidas" from "Variable" combo box in "Edit Ownership Node" grid
And I click on "ownershipNodeData" "detail" "link"
Then I verify i am able to see details of the movement

@testcase=25232 @bvt @ui @version=2 @bvt1.5
Scenario: Verify TRUE User is able to view movement information for an edited movement
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I select "Equion" from "Propietario" combo box in "Edit Ownership Node" grid
When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
And I click on "ownershipNodeData" "detail" "link"
Then I verify i am able to see details of the movement
And I verify i am able to see the reason for change comment

@testcase=25233 @bvt @ui @version=2 @bvt1.5
Scenario: Verify TRUE User is able to view inventory information for an edited inventory
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I select "Equion" from "Propietario" combo box in "Edit Ownership Node" grid
When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
And I click on "ownershipNodeData" "detail" "link"
Then I verify i am able to see details of the inventory
And I verify i am able to see the reason for change comment

@testcase=25234 @bvt @ui @version=2 @bvt1.5
Scenario: Verify TRUE User is able to view inventory information for a unedited inventory
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I select "Equion" from "Propietario" combo box in "Edit Ownership Node" grid
When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
And I click on "ownershipNodeData" "detail" "link"
Then I verify i am able to see details of the inventory
