@sharedsteps=7539 @testsuite=31105 @testplan=31102 @owner=jagudelos
Feature: ReportToVisualizePendingApprovals
As TRUE Approver
I need to visualize pending approvals

Background: Approver Login
Given I am logged in as "admin"

@testcase=33903 @ui
Scenario: Verify True Approver user can view pending approvals Page
When I navigate to "Node Approval" page
Then I should see the "Tabs" on the Node Approval page
| Tabs     |
| Received |
| Sent     |
| History  |
And "Received" tab should be selected by default

@testcase=33904 @ui
Scenario: Verify True Approver can view pending approvals assigned to him/her.
When I navigate to "Node Approval" page
Then I should see Pending Approvals Assigned to me

@testcase=33905 @ui
Scenario: Verify True Approver can approve request assigned to him/her.
When I navigate to "Node Approval" page
And I click on "Approve" Icon
And I click on the Confirm button
Then "Esta solicitud ha sido rechazada" Message should be appeared

@testcase=33906 @ui
Scenario: Verify True Approver can reject request assigned to him/her.
When I navigate to "Node Approval" page
And I click on "Reject" Icon
And I click on the Confirm button
Then "Esta solicitud ha sido rechazada" Message should be appeared
