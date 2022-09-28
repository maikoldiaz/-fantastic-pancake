@sharedsteps=16581 @owner=jagudelos @ui @testplan=24148 @testsuite=24164 @parallel=false
Feature: EnableAndDisableGridOptionsAsPerNodeState
In order to track new states of a node
As a Balance Segment Professional User
I need to enable and disable grid options as per node state

Background: Login
	Given I am logged in as "profesional"

@testcase=25166 @bvt @version=3 @bvt1.5
Scenario Outline: 01 Verify Records in Sent or Publishing status
	Given I have ownershipcalculated segment
	When I navigate to "Volumetric Balance with ownership for node" page
	When I search for a record with "<State>" status
	Then verify that "ownershipNodes" "viewError" "link" is "disabled"
	And verify that "ownershipNodes" "viewReport" "link" is "disabled"
	And verify that "ownershipNodes" "editOwnership" "link" is "disabled"

	Examples:
		| State      |
		| Processing |
		| Publishing |

@testcase=25167 @bvt @version=2 @bvt1.5
Scenario:02 Verify Records in Failed status
	Given I have ownershipcalculated segment
	When I navigate to "Volumetric Balance with ownership for node" page
	When I search for a record with "Failed" status
	Then verify that "ownershipNodes" "viewError" "link" is "enabled"
	And verify that "ownershipNodes" "viewReport" "link" is "disabled"
	And verify that "ownershipNodes" "editOwnership" "link" is "disabled"

@testcase=25168 @bvt @version=2 @bvt1.5
Scenario Outline:03 Verify Records in status other than Sent Publishing or Failed
	Given I have ownershipcalculated segment
	When I navigate to "Volumetric Balance with ownership for node" page
	When I search for a record with "<State>" status
	Then verify that "ownershipNodes" "viewError" "link" is "disabled"
	And verify that "ownershipNodes" "viewReport" "link" is "enabled"
	And verify that "ownershipNodes" "editOwnership" "link" is "enabled"

	Examples:
		| State             |
		| Ownership         |
		| Locked            |
		| Unlocked          |
		| Published         |
		| SubmitForApproval |
		| Approved          |
		| Rejected          |
		| Reopened          |

@testcase=25169 @version=2
Scenario:05 Verify Ownership state in nodes
	When I navigate to "Volumetric Balance with ownership for node" page
	When I search for a record with "Ownership" status
	Then the state name must be "Ownership"

@testcase=25170 @version=2
Scenario:06 Verify Ownership state in segments
	When I navigate to "Ownershipcalculation" page
	When I search for a record with "Ownership" status for segments
	Then the state name must be "Ownership" for segments

@testcase=25171 @manual @version=2
Scenario Outline:04 Verify Records of previous periods in status other than Sent Publishing or Failed
	When I navigate to "Volumetric Balance with ownership for node" page
	When I search for a record with "<State>" status of previous record
	Then verify that "ownershipNodes" "editOwnership" "link" is "disabled"

	Examples:
		| State             |
		| Ownership         |
		| Locked            |
		| Unlocked          |
		| Published         |
		| SubmitForApproval |
		| Approved          |
		| Rejected          |
		| Reopened          |