@sharedsteps=16581 @owner=jagudelos @ui @testsuite=24150 @testplan=24148 @parallel=false
Feature: UIForTheOperationalBalanceWithOwnership
As a Balance Segment Professional User, I need a UI
for the operational balance with ownership to edit
the ownership for the movements and inventories (Balance Summary)

Background: Login
Given I am logged in as "profesional"

@testcase=25273 @bvt @version=2 @bvt1.5
Scenario: Verify initial page of UI to edit the balance with ownership
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see breadcrumb "balance volumétrico con propiedad por nodo"
And I should see status of the Node and list of Actions
And I should see node name, segment name and period
And I should see balance summary with ownership corresponding to ownershipnodeid

@testcase=25274 @version=2
Scenario: Verify the icon when Node exceeds the acceptable balance percentage
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When the % Control of the node is greater than the % Acceptable Balance configured for the node
Then I should see the icon where the node totals are displayed in red

@testcase=25275 @version=2
Scenario: Verify the icon when Node with the acceptable balance percentage
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When the % Control of the node is less than or equal to the % Acceptable Balance configured for the node
Then I should see the icon where the node totals are displayed in green

@testcase=25276 @version=2
Scenario: Verify View Report option enabled based on Node status
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When node state is equal to following status
| Status               |
| Propiedad            |
| Bloqueado            |
| Desbloqueado         |
| Publicado            |
| Enviado a aprobación |
| Aprobado             |
| Rechazado            |
| Reabierto            |
Then verify that "OwnershipNodeDetails" "ViewReport" "link" is "enabled"

@testcase=25277 @version=2
Scenario: Verify Publish and Unlock option enabled based on Node status
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When node state is equal to "Bloqueado"
Then verify that "OwnershipNodeDetails" "Publish" "link" is "enabled"
And verify that "OwnershipNodeDetails" "Unlock" "link" is "enabled"

@testcase=25278 @version=2
Scenario: Verify Enviado a aprobación option enabled based on Node status
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When node state is equal to below status
| Status       |
| Propiedad    |
| Desbloqueado |
| Publicado    |
| Rechazado    |
| Reabierto    |
Then verify that "OwnershipNodeDetails" "SubmitToApproval" "link" is "enabled"

@testcase=25279 @version=2
Scenario: Verify Column names in the balance summary with ownership corresponding to ownershipnodeid
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
And I should see the "Columns" on the page
| Columns      |
| Inv. Inicial |
| Entradas     |
| Salidas      |
| PI           |
| Interfases   |
| Tolerancia   |
| PNI          |
| Inv. Final   |
| Vol. Control |
| Unidad       |
| % Control    |

@testcase=25280  @version=2
Scenario: Verify text Error should be displayed in Red under % Control column when input value is equal to Zero
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When I have input value equals to zero
Then text "Error" should be displayed in red under "% Control" column

@testcase=52422 
Scenario: Verify Report is opened when user clicked on view report option
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
Then I should see the "Volumetric Balance with ownership for node" page
When node state is equal to following status
| Status               |
| Propiedad            |
| Bloqueado            |
| Desbloqueado         |
| Publicado            |
| Enviado a aprobación |
| Aprobado             |
| Rechazado            |
| Reabierto            |
And I click on "Actions" "combobox"
And I click on "OwnershipNodeDetails" "ViewReport" "link"
Then page to view the report of the operational balance with ownership by node must be displayed
