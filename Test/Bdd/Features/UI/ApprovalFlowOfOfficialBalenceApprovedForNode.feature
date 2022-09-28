@sharedsteps=71682 @owner=jagudelos @testplan=70526 @testsuite=70807 @ui @S16 @MVP2and3
Feature: ApprovalFlowOfOfficialBalenceApprovedForNode

Background: Login
Given I am logged in as "admin"
@testcase=71683
Scenario: Verify official delta calculation approval flow is created and renamed the existing ownership approval flow
When I navigate to "Flow Settings" page
And I navigate to microsoft flows
Then I should see "Aprobación-Propiedad-Nodo-Diaria" flow
And I should see "Aprobación-Balance-Oficial-Nodo-Mensual" flow
@testcase=71684
Scenario:  Verify Data base table should have the list of approvers separated by semi-colons for each node
Given I have added the approver list in to the data base
When I fetch DB table per node
Then I should see the NodeId,Level,Approvers columns should be present
Then I should see the "Approvers" column with list of approver email ids separated by semi-colons
@testcase=71685
Scenario: Verify Official Balance approval flow template details
When I navigate to "Flow Settings" page
And I navigate to microsoft flows
And I click on "Aprobación-Balance-Oficial-Nodo-Mensual" flow
Then I should see read a message from a service bus tab with a variables to unique delta identifier for the node and period"
And I should see HTTP - Get node information  tab
And I should see a step to construct "OFFICIAL NODE BALANCE SUMMARY table"
And I should see a step to assign/fetch the level 1 node approver
And I should see Notification to the approver mail tab
Then I should see Approval Action tab
Then I should see Reject Action tab
And I should see a tab configured to trigger email to admin for general error
Then I should see a tab configured to trigger email to approver for API error
@testcase=71686
Scenario: Verify the Official Balance approval flow Email template
Given I have delta for official balance and assign approver for node
When I Submit the Approval Request for "Aprobación-Balance-Oficial-Nodo-Mensual" for node
Then I should receive mail with subject "Aprobación del balance oficial del nodo [“NodeName”] para el periodo [“StartTime | EndTime]."
And Message Body of that mail should be "El usuario [BalanceProfessionalUserName] ha solicitado a usted aprobación del nodo [“NodeName”] para el periodo [“StartTime | EndTime]."
And Mail should contain "Reporte balance oficial por nodo" Link
Then I click on "Reporte balance oficial por nodo" link
And Verify the report is opened
And It should contain "Approve" "button"
And It should contain "Reject" "button"
And It should contain "observation" "textbox"
@testcase=71687
Scenario: Verify the Official Balance approval flow Template which is Approved by the Approver
When I recieve the Approval Mail for "Aprobación-Balance-Oficial-Nodo-Mensual" for node
When I click on "Approve" "button"
Then I should see the node status as "Aprobado"
@testcase=71688
Scenario: Verify the Email Content for the Official Balance approval flow which is Approved by the Approver
When "Approver" click on the "Approve" "button"
Then I should recieve Email with subject "Aprobación balance oficial del nodo [“NodeName”] para el periodo [“StartTime | EndTime]."
And I should receive mail with message body "El nodo [“NodeName”] ha sido aprobado para el periodo [“StartTime | EndTime] con la siguiente observación [“ObservationText”]."
@testcase=71689
Scenario: Verify the Email Flow Template of Official Balance approval flow which is Rejected by the Approver
When I recieve the Approval Mail for "Aprobación-Balance-Oficial-Nodo-Mensual" for node
And I enter value into "observation" "textbox"
And I click on "Reject" "button"
Then I should see the node status as "Rechazado"
@testcase=71690
Scenario: Verify the Email Content for the Official Balance approval flow which is Rejected by the Approver
When I click on "Reject" "button"
Then I should recieve Email with subject "Rechazo balance oficial del nodo [“NodeName”] para el periodo [“StartTime | EndTime]."
And I should receive mail with message body "El nodo [“NodeName”] ha sido rechazado para el periodo [“StartTime | EndTime] con la siguiente observación [“ObservationText”]."
@testcase=71691
Scenario: Verify the Official Balance approval flow email template for API Failure triggered to the Approval User
When I recieve the Approval Mail for "Aprobación-Balance-Oficial-Nodo-Mensual" for node
And I click on "Approve" "button"
And If an error in the call of node status API
Then I should receive mail with subject "Error en el llamado del API de aprobación de balance oficial de nodos."
And I should receive mail with message body "Ha ocurrido un error actualizando el estado del nodo [“NodeName”]. Causa: [“ApiResponse”], Por favor verifique la causa y relance el flujo."
@testcase=71692
Scenario: Verify the Official Balance approval flow email template for general failure triggered to the Administrator
When I recieve the Approval Mail for "Aprobación-Balance-Oficial-Nodo-Mensual" for node
When I click on "Approve" "button"
And If an error in the call of node status API
Then I should receive mail with subject "Error en el llamado del API de aprobación de balance oficial de nodos."
And I should receive mail with message body "Ha ocurrido un error actualizando el estado del nodo [“NodeName”]. Causa: [“ApiResponse”], Por favor verifique la causa y relance el flujo."
@testcase=72613 
Scenario: Verify ADF pipeline must load the contents of the file into the corresponding database structure by replacing the existing values when the Excel is uploaded on the Azure
When I upload the Excel file containing NodeId with corresponding L1 approvers
And I Manually trigger the ADF pipeline
Then Verify contents of the file into the corresponding columns in DeltaNodeApproval table by replacing the existing values
