@owner=jagudelos @testplan=4938 @testsuite=5403 @backend @servicebus
Feature: MessageQueueRetrieveInventories
In order to receive the Inventories from SINOPER
As a TRUE system, I want to connect to an Enterprise MQ

@testcase=5873
Scenario: Validate data mapping for each Inventory
	Given the system is processing a valid Inventory message from MQ
	When it is transforming the original message to the canonical
	Then the system must apply the defined mapping for "Inventory"

@testcase=5874
Scenario: Verify retry strategy in case of Transient Failure
	Given the system is processing a message from MQ
	When process fails due to "TransientFailure"
	Then the system must log the failure
	And implement a retry strategy

@testcase=5875
Scenario: Verify retry strategy when the system reaches maximum number of defined retries
	Given the system is retrying a message processing
	When it reaches the "MaximumNumberOfRetries"
	Then the system must log the failure
	And it must release the message to be processed later

@testcase=5876
Scenario: Verify message processing in case of Inconsistent Message
	Given the system is processing a message from MQ
	When process fails due to "InconsistentMessage"
	Then the system must log the failure
	And it must release the message to be processed later

@testcase=5877
Scenario: Verify message processing when Similar Failures occurs
	Given the system is processing a message from MQ
	When process fails due to "TransientFailure"
	And the system detects many "SimilarFailures" recently
	Then the system must log the failure
	And implement a circuit breaker strategy