@sharedsteps=4013 @owner=jagudelos @ui @testplan=31102 @testsuite=31104
Feature:OperatingBalanceApproval
I need to configure approval flows
As TRUE administrator,
to make the closure of the balance with ownership

Background: Login
Given I am logged in as "admin"
@testcase=33891
Scenario:Verify that User should be able to see the Flow Template
When I navigate to "Flow Settings" page
And I select any Flow
Then I should see Flow Template
@testcase=33892
Scenario: Verify the Flow Template deatils
When I navigate to "Flow Settings" page
And I select any Flow
Then I should see Flow Template
And I should see Http request tab
And I should see SetApprovalMail tab
And I should see Balance Table tab
And I should see Start an approval tab

@testcase=33893 @manual
Scenario: Verify the Flow Email Template sent for approval
When I Submit the Approval Request for "Balance with Ownership" for node
Then I should receive mail with subject "Aprobación del nodo [“NodeName”]para el día [“StartDate”]"
And Message Body of that mail should be "El usuario [BalanceProfessionalUserName] ha solicitado a usted aprobación del nodo [“NodeName”] para el día [“StartDate”]"
And Table in the email should contains "<Column>" Fields
| Column              |
| Product             |
| Initial Inventory   |
| Inputs              |
| Output              |
| Final Inventory     |
| Idnetified Losses   |
| Interfaces          |
| Tolearance          |
| Unidentified losses |
| Unbalance           |
And Mail should contain "Reporte Balance con propiedad" Link
And It should contain "Approve" "button"
And It should contain "Reject" "button"
And It should contain "observation" "textbox"

@testcase=33894 @manual
Scenario: Verify the Flow Template which is Approved by the Approver
When I recieve the Approval Mail for "Balance with Ownership" for node
When I click on "Approve" "button"
Then I should see the node status as "Aprobado"

@testcase=33895 @manual
Scenario: Verify the Email Content for the flow which is Approved by the Approver
When "Approver" click on the "Approve" "button"
Then I should recieve Email with subject "Aprobación del nodo [“NodeName”]  para el día [“StartDate”]"
And I should receive mail with message body "El nodo [“NodeName”] ha sido aprobado  para el día [“StartDate”]"

@testcase=33896 @manual
Scenario: Verify the Email Flow Template which is Rejected by the Approver
When I recieve the Approval Mail for "Balance with Ownership" for node
And I enter value into "observation" "textbox"
And I click on "Reject" "button"
Then I should see the node status as "Rechazado"
@testcase=33897
Scenario: Verify the Email Content for the flow which is Rejected by the Approver
When I click on "Reject" "button"
Then I should recieve Email with subject "Rechazo del nodo [“NodeName”] para el día [“StartDate”]"
And I should receive mail with message body "El nodo [“NodeName”] ha sido rechazado para el día [“StartDate”] con la siguiente observación [“ObservationText”]”

@testcase=33898
Scenario: Verify the Flow Template for failed approvals - Node status API Fail triggered to the Approval User
When I recieve the Approval Mail for "Balance with Ownership" for node
And I click on "Approve" "button"
And If an error in the call of node status API
Then I should receive mail with subject "Error en el llamado del API de aprobación de nodos"
And I should receive mail with high importance
And I should receive mail with message body "Ha ocurrido un error actualizando el estado del nodo [“NodeName”]. Causa: [“ApiResponse”], Por favor verifique la causa y relance el flujo"
@testcase=33899 
Scenario: Verify the Flow Template for failed approvals - Node status API Fail triggered to the Administrator
And I recieve the Approval Mail for "Balance with Ownership" for node
When I click on "Approve" "button"
And If an error in the call of node status API
Then I should receive mail with subject "Error en el llamado del API de aprobación de nodos"
And I should receive mail with high importance
And I should receive mail with message body "Ha ocurrido un error actualizando el estado del nodo [“NodeName”]. Causa: [“ApiResponse”], Por favor verifique la causa y relance el flujo"

