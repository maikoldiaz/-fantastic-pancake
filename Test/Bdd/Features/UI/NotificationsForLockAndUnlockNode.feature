@sharedsteps=4013 @owner=jagudelos @ui @testplan=26817 @testsuite=26831
Feature: NotificationsForLockAndUnlockNode
As a Balance Segment Professional User, I need to send a notification to a user to unlock a node

Background: Login
Given I am logged in as "profesional"

@testcase=28210
Scenario: Validate the user is able to view the blocked node message when editing the inventory ownership
Given I have ownershipcalculated segment
And I am already editing the inventory ownership as "admin"
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify the grid with the columns corresponding to the "Inventario Final", selected owner and product is displayed
When I click on "ownershipNodeData" "edit" "link"
And I click on "movementOwnership" "AddOwner" "button"
And I click on "Save" "button"
Then I validate the message "No se pueden realizar cambios. El balance está siendo modificado por [Nombre del usuario]"
And verify that "Solicitar desbloqueo" "button" is "enabled"

@testcase=28211
Scenario: Validate the user is able to view the blocked node message when editing the movements ownership
Given I have ownershipcalculated segment
And I am already editing the inventory ownership as "admin"
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I select "Salida" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify the grid with the columns corresponding to the "Inventario Final", selected owner and product is displayed
When I click on "ownershipNodeData" "edit" "link"
And I click on "movementOwnership" "AddOwner" "button"
And I click on "Save" "button"
Then I validate the message "No se pueden realizar cambios. El balance está siendo modificado por [Nombre del usuario]"
And verify that "Solicitar desbloqueo" "button" is "enabled"

@testcase=28212
Scenario: Validate the notification for Request the unlocking of a node
Given I have ownershipcalculated segment
And I am already editing the inventory ownership as "admin"
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I select "Salida" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify the grid with the columns corresponding to the "Inventario Final", selected owner and product is displayed
When I click on "ownershipNodeData" "edit" "link"
And I click on "movementOwnership" "AddOwner" "button"
And I click on "Save" "button"
Then I validate the message "No se pueden realizar cambios. El balance está siendo modificado por [Nombre del usuario]"
And verify that "Solicitar desbloqueo" "button" is "enabled"
And I click on "Solicitar desbloqueo" "button"
Then I verify the notification message "[Nombre del usuario] solicita el desbloqueo del nodo [Nombre del nodo]." to "admin"

@testcase=28213 @bvt
Scenario: Validate Accept of unlocking of a node
Given I have ownershipcalculated segment
And I am already editing the inventory ownership as "admin"
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I select "Salida" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify the grid with the columns corresponding to the "Inventario Final", selected owner and product is displayed
When I click on "ownershipNodeData" "edit" "link"
And I click on "movementOwnership" "AddOwner" "button"
And I click on "Save" "button"
Then I validate the message "No se pueden realizar cambios. El balance está siendo modificado por [Nombre del usuario]"
And verify that "Solicitar desbloqueo" "button" is "enabled"
And I click on "Solicitar desbloqueo" "button"
Then I verify the notification message "[Nombre del usuario] solicita el desbloqueo del nodo [Nombre del nodo]." to "admin"
When I click on "Aceptar" button as "admin"
Then I verify the notification message "El nodo [Nombre del nodo] esta desbloqueado, la edición de la propiedad ya está permitida." to "profesional"

@testcase=28214
Scenario: : Validate Reject the unlock of a node
Given I have ownershipcalculated segment
And I am already editing the inventory ownership as "admin"
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I select "Salida" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify the grid with the columns corresponding to the "Inventario Final", selected owner and product is displayed
When I click on "ownershipNodeData" "edit" "link"
And I click on "movementOwnership" "AddOwner" "button"
And I click on "Save" "button"
Then I validate the message "No se pueden realizar cambios. El balance está siendo modificado por [Nombre del usuario]"
And verify that "Solicitar desbloqueo" "button" is "enabled"
And I click on "Solicitar desbloqueo" "button"
Then I verify the notification message "[Nombre del usuario] solicita el desbloqueo del nodo [Nombre del nodo]." to "admin"
When I click on "Rechazar" button as "admin"
Then I validate the message "No se pueden realizar cambios. El balance está siendo modificado por [Nombre del usuario]"
And verify that "Solicitar desbloqueo" "button" is "enabled"

@testcase=28215 
Scenario: Validate node is still blocked after page refresh
Given I have ownershipcalculated segment
And I am already editing the inventory ownership as "admin"
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify the grid with the columns corresponding to the "Inventario Final", selected owner and product is displayed
When I click on "ownershipNodeData" "edit" "link"
And I click on "movementOwnership" "AddOwner" "button"
And I click on "Save" "button"
Then I validate the message "No se pueden realizar cambios. El balance está siendo modificado por [Nombre del usuario]"
And verify that "Solicitar desbloqueo" "button" is "enabled"
When I refresh the page
Then I validate the message "No se pueden realizar cambios. El balance está siendo modificado por [Nombre del usuario]"
And verify that "Solicitar desbloqueo" "button" is "enabled"

