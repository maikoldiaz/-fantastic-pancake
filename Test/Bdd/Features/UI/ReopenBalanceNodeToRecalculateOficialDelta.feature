@sharedsteps=4013 @owner=jagudelos @ui @testsuite=70803 @testplan=70526 @MVP2and3 @S16
Feature: ReopenBalanceNodeToRecalculateOficialDelta
As a chain user, I need to reopen an approved balance node to recalculate the official deltas with a new balance version

Background:
Given  I am logged in as "admin"
@testcase=72642
Scenario: To verify button to send approval or node reopen
Given I require reopening a balance-approved node to recalculate the official deltas with a new version of the balance
When the official balance report by node page is loaded
Then Display the text “Reabrir” / "Reopen" on the button when the status of the official balance per node for the period is “Aprobado”
And Display the text “Enviar a aprobación” / "Send to approval" on the button when the official balance status per node for the period is [ “Deltas” o “Reabierto”, o “Rechazado”]
@testcase=72643
Scenario: To verify button action to reopen a balance-approved node to recalculate the official deltas with a new version of the balance
Given I require reopening a balance-approved node to recalculate the official deltas with a new version of the balance
When the official balance report by node page is loaded
And the text of the reopening button is "Reabrir"
And Click on "Reabrir" button
Then verify the node reopening action have order greater or equal than the node want to reopen.
And verify the nodes reopening are they are in "actives" and they are in the "Aprobado" state
And verify nodes are directly connected
And verify each node found in a list including the "Nodesegment" and "NodeName" of the node should be stored in the system
@testcase=72644
Scenario: To verify button action to submit for approval
Given I require reopening a balance-approved node to recalculate the official deltas with a new version of the balance
When the official balance report by node page is loaded
And the text of the reopening button is "Enviar a aprobación"
And Click on "Enviar a aprobación" button
Then verify if the node has no predecessors the current state must be changed to “Enviado a aprobación” / "Sent to Approval"
And verify if the node has predecessors and all predecessors are from another segment the current state must be changed to “Enviado a aprobación” / "Sent to Approval"
And verify if the node has predecessors and all predecessors are of the same segment it must be validated that all predecessor nodes with lower order in the chain are in the “Enviado a aprobación” / "Sent for Approval" state
@testcase=72645
Scenario: To verify Confirmation dialogue screen
Given I require reopening a balance-approved node to recalculate the official deltas with a new version of the balance
When the official balance report by node page is loaded
And the text of the reopening button is "Reabrir"
And Click on "Reabrir" button
Then verify the following feilds and labels "<values>" on the Confirmation dialogue screen
| values       |
| Node         |
| Segment      |
| Period       |
| Total Nodes  |
| Node Segment |
| Node name    |
| Note         |
And I verify the "Label" on popup as "Los siguientes nodos tienen dependencia con el nodo a reabrir. Por favor confirme si quiere reabrir todos los nodos o sólo reabrir el nodo seleccionado."
And verify "Title" as “Reapertura de nodos”
And verify the buttons "<values>" on the screen
| Cancel                      |
| Reopen node                 |
| Reopen with dependent nodes |
@testcase=72646
Scenario: To verify Justification note to reopen a node
Given I require reopening a balance-approved node to recalculate the official deltas with a new version of the balance
When the official balance report by node page is loaded
And the text of the reopening button is "Reabrir"
And Click on "Reabrir" button
Then Validate the entry of a required note is "1000 caracteres"
And verify if note is more than 1000 char then the message “Máximo 1000 caracteres” / "Maximum 1000 characters" should be displayed at the bottom of the field
@testcase=72647
Scenario: To verify Reopen only the current node
Given I require reopening a balance-approved node to recalculate the official deltas with a new version of the balance
When the official balance report by node page is loaded
And the text of the reopening button is "Reabrir"
And Click on "Reabrir" button
And Click on "Reabrir nodo" button
Then Reopen only the current node
And Display the list of nodes because the page was invoked from the node list
@testcase=72648 
Scenario: To verify Reopen the current node and dependents
Given I require reopening a balance-approved node to recalculate the official deltas with a new version of the balance
When the official balance report by node page is loaded
And the text of the reopening button is "Reabrir"
And Click on "Reabrir" button
And click on "Reabrir con nodos dependientes" button
Then Reopen the selected node and dependent list nodes
And Display the list of nodes because the page was invoked from the node list.
