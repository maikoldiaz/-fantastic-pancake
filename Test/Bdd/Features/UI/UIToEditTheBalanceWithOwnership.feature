@sharedsteps=16581 @owner=jagudelos @ui @testsuite=26826  @testplan=26817
Feature: UIToEditTheBalanceWithOwnership
As a Balance Segment Professional User,
I need to send a ticket node for approval
to be reviewed by the supervisor

Background: Login
Given I am logged in as "profesional"


@testcase=28298 @bvt @version=1
Scenario: Verify message of unbalanced node
	Given I have ownershipcalculated segment	
	When I click on "OwnershipNodes" "EditOwnership" "link"
	When I click on "Actions" "dropdown" 
	When I click on "OwnershipNodeDetails" "SubmitToApproval" "link"
	Then I should see the error message "El nodo debe estar balanceado para enviarlo a aprobación" displayed


@testcase=28299 @bvt @version=1
Scenario: Verify automatic approval conditions configured is met
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
And I click on "SubmitForApproval" "link" of balanced node
Then I should see node satus as "approved"
And I should see observation as "node automatically approved"
And I validate ownership calculation in the nodes


@testcase=28300 @bvt @version=1
Scenario: Verify technical error of malformed arithmetic expression
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
And I click on "SubmitForApproval" "link" of balanced node
And I get technical error while executing automatic approval conditions
Then I should see node satus as "submit for approval"
And I consume the service to start the approval flow
And I validate ownership calculation in the nodes


@testcase=28301 @bvt @version=1
Scenario: Verify manual approval of a balanced node
Given I have ownershipcalculated segment
When I click on "OwnershipNodes" "EditOwnership" "link"
And I click on "SubmitForApproval" "link" of balanced node
And automatic approval conditions are not met
Then I should see node satus as "submit for approval"
And I consume the service to start the approval flow
And I validate ownership calculation in the nodes
