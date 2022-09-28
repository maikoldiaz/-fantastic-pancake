@sharedsteps=7539 @owner=jagudelos @api @testplan=26817 @testsuite=26827
Feature: UpdateStatusAndObservationsOfApprovalOfNode
As a TRUE system,
I require updating the status and observations of the approval of a node
owned to maintain traceability

Background: Login
Given I am authenticated as "admin"

@testcase=29502 @api @output=QueryAll(GetNodes) @bvt @version = 2
Scenario Outline: Validate error message when mandatory fields are empty
Given I have "nodes" in the system
When I Change the status of node
| keys                 | values                 |
| ownershipNodeId      | <ownershipNodeId>      |
| ApproverAlias        | <ApproverAlias>        |
| Comment              | <comment>              |
| Status               | <status>               |
Then I validate the below error log message
| Keys  | values  |
| Error | <error> |

Examples:
| ownershipNodeId | ApproverAlias | comment | status | error |
|                 |               |         |        |       |


@testcase=29503 @api @output=QueryAll(GetNodes) @bvt @version = 2
Scenario: Validate error message when status of the node is not sent for approval
Given I have "nodes" in the system
When I Change the status of node to not send for approval
Then I validate the below error log message
| Keys  | values                                              |
| Error | current state of the node is "Enviado a aprobación" |

@testcase=29504 @api @output=QueryAll(GetNodes) @bvt @version = 2
Scenario: Validate audit log record when status of node is sent for approval
Given I have "nodes" in the system
When I Change the status of node to send for approval
Then I validate the current state is "send for approval"
And I validate change is registerd in audit log table

@testcase=29505 @api @output=QueryAll(GetNodes) @bvt @version = 2
Scenario Outline: Validate audit log record when approval flow is successful
Given I have "nodes" in the system
When I Change the status of node
| keys                 | values                 |
| ownershipNodeId      | <ownershipNodeId>      |
| ApproverAlias        | <ApproverAlias>        |
| Comment              | <comment>              |
| Status               | <status>               |
Then I validate the record in audit log table

Examples:
| ownershipNodeId | ApproverAlias | comment | status | error |
|                 |               |         |        |       |

@testcase=29506 @api @output=QueryAll(GetNodes) @bvt @version =2
Scenario Outline: Validate error message when comment is empty
Given I have "nodes" in the system
When I Change the status of node to rejected
| keys                 | values                 |
| ownershipNodeId      | <ownershipNodeId>      |
| ApproverAlias        | <ApproverAlias>        |
| Comment              | <comment>              |
| Status               | <status>               |
Then I see the error message
| keys  | values  |
| Error | <error> |

Examples:
| ownershipNodeId | ApproverAlias | comment | status   | error                                             |
|                 |               |         | rejected | A comment for change the node status is mandatory |
