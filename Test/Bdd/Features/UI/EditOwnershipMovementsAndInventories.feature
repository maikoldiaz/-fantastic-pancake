@sharedsteps=4013 @owner=jagudelos @ui  @testsuite=24156 @testplan=24148 @parallel=false
Feature: EditOwnershipMovementsAndInventories
	As a Balance Segment Professional User, 
	I need to edit the ownership for the movements and inventories

Background: Login
Given I am logged in as "admin"

@testcase=25147 @version=2
Scenario: Validate edit and delete is disabled for Initial Inventory variable
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I select "Inventario Inicial" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify the grid with the columns corresponding to the "Inventario Inicial", selected owner and product is displayed
And verify that "edit" "link" is "disabled" in "Ownership Node Data" grid
And verify that "delete" "link" is "disabled" in "Ownership Node Data" grid

@testcase=25148 @version=2
Scenario Outline: Validate edit, delete and new movement option is enabled for movements with Input variable
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I change the node state "<NodeState>"
When I refresh the page
And I select "Entrada" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify the grid with the columns corresponding to the "Entrada", selected owner and product is displayed
And verify that "detail" "link" is "enabled" in "Ownership Node Data" grid
And verify that "edit" "link" is "enabled" in "Ownership Node Data" grid
And verify that "delete" "link" is "enabled" in "Ownership Node Data" grid
And verify that "new movement" "button" is "enabled" in "Edit Ownership Node" page
Examples: 
| NodeState    |
| Propiedad    |
| Desbloqueado |
| Publicado    |
| Rechazado    |
| Reabierto    |

@testcase=25149 @version=2
Scenario Outline: Validate edit, delete and new movement option is enabled for movements with Output variable
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I change the node state "<NodeState>"
When I refresh the page
And I select "Salida" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify the grid with the columns corresponding to the "Salida", selected owner and product is displayed
And verify that "detail" "link" is "enabled" in "Ownership Node Data" grid
And verify that "edit" "link" is "enabled" in "Ownership Node Data" grid
And verify that "delete" "link" is "enabled" in "Ownership Node Data" grid
And verify that "new movement" "button" is "enabled" in "Edit Ownership Node" page
Examples: 
| NodeState    |
| Propiedad    |
| Desbloqueado |
| Publicado    |
| Rechazado    |
| Reabierto    |

@testcase=25150 @version=2
Scenario Outline: Validate edit, delete and new movement option is enabled for movements with Identifies Loss variable
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I change the node state "<NodeState>"
When I refresh the page
And I select "Pérdida Identificada" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify the grid with the columns corresponding to the "Pérdida Identificada", selected owner and product is displayed
And verify that "detail" "link" is "enabled" in "Ownership Node Data" grid
And verify that "edit" "link" is "enabled" in "Ownership Node Data" grid
And verify that "delete" "link" is "enabled" in "Ownership Node Data" grid
And verify that "new movement" "button" is "enabled" in "Edit Ownership Node" page
Examples: 
| NodeState    |
| Propiedad    |
| Desbloqueado |
| Publicado    |
| Rechazado    |
| Reabierto    |

@testcase=25151 @version=2
Scenario Outline: Validate edit, delete and new movement option is enabled for movements with Interfaces variable
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I change the node state "<NodeState>"
When I refresh the page
And I select "Interfase" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify the grid with the columns corresponding to the "Interfase", selected owner and product is displayed
And verify that "detail" "link" is "enabled" in "Ownership Node Data" grid
And verify that "edit" "link" is "enabled" in "Ownership Node Data" grid
And verify that "delete" "link" is "enabled" in "Ownership Node Data" grid
And verify that "new movement" "button" is "enabled" in "Edit Ownership Node" page
Examples: 
| NodeState    |
| Propiedad    |
| Desbloqueado |
| Publicado    |
| Rechazado    |
| Reabierto    |

