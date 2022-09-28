@sharedsteps=4013 @owner=jagudelos  @ui @testplan=39221 @testsuite=39230
Feature: PositioneDocumentAndSourceMovementAddedtoPropertyUpdateForms
As a Balance Segment Professional User, I need the property update forms to be modified to include the position, the document, and the source movement

@testcase=41440 @bvt
Scenario Outline: Verify in the Movement Information, Edit Movement Page and Remove Movement Page when movement type is purchase or Sell the movement origin, Order and Position values are displayed
Given I have ownershipcalculated segment calculated for movement type Purchase or Sell
When I click on "OwnershipNodes" "EditOwnership" "link"
And I select the movement
And I navigate to the specific <screen>
Then I verify that the movement Origin, Order and Position values are displayed for the selected movement

Examples:
| screen               |
| Movement Information |
| Edit Movement        |
| Delete Movement      |

@testcase=41441 @bvt
Scenario Outline: Verify in the Movement Information, Edit Movement Page and Remove Movement Page when movement type is ACE ENTRADA or ACE SALIDA only the movement origin is displayed
Given I have ownershipcalculated segment calculated for movement type ACE ENTRADA or ACE SALIDA
When I click on "OwnershipNodes" "EditOwnership" "link"
And I select the movement
And I navigate to the specific <screen>
Then I verify that the movement Origin values are displayed for the selected movement
And I verify that the Order and Position values are not displayed for the selected movement

Examples:
| screen               |
| Movement Information |
| Edit Movement        |
| Delete Movement      |

Scenario Outline: Verify in the Movement Information, Edit Movement Page and Remove Movement Page when movement type is not ACE ENTRADA/ACE SALIDA/ Compra/Venta the movement origin, Order and Position values are empty
Given I have ownershipcalculated segment calculated for movement not of type ACE ENTRADA/ACE SALIDA/Compra/Venta
When I click on "OwnershipNodes" "EditOwnership" "link"
And I select the movement
And I navigate to the specific <screen>
And I verify that the Movement Origin, Order and Position values are not displayed for the selected movement

Examples:
| screen               |
| Movement Information |
| Edit Movement        |
| Delete Movement      |
@testcase=41442
Scenario: Verify from the Edit Movement page, we can change the contract to which the movement is associated
Given I have ownershipcalculated segment calculated for movement type Compra/Purchase or Venta/Sale
When I click on "OwnershipNodes" "EditOwnership" "link"
And I select the movement
And I navigate to the specific Edit Movement Screen
Then I can verify that the combo -box Pedido-Posición is displayed
Then I can change the contract to which the movement is associated
@testcase=41443
Scenario: Verify in the Edit Movement page, only the valid list of contracts are provided to choose from
Given I have ownershipcalculated segment calculated for movement type Compra/Purchase or Venta/Sale
When I click on "OwnershipNodes" "EditOwnership" "link"
And I select the movement
And I navigate to the specific Edit Movement Screen
Then I can verify that the combo -box Pedido-Posición is displayed
Then I can verify that only the valid list of contracts are displayed in the list which satisfy the following "conditions"
| conditions                                                                                            |
| Different from the current contract of the movement                                                   |
| The contract type must be equal to the movement type ​                                                |
| The product of the contract must be equal to the source or destination product of the movement​       |
| The destination node of the contract must be equal to the source or destination node of the movement​ |
| The date of the movement must be between the start and end date of the contract                       |
And I verify that the contracts are displayed in the format [Order]-[Position]
And I verify that when there are no contracts to be displayed then list should contain only Seleccionar/Select option
@testcase=41444
Scenario: Verify in the Edit Movement page, we cannot change the contract for movements not of type Compra/Purchase or Venta/Sale
Given I have ownershipcalculated segment calculated for movement not of type Compra/Purchase or Venta/Sale
When I click on "OwnershipNodes" "EditOwnership" "link"
And I select the movement
And I navigate to the specific Edit Movement Screen
Then I can verify that the combo -box Pedido-Posición is not displayed
@testcase=41445
Scenario: Verify in the Edit Movement page, based on the contract selected the relevant contract information must be displayed
Given I have ownershipcalculated segment calculated for movement type Compra/Purchase or Venta/Sale
When I click on "OwnershipNodes" "EditOwnership" "link"
And I select the movement
And I navigate to the specific Edit Movement Screen
Then I can verify that the combo -box Pedido-Posición is displayed
Then I can change the contract to which the movement is associated
Then I can verify the relevant <information> for the selected contract is displayed
| information              |
| Fecha inicial/Start date |
| Fecha final/End date     |
| Propietario 1/Owner 1    |
| Propietario 2/Owner 2    |
| Valor/Value              |
| Unidad/Unit              |
@testcase=41446
Scenario: Verify in the Edit Movement page, when we select a new contract and click on Save the changes are updated
Given I have ownershipcalculated segment calculated for movement type Compra/Purchase or Venta/Sale
When I click on "OwnershipNodes" "EditOwnership" "link"
And I select the movement
And I navigate to the specific Edit Movement Screen
Then I can verify that the combo -box Pedido-Posición is displayed
Then I can change the contract to which the movement is associated
Then I click on the Save button
Then I can verify that the contract details are updated in the database
And I can verify that the previous contract details are inactivated

Scenario: Verify in the New Movement Page, when the user selects the variable UNIDENTIFIED LOSS and chooses the movement type Compra/Venta, the available list of contracts to choose from, are displayed
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
And I click on "EditOwnershipNode" "NewMovement" "button"
And the user selects variable UNIDENTIFIED loss
And the user selects movement type Compra/Venta
Then I can verify that the combo -box Pedido-Posición is displayed
Then I can verify that only the valid list of contracts are displayed in the list which satisfy the following "conditions"
| conditions                                                                                            |
| The contract type must be equal to the movement type ​                                                |
| The product of the contract must be equal to the source or destination product of the movement​       |
| The destination node of the contract must be equal to the source or destination node of the movement​ |
| The date of the movement must be between the start and end date of the contract                       |
And verify that if there are no valid contracts present then we should only see the Seleccionar/Select option
@testcase=41447
Scenario: Verify in the New Movement Page, based on selections made in the UI, the Pedido Posicion list must be updated
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
And I click on "EditOwnershipNode" "NewMovement" "button"
And the user selects any variable
And the user makes changes to "control"
| control                              |
| Nodo origen/Source node              |
| Nodo destino/Destination node        |
| Producto origen/Source product       |
| Producto destino/Destination product |
| Tipo movimiento/Movement type        |
Then I can verify that the combo -box Pedido-Posición is updated based on the changes made
@testcase=41448
Scenario: Verify in the New Movement page, based on the contract selected the relevenat contract information must be displayed
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
And I click on "EditOwnershipNode" "NewMovement" "button"
Then I can verify that the combo -box Pedido-Posición is displayed
And I can change the contract to which the movement is associated
And I can verify the relevant <information> for the selected contract is displayed
| information              |
| Fecha inicial/Start date |
| Fecha final/End date     |
| Propietario 1/Owner 1    |
| Propietario 2/Owner 2    |
| Valor/Value              |
| Unidad/Unit              |
@testcase=41449 
Scenario: Verify in the New Movement page, when we select a new contract and click on Save the changes are updated
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
And I click on "EditOwnershipNode" "NewMovement" "button"
Then I can verify that the combo -box Pedido-Posición is displayed
And verify that the message Requerido/Required is displayed when user does not select any item from the list
When I select a contract from the list displayed
And I provide all the required data
Then I click on the Save button
Then I can verify that the contract details are updated in the database
