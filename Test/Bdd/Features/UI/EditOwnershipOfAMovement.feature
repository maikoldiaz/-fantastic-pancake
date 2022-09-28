@ui @sharedsteps=  @owner=jagudelos @ui  @testsuite=24151 @testplan=24148 @parallel=false
Feature: EditOwnershipOfAMovement
As a Balance Segment Professional User,
I need a UI to edit the ownership of a movement


@testcase=25158 @bvt @ui @bvt1.5
Scenario: Verify TRUE User is able to edit the ownership of a movement
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When I select "Equion" from "Propietario" combo box in "Edit Ownership Node" grid
When I select "Salidas" from "Variable" combo box in "Edit Ownership Node" grid
And I click on "ownershipNodeData" "edit" "link"
And I enter morethan 10 characters into "comments" "textbox"
When I select any "Reason" from "movementOwnership" "reasonForChange" "dropdown"
When I click on "movementOwnership" "AddOwner" "button"
Then I should see the edit inventory model window is closed


@testcase=25159 @bvt @ui @bvt1.5
Scenario: Verify TRUE User is able to edit the owner's volume of a movement
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When I select "Equion" from "Propietario" combo box in "Edit Ownership Node" grid
When I select "Salidas" from "Variable" combo box in "Edit Ownership Node" grid
And I click on "ownershipNodeData" "edit" "link"
When I have modified owners volume percentage
Then I should see the owners volume is auto updated
And total volume and percentage should be auto updated

@testcase=25160 @bvt @ui @bvt1.5
Scenario: Verify TRUE User is able to edit the ownership percentages of a movement
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When I select "Equion" from "Propietario" combo box in "Edit Ownership Node" grid
When I select "Salidas" from "Variable" combo box in "Edit Ownership Node" grid
And I click on "ownershipNodeData" "edit" "link"
When I have modified owners volume
Then ownership percentage is auto updated as per modifed volume
And total volume and percentage should be auto updated

@testcase=25161 @bvt @ui @bvt1.5
Scenario: Verify TRUE User is able to add owners to a movement
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When I select "Equion" from "Propietario" combo box in "Edit Ownership Node" grid
And I select "Salidas" from "Variable" combo box in "Edit Ownership Node" grid
And I click on "ownershipNodeData" "edit" "link"
And I click on "movementOwnership" "AddOwner" "button"
Then I verify I am able to add the owners

@testcase=25162 @bvt @ui @bvt1.5	
Scenario: Verify TRUE User is able to add owners to a movement but no owner are present
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When I select "Equion" from "Propietario" combo box in "Edit Ownership Node" grid
When I select "Salidas" from "Variable" combo box in "Edit Ownership Node" grid
And I click on "ownershipNodeData" "edit" "link"
And I click on "movementOwnership" "AddOwner" "button"
And I click on "movementOwnership" "AddOwner" "button"
Then I should see error message "No existen más propietarios asociados"
And I should see error message "Por favor configure los propietarios a nivel conexión-producto"


@testcase=25163 @ui @bvt @bvt1.5
Scenario Outline: Verify when TRUE user tries to save without required fields
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When I select "Equion" from "Propietario" combo box in "Edit Ownership Node" grid
When I select "Salidas" from "Variable" combo box in "Edit Ownership Node" grid
And I click on "ownershipNodeData" "edit" "link"
And I save without entering required <"fields">
When I click on "movementOwnership" "submit" "button"
Then I should see the message on interface "Requerido"
Examples:
| fields     |
| Volume     |
| Percentage |
| Reason     |
| Comments   |

@testcase=25164 @ui @bvt
Scenario: Verify when TRUE user updates movement with owners and volume doesn't match
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When I select "Equion" from "Propietario" combo box in "Edit Ownership Node" grid
When I select "Salidas" from "Variable" combo box in "Edit Ownership Node" grid
And I click on "ownershipNodeData" "edit" "link"
Then Total ownership percentage is not met 100
When I select any "Reason" from "movementOwnership" "reasonForChange" "dropdown"
And I enter morethan 10 characters into "comments" "textbox"
And I click on "movementOwnership" "submit" "button"
Then I should see the message on interface "La sumatoria del volumen de propiedad debe ser igual al volumen del inventario y la sumatoria de los porcentajes debe ser igual a 100"