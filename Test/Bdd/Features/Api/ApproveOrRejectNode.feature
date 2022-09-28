@sharedsteps=7539 @owner=jagudelos @api @testplan= @testsuite=
Feature: ApproveOrRejectNode
	As an application administrator, 
	I want to Approve/Reject the node with ownership to maintain traceability

Background: Login
	Given I am authenticated as "admin"

@api @output=QueryAll(GetNodes) @bvt
Scenario: Validate Approval flow of the node with valid data
	Given I have created a node with state "Submit for Approval"
	When  I Approve the node with valid data
	Then I validate the node is approved
	And I validate the below audit log message
	| Keys                 |
	| IdNode               |
	| TicketId             |
	| ApproverAlias        |
	| Comment              |
	| Status               |
	| Date of registration |

@api @output=QueryAll(GetNodes)
Scenario: Validate the error message if the mandatory fields is not passed in the approval flow
	Given I have created a node with state "Submit for Approval"
	When  I Approve the node without mandatory fields
	Then I validate the error message for missing required fields

@api @output=QueryAll(GetNodes)
Scenario: Validate Rejected flow of the node with comments
	Given I have created a node with state "Submit for Approval"
	When  I Reject the node with comments
	Then I validate the node is rejected
	And I validate the below audit log message
	| Keys                 |
	| IdNode               |
	| TicketId             |
	| ApproverAlias        |
	| Comment              |
	| Status               |
	| Date of registration |

@api @output=QueryAll(GetNodes)
Scenario: Validate Rejected flow of the node without comments
	Given I have created a node with state "Submit for Approval"
	When  I Reject the node without comments
	Then I validate the error message for missing required fields
	
@api @output=QueryAll(GetNodes)
Scenario: Validate the error message when the node is not in the state "Submit for Approval"
	Given I have created a node not in the state "Submit for Approval"
	When  I Approve the node with valid data
	Then I validate the error message "El nodo tiene un estado no válido para aprobación, verificar que no haya cambios pendientes de publicación o publicarlos antes de aprobación."