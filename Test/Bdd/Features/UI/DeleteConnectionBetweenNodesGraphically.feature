@sharedsteps=4013 @owner=jagudelos @ui @testsuite=35678 @testplan=35673
Feature: DeleteConnectionBetweenNodesGraphically
As an application administrator
I need to graphically delete a connection between nodes to update the current network configuration and update some connections properties

Background: Login
Given I am logged in as "admin"
@testcase=37336
Scenario Outline: Verify different node connection properties
Given I have data configured for "segment" network
And I have "<Type>" connection created between the nodes
When I hover over on "Connection" "link"
And I click on "ModifyProperties" "button"
Then I verify that "<Property>" "Connection" "button" is "enabled"
When I hover over on "<Property>" "Connection" "button"
Then I should see the message "<Message>"

Examples:
| Property | Type     | Message              |
| Inactive | Active   | Activar Connection   |
| Active   | Inactive | Inactivar Connection |
| Delete   | Active   | Eliminar Connection  |
@testcase=37337
Scenario Outline: Verify connection button is disabled for different node connection properties
Given I have data configured for "segment" network
And I have "<Type>" connection created between the nodes
When I hover over on "Connection" "link"
And I click on "ModifyProperties" "button"
Then I verify that "<Property>" "Connection" "button" is "disabled"

Examples:
| Property | Type     |
| Active   | Active   |
| Inactive | Inactive |
| Delete   | Active   |
@testcase=37338
Scenario: Verify modify properties button is disabled
Given I have data configured for "segment" network
And I have "active" connection created between the nodes
When I hover over on "Connection" "link"
And I click on "ModifyProperties" "button"
Then I verify that "ModifyProperties" "button" is "disabled"
@testcase=37339
Scenario Outline: Update node connection properties
Given I have data configured for "segment" network
And I have "<Type>" connection created between the nodes
When I hover over on "Connection" "link"
And I click on "ModifyProperties" "button"
And I click on "<Property>" "connection" "button"
And I hover on "<Property>" "connection" "button"
And I click on "Accept" "button"
Then it should be updated to "<Property>"

Examples:
| Property | Type     |
| Inactive | Active   |
| Active   | Inactive |
| Delete   | Active   |
@testcase=37340
Scenario: Update the node connection as a transferpoint
Given I have data configured for "segment" network
And I have active connection "wihout" transferpoint
When I hover over on "Connection" "link"
And I click on "TransferPoint" "button"
And I hover on "TransferPoint" "connection" "button"
And I click on "TransferPoint" "connection" "button"
And I select "TransferPoint" from "Predictive Analytical Model" "dropdown"
And I click on "Accept" "button"
Then it should be updated to "transaferpoint"
@testcase=37341
Scenario: Verify that transfer point conncetion button is disabled when the node is marked as transferpoint
Given I have data configured for "segment" network
And I have active connection "with" transferpoint
When I hover over on "Connection" "link"
And I click on "TransferPoint" "button"
Then I I verify that "ModifyProperties" "button" is "disabled"
@testcase=37342
Scenario: Verify the analytical model interface
Given I have data configured for "segment" network
And I have active connection wihout transferpoint
When I hover over on "Connection" "link"
And I click on "TransferPoint" "button"
And I hover on "TransferPoint" "connection" "button"
And I click on "TransferPoint" "connection" "button"
Then I should see the "Edit Connection Attribute" interface
And I should see the "Predictive Analytical Model" "dropdown"
@testcase=37343
Scenario: Verify the error message when transfer point belongs to different segment
Given I have data configured for "segment" network
And I have active connection wihout transferpoint
When I hover over on "Connection" "link"
And I click on "TransferPoint" "button"
And I hover on "TransferPoint" "connection" "button"
And I click on "TransferPoint" "connection" "button"
And I select "TransferPoint" from "Predictive Analytical Model" "dropdown"
And I click on "Accept" "button"
Then I should see the message "Los nodos de la conexión deben pertenecer a segmentos diferentes para ser marcada como punto de transferencia."
@testcase=37344
Scenario Outline: Verify that the node connection property changes are registered in Audit log
Given I have data configured for "segment" network
And I have "<Type>" connection created between the nodes
When I hover over on "Connection" "link"
And I click on "ModifyProperties" "button"
And I click on "<Property>" "connection" "button"
And I click on "Accept" "button"
Then I should see the changes registered in audit log

Examples:
| Property | Type     |
| Inactive | Active   |
| Active   | Inactive |
| Delete   | Active   |
@testcase=37345
Scenario: Verify that the ownership strategy changes are registered in Audit log
Given I have data configured for "segment" network
And I have active connection wihout transferpoint
When I hover over on "Connection" "link"
And I click on "TransferPoint" "button"
And I hover on "TransferPoint" "connection" "button"
And I click on "TransferPoint" "connection" "button"
And I select "TransferPoint" from "Predictive Analytical Model" "dropdown"
And I click on "Accept" "button"
Then I should see the changes registered in audit log
@testcase=37346
Scenario Outline: Verify the error message when node connection property service fails
Given I have data configured for "segment" network
And I have "<Type>" connection created between the nodes
When I hover over on "Connection" "link"
And I click on "ModifyProperties" "button"
And I click on "<Property>" "connection" "button"
And I hover on "<Property>" "connection" "button"
And I click on "Accept" "button"
Then I should see the message ""

Examples:
| Property | Type     |
| Inactive | Active   |
| Active   | Inactive |
| Delete   | Active   |
@testcase=37347 
Scenario: Verify the error message when deleting the node connection with associated movements
Given I have data configured for "segment" network
And I have "Delete" connection created between the nodes
When I hover over on "Connection" "link"
And I click on "ModifyProperties" "button"
And I click on "Delete" "connection" "button"
And I hover on "Delete" "connection" "button"
And I click on "Accept" "button"
Then I should see the message "No es posible eliminar la conexión debido a que tiene movimientos asociados"