@testcase=25152 @version=2
Scenario Outline: Validate edit, delete and new movement option is enabled for movements with Tolerance variable
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I change the node state "<NodeState>"
When I refresh the page
And I select "Tolerancia" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify the grid with the columns corresponding to the "Tolerancia", selected owner and product is displayed
And verify that "detail" "link" is "enabled" in "Ownership Node Data" grid
And verify that "edit" "link" is "enabled" in "Ownership Node Data" grid
And verify that "delete" "link" is "enabled" in "Ownership Node Data" grid
And verify that "new movement" "button" is "enabled" in "Edit Ownership Node" page
Examples: 
| NodeState    |
| Propiedad    |
| Desbloqueado |
| Publicado    |
| Rechazado    |
| Reabierto    |

@testcase=25153 @version=2
Scenario Outline: Validate edit, delete and new movement option is enabled for movements with UnIdentified Losses variable
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I change the node state "<NodeState>"
When I refresh the page
And I select "Pérdida No Identificada" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify the grid with the columns corresponding to the "Pérdida No Identificada", selected owner and product is displayed
And verify that "detail" "link" is "enabled" in "Ownership Node Data" grid
And verify that "edit" "link" is "enabled" in "Ownership Node Data" grid
And verify that "delete" "link" is "enabled" in "Ownership Node Data" grid
And verify that "new movement" "button" is "enabled" in "Edit Ownership Node" page
Examples: 
| NodeState    |
| Propiedad    |
| Desbloqueado |
| Publicado    |
| Rechazado    |
| Reabierto    |

@testcase=25154 @version=2
Scenario Outline: Validate edit, delete and new movement option is enabled for Final Inventory variable
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I change the node state "<NodeState>"
When I refresh the page
And I select "Inventario Final" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify the grid with the columns corresponding to the "Inventario Final", selected owner and product is displayed
And verify that "detail" "link" is "enabled" in "Ownership Node Data" grid
And verify that "edit" "link" is "enabled" in "Ownership Node Data" grid
And verify that "delete" "link" is "enabled" in "Ownership Node Data" grid
And verify that "new movement" "button" is "enabled" in "Edit Ownership Node" page
Examples: 
| NodeState    |
| Propiedad    |
| Desbloqueado |
| Publicado    |
| Rechazado    |
| Reabierto    |

@testcase=25155 @version=2
Scenario: Validate no records is displayed, If there are no initial inventories for a node
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Resultado Balance Volumétrico con Propiedad por Nodo" page
When I select "Inventario Inicial" from "Variable" combo box in "Edit Ownership Node" grid
Then I verify "Sin registros" message in the grid 

@testcase=25156 @bvt  @version=2 @bvt1.5
Scenario Outline: Validate edit, delete and new movement option is disabled for nodes with state "Submit for Approval"/"Approved"
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When I change the node state "Enviado a aprobación"
And I refresh the page
And I select "<Variable>" from "Variable" combo box in "Edit Ownership Node" grid
Then verify that "detail" "link" is "enabled" in "Ownership Node Data" grid
And verify that "edit" "link" is "disabled" in "Ownership Node Data" grid
And verify that "delete" "link" is "disabled" in "Ownership Node Data" grid
And verify that "new movement" "button" is "disabled" in "Edit Ownership Node" page
When I change the node state "Aprobado"
And I refresh the page
Then verify that "detail" "link" is "enabled" in "Ownership Node Data" grid
And verify that "edit" "link" is "disabled" in "Ownership Node Data" grid
And verify that "delete" "link" is "disabled" in "Ownership Node Data" grid
And verify that "new movement" "button" is "disabled" in "Edit Ownership Node" page
Examples: 
| Variable                |
| Inventario Inicial      |
| Entrada                 |
| Salida                  |
| Pérdida Identificada    |
| Interfase               |
| Tolerancia              |
| Pérdida No Identificada |
| Inventario Final        |

@testcase=25157 @version=2
Scenario: Validate the default values of the Variable, Product and Owner filters in the Balance ownership per node page
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Balance volumétrico con propiedad por nodo" page
Then I verify the default value for "Product" dropdown is the first product from the "Ownership Node Balance" grid
And I verify the default value for "Variable" dropdown is "Inventario Inicial"
And I verify the default value for "Owner" dropdown is "ECOPETROL"