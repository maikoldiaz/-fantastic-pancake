@owner=jagudelos @testplan=70526 @testsuite=70808 @api @S16 @MVP2and3
Feature: OfficialBalanceApprovalInformation

Background: Login
	Given I am authenticated as "admin"

 @parallel=false @testcase=72592 @version=2 @BVT2
Scenario:  Verify API response if the API is invoked to get additional information from the node in the API
	Given I have delta node to "Get Delta Node details"
	When I get the node details with valid delta node
	Then response should have level 1 approver and user requesting for approval

@parallel=false @testcase=72593 @version=2 @BVT2
Scenario:  Verify API is invoked to save new status of approval flow of official delta calculation for a node if accepted
	Given I have delta node to perform "Change Delta Node Status"
	When I "Approve Node Status" request with comment
	Then I validate node state with change date, and observation are stored

@parallel=false @testcase=72594 @version=2 @BVT2
Scenario:  Verify API is invoked to save new status of reject flow of official delta calculation for a node if rejected
	Given I have delta node to perform "Change Delta Node Status"
	When I "Reject Node Status" request with out comment
	Then the response should fail with message "Es obligatorio un comentario para cambio de estado del nodo."
	When I "Reject Node Status" request with comment
	Then I validate node state with change date, and observation are stored

@parallel=false @testcase=72595  @manual @version=2
Scenario:  Verify API is invoked to store the last approval date property and save official balance by node
	Given I have delta calculation completed
	When I invoke API to accept the approval flow of official delta calculation for a node
	Then Save or update in a separate property the last approval date of the node
	And Save movements related to the official balance by node on block chain